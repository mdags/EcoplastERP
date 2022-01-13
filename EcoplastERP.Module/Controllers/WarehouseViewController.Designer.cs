namespace EcoplastERP.Module.Controllers
{
    partial class WarehouseViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.WarehousePermissionWindowShowAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // WarehousePermissionWindowShowAction
            // 
            this.WarehousePermissionWindowShowAction.AcceptButtonCaption = null;
            this.WarehousePermissionWindowShowAction.CancelButtonCaption = null;
            this.WarehousePermissionWindowShowAction.Caption = "Kullanıcı Ekle";
            this.WarehousePermissionWindowShowAction.ConfirmationMessage = null;
            this.WarehousePermissionWindowShowAction.Id = "WarehousePermissionWindowShowAction";
            this.WarehousePermissionWindowShowAction.ImageName = "Action_LinkUnlink_Link";
            this.WarehousePermissionWindowShowAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.WarehousePermissionWindowShowAction.ToolTip = null;
            this.WarehousePermissionWindowShowAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.WarehousePermissionWindowShowAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.WarehousePermissionWindowShowAction_CustomizePopupWindowParams);
            this.WarehousePermissionWindowShowAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.WarehousePermissionWindowShowAction_Execute);
            // 
            // WarehouseViewController
            // 
            this.Actions.Add(this.WarehousePermissionWindowShowAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductObjects.Warehouse);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction WarehousePermissionWindowShowAction;
    }
}
