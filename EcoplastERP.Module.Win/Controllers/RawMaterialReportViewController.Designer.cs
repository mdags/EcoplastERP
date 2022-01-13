namespace EcoplastERP.Module.Win.Controllers
{
    partial class RawMaterialReportViewController
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
            this.ExportRawMaterialReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RawMaterialReportSelectTypeAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ExportRawMaterialReportAction
            // 
            this.ExportRawMaterialReportAction.Caption = "Dışarı Aktar";
            this.ExportRawMaterialReportAction.ConfirmationMessage = null;
            this.ExportRawMaterialReportAction.Id = "ExportRawMaterialReportAction";
            this.ExportRawMaterialReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportRawMaterialReportAction.ToolTip = null;
            this.ExportRawMaterialReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportRawMaterialReportAction_Execute);
            // 
            // RawMaterialReportSelectTypeAction
            // 
            this.RawMaterialReportSelectTypeAction.Caption = "Dinamik Raporlar";
            this.RawMaterialReportSelectTypeAction.ConfirmationMessage = null;
            this.RawMaterialReportSelectTypeAction.Id = "RawMaterialReportSelectTypeAction";
            this.RawMaterialReportSelectTypeAction.ImageName = "BO_State";
            choiceActionItem1.Caption = "Reçete Bazında";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Çıkış Hareketi Bazında";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            this.RawMaterialReportSelectTypeAction.Items.Add(choiceActionItem1);
            this.RawMaterialReportSelectTypeAction.Items.Add(choiceActionItem2);
            this.RawMaterialReportSelectTypeAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            this.RawMaterialReportSelectTypeAction.ShowItemsOnClick = true;
            this.RawMaterialReportSelectTypeAction.ToolTip = null;
            this.RawMaterialReportSelectTypeAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.RawMaterialReportSelectTypeAction_Execute);
            // 
            // RawMaterialReportViewController
            // 
            this.Actions.Add(this.ExportRawMaterialReportAction);
            this.Actions.Add(this.RawMaterialReportSelectTypeAction);
            this.TargetViewId = "RawMaterialReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportRawMaterialReportAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction RawMaterialReportSelectTypeAction;
    }
}
