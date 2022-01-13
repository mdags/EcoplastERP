using System;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class DeliveryViewController : ViewController
    {
        public DeliveryViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            this.CreateSalesWaybillAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("İrsaliye Kesme") ? true : false);
        }
        private void CreateSalesWaybillAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is DevExpress.ExpressApp.ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Delivery delivery = (Delivery)objectSpace.GetObject(e.CurrentObject);
            bool confirm = true;

            SalesWaybill existSalesWaybill = objectSpace.FindObject<SalesWaybill>(CriteriaOperator.Parse("Delivery = ?", delivery));
            if(existSalesWaybill!=null)
            {
                confirm = false;
                throw new UserFriendlyException("Bu teslimat için daha önce irsaliye oluşturulmuş. Yeni bir belge oluşturmak için önce eskisini siliniz.");
            }

            foreach (DeliveryDetail item in delivery.DeliveryDetails)
            {
                if (item.LoadedcQuantity == 0)
                {
                    if (XtraMessageBox.Show("Teslimatta okutulmayan kalem var. Devam etmeyi seçerseniz bu kalem silinecektir. Devam etmek istiyor musunuz?") != DialogResult.Yes)
                        confirm = false;
                }
            }

            if (confirm)
            {
                SalesWaybill salesWaybill = objectSpace.CreateObject<SalesWaybill>();
                salesWaybill.ShippingDate = DateTime.Now;
                salesWaybill.Contact = delivery.Contact;
                salesWaybill.Expedition = delivery.Expedition;
                salesWaybill.Delivery = delivery;
                foreach (DeliveryDetail detail in delivery.DeliveryDetails)
                {
                    if (detail.SalesOrderDetail.SalesOrder.Blockage == Blockage.AskBeforeInvoice)
                    {
                        throw new UserFriendlyException(string.Format("{0} nolu siparişin blokajı Faturadan Önce Sor olarak kaydedilmiş. Müşteri temsilcisi ile görüşünüz.", detail.SalesOrderDetail.SalesOrder.OrderNumber));
                    }
                    else
                    {
                        SalesWaybillDetail salesWaybillDetail = objectSpace.CreateObject<SalesWaybillDetail>();
                        salesWaybillDetail.SalesWaybill = salesWaybill;
                        salesWaybillDetail.Product = detail.SalesOrderDetail.Product;
                        salesWaybillDetail.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
                        salesWaybillDetail.Unit = detail.Unit;
                        decimal quantity = 0, cquantity = 0;
                        foreach (DeliveryDetailLoading loading in detail.DeliveryDetailLoadings)
                        {
                            quantity += loading.Quantity;
                            cquantity += loading.cQuantity;
                        }
                        salesWaybillDetail.Quantity = quantity;
                        salesWaybillDetail.cUnit = detail.cUnit;
                        salesWaybillDetail.cQuantity = cquantity;
                        salesWaybillDetail.ExpeditionDetail = detail.ExpeditionDetail;
                        salesWaybillDetail.DeliveryDetail = detail;
                        salesWaybillDetail.SalesOrderDetail = detail.SalesOrderDetail;
                        salesWaybill.SalesWaybillDetails.Add(salesWaybillDetail);
                        detail.SalesWaybillDetail = salesWaybillDetail;

                        if (detail.SalesOrderDetail.AnalysisCertificate != null)
                        {
                            if (detail.SalesOrderDetail.AnalysisCertificate.Name.Contains("İSTENİYOR"))
                            {
                                XtraMessageBox.Show(string.Format("{0} nolu teslimatın siparişinde müşteri analiz sertifikası istiyor.", delivery.DeliveryNumber), "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, salesWaybill);
            }
        }

        private void ChangeExpeditionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is DevExpress.ExpressApp.ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            const string expedition_ListView = "Expedition_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Expedition), expedition_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(expedition_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = (e.CurrentObject as Delivery).Oid;
            dc.Accepting += Dc_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(dc);
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "Expedition")
            {
                Delivery delivery = objectSpace.FindObject<Delivery>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                Expedition expedition = (Expedition)objectSpace.GetObject(e.AcceptActionArgs.SelectedObjects[0]);
                delivery.Expedition = expedition;
                foreach (DeliveryDetail detail in delivery.DeliveryDetails)
                {
                    ExpeditionDetail expeditionDetail = null;
                    ExpeditionDetail existsExpeditionDetail = objectSpace.FindObject<ExpeditionDetail>(CriteriaOperator.Parse("Expedition = ? and SalesOrderDetail = ?", expedition, detail.SalesOrderDetail));
                    if (existsExpeditionDetail != null)
                    {
                        existsExpeditionDetail.Quantity += detail.Quantity;
                        existsExpeditionDetail.cQuantity += detail.cQuantity;
                        expeditionDetail = existsExpeditionDetail;
                    }
                    else
                    {
                        ExpeditionDetail newExpeditionDetail = objectSpace.CreateObject<ExpeditionDetail>();
                        newExpeditionDetail.SalesOrderDetail = detail.SalesOrderDetail;
                        newExpeditionDetail.Unit = detail.Unit;
                        newExpeditionDetail.Quantity = detail.Quantity;
                        newExpeditionDetail.ShippingPlan = detail.ExpeditionDetail.ShippingPlan;
                        expedition.ExpeditionDetails.Add(newExpeditionDetail);
                        expeditionDetail = newExpeditionDetail;
                    }
                    detail.ExpeditionDetail = expeditionDetail;
                }

                objectSpace.CommitChanges();
            }
        }
    }
}
