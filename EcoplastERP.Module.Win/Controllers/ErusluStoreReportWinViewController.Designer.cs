namespace EcoplastERP.Module.Win.Controllers
{
    partial class ErusluStoreReportWinViewController
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
            this.ExportErusluStoreReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ErusluStoreReportSelectTypeAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ExportErusluStoreReportAction
            // 
            this.ExportErusluStoreReportAction.Caption = "Dışarı Aktar";
            this.ExportErusluStoreReportAction.ConfirmationMessage = null;
            this.ExportErusluStoreReportAction.Id = "ExportErusluStoreReportAction";
            this.ExportErusluStoreReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportErusluStoreReportAction.ToolTip = null;
            this.ExportErusluStoreReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportErusluStoreReportAction_Execute);
            // 
            // ErusluStoreReportSelectTypeAction
            // 
            this.ErusluStoreReportSelectTypeAction.Caption = "Dinamik Raporlar";
            this.ErusluStoreReportSelectTypeAction.ConfirmationMessage = null;
            this.ErusluStoreReportSelectTypeAction.Id = "ErusluStoreReportSelectTypeAction";
            this.ErusluStoreReportSelectTypeAction.ImageName = "BO_State";
            choiceActionItem1.Caption = "Sipariş Kalemi Bazında";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Palet Bazında";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Barkod Bazında";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            this.ErusluStoreReportSelectTypeAction.Items.Add(choiceActionItem1);
            this.ErusluStoreReportSelectTypeAction.Items.Add(choiceActionItem2);
            this.ErusluStoreReportSelectTypeAction.Items.Add(choiceActionItem3);
            this.ErusluStoreReportSelectTypeAction.ShowItemsOnClick = true;
            this.ErusluStoreReportSelectTypeAction.ToolTip = null;
            this.ErusluStoreReportSelectTypeAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ErusluStoreReportSelectTypeAction_Execute);
            // 
            // ErusluStoreReportWinViewController
            // 
            this.Actions.Add(this.ExportErusluStoreReportAction);
            this.Actions.Add(this.ErusluStoreReportSelectTypeAction);
            this.TargetViewId = "ErusluStoreReport_DashboardView";

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ExportErusluStoreReportAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ErusluStoreReportSelectTypeAction;
    }
}
