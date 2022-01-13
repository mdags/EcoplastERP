using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class DemandViewController : ViewController
    {
        public DemandViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            Frame.GetController<ObjectMethodActionsViewController>().Actions["Demand.CancelAllDemand"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["Demand.GiveAdminConfirmToAll"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);

            this.EnterOfferAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
            this.CreatePurchaseOrderAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["DemandDetail.CancelDemand"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["DemandDetail.ChangePurchaser.AssignPurchaseParameters"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);

            Frame.GetController<ObjectMethodActionsViewController>().Actions["DemandDetail.GiveWarehouseConfirm"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Talep Depo Onayý") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["DemandDetail.GiveAdminConfirm"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Talep Yönetim Onayý") ? true : false);
        }

        private void EnterOfferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            int count = 1;
            Offer offer = objectSpace.CreateObject<Offer>();
            foreach (DemandDetail item in e.SelectedObjects)
            {
                OfferDetail offerDetail = objectSpace.CreateObject<OfferDetail>();
                offerDetail.LineNumber = count;
                offerDetail.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                offerDetail.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                offerDetail.Quantity = item.Quantity;
                offerDetail.DemandDetail = objectSpace.FindObject<DemandDetail>(new BinaryOperator("Oid", item.Oid));
                if (item.TakeOffer) offerDetail.OfferStatus = OfferStatus.WaitingForConfirm;
                else offerDetail.OfferStatus = OfferStatus.WaitingForOrder;
                offer.OfferDetails.Add(offerDetail);
                count++;
            }
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, offer);
        }

        private void CreatePurchaseOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            int count = 1;
            PurchaseOrder purchaseOrder = objectSpace.CreateObject<PurchaseOrder>();
            foreach (DemandDetail item in e.SelectedObjects)
            {
                PurchaseOrderDetail purchaseOrderDetail = objectSpace.CreateObject<PurchaseOrderDetail>();
                purchaseOrderDetail.LineNumber = count;
                purchaseOrderDetail.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                purchaseOrderDetail.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                purchaseOrderDetail.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                purchaseOrderDetail.Quantity = item.Quantity;
                purchaseOrderDetail.DemandDetail = objectSpace.FindObject<DemandDetail>(new BinaryOperator("Oid", item.Oid));
                if (item.Product.ProductGroup.SupplierEvaluation & item.Demand.InputControlPerson.Department.Name.Contains("KALÝTE GÜVENCE"))
                {
                    purchaseOrderDetail.PurchaseOrderStatus = PurchaseOrderStatus.WaitingForSupplierEvaluation;
                }
                else purchaseOrderDetail.PurchaseOrderStatus = PurchaseOrderStatus.WaitingForWaybill;
                purchaseOrder.PurchaseOrderDetails.Add(purchaseOrderDetail);
                count++;
            }
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, purchaseOrder);
        }
    }
}
