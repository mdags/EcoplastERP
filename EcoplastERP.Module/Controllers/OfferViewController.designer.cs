namespace EcoplastERP.Module.Controllers
{
    partial class OfferViewController
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
            this.EnterOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // EnterOrderAction
            // 
            this.EnterOrderAction.Caption = "Sipariþe Çevir";
            this.EnterOrderAction.ConfirmationMessage = null;
            this.EnterOrderAction.Id = "EnterOrderAction";
            this.EnterOrderAction.ImageName = "BO_Order";
            this.EnterOrderAction.TargetObjectsCriteria = "OfferStatus = \'WaitingForOrder\'";
            this.EnterOrderAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.OfferDetail);
            this.EnterOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.EnterOrderAction.ToolTip = null;
            this.EnterOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.EnterOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EnterOrderAction_Execute);
            // 
            // OfferViewController
            // 
            this.Actions.Add(this.EnterOrderAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction EnterOrderAction;
    }
}
