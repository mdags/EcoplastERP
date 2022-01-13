namespace EcoplastERP.Module.Controllers
{
    partial class DemandViewController
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
            this.EnterOfferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreatePurchaseOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // EnterOfferAction
            // 
            this.EnterOfferAction.Caption = "Teklife Çevir";
            this.EnterOfferAction.ConfirmationMessage = null;
            this.EnterOfferAction.Id = "EnterOfferAction";
            this.EnterOfferAction.ImageName = "BO_Invoice";
            this.EnterOfferAction.TargetObjectsCriteria = "DemandStatus = \'WaitingForPurchase\' or DemandStatus = \'WaitingForOrder\'";
            this.EnterOfferAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.DemandDetail);
            this.EnterOfferAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.EnterOfferAction.ToolTip = null;
            this.EnterOfferAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.EnterOfferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EnterOfferAction_Execute);
            // 
            // CreatePurchaseOrderAction
            // 
            this.CreatePurchaseOrderAction.Caption = "Sipariþe Çevir";
            this.CreatePurchaseOrderAction.ConfirmationMessage = null;
            this.CreatePurchaseOrderAction.Id = "CreatePurchaseOrderAction";
            this.CreatePurchaseOrderAction.ImageName = "BO_Order";
            this.CreatePurchaseOrderAction.TargetObjectsCriteria = "DemandStatus = \'WaitingForPurchase\' or DemandStatus = \'WaitingForOrder\'";
            this.CreatePurchaseOrderAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.DemandDetail);
            this.CreatePurchaseOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CreatePurchaseOrderAction.ToolTip = null;
            this.CreatePurchaseOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CreatePurchaseOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreatePurchaseOrderAction_Execute);
            // 
            // DemandViewController
            // 
            this.Actions.Add(this.EnterOfferAction);
            this.Actions.Add(this.CreatePurchaseOrderAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction EnterOfferAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreatePurchaseOrderAction;
    }
}
