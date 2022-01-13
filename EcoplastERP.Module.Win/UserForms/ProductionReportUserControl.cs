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
    public partial class ProductionReportUserControl : XtraUserControl, IComplexControl
    {
        public ProductionReportUserControl()
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
            SqlDataAdapter adapter = new SqlDataAdapter(@"select Oid, Code as [Adı] from Station where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Station.Properties.DataSource = dt;
            Station.Properties.DisplayMember = "Adı";
            Station.Properties.ValueMember = "Oid";
            Station.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Kod], Name as [Adı] from Machine where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Machine.Properties.DataSource = dt;
            Machine.Properties.DisplayMember = "Kod";
            Machine.Properties.ValueMember = "Oid";
            Machine.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Name as Adı from ProductKind where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            ProductKind.Properties.DataSource = dt;
            ProductKind.Properties.DisplayMember = "Adı";
            ProductKind.Properties.ValueMember = "Oid";
            ProductKind.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Stok Kodu], Name as [Adı] from Product where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            ProductCode.Properties.DataSource = dt;
            ProductCode.Properties.DisplayMember = "Adı";
            ProductCode.Properties.ValueMember = "Oid";
            ProductCode.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Kodu], Name as [Adı] from Contact where GCRecord is null order by Code", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Contact.Properties.DataSource = dt;
            Contact.Properties.DisplayMember = "Adı";
            Contact.Properties.ValueMember = "Oid";
            Contact.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Name as [Adı] from WorkShift where GCRecord is null and ProductionShift = 1 order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
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
            sqlText = string.Empty;
            where = string.Empty;
            groupby = string.Empty;
            if (reportID == 1)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], C.Name as [Müşteri], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, C.Name order by S.Code";
                ProductKind.Enabled = false;
                ProductKind.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = true;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 2)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], C.Name as [Müşteri], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, O.OrderNumber, D.LineNumber, C.Name order by S.Code";
                ProductKind.Enabled = false;
                ProductKind.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 3)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], PR.Code as [Stok Kodu], PR.Name as [Stok Adı], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product PR on PR.Oid = D.Product where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, PR.Code, PR.Name order by S.Code";
                ProductKind.Enabled = true;
                WorkShift.Enabled = true;
                ProductCode.Enabled = true;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = true;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 4)
            {
                sqlText = @"select S.Code as [İstasyon ], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], C.Name as [Müşteri], PR.Code as [Stok Kodu], PR.Name as [Stok Adı], WS.Name as [Vardiya], E.NameSurname as [Operatör], PP.PaletteNumber as [Palet No], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product PR on PR.Oid = D.Product left outer join ProductionPalette PP on PP.Oid = P.ProductionPalette inner join ShiftStart SS on SS.Oid = P.Shift left outer join Employee E on E.Oid = P.Employee inner join WorkShift WS on WS.Oid = SS.WorkShift where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, O.OrderNumber, D.LineNumber, C.Name, PR.Code, PR.Name, PP.PaletteNumber, WS.Name, E.NameSurname order by S.Code";
                ProductKind.Enabled = true;
                WorkShift.Enabled = true;
                ProductCode.Enabled = true;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                WorkShift.Enabled = true;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = true;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 5)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], WS.Name as [Vardiya], E.NameSurname as [Operatör], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join ShiftStart SS on SS.Oid = P.Shift left outer join Employee E on E.Oid = P.Employee inner join WorkShift WS on WS.Oid = SS.WorkShift where P.GCRecord is null ";
                groupby = " group by S.Code, M.Code, WS.Name, E.NameSurname order by S.Code";
                ProductKind.Enabled = false;
                ProductKind.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                PaletteNumber.Enabled = true;
            }
            if (reportID == 6)
            {
                sqlText = @"select S.Code as [İstasyon], PK.Name as [Ürün Grubu], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Product PR on PR.Oid = D.Product left outer join ProductKind PK on PK.Oid = PR.ProductKind where P.GCRecord is null ";
                groupby = " group by S.Code, PK.Name order by S.Code";
                ProductKind.Enabled = true;
                ProductKind.EditValue = null;
                WorkShift.Enabled = true;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                PaletteNumber.Enabled = false;
            }
            if (reportID == 7)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], CAST(P.ProductionDate as date) as [Üretim Tarihi], C.Name as [Müşteri], PR.Code as [Stok Kodu], PR.Name as [Stok Adı], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product PR on PR.Oid = D.Product where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code, CAST(ProductionDate as date), C.Name, PR.Code, PR.Name order by S.Code";
                ProductKind.Enabled = true;
                WorkShift.Enabled = true;
                ProductCode.Enabled = true;
                OrderNumber.Enabled = true;
                Contact.Enabled = true;
                WorkShift.Enabled = true;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = true;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 8)
            {
                sqlText = @"select S.Code as [İstasyon], M.Code as [Makine], sum(P.GrossQuantity) as [Brüt], sum(P.NetQuantity) as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null ";
                groupby = " group by M.Code, S.Code order by S.Code, M.Code ";
                ProductKind.Enabled = false;
                ProductKind.EditValue = null;
                WorkShift.Enabled = false;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
            }
            if (reportID == 9)
            {
                sqlText = @"select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.FilmingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from FilmingWorkOrder where Oid = P.FilmingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and FilmingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.CastFilmingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from CastFilmingWorkOrder where Oid = P.CastFilmingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and CastFilmingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.CastTransferingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from CastTransferingWorkOrder where Oid = P.CastTransferingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and CastTransferingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.BalloonFilmingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from BalloonFilmingWorkOrder where Oid = P.BalloonFilmingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and BalloonFilmingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.PrintingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from PrintingWorkOrder where Oid = P.PrintingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and PrintingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.LaminationWorkOrder is not null then (select Name from Station where Oid = (select NextStation from LaminationWorkOrder where Oid = P.LaminationWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and LaminationWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.SlicingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from SlicingWorkOrder where Oid = P.SlicingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and SlicingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.CastSlicingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from CastSlicingWorkOrder where Oid = P.CastSlicingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and CastSlicingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.CuttingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from CuttingWorkOrder where Oid = P.CuttingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and CuttingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.RegeneratedWorkOrder is not null then (select Name from Station where Oid = (select NextStation from RegeneratedWorkOrder where Oid = P.RegeneratedWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and RegeneratedWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.CastRegeneratedWorkOrder is not null then (select Name from Station where Oid = (select NextStation from CastRegeneratedWorkOrder where Oid = P.CastRegeneratedWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and CastRegeneratedWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.Eco6WorkOrder is not null then (select Name from Station where Oid = (select NextStation from Eco6WorkOrder where Oid = P.Eco6WorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and Eco6WorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] 
                UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.Eco6CuttingWorkOrder is not null then (select Name from Station where Oid = (select NextStation from Eco6CuttingWorkOrder where Oid = P.Eco6CuttingWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and Eco6CuttingWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] UNION 
                select [İstasyon], [Sonraki İstasyon], SUM([Brüt]) as [Brüt], SUM([Net]) as [Net] from (select S.Code as [İstasyon], (case when P.Eco6LaminationWorkOrder is not null then (select Name from Station where Oid = (select NextStation from Eco6LaminationWorkOrder where Oid = P.Eco6LaminationWorkOrder)) else '' end) as [Sonraki İstasyon], P.GrossQuantity as [Brüt], P.NetQuantity as [Net] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and Eco6LaminationWorkOrder is not null and P.ProductionDate between @begindate and @enddate) T group by [İstasyon], [Sonraki İstasyon] order by [İstasyon], [Sonraki İstasyon]";
                Station.Enabled = false;
                Station.EditValue = null;
                Machine.Enabled = false;
                Machine.EditValue = null;
                ProductKind.Enabled = false;
                ProductKind.EditValue = null;
                WorkShift.Enabled = false;
                ProductCode.Enabled = false;
                ProductCode.EditValue = null;
                OrderNumber.Enabled = false;
                OrderNumber.Text = string.Empty;
                Contact.Enabled = false;
                WorkShift.Enabled = false;
                WorkShift.EditValue = null;
                PaletteNumber.Enabled = false;
                PaletteNumber.Text = string.Empty;
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
                if (reportID == 9)
                {
                    sqlText = string.Format("declare @begindate datetime, @enddate datetime set @begindate = '{0}'  set @enddate = '{1}' ", BeginDate.DateTime.ToString("yyyy-MM-dd HH:mm"), EndDate.DateTime.ToString("yyyy-MM-dd HH:mm")) + sqlText;
                }
                else
                {
                    if (BeginDate.DateTime != null) where += string.Format(" and P.ProductionDate >= '{0}'", BeginDate.DateTime.ToString("yyyy-MM-dd HH:mm"));
                    if (EndDate.DateTime != null) where += string.Format(" and P.ProductionDate <= '{0}'", EndDate.DateTime.ToString("yyyy-MM-dd HH:mm"));
                    if (Station.EditValue != null) where += string.Format(" and M.Station = '{0}'", Station.EditValue);
                    if (Machine.EditValue != null) where += string.Format(" and P.Machine = '{0}'", Machine.EditValue);
                    if (ProductKind.EditValue != null)
                    {
                        var productKind = objectSpace.FindObject<ProductKind>(new BinaryOperator("Oid", ProductKind.EditValue));
                        if (productKind != null) where += string.Format(" and PR.ProductKind = '{0}'", productKind.Oid);
                    }
                    if (WorkShift.EditValue != null) where += string.Format(" and WS.Oid = '{0}'", WorkShift.EditValue);
                    if (ProductCode.EditValue != null) where += string.Format(" and PR.Oid = '{0}'", ProductCode.EditValue);
                    if (!string.IsNullOrEmpty(OrderNumber.Text)) where += string.Format(" and O.OrderNumber = '{0}'", OrderNumber.Text);
                    if (Contact.EditValue != null) where += string.Format(" and O.Contact = '{0}'", Contact.EditValue);
                    if (!string.IsNullOrEmpty(PaletteNumber.Text)) where += string.Format(" and PP.PaletteNumber = '{0}'", PaletteNumber.Text);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
                adapter.Fill(dt);
                gridControl1.DataSource = dt;

                if (gridView1.Columns["Brüt"] != null)
                {
                    gridView1.Columns["Brüt"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView1.Columns["Brüt"].DisplayFormat.FormatString = "n2";
                    if (gridView1.Columns["Brüt"].Summary.Count == 0)
                        gridView1.Columns["Brüt"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Brüt", "{0:n2}");
                }
                if (gridView1.Columns["Net"] != null)
                {
                    gridView1.Columns["Net"].DisplayFormat.FormatType = FormatType.Numeric;
                    gridView1.Columns["Net"].DisplayFormat.FormatString = "n2";
                    if (gridView1.Columns["Net"].Summary.Count == 0)
                        gridView1.Columns["Net"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Net", "{0:n2}");
                }

                if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            }
        }
    }
}
