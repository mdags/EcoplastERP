namespace EcoplastERP.Module.Controllers
{
    partial class SalesReturnViewController
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
            this.CompleteSalesReturnAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CompleteSalesReturnAction
            // 
            this.CompleteSalesReturnAction.Caption = "Depo Giriş Onayı";
            this.CompleteSalesReturnAction.ConfirmationMessage = null;
            this.CompleteSalesReturnAction.Id = "CompleteSalesReturnAction";
            this.CompleteSalesReturnAction.ImageName = "Action_Grant";
            this.CompleteSalesReturnAction.TargetObjectsCriteria = "SalesReturnStatus != \'Completed\'";
            this.CompleteSalesReturnAction.ToolTip = null;
            this.CompleteSalesReturnAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CompleteSalesReturnAction_Execute);
            // 
            // SalesReturnViewController
            // 
            this.Actions.Add(this.CompleteSalesReturnAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MarketingObjects.SalesReturn);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CompleteSalesReturnAction;
    }
}
