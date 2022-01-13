using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using DevExpress.Persistent.AuditTrail;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class WasteStickerForm : XtraForm
    {
        public XafApplication winApplication;

        public WasteStickerForm()
        {
            InitializeComponent();
        }

        private void WasteStickerForm_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select Oid, Code as [Adı] from Station where GCRecord is null and IsLastStation = 0 order by Code", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Station.Properties.DataSource = dt;
            Station.Properties.DisplayMember = "Adı";
            Station.Properties.ValueMember = "Oid";
            Station.ForceInitialize();

            dt = new DataTable();
            da = new SqlDataAdapter(@"select Oid, Name as [Adı] from WasteCode where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            WasteCode.Properties.DataSource = dt;
            WasteCode.Properties.DisplayMember = "Adı";
            WasteCode.Properties.ValueMember = "Oid";
            WasteCode.ForceInitialize();
        }

        private void WasteStickerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        void RefreshGrid()
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(string.Format(@"select top 100 W.Oid, W.WasteDate as [Atık Tarihi], W.Barcode as [Barkod], S.Name as [İstasyon], C.Name as [Atık Kodu], W.Quantity as [Miktar] from Waste W inner join Station S on S.Oid = W.Station inner join WasteCode C on C.Oid = W.WasteCode where W.GCRecord is null and W.Station = '{0}' order by W.WasteDate desc", Station.EditValue), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            if (gridView1.Columns.Count > 0)
            {
                gridView1.Columns["Atık Tarihi"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView1.Columns["Atık Tarihi"].DisplayFormat.FormatString = "g";
                gridView1.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Miktar"].Summary.Count == 0) gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
            }
        }

        private void Station_EditValueChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Station.EditValue != null)
            {
                if (WasteCode.EditValue != null)
                {
                    if (!string.IsNullOrEmpty(txtQuantity.Text))
                    {
                        if (Convert.ToDecimal(txtQuantity.Text) > 0)
                        {
                            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                            Waste waste = objectSpace.CreateObject<Waste>();
                            waste.Barcode = Helpers.GetDocumentNumber(((XPObjectSpace)objectSpace).Session, "EcoplastERP.Module.BusinessObjects.ProductionObjects.Waste");
                            waste.Station = objectSpace.FindObject<Station>(new BinaryOperator("Oid", Guid.Parse(Station.EditValue.ToString())));
                            waste.WasteCode = objectSpace.FindObject<WasteCode>(new BinaryOperator("Oid", Guid.Parse(WasteCode.EditValue.ToString())));
                            waste.Quantity = Convert.ToDecimal(txtQuantity.Text);

                            objectSpace.CommitChanges();

                            ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                            ReportDataV2 sticker = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Atık Çıktısı"));
                            if (sticker != null)
                            {
                                XtraReport stickerReport = ReportDataProvider.ReportsStorage.LoadReport(sticker);
                                stickerReport.DataAdapter = null;
                                XPCollection<Waste> productionList = new XPCollection<Waste>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Barcode = ?", waste.Barcode));
                                stickerReport.DataSource = productionList;
                                stickerReport.CreateDocument(false);
                                reportsModule.ReportsDataSourceHelper.SetupBeforePrint(stickerReport);
                                stickerReport.Print("İŞLETME");
                            }

                            txtQuantity.Text = "0,00";
                            RefreshGrid();
                        }
                        else throw new UserFriendlyException("Miktar giriniz.");
                    }
                    else throw new UserFriendlyException("Miktar giriniz.");
                }
                else throw new UserFriendlyException("Atık Kodu seçiniz.");
            }
            else throw new UserFriendlyException("İstasyon seçiniz.");
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Waste waste = objectSpace.FindObject<Waste>(new BinaryOperator("Barcode", gridView1.GetFocusedRowCellValue("Barkod").ToString()));
            if (waste != null)
            {
                ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                ReportDataV2 sticker = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Atık Çıktısı"));
                if (sticker != null)
                {
                    XtraReport stickerReport = ReportDataProvider.ReportsStorage.LoadReport(sticker);
                    stickerReport.DataAdapter = null;
                    XPCollection<Waste> productionList = new XPCollection<Waste>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Barcode = ?", waste.Barcode));
                    stickerReport.DataSource = productionList;
                    stickerReport.CreateDocument(false);
                    reportsModule.ReportsDataSourceHelper.SetupBeforePrint(stickerReport);
                    stickerReport.Print("İŞLETME");
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.GetFocusedRow() == null) return;
            if (XtraMessageBox.Show("Kaydı silmek istiyor musunuz?", "Onaylana", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                Waste waste = objectSpace.FindObject<Waste>(new BinaryOperator("Oid", Guid.Parse(gridView1.GetFocusedRowCellValue("Oid").ToString())));
                if (waste != null)
                {
                    waste.Delete();
                    objectSpace.CommitChanges();
                    RefreshGrid();
                }
            }
        }
    }
}
