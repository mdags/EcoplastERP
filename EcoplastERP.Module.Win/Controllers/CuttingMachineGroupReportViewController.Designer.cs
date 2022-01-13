namespace EcoplastERP.Module.Win.Controllers
{
    partial class CuttingMachineGroupReportViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.CuttingMachineGroupDynamicReportAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CreateCuttingMachineReportChoiceAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // CuttingMachineGroupDynamicReportAction
            // 
            this.CuttingMachineGroupDynamicReportAction.Caption = "Dinamik Raporlar";
            this.CuttingMachineGroupDynamicReportAction.ConfirmationMessage = null;
            this.CuttingMachineGroupDynamicReportAction.Id = "CuttingMachineGroupDynamicReportAction";
            this.CuttingMachineGroupDynamicReportAction.ImageName = "Action_Report_Object_Inplace_Preview";
            choiceActionItem1.Caption = "Tümü";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Satış Siparişleri";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Planlama Siparişleri";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            this.CuttingMachineGroupDynamicReportAction.Items.Add(choiceActionItem1);
            this.CuttingMachineGroupDynamicReportAction.Items.Add(choiceActionItem2);
            this.CuttingMachineGroupDynamicReportAction.Items.Add(choiceActionItem3);
            this.CuttingMachineGroupDynamicReportAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            this.CuttingMachineGroupDynamicReportAction.ShowItemsOnClick = true;
            this.CuttingMachineGroupDynamicReportAction.ToolTip = null;
            this.CuttingMachineGroupDynamicReportAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CuttingMachineGroupDynamicReportAction_Execute);
            // 
            // CreateCuttingMachineReportChoiceAction
            // 
            this.CreateCuttingMachineReportChoiceAction.Caption = "Rapor Oluştur";
            this.CreateCuttingMachineReportChoiceAction.ConfirmationMessage = null;
            this.CreateCuttingMachineReportChoiceAction.Id = "CreateCuttingMachineReportChoiceAction";
            this.CreateCuttingMachineReportChoiceAction.ImageName = "BO_Report";
            this.CreateCuttingMachineReportChoiceAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.CreateCuttingMachineReportChoiceAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.CreateCuttingMachineReportChoiceAction.ShowItemsOnClick = true;
            this.CreateCuttingMachineReportChoiceAction.ToolTip = null;
            this.CreateCuttingMachineReportChoiceAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CreateCuttingMachineReportChoiceAction_Execute);
            // 
            // CuttingMachineGroupReportViewController
            // 
            this.Actions.Add(this.CuttingMachineGroupDynamicReportAction);
            this.Actions.Add(this.CreateCuttingMachineReportChoiceAction);
            this.TargetViewId = "CuttingMachineGroupReport_DashboardView";

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CuttingMachineGroupDynamicReportAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CreateCuttingMachineReportChoiceAction;
    }
}
