using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.QualityObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class QualityAnalysisImportForm : XtraForm
    {
        public XafApplication winApplication;

        public QualityAnalysisImportForm()
        {
            InitializeComponent();
        }

        private void QualityAnalysisImportForm_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select Oid, Code as [Kod] from Reciept where GCRecord is null", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            data.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            sleReciept.Properties.DataSource = data;
            sleReciept.Properties.DisplayMember = "Kod";
            sleReciept.Properties.ValueMember = "Oid";
            sleReciept.ForceInitialize();

            gridView1.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.False;
        }

        private void QualityAnalysisImportForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel 97-2003 Files|*.xls;", Multiselect = false, Title = "Excel Dosyası Seçiniz..." };
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                txtFilePath.Text = openFileDialog.FileName;

                gridControl1.DataSource = null;
                gridControl1.DataSource = ExcelDataBaseHelper.OpenFile(openFileDialog.FileName);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (sleReciept.EditValue != null)
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                Reciept reciept = objectSpace.FindObject<Reciept>(new BinaryOperator("Oid", sleReciept.EditValue));
                if (reciept != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    AnalysisCertificate analysiscertificate = null;
                    AnalysisCertificate existsAnalysiscertificate = objectSpace.FindObject<AnalysisCertificate>(new BinaryOperator("Reciept.Oid", reciept));
                    if (existsAnalysiscertificate != null)
                    {
                        analysiscertificate = existsAnalysiscertificate;
                        if (grpRecordType.SelectedIndex == 1)
                        {
                            objectSpace.Delete(existsAnalysiscertificate.AnalysisCertificateDetails);
                        }
                    }
                    else
                    {
                        AnalysisCertificate newAnalysisCertificate = objectSpace.CreateObject<AnalysisCertificate>();
                        newAnalysisCertificate.Reciept = reciept;
                        analysiscertificate = newAnalysisCertificate;
                    }

                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        if (!string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[2].FieldName).ToString()))
                        {
                            QuantitativeAttribute QuantitativeAttribute = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[3].FieldName).ToString()) ? objectSpace.FindObject<QuantitativeAttribute>(CriteriaOperator.Parse("Station = ? and Name = ?", reciept.Station, gridView1.GetRowCellValue(i, gridView1.Columns[2].FieldName).ToString())) : objectSpace.FindObject<QuantitativeAttribute>(CriteriaOperator.Parse("Station = ? and Name = ? and TestDirection = ?", reciept.Station, gridView1.GetRowCellValue(i, gridView1.Columns[2].FieldName).ToString(), gridView1.GetRowCellValue(i, gridView1.Columns[3].FieldName).ToString()));

                            if (QuantitativeAttribute != null)
                            {
                                AnalysisCertificateDetail detail = objectSpace.CreateObject<AnalysisCertificateDetail>();
                                detail.AnalysisCertificate = analysiscertificate;
                                detail.MinimumThickness = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[0].FieldName).ToString()) ? 0 : Convert.ToInt32(gridView1.GetRowCellValue(i, gridView1.Columns[0].FieldName));
                                detail.MaximumThickness = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[1].FieldName).ToString()) ? 0 : Convert.ToInt32(gridView1.GetRowCellValue(i, gridView1.Columns[1].FieldName));
                                detail.QuantitativeAttribute = QuantitativeAttribute;
                                detail.Value = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[4].FieldName).ToString()) ? 0 : Convert.ToDecimal(gridView1.GetRowCellValue(i, gridView1.Columns[4].FieldName));
                                detail.ContactToleranceMin = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[5].FieldName).ToString()) ? 0 : Convert.ToDecimal(gridView1.GetRowCellValue(i, gridView1.Columns[5].FieldName));
                                detail.ContactToleranceMax = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[6].FieldName).ToString()) ? 0 : Convert.ToDecimal(gridView1.GetRowCellValue(i, gridView1.Columns[6].FieldName));
                                detail.ProductionToleranceMin = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[7].FieldName).ToString()) ? 0 : Convert.ToDecimal(gridView1.GetRowCellValue(i, gridView1.Columns[7].FieldName));
                                detail.ProductionToleranceMax = string.IsNullOrEmpty(gridView1.GetRowCellValue(i, gridView1.Columns[8].FieldName).ToString()) ? 0 : Convert.ToDecimal(gridView1.GetRowCellValue(i, gridView1.Columns[8].FieldName));
                                analysiscertificate.AnalysisCertificateDetails.Add(detail);
                            }
                        }
                    }


                    objectSpace.CommitChanges();
                    Cursor.Current = Cursors.Default;
                    XtraMessageBox.Show("İşlem tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else XtraMessageBox.Show("Reçete(Film Kodu) bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else XtraMessageBox.Show("Reçete(Film Kodu) seçimini yapınız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void sleReciept_EditValueChanged(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            AnalysisCertificate existsAnalysiscertificate = objectSpace.FindObject<AnalysisCertificate>(new BinaryOperator("Reciept.Oid", sleReciept.EditValue));
            if (existsAnalysiscertificate != null)
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                grpRecordType.SelectedIndex = 0;
            }
        }
    }
}