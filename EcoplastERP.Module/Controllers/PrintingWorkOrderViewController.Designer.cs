namespace EcoplastERP.Module.Controllers
{
    partial class PrintingWorkOrderViewController
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
            this.SetPrintingWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewPrintingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintPrintingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetPrintingWorkOrderStatusAction
            // 
            this.SetPrintingWorkOrderStatusAction.Caption = "Set Printing Work Order Status";
            this.SetPrintingWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetPrintingWorkOrderStatusAction.Id = "SetPrintingWorkOrderStatusAction";
            this.SetPrintingWorkOrderStatusAction.ImageName = "BO_State";
            this.SetPrintingWorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetPrintingWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetPrintingWorkOrderStatusAction.ToolTip = null;
            this.SetPrintingWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetPrintingWorkOrderStatusAction_Execute);
            // 
            // ViewPrintingWorkOrderAction
            // 
            this.ViewPrintingWorkOrderAction.Caption = "Ýncele/Deðiþtir";
            this.ViewPrintingWorkOrderAction.ConfirmationMessage = null;
            this.ViewPrintingWorkOrderAction.Id = "ViewPrintingWorkOrderAction";
            this.ViewPrintingWorkOrderAction.ImageName = "Action_Printing_Preview";
            choiceActionItem1.Caption = "Ayný Firmanýn Ayný Stoðu";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Ayný Stoktan";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Ayný Firmanýn Son Üretim Sipariþi";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "Ayný Makine Yükünden";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "Ayný Sipariþten";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.ViewPrintingWorkOrderAction.Items.Add(choiceActionItem1);
            this.ViewPrintingWorkOrderAction.Items.Add(choiceActionItem2);
            this.ViewPrintingWorkOrderAction.Items.Add(choiceActionItem3);
            this.ViewPrintingWorkOrderAction.Items.Add(choiceActionItem4);
            this.ViewPrintingWorkOrderAction.Items.Add(choiceActionItem5);
            this.ViewPrintingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewPrintingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewPrintingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewPrintingWorkOrderAction.ToolTip = null;
            this.ViewPrintingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewPrintingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewPrintingWorkOrderAction_Execute);
            // 
            // PrintPrintingMachineLoadAction
            // 
            this.PrintPrintingMachineLoadAction.Caption = "Makine Yükü Yazdýr";
            this.PrintPrintingMachineLoadAction.ConfirmationMessage = null;
            this.PrintPrintingMachineLoadAction.Id = "PrintPrintingMachineLoadAction";
            this.PrintPrintingMachineLoadAction.ImageName = "BO_Note";
            this.PrintPrintingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintPrintingMachineLoadAction.ToolTip = null;
            this.PrintPrintingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintPrintingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintPrintingMachineLoadAction_Execute);
            // 
            // PrintingWorkOrderViewController
            // 
            this.Actions.Add(this.SetPrintingWorkOrderStatusAction);
            this.Actions.Add(this.ViewPrintingWorkOrderAction);
            this.Actions.Add(this.PrintPrintingMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.PrintingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetPrintingWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewPrintingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintPrintingMachineLoadAction;
    }
}
