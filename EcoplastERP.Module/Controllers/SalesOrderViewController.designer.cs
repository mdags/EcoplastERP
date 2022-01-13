namespace EcoplastERP.Module.Controllers
{
    partial class SalesOrderViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.CopyWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction();
            this.SetSalesOrderDetailStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction();
            this.CopySalesOrderDetailAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction();
            this.CreateWorkOrderAction = new DevExpress.ExpressApp.Actions.SimpleAction();
            // 
            // CopyWorkOrderAction
            // 
            this.CopyWorkOrderAction.Caption = "Copy Work Order";
            this.CopyWorkOrderAction.ConfirmationMessage = null;
            this.CopyWorkOrderAction.Id = "CopyWorkOrderAction";
            this.CopyWorkOrderAction.ImageName = "Action_Copy";
            choiceActionItem1.Caption = "Aynı Firmanın Aynı Stoğu";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Aynı Stoktan";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Aynı Firmanın Son Üretim Siparişi";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "Aynı Makine Yükünden";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "Farklı Firmadan";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "Aynı Siparişten";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            this.CopyWorkOrderAction.Items.Add(choiceActionItem1);
            this.CopyWorkOrderAction.Items.Add(choiceActionItem2);
            this.CopyWorkOrderAction.Items.Add(choiceActionItem3);
            this.CopyWorkOrderAction.Items.Add(choiceActionItem4);
            this.CopyWorkOrderAction.Items.Add(choiceActionItem5);
            this.CopyWorkOrderAction.Items.Add(choiceActionItem6);
            this.CopyWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.CopyWorkOrderAction.ShowItemsOnClick = true;
            this.CopyWorkOrderAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MarketingObjects.SalesOrderDetail);
            this.CopyWorkOrderAction.ToolTip = null;
            this.CopyWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CopyWorkOrderAction_Execute);
            // 
            // SetSalesOrderDetailStatusAction
            // 
            this.SetSalesOrderDetailStatusAction.Caption = "Set Sales Order Detail Status";
            this.SetSalesOrderDetailStatusAction.ConfirmationMessage = null;
            this.SetSalesOrderDetailStatusAction.Id = "SetSalesOrderDetailStatusAction";
            this.SetSalesOrderDetailStatusAction.ImageName = "BO_State";
            this.SetSalesOrderDetailStatusAction.ShowItemsOnClick = true;
            this.SetSalesOrderDetailStatusAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MarketingObjects.SalesOrderDetail);
            this.SetSalesOrderDetailStatusAction.ToolTip = null;
            this.SetSalesOrderDetailStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetSalesOrderDetailStatusAction_Execute);
            // 
            // CopySalesOrderDetailAction
            // 
            this.CopySalesOrderDetailAction.AcceptButtonCaption = null;
            this.CopySalesOrderDetailAction.CancelButtonCaption = null;
            this.CopySalesOrderDetailAction.Caption = "Copy Sales Order Detail";
            this.CopySalesOrderDetailAction.ConfirmationMessage = null;
            this.CopySalesOrderDetailAction.Id = "CopySalesOrderDetailAction";
            this.CopySalesOrderDetailAction.ImageName = "Action_Copy";
            this.CopySalesOrderDetailAction.TargetObjectsCriteria = "Contact is not null";
            this.CopySalesOrderDetailAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MarketingObjects.SalesOrder);
            this.CopySalesOrderDetailAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CopySalesOrderDetailAction.ToolTip = null;
            this.CopySalesOrderDetailAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CopySalesOrderDetailAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CopySalesOrderDetailAction_CustomizePopupWindowParams);
            this.CopySalesOrderDetailAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CopySalesOrderDetailAction_Execute);
            // 
            // CreateWorkOrderAction
            // 
            this.CreateWorkOrderAction.Caption = "Üretim Siparişi Oluştur";
            this.CreateWorkOrderAction.ConfirmationMessage = null;
            this.CreateWorkOrderAction.Id = "CreateWorkOrderAction";
            this.CreateWorkOrderAction.ImageName = "BO_Task";
            this.CreateWorkOrderAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CreateWorkOrderAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MarketingObjects.SalesOrderDetail);
            this.CreateWorkOrderAction.ToolTip = null;
            this.CreateWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateWorkOrderAction_Execute);
            // 
            // SalesOrderViewController
            // 
            this.Actions.Add(this.CopyWorkOrderAction);
            this.Actions.Add(this.SetSalesOrderDetailStatusAction);
            this.Actions.Add(this.CopySalesOrderDetailAction);
            this.Actions.Add(this.CreateWorkOrderAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetSalesOrderDetailStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CopyWorkOrderAction;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CopySalesOrderDetailAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateWorkOrderAction;
    }
}
