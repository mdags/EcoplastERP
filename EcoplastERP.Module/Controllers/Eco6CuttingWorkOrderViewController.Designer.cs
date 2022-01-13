namespace EcoplastERP.Module.Controllers
{
    partial class Eco6CuttingWorkOrderViewController
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
            this.SetEco6CuttingWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewEco6CuttingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintEco6CuttingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetEco6CuttingWorkOrderStatusAction
            // 
            this.SetEco6CuttingWorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetEco6CuttingWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetEco6CuttingWorkOrderStatusAction.Id = "SetEco6CuttingWorkOrderStatusAction";
            this.SetEco6CuttingWorkOrderStatusAction.ImageName = "BO_State";
            this.SetEco6CuttingWorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetEco6CuttingWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetEco6CuttingWorkOrderStatusAction.ToolTip = null;
            this.SetEco6CuttingWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetEco6CuttingWorkOrderStatusAction_Execute);
            // 
            // ViewEco6CuttingWorkOrderAction
            // 
            this.ViewEco6CuttingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewEco6CuttingWorkOrderAction.ConfirmationMessage = null;
            this.ViewEco6CuttingWorkOrderAction.Id = "ViewEco6CuttingWorkOrderAction";
            this.ViewEco6CuttingWorkOrderAction.ImageName = "Action_Printing_Preview";
            this.ViewEco6CuttingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewEco6CuttingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewEco6CuttingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewEco6CuttingWorkOrderAction.ToolTip = null;
            this.ViewEco6CuttingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewEco6CuttingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewEco6CuttingWorkOrderAction_Execute);
            // 
            // PrintEco6CuttingMachineLoadAction
            // 
            this.PrintEco6CuttingMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintEco6CuttingMachineLoadAction.ConfirmationMessage = null;
            this.PrintEco6CuttingMachineLoadAction.Id = "PrintEco6CuttingMachineLoadAction";
            this.PrintEco6CuttingMachineLoadAction.ImageName = "BO_Note";
            this.PrintEco6CuttingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintEco6CuttingMachineLoadAction.ToolTip = null;
            this.PrintEco6CuttingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintEco6CuttingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintEco6CuttingMachineLoadAction_Execute);
            // 
            // Eco6CuttingWorkOrderViewController
            // 
            this.Actions.Add(this.SetEco6CuttingWorkOrderStatusAction);
            this.Actions.Add(this.ViewEco6CuttingWorkOrderAction);
            this.Actions.Add(this.PrintEco6CuttingMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.Eco6CuttingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetEco6CuttingWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewEco6CuttingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintEco6CuttingMachineLoadAction;
    }
}
