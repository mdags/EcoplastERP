using System;
using System.Collections;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class LaminationWorkOrderViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public LaminationWorkOrderViewController()
        {
            InitializeComponent();

            SetLaminationWorkOrderStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(LaminationWorkOrder), "WorkOrderStatus"), null);
            SetLaminationWorkOrderStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(WorkOrderStatus));
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            this.SetLaminationWorkOrderStatusAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco1 Planlama") ? true : false);
            this.PrintLaminationMachineLoadAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Eco1 Planlama") ? true : false);
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

        private void SetLaminationWorkOrderStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    LaminationWorkOrder objInNewObjectSpace = (LaminationWorkOrder)objectSpace.GetObject(obj);
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

        private void ViewLaminationWorkOrderAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            LaminationWorkOrder workOrder = (LaminationWorkOrder)objectSpace.GetObject(e.CurrentObject);
            const string workOrder_ListView = "LaminationWorkOrder_ListView_Copy";

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product, BinaryOperatorType.Equal);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product", workOrder.SalesOrderDetail.Product);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact", workOrder.SalesOrderDetail.SalesOrder.Contact);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("Machine", workOrder.Machine);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Siparişten")
            {
                if (workOrder == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), workOrder_ListView);
                collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", workOrder.SalesOrderDetail.SalesOrder);
                collectionSource.Criteria["FilterbyWithoutThis"] = new BinaryOperator("Oid", workOrder, BinaryOperatorType.NotEqual);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(workOrder_ListView, collectionSource, true);
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
        }

        private void PrintLaminationMachineLoadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Eco1 Laminasyon Makine Yükü"));
            if (reportData != null)
            {
                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                Frame.GetController<ReportServiceController>().ShowPreview(handle, null, null);
            }
        }
    }
}
