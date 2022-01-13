namespace EcoplastERP.Module.Controllers
{
    partial class CastSlicingViewController
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
            this.SetCastSlicingWorkOrderStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewCastSlicingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintCastSlicingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetCastSlicingWorkOrderStatusAction
            // 
            this.SetCastSlicingWorkOrderStatusAction.Caption = "Durumu Değiştir";
            this.SetCastSlicingWorkOrderStatusAction.ConfirmationMessage = null;
            this.SetCastSlicingWorkOrderStatusAction.Id = "SetCastSlicingWorkOrderStatusAction";
            this.SetCastSlicingWorkOrderStatusAction.ImageName = "BO_State";
            this.SetCastSlicingWorkOrderStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetCastSlicingWorkOrderStatusAction.ShowItemsOnClick = true;
            this.SetCastSlicingWorkOrderStatusAction.ToolTip = null;
            this.SetCastSlicingWorkOrderStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetCastSlicingWorkOrderStatusAction_Execute);
            // 
            // ViewCastSlicingWorkOrderAction
            // 
            this.ViewCastSlicingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewCastSlicingWorkOrderAction.ConfirmationMessage = null;
            this.ViewCastSlicingWorkOrderAction.Id = "ViewCastSlicingWorkOrderAction";
            this.ViewCastSlicingWorkOrderAction.ImageName = "Action_Printing_Preview";
            this.ViewCastSlicingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewCastSlicingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewCastSlicingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewCastSlicingWorkOrderAction.ToolTip = null;
            this.ViewCastSlicingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewCastSlicingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewCastSlicingWorkOrderAction_Execute);
            // 
            // PrintCastSlicingMachineLoadAction
            // 
            this.PrintCastSlicingMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintCastSlicingMachineLoadAction.ConfirmationMessage = null;
            this.PrintCastSlicingMachineLoadAction.Id = "PrintCastSlicingMachineLoadAction";
            this.PrintCastSlicingMachineLoadAction.ImageName = "BO_Note";
            this.PrintCastSlicingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintCastSlicingMachineLoadAction.ToolTip = null;
            this.PrintCastSlicingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintCastSlicingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintCastSlicingMachineLoadAction_Execute);
            // 
            // CastSlicingViewController
            // 
            this.Actions.Add(this.SetCastSlicingWorkOrderStatusAction);
            this.Actions.Add(this.ViewCastSlicingWorkOrderAction);
            this.Actions.Add(this.PrintCastSlicingMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.CastSlicingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetCastSlicingWorkOrderStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewCastSlicingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintCastSlicingMachineLoadAction;
    }
}
