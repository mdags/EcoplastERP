namespace EcoplastERP.Module.Win.Controllers
{
    partial class StoreReportViewController
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
            this.StoreReportSelectTypeAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.StoreSaleOrderTransferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.StorePaletteTransferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.StoreWarehouseTransferAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExportStoreReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.StoreUpdateOthersAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ConsumeBarcodeAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.WarehouseExitAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.WarehouseEntryAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // StoreReportSelectTypeAction
            // 
            this.StoreReportSelectTypeAction.Caption = "Dinamik Raporlar";
            this.StoreReportSelectTypeAction.ConfirmationMessage = null;
            this.StoreReportSelectTypeAction.Id = "StoreReportSelectTypeAction";
            this.StoreReportSelectTypeAction.ImageName = "BO_State";
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
            this.StoreReportSelectTypeAction.Items.Add(choiceActionItem1);
            this.StoreReportSelectTypeAction.Items.Add(choiceActionItem2);
            this.StoreReportSelectTypeAction.Items.Add(choiceActionItem3);
            this.StoreReportSelectTypeAction.ShowItemsOnClick = true;
            this.StoreReportSelectTypeAction.ToolTip = null;
            this.StoreReportSelectTypeAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.StoreReportSelectTypeAction_Execute);
            // 
            // StoreSaleOrderTransferAction
            // 
            this.StoreSaleOrderTransferAction.Caption = "Sipariş Transfer";
            this.StoreSaleOrderTransferAction.Category = "RecordEdit";
            this.StoreSaleOrderTransferAction.ConfirmationMessage = null;
            this.StoreSaleOrderTransferAction.Id = "StoreSaleOrderTransferAction";
            this.StoreSaleOrderTransferAction.ImageName = "BO_Order";
            this.StoreSaleOrderTransferAction.ToolTip = null;
            this.StoreSaleOrderTransferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.StoreSaleOrderTransferAction_Execute);
            // 
            // StorePaletteTransferAction
            // 
            this.StorePaletteTransferAction.Caption = "Palet Transfer";
            this.StorePaletteTransferAction.Category = "RecordEdit";
            this.StorePaletteTransferAction.ConfirmationMessage = null;
            this.StorePaletteTransferAction.Id = "StorePaletteTransferAction";
            this.StorePaletteTransferAction.ImageName = "BO_List";
            this.StorePaletteTransferAction.ToolTip = null;
            this.StorePaletteTransferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.StorePaletteTransferAction_Execute);
            // 
            // StoreWarehouseTransferAction
            // 
            this.StoreWarehouseTransferAction.Caption = "Depo Transfer";
            this.StoreWarehouseTransferAction.Category = "RecordEdit";
            this.StoreWarehouseTransferAction.ConfirmationMessage = null;
            this.StoreWarehouseTransferAction.Id = "StoreWarehouseTransferAction";
            this.StoreWarehouseTransferAction.ImageName = "BO_State";
            this.StoreWarehouseTransferAction.ToolTip = null;
            this.StoreWarehouseTransferAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.StoreWarehouseTransferAction_Execute);
            // 
            // ExportStoreReportAction
            // 
            this.ExportStoreReportAction.Caption = "Dışarı Aktar";
            this.ExportStoreReportAction.ConfirmationMessage = null;
            this.ExportStoreReportAction.Id = "ExportStoreReportAction";
            this.ExportStoreReportAction.ImageName = "Action_Export_ToExcel";
            this.ExportStoreReportAction.ToolTip = null;
            this.ExportStoreReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EportStoreReportAction_Execute);
            // 
            // StoreUpdateOthersAction
            // 
            this.StoreUpdateOthersAction.Caption = "Diğerlerine Uygula";
            this.StoreUpdateOthersAction.Category = "RecordEdit";
            this.StoreUpdateOthersAction.ConfirmationMessage = null;
            this.StoreUpdateOthersAction.Id = "StoreUpdateOthersAction";
            this.StoreUpdateOthersAction.ImageName = "Action_ModelDifferences_Import";
            this.StoreUpdateOthersAction.ToolTip = null;
            this.StoreUpdateOthersAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.StoreUpdateOthersAction_Execute);
            // 
            // ConsumeBarcodeAction
            // 
            this.ConsumeBarcodeAction.Caption = "Sarfiyat Çıkış";
            this.ConsumeBarcodeAction.Category = "RecordEdit";
            this.ConsumeBarcodeAction.ConfirmationMessage = null;
            this.ConsumeBarcodeAction.Id = "ConsumeBarcodeAction";
            this.ConsumeBarcodeAction.ImageName = "Action_ParametrizedAction";
            this.ConsumeBarcodeAction.ToolTip = null;
            this.ConsumeBarcodeAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ConsumeBarcodeAction_Execute);
            // 
            // WarehouseExitAction
            // 
            this.WarehouseExitAction.Caption = "Sayım Eksiği Çıkış";
            this.WarehouseExitAction.Category = "RecordEdit";
            this.WarehouseExitAction.ConfirmationMessage = null;
            this.WarehouseExitAction.Id = "WarehouseExitAction";
            this.WarehouseExitAction.ImageName = "Action_LinkUnlink_Unlink";
            this.WarehouseExitAction.ToolTip = null;
            this.WarehouseExitAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WarehouseExitAction_Execute);
            // 
            // WarehouseEntryAction
            // 
            this.WarehouseEntryAction.Caption = "Sayım Fazlası Giriş";
            this.WarehouseEntryAction.Category = "RecordEdit";
            this.WarehouseEntryAction.ConfirmationMessage = null;
            this.WarehouseEntryAction.Id = "WarehouseEntryAction";
            this.WarehouseEntryAction.ImageName = "Action_LinkUnlink_Link";
            this.WarehouseEntryAction.ToolTip = null;
            this.WarehouseEntryAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WarehouseEntryAction_Execute);
            // 
            // StoreReportViewController
            // 
            this.Actions.Add(this.StoreReportSelectTypeAction);
            this.Actions.Add(this.StoreSaleOrderTransferAction);
            this.Actions.Add(this.StorePaletteTransferAction);
            this.Actions.Add(this.StoreWarehouseTransferAction);
            this.Actions.Add(this.ExportStoreReportAction);
            this.Actions.Add(this.StoreUpdateOthersAction);
            this.Actions.Add(this.ConsumeBarcodeAction);
            this.Actions.Add(this.WarehouseExitAction);
            this.Actions.Add(this.WarehouseEntryAction);
            this.TargetViewId = "StoreReport_DashboardView";

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SingleChoiceAction StoreReportSelectTypeAction;
        private DevExpress.ExpressApp.Actions.SimpleAction StoreSaleOrderTransferAction;
        private DevExpress.ExpressApp.Actions.SimpleAction StorePaletteTransferAction;
        private DevExpress.ExpressApp.Actions.SimpleAction StoreWarehouseTransferAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportStoreReportAction;
        private DevExpress.ExpressApp.Actions.SimpleAction StoreUpdateOthersAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ConsumeBarcodeAction;
        private DevExpress.ExpressApp.Actions.SimpleAction WarehouseExitAction;
        private DevExpress.ExpressApp.Actions.SimpleAction WarehouseEntryAction;
    }
}
