using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class ErusluSalesOrderReportUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;

        public ErusluSalesOrderReportUserControl()
        {
            InitializeComponent();
        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        private void ErusluSalesOrderReport_Load(object sender, EventArgs e)
        {
            foreach (object current in Enum.GetValues(typeof(SalesOrderStatus)))
            {
                EnumDescriptor ed = new EnumDescriptor(typeof(SalesOrderStatus));
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current) { ImageName = ImageLoader.Instance.GetEnumValueImageName(current) };
                cmbSalesOrderStatus.Properties.Items.Add(item);
            }

            deBeginDate.DateTime = DateTime.Now.AddDays(-1);
            deEndDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            string where = string.Empty;
            string sqlText = string.Format(@"select O.OrderNumber as [Sipariş No], D.LineNumber as [Pozisyon], O.OrderDate as [Sipariş Tarihi], C.Name as [Siparişi Veren], O.ContactOrderNumber as [Müş.Sip.No], PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], P.Name as [Stok Adı], U.Code as [SÖB Birim], D.Quantity as [SÖB Miktar], CU.Code as [TÖB Birim], D.cQuantity as [TÖB Miktar], D.LineDeliveryDate as [Termin Tarihi], (case when D.SalesOrderWorkStatus = 0 then 'Yeni' when D.SalesOrderWorkStatus = 1 then 'Tekrar' else 'Revizyon' end) as [İş Durumu], (select isnull(sum(Quantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and SalesOrderDetail = D.Oid) as [Sevk Depo SÖB Miktar], (select isnull(sum(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and SalesOrderDetail = D.Oid) as [Sevk Depo TÖB Miktar], (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and IsLastProduction = 1 and SalesOrderDetail = D.Oid) as [Üretim Miktarı], (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen SÖB Miktar], (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen TÖB Miktar] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind inner join Unit U on U.Oid = D.Unit inner join Unit CU on CU.Oid = D.cUnit where D.GCRecord is null and D.SalesOrderStatus < 200 and C.Code = '120-04-001' and O.OrderDate between '{0}' and '{1}' order by D.LineDeliveryDate", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));

            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;

            if (!string.IsNullOrEmpty(cmbSalesOrderStatus.Text))
            {
                foreach (object current in Enum.GetValues(typeof(SalesOrderStatus)))
                {
                    EnumDescriptor ed = new EnumDescriptor(typeof(SalesOrderStatus));
                    if (ed.GetCaption(current) == cmbSalesOrderStatus.Text)
                    {
                        where += string.Format(" and D.SalesOrderStatus = '{0}'", (int)current);
                    }
                }
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();

            if (gridView1.Columns["Sipariş No"] != null)
            {
                gridView1.Columns["Sipariş No"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sipariş No"].DisplayFormat.FormatString = "n0";
                if (gridView1.Columns["Sipariş No"].Summary.Count == 0)
                    gridView1.Columns["Sipariş No"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Sipariş No", "{0:n0}");
            }
            if (gridView1.Columns["SÖB Miktar"] != null)
            {
                gridView1.Columns["SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["TÖB Miktar"] != null)
            {
                gridView1.Columns["TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Depo SÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Depo SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Depo SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Depo SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Depo SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Depo SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Depo TÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Depo TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Depo TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Depo TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Depo TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Depo TÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Üretim Miktarı"] != null)
            {
                gridView1.Columns["Üretim Miktarı"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Üretim Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Üretim Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Üretim Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Üretim Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Edilen SÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Edilen SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Edilen TÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Edilen TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Edilen TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen TÖB Miktar", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }
    }
}
