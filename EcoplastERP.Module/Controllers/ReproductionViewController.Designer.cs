namespace EcoplastERP.Module.Controllers
{
    partial class ReproductionViewController
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
            this.SetReproductionStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintReporoductionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetReproductionStatusAction
            // 
            this.SetReproductionStatusAction.Caption = "Durumu Değiştir";
            this.SetReproductionStatusAction.ConfirmationMessage = null;
            this.SetReproductionStatusAction.Id = "SetReproductionStatusAction";
            this.SetReproductionStatusAction.ImageName = "BO_State";
            this.SetReproductionStatusAction.ShowItemsOnClick = true;
            this.SetReproductionStatusAction.ToolTip = null;
            this.SetReproductionStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetReproductionStatusAction_Execute);
            // 
            // PrintReporoductionAction
            // 
            this.PrintReporoductionAction.Caption = "Yazdır";
            this.PrintReporoductionAction.ConfirmationMessage = null;
            this.PrintReporoductionAction.Id = "PrintReporoductionAction";
            this.PrintReporoductionAction.ImageName = "Action_Printing_Print";
            this.PrintReporoductionAction.TargetObjectsCriteria = "ReproductionStatus != \'Produced\'";
            this.PrintReporoductionAction.ToolTip = null;
            this.PrintReporoductionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintReporoductionAction_Execute);
            // 
            // ReproductionViewController
            // 
            this.Actions.Add(this.SetReproductionStatusAction);
            this.Actions.Add(this.PrintReporoductionAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.PlateArchiveObjects.Reproduction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetReproductionStatusAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintReporoductionAction;
    }
}
