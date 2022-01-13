using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class SalesReturnViewController : ViewController
    {
        public SalesReturnViewController()
        {
            InitializeComponent();
        }

        private void CompleteSalesReturnAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            SalesReturn salesReturn = (SalesReturn)objectSpace.GetObject(e.CurrentObject);
            foreach (SalesReturnDetail item in salesReturn.SalesReturnDetails)
            {
                string barcode = Helpers.GetDocumentNumber(((XPObjectSpace)objectSpace).Session, "EcoplastERP.Module.BusinessObjects.ProductionObjects.Production");
                var headerId = Guid.NewGuid();
                item.Barcode = barcode;

                Movement movement = objectSpace.CreateObject<Movement>();
                movement.HeaderId = headerId;
                movement.DocumentNumber = salesReturn.DocumentNumber;
                movement.DocumentDate = salesReturn.DocumentDate;
                movement.Barcode = barcode;
                movement.SalesOrderDetail = null;
                movement.Product = item.Product;
                movement.PartyNumber = string.Empty;
                movement.PaletteNumber = string.Empty;
                movement.Warehouse = item.Warehouse;
                movement.MovementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P140"));
                movement.Unit = item.Unit;
                movement.Quantity = item.Quantity;
                movement.cUnit = item.Unit;
                movement.cQuantity = item.Quantity;
            }
            salesReturn.SalesReturnStatus = SalesReturnStatus.Completed;

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