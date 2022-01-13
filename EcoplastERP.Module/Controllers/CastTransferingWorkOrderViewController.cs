using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using System.Collections;
using DevExpress.Xpo;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using DevExpress.ExpressApp.Xpo;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.Controllers
{
    public partial class CastTransferingWorkOrderViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public CastTransferingWorkOrderViewController()
        {
            InitializeComponent();

            SetCastTransferingStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(CastTransferingWorkOrder), "WorkOrderStatus"), null);
            SetCastTransferingStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(WorkOrderStatus));
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.SetCastTransferingStatusAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco5 Planlama") ? true : false);
            this.PrintCastTransferingMachineLoadAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco5 Planlama") ? true : false);
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

        private void SetCastTransferingStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    CastTransferingWorkOrder objInNewObjectSpace = (CastTransferingWorkOrder)objectSpace.GetObject(obj);
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

        private void CopyCastTransferingWorkOrderRecieptAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            CastTransferingWorkOrder workOrder = (CastTransferingWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string castTransferingWorkOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                if (workOrder.Station != null)
                {
                    if (workOrder.Machine != null)
                    {
                        CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                        collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine", workOrder.Machine);
                        collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                        e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
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
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
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
            CastTransferingWorkOrder workOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Machine machine = ((Machine)(e.AcceptActionArgs.SelectedObjects[0]));
            const string castTransferingWorkOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";

            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", machine.Oid);
            collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dcContact_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            CastTransferingWorkOrder workOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Contact contact = ((Contact)(e.AcceptActionArgs.SelectedObjects[0]));
            const string castTransferingWorkOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";

            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", contact.Oid);
            collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);


            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = workOrder.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            CastTransferingWorkOrder workOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            if (workOrder != null)
            {
                XPCollection<CastTransferingWorkOrderReciept> castTransferingReciepts = workOrder.CastTransferingWorkOrderReciepts;
                ((XPObjectSpace)objectSpace).Session.Delete(castTransferingReciepts);
                foreach (var item in ((CastTransferingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0])).CastTransferingWorkOrderReciepts)
                {
                    CastTransferingWorkOrderReciept castTransferingWorkOrderReciept = objectSpace.CreateObject<CastTransferingWorkOrderReciept>();
                    castTransferingWorkOrderReciept.CastTransferingWorkOrder = workOrder;
                    castTransferingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    castTransferingWorkOrderReciept.Quantity = item.Quantity;
                    castTransferingWorkOrderReciept.Rate = item.Rate;
                    castTransferingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    castTransferingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                }
            }

            objectSpace.CommitChanges();
            objectSpace.Refresh();

            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void ViewCastTransferingWorkOrderAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            CastTransferingWorkOrder workOrder = (CastTransferingWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string workOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product, BinaryOperatorType.Equal);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", workOrder.Machine);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Siparişten")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
        }

        private void PrintCastTransferingMachineLoadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Eco5 Aktarma Makine Yükü"));
            if (reportData != null)
            {
                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                Frame.GetController<ReportServiceController>().ShowPreview(handle, null, null);
            }
        }
    }
}
