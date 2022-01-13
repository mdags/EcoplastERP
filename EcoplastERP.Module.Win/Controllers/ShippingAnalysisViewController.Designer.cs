namespace EcoplastERP.Module.Win.Controllers
{
    partial class ShippingAnalysisViewController
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
            this.ExportShippingAnalysisAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportShippingAnalysisAction
            // 
            this.ExportShippingAnalysisAction.Caption = "Dışarı Aktar";
            this.ExportShippingAnalysisAction.ConfirmationMessage = null;
            this.ExportShippingAnalysisAction.Id = "ExportShippingAnalysisAction";
            this.ExportShippingAnalysisAction.ImageName = "Action_Export_ToExcel";
            this.ExportShippingAnalysisAction.ToolTip = null;
            this.ExportShippingAnalysisAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportShippingAnalysisAction_Execute);
            // 
            // ShippingAnalysisViewController
            // 
            this.Actions.Add(this.ExportShippingAnalysisAction);
            this.TargetViewId = "ShippingAnalysis_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportShippingAnalysisAction;
    }
}
