using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class WastageReportUserControl : XtraUserControl, IComplexControl
    {
        public WastageReportUserControl()
        {
            InitializeComponent();
        }

        private IObjectSpace objectSpace;
        public int reportID;
        string sqlText = string.Empty, where = string.Empty, groupby = string.Empty;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
            reportID = 1;
            BeginDate.DateTime = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd 08:15:00"));
            EndDate.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 08:15:00"));

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select Oid, Code as [Adı] from Station where GCRecord is null order by Name",
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

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Name as [Adı] from WorkShift where GCRecord is null and ProductionShift = 1 order by Name",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            WorkShift.Properties.DataSource = dt;
            WorkShift.Properties.DisplayMember = "Adı";
            WorkShift.Properties.ValueMember = "Oid";
            WorkShift.ForceInitialize();

            SelectReportType();
        }

        public void SelectReportType()
        {
            if (reportID == 1)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], C.Name as [Müşteri], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, C.Name order by S.Code";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = true;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
            }
            if (reportID == 2)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], C.Name as [Müşteri], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, O.OrderNumber, D.LineNumber, C.Name order by S.Code";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
            }
            if (reportID == 3)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], PR.Name as [Malzeme], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join Product PR on PR.Oid = P.Product where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, PR.Name order by S.Code";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = false;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
            }
            if (reportID == 4)
            {
                sqlText = @"select S.Code as [İstasyon ], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], C.Name as [Müşteri], PR.Code as [Stok Kodu], PR.Name as [Stok Adı], WS.Name as [Vardiya], E.NameSurname as [Operatör], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product PR on PR.Oid = D.Product inner join ShiftStart SS on SS.Oid = P.Shift left outer join Employee E on E.Oid = P.Employee inner join WorkShift WS on WS.Oid = SS.WorkShift where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, O.OrderNumber, D.LineNumber, C.Name, PR.Code, PR.Name, WS.Name, E.NameSurname order by S.Code";
                ProductGroup.Enabled = true;
                WorkShift.Enabled = true;
                ProductCode.Enabled = true;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                WorkShift.Enabled = true;
                WorkShift.EditValue = null;
            }
            if (reportID == 5)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], WS.Name as [Vardiya], E.NameSurname as [Operatör], sum(P.GrossQuantity) as [Miktar]from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join ShiftStart SS on SS.Oid = P.Shift left outer join Employee E on E.Oid = P.Employee inner join WorkShift WS on WS.Oid = SS.WorkShift where P.GCRecord is null ";
                groupby = " group by S.Code, M.Code, WS.Name, E.NameSurname order by S.Code";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
            }
            if (reportID == 6)
            {
                sqlText = @"select S.Code as [İstasyon], PK.Name as [Ürün Grubu], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Product PR on PR.Oid = D.Product left outer join ProductKind PK on PK.Oid = PR.ProductKind where P.GCRecord is null ";
                groupby = " group by S.Code, PK.Name order by S.Code";
                ProductGroup.Enabled = true;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
            }
            if (reportID == 7)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], WR.Name, WS.Name as [Vardiya], E.NameSurname as [Operatör], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join ShiftStart SS on SS.Oid = P.Shift left outer join Employee E on E.Oid = P.Employee inner join WorkShift WS on WS.Oid = SS.WorkShift inner join WastageReason WR on WR.Oid = P.WastageReason where P.GCRecord is null ";
                groupby = " group by S.Code, M.Code, WR.Name, WS.Name, E.NameSurname order by S.Code";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
            }
            if (reportID == 8)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], sum(P.GrossQuantity) as [Miktar] from Wastage P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code order by S.Code, M.Code";
                ProductGroup.Enabled = false;
                ProductGroup.EditValue = null;
                WorkShift.Enabled = false;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
            }
        }

        public void RefreshGrid()
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
                if (BeginDate.DateTime != null) where += string.Format(" and P.WastageDate >= '{0}'", BeginDate.DateTime.ToString("yyyy-MM-dd HH:mm"));
                if (EndDate.DateTime != null) where += string.Format(" and P.WastageDate <= '{0}'", EndDate.DateTime.ToString("yyyy-MM-dd HH:mm"));
                if (Station.EditValue != null) where += string.Format(" and M.Station = '{0}'", Station.EditValue);
                if (Machine.EditValue != null) where += string.Format(" and P.Machine = '{0}'", Machine.EditValue);
                if (ProductGroup.EditValue != null)
                {
                    var productGroup = objectSpace.FindObject<ProductGroup>(new BinaryOperator("Oid", ProductGroup.EditValue));
                    if (productGroup != null) where += string.Format(" and PR.ProductGroup = '{0}'", productGroup.Oid);
                }
                if (WorkShift.EditValue != null) where += string.Format(" and WS.Oid = '{0}'", WorkShift.EditValue);
                if (ProductCode.EditValue != null) where += string.Format(" and PR.Oid = '{0}'", ProductCode.EditValue);
                if (!string.IsNullOrEmpty(OrderNumber.Text)) where += string.Format(" and O.OrderNumber = '{0}'", OrderNumber.Text);
                if (Contact.EditValue != null) where += string.Format(" and O.Contact = '{0}'", Contact.EditValue);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
                adapter.Fill(dt);
                gridControl1.DataSource = dt;

                gridView1.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Miktar"] != null)
                {
                    if (gridView1.Columns["Miktar"].Summary.Count == 0)
                        gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
                }

                if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            }
        }
    }
}
