namespace EcoplastERP.Module.Win.Controllers
{
    partial class StorePastReportWinViewController
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
            this.ExportStorePastReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportStorePastReportAction
            // 
            this.ExportStorePastReportAction.Caption = "Dışarı Aktar";
            this.ExportStorePastReportAction.ConfirmationMessage = null;
            this.ExportStorePastReportAction.Id = "ExportStorePastReportAction";
            this.ExportStorePastReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportStorePastReportAction.ToolTip = null;
            this.ExportStorePastReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportStorePastReportAction_Execute);
            // 
            // StorePastReportWinViewController
            // 
            this.Actions.Add(this.ExportStorePastReportAction);
            this.TargetViewId = "StorePastReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportStorePastReportAction;
    }
}
