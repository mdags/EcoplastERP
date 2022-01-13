using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class WastageReport : XtraForm
    {
        public IObjectSpace objectSpace;
        public int reportID;
        string sqlText = string.Empty, where = string.Empty, groupby = string.Empty;

        public WastageReport()
        {
            InitializeComponent();
        }

        private void WastageReport_Load(object sender, EventArgs e)
        {
            if (reportID == 1)
            {
                sqlText = @"select S.Name as [İstasyon], M.Code as [Makine], C.Name as [Müşteri], sum(W.GrossQuantity) as [Brüt], sum(W.NetQuantity) as [Net] 
                    from Wastage W inner join Machine M on M.Oid = W.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact
                    where W.GCRecord is null ";
                groupby = " group by M.Code, S.Name, C.Name";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                Shift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = true;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 2)
            {
                sqlText = @"select S.Name as [İstasyon], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], C.Name as [Müşteri], sum(W.GrossQuantity) as [Brüt], sum(W.NetQuantity) as [Net] 
                    from Wastage W inner join Machine M on M.Oid = W.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact
                    where W.GCRecord is null ";
                groupby = " group by M.Code, S.Name, O.OrderNumber, D.LineNumber, C.Name";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                Shift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 3)
            {
                sqlText = @"select S.Name as [İstasyon], M.Code as [Makine], PR.Code as [Stok Kodu], PR.Name as [Stok Adı], sum(W.GrossQuantity) as [Brüt], sum(W.NetQuantity) as [Net] 
                    from Wastage W inner join Machine M on M.Oid = W.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product PR on PR.Oid = D.Product
                    where W.GCRecord is null ";
                groupby = " group by M.Code, S.Name, PR.Code, PR.Name";
                ProductGroup.Enabled = true;
                Shift.Enabled = true;
                ProductCode.Enabled = true;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = true;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 4)
            {
                sqlText = @"select S.Name as [İstasyon ], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], C.Name as [Müşteri], PR.Code as [Stok Kodu], PR.Name as [Stok Adı], sum(W.GrossQuantity) as [Brüt], sum(W.NetQuantity) as [Net] 
                    from Wastage W inner join Machine M on M.Oid = W.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product PR on PR.Oid = D.Product where W.GCRecord is null ";
                groupby = " group by M.Code, S.Name, O.OrderNumber, D.LineNumber, C.Name, PR.Code, PR.Name";
                ProductGroup.Enabled = true;
                Shift.Enabled = true;
                ProductCode.Enabled = true;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 5)
            {
                sqlText = @"select S.Name as [İstasyon], M.Code as [Makine], (case when SS.Shift = 1 then 'A' when SS.Shift = 2 then 'B' when SS.Shift = 3 then 'C' else null end) as [Vardiya], E.NameSurname as [Operatör], sum(W.GrossQuantity) as [Brüt], sum(W.NetQuantity) as [Net] 
                    from Wastage W inner join Machine M on M.Oid = W.Machine inner join Station S on S.Oid = M.Station inner join ShiftStart SS on SS.Oid = W.Shift left outer join Employee E on E.Oid = W.Employee where W.GCRecord is null ";
                groupby = " group by S.Name, M.Code, SS.Shift, E.NameSurname";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                Shift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                PaletteNumber.Enabled = true;
            }
            if (reportID == 6)
            {
                sqlText = @"select S.Name as [İstasyon], H.Name as [Ürün Grubu], T.Name as [Malzeme Türü], sum(W.GrossQuantity) as [Brüt], sum(W.NetQuantity) as [Net] from Wastage W inner join Machine M on M.Oid = W.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join Product PR on PR.Oid = D.Product inner join ProductGroup PG on PR.ProductGroup = PG.Oid inner join HCategory H on H.Oid = PG.Oid inner join MaterialType T on T.Oid = PR.MaterialType where W.GCRecord is null  ";
                groupby = " group by S.Name, H.Name, T.Name";
                ProductGroup.Enabled = true;
                ProductGroup.EditValue = null;
                Shift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                PaletteNumber.Enabled = false;
            }

            BeginDate.DateTime = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd 08:15:00"));
            EndDate.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 08:15:00"));

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select Oid, Name as [Adı] from Station where GCRecord is null order by Name",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Station.Properties.DataSource = dt;
            Station.Properties.DisplayMember = "Adı";
            Station.Properties.ValueMember = "Oid";
            Station.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Kod], Name as [Adı] from Machine where GCRecord is null order by Name",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Machine.Properties.DataSource = dt;
            Machine.Properties.DisplayMember = "Kod";
            Machine.Properties.ValueMember = "Oid";
            Machine.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select G.Oid as Oid, G.Code as [Kod], C.Name as [Adı] from ProductGroup G inner join HCategory C on C.Oid = G.Oid where C.GCRecord is null and Parent is not null order by C.Name",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            ProductGroup.Properties.DataSource = dt;
            ProductGroup.Properties.DisplayMember = "Adı";
            ProductGroup.Properties.ValueMember = "Oid";
            ProductGroup.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Stok Kodu], Name as [Adı] from Product where GCRecord is null order by Name",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            ProductCode.Properties.DataSource = dt;
            ProductCode.Properties.DisplayMember = "Adı";
            ProductCode.Properties.ValueMember = "Oid";
            ProductCode.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Kodu], Name as [Adı] from Contact where GCRecord is null order by Code",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Contact.Properties.DataSource = dt;
            Contact.Properties.DisplayMember = "Adı";
            Contact.Properties.ValueMember = "Oid";
            Contact.ForceInitialize();    
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;
            if (EndDate.DateTime < BeginDate.DateTime)
            {
                throw new UserFriendlyException("Başlangıç tarihi bitiş tarihinden büyük olamaz...");
            }
            else
            {
                if (BeginDate.DateTime != null) where += String.Format(" and W.WastageDate >= '{0}'", BeginDate.DateTime.ToString("yyyy-MM-dd HH:mm"));
                if (EndDate.DateTime != null) where += String.Format(" and W.WastageDate <= '{0}'", EndDate.DateTime.ToString("yyyy-MM-dd HH:mm"));
                if (Station.EditValue != null) where += String.Format(" and M.Station = '{0}'", Station.EditValue);
                if (Machine.EditValue != null) where += String.Format(" and W.Machine = '{0}'", Machine.EditValue);
                if (ProductGroup.EditValue != null)
                {
                    var productGroup = objectSpace.FindObject<ProductGroup>(new BinaryOperator("Oid", ProductGroup.EditValue));
                    if (productGroup != null) where += String.Format(" and PR.ProductGroup = '{0}'", productGroup.Oid);
                }
                if (!String.IsNullOrEmpty(Shift.SelectedText)) where += String.Format(" and SS.Shift = {0}", Shift.SelectedText == "A" ? "1" : Shift.SelectedText == "B" ? "2" : Shift.SelectedText == "C" ? "3" : "");
                if (ProductCode.EditValue != null) where += String.Format(" and PR.Oid = '{0}'", ProductCode.EditValue);
                if (!String.IsNullOrEmpty(OrderNumber.Text)) where += String.Format(" and O.OrderNumber = '{0}'", OrderNumber.Text);
                if (Contact.EditValue != null) where += String.Format(" and O.Contact = '{0}'", Contact.EditValue);
                if (!String.IsNullOrEmpty(PaletteNumber.Text)) where += String.Format(" and W.PaletteNumber = '{0}'", PaletteNumber.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
                adapter.Fill(dt);
                gridControl1.DataSource = dt;

                gridView1.Columns["Brüt"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Brüt"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Net"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Net"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Brüt"] != null)
                {
                    if (gridView1.Columns["Brüt"].Summary.Count == 0)
                        gridView1.Columns["Brüt"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Brüt", "{0:n2}");
                }
                if (gridView1.Columns["Net"] != null)
                {
                    if (gridView1.Columns["Net"].Summary.Count == 0)
                        gridView1.Columns["Net"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Net", "{0:n2}");
                }

                if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            }
        }

        private void btnPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.ShowRibbonPrintPreview();
        }

        private void bbiProductionbyContact_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reportID = 1;
            WastageReport_Load(null, null);
        }

        private void bbiProductionbyOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reportID = 2;
            WastageReport_Load(null, null);
        }

        private void bbiProductionbyProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reportID = 3;
            WastageReport_Load(null, null);
        }

        private void bbiProductionbyContactOrderProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reportID = 4;
            WastageReport_Load(null, null);
        }

        private void bbiProductionbyShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reportID = 5;
            WastageReport_Load(null, null);
        }
    }
}
