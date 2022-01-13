namespace EcoplastERP.Module.Controllers
{
    partial class PurchaseWaybillViewController
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
            this.ConfirmPurchaseWaybillAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ConfirmPurchaseWaybillAction
            // 
            this.ConfirmPurchaseWaybillAction.Caption = "Confirm Purchase Waybill Action";
            this.ConfirmPurchaseWaybillAction.ConfirmationMessage = null;
            this.ConfirmPurchaseWaybillAction.Id = "ConfirmPurchaseWaybillAction";
            this.ConfirmPurchaseWaybillAction.ImageName = "Action_Grant";
            this.ConfirmPurchaseWaybillAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ConfirmPurchaseWaybillAction.TargetObjectsCriteria = "PurchaseWaybillStatus = \'WaitingForComplete\'";
            this.ConfirmPurchaseWaybillAction.ToolTip = null;
            this.ConfirmPurchaseWaybillAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ConfirmPurchaseWaybillAction_Execute);
            // 
            // PurchaseWaybillViewController
            // 
            this.Actions.Add(this.ConfirmPurchaseWaybillAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.PurchaseWaybill);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ConfirmPurchaseWaybillAction;
    }
}
