namespace EcoplastERP.Module.Win.Controllers
{
    partial class SalesOrderReportViewController
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
            this.ExportSalesOrderReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportSalesOrderReportAction
            // 
            this.ExportSalesOrderReportAction.Caption = "Dışarı Aktar";
            this.ExportSalesOrderReportAction.ConfirmationMessage = null;
            this.ExportSalesOrderReportAction.Id = "ExportSalesOrderReportAction";
            this.ExportSalesOrderReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportSalesOrderReportAction.ToolTip = null;
            this.ExportSalesOrderReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportSalesOrderReportAction_Execute);
            // 
            // SalesOrderReportViewController
            // 
            this.Actions.Add(this.ExportSalesOrderReportAction);
            this.TargetViewId = "SalesOrderReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportSalesOrderReportAction;
    }
}
