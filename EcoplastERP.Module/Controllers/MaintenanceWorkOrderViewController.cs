using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.MaintenanceObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class MaintenanceWorkOrderViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public MaintenanceWorkOrderViewController()
        {
            InitializeComponent();

            SetMaintenanceWorkOrderStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(MaintenanceWorkOrder), "WorkOrderStatus"), null);
            SetMaintenanceWorkOrderStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(MaintenanceWorkOrderStatus));
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

        private void SetMaintenanceWorkOrderStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    MaintenanceWorkOrder objInNewObjectSpace = (MaintenanceWorkOrder)objectSpace.GetObject(obj);
                    objInNewObjectSpace.Status = (MaintenanceWorkOrderStatus)e.SelectedChoiceActionItem.Data;
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

        private void SetMaintenanceTeamForWorkOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            const string maintenanceTeam_ListView = "MaintenanceTeam_ListView";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(MaintenanceTeam), maintenanceTeam_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(maintenanceTeam_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = (e.CurrentObject as MaintenanceWorkOrder).Oid;
            dc.Accepting += Dc_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(dc);
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "MaintenanceTeam")
            {
                MaintenanceTeam maintenanceTeam = ((MaintenanceTeam)(e.AcceptActionArgs.SelectedObjects[0]));
                MaintenanceWorkOrder workOrder = objectSpace.FindObject<MaintenanceWorkOrder>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                foreach (Employee item in maintenanceTeam.Employees)
                {
                    Employee employee = objectSpace.FindObject<Employee>(new BinaryOperator("Oid", item.Oid));
                    if (employee != null)
                    {
                        workOrder.Employees.Add(employee);
                    }
                }
            }
            objectSpace.CommitChanges();
            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void StartMaintenanceAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            foreach (Object obj in objectsToProcess)
            {
                MaintenanceWorkOrder objInNewObjectSpace = (MaintenanceWorkOrder)objectSpace.GetObject(obj);
                objInNewObjectSpace.Status = MaintenanceWorkOrderStatus.Started;
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
