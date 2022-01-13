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
                set @SalesOrderDetail = '1DB3C1F8-807E-4F34-B6AA-85FD0D9424D6'

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

                select Station as [Ýstasyon], Machine as [Makine], NextStation as [Sonraki Ýstasyon], SequenceNumber as [Sýra No], WorkOrderNumber as [Üretim Sipariþi No], WorkOrderDate as [Üretim Sipariþi Tarihi], Quantity as Miktar, isnull(TotalProduction,0) as [Toplam Üretim], isnull(RemainingProduction,0) as [Kalan Üretim], isnull(TotalWastage,0) as [Toplam Fire], RollCount as [Üretim Adedi] from #WorkOrders order by Station, Machine, SequenceNumber