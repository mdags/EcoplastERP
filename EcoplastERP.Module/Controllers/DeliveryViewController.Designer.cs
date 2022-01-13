namespace EcoplastERP.Module.Controllers
{
    partial class DeliveryViewController
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
            this.CreateSalesWaybillAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            this.ChangeExpeditionAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            // 
            // CreateSalesWaybillAction
            // 
            this.CreateSalesWaybillAction.Caption = "İrsaliye Oluştur";
            this.CreateSalesWaybillAction.ConfirmationMessage = null;
            this.CreateSalesWaybillAction.Id = "CreateSalesWaybillAction";
            this.CreateSalesWaybillAction.ImageName = "BO_Sale";
            this.CreateSalesWaybillAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CreateSalesWaybillAction.TargetObjectsCriteria = "DeliveryStatus = \'WaitingforWaybill\'";
            this.CreateSalesWaybillAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CreateSalesWaybillAction.ToolTip = null;
            this.CreateSalesWaybillAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CreateSalesWaybillAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateSalesWaybillAction_Execute);
            // 
            // ChangeExpeditionAction
            // 
            this.ChangeExpeditionAction.Caption = "Sefer Değiştir";
            this.ChangeExpeditionAction.ConfirmationMessage = null;
            this.ChangeExpeditionAction.Id = "ChangeExpeditionAction";
            this.ChangeExpeditionAction.ImageName = "Action_Change_State";
            this.ChangeExpeditionAction.TargetViewId = "Delivery_ListView";
            this.ChangeExpeditionAction.ToolTip = null;
            this.ChangeExpeditionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ChangeExpeditionAction_Execute);
            // 
            // DeliveryViewController
            // 
            this.Actions.Add(this.CreateSalesWaybillAction);
            this.Actions.Add(this.ChangeExpeditionAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ShippingObjects.Delivery);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateSalesWaybillAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ChangeExpeditionAction;
    }
}
