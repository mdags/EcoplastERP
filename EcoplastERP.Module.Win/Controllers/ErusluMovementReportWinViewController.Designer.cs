namespace EcoplastERP.Module.Win.Controllers
{
    partial class ErusluMovementReportWinViewController
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
            this.ExportErusluMovementReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportErusluMovementReportAction
            // 
            this.ExportErusluMovementReportAction.Caption = "Dışarı Aktar";
            this.ExportErusluMovementReportAction.ConfirmationMessage = null;
            this.ExportErusluMovementReportAction.Id = "ExportErusluMovementReportAction";
            this.ExportErusluMovementReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportErusluMovementReportAction.ToolTip = null;
            this.ExportErusluMovementReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportErusluMovementReportAction_Execute);
            // 
            // ErusluMovementReportWinViewController
            // 
            this.Actions.Add(this.ExportErusluMovementReportAction);
            this.TargetViewId = "ErusluMovementReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportErusluMovementReportAction;
    }
}
