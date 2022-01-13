USE [MadERP]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenelUrunDurumRaporu]    Script Date: 18.01.2017 10:58:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenelUrunDurumRaporu] 
	@UrunGrubu	nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    IF OBJECT_ID('tempdb..#Report') IS NOT NULL BEGIN DROP TABLE #Report END
create table #Report ([Ürün Grubu] nvarchar(100), [Ürün Tipi] nvarchar(100), [Stok Kodu] nvarchar(100), [Bekleyen Sipariş Miktarı] money, [Baskı Depo] money, [Laminasyon Depo]money, [Dilme Depo] money, [Kesim Depo] money, [Depo Kg] money, [Depo Adet] money, [Ort.Kg] money, [Std.Kg] money, [Sevk Bildirilen] money, [Kamyon Bekliyor] money)

insert into #Report ([Ürün Grubu], [Ürün Tipi], [Stok Kodu], [Bekleyen Sipariş Miktarı], [Baskı Depo], [Laminasyon Depo], [Dilme Depo], [Kesim Depo], [Depo Kg], [Depo Adet], [Ort.Kg], [Std.Kg], [Sevk Bildirilen], [Kamyon Bekliyor])
select G.Name, T.Name, P.Code 
	, isnull((select sum(cQuantity) - (select isnull(SUM(P.NetQuantity), 0) from Loading L inner join Production P on P.Oid = L.Production where L.SalesOrderDetail in (select Oid from SalesOrderDetail where Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and SalesOrderStatus < 200 and SalesOrder not in (select Oid from SalesOrder where GCRecord is null and Contact = '84BC13C4-B004-4ABD-8992-2D2DA92DDF65'))) from SalesOrderDetail where Product = P.Oid and SalesOrderStatus < 200 and SalesOrder not in (select Oid from SalesOrder where GCRecord is null and Contact = '84BC13C4-B004-4ABD-8992-2D2DA92DDF65')),0) as [Bekleyen Sipariş Miktarı]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'BASKI') and Product = P.Oid),0) as [Baskı Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product = P.Oid),0) as [Laminasyon Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product = P.Oid),0) as [Dilme Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse in (select SourceWarehouse from Station where GCRecord is null and (Name = 'Kesim' or Name = 'Balonlu Kesim' or Name = 'Cast Aktarma')) and Product = P.Oid),0) as [Kesim Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product = P.Oid),0) as [Depo Kg]
	, isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product = P.Oid), 0) as [Depo Adet]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product = P.Oid), 0) / (case when isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product = P.Oid), 0) != 0 then isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product = P.Oid), 1) else 1 end) as [Ort. Kg]
    , P.ParcelWeight as [Std.Kg] 
	, isnull((select SUM(P.Quantity) from ShippingPlan P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail where P.GCRecord is null and P.Expedition is null and D.Product = P.Oid and D.SalesOrderStatus < 200), 0) as [Sevk Bildirilen]
	, isnull((select SUM(P.Quantity) from ShippingPlan P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Expedition E on E.Oid = P.Expedition where P.GCRecord is null and P.Expedition is not null and E.ExpeditionStatus = 0 and D.Product = P. Oid and D.SalesOrderStatus < 200), 0) as [Kamyon Bekliyor] 
from Product P left outer join HCategory G on G.Oid = P.ProductGroup left outer join ProductKind K on K.Oid = P.ProductKind inner join ProductType T on T.Oid = P.ProductType
where (K.GCRecord is null) and (P.GCRecord is null) and (K.Code = 1 or K.Code = 2)

insert into #Report ([Ürün Grubu], [Ürün Tipi], [Stok Kodu], [Bekleyen Sipariş Miktarı], [Baskı Depo], [Laminasyon Depo], [Dilme Depo], [Kesim Depo], [Depo Kg], [Depo Adet], [Ort.Kg], [Std.Kg], [Sevk Bildirilen], [Kamyon Bekliyor])
select G.Name, T.Name, K.Name 
	, isnull((select sum(cQuantity) - (select isnull(SUM(P.NetQuantity), 0) from Loading L inner join Production P on P.Oid = L.Production where L.SalesOrderDetail in (select Oid from SalesOrderDetail where Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and SalesOrderStatus < 200 and SalesOrder not in (select Oid from SalesOrder where GCRecord is null and Contact = '84BC13C4-B004-4ABD-8992-2D2DA92DDF65'))) from SalesOrderDetail where Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and SalesOrderStatus < 200 and SalesOrder not in (select Oid from SalesOrder where GCRecord is null and Contact = '84BC13C4-B004-4ABD-8992-2D2DA92DDF65')),0) as [Bekleyen Sipariş Miktarı]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'BASKI') and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Baskı Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Laminasyon Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Dilme Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse in (select SourceWarehouse from Station where GCRecord is null and (Name = 'Kesim' or Name = 'Balonlu Kesim' or Name = 'Cast Aktarma')) and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Kesim Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Depo Kg]
	, isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 0) as [Depo Adet]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 0) / (case when isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 0) != 0 then isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 1) else 1 end) as [Ort. Kg], 0 
	, isnull((select SUM(P.Quantity) from ShippingPlan P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail where P.GCRecord is null and P.Expedition is null and D.Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and D.SalesOrderStatus < 200), 0) as [Sevk Bildirilen]
	, isnull((select SUM(P.Quantity) from ShippingPlan P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Expedition E on E.Oid = P.Expedition where P.GCRecord is null and P.Expedition is not null and E.ExpeditionStatus = 0 and D.Product = P. Oid and D.SalesOrderStatus < 200), 0) as [Kamyon Bekliyor] 
