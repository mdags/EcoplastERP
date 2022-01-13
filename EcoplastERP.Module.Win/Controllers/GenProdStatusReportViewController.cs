using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.Win.UserForms;
using EcoplastERP.Module.BusinessObjects.ReportObjects;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class GenProdStatusReportViewController : ViewController
    {
        public GenProdStatusReportViewController()
        {
            InitializeComponent();
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
                GenProdStatusReportUserControl userControl = cvi.Control as GenProdStatusReportUserControl;
                userControl.SelectReportType();
                userControl.RefreshGrid();
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

        private void ExportGenProdStatusReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                GenProdStatusReportUserControl userControl = cvi.Control as GenProdStatusReportUserControl;
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

        private void GenProdStatusReportSetReportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                GenProdStatusReportUserControl userControl = cvi.Control as GenProdStatusReportUserControl;
                if (e.SelectedChoiceActionItem.Id == "Tümü") userControl.reportID = 1;
                if (e.SelectedChoiceActionItem.Id == "Satış Siparişleri") userControl.reportID = 2;
                if (e.SelectedChoiceActionItem.Id == "Planlama Siparişleri") userControl.reportID = 3;
                userControl.SelectReportType();
            }
        }

        private void CreateGenProdStatusReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                GenProdStatusReportUserControl userControl = cvi.Control as GenProdStatusReportUserControl;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"delete GeneralProductStatusReport");

                for (int i = 0; i < userControl.gridView1.RowCount; i++)
                {
                    GeneralProductStatusReport userReport = objectSpace.CreateObject<GeneralProductStatusReport>();
                    userReport.ProductGroup = userControl.gridView1.GetRowCellValue(i, "Ürün Grubu").ToString();
                    userReport.ProductType = userControl.gridView1.GetRowCellValue(i, "Ürün Tipi").ToString();
                    userReport.ProductKind = userControl.gridView1.GetRowCellValue(i, "Ürün Cinsi").ToString();
                    userReport.ProductKindGroup = userControl.gridView1.GetRowCellValue(i, "Satış Ürün Grubu").ToString();
                    userReport.SalesOrdercQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Sipariş TÖB Miktarı"));
                    userReport.ShippedcQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Sevk Edilen TÖB Miktar"));
                    userReport.WaitingcQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bekleyen TÖB Miktar"));
                    userReport.StorecQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Depo TÖB Miktar"));
                    userReport.PrintingQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Baskı Depo"));
                    userReport.CuttingQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kesim Depo"));
                    userReport.SlicingQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Dilme Depo"));
                    userReport.LaminationQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Laminasyon Depo"));
                    userReport.CastQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Cast Depo"));
                    userReport.FilmingProductionQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Çekim Üretilecek Miktar"));
                }

                objectSpace.CommitChanges();

                IObjectSpace reportObjectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                IReportDataV2 reportData = reportObjectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("[DisplayName] = 'Genel Ürün Durum Raporu'"));
                if (reportData != null)
                {
                    //string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                    //Frame.GetController<ReportServiceController>().ShowPreview(handle);
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = "Pdf Dosyası (.pdf)|*.pdf";
                        if (saveDialog.ShowDialog() != DialogResult.Cancel)
                        {
                            string exportFilePath = saveDialog.FileName;
                            var report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
                            ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(Application.Modules);
                            if (reportsModule != null & reportsModule.ReportsDataSourceHelper != null)
                            {
                                reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report);
                                report.ExportToPdf(exportFilePath);

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
            }
        }
    }
}
