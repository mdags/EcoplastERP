using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class FindPaletteUserControl : XtraUserControl, IComplexControl
    {
        public FindPaletteUserControl()
        {
            InitializeComponent();
        }

        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        void RefreshGrid(string paletteNumber)
        {
            var currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select LastWeight, Tare, GrossWeight, NetWeight from ProductionPalette where PaletteNumber = '{0}'", paletteNumber), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            if (data.Rows.Count > 0)
            {
                txtLastWeight.Text = string.Format("{0:n2}", data.Rows[0]["LastWeight"]);
                txtTare.Text = string.Format("{0:n2}", data.Rows[0]["Tare"]);
                txtGross.Text = string.Format("{0:n2}", data.Rows[0]["GrossWeight"]);
                txtNet.Text = string.Format("{0:n2}", data.Rows[0]["NetWeight"]);
            }

            data = new DataTable();
            adapter = new SqlDataAdapter(string.Format(@"select (case when P.GCRecord is null then 'Aktif' else 'Silinmiş' end) as [Durumu], P.Barcode as [Barkod], P.WorkOrderNumber as [Üretim Siparişi No], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], WS.Name as [Vardiya], GrossQuantity as [Brüt], NetQuantity as [Net], ProductionDate as [Üretim Tarihi] from Production P inner join Machine M on M.Oid = P.Machine inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join ShiftStart S on S.Oid = P.[Shift] inner join WorkShift WS on WS.Oid = S.WorkShift inner join Employee E on E.Oid = P.Employee where ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = '{0}')", paletteNumber), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            if (data.Rows.Count > 0)
            {
                gridControl1.DataSource = data;
                gridView1.Columns["Barkod"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                gridView1.Columns["Barkod"].SummaryItem.DisplayFormat = "{0:n0}";
                gridView1.Columns["Brüt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView1.Columns["Brüt"].SummaryItem.DisplayFormat = "{0:n2}";
                gridView1.Columns["Net"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                gridView1.Columns["Net"].SummaryItem.DisplayFormat = "{0:n2}";
                gridView1.Columns["Net"].DisplayFormat.FormatString = "g";
            }
            else { gridControl1.DataSource = null; gridView1.Columns.Clear(); }
            Cursor.Current = currentCursor;
        }

        private void txtPaletteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtPaletteNumber.Text)) RefreshGrid(txtPaletteNumber.Text);
            }
        }
    }
}