from Product P left outer join HCategory G on G.Oid = P.ProductGroup left outer join ProductKind K on K.Oid = P.ProductKind inner join ProductType T on T.Oid = P.ProductType
where (K.GCRecord is null) and (P.GCRecord is null) and (K.Code = 3 or K.Code = 4) 
group by G.Name, T.Oid, T.Name, K.Oid, K.Name

insert into #Report ([Ürün Grubu], [Ürün Tipi], [Stok Kodu], [Bekleyen Sipariş Miktarı], [Baskı Depo], [Laminasyon Depo], [Dilme Depo], [Kesim Depo], [Depo Kg], [Depo Adet], [Ort.Kg], [Std.Kg], [Sevk Bildirilen], [Kamyon Bekliyor])
select G.Name, T.Name, K.Name 
	, isnull((select sum(cQuantity) - (select isnull(SUM(P.NetQuantity), 0) from Loading L inner join Production P on P.Oid = L.Production where L.SalesOrderDetail in (select Oid from SalesOrderDetail where Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and SalesOrderStatus < 200 and SalesOrder not in (select Oid from SalesOrder where GCRecord is null and Contact = '84BC13C4-B004-4ABD-8992-2D2DA92DDF65'))) from SalesOrderDetail where Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and SalesOrderStatus < 200 and SalesOrder not in (select Oid from SalesOrder where GCRecord is null and Contact = '84BC13C4-B004-4ABD-8992-2D2DA92DDF65')),0) as [Bekleyen Sipariş Miktarı]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'BASKI') and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Baskı Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Laminasyon Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Dilme Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse in (select SourceWarehouse from Station where GCRecord is null and (Name = 'Kesim' or Name = 'Balonlu Kesim' or Name = 'Cast Aktarma')) and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Kesim Depo]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)),0) as [Depo Kg]
	, isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 0) as [Depo Adet]
	, isnull((select sum(Quantity) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 0) / (case when isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 0) != 0 then isnull((select Count(Oid) from Store where GCRecord is null and Warehouse = '94291C4A-1626-4FB0-821A-241D8E85D705' and Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid)), 1) else 1 end) as [Ort. Kg], 0
	, isnull((select SUM(P.Quantity) from ShippingPlan P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail where P.GCRecord is null and P.Expedition is null and D.Product in (select Oid from Product where ProductType = T.Oid and ProductKind = K.Oid) and D.SalesOrderStatus < 200), 0) as [Sevk Bildirilen]
	, isnull((select SUM(P.Quantity) from ShippingPlan P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Expedition E on E.Oid = P.Expedition where P.GCRecord is null and P.Expedition is not null and E.ExpeditionStatus = 0 and D.Product = P. Oid and D.SalesOrderStatus < 200), 0) as [Kamyon Bekliyor] 
from Product P left outer join HCategory G on G.Oid = P.ProductGroup left outer join ProductKind K on K.Oid = P.ProductKind inner join ProductType T on T.Oid = P.ProductType
where (K.GCRecord is null) and (P.GCRecord is null) and (K.Code = 5 or K.Code = 6) 
group by G.Name, T.Oid, T.Name, K.Oid, K.Name

	if(@UrunGrubu = '')
	begin
		select [Ürün Grubu], [Ürün Tipi], [Stok Kodu], sum(case when [Bekleyen Sipariş Miktarı] >= 0 then [Bekleyen Sipariş Miktarı] else 0 end) as  [Bekleyen Sipariş Miktarı], sum([Baskı Depo]) as [Baskı Depo], sum([Laminasyon Depo]) as [Laminasyon Depo], sum([Dilme Depo]) as [Dilme Depo], sum([Kesim Depo]) as [Kesim Depo], sum([Depo Kg]) as [Depo Kg], sum([Depo Adet]) as [Depo Adet], avg([Ort.Kg]) as [Ort.Kg], avg([Std.Kg]) as [Std.Kg], sum([Sevk Bildirilen]) as [Sevk Bildirilen], sum([Kamyon Bekliyor]) as [Kamyon Bekliyor] from #Report where 1 = 1 group by [Ürün Grubu], [Ürün Tipi], [Stok Kodu] order by [Ürün Tipi], [Ürün Grubu]
	end
	else
	begin
		select [Ürün Grubu], [Ürün Tipi], [Stok Kodu], sum(case when [Bekleyen Sipariş Miktarı] >= 0 then [Bekleyen Sipariş Miktarı] else 0 end) as  [Bekleyen Sipariş Miktarı], sum([Baskı Depo]) as [Baskı Depo], sum([Laminasyon Depo]) as [Laminasyon Depo], sum([Dilme Depo]) as [Dilme Depo], sum([Kesim Depo]) as [Kesim Depo], sum([Depo Kg]) as [Depo Kg], sum([Depo Adet]) as [Depo Adet], avg([Ort.Kg]) as [Ort.Kg], avg([Std.Kg]) as [Std.Kg], sum([Sevk Bildirilen]) as [Sevk Bildirilen], sum([Kamyon Bekliyor]) as [Kamyon Bekliyor] from #Report where [Ürün Grubu] = @UrunGrubu group by [Ürün Grubu], [Ürün Tipi], [Stok Kodu] order by [Ürün Tipi], [Ürün Grubu]
	end
END

GO

