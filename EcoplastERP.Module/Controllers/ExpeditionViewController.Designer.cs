namespace EcoplastERP.Module.Controllers
{
    partial class ExpeditionViewController
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
            this.AllocateShippingCostAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CloseExpeditionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenExpeditionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AllocateShippingCostAction
            // 
            this.AllocateShippingCostAction.Caption = "Nakliye Daðýt";
            this.AllocateShippingCostAction.ConfirmationMessage = null;
            this.AllocateShippingCostAction.Id = "AllocateShippingCostAction";
            this.AllocateShippingCostAction.ImageName = "BO_Opportunity";
            this.AllocateShippingCostAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AllocateShippingCostAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AllocateShippingCostAction.ToolTip = null;
            this.AllocateShippingCostAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AllocateShippingCostAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AllocateShippingCostAction_Execute);
            // 
            // CloseExpeditionAction
            // 
            this.CloseExpeditionAction.Caption = "Seferi Kapat";
            this.CloseExpeditionAction.ConfirmationMessage = null;
            this.CloseExpeditionAction.Id = "CloseExpeditionAction";
            this.CloseExpeditionAction.ImageName = "BO_Security_Permission_Member";
            this.CloseExpeditionAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CloseExpeditionAction.TargetObjectsCriteria = "ExpeditionStatus != \'WaitingforTruck\' or ExpeditionStatus != \'Completed\'";
            this.CloseExpeditionAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CloseExpeditionAction.ToolTip = null;
            this.CloseExpeditionAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CloseExpeditionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CloseExpeditionAction_Execute);
            // 
            // OpenExpeditionAction
            // 
            this.OpenExpeditionAction.Caption = "Seferi Aç";
            this.OpenExpeditionAction.ConfirmationMessage = null;
            this.OpenExpeditionAction.Id = "OpenExpeditionAction";
            this.OpenExpeditionAction.ImageName = "Action_Edit";
            this.OpenExpeditionAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.OpenExpeditionAction.TargetObjectsCriteria = "ExpeditionStatus = \'Completed\'";
            this.OpenExpeditionAction.ToolTip = null;
            this.OpenExpeditionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenExpeditionAction_Execute);
            // 
            // ExpeditionViewController
            // 
            this.Actions.Add(this.AllocateShippingCostAction);
            this.Actions.Add(this.CloseExpeditionAction);
            this.Actions.Add(this.OpenExpeditionAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ShippingObjects.Expedition);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction AllocateShippingCostAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CloseExpeditionAction;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenExpeditionAction;
    }
}
