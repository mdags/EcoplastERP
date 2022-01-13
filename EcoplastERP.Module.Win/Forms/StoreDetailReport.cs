using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.XtraEditors.Repository;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class StoreDetailReport : XtraForm
    {
        public XafApplication winApplication;
        public string PaletteNumber = string.Empty;
        public string SalesOrderDetail = string.Empty;
        public StoreDetailReport()
        {
            InitializeComponent();
        }

        private void StoreDetailReport_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select Oid, Code as [Depo Kodu], Name as [Adı] from Warehouse where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            (beiWarehouse.Edit as RepositoryItemSearchLookUpEdit).DataSource = dt;
            (beiWarehouse.Edit as RepositoryItemSearchLookUpEdit).DisplayMember = "Depo Kodu";
            (beiWarehouse.Edit as RepositoryItemSearchLookUpEdit).ValueMember = "Oid";

            RefreshGrid();
        }

        void RefreshGrid()
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select S.Oid, S.Barcode as [Barkod], W.Code as [Depo Kodu], (select Code from Unit where Oid = S.Unit) as [Birim], S.Quantity as [Miktar], (select Code from Unit where Oid = S.cUnit) as [Çevrim Birimi], S.cQuantity as [Çevrim Miktarı] from Store S inner join Warehouse W on W.Oid = S.Warehouse where S.GCRecord is null and S.PaletteNumber = '{0}' and S.SalesOrderDetail = '{1}'", PaletteNumber, SalesOrderDetail), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(dt);
            gridControl1.DataSource = dt;
            gridView1.Columns["Oid"].Visible = false;
        }

        private void btnTransfer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            if (beiWarehouse.EditValue != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                var input = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P120"));
                var output = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P121"));
                var destinyWarehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", beiWarehouse.EditValue));
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    var row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                    var sourceWarehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Code", row["Depo Kodu"].ToString()));
                    if (sourceWarehouse != destinyWarehouse)
                    {
                        var headerId = Guid.NewGuid();
                        var store = objectSpace.FindObject<Store>(new BinaryOperator("Oid", row["Oid"]));
                        if (store != null)
                        {
                            var outputMovement = new Movement(((XPObjectSpace)objectSpace).Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = null,
                                DocumentDate = DateTime.Now,
                                Barcode = store.Barcode,
                                SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", store.SalesOrderDetail.Oid)),
                                Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", store.Product.Oid)),
                                PartyNumber = store.PartyNumber,
                                PaletteNumber = store.PaletteNumber,
                                Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", store.Warehouse.Oid)),
                                MovementType = output,
                                Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", store.Unit.Oid)),
                                Quantity = store.Quantity,
                                cUnit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", store.cUnit.Oid)),
                                cQuantity = store.cQuantity
                            };

                            var inputMovement = new Movement(((XPObjectSpace)objectSpace).Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = null,
                                DocumentDate = DateTime.Now,
                                Barcode = store.Barcode,
                                SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", store.SalesOrderDetail.Oid)),
                                Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", store.Product.Oid)),
                                PartyNumber = store.PartyNumber,
                                PaletteNumber = store.PaletteNumber,
                                Warehouse = destinyWarehouse,
                                MovementType = input,
                                Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", store.Unit.Oid)),
                                Quantity = store.Quantity,
                                cUnit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", store.cUnit.Oid)),
                                cQuantity = store.cQuantity
                            };
                        }
                    }
                }
                objectSpace.CommitChanges();
                RefreshGrid();
                Cursor.Current = Cursors.Default;
            }
            else XtraMessageBox.Show("Depo seçilmemiş !", "Uyarı!");
        }

        private void btnChangeOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            var salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", Guid.Parse(SalesOrderDetail)));
            string barcodeNumber = gridView1.GetFocusedRowCellValue("Barkod").ToString();
            ChangeOrderForm form = new ChangeOrderForm()
            {
                winApplication = winApplication,
                oldOrderNumber = salesOrderDetail.SalesOrder.OrderNumber,
                oldOrderLineNumber = salesOrderDetail.LineNumber,
                barcode = barcodeNumber
            };
            form.ShowDialog();
        }
    }
}
