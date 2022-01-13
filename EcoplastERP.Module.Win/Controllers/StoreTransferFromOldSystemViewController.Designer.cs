namespace EcoplastERP.Module.Win.Controllers
{
    partial class StoreTransferFromOldSystemViewController
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
            this.TransferStoreAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TransferStoreAction
            // 
            this.TransferStoreAction.Caption = "Seçili Kayıtları Transfer Et";
            this.TransferStoreAction.ConfirmationMessage = null;
            this.TransferStoreAction.Id = "TransferStoreAction";
            this.TransferStoreAction.ImageName = "Action_Change_State";
            this.TransferStoreAction.ToolTip = null;
            this.TransferStoreAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TransferStoreAction_Execute);
            // 
            // StoreTransferFromOldSystemViewController
            // 
            this.Actions.Add(this.TransferStoreAction);
            this.TargetViewId = "StoreTransferFromOldSystem_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction TransferStoreAction;
    }
}
