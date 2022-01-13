namespace EcoplastERP.Module.Win.Controllers
{
    partial class MachineCapacityTargetReportViewController
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
            this.ExportMachineCapacityTargetReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportMachineCapacityTargetReportAction
            // 
            this.ExportMachineCapacityTargetReportAction.Caption = "Dışarı Aktar";
            this.ExportMachineCapacityTargetReportAction.ConfirmationMessage = null;
            this.ExportMachineCapacityTargetReportAction.Id = "ExportMachineCapacityTargetReportAction";
            this.ExportMachineCapacityTargetReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportMachineCapacityTargetReportAction.ToolTip = null;
            this.ExportMachineCapacityTargetReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportMachineCapacityTargetReportAction_Execute);
            // 
            // MachineCapacityTargetReportViewController
            // 
            this.Actions.Add(this.ExportMachineCapacityTargetReportAction);
            this.TargetViewId = "MachineCapacityTargetReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportMachineCapacityTargetReportAction;
    }
}
