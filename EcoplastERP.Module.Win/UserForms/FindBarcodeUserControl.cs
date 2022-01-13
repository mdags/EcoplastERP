using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using EcoplastERP.Module.Win.Editors;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class FindBarcodeUserControl : XtraUserControl, IComplexControl
    {
        public FindBarcodeUserControl()
        {
            InitializeComponent();
        }

        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
            RefreshGrid("xxx");
        }

        void RefreshGrid(string barcode)
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select C.Name as Contact, P.WorkOrderNumber, M.Code, WS.Name as [Shift], L.PaletteNumber, H.Code as Machine, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as OrderNumber, E.NameSurname as Employee, P.ProductionDate, P.GrossQuantity, P.NetQuantity from Production P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Product M on M.Oid = D.Product left outer join ProductionPalette L on L.Oid = P.ProductionPalette inner join Machine H on H.Oid = P.Machine inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Employee E on E.Oid = P.Employee inner join ShiftStart SS on SS.Oid = P.[Shift] inner join WorkShift WS on WS.Oid = SS.WorkShift where P.Barcode = '{0}'", barcode), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            if (data.Rows.Count > 0)
            {
                txtContact.Text = data.Rows[0]["Contact"].ToString();
                txtWorkOrderNumber.Text = data.Rows[0]["WorkOrderNumber"].ToString();
                txtProductCode.Text = data.Rows[0]["Code"].ToString();
                txtShift.Text = data.Rows[0]["Shift"].ToString();
                txtPaletteNumber.Text = data.Rows[0]["PaletteNumber"].ToString();
                txtMachine.Text = data.Rows[0]["Machine"].ToString();
                txtOrderNumber.Text = data.Rows[0]["OrderNumber"].ToString();
                txtEmployee.Text = data.Rows[0]["Employee"].ToString();
                txtProductionDate.Text = data.Rows[0]["ProductionDate"].ToString();
                txtGrossQuantity.Text = string.Format("{0:n2}", data.Rows[0]["GrossQuantity"]);
                txtNetQuantity.Text = string.Format("{0:n2}", data.Rows[0]["NetQuantity"]);
            }

            data = new DataTable();
            adapter = new SqlDataAdapter(string.Format(@"IF OBJECT_ID('tempdb..#movements') IS NOT NULL DROP TABLE #movements
            create table #movements([Barkod] nvarchar(100), [Hareket Türü] nvarchar(100), [Malzeme Adı] nvarchar(100), [Depo Kodu] nvarchar(100), [Birim] nvarchar(10), [Miktar] money)
            insert into #movements(Barkod, [Hareket Türü], [Malzeme Adı], [Depo Kodu], Birim, Miktar)
            select M.Barcode as [Barkod], T.Name as [Hareket Türü], P.Name as [Malzeme Adı], W.Code as [Depo Kodu], U.Code as [Birim], M.Quantity as [Miktar] from Movement M inner join MovementType T on T.Oid = M.MovementType inner join Warehouse W on W.Oid = M.Warehouse inner join Product P on P.Oid = M.Product inner join Unit U on U.Oid = M.Unit where M.GCRecord is null and HeaderId in (select top 1 HeaderId from Movement where GCRecord is null and MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P110') and Barcode = '{0}') order by M.DocumentDate desc
            insert into #movements(Barkod, [Hareket Türü], [Malzeme Adı], [Depo Kodu], Birim, Miktar)
            select R.Barcode, 'Üretime Çıkış', (select Name from Product where GCRecord is null and Oid = (select Product from SalesOrderDetail where GCRecord is null and Oid = (select SalesOrderDetail from Production where GCRecord is null and Barcode = '{0}'))), '', U.Code, M.Quantity from ReadResourceMovement M inner join ReadResource R on R.Oid = M.ReadResource inner join Unit U on U.Oid = R.Unit where M.GCRecord is null and M.ProductionBarcode = '{0}'
            select * from #movements", barcode), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            if (data.Rows.Count > 0) gridControl1.DataSource = data;
            else { gridControl1.DataSource = null; gridView1.Columns.Clear(); }

            if (gridView1.RowCount > 0)
            {
                var op = new TreeListOperationFindNodeByText(treeListColumn1, barcode);
                treeListBarcode.NodesIterator.DoOperation(op);
                if (op.Node == null)
                {
                    var root = treeListBarcode.AppendNode(barcode, null);
                    root.SetValue(0, barcode);
                }
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtBarcode.Text))
                {
                    RefreshGrid(txtBarcode.Text);
                }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            if (!string.IsNullOrEmpty(gridView1.GetFocusedRowCellValue("Barkod").ToString()))
            {
                txtBarcode.Text = gridView1.GetFocusedRowCellValue("Barkod").ToString();
                RefreshGrid(gridView1.GetFocusedRowCellValue("Barkod").ToString());
            }
        }

        private void repositoryItemHyperLinkEdit1_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            txtBarcode.Text = e.EditValue.ToString();
            RefreshGrid(e.EditValue.ToString());
        }
    }
}
