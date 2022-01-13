namespace EcoplastERP.Module.Controllers
{
    partial class SlicingWorkOrderViewController
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
            this.SetSlicingWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewSlicingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintSlicingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetSlicingWorkOrderStatusAction
            // 
            this.SetSlicingWorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetSlicingWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetSlicingWorkOrderStatusAction.Id = "SetSlicingWorkOrderStatusAction";
            this.SetSlicingWorkOrderStatusAction.ImageName = "BO_State";
            this.SetSlicingWorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetSlicingWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetSlicingWorkOrderStatusAction.ToolTip = null;
            this.SetSlicingWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetSlicingWorkOrderStatusAction_Execute);
            // 
            // ViewSlicingWorkOrderAction
            // 
            this.ViewSlicingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewSlicingWorkOrderAction.ConfirmationMessage = null;
            this.ViewSlicingWorkOrderAction.Id = "ViewSlicingWorkOrderAction";
            this.ViewSlicingWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            this.ViewSlicingWorkOrderAction.Items.Add(choiceActionItem1);
            this.ViewSlicingWorkOrderAction.Items.Add(choiceActionItem2);
            this.ViewSlicingWorkOrderAction.Items.Add(choiceActionItem3);
            this.ViewSlicingWorkOrderAction.Items.Add(choiceActionItem4);
            this.ViewSlicingWorkOrderAction.Items.Add(choiceActionItem5);
            this.ViewSlicingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewSlicingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewSlicingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewSlicingWorkOrderAction.ToolTip = null;
            this.ViewSlicingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewSlicingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewSlicingWorkOrderAction_Execute);
            // 
            // PrintSlicingMachineLoadAction
            // 
            this.PrintSlicingMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintSlicingMachineLoadAction.ConfirmationMessage = null;
            this.PrintSlicingMachineLoadAction.Id = "PrintSlicingMachineLoadAction";
            this.PrintSlicingMachineLoadAction.ImageName = "BO_Note";
            this.PrintSlicingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintSlicingMachineLoadAction.ToolTip = null;
            this.PrintSlicingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintSlicingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintSlicingMachineLoadAction_Execute);
            // 
            // SlicingWorkOrderViewController
            // 
            this.Actions.Add(this.SetSlicingWorkOrderStatusAction);
            this.Actions.Add(this.ViewSlicingWorkOrderAction);
            this.Actions.Add(this.PrintSlicingMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.SlicingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetSlicingWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewSlicingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintSlicingMachineLoadAction;
    }
}
