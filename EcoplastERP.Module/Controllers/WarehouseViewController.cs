using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security.Strategy;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class WarehouseViewController : ViewController
    {
        public WarehouseViewController()
        {
            InitializeComponent();
        }

        private void WarehousePermissionWindowShowAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SecuritySystemUser));
            string noteListViewId = Application.FindLookupListViewId(typeof(SecuritySystemUser));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(SecuritySystemUser), noteListViewId);
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }

        private void WarehousePermissionWindowShowAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Warehouse warehouse = (Warehouse)View.CurrentObject;
            foreach (SecuritySystemUser user in e.PopupWindowViewSelectedObjects)
            {
                SecuritySystemUser securitySystemUser = View.ObjectSpace.FindObject<SecuritySystemUser>(new BinaryOperator("Oid", user.Oid));
                WarehouseMovementPermission searchPermission = View.ObjectSpace.FindObject<WarehouseMovementPermission>(CriteriaOperator.Parse("DynamicReport = ? and SecuritySystemUser = ?", warehouse, securitySystemUser));
                if (searchPermission == null)
                {
                    WarehouseMovementPermission userPermission = View.ObjectSpace.CreateObject<WarehouseMovementPermission>();
                    userPermission.SecuritySystemUser = securitySystemUser;
                    userPermission.Warehouse = warehouse;
                    warehouse.WarehouseMovementPermissions.Add(userPermission);
                }
            }
            if (((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                View.ObjectSpace.CommitChanges();
            }
        }
    }
}
