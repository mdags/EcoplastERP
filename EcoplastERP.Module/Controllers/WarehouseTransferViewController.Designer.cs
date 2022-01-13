namespace EcoplastERP.Module.Controllers
{
    partial class WarehouseTransferViewController
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
            this.WarehouseTransferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // WarehouseTransferAction
            // 
            this.WarehouseTransferAction.Caption = "Transfer Et";
            this.WarehouseTransferAction.ConfirmationMessage = null;
            this.WarehouseTransferAction.Id = "WarehouseTransferAction";
            this.WarehouseTransferAction.ImageName = "Action_Workflow_Activate";
            this.WarehouseTransferAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.WarehouseTransferAction.TargetObjectsCriteria = "Status = \'Waiting\'";
            this.WarehouseTransferAction.ToolTip = null;
            this.WarehouseTransferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WarehouseTransferAction_Execute);
            // 
            // WarehouseTransferViewController
            // 
            this.Actions.Add(this.WarehouseTransferAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductObjects.WarehouseTransfer);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction WarehouseTransferAction;
    }
}
