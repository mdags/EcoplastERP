using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class WaitingSalesOrderReport : XtraForm
    {
        public XafApplication winApplication;
        string sqlText = string.Empty, where = string.Empty, orderby = string.Empty;

        public WaitingSalesOrderReport()
        {
            InitializeComponent();
        }

        private void WaitingSalesOrderReport_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();

            cbContact.Properties.DataSource = new XPCollection<Contact>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Active", true), new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbContact.Properties.DisplayMember = "Name";
            cbContact.Properties.ValueMember = "Oid";

            cbProduct.Properties.DataSource = new XPCollection<Product>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Active", true), new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProduct.Properties.DisplayMember = "Name";
            cbProduct.Properties.ValueMember = "Oid";

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select G.Oid, H.Name from ProductGroup G inner join HCategory H on G.Oid = H.Oid where H.GCRecord is null order by H.Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            cbProductGroup.Properties.DataSource = dt;
            cbProductGroup.Properties.DisplayMember = "Name";
            cbProductGroup.Properties.ValueMember = "Oid";

            sqlText = @"select (case when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Çekim Bekliyor' when D.SalesOrderStatus = 106 then 'Cast Aktarma Bekliyor' when D.SalesOrderStatus = 107 then 'Cast Çekim Bekliyor' when D.SalesOrderStatus = 108 then 'Balonlu Çekim Bekliyor' when D.SalesOrderStatus = 109 then 'Katlama Bekliyor' when D.SalesOrderStatus = 111 then 'Balonlu Kesim Bekliyor' when D.SalesOrderStatus = 112 then 'Dilme Bekliyor' when D.SalesOrderStatus = 113 then 'Laminasyon Bekliyor' when D.SalesOrderStatus = 105 then ' Baskı Bekliyor' when D.SalesOrderStatus = 102 then 'Kesim Bekliyor' when D.SalesOrderStatus = 103 then 'Rejenere Bekliyor' when D.SalesOrderStatus = 104 then 'Üretim Bekliyor' when D.SalesOrderStatus = 110 then 'Sevk Bekliyor' when D.SalesOrderStatus = 120 then 'Yükleme Bekliyor' else '' end) as [Durumu], 
	O.OrderNumber as [Sipariş No], D.LineNumber as [Satır No], H.Name as [Cari Grup], 
	C.Name as [Müşteri], D.LineInstruction as [Satır Açıklama], P.Name as [Malzeme Adı], PG.Name as [Ürün Grubu], O.OrderDate as [Sipariş Tarihi], D.LineDeliveryDate as [Satır Termin], U.Code as [Birimi], D.Quantity as [Miktar], Cu.Code as [Çevrim Birimi], D.cQuantity as [Çevrim Miktarı], 
	(select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and FilmingWorkOrder is not null and SalesOrderDetail = D.Oid) as [Çekim Üretilen], 
	(select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CuttingWorkOrder is not null and SalesOrderDetail = D.Oid) as [Kesim Üretilen], 
	(select isnull(sum(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705') as [Sevk Bekleyen], 
	(select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) as [Sipariş Üretilen], 
	(select isnull(sum(GrossQuantity), 0) from Production where Oid in (select Production from Loading where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen] 
from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join ContactGroup G on G.Oid = C.ContactGroup left outer join HCategory H on H.Oid = G.Oid and H.GCRecord is null inner join Product P on P.Oid = D.Product left outer join Unit U on U.Oid = D.Unit left outer join Unit Cu on Cu.Oid = D.cUnit inner join HCategory PG on PG.Oid = P.ProductGroup 
where D.GCRecord is null and D.SalesOrderStatus < 200 ";
            orderby = " order by O.OrderDate";
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;

            if (!string.IsNullOrEmpty(cbProduct.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProduct.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and P.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbContact.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbContact.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and C.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductGroup.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductGroup.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and PG.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(txtOrderNumber.Text))
            {
                string list = string.Empty;
                foreach (string item in txtOrderNumber.Text.Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and O.OrderNumber in ({0})", list.Substring(0, list.Length - 1));
            }
            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(dt);
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Durumu"] != null)
            {
                gridView1.Columns["Durumu"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Durumu"].DisplayFormat.FormatString = "n0";
                if (gridView1.Columns["Durumu"].Summary.Count == 0)
                    gridView1.Columns["Durumu"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Çekim Üretilen", "{0:n0}");
            }
            if (gridView1.Columns["Miktar"] != null)
            {
                gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Miktar"].Summary.Count == 0)
                    gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Çevrim Miktarı"] != null)
            {
                gridView1.Columns["Çevrim Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Çevrim Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Çevrim Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Çevrim Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Çevrim Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["Çekim Üretilen"] != null)
            {
                gridView1.Columns["Çekim Üretilen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Çekim Üretilen"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Çekim Üretilen"].Summary.Count == 0)
                    gridView1.Columns["Çekim Üretilen"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Çekim Üretilen", "{0:n2}");
            }
            if (gridView1.Columns["Kesim Üretilen"] != null)
            {
                gridView1.Columns["Kesim Üretilen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Kesim Üretilen"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Kesim Üretilen"].Summary.Count == 0)
                    gridView1.Columns["Kesim Üretilen"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Kesim Üretilen", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Bekleyen"] != null)
            {
                gridView1.Columns["Sevk Bekleyen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Sevk Bekleyen"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Bekleyen"].Summary.Count == 0)
                    gridView1.Columns["Sevk Bekleyen"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Bekleyen", "{0:n2}");
            }
            if (gridView1.Columns["Sipariş Üretilen"] != null)
            {
                gridView1.Columns["Sipariş Üretilen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Sipariş Üretilen"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sipariş Üretilen"].Summary.Count == 0)
                    gridView1.Columns["Sipariş Üretilen"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sipariş Üretilen", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Edilen"] != null)
            {
                gridView1.Columns["Sevk Edilen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Sevk Edilen"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen"].Summary.Count == 0)
                    gridView1.Columns["Sevk Edilen"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }

        private void btnPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.ShowRibbonPrintPreview();
        }
    }
}
