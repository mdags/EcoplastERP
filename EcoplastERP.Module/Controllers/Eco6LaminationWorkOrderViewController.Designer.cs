namespace EcoplastERP.Module.Controllers
{
    partial class Eco6LaminationWorkOrderViewController
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
            this.SetEco6LaminationWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewEco6LaminationWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintEco6LaminationMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetEco6LaminationWorkOrderStatusAction
            // 
            this.SetEco6LaminationWorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetEco6LaminationWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetEco6LaminationWorkOrderStatusAction.Id = "SetEco6LaminationWorkOrderStatusAction";
            this.SetEco6LaminationWorkOrderStatusAction.ImageName = "BO_State";
            this.SetEco6LaminationWorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetEco6LaminationWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetEco6LaminationWorkOrderStatusAction.ToolTip = null;
            this.SetEco6LaminationWorkOrderStatusAction.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.SetEco6LaminationWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetLaminationWorkOrderStatusAction_Execute);
            // 
            // ViewEco6LaminationWorkOrderAction
            // 
            this.ViewEco6LaminationWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewEco6LaminationWorkOrderAction.ConfirmationMessage = null;
            this.ViewEco6LaminationWorkOrderAction.Id = "ViewEco6LaminationWorkOrderAction";
            this.ViewEco6LaminationWorkOrderAction.ImageName = "Action_Printing_Preview";
            this.ViewEco6LaminationWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewEco6LaminationWorkOrderAction.ShowItemsOnClick = true;
            this.ViewEco6LaminationWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewEco6LaminationWorkOrderAction.ToolTip = null;
            this.ViewEco6LaminationWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewEco6LaminationWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewEco6LaminationWorkOrderAction_Execute);
            // 
            // PrintEco6LaminationMachineLoadAction
            // 
            this.PrintEco6LaminationMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintEco6LaminationMachineLoadAction.ConfirmationMessage = null;
            this.PrintEco6LaminationMachineLoadAction.Id = "PrintEco6LaminationMachineLoadAction";
            this.PrintEco6LaminationMachineLoadAction.ImageName = "BO_Note";
            this.PrintEco6LaminationMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintEco6LaminationMachineLoadAction.ToolTip = null;
            this.PrintEco6LaminationMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintEco6LaminationMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintEco6LaminationMachineLoadAction_Execute);
            // 
            // Eco6LaminationWorkOrderViewController
            // 
            this.Actions.Add(this.SetEco6LaminationWorkOrderStatusAction);
            this.Actions.Add(this.ViewEco6LaminationWorkOrderAction);
            this.Actions.Add(this.PrintEco6LaminationMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.Eco6LaminationWorkOrder);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetEco6LaminationWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewEco6LaminationWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintEco6LaminationMachineLoadAction;
    }
}
