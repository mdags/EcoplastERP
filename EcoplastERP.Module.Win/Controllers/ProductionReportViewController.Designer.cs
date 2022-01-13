namespace EcoplastERP.Module.Win.Controllers
{
    partial class ProductionReportViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.ProductionReportSelectTypeAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ExportProductionReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProductionReportSelectTypeAction
            // 
            this.ProductionReportSelectTypeAction.Caption = "Dinamik Raporlar";
            this.ProductionReportSelectTypeAction.ConfirmationMessage = null;
            this.ProductionReportSelectTypeAction.Id = "ProductionReportSelectTypeAction";
            this.ProductionReportSelectTypeAction.ImageName = "BO_State";
            choiceActionItem1.Caption = "Firma Bazında";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Firma Sipariş Bazında";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Stok Bazında";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "Firma Sipariş Stok Bazında";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "Vardiya Operatör Bazında";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "Ürün Cinsi Bazında";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "Firma Stok Bazında Günlük";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            choiceActionItem8.Caption = "İstasyon Makine Bazında";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            choiceActionItem9.Caption = "Sonraki İstasyon Bazında";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem1);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem2);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem3);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem4);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem5);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem6);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem7);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem8);
            this.ProductionReportSelectTypeAction.Items.Add(choiceActionItem9);
            this.ProductionReportSelectTypeAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            this.ProductionReportSelectTypeAction.ShowItemsOnClick = true;
            this.ProductionReportSelectTypeAction.ToolTip = null;
            this.ProductionReportSelectTypeAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ProductionReportSelectTypeAction_Execute);
            // 
            // ExportProductionReportAction
            // 
            this.ExportProductionReportAction.Caption = "Dışarı Aktar";
            this.ExportProductionReportAction.ConfirmationMessage = null;
            this.ExportProductionReportAction.Id = "ExportProductionReportAction";
            this.ExportProductionReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportProductionReportAction.ToolTip = null;
            this.ExportProductionReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportProductionReportAction_Execute);
            // 
            // ProductionReportViewController
            // 
            this.Actions.Add(this.ProductionReportSelectTypeAction);
            this.Actions.Add(this.ExportProductionReportAction);
            this.TargetViewId = "ProductionReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction ProductionReportSelectTypeAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportProductionReportAction;
    }
}
