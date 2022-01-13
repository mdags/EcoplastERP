namespace EcoplastERP.Module.Controllers
{
    partial class ShippingPlanViewController
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
            this.CreateExpeditionByChoiceAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddExistingExpeditionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SetShippingPlanStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.RemoveFromExpeditionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateExpeditionByChoiceAction
            // 
            this.CreateExpeditionByChoiceAction.Caption = "Seçim İle Sefer Oluştur";
            this.CreateExpeditionByChoiceAction.ConfirmationMessage = null;
            this.CreateExpeditionByChoiceAction.Id = "CreateExpeditionByChoiceAction";
            this.CreateExpeditionByChoiceAction.ImageName = "ModelEditor_Action_Schema";
            this.CreateExpeditionByChoiceAction.TargetObjectsCriteria = "ShippingPlanStatus = \'WaitingforExpedition\'";
            this.CreateExpeditionByChoiceAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CreateExpeditionByChoiceAction.ToolTip = null;
            this.CreateExpeditionByChoiceAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CreateExpeditionByChoiceAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateExpeditionByChoiceAction_Execute);
            // 
            // AddExistingExpeditionAction
            // 
            this.AddExistingExpeditionAction.Caption = "Varolan Sefere Ekle";
            this.AddExistingExpeditionAction.ConfirmationMessage = null;
            this.AddExistingExpeditionAction.Id = "AddExistingExpeditionAction";
            this.AddExistingExpeditionAction.ImageName = "Action_Debug_Step";
            this.AddExistingExpeditionAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AddExistingExpeditionAction.TargetObjectsCriteria = "ShippingPlanStatus = \'WaitingforExpedition\'";
            this.AddExistingExpeditionAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.AddExistingExpeditionAction.ToolTip = null;
            this.AddExistingExpeditionAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.AddExistingExpeditionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddExistingExpeditionAction_Execute);
            // 
            // SetShippingPlanStatusAction
            // 
            this.SetShippingPlanStatusAction.Caption = "Durumu Değiştir";
            this.SetShippingPlanStatusAction.ConfirmationMessage = null;
            this.SetShippingPlanStatusAction.Id = "SetShippingPlanStatusAction";
            this.SetShippingPlanStatusAction.ImageName = "BO_State";
            this.SetShippingPlanStatusAction.ShowItemsOnClick = true;
            this.SetShippingPlanStatusAction.ToolTip = null;
            this.SetShippingPlanStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetShippingPlanStatusAction_Execute);
            // 
            // RemoveFromExpeditionAction
            // 
            this.RemoveFromExpeditionAction.Caption = "Seferden Çıkart";
            this.RemoveFromExpeditionAction.ConfirmationMessage = null;
            this.RemoveFromExpeditionAction.Id = "RemoveFromExpeditionAction";
            this.RemoveFromExpeditionAction.ImageName = "Action_Cancel";
            this.RemoveFromExpeditionAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.RemoveFromExpeditionAction.TargetObjectsCriteria = "ExpeditionDetail is not null";
            this.RemoveFromExpeditionAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.RemoveFromExpeditionAction.ToolTip = null;
            this.RemoveFromExpeditionAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.RemoveFromExpeditionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveFromExpeditionAction_Execute);
            // 
            // ShippingPlanViewController
            // 
            this.Actions.Add(this.CreateExpeditionByChoiceAction);
            this.Actions.Add(this.AddExistingExpeditionAction);
            this.Actions.Add(this.SetShippingPlanStatusAction);
            this.Actions.Add(this.RemoveFromExpeditionAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ShippingObjects.ShippingPlan);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateExpeditionByChoiceAction;
        private DevExpress.ExpressApp.Actions.SimpleAction AddExistingExpeditionAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetShippingPlanStatusAction;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveFromExpeditionAction;
    }
}
