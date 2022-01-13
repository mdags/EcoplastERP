namespace EcoplastERP.Module.Controllers
{
    partial class RegeneratedWorkOrderViewController
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
            this.SetRegeneratedStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewRegeneratedWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintRegeneratedMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetRegeneratedStatusAction
            // 
            this.SetRegeneratedStatusAction.Caption = "Durumu Değiştir";
            this.SetRegeneratedStatusAction.ConfirmationMessage = null;
            this.SetRegeneratedStatusAction.Id = "SetRegeneratedStatusAction";
            this.SetRegeneratedStatusAction.ImageName = "BO_State";
            this.SetRegeneratedStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetRegeneratedStatusAction.ShowItemsOnClick = true;
            this.SetRegeneratedStatusAction.ToolTip = null;
            this.SetRegeneratedStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetRegeneratedStatusAction_Execute);
            // 
            // ViewRegeneratedWorkOrderAction
            // 
            this.ViewRegeneratedWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewRegeneratedWorkOrderAction.ConfirmationMessage = null;
            this.ViewRegeneratedWorkOrderAction.Id = "ViewRegeneratedWorkOrderAction";
            this.ViewRegeneratedWorkOrderAction.ImageName = "Action_Printing_Preview";
            this.ViewRegeneratedWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewRegeneratedWorkOrderAction.ShowItemsOnClick = true;
            this.ViewRegeneratedWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewRegeneratedWorkOrderAction.ToolTip = null;
            this.ViewRegeneratedWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewRegeneratedWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewRegeneratedWorkOrderAction_Execute);
            // 
            // PrintRegeneratedMachineLoadAction
            // 
            this.PrintRegeneratedMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintRegeneratedMachineLoadAction.ConfirmationMessage = null;
            this.PrintRegeneratedMachineLoadAction.Id = "PrintRegeneratedMachineLoadAction";
            this.PrintRegeneratedMachineLoadAction.ImageName = "BO_Note";
            this.PrintRegeneratedMachineLoadAction.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.RegeneratedWorkOrder);
            this.PrintRegeneratedMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintRegeneratedMachineLoadAction.ToolTip = null;
            this.PrintRegeneratedMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintRegeneratedMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintRegeneratedMachineLoadAction_Execute);
            // 
            // RegeneratedWorkOrderViewController
            // 
            this.Actions.Add(this.SetRegeneratedStatusAction);
            this.Actions.Add(this.ViewRegeneratedWorkOrderAction);
            this.Actions.Add(this.PrintRegeneratedMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.RegeneratedWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetRegeneratedStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewRegeneratedWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintRegeneratedMachineLoadAction;
    }
}
