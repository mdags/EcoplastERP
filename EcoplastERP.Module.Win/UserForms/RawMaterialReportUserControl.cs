using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class RawMaterialReportUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;
        public int reportID;

        public RawMaterialReportUserControl()
        {
            InitializeComponent();
        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        private void RawMaterialReportUserControl_Load(object sender, EventArgs e)
        {
            cbProductGroup.Properties.DataSource = new XPCollection<ProductGroup>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Code <> 'OM' or Code <> 'SM'"), new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductGroup.Properties.DisplayMember = "Name";
            cbProductGroup.Properties.ValueMember = "Oid";

            cbProduct.Properties.DataSource = new XPCollection<Product>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("ProductGroup.Code = 'HM' or ProductGroup.Code = 'GR' or ProductGroup.Code = 'KT' or ProductGroup.Code = 'YM'"), new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProduct.Properties.DisplayMember = "Name";
            cbProduct.Properties.ValueMember = "Oid";

            deBeginDate.DateTime = DateTime.Now.AddDays(-1);
            deEndDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sqlText = string.Empty, groupby = string.Empty, where = string.Empty;
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            if (reportID == 1)
            {
                sqlText = string.Format(@"IF OBJECT_ID('tempdb..#Report') IS NOT NULL BEGIN DROP TABLE #Report END
create table #Report ([Müşteri] nvarchar(100), [Sipariş No] nvarchar(100), [Satır No] int, POid uniqueidentifier, [Malzeme Kodu] nvarchar(100), [Malzeme Adı] nvarchar(100), [Üretim Siparişi No] nvarchar(100), [İşin Adı] nvarchar(100), [Brüt Üretim] money, [Tüketim Miktarı] money, [Birim] nvarchar(100), [Oran (%)] money, [Son Üretim] money)

insert into #Report ([Müşteri], [Sipariş No], [Satır No], [POid], [Malzeme Kodu], [Malzeme Adı], [Üretim Siparişi No], [İşin Adı], [Brüt Üretim], [Tüketim Miktarı], [Birim], [Oran (%)], [Son Üretim])
select C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, F.WorkOrderNumber, F.WorkName, (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = F.WorkOrderNumber), (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = F.WorkOrderNumber) * (select sum(Rate) from FilmingWorkOrderReciept where GCRecord is null and FilmingWorkOrder = F.Oid and Product = P.Oid) / 100, U.Code, (select sum(Rate) from FilmingWorkOrderReciept where GCRecord is null and FilmingWorkOrder = F.Oid and Product = P.Oid) as Rate, (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and ProductionDate <= '{1}' and IsLastProduction = 1) as LastProduction from FilmingWorkOrder F inner join FilmingWorkOrderReciept R on R.FilmingWorkOrder = F.Oid and R.GCRecord is null inner join Product P on P.Oid = R.Product inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Unit U on U.Oid = R.Unit inner join Production Pr on Pr.WorkOrderNumber = F.WorkOrderNumber and Pr.GCRecord is null where F.GCRecord is null and Pr.ProductionDate between '{0}' and '{1}' group by C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, D.Oid, F.Oid, F.WorkOrderNumber, F.WorkName, U.Code order by F.WorkOrderNumber 

insert into #Report ([Müşteri], [Sipariş No], [Satır No], [POid], [Malzeme Kodu], [Malzeme Adı], [Üretim Siparişi No], [İşin Adı], [Brüt Üretim], [Tüketim Miktarı], [Birim], [Oran (%)], [Son Üretim])
select C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, F.WorkOrderNumber, F.WorkName, (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = F.WorkOrderNumber), (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = F.WorkOrderNumber) * (select sum(Rate) from CastFilmingWorkOrderReciept where GCRecord is null and CastFilmingWorkOrder = F.Oid and Product = P.Oid) / 100, U.Code, (select sum(Rate) from CastFilmingWorkOrderReciept where GCRecord is null and CastFilmingWorkOrder = F.Oid and Product = P.Oid) as Rate, (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and ProductionDate <= '{1}' and IsLastProduction = 1) as LastProduction from CastFilmingWorkOrder F inner join CastFilmingWorkOrderReciept R on R.CastFilmingWorkOrder = F.Oid and R.GCRecord is null inner join Product P on P.Oid = R.Product inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Unit U on U.Oid = R.Unit inner join Production Pr on Pr.WorkOrderNumber = F.WorkOrderNumber and Pr.GCRecord is null where F.GCRecord is null and Pr.ProductionDate between '{0}' and '{1}' group by C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, D.Oid, F.Oid, F.WorkOrderNumber, F.WorkName, U.Code order by F.WorkOrderNumber 

insert into #Report ([Müşteri], [Sipariş No], [Satır No], [POid], [Malzeme Kodu], [Malzeme Adı], [Üretim Siparişi No], [İşin Adı], [Brüt Üretim], [Tüketim Miktarı], [Birim], [Oran (%)], [Son Üretim])
select C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, F.WorkOrderNumber, F.WorkName, (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = F.WorkOrderNumber), (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = F.WorkOrderNumber) * (select sum(Rate) from BalloonFilmingWorkOrderReciept where GCRecord is null and BalloonFilmingWorkOrder = F.Oid and Product = P.Oid) / 100, U.Code, (select sum(Rate) from BalloonFilmingWorkOrderReciept where GCRecord is null and BalloonFilmingWorkOrder = F.Oid and Product = P.Oid) as Rate, (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and ProductionDate <= '{1}' and IsLastProduction = 1) as LastProduction from BalloonFilmingWorkOrder F inner join BalloonFilmingWorkOrderReciept R on R.BalloonFilmingWorkOrder = F.Oid and R.GCRecord is null inner join Product P on P.Oid = R.Product inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Unit U on U.Oid = R.Unit inner join Production Pr on Pr.WorkOrderNumber = F.WorkOrderNumber and Pr.GCRecord is null where F.GCRecord is null and Pr.ProductionDate between '{0}' and '{1}' group by C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, D.Oid, F.Oid, F.WorkOrderNumber, F.WorkName, U.Code order by F.WorkOrderNumber 

select [Müşteri], [Sipariş No], [Satır No], [POid], [Malzeme Kodu], [Malzeme Adı], [Üretim Siparişi No], [İşin Adı], [Brüt Üretim], sum([Tüketim Miktarı]) as [Tüketim Miktarı], [Birim], sum([Oran (%)]) as [Oran (%)], [Son Üretim] from #Report where [Brüt Üretim] is not null ", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));
                groupby = @"group by [Müşteri], [Sipariş No], [Satır No], [POid], [Malzeme Kodu], [Malzeme Adı], [Üretim Siparişi No], [İşin Adı], [Birim], [Brüt Üretim], [Son Üretim] ";
            }
            if (reportID == 2)
            {
                sqlText = string.Format(@"declare @begindate date, @enddate date 
                    set @begindate = '{0}' set @enddate = '{1}' 
                    select C.Name as [Müşteri], O.OrderNumber as [Sipariş No], D.LineNumber as [Satır No], P.Oid as POid, P.Code as [Malzeme Kodu], P.Name as [Malzeme Adı], M.DocumentNumber as [Üretim Siparişi No], (select SUM(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = M.DocumentNumber and cast(ProductionDate as date) between @begindate and @enddate) as [Net Üretim], (case when M.MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P111') then sum(M.Quantity) else 0 end) - (case when M.MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P114') then sum(M.Quantity) else 0 end) as [Tüketim Miktarı], ((case when M.MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P111') then sum(M.Quantity) else 0 end) - (case when M.MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P114') then sum(M.Quantity) else 0 end)) * 100 / (select SUM(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = M.DocumentNumber and cast(ProductionDate as date) between @begindate and @enddate) as [Oran(%)], U.Code as [Birimi] from Movement M inner join Product P on P.Oid = M.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join Unit U on U.Oid = M.Unit inner join SalesOrderDetail D on D.Oid = M.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where M.GCRecord is null and P.ProductGroup not in (select Oid from ProductGroup where GCRecord is null and Code in ('OM', 'SM')) and cast(M.DocumentDate as date) between @begindate and @enddate ", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));
                groupby = @" group by C.Name, O.OrderNumber, D.LineNumber, P.Oid, P.Code, P.Name, M.DocumentNumber, U.Code, M.MovementType having ((case when M.MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P111') then sum(M.Quantity) else 0 end) - (case when M.MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P114') then sum(M.Quantity) else 0 end)) > 0 order by M.DocumentNumber ";
            }

            where = string.Empty;
            if (reportID == 2)
            {
                if (!string.IsNullOrEmpty(cbProductGroup.EditValue.ToString()))
                {
                    string list = string.Empty;
                    foreach (string item in cbProductGroup.EditValue.ToString().Split(','))
                    {
                        list += string.Format("'{0}',", item.Trim());
                    }
                    where += string.Format(" and PG.Oid in ({0})", list.Substring(0, list.Length - 1));
                }
            }
            if (!string.IsNullOrEmpty(cbProduct.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProduct.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                if (reportID == 1)
                    where += string.Format(" and POid in ({0})", list.Substring(0, list.Length - 1));
                else if (reportID == 2)
                    where += string.Format(" and P.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                string list = string.Empty;
                foreach (string item in txtWorkOrderNumber.Text.Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and [Üretim Siparişi No] in ({0})", list.Substring(0, list.Length - 1));
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(dt);
            dt.Columns["POid"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Brüt Üretim"] != null)
            {
                gridView1.Columns["Brüt Üretim"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Brüt Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Brüt Üretim"].Summary.Count == 0)
                    gridView1.Columns["Brüt Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Brüt Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Tüketim Miktarı"] != null)
            {
                gridView1.Columns["Tüketim Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Tüketim Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Tüketim Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Tüketim Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Tüketim Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["Oran (%)"] != null)
            {
                gridView1.Columns["Oran (%)"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Oran (%)"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Oran (%)"].Summary.Count == 0)
                    gridView1.Columns["Oran (%)"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Oran (%)", "{0:n2}");
            }
            if (gridView1.Columns["Son Üretim"] != null)
            {
                gridView1.Columns["Son Üretim"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Son Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Son Üretim"].Summary.Count == 0)
                    gridView1.Columns["Son Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Son Üretim", "{0:n2}");
            }

            if (gridView1.Columns["Net Üretim"] != null)
            {
                gridView1.Columns["Net Üretim"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Net Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Net Üretim"].Summary.Count == 0)
                    gridView1.Columns["Net Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Net Üretim", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }
    }
}
