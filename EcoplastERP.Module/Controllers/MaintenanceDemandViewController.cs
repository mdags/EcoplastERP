using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.MaintenanceObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class MaintenanceDemandViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public MaintenanceDemandViewController()
        {
            InitializeComponent();

            SetMaintenanceDemandStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(MaintenanceDemand), "WorkOrderStatus"), null);
            SetMaintenanceDemandStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(MaintenanceDemandStatus));
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

        private void SetMaintenanceDemandStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    MaintenanceDemand objInNewObjectSpace = (MaintenanceDemand)objectSpace.GetObject(obj);
                    objInNewObjectSpace.Status = (MaintenanceDemandStatus)e.SelectedChoiceActionItem.Data;
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

        private void CreateMaintenanceWorkOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            MaintenanceDemand maintenanceDemand = (MaintenanceDemand)objectSpace.GetObject(e.CurrentObject);

            MaintenanceWorkOrder workOrder = objectSpace.CreateObject<MaintenanceWorkOrder>();
            workOrder.MaintenanceDemand = maintenanceDemand;

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, workOrder);
        }
    }
}
