using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class PurchaseWaybillViewController : ViewController
    {
        public PurchaseWaybillViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            this.ConfirmPurchaseWaybillAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Depo Giriş Sorumlusu") ? true : false);
        }

        private void ConfirmPurchaseWaybillAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            PurchaseWaybill purchaseWaybill = (PurchaseWaybill)objectSpace.GetObject(e.CurrentObject);
            foreach (PurchaseWaybillDetail item in purchaseWaybill.PurchaseWaybillDetails)
            {
                var headerId = Guid.NewGuid();
                var input = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P102"));

                Movement movement = objectSpace.CreateObject<Movement>();
                movement.HeaderId = headerId;
                movement.DocumentNumber = purchaseWaybill.WaybillNumber;
                movement.DocumentDate = purchaseWaybill.WaybillDate;
                movement.Barcode = string.Empty;
                movement.SalesOrderDetail = null;
                movement.Product = item.Product;
                movement.PartyNumber = string.IsNullOrEmpty(item.PartyNumber) ? string.Empty : item.PartyNumber;
                movement.PaletteNumber = string.Empty;
                movement.Warehouse = item.Warehouse;
                movement.MovementType = input;
                movement.Unit = item.Unit;
                movement.Quantity = item.Quantity;
                movement.cUnit = item.Unit;
                movement.cQuantity = item.Quantity;
                movement.PurchaseWaybillDetail = item;
            }
            purchaseWaybill.PurchaseWaybillStatus = PurchaseWaybillStatus.Completed;

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
