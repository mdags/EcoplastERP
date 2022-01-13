using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.Xpo;
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
    public partial class CuttingMachineGroupReportViewController : ViewController
    {
        public CuttingMachineGroupReportViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            Frame.GetController<RefreshController>().RefreshAction.Execute += RefreshAction_Execute;

            CreateCuttingMachineReportChoiceAction.Items.Clear();
            FillItemWithEnumValues(typeof(CuttingMachineGroupReport).FullName);
        }

        private void FillItemWithEnumValues(string objectTypeName)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            XPCollection<ReportDataV2> reportDataList = new XPCollection<ReportDataV2>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("[DataTypeName] = ?", objectTypeName));
            foreach (ReportDataV2 report in reportDataList)
            {
                ChoiceActionItem item = new ChoiceActionItem(report.DisplayName, report);
                CreateCuttingMachineReportChoiceAction.Items.Add(item);
            }
        }

        private void RefreshAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                CuttingMachineGroupReportUserControl userControl = cvi.Control as CuttingMachineGroupReportUserControl;
                userControl.SelectReportType();
                userControl.RefreshGrid();
            }
        }

        private void ExportCuttingMachinegroupReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                CuttingMachineGroupReportUserControl userControl = cvi.Control as CuttingMachineGroupReportUserControl;
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

        private void CreateCuttingMachineGroupReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                CuttingMachineGroupReportUserControl userControl = cvi.Control as CuttingMachineGroupReportUserControl;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"delete CuttingMachineGroupReport");

                for (int i = 0; i < userControl.gridView1.RowCount; i++)
                {
                    CuttingMachineGroupReport userReport = objectSpace.CreateObject<CuttingMachineGroupReport>();
                    userReport.ProductGroup = userControl.reportID == 1 ? "Tümü" : userControl.reportID == 2 ? "Satış Siparişleri" : "Planlama Siparişleri";
                    userReport.CuttingMachineGroup = userControl.gridView1.GetRowCellValue(i, "Kesim Makine Grubu").ToString();
                    userReport.ProduceQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Üretilecek TÖB Miktar"));
                    userReport.Capacity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kapasite (gün)"));
                    userReport.CapacityDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Toplam Dolu Gün"));
                    userReport.StoreQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kesim Depo"));
                    userReport.StoreDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kapasite (gün)")) > 0 ? Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kesim Depo")) / Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kapasite (gün)")) : 0;
                    userReport.PastProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Geçmiş Üretilecek"));
                    userReport.PastProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Geçmiş Gün Doluluk"));
                    userReport.ThisMonthProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bu Ay Üretilecek"));
                    userReport.ThisMonthProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bu Ay Doluluk"));
                    userReport.NextMonthProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bir Sonraki Ay Üretilecek"));
                    userReport.NextMontProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bir Sonraki Ay Doluluk"));
                    userReport.NextProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "İleri Termin Üretilecek"));
                    userReport.NextProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "İleri Termin Doluluk"));
                }

                objectSpace.CommitChanges();

                IObjectSpace reportObjectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                IReportDataV2 reportData = reportObjectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("[DisplayName] = 'Kesim Makine Grubu Raporu'"));
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

        private void CuttingMachineGroupDynamicReportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                CuttingMachineGroupReportUserControl userControl = cvi.Control as CuttingMachineGroupReportUserControl;
                if (e.SelectedChoiceActionItem.Id == "Tümü") userControl.reportID = 1;
                if (e.SelectedChoiceActionItem.Id == "Satış Siparişleri") userControl.reportID = 2;
                if (e.SelectedChoiceActionItem.Id == "Planlama Siparişleri") userControl.reportID = 3;
                userControl.SelectReportType();
            }
        }

        private void CreateCuttingMachineReportChoiceAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                CuttingMachineGroupReportUserControl userControl = cvi.Control as CuttingMachineGroupReportUserControl;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"delete CuttingMachineGroupReport");

                for (int i = 0; i < userControl.gridView1.RowCount; i++)
                {
                    CuttingMachineGroupReport userReport = objectSpace.CreateObject<CuttingMachineGroupReport>();
                    userReport.ProductGroup = userControl.reportID == 1 ? "Tümü" : userControl.reportID == 2 ? "Satış Siparişleri" : "Planlama Siparişleri";
                    userReport.CuttingMachineGroup = userControl.gridView1.GetRowCellValue(i, "Kesim Makine Grubu").ToString();
                    userReport.ProduceQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Üretilecek TÖB Miktar"));
                    userReport.Capacity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kapasite (gün)"));
                    userReport.CapacityDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Toplam Dolu Gün"));
                    userReport.StoreQuantity = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kesim Depo"));
                    userReport.StoreDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kapasite (gün)")) > 0 ? Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kesim Depo")) / Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Kapasite (gün)")) : 0;
                    userReport.PastProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Geçmiş Üretilecek"));
                    userReport.PastProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Geçmiş Gün Doluluk"));
                    userReport.ThisMonthProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bu Ay Üretilecek"));
                    userReport.ThisMonthProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bu Ay Doluluk"));
                    userReport.NextMonthProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bir Sonraki Ay Üretilecek"));
                    userReport.NextMontProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "Bir Sonraki Ay Doluluk"));
                    userReport.NextProduction = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "İleri Termin Üretilecek"));
                    userReport.NextProductionDay = Convert.ToDecimal(userControl.gridView1.GetRowCellValue(i, "İleri Termin Doluluk"));
                }

                objectSpace.CommitChanges();

                //IObjectSpace reportObjectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                ReportDataV2 reportData = (ReportDataV2)e.SelectedChoiceActionItem.Data;
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