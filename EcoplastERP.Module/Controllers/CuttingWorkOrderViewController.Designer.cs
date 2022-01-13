namespace EcoplastERP.Module.Controllers
{
    partial class CuttingWorkOrderViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem10 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.SetCuttingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewCuttingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintCuttingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetCuttingStatusAction
            // 
            this.SetCuttingStatusAction.Caption = "Set Cutting Status";
            this.SetCuttingStatusAction.ConfirmationMessage = null;
            this.SetCuttingStatusAction.Id = "SetCuttingStatusAction";
            this.SetCuttingStatusAction.ImageName = "BO_State";
            this.SetCuttingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetCuttingStatusAction.ShowItemsOnClick = true;
            this.SetCuttingStatusAction.ToolTip = null;
            this.SetCuttingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetStatusAction_Execute);
            // 
            // ViewCuttingWorkOrderAction
            // 
            this.ViewCuttingWorkOrderAction.Caption = "Ýncele/Deðiþtir";
            this.ViewCuttingWorkOrderAction.ConfirmationMessage = null;
            this.ViewCuttingWorkOrderAction.Id = "ViewCuttingWorkOrderAction";
            this.ViewCuttingWorkOrderAction.ImageName = "Action_Printing_Preview";
            choiceActionItem6.Caption = "Ayný Firmanýn Ayný Stoðu";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "Ayný Stoktan";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            choiceActionItem8.Caption = "Ayný Firmanýn Son Üretim Sipariþi";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            choiceActionItem9.Caption = "Ayný Makine Yükünden";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
            choiceActionItem10.Caption = "Ayný Sipariþten";
            choiceActionItem10.ImageName = null;
            choiceActionItem10.Shortcut = null;
            choiceActionItem10.ToolTip = null;
            this.ViewCuttingWorkOrderAction.Items.Add(choiceActionItem6);
            this.ViewCuttingWorkOrderAction.Items.Add(choiceActionItem7);
            this.ViewCuttingWorkOrderAction.Items.Add(choiceActionItem8);
            this.ViewCuttingWorkOrderAction.Items.Add(choiceActionItem9);
            this.ViewCuttingWorkOrderAction.Items.Add(choiceActionItem10);
            this.ViewCuttingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewCuttingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewCuttingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewCuttingWorkOrderAction.ToolTip = null;
            this.ViewCuttingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewCuttingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewCuttingWorkOrderAction_Execute);
            // 
            // PrintCuttingMachineLoadAction
            // 
            this.PrintCuttingMachineLoadAction.Caption = "Makine Yükü Yazdýr";
            this.PrintCuttingMachineLoadAction.ConfirmationMessage = null;
            this.PrintCuttingMachineLoadAction.Id = "PrintCuttingMachineLoadAction";
            this.PrintCuttingMachineLoadAction.ImageName = "BO_Note";
            this.PrintCuttingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintCuttingMachineLoadAction.ToolTip = null;
            this.PrintCuttingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintCuttingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintCuttingMachineLoadAction_Execute);
            // 
            // CuttingWorkOrderViewController
            // 
            this.Actions.Add(this.SetCuttingStatusAction);
            this.Actions.Add(this.ViewCuttingWorkOrderAction);
            this.Actions.Add(this.PrintCuttingMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.CuttingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetCuttingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewCuttingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintCuttingMachineLoadAction;
    }
}
