namespace EcoplastERP.Module.Win.Controllers
{
    partial class NotifyShippedViewController
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
            this.NotifyShippedCompleteAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // NotifyShippedCompleteAction
            // 
            this.NotifyShippedCompleteAction.Caption = "Sevk Bildir";
            this.NotifyShippedCompleteAction.ConfirmationMessage = null;
            this.NotifyShippedCompleteAction.Id = "NotifyShippedCompleteAction";
            this.NotifyShippedCompleteAction.ImageName = "BO_Vendor";
            this.NotifyShippedCompleteAction.ToolTip = null;
            this.NotifyShippedCompleteAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NotifyShippedCompleteAction_Execute);
            // 
            // NotifyShippedViewController
            // 
            this.Actions.Add(this.NotifyShippedCompleteAction);
            this.TargetViewId = "NotifyShipped_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction NotifyShippedCompleteAction;
    }
}
