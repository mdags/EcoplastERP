using System;
using System.Data;
using System.Collections;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class FilmingWorkOrderViewController : ViewController
    {
        private ChoiceActionItem setFilmingStatusItem;

        public FilmingWorkOrderViewController()
        {
            InitializeComponent();
            RegisterActions(components);

            SetFilmingStatusAction.Items.Clear();
            setFilmingStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(FilmingWorkOrder), "WorkOrderStatus"), null);
            SetFilmingStatusAction.Items.Add(setFilmingStatusItem);
            FillItemWithEnumValues(setFilmingStatusItem, typeof(WorkOrderStatus));
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.SetFilmingStatusAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco2 Planlama") ? true : false);
            this.PrintFilmingMachineLoadAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco2 Planlama") ? true : false);
            this.CreateFilmingRezervationAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco2 Planlama") ? true : false);
            this.UpdateFilmingRecieptAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() ? true : false);
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

        private void SetStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setFilmingStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    FilmingWorkOrder objInNewObjectSpace = (FilmingWorkOrder)objectSpace.GetObject(obj);
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

        private void CopyFilmingWorkOrderRecieptAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilmingWorkOrder workOrder = (FilmingWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string filmingWorkOrder_ListView = "FilmingWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                if (workOrder.Station != null)
                {
                    if (workOrder.Machine != null)
                    {
                        CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                        collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine", workOrder.Machine);
                        collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                        e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
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
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
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
            FilmingWorkOrder workOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Machine machine = ((Machine)(e.AcceptActionArgs.SelectedObjects[0]));
            const string filmingWorkOrder_ListView = "FilmingWorkOrder_ListView_Copy";

            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", machine.Oid);
            collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dcContact_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilmingWorkOrder workOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Contact contact = ((Contact)(e.AcceptActionArgs.SelectedObjects[0]));
            const string filmingWorkOrder_ListView = "FilmingWorkOrder_ListView_Copy";

            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", contact.Oid);
            collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);


            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilmingWorkOrder workOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            if (workOrder != null)
            {
                XPCollection<FilmingWorkOrderReciept> filmingReciepts = workOrder.FilmingWorkOrderReciepts;
                ((XPObjectSpace)objectSpace).Session.Delete(filmingReciepts);
                foreach (var item in ((FilmingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0])).FilmingWorkOrderReciepts)
                {
                    FilmingWorkOrderReciept filmingWorkOrderReciept = objectSpace.CreateObject<FilmingWorkOrderReciept>();
                    filmingWorkOrderReciept.FilmingWorkOrder = workOrder;
                    filmingWorkOrderReciept.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                    filmingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    filmingWorkOrderReciept.Quantity = item.Quantity;
                    filmingWorkOrderReciept.Rate = item.Rate;
                    filmingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    filmingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    filmingWorkOrderReciept.WorkOrderRate = item.WorkOrderRate;
                }
            }

            objectSpace.CommitChanges();
            objectSpace.Refresh();

            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void ViewFilmingWorkOrderAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilmingWorkOrder workOrder = (FilmingWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string workOrder_ListView = "FilmingWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product, BinaryOperatorType.Equal);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", workOrder.Machine);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Siparişten")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
        }

        private void PrintFilmingMachineLoadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Eco2 Makine Yükü"));
            if (reportData != null)
            {
                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                Frame.GetController<ReportServiceController>().ShowPreview(handle, null, null);
            }
        }

        private void CreateFilmingRezervationAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            string where = string.Empty, list = string.Empty;

            Rezervation rezervation = objectSpace.CreateObject<Rezervation>();
            rezervation.Description = string.Format("Çekim {0} tarihli rezervasyon", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            foreach (Object obj in objectsToProcess)
            {
                FilmingWorkOrder workOrder = (FilmingWorkOrder)objectSpace.GetObject(obj);
                list += string.Format("'{0}',", workOrder.Oid);
            }

            where += string.Format(" and FilmingWorkOrder in ({0})", list.Substring(0, list.Length - 1));
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, string.Format(@"select Product, Warehouse, Unit, SUM(Quantity) as Quantity from FilmingWorkOrderReciept where GCRecord is null {0} group by Product, Warehouse, Unit", where)).Tables[0];
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

        private void UpdateFilmingRecieptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            foreach (Object obj in objectsToProcess)
            {
                FilmingWorkOrder workOrder = (FilmingWorkOrder)objectSpace.GetObject(obj);
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