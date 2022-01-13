using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.QualityObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class PurchaseOrderViewController : ViewController
    {
        public PurchaseOrderViewController()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.EnterPurchaseWaybillAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Depo Giriþ Sorumlusu") ? true : false);
            this.ClosePurchaseOrderAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Depo Giriþ Sorumlusu") ? true : false);
            this.EnterSupplierEvaluationAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Kalite Güvence") ? true : false);
        }

        private void EnterPurchaseWaybillAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Contact contact = null;
            bool error = false;
            foreach (PurchaseOrderDetail item in e.SelectedObjects)
            {
                if (contact == null) contact = item.PurchaseOrder.Contact;
                else if (contact != item.PurchaseOrder.Contact)
                {
                    error = true;
                    throw new Exception("Fatura/Ýrsaliye girebilmeniz için ayný müþterinin sipariþlerini seçmeniz gerekmektedir.");
                }
            }

            if (!error)
            {
                int count = 1;
                PurchaseWaybill purchaseWaybill = objectSpace.CreateObject<PurchaseWaybill>();
                purchaseWaybill.Contact = objectSpace.FindObject<Contact>(new BinaryOperator("Oid", contact.Oid));
                purchaseWaybill.DistributionChannel = objectSpace.FindObject<DistributionChannel>(new BinaryOperator("Oid", (e.SelectedObjects[0] as PurchaseOrderDetail).PurchaseOrder.DistributionChannel.Oid));
                foreach (PurchaseOrderDetail item in e.SelectedObjects)
                {
                    PurchaseWaybillDetail purchaseWaybillDetail = objectSpace.CreateObject<PurchaseWaybillDetail>();
                    //purchaseWaybillDetail.PurchaseWaybill = purchaseWaybill;
                    purchaseWaybillDetail.LineNumber = count;
                    purchaseWaybillDetail.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    purchaseWaybillDetail.Warehouse = item.DemandDetail != null ? objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.DemandDetail.Warehouse.Oid)) : objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Product.Warehouse.Oid));
                    purchaseWaybillDetail.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    purchaseWaybillDetail.Quantity = item.Quantity;
                    purchaseWaybillDetail.Currency = objectSpace.FindObject<Currency>(new BinaryOperator("Oid", item.Currency.Oid));
                    purchaseWaybillDetail.CurrencyPrice = item.CurrencyPrice;
                    purchaseWaybillDetail.ExchangeRate = item.ExchangeRate;
                    purchaseWaybillDetail.Price = item.Price;
                    purchaseWaybillDetail.CurrencyTotal = item.CurrencyTotal;
                    purchaseWaybillDetail.Total = item.Total;
                    purchaseWaybillDetail.TaxRate = item.TaxRate;
                    purchaseWaybillDetail.CurrencyTax = item.CurrencyTax;
                    purchaseWaybillDetail.Tax = item.Tax;
                    purchaseWaybillDetail.DemandDetail = item.DemandDetail != null ? objectSpace.FindObject<DemandDetail>(new BinaryOperator("Oid", item.DemandDetail.Oid)) : null;
                    purchaseWaybillDetail.OfferDetail = item.OfferDetail != null ? objectSpace.FindObject<OfferDetail>(new BinaryOperator("Oid", item.OfferDetail.Oid)) : null;
                    purchaseWaybillDetail.PurchaseOrderDetail = objectSpace.FindObject<PurchaseOrderDetail>(new BinaryOperator("Oid", item.Oid));
                    purchaseWaybill.PurchaseWaybillDetails.Add(purchaseWaybillDetail);
                    purchaseWaybill.UpdateTotals();
                    count++;
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, purchaseWaybill);
            }
        }

        private void EnterSupplierEvaluationAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;

            PurchaseOrderDetail purchaseOrderDetail = (PurchaseOrderDetail)objectSpace.GetObject(e.CurrentObject);
            SupplierEvaluation obj = null;
            SupplierEvaluation existsSupplierEvaluation = objectSpace.FindObject<SupplierEvaluation>(CriteriaOperator.Parse("PurchaseOrderDetail = ?", purchaseOrderDetail));
            if (existsSupplierEvaluation != null)
            {
                obj = existsSupplierEvaluation;
            }
            else obj = objectSpace.CreateObject<SupplierEvaluation>();

            obj.Contact = objectSpace.FindObject<Contact>(new BinaryOperator("Oid", purchaseOrderDetail.PurchaseOrder.Contact.Oid));
            obj.PurchaseOrderDetail = objectSpace.FindObject<PurchaseOrderDetail>(new BinaryOperator("Oid", purchaseOrderDetail.Oid));
            obj.IncomingQuantity = purchaseOrderDetail.Quantity;

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.Context = TemplateContext.View;
            e.ShowViewParameters.CreateAllControllers = true;
        }

        private void ClosePurchaseOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            foreach (Object item in objectsToProcess)
            {
                PurchaseOrderDetail purchaseOrderDetail = (PurchaseOrderDetail)objectSpace.GetObject(item);
                purchaseOrderDetail.PurchaseOrderStatus = PurchaseOrderStatus.Receipted;
                if (purchaseOrderDetail.OfferDetail != null) purchaseOrderDetail.OfferDetail.OfferStatus = OfferStatus.Completed;
                if (purchaseOrderDetail.DemandDetail != null) purchaseOrderDetail.DemandDetail.DemandStatus = DemandStatus.Completed;
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
    }
}
