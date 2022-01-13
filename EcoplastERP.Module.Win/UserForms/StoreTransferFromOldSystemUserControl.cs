using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.Win.Forms;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class StoreTransferFromOldSystemUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;

        public StoreTransferFromOldSystemUserControl()
        {
            InitializeComponent();
        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        void IComplexControl.Refresh()
        {

        }

        public void RefreshGrid(string warehouseCode, string orderNumber, string paletteNumber)
        {
            string where = string.Empty;
            string select = @"SELECT S.ORCODE as [Sipariş No], C.CSNAME as [Müşteri], M.MTCODE as [Stok Kodu], M.TEXT100 as [Stok Adı], W.STORECODE as [Depo Kodu], W.STORENAME as [Depo Adı], S.PALETTENO as [Palet No], SUM(S.AMOUNT) AS [Miktar], S.UNIT as [Birim] FROM STORES S INNER JOIN MATERIALS M ON M.MATERIALID = S.MATERIALID INNER JOIN STORAGE W ON W.STOREID = S.STOREID INNER JOIN ORDERS O ON O.ORCODE = S.ORCODE INNER JOIN CUSTOMERS C ON C.CUSTOMERID = O.CUSTOMERID WHERE S.AMOUNT > 0 AND M.MTYPEID = 1 AND W.STORECODE NOT IN ('898', '899') ";
            if (!string.IsNullOrEmpty(warehouseCode))
            {
                where += string.Format(" AND W.STORECODE = '{0}'", warehouseCode);
            }
            if (!string.IsNullOrEmpty(orderNumber))
            {
                where += string.Format(" AND S.ORCODE = '{0}'", orderNumber);
            }
            if (!string.IsNullOrEmpty(paletteNumber))
            {
                where += string.Format(" AND S.PALETTENO = '{0}'", paletteNumber);
            }
            string groupby = @" GROUP BY S.ORCODE, C.CSNAME, M.MTCODE, M.TEXT100, W.STORECODE, W.STORENAME, S.PALETTENO, S.UNIT";
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionStringOldERP, CommandType.Text, select + where + groupby).Tables[0];
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();
            if (gridView1.Columns.Count > 0)
            {
                gridView1.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sipariş No"].Summary.Count == 0) gridView1.Columns["Sipariş No"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Sipariş No", "{0:n0}");
                if (gridView1.Columns["Miktar"].Summary.Count == 0) gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
            }
        }

        private void gridView1_DoubleClick(object sender, System.EventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            OldSystemDetailForm form = new OldSystemDetailForm()
            {
                paletteNumber = gridView1.GetFocusedRowCellValue("Palet No").ToString()
            };
            form.ShowDialog();
        }
    }
}
