namespace EcoplastERP.Module.Controllers
{
    partial class RezervationViewController
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
            this.RezervationTransferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // RezervationTransferAction
            // 
            this.RezervationTransferAction.Caption = "Rezervation Transfer";
            this.RezervationTransferAction.ConfirmationMessage = null;
            this.RezervationTransferAction.Id = "RezervationTransferAction";
            this.RezervationTransferAction.ImageName = "Action_Workflow_Activate";
            this.RezervationTransferAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.RezervationTransferAction.TargetObjectsCriteria = "Status = \'Waiting\'";
            this.RezervationTransferAction.ToolTip = null;
            this.RezervationTransferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RezervationTransferAction_Execute);
            // 
            // RezervationViewController
            // 
            this.Actions.Add(this.RezervationTransferAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.Rezervation);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction RezervationTransferAction;
    }
}
