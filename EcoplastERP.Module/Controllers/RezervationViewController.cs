using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class RezervationViewController : ViewController
    {
        public RezervationViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.RezervationTransferAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Depo Giriþ Sorumlusu") ? true : false);
        }

        private void RezervationTransferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is DetailView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Rezervation rezervation = (Rezervation)objectSpace.GetObject(e.CurrentObject);
            foreach (RezervationDetail item in rezervation.RezervationDetails)
            {
                var headerId = Guid.NewGuid();
                var input = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P128"));
                var output = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P129"));

                Movement outputMovement = objectSpace.CreateObject<Movement>();
                outputMovement.HeaderId = headerId;
                outputMovement.DocumentNumber = item.Rezervation.RezervationNumber;
                outputMovement.DocumentDate = item.Rezervation.RezervationDate;
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
                inputMovement.DocumentNumber = item.Rezervation.RezervationNumber;
                inputMovement.DocumentDate = item.Rezervation.RezervationDate;
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
                rezervation.Status = WarehouseTransferStatus.Completed;
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
                rezervation.Status = WarehouseTransferStatus.Waiting;
                objectSpace.CommitChanges();
            }
        }
    }
}
