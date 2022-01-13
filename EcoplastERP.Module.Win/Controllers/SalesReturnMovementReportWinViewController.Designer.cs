namespace EcoplastERP.Module.Win.Controllers
{
    partial class SalesReturnMovementReportWinViewController
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
            this.SalesReturnMovementReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SalesReturnMovementReportAction
            // 
            this.SalesReturnMovementReportAction.Caption = "Dışarı Aktar";
            this.SalesReturnMovementReportAction.ConfirmationMessage = null;
            this.SalesReturnMovementReportAction.Id = "SalesReturnMovementReportAction";
            this.SalesReturnMovementReportAction.ImageName = "Action_Export_ToExcel";
            this.SalesReturnMovementReportAction.ToolTip = null;
            this.SalesReturnMovementReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SalesReturnMovementReportAction_Execute);
            // 
            // SalesReturnMovementReportWinViewController
            // 
            this.Actions.Add(this.SalesReturnMovementReportAction);
            this.TargetViewId = "SalesReturnMovementReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SalesReturnMovementReportAction;
    }
}
