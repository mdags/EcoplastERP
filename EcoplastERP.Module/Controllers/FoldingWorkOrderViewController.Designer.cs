namespace EcoplastERP.Module.Controllers
{
    partial class FoldingWorkOrderViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.SetFoldingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewFoldingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // SetFoldingStatusAction
            // 
            this.SetFoldingStatusAction.Caption = "Durumu Değiştir";
            this.SetFoldingStatusAction.ConfirmationMessage = null;
            this.SetFoldingStatusAction.Id = "SetFoldingStatusAction";
            this.SetFoldingStatusAction.ImageName = "BO_State";
            this.SetFoldingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetFoldingStatusAction.ShowItemsOnClick = true;
            this.SetFoldingStatusAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.FoldingWorkOrder);
            this.SetFoldingStatusAction.ToolTip = null;
            this.SetFoldingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetFoldingStatusAction_Execute);
            // 
            // ViewFoldingWorkOrderAction
            // 
            this.ViewFoldingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewFoldingWorkOrderAction.ConfirmationMessage = null;
            this.ViewFoldingWorkOrderAction.Id = "ViewFoldingWorkOrderAction";
            this.ViewFoldingWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            choiceActionItem5.Caption = "Aynı Siparişten";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.ViewFoldingWorkOrderAction.Items.Add(choiceActionItem1);
            this.ViewFoldingWorkOrderAction.Items.Add(choiceActionItem2);
            this.ViewFoldingWorkOrderAction.Items.Add(choiceActionItem3);
            this.ViewFoldingWorkOrderAction.Items.Add(choiceActionItem4);
            this.ViewFoldingWorkOrderAction.Items.Add(choiceActionItem5);
            this.ViewFoldingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewFoldingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewFoldingWorkOrderAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.FoldingWorkOrder);
            this.ViewFoldingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewFoldingWorkOrderAction.ToolTip = null;
            this.ViewFoldingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewFoldingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewFoldingWorkOrderAction_Execute);
            // 
            // FoldingWorkOrderViewController
            // 
            this.Actions.Add(this.SetFoldingStatusAction);
            this.Actions.Add(this.ViewFoldingWorkOrderAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetFoldingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewFoldingWorkOrderAction;
    }
}
