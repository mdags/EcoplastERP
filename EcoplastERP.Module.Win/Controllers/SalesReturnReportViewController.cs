using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.Win.UserForms;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class SalesReturnReportViewController : ViewController
    {
        public SalesReturnReportViewController()
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
                SalesReturnReportUserControl userControl = cvi.Control as SalesReturnReportUserControl;
                userControl.RefreshGrid();
            }
        }

        private void ExportSalesReturnReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                SalesOrderReportUserControl userControl = cvi.Control as SalesOrderReportUserControl;
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
    }
}
