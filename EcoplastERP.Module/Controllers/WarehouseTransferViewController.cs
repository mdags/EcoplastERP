using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class WarehouseTransferViewController : ViewController
    {
        public WarehouseTransferViewController()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.WarehouseTransferAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Depo Giriş Sorumlusu") ? true : false);
        }

        private void WarehouseTransferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is DetailView ? Application.CreateObjectSpace() : View.ObjectSpace;
            WarehouseTransfer warehouseTransfer = (WarehouseTransfer)objectSpace.GetObject(e.CurrentObject);
            foreach (WarehouseTransferDetail item in warehouseTransfer.WarehouseTransferDetails)
            {
                Guid headerId = Guid.NewGuid();
                MovementType input = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P120"));
                MovementType output = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P121"));

                Movement outputMovement = objectSpace.CreateObject<Movement>();
                outputMovement.HeaderId = headerId;
                outputMovement.DocumentNumber = item.WarehouseTransfer.DocumentNumber.ToString();
                outputMovement.DocumentDate = item.WarehouseTransfer.DocumentDate;
                outputMovement.Barcode = string.Empty;
                outputMovement.SalesOrderDetail = null;
                outputMovement.Product = item.Product;
                outputMovement.PartyNumber = string.Empty;
                outputMovement.PaletteNumber = string.Empty;
                outputMovement.Warehouse = item.SourceWarehouse;
                outputMovement.MovementType = output;
                outputMovement.Unit = item.Unit;
                outputMovement.Quantity = item.Quantity;
                outputMovement.cUnit = item.Unit;
                outputMovement.cQuantity = item.Quantity;

                Movement inputMovement = objectSpace.CreateObject<Movement>();
                inputMovement.HeaderId = headerId;
                inputMovement.DocumentNumber = item.WarehouseTransfer.DocumentNumber.ToString();
                inputMovement.DocumentDate = item.WarehouseTransfer.DocumentDate;
                inputMovement.Barcode = string.Empty;
                inputMovement.SalesOrderDetail = null;
                inputMovement.Product = item.Product;
                inputMovement.PartyNumber = string.Empty;
                inputMovement.PaletteNumber = string.Empty;
                inputMovement.Warehouse = item.DestinationWarehouse;
                inputMovement.MovementType = input;
                inputMovement.Unit = item.Unit;
                inputMovement.Quantity = item.Quantity;
                inputMovement.cUnit = item.Unit;
                inputMovement.cQuantity = item.Quantity;
            }

            try
            {
                warehouseTransfer.Status = WarehouseTransferStatus.Completed;
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
            catch
            {
                warehouseTransfer.Status = WarehouseTransferStatus.Waiting;
                objectSpace.CommitChanges();
            }
        }
    }
}
