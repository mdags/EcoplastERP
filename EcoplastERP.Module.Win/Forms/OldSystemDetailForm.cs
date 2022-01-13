using System;
using System.Data;
using DevExpress.Utils;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class OldSystemDetailForm : XtraForm
    {
        public string paletteNumber;

        public OldSystemDetailForm()
        {
            InitializeComponent();
        }

        private void OldSystemDetailForm_Load(object sender, EventArgs e)
        {
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionStringOldERP, CommandType.Text, @"SELECT PALETTENO as [Palet No], BARCODE AS [Barkod], AMOUNT AS [Miktar], UNIT as [Birim] FROM STORES WHERE PALETTENO = @paletteNumber", new SqlParameter("@paletteNumber", paletteNumber)).Tables[0];
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();
            if (gridView1.Columns.Count > 0)
            {
                gridView1.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Palet No"].Summary.Count == 0) gridView1.Columns["Palet No"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Palet No", "{0:n0}");
                if (gridView1.Columns["Miktar"].Summary.Count == 0) gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
            }
        }

        private void OldSystemDetailForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Escape) this.Close();
        }
    }
}
