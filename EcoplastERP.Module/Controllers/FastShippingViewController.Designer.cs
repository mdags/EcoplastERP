namespace EcoplastERP.Module.Controllers
{
    partial class FastShippingViewController
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
            this.CompleteFastShippingAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CompleteFastShippingAction
            // 
            this.CompleteFastShippingAction.Caption = "Hızlı Sevkiyat Sonlandır";
            this.CompleteFastShippingAction.ConfirmationMessage = null;
            this.CompleteFastShippingAction.Id = "CompleteFastShippingAction";
            this.CompleteFastShippingAction.ImageName = "BO_Vendor";
            this.CompleteFastShippingAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ShippingObjects.FastShipping);
            this.CompleteFastShippingAction.ToolTip = null;
            this.CompleteFastShippingAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CompleteFastShippingAction_Execute);
            // 
            // FastShippingViewController
            // 
            this.Actions.Add(this.CompleteFastShippingAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CompleteFastShippingAction;
    }
}
