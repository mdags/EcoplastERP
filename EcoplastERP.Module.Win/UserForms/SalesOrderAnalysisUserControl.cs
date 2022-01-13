using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class SalesOrderAnalysisUserControl : XtraUserControl, IComplexControl
    {
        public SalesOrderAnalysisUserControl()
        {
            InitializeComponent();
        }
        private IObjectSpace objectSpace;
        private XafApplication application;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
            this.application = application;

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select (case when D.SalesOrderStatus = 0 then 'Planlama Onayı Bekliyor' when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Eco1 Bekliyor' when D.SalesOrderStatus = 102 then 'Eco1 Laminasyon Bekliyor' when D.SalesOrderStatus = 103 then 'Eco2 Bekliyor' when D.SalesOrderStatus = 104 then 'Eco3 Bekliyor' when D.SalesOrderStatus = 105 then 'Eco4 Bekliyor' when D.SalesOrderStatus = 106 then 'Eco4 Dilme Bekliyor' when D.SalesOrderStatus = 107 then 'Eco5 Cpp Bekliyor' when D.SalesOrderStatus = 108 then 'Eco5 Stretch Bekliyor' when D.SalesOrderStatus = 109 then 'Eco5 Aktarma Bekliyor' when D.SalesOrderStatus = 110 then 'Eco5 Dilme Bekliyor' when D.SalesOrderStatus = 111 then 'Eco5 Rejenere Bekliyor' when D.SalesOrderStatus = 112 then 'Eco6 Bekliyor' when D.SalesOrderStatus = 113 then 'Eco6 Kesim Bekliyor' when D.SalesOrderStatus = 114 then 'Eco6 Laminasyon Bekliyor' when D.SalesOrderStatus = 120 then 'Üretim Bekliyor' when D.SalesOrderStatus = 130 then 'Sevkiyat Bekliyor' when D.SalesOrderStatus = 131 then 'Yükleme Bekliyor' when D.SalesOrderStatus = 200 then 'Sevk Edildi' when D.SalesOrderStatus = 900 then 'İptal Edildi' else '' end) as Durumu, O.OrderNumber as [Sipariş Numarası], D.LineNumber as [Pozisyon], O.OrderDate as [Sipariş Tarihi], C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], P.Name as [Malzeme Adı] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = D.Product where D.GCRecord is null",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            sleSalesOrder.Properties.DataSource = dt;
            sleSalesOrder.Properties.DisplayMember = "Sipariş Numarası";
            sleSalesOrder.Properties.ValueMember = "Sipariş Numarası";
            sleSalesOrder.ForceInitialize();

            RefreshGrid("xxx");
        }
        void IComplexControl.Refresh()
        {
            RefreshGrid("xxx");
        }
        public void RefreshGrid(string orderNumber)
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select D.Oid, D.LineNumber as Pozisyon, (case when D.SalesOrderStatus = 0 then 'Planlama Onayı Bekliyor' when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Eco1 Bekliyor' when D.SalesOrderStatus = 102 then 'Eco1 Laminasyon Bekliyor' when D.SalesOrderStatus = 103 then 'Eco2 Bekliyor' when D.SalesOrderStatus = 104 then 'Eco3 Bekliyor' when D.SalesOrderStatus = 105 then 'Eco4 Bekliyor' when D.SalesOrderStatus = 106 then 'Eco4 Dilme Bekliyor' when D.SalesOrderStatus = 107 then 'Eco5 Cpp Bekliyor' when D.SalesOrderStatus = 108 then 'Eco5 Stretch Bekliyor' when D.SalesOrderStatus = 109 then 'Eco5 Aktarma Bekliyor' when D.SalesOrderStatus = 110 then 'Eco5 Dilme Bekliyor' when D.SalesOrderStatus = 111 then 'Eco5 Rejenere Bekliyor' when D.SalesOrderStatus = 112 then 'Eco6 Bekliyor' when D.SalesOrderStatus = 113 then 'Eco6 Kesim Bekliyor' when D.SalesOrderStatus = 114 then 'Eco6 Laminasyon Bekliyor' when D.SalesOrderStatus = 120 then 'Üretim Bekliyor' when D.SalesOrderStatus = 130 then 'Sevkiyat Bekliyor' when D.SalesOrderStatus = 131 then 'Yükleme Bekliyor' when D.SalesOrderStatus = 200 then 'Sevk Edildi' when D.SalesOrderStatus = 900 then 'İptal Edildi' else '' end) as Durumu, D.LineDeliveryDate as [Teslim Tarihi], P.Code as [Stok Kodu], D.Quantity as [Sipariş Miktarı], (select SUM(GrossQuantity) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) as [Sipariş Üretilen], (select SUM(Quantity) from ShippingPlan where GCRecord is null and SalesOrderDetail = D.Oid) as [Sevk Bildirilen], (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen SÖB Miktar], D.Quantity - (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Kalan], (select isnull(SUM(Quantity),0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and SalesOrderDetail = D.Oid) as [Sevk Depo], P.Name as [Stok Açıklama] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = D.Product where O.OrderNumber = '{0}' And D.GCRecord is null group by D.Oid, D.LineNumber, D.SalesOrderStatus, D.LineDeliveryDate, P.Code, D.Quantity, P.Name order by D.LineNumber", orderNumber), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            gridControl1.DataSource = data;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();
            if (gridView1.Columns["Oid"] != null) gridView1.Columns["Oid"].Visible = false;
            if (gridView1.Columns.Count > 0)
            {
                gridView1.Columns["Oid"].Visible = false;
                gridView1.Columns["Sipariş Miktarı"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sipariş Miktarı"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Sipariş Üretilen"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sipariş Üretilen"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Sevk Bildirilen"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Bildirilen"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Kalan"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Kalan"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Sevk Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Depo"].DisplayFormat.FormatString = "n2";
            }
            if (gridView1.FocusedValue != null)
            {
                GetWorkOrder(gridView1.GetFocusedRowCellValue("Oid"));
                GetStore(gridView1.GetFocusedRowCellValue("Oid"));
                GetShippingPlan(gridView1.GetFocusedRowCellValue("Oid"));
                GetOtherOrder();
            }
        }
        void GetWorkOrder(object salesOrderDetail)
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"
                IF OBJECT_ID('tempdb..#WorkOrders') IS NOT NULL BEGIN DROP TABLE #WorkOrders END
                create table #WorkOrders
                (
	                Station				varchar(100),
	                Machine				varchar(100),
					NextStation			nvarchar(100),
	                SequenceNumber		int,
	                WorkOrderNumber		varchar(100),
	                WorkOrderDate		datetime,
	                Quantity			money,
	                TotalProduction		money,
                    TotalWastage		money,
	                RemainingProduction	money,
	                RollCount			int
                )

                declare @SalesOrderDetail uniqueidentifier
                set @SalesOrderDetail = '{0}'

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then  (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen], W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then  (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from FilmingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from CastFilmingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from CastTransferingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from BalloonFilmingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
               (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from PrintingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
               (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from LaminationWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from SlicingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from CastSlicingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from CuttingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from RegeneratedWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from CastRegeneratedWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber 
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from Eco6WorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from Eco6CuttingWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                insert into #WorkOrders (Station, Machine, NextStation, SequenceNumber, WorkOrderNumber, WorkOrderDate, Quantity, TotalProduction, RemainingProduction, TotalWastage, RollCount)
                select S.Name, M.Code, NS.Name, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, W.Quantity, 
                (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end) as [Üretilen],
                (case when (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) >= 0 then (W.Quantity - (case when (W.NextStation not in (select Oid from Station where IslastStation = 0)) then 
	                (select sum(NetQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) 
	                else (select sum(GrossQuantity) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) end)) else 0 end) as [Kalan Üretim], 
	                (select sum(GrossQuantity) from Wastage where WorkOrderNumber = W.WorkOrderNumber) as [Fire], 
		                (select COUNT(WorkOrderNumber) from Production where SalesOrderDetail = @SalesOrderDetail and GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as RollCount 
                from Eco6LaminationWorkOrder W inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Station NS on NS.Oid = W.NextStation left outer join Wastage Ws on W.WorkOrderNumber = Ws.WorkOrderNumber  
                where W.GCRecord is null And W.SalesOrderDetail = @SalesOrderDetail 
                group by W.SequenceNumber, NS.Name, W.WorkOrderNumber, W.WorkOrderDate, W.NextStation, S.Name, M.Code, W.Quantity 

                select Station as [İstasyon], Machine as [Makine], NextStation as [Sonraki İstasyon], SequenceNumber as [Sıra No], WorkOrderNumber as [Üretim Siparişi No], WorkOrderDate as [Üretim Siparişi Tarihi], Quantity as Miktar, isnull(TotalProduction,0) as [Toplam Üretim], isnull(RemainingProduction,0) as [Kalan Üretim], isnull(TotalWastage,0) as [Toplam Fire], RollCount as [Üretim Adedi] from #WorkOrders order by Station, Machine, SequenceNumber", salesOrderDetail), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            gridControlWorkOrders.DataSource = data;
            gridControlWorkOrders.ForceInitialize();
            gridViewWorkOrders.BestFitColumns();
            gridViewWorkOrders.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewWorkOrders.Columns["Miktar"].DisplayFormat.FormatString = "n2";
            gridViewWorkOrders.Columns["Toplam Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewWorkOrders.Columns["Toplam Üretim"].DisplayFormat.FormatString = "n2";
            gridViewWorkOrders.Columns["Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewWorkOrders.Columns["Kalan Üretim"].DisplayFormat.FormatString = "n2";
            gridViewWorkOrders.Columns["Toplam Fire"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewWorkOrders.Columns["Toplam Fire"].DisplayFormat.FormatString = "n2";
        }
        void GetStore(object salesOrderDetail)
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select D.LineNumber as [Pozisyon], P.Code as [Malzeme Kodu], P.Name as [Malzeme Adı], S.PaletteNumber as [Palet Numarası], W.Code as [Depo Kodu], sum(S.Quantity) as [SÖB Miktar], U.Code as [SÖB Birim], SUM(S.cQuantity) as [TÖB Miktar], CU.Code as [TÖB Birim], COUNT(S.Oid) as [Rl/Pk Adedi] from Store S inner join SalesOrderDetail D on D.Oid = S.SalesOrderDetail inner join Warehouse W on W.Oid = S.Warehouse inner join Unit U on U.Oid = S.Unit inner join Unit CU on CU.Oid = S.cUnit inner join Product P on P.Oid = S.Product where S.GCRecord is null And SalesOrderDetail = '{0}' group by D.LineNumber, P.Code, P.Name, S.PaletteNumber, W.Code, U.Code, CU.Code", salesOrderDetail), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            gridControlStore.DataSource = data;
            gridControlStore.ForceInitialize();
            gridViewStore.BestFitColumns();
            gridViewStore.Columns["SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewStore.Columns["SÖB Miktar"].DisplayFormat.FormatString = "n2";
            gridViewStore.Columns["TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewStore.Columns["TÖB Miktar"].DisplayFormat.FormatString = "n2";
            gridViewStore.Columns["Depo Kodu"].GroupIndex = 0;
        }
        void GetShippingPlan(object salesOrderDetail)
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select (case when P.ShippingPlanStatus = 0 then 'Sefer Bekliyor' when P.ShippingPlanStatus = 1 then 'Sevk Termini Bekliyor' when P.ShippingPlanStatus = 2 then 'Ödeme Problemi Bekliyor' when P.ShippingPlanStatus = 3 then 'Müşteri Aracı Bekliyor' when P.ShippingPlanStatus = 4 then 'Sonraki Ay Fatura Bekliyor' when P.ShippingPlanStatus = 5 then 'Tam Teslimat Bekliyor' when P.ShippingPlanStatus = 6 then 'Stok Üretim Bekliyor' when P.ShippingPlanStatus = 7 then 'Sevk Adresi Bekliyor' when P.ShippingPlanStatus = 8 then 'Yükleme Bekliyor' when P.ShippingPlanStatus = 9 then 'Sevk Edildi' else '' end) as [Durumu], P.ShippingPlanDate as [Sevk Bildirme Tarihi], P.NotifiedUser as [Sevk Bildiren], U.Code as [Birim], P.Quantity as [Miktar], CU.Code as [Çevrim Birimi], P.cQuantity as [Çevrim Miktarı], E.ExpeditionNumber +'/'+CAST(ED.LineNumber as nchar(5)) as [Sefer Numarası], E.ExpeditionDate as [Sefer Tarihi], R.Name as [Rota], T.PlateNumber as [Kamyon], D.NameSurname as [Şoför], D.CellPhone as [Cep Telefonu], DD.Oid as DeliveryDetailOid, DE.DeliveryNumber + '/' + cast(DD.LineNumber as nchar(5)) as [Teslimat Numarası], DE.DeliveryDate as [Teslimat Tarihi] from ShippingPlan P inner join Unit U on U.Oid = P.Unit inner join Unit CU on Cu.Oid = P.cUnit left outer join  ExpeditionDetail ED on ED.Oid = P.ExpeditionDetail inner join Expedition E on E.Oid = ED.Expedition inner join DeliveryDetail DD on DD.Oid = Ed.DeliveryDetail inner join Delivery DE on DE.Oid = DD.Delivery left outer join [Route] R on R.Oid = E.[Route] left outer join Truck T on T.Oid = E.Truck left outer join TruckDriver D on D.Oid = E.TruckDriver where P.GCRecord is null and P.SalesOrderDetail = '{0}'", salesOrderDetail), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            data.Columns["DeliveryDetailOid"].ColumnMapping = MappingType.Hidden;
            gridControlShippingPlan.DataSource = data;
            gridControlShippingPlan.ForceInitialize();
            gridViewShippingPlan.BestFitColumns();
            gridViewShippingPlan.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
            gridViewShippingPlan.Columns["Miktar"].DisplayFormat.FormatString = "n2";
        }
        void GetOtherOrder()
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select D.Oid, O.OrderNumber as [Sipariş Numarası], D.LineNumber as [Pozisyon], (case when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Çekim Bekliyor' when D.SalesOrderStatus = 102 then 'Kesim Bekliyor' when D.SalesOrderStatus = 103 then 'Rejenere Bekliyor' when D.SalesOrderStatus = 105 then 'Baskı Bekliyor' when D.SalesOrderStatus = 106 then 'Cast Aktarma Bekliyor' when D.SalesOrderStatus = 107 then 'Cast Çekim Bekliyor' when D.SalesOrderStatus = 108 then 'Balonlu Çekim Bekliyor' when D.SalesOrderStatus = 109 then 'Katlama Bekliyor' when D.SalesOrderStatus = 111 then 'Balonlu Kesim Bekliyor'  when D.SalesOrderStatus = 112 then 'Dilme Bekliyor'  when D.SalesOrderStatus = 113 then 'Laminasyon Bekliyor' when D.SalesOrderStatus = 104 then 'Üretim Bekliyor' when D.SalesOrderStatus = 110 then 'Sevkiyat Bekliyor' when D.SalesOrderStatus = 120 then 'Yükleme Bekliyor' when D.SalesOrderStatus = 200 then 'Sevk Edildi' when D.SalesOrderStatus = 900 then 'İptal Edildi' else '' end) as Durumu, D.LineDeliveryDate as [Teslim Tarihi], P.Code as [Stok Kodu], D.Quantity as [Sipariş Miktarı], (select SUM(GrossQuantity) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) as [Sipariş Üretilen], (select SUM(Quantity) from ShippingPlan where GCRecord is null and SalesOrderDetail = D.Oid) as [Sevk Bildirilen], (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen SÖB Miktar], (select SUM(GrossQuantity) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) - (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Kalan], (select isnull(SUM(Quantity),0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and SalesOrderDetail = D.Oid) as [Sevk Depo], P.Name as [Stok Açıklama] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product where C.Oid = (select Contact from SalesOrder where GCRecord is null and OrderNumber = '{0}') And D.GCRecord is null And D.SalesOrderStatus < 200 And C.Name not like 'ECO%' group by D.Oid, O.OrderNumber, D.LineNumber, D.SalesOrderStatus, D.LineDeliveryDate, P.Code, D.Quantity, P.Name order by D.LineNumber", sleSalesOrder.EditValue.ToString()), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            gridControlOtherOrder.DataSource = data;
            gridControlOtherOrder.ForceInitialize();
            gridViewOtherOrder.BestFitColumns();
            if (gridViewOtherOrder.Columns.Count > 0)
            {
                gridViewOtherOrder.Columns["Sipariş Numarası"].GroupIndex = 0;
                gridViewOtherOrder.Columns["Oid"].Visible = false;
                gridViewOtherOrder.Columns["Sipariş Miktarı"].DisplayFormat.FormatType = FormatType.Numeric;
                gridViewOtherOrder.Columns["Sipariş Miktarı"].DisplayFormat.FormatString = "n2";
                gridViewOtherOrder.Columns["Sipariş Üretilen"].DisplayFormat.FormatType = FormatType.Numeric;
                gridViewOtherOrder.Columns["Sipariş Üretilen"].DisplayFormat.FormatString = "n2";
                gridViewOtherOrder.Columns["Sevk Bildirilen"].DisplayFormat.FormatType = FormatType.Numeric;
                gridViewOtherOrder.Columns["Sevk Bildirilen"].DisplayFormat.FormatString = "n2";
                gridViewOtherOrder.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridViewOtherOrder.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatString = "n2";
                gridViewOtherOrder.Columns["Kalan"].DisplayFormat.FormatType = FormatType.Numeric;
                gridViewOtherOrder.Columns["Kalan"].DisplayFormat.FormatString = "n2";
                gridViewOtherOrder.Columns["Sevk Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridViewOtherOrder.Columns["Sevk Depo"].DisplayFormat.FormatString = "n2";
            }
        }

        private void sleSalesOrder_EditValueChanged(object sender, EventArgs e)
        {
            if (sleSalesOrder.EditValue != null)
            {
                RefreshGrid(sleSalesOrder.EditValue.ToString());
                SalesOrder salesOrder = objectSpace.FindObject<SalesOrder>(new BinaryOperator("OrderNumber", sleSalesOrder.EditValue.ToString()));
                if (salesOrder != null)
                {
                    if (salesOrder.Contact != null)
                    {
                        txtContact.Text = salesOrder.Contact.Name;
                        txtShippingAddress.Text = salesOrder.ShippingContact != null ? salesOrder.ShippingContact.Address : string.Empty;
                        if (salesOrder.Contact.City != null) txtCity.Text = salesOrder.Contact.City.Name;
                    }
                    txtSalesOrderType.Text =
                          salesOrder.SalesOrderType == SalesOrderType.CustomerOrder ? "Müşteri Siparişi" :
                          salesOrder.SalesOrderType == SalesOrderType.PlanningOrder ? "Planlama Siparişi" :
                          salesOrder.SalesOrderType == SalesOrderType.A3Order ? "A3 Siparişi" :
                          salesOrder.SalesOrderType == SalesOrderType.RegeneratedOrder ? "Rejenere Siparişi" :
                          salesOrder.SalesOrderType == SalesOrderType.ExportingOrder ? "İhracat Sipariş" :
                          salesOrder.SalesOrderType == SalesOrderType.ExportRegisteredOrder ? "İhracat Sipariş" : string.Empty;
                    if (salesOrder.OrderDate != null) txtSalesOrderDate.Text = salesOrder.OrderDate.ToShortDateString();
                }
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            GetWorkOrder(gridView1.GetFocusedRowCellValue("Oid"));
            GetStore(gridView1.GetFocusedRowCellValue("Oid"));
            GetShippingPlan(gridView1.GetFocusedRowCellValue("Oid"));
        }

        private void gridViewShippingPlan_DoubleClick(object sender, System.EventArgs e)
        {
            if (gridViewShippingPlan.FocusedValue == null) return;
            DeliveryDetail deliveryDetail = objectSpace.FindObject<DeliveryDetail>(new BinaryOperator("Oid", Guid.Parse(gridViewShippingPlan.GetFocusedRowCellValue("DeliveryDetailOid").ToString())));

            if (deliveryDetail != null)
            {
                IObjectSpace dvObjectSpace = application.CreateObjectSpace();
                ShowViewParameters showViewParams = new ShowViewParameters
                {
                    CreatedView = application.CreateDetailView(dvObjectSpace, dvObjectSpace.GetObject(deliveryDetail)),
                    TargetWindow = TargetWindow.NewModalWindow,
                    Context = TemplateContext.PopupWindow
                };
                application.ShowViewStrategy.ShowView(showViewParams, new ShowViewSource(null, null));
            }
        }
    }
}
