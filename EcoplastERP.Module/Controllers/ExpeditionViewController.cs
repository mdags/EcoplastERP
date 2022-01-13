using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class ExpeditionViewController : ViewController
    {
        public ExpeditionViewController()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.OpenExpeditionAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Sefer Açma") ? true : false);
        }

        private void AllocateShippingCostAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Expedition expedition = (Expedition)objectSpace.GetObject(e.CurrentObject);
            foreach (Delivery delivery in expedition.Deliveries)
            {
                delivery.ShippingCost = expedition.TotalShippingCost / (expedition.TotalLoadedcQuantity / delivery.TotalLoadedcQuantity);
            }
            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if (View is ListView)
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }

        private void CloseExpeditionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Expedition expedition = (Expedition)objectSpace.GetObject(e.CurrentObject);
            //1 Seferin içinde teslimatlar hepsinin irsaliyesi kesilmiþ olmalý
            foreach (Delivery delivery in expedition.Deliveries)
            {
                if (delivery.DeliveryStatus != DeliveryStatus.Completed)
                {
                    throw new UserFriendlyException(string.Format("{0} nolu Teslimat belgesi irsaliyesi kesilmemiþ. Sefer kapatýlamaz.", delivery.DeliveryNumber));
                }
            }

            //2 Kamyon dara yazýlmýþ ve yüklenen ürünlerin palet toplamý arasýnda 30 kð + / - var
            //decimal palettesTotal = 0;
            //foreach (Delivery delivery in expedition.Deliveries)
            //{
            //    foreach (DeliveryDetail deliveryDetail in delivery.DeliveryDetails)
            //    {
            //        DataTable dt = new DataTable();
            //        SqlDataAdapter da = new SqlDataAdapter(string.Format("select MAX(PP.LastWeight) as LastWeight from DeliveryDetailLoading L inner join ProductionPalette PP on PP.PaletteNumber = L.PaletteNumber where L.GCRecord is null and L.DeliveryDetail = '{0}' group by L.PaletteNumber", deliveryDetail.Oid), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            //        da.Fill(dt);
            //        foreach (DataRow row in dt.Rows)
            //        {
            //            palettesTotal += Convert.ToDecimal(row["LastWeight"]);
            //        }
            //    }
            //}
            //if (palettesTotal < (expedition.WeighbridgeTare - 30) || palettesTotal > (expedition.WeighbridgeTare + 30))
            //{
            //    throw new UserFriendlyException("Yüklenen ürünlerin toplamý ile kantar dara arasýnda +-30 kg'den fazla fark bulunmaktadýr. Yüklenen ürünleri kontrol ediniz.");
            //}

            //3 Yükleyenin atadýðý plaka ve dorse ile sefere atanan plaka ve dorse ayný is
            //if (expedition.AssignTruckPlate != expedition.LoadingTruckPlate)
            //{
            //    throw new UserFriendlyException("Atanan Plaka ve Yükleyen Plaka uyuþmuyor. Sefer kapatýlamaz.");
            //}
            //if (expedition.AssignTruckDorsePlate != expedition.LoadingDorsePlate)
            //{
            //    throw new UserFriendlyException("Atanan Dorse Plaka ve Yükleyen Dorse Plaka uyuþmuyor. Sefer kapatýlamaz.");
            //}

            //4 Araçta ihracat firmalarý var ve sefere teslimata resim eklenmiþ ise ( döküman ekleme )
            //foreach (Delivery delivery in expedition.Deliveries)
            //{
            //    foreach (DeliveryDetail deliveryDetail in delivery.DeliveryDetails)
            //    {
            //        if (deliveryDetail.SalesOrderDetail.SalesOrder.Contact.DistributionChannel.Code == "YD")
            //        {
            //            if (expedition.ExpeditionPortfolios.Count == 0)
            //            {
            //                throw new UserFriendlyException("Araçta ihracat firmasý var sefere resim belgesi ekleyiniz.");
            //            }
            //        }
            //    }
            //}

            foreach (ExpeditionDetail detail in expedition.ExpeditionDetails)
            {
                if (detail.ShippingPlan.ExpeditionDetail == detail)
                    detail.ShippingPlan.ShippingPlanStatus = ShippingPlanStatus.Completed;
            }

            expedition.ExpeditionStatus = ExpeditionStatus.Completed;

            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if (View is ListView)
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }

        private void OpenExpeditionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Expedition expedition = (Expedition)objectSpace.GetObject(e.CurrentObject);
            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Expedition set ExpeditionStatus = 3 where Oid = @oid", new string[] { "@oid" }, new object[] { expedition.Oid });

            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if (View is ListView)
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }
    }
}
