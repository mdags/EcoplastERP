using System;
using System.Web.UI;
using System.Web.Services;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingcomplete : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }

        [WebMethod]
        public static string CheckExpedition(string expeditionNumber)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Expedition expedition = session.FindObject<Expedition>(CriteriaOperator.Parse("ExpeditionNumber = ?", expeditionNumber));
            if (expedition != null)
            {
                return string.Empty;
            }
            else return "Sefer bulunamadı.";
        }

        [WebMethod]
        public static string CheckDelivery(string expeditionNumber, string deliveryNumber)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Delivery delivery = session.FindObject<Delivery>(CriteriaOperator.Parse("DeliveryNumber = ?", deliveryNumber));
            if (delivery != null)
            {
                if (delivery.Expedition.ExpeditionNumber != expeditionNumber)
                {
                    return "Bu teslimat belgesi girilen sefere ait değil.";
                }
                else return string.Empty;
            }
            else return "Teslimat belgesi bulunamadı.";
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Delivery delivery = session.FindObject<Delivery>(new BinaryOperator("DeliveryNumber", txtDeliveryNumber.Text));
            if (delivery != null)
            {
                string message = string.Empty;
                if (delivery.DeliveryBlockage)
                {
                    foreach (DeliveryDetail item in delivery.DeliveryDetails)
                    {
                        Store salesOrderDetailStore = session.FindObject<Store>(CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.ShippingWarehouse = true", item.SalesOrderDetail));
                        Store contactStore = session.FindObject<Store>(CriteriaOperator.Parse("SalesOrderDetail.SalesOrder.Contact = ? and Warehouse.ShippingWarehouse = true", delivery.ShippingContact));
                        if (item.DeliveryDetailLoadings.Count == 0)
                        {
                            delivery.DeliveryBlockStatus = Module.DeliveryBlockStatus.MissingOrder;
                            message = "Okutulmayan Sipariş Var";
                        }
                        else if (item.SalesOrderDetail.Product.ProductGroup.Name.Contains("ÖZEL") && salesOrderDetailStore != null)
                        {
                            delivery.DeliveryBlockStatus = Module.DeliveryBlockStatus.OrderStore;
                            message = "Depoda Sipariş Ürünü Mevcut";
                        }
                        else if (contactStore != null)
                        {
                            delivery.DeliveryBlockStatus = Module.DeliveryBlockStatus.ContactStore;
                            message = "Müşterinin Depoda Başka Ürünü Mevcut";
                        }
                        else
                        {
                            delivery.DeliveryBlockStatus = Module.DeliveryBlockStatus.Documentable;
                            message = "Evrak Kesilebilir";
                        }
                    }
                }
                else
                {
                    delivery.DeliveryBlockStatus = Module.DeliveryBlockStatus.Documentable;
                    message = "Evrak Kesilebilir";
                }
                delivery.Save();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", string.Format("alert('Teslimat durumu {0} olarak ayarlandı.');", message), true);
            }
        }

        protected void btnUnComplete_Click(object sender, EventArgs e)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Delivery delivery = session.FindObject<Delivery>(new BinaryOperator("DeliveryNumber", txtDeliveryNumber.Text));
            if (delivery != null)
            {
                delivery.DeliveryBlockStatus = Module.DeliveryBlockStatus.DeliveryBlock;
                delivery.DeliveryBlockage = true;
                delivery.Save();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('İşlem tamamlandı.');", true);
            }
        }
    }
}