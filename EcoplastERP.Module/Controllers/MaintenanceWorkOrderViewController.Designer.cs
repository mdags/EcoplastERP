namespace EcoplastERP.Module.Controllers
{
    partial class MaintenanceWorkOrderViewController
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
            this.SetMaintenanceWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction();
            this.SetMaintenanceTeamForWorkOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            this.StartMaintenanceAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            // 
            // SetMaintenanceWorkOrderStatusAction
            // 
            this.SetMaintenanceWorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetMaintenanceWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetMaintenanceWorkOrderStatusAction.Id = "SetMaintenanceWorkOrderStatusAction";
            this.SetMaintenanceWorkOrderStatusAction.ImageName = "BO_State";
            this.SetMaintenanceWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetMaintenanceWorkOrderStatusAction.ToolTip = null;
            this.SetMaintenanceWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetMaintenanceWorkOrderStatusAction_Execute);
            // 
            // SetMaintenanceTeamForWorkOrderAction
            // 
            this.SetMaintenanceTeamForWorkOrderAction.Caption = "Bakım Ekibi Seç";
            this.SetMaintenanceTeamForWorkOrderAction.ConfirmationMessage = null;
            this.SetMaintenanceTeamForWorkOrderAction.Id = "SetMaintenanceTeamForWorkOrderAction";
            this.SetMaintenanceTeamForWorkOrderAction.ImageName = "BO_Department";
            this.SetMaintenanceTeamForWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.SetMaintenanceTeamForWorkOrderAction.ToolTip = null;
            this.SetMaintenanceTeamForWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.SetMaintenanceTeamForWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetMaintenanceTeamForWorkOrderAction_Execute);
            // 
            // StartMaintenanceAction
            // 
            this.StartMaintenanceAction.Caption = "Bakımı Başlat";
            this.StartMaintenanceAction.ConfirmationMessage = null;
            this.StartMaintenanceAction.Id = "StartMaintenanceAction";
            this.StartMaintenanceAction.ImageName = "Action_ResumeRecording";
            this.StartMaintenanceAction.ToolTip = null;
            this.StartMaintenanceAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.StartMaintenanceAction_Execute);
            // 
            // MaintenanceWorkOrderViewController
            // 
            this.Actions.Add(this.SetMaintenanceWorkOrderStatusAction);
            this.Actions.Add(this.SetMaintenanceTeamForWorkOrderAction);
            this.Actions.Add(this.StartMaintenanceAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MaintenanceObjects.MaintenanceWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetMaintenanceWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SetMaintenanceTeamForWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction StartMaintenanceAction;
    }
}
