namespace EcoplastERP.Module.Controllers
{
    partial class Eco6WorkOrderViewController
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
            this.SetEco6WorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewEco6WorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintEco6MachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetEco6WorkOrderStatusAction
            // 
            this.SetEco6WorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetEco6WorkOrderStatusAction.ConfirmationMessage = null;
            this.SetEco6WorkOrderStatusAction.Id = "SetEco6WorkOrderStatusAction";
            this.SetEco6WorkOrderStatusAction.ImageName = "BO_State";
            this.SetEco6WorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetEco6WorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetEco6WorkOrderStatusAction.ToolTip = null;
            this.SetEco6WorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetEco6WorkOrderStatusAction_Execute);
            // 
            // ViewEco6WorkOrderAction
            // 
            this.ViewEco6WorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewEco6WorkOrderAction.ConfirmationMessage = null;
            this.ViewEco6WorkOrderAction.Id = "ViewEco6WorkOrderAction";
            this.ViewEco6WorkOrderAction.ImageName = "Action_Printing_Preview";
            this.ViewEco6WorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewEco6WorkOrderAction.ShowItemsOnClick = true;
            this.ViewEco6WorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewEco6WorkOrderAction.ToolTip = null;
            this.ViewEco6WorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewEco6WorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewEco6WorkOrderAction_Execute);
            // 
            // PrintEco6MachineLoadAction
            // 
            this.PrintEco6MachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintEco6MachineLoadAction.ConfirmationMessage = null;
            this.PrintEco6MachineLoadAction.Id = "PrintEco6MachineLoadAction";
            this.PrintEco6MachineLoadAction.ImageName = "BO_Note";
            this.PrintEco6MachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintEco6MachineLoadAction.ToolTip = null;
            this.PrintEco6MachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintEco6MachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintEco6MachineLoadAction_Execute);
            // 
            // Eco6WorkOrderViewController
            // 
            this.Actions.Add(this.SetEco6WorkOrderStatusAction);
            this.Actions.Add(this.ViewEco6WorkOrderAction);
            this.Actions.Add(this.PrintEco6MachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.Eco6WorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetEco6WorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewEco6WorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintEco6MachineLoadAction;
    }
}
