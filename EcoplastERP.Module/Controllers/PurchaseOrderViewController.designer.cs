namespace EcoplastERP.Module.Controllers {
	partial class PurchaseOrderViewController {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.EnterPurchaseWaybillAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.EnterSupplierEvaluationAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ClosePurchaseOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // EnterPurchaseWaybillAction
            // 
            this.EnterPurchaseWaybillAction.Caption = "Ýrsaliyeye Çevir";
            this.EnterPurchaseWaybillAction.ConfirmationMessage = null;
            this.EnterPurchaseWaybillAction.Id = "EnterPurchaseWaybillAction";
            this.EnterPurchaseWaybillAction.ImageName = "BO_Order";
            this.EnterPurchaseWaybillAction.TargetObjectsCriteria = "PurchaseOrderStatus = \'WaitingForWaybill\'";
            this.EnterPurchaseWaybillAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.PurchaseOrderDetail);
            this.EnterPurchaseWaybillAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.EnterPurchaseWaybillAction.ToolTip = null;
            this.EnterPurchaseWaybillAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.EnterPurchaseWaybillAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EnterPurchaseWaybillAction_Execute);
            // 
            // EnterSupplierEvaluationAction
            // 
            this.EnterSupplierEvaluationAction.Caption = "Tedarikçi Deðerlendirme";
            this.EnterSupplierEvaluationAction.ConfirmationMessage = null;
            this.EnterSupplierEvaluationAction.Id = "EnterSupplierEvaluationAction";
            this.EnterSupplierEvaluationAction.ImageName = "BO_Note";
            this.EnterSupplierEvaluationAction.TargetObjectsCriteria = "";
            this.EnterSupplierEvaluationAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.PurchaseOrderDetail);
            this.EnterSupplierEvaluationAction.ToolTip = null;
            this.EnterSupplierEvaluationAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EnterSupplierEvaluationAction_Execute);
            // 
            // ClosePurchaseOrderAction
            // 
            this.ClosePurchaseOrderAction.Caption = "Satýnalma Sipariþ Kapat";
            this.ClosePurchaseOrderAction.ConfirmationMessage = null;
            this.ClosePurchaseOrderAction.Id = "ClosePurchaseOrderAction";
            this.ClosePurchaseOrderAction.ImageName = "Action_Grant_Set";
            this.ClosePurchaseOrderAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PurchaseObjects.PurchaseOrderDetail);
            this.ClosePurchaseOrderAction.ToolTip = null;
            this.ClosePurchaseOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClosePurchaseOrderAction_Execute);
            // 
            // PurchaseOrderViewController
            // 
            this.Actions.Add(this.EnterPurchaseWaybillAction);
            this.Actions.Add(this.EnterSupplierEvaluationAction);
            this.Actions.Add(this.ClosePurchaseOrderAction);

		}

		#endregion

        private DevExpress.ExpressApp.Actions.SimpleAction EnterPurchaseWaybillAction;
        private DevExpress.ExpressApp.Actions.SimpleAction EnterSupplierEvaluationAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ClosePurchaseOrderAction;
    }
}
