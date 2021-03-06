namespace EcoplastERP.Module.Controllers
{
    partial class LaminationWorkOrderViewController
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
            this.SetLaminationWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewLaminationWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintLaminationMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetLaminationWorkOrderStatusAction
            // 
            this.SetLaminationWorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetLaminationWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetLaminationWorkOrderStatusAction.Id = "SetLaminationWorkOrderStatusAction";
            this.SetLaminationWorkOrderStatusAction.ImageName = "BO_State";
            this.SetLaminationWorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetLaminationWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetLaminationWorkOrderStatusAction.ToolTip = null;
            this.SetLaminationWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetLaminationWorkOrderStatusAction_Execute);
            // 
            // ViewLaminationWorkOrderAction
            // 
            this.ViewLaminationWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewLaminationWorkOrderAction.ConfirmationMessage = null;
            this.ViewLaminationWorkOrderAction.Id = "ViewLaminationWorkOrderAction";
            this.ViewLaminationWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            this.ViewLaminationWorkOrderAction.Items.Add(choiceActionItem1);
            this.ViewLaminationWorkOrderAction.Items.Add(choiceActionItem2);
            this.ViewLaminationWorkOrderAction.Items.Add(choiceActionItem3);
            this.ViewLaminationWorkOrderAction.Items.Add(choiceActionItem4);
            this.ViewLaminationWorkOrderAction.Items.Add(choiceActionItem5);
            this.ViewLaminationWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewLaminationWorkOrderAction.ShowItemsOnClick = true;
            this.ViewLaminationWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewLaminationWorkOrderAction.ToolTip = null;
            this.ViewLaminationWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewLaminationWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewLaminationWorkOrderAction_Execute);
            // 
            // PrintLaminationMachineLoadAction
            // 
            this.PrintLaminationMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintLaminationMachineLoadAction.ConfirmationMessage = null;
            this.PrintLaminationMachineLoadAction.Id = "PrintLaminationMachineLoadAction";
            this.PrintLaminationMachineLoadAction.ImageName = "BO_Note";
            this.PrintLaminationMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintLaminationMachineLoadAction.ToolTip = null;
            this.PrintLaminationMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintLaminationMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintLaminationMachineLoadAction_Execute);
            // 
            // LaminationWorkOrderViewController
            // 
            this.Actions.Add(this.SetLaminationWorkOrderStatusAction);
            this.Actions.Add(this.ViewLaminationWorkOrderAction);
            this.Actions.Add(this.PrintLaminationMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.LaminationWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetLaminationWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewLaminationWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintLaminationMachineLoadAction;
    }
}
