using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class ErusluStoreReportUserControl : XtraUserControl, IComplexControl
    {
        public ErusluStoreReportUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        public int reportID;
        string sqlText = string.Empty;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

            reportID = 1;
            SelectReportType();
        }

        public void SelectReportType()
        {
            if (reportID == 1)
            {
                sqlText = @"select C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.Oid as SalesOrderDetail, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], D.LineDeliveryDate as [Termin Tarihi], G.Name as [Ürün Grubu], T.Name as [Ürün Tipi], K.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], SUM(S.Quantity) as [SÖB Miktarı], (select Code from Unit where Oid = S.Unit) as [SÖB Birimi], SUM(S.cQuantity) as [TÖB Miktarı], (select Code from Unit where Oid = S.cUnit) as [TÖB Birimi], D.PetkimUnitPrice as [Petkim Birim Fiyat ($)] from Store S left outer join SalesOrderDetail D on D.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder left outer join Contact C on C.Oid = O.Contact left outer join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = S.Product left outer join ProductGroup G on G.Oid = P.ProductGroup left outer join ProductType T on T.Oid = P.ProductType left outer join ProductKind K on K.Oid = P.ProductKind left outer join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell where S.GCRecord is null and S.Warehouse in (select Oid from Warehouse where Code in ('900', '901')) group by C.Name, SC.Name, D.Oid, D.PetkimUnitPrice, O.OrderNumber, D.LineNumber, D.LineDeliveryDate, G.Name, T.Name, K.Name, P.Code, P.Name, W.Code, WC.Name, S.Unit, S.cUnit order by W.Code";
            }
            if (reportID == 2)
            {
                sqlText = @"select C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.Oid as SalesOrderDetail, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], D.LineDeliveryDate as [Termin Tarihi], G.Name as [Ürün Grubu], T.Name as [Ürün Tipi], K.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], S.PaletteNumber as [Palet Numarası], count(S.Oid) as [Paletteki Ür. Ad.], isnull((select top 1 Tare from ProductionPalette where PaletteNumber = S.PaletteNumber), 0) as [Palet Darası], isnull((select top 1 LastWeight from ProductionPalette where PaletteNumber = S.PaletteNumber), 0) as [Palet Son Ağırlık], SUM(S.Quantity) as [SÖB Miktarı], (select Code from Unit where Oid = S.Unit) as [SÖB Birimi], SUM(S.cQuantity) as [TÖB Miktarı], (select Code from Unit where Oid = S.cUnit) as [TÖB Birimi], D.PetkimUnitPrice as [Petkim Birim Fiyat ($)] from Store S left outer join SalesOrderDetail D on D.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder left outer join Contact C on C.Oid = O.Contact left outer join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = S.Product left outer join ProductGroup G on G.Oid = P.ProductGroup left outer join ProductType T on T.Oid = P.ProductType left outer join ProductKind K on K.Oid = P.ProductKind left outer join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell where S.GCRecord is null and S.Warehouse in (select Oid from Warehouse where Code in ('900', '901')) group by C.Name, SC.Name, D.Oid, D.PetkimUnitPrice, O.OrderNumber, D.LineNumber, D.LineDeliveryDate, G.Name, T.Name, K.Name, P.Code, P.Name, W.Code, WC.Name, S.PaletteNumber, S.Unit, S.cUnit order by W.Code";
            }
            if (reportID == 3)
            {
                sqlText = @"select C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.Oid as SalesOrderDetail, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], D.LineDeliveryDate as [Termin Tarihi], G.Name as [Ürün Grubu], T.Name as [Ürün Tipi], K.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], S.PaletteNumber as [Palet Numarası], S.Barcode as [Barkod], (select ProductionDate from Production where Barcode = S.Barcode) as [Üretim Tarihi], SUM(S.Quantity) as [SÖB Miktarı], (select Code from Unit where Oid = S.Unit) as [SÖB Birimi], SUM(S.cQuantity) as [TÖB Miktarı], (select Code from Unit where Oid = S.cUnit) as [TÖB Birimi], D.PetkimUnitPrice as [Petkim Birim Fiyat ($)] from Store S left outer join SalesOrderDetail D on D.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder left outer join Contact C on C.Oid = O.Contact left outer join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = S.Product left outer join ProductGroup G on G.Oid = P.ProductGroup left outer join ProductType T on T.Oid = P.ProductType left outer join ProductKind K on K.Oid = P.ProductKind left outer join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell where S.GCRecord is null and S.Warehouse in (select Oid from Warehouse where Code in ('900', '901')) group by C.Name, SC.Name, D.Oid, D.PetkimUnitPrice, O.OrderNumber, D.LineNumber, D.LineDeliveryDate, G.Name, T.Name, K.Name, P.Code, P.Name, S.PaletteNumber, W.Code, WC.Name, S.Barcode, S.Unit, S.cUnit order by W.Code";
            }
        }
        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(dt);
            dt.Columns["SalesOrderDetail"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Sipariş No"] != null)
            {
                gridView1.Columns["Sipariş No"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Sipariş No"].DisplayFormat.FormatString = "n0";
                if (gridView1.Columns["Sipariş No"].Summary.Count == 0)
                    gridView1.Columns["Sipariş No"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Sipariş No", "{0:n0}");
            }
            if (gridView1.Columns["SÖB Miktarı"] != null)
            {
                if (gridView1.Columns["SÖB Miktarı"].ColumnEdit != null)
                {
                    gridView1.Columns["SÖB Miktarı"].ColumnEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["SÖB Miktarı"].ColumnEdit.EditFormat.FormatString = "n2";
                }
                gridView1.Columns["SÖB Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["SÖB Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["SÖB Miktarı"].Summary.Count == 0)
                    gridView1.Columns["SÖB Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "SÖB Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["TÖB Miktarı"] != null)
            {
                gridView1.Columns["TÖB Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["TÖB Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["TÖB Miktarı"].Summary.Count == 0)
                    gridView1.Columns["TÖB Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TÖB Miktarı", "{0:n2}");
            }
            if (reportID == 2)
            {
                if (gridView1.Columns["Palet Darası"] != null)
                {
                    gridView1.Columns["Palet Darası"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Palet Darası"].DisplayFormat.FormatString = "n2";
                    if (gridView1.Columns["Palet Darası"].Summary.Count == 0)
                        gridView1.Columns["Palet Darası"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Palet Darası", "{0:n2}");
                }
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
