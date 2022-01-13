namespace EcoplastERP.Module.Controllers
{
    partial class CastRegeneratedViewController
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
            this.SetCastRegeneratedStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewCastRegeneratedWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintCastRegeneratedMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetCastRegeneratedStatusAction
            // 
            this.SetCastRegeneratedStatusAction.Caption = "Durumu Değiştir";
            this.SetCastRegeneratedStatusAction.ConfirmationMessage = null;
            this.SetCastRegeneratedStatusAction.Id = "SetCastRegeneratedStatusAction";
            this.SetCastRegeneratedStatusAction.ImageName = "BO_State";
            this.SetCastRegeneratedStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetCastRegeneratedStatusAction.ShowItemsOnClick = true;
            this.SetCastRegeneratedStatusAction.ToolTip = null;
            this.SetCastRegeneratedStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetCastRegeneratedStatusAction_Execute);
            // 
            // ViewCastRegeneratedWorkOrderAction
            // 
            this.ViewCastRegeneratedWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewCastRegeneratedWorkOrderAction.ConfirmationMessage = null;
            this.ViewCastRegeneratedWorkOrderAction.Id = "ViewCastRegeneratedWorkOrderAction";
            this.ViewCastRegeneratedWorkOrderAction.ImageName = "Action_Printing_Preview";
            this.ViewCastRegeneratedWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewCastRegeneratedWorkOrderAction.ShowItemsOnClick = true;
            this.ViewCastRegeneratedWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewCastRegeneratedWorkOrderAction.ToolTip = null;
            this.ViewCastRegeneratedWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewCastRegeneratedWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewCastRegeneratedWorkOrderAction_Execute);
            // 
            // PrintCastRegeneratedMachineLoadAction
            // 
            this.PrintCastRegeneratedMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintCastRegeneratedMachineLoadAction.ConfirmationMessage = null;
            this.PrintCastRegeneratedMachineLoadAction.Id = "PrintCastRegeneratedMachineLoadAction";
            this.PrintCastRegeneratedMachineLoadAction.ImageName = "BO_Note";
            this.PrintCastRegeneratedMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintCastRegeneratedMachineLoadAction.ToolTip = null;
            this.PrintCastRegeneratedMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintCastRegeneratedMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintCastRegeneratedMachineLoadAction_Execute);
            // 
            // CastRegeneratedViewController
            // 
            this.Actions.Add(this.SetCastRegeneratedStatusAction);
            this.Actions.Add(this.ViewCastRegeneratedWorkOrderAction);
            this.Actions.Add(this.PrintCastRegeneratedMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.CastRegeneratedWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetCastRegeneratedStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewCastRegeneratedWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintCastRegeneratedMachineLoadAction;
    }
}
