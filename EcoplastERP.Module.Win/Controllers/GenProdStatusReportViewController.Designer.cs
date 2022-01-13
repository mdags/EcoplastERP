namespace EcoplastERP.Module.Win.Controllers
{
    partial class GenProdStatusReportViewController
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
            this.ExportGenProdStatusReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GenProdStatusReportSetReportAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CreateGenProdStatusReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportGenProdStatusReportAction
            // 
            this.ExportGenProdStatusReportAction.Caption = "Dışarı Aktar";
            this.ExportGenProdStatusReportAction.ConfirmationMessage = null;
            this.ExportGenProdStatusReportAction.Id = "ExportGenProdStatusReportAction";
            this.ExportGenProdStatusReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportGenProdStatusReportAction.ToolTip = null;
            this.ExportGenProdStatusReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportGenProdStatusReportAction_Execute);
            // 
            // GenProdStatusReportSetReportAction
            // 
            this.GenProdStatusReportSetReportAction.Caption = "Dinamik Raporlar";
            this.GenProdStatusReportSetReportAction.ConfirmationMessage = null;
            this.GenProdStatusReportSetReportAction.Id = "GenProdStatusReportSetReportAction";
            this.GenProdStatusReportSetReportAction.ImageName = "Action_Report_Object_Inplace_Preview";
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
            this.GenProdStatusReportSetReportAction.Items.Add(choiceActionItem1);
            this.GenProdStatusReportSetReportAction.Items.Add(choiceActionItem2);
            this.GenProdStatusReportSetReportAction.Items.Add(choiceActionItem3);
            this.GenProdStatusReportSetReportAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            this.GenProdStatusReportSetReportAction.ShowItemsOnClick = true;
            this.GenProdStatusReportSetReportAction.ToolTip = null;
            this.GenProdStatusReportSetReportAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.GenProdStatusReportSetReportAction_Execute);
            // 
            // CreateGenProdStatusReportAction
            // 
            this.CreateGenProdStatusReportAction.Caption = "Rapor Oluştur";
            this.CreateGenProdStatusReportAction.ConfirmationMessage = null;
            this.CreateGenProdStatusReportAction.Id = "CreateGenProdStatusReportAction";
            this.CreateGenProdStatusReportAction.ImageName = "BO_Report";
            this.CreateGenProdStatusReportAction.ToolTip = null;
            this.CreateGenProdStatusReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateGenProdStatusReportAction_Execute);
            // 
            // GenProdStatusReportViewController
            // 
            this.Actions.Add(this.ExportGenProdStatusReportAction);
            this.Actions.Add(this.GenProdStatusReportSetReportAction);
            this.Actions.Add(this.CreateGenProdStatusReportAction);
            this.TargetViewId = "GenProdStatusReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportGenProdStatusReportAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction GenProdStatusReportSetReportAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateGenProdStatusReportAction;
    }
}
