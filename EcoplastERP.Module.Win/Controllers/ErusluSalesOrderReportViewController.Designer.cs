namespace EcoplastERP.Module.Win.Controllers
{
    partial class ErusluSalesOrderReportViewController
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
            this.ExportErusluSalesOrderReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportErusluSalesOrderReportAction
            // 
            this.ExportErusluSalesOrderReportAction.Caption = "Dışarı Aktar";
            this.ExportErusluSalesOrderReportAction.ConfirmationMessage = null;
            this.ExportErusluSalesOrderReportAction.Id = "ExportErusluSalesOrderReportAction";
            this.ExportErusluSalesOrderReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportErusluSalesOrderReportAction.ToolTip = null;
            this.ExportErusluSalesOrderReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportErusluSalesOrderReportAction_Execute);
            // 
            // ErusluSalesOrderReportViewController
            // 
            this.Actions.Add(this.ExportErusluSalesOrderReportAction);
            this.TargetViewId = "ErusluSalesOrderReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportErusluSalesOrderReportAction;
    }
}
