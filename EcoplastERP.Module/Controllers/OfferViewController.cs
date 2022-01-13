using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class OfferViewController : ViewController
    {
        public OfferViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            Frame.GetController<ObjectMethodActionsViewController>().Actions["OfferDetail.CancelOffer"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["OfferDetail.ConfirmOffer"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
            this.EnterOrderAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satýnalma") ? true : false);
        }

        private void EnterOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Contact contact = null;
            bool error = false;
            foreach (OfferDetail item in e.SelectedObjects)
            {
                if (contact == null) contact = item.Offer.Contact;
                else if (contact != item.Offer.Contact)
                {
                    error = true;
                    throw new Exception("Tedarikçi Sipariþi girebilmeniz için ayný müþterinin tekliflerini seçmeniz gerekmektedir.");
                }
            }

            if (!error)
            {
                int count = 1;
                PurchaseOrder purchaseOrder = objectSpace.CreateObject<PurchaseOrder>();
                purchaseOrder.Contact = objectSpace.FindObject<Contact>(new BinaryOperator("Oid", contact.Oid));
                purchaseOrder.DistributionChannel = objectSpace.FindObject<DistributionChannel>(new BinaryOperator("Oid", (e.SelectedObjects[0] as OfferDetail).Offer.DistributionChannel.Oid));
                foreach (OfferDetail item in e.SelectedObjects)
                {
                    PurchaseOrderDetail purchaseOrderDetail = objectSpace.CreateObject<PurchaseOrderDetail>();
                    //purchaseOrderDetail.PurchaseOrder = purchaseOrder;
                    purchaseOrderDetail.LineNumber = count;
                    purchaseOrderDetail.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    purchaseOrderDetail.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.DemandDetail.Warehouse.Oid));
                    purchaseOrderDetail.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    purchaseOrderDetail.Quantity = item.Quantity;
                    purchaseOrderDetail.Currency = objectSpace.FindObject<Currency>(new BinaryOperator("Oid", item.Currency.Oid));
                    purchaseOrderDetail.CurrencyPrice = item.CurrencyPrice;
                    purchaseOrderDetail.ExchangeRate = item.ExchangeRate;
                    purchaseOrderDetail.Price = item.Price;
                    purchaseOrderDetail.CurrencyTotal = item.CurrencyTotal;
                    purchaseOrderDetail.Total = item.Total;
                    purchaseOrderDetail.TaxRate = item.TaxRate;
                    purchaseOrderDetail.CurrencyTax = item.CurrencyTax;
                    purchaseOrderDetail.Tax = item.Tax;
                    purchaseOrderDetail.LineInstruction = item.LineInstruction;
                    purchaseOrderDetail.LineDeliveryDate = item.LineDeliveryDate;
                    purchaseOrderDetail.DemandDetail = objectSpace.FindObject<DemandDetail>(new BinaryOperator("Oid", item.DemandDetail.Oid));
                    purchaseOrderDetail.OfferDetail = objectSpace.FindObject<OfferDetail>(new BinaryOperator("Oid", item.Oid));
                    purchaseOrder.PurchaseOrderDetails.Add(purchaseOrderDetail);
                    purchaseOrder.UpdateTotals();
                    count++;
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, purchaseOrder);
            }
        }
    }
}
