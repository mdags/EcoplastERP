namespace EcoplastERP.Module.Win.Controllers
{
    partial class WastageReportViewController
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
            this.WastageReportSelectTypeAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ExportWastageReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // WastageReportSelectTypeAction
            // 
            this.WastageReportSelectTypeAction.Caption = "Dinamik Raporlar";
            this.WastageReportSelectTypeAction.ConfirmationMessage = null;
            this.WastageReportSelectTypeAction.Id = "WastageReportSelectTypeAction";
            this.WastageReportSelectTypeAction.ImageName = "BO_State";
            choiceActionItem1.Caption = "Firma Bazında";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Firma Sipariş Bazında";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Fire Kodu Bazında";
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
            choiceActionItem7.Caption = "Fire Nedeni Bazında";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            choiceActionItem8.Caption = "İstasyon Makine Bazında";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem1);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem2);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem3);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem4);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem5);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem6);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem7);
            this.WastageReportSelectTypeAction.Items.Add(choiceActionItem8);
            this.WastageReportSelectTypeAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            this.WastageReportSelectTypeAction.ShowItemsOnClick = true;
            this.WastageReportSelectTypeAction.ToolTip = null;
            this.WastageReportSelectTypeAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.WastageReportSelectTypeAction_Execute);
            // 
            // ExportWastageReportAction
            // 
            this.ExportWastageReportAction.Caption = "Dışarı Aktar";
            this.ExportWastageReportAction.ConfirmationMessage = null;
            this.ExportWastageReportAction.Id = "ExportWastageReportAction";
            this.ExportWastageReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportWastageReportAction.ToolTip = null;
            this.ExportWastageReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportWastageReportAction_Execute);
            // 
            // WastageReportViewController
            // 
            this.Actions.Add(this.WastageReportSelectTypeAction);
            this.Actions.Add(this.ExportWastageReportAction);
            this.TargetViewId = "WastageReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction WastageReportSelectTypeAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportWastageReportAction;
    }
}
