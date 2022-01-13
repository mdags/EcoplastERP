using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.Win.UserForms;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class StoreReportViewController : ViewController
    {
        public StoreReportViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            Frame.GetController<RefreshController>().RefreshAction.Execute += RefreshAction_Execute;
        }

        private void RefreshAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                userControl.RefreshGrid();

                if (userControl.reportID == 1)
                {
                    this.StoreSaleOrderTransferAction.Enabled["StoreSaleOrderTransferAction"] = false;
                    this.StorePaletteTransferAction.Enabled["StorePaletteTransferAction"] = false;
                    this.StoreWarehouseTransferAction.Enabled["StoreWarehouseTransferAction"] = false;
                    this.StoreUpdateOthersAction.Enabled["StoreUpdateOthersAction"] = false;
                    this.ConsumeBarcodeAction.Enabled["ConsumeBarcodeAction"] = false;
                }
                if (userControl.reportID == 2)
                {
                    this.StoreSaleOrderTransferAction.Enabled["StoreSaleOrderTransferAction"] = true;
                    this.StorePaletteTransferAction.Enabled["StorePaletteTransferAction"] = true;
                    this.StoreWarehouseTransferAction.Enabled["StoreWarehouseTransferAction"] = true;
                    this.StoreUpdateOthersAction.Enabled["StoreUpdateOthersAction"] = false;
                    this.ConsumeBarcodeAction.Enabled["ConsumeBarcodeAction"] = false;
                }
                if (userControl.reportID == 3)
                {
                    this.StoreSaleOrderTransferAction.Enabled["StoreSaleOrderTransferAction"] = true;
                    this.StorePaletteTransferAction.Enabled["StorePaletteTransferAction"] = true;
                    this.StoreWarehouseTransferAction.Enabled["StoreWarehouseTransferAction"] = true;
                    this.StoreUpdateOthersAction.Enabled["StoreUpdateOthersAction"] = true;
                    this.ConsumeBarcodeAction.Enabled["ConsumeBarcodeAction"] = true;
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void StoreReportSelectTypeAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                if (e.SelectedChoiceActionItem.Id == "Sipariş Kalemi Bazında") userControl.reportID = 1;
                if (e.SelectedChoiceActionItem.Id == "Palet Bazında") userControl.reportID = 2;
                if (e.SelectedChoiceActionItem.Id == "Barkod Bazında") userControl.reportID = 3;
                userControl.SelectReportType();
            }
        }

        private void StoreSaleOrderTransferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            const string salesOrderDetail_ListView = "SalesOrderDetail_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(SalesOrderDetail), salesOrderDetail_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(salesOrderDetail_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController salesOrderDetailDialogController = Application.CreateController<DialogController>();
            salesOrderDetailDialogController.Accepting += SalesOrderDetailDialogController_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(salesOrderDetailDialogController);
        }

        private void SalesOrderDetailDialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                SalesOrderDetail salesOrderDetail = (SalesOrderDetail)objectSpace.GetObject(e.AcceptActionArgs.SelectedObjects[0]);
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                userControl.SalesOrderTransfer(salesOrderDetail);
            }
        }

        private void StorePaletteTransferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            const string palette_ListView = "ProductionPalette_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(ProductionPalette), palette_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(palette_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController paletteDialogController = Application.CreateController<DialogController>();
            paletteDialogController.Accepting += PaletteDialogController_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(paletteDialogController);
        }

        private void PaletteDialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ProductionPalette productionPalette = (ProductionPalette)objectSpace.GetObject(e.AcceptActionArgs.SelectedObjects[0]);
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                userControl.PaletteTransfer(productionPalette.PaletteNumber);
            }
        }

        private void StoreWarehouseTransferAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            const string warehouse_ListView = "Warehouse_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Warehouse), warehouse_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(warehouse_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController warehouseDialogController = Application.CreateController<DialogController>();
            warehouseDialogController.Accepting += WarehouseDialogController_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(warehouseDialogController);
        }

        private void WarehouseDialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Warehouse warehouse = (Warehouse)objectSpace.GetObject(e.AcceptActionArgs.SelectedObjects[0]);
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                userControl.WarehouseTransfer(warehouse);
            }
        }

        private void EportStoreReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |Zengin Metin Dosyası (.rtf)|*.rtf |Pdf Dosyası (.pdf)|*.pdf |Html Dosya (.html)|*.html";
                    if (saveDialog.ShowDialog() != DialogResult.Cancel)
                    {
                        string exportFilePath = saveDialog.FileName;
                        string fileExtenstion = new FileInfo(exportFilePath).Extension;

                        switch (fileExtenstion)
                        {
                            case ".xls":
                                userControl.gridControl1.ExportToXls(exportFilePath);
                                break;
                            case ".xlsx":
                                userControl.gridControl1.ExportToXlsx(exportFilePath);
                                break;
                            case ".rtf":
                                userControl.gridControl1.ExportToRtf(exportFilePath);
                                break;
                            case ".pdf":
                                userControl.gridControl1.ExportToPdf(exportFilePath);
                                break;
                            case ".html":
                                userControl.gridControl1.ExportToHtml(exportFilePath);
                                break;
                            case ".mht":
                                userControl.gridControl1.ExportToMht(exportFilePath);
                                break;
                            default:
                                break;
                        }

                        if (File.Exists(exportFilePath))
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(exportFilePath);
                            }
                            catch
                            {
                                string msg = string.Format("The file could not be opened.{0}{1}Path: {2}", Environment.NewLine, Environment.NewLine, exportFilePath);
                                MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            string msg = string.Format("The file could not be saved.{0}{1}Path: {2}", Environment.NewLine, Environment.NewLine, exportFilePath);
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void StoreUpdateOthersAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                userControl.UpdateOthers();
            }
        }

        private void ConsumeBarcodeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                string workOrderNumber = Interaction.InputBox("Üretim Siparişi No", "Üretim Siparişi No Giriniz.", "", 0, 0);
                if (!string.IsNullOrEmpty(workOrderNumber)) userControl.ConsumeBarcode(workOrderNumber);
            }
        }

        private void WarehouseExitAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                userControl.WarehouseExit();
            }
        }

        private void WarehouseEntryAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreReportUserControl userControl = cvi.Control as StoreReportUserControl;
                string barcode = Interaction.InputBox("Palet/Ürün Barkodu", "Palet/Ürün Barkodu No Giriniz.", "", 0, 0);
                if (!string.IsNullOrEmpty(barcode)) userControl.WarehouseEntry(barcode);
            }
        }
    }
}