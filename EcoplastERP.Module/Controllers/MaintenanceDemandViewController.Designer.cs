namespace EcoplastERP.Module.Controllers
{
    partial class MaintenanceDemandViewController
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
            this.SetMaintenanceDemandStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CreateMaintenanceWorkOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetMaintenanceDemandStatusAction
            // 
            this.SetMaintenanceDemandStatusAction.Caption = "Durumu Değiştir";
            this.SetMaintenanceDemandStatusAction.ConfirmationMessage = null;
            this.SetMaintenanceDemandStatusAction.Id = "SetMaintenanceDemandStatusAction";
            this.SetMaintenanceDemandStatusAction.ImageName = "BO_State";
            this.SetMaintenanceDemandStatusAction.ShowItemsOnClick = true;
            this.SetMaintenanceDemandStatusAction.ToolTip = null;
            this.SetMaintenanceDemandStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetMaintenanceDemandStatusAction_Execute);
            // 
            // CreateMaintenanceWorkOrderAction
            // 
            this.CreateMaintenanceWorkOrderAction.Caption = "Bakım Emri Oluştur";
            this.CreateMaintenanceWorkOrderAction.ConfirmationMessage = null;
            this.CreateMaintenanceWorkOrderAction.Id = "CreateMaintenanceWorkOrderAction";
            this.CreateMaintenanceWorkOrderAction.ImageName = "BO_Task";
            this.CreateMaintenanceWorkOrderAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CreateMaintenanceWorkOrderAction.TargetObjectsCriteria = "Status = \'WaitingMaintenance\'";
            this.CreateMaintenanceWorkOrderAction.ToolTip = null;
            this.CreateMaintenanceWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateMaintenanceWorkOrderAction_Execute);
            // 
            // MaintenanceDemandViewController
            // 
            this.Actions.Add(this.SetMaintenanceDemandStatusAction);
            this.Actions.Add(this.CreateMaintenanceWorkOrderAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MaintenanceObjects.MaintenanceDemand);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetMaintenanceDemandStatusAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateMaintenanceWorkOrderAction;
    }
}
