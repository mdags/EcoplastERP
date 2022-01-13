using System;
using System.Collections;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using System.Data;

namespace EcoplastERP.Module.Controllers
{
    public partial class BalloonFilmingWorkOrderViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public BalloonFilmingWorkOrderViewController()
        {
            InitializeComponent();

            SetBalloonFilmingStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(BalloonFilmingWorkOrder), "WorkOrderStatus"), null);
            SetBalloonFilmingStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(WorkOrderStatus));
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.SetBalloonFilmingStatusAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco5 Planlama") ? true : false);
            this.PrintEco5CPPMachineLoadAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco5 Planlama") ? true : false);
            this.CreateBalloonFilmingRezervationAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco5 Planlama") ? true : false);
            this.UpdateBalloonFilmingRecieptAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() ? true : false);
        }

        private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)
        {
            foreach (object current in Enum.GetValues(enumType))
            {
                EnumDescriptor ed = new EnumDescriptor(enumType);
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current) { ImageName = ImageLoader.Instance.GetEnumValueImageName(current) };
                parentItem.Items.Add(item);
            }
        }

        private void SetBalloonFilmingStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    BalloonFilmingWorkOrder objInNewObjectSpace = (BalloonFilmingWorkOrder)objectSpace.GetObject(obj);
                    objInNewObjectSpace.WorkOrderStatus = (WorkOrderStatus)e.SelectedChoiceActionItem.Data;
                }
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

        private void CopyBalloonFilmingWorkOrderRecieptAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            BalloonFilmingWorkOrder workOrder = (BalloonFilmingWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string balloonFilmingWorkOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                if (workOrder.Station != null)
                {
                    if (workOrder.Machine != null)
                    {
                        CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                        collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine", workOrder.Machine);
                        collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                        e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
                    }
                }
            }

            if (e.SelectedChoiceActionItem.Caption == "Farklı Firmadan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Contact), "Contact_ListView");
                collectionSource.Criteria["FilterbyActive"] = new BinaryOperator("Active", true, BinaryOperatorType.Equal);
                e.ShowViewParameters.CreatedView = Application.CreateListView("Contact_ListView", collectionSource, true);
                DialogController dcContact = Application.CreateController<DialogController>();
                dcContact.Accepting += dcContact_Accepting;
                dcContact.Tag = workOrder.Oid;
                e.ShowViewParameters.Controllers.Add(dcContact);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Siparişten")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Farklı Makineden")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Machine), "Machine_ListView");
                collectionSource.Criteria["FilterbyActive"] = new BinaryOperator("Station", workOrder.Station);
                e.ShowViewParameters.CreatedView = Application.CreateListView("Machine_ListView", collectionSource, true);
                DialogController dcMachine = Application.CreateController<DialogController>();
                dcMachine.Accepting += DcMachine_Accepting;
                dcMachine.Tag = workOrder.Oid;
                e.ShowViewParameters.Controllers.Add(dcMachine);
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(dc);
        }
        private void DcMachine_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            BalloonFilmingWorkOrder workOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Machine machine = ((Machine)(e.AcceptActionArgs.SelectedObjects[0]));
            const string BalloonFilmingWorkOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";

            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), BalloonFilmingWorkOrder_ListView);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", machine.Oid);
            collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            e.ShowViewParameters.CreatedView = Application.CreateListView(BalloonFilmingWorkOrder_ListView, collectionSource, true);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dcContact_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            BalloonFilmingWorkOrder workOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Contact contact = ((Contact)(e.AcceptActionArgs.SelectedObjects[0]));
            const string BalloonFilmingWorkOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";

            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), BalloonFilmingWorkOrder_ListView);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", contact.Oid);
            collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            e.ShowViewParameters.CreatedView = Application.CreateListView(BalloonFilmingWorkOrder_ListView, collectionSource, true);


            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            BalloonFilmingWorkOrder workOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            if (workOrder != null)
            {
                XPCollection<BalloonFilmingWorkOrderReciept> balloonFilmingReciepts = workOrder.BalloonFilmingWorkOrderReciepts;
                ((XPObjectSpace)objectSpace).Session.Delete(balloonFilmingReciepts);
                foreach (var item in ((BalloonFilmingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0])).BalloonFilmingWorkOrderReciepts)
                {
                    BalloonFilmingWorkOrderReciept BalloonFilmingWorkOrderReciept = objectSpace.CreateObject<BalloonFilmingWorkOrderReciept>();
                    BalloonFilmingWorkOrderReciept.BalloonFilmingWorkOrder = workOrder;
                    BalloonFilmingWorkOrderReciept.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                    BalloonFilmingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    BalloonFilmingWorkOrderReciept.Quantity = item.Quantity;
                    BalloonFilmingWorkOrderReciept.Rate = item.Rate;
                    BalloonFilmingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    BalloonFilmingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    BalloonFilmingWorkOrderReciept.WorkOrderRate = item.WorkOrderRate;
                }
            }

            objectSpace.CommitChanges();
            objectSpace.Refresh();

            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void ViewBalloonFilmingWorkOrderAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            BalloonFilmingWorkOrder workOrder = (BalloonFilmingWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string workOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product, BinaryOperatorType.Equal);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", workOrder.Machine);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Siparişten")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
        }

        private void PrintEco5CPPMachineLoadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Eco5 CPP Makine Yükü"));
            if (reportData != null)
            {
                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                Frame.GetController<ReportServiceController>().ShowPreview(handle, null, null);
            }
        }

        private void CreateBalloonFilmingRezervationAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            string where = string.Empty, list = string.Empty;

            Rezervation rezervation = objectSpace.CreateObject<Rezervation>();
            rezervation.Description = string.Format("Cast Çekim {0} tarihli rezervasyon", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            foreach (Object obj in objectsToProcess)
            {
                BalloonFilmingWorkOrder workOrder = (BalloonFilmingWorkOrder)objectSpace.GetObject(obj);
                list += string.Format("'{0}',", workOrder.Oid);
            }

            where += string.Format(" and BalloonFilmingWorkOrder in ({0})", list.Substring(0, list.Length - 1));
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, string.Format(@"select Product, Warehouse, Unit, SUM(Quantity) as Quantity from BalloonFilmingWorkOrderReciept where GCRecord is null {0} group by Product, Warehouse, Unit", where)).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                RezervationDetail rezervationDetail = objectSpace.CreateObject<RezervationDetail>();
                decimal quantity = Convert.ToDecimal(row["Quantity"]);
                Product product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", Guid.Parse(row["Product"].ToString())));
                Warehouse warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", Guid.Parse(row["Warehouse"].ToString())));
                Unit unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString())));
                var store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", product, warehouse, unit));
                if (store != null)
                {
                    rezervationDetail.Product = product;
                    rezervationDetail.SourceWarehouse = product.Warehouse;
                    rezervationDetail.DestinationWarehouse = warehouse;
                    rezervationDetail.Unit = unit;
                    rezervationDetail.RecieptQuantity = quantity;
                    rezervationDetail.Quantity = store.Quantity < quantity ? quantity - store.Quantity : quantity;
                }
                else
                {
                    rezervationDetail.Product = product;
                    rezervationDetail.SourceWarehouse = product.Warehouse;
                    rezervationDetail.DestinationWarehouse = warehouse;
                    rezervationDetail.Unit = unit;
                    rezervationDetail.RecieptQuantity = quantity;
                    rezervationDetail.Quantity = 0;
                }
                rezervationDetail.Rezervation = rezervation;
                rezervation.RezervationDetails.Add(rezervationDetail);
            }

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, rezervation);
        }

        private void UpdateBalloonFilmingRecieptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            foreach (Object obj in objectsToProcess)
            {
                BalloonFilmingWorkOrder workOrder = (BalloonFilmingWorkOrder)objectSpace.GetObject(obj);
                workOrder.UpdateRecieptRate();
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
