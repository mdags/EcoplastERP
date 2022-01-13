select PG.Name as [�r�n Grubu], PT.Name as [�r�n Tipi], PK.Oid as [ProductKindOid], PK.Name as [�r�n Cinsi], PKG.Name as [Sat�� �r�n Grubu] 
, SUM(D.cQuantity) as [Sipari� T�B Miktar�], (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Sevk Edilen T�B Miktar]
, SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Bekleyen T�B Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Depo T�B Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Bask�') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Bask� Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Kesim Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Dilme Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Laminasyon Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu �ekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Cast Depo]
, (SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid)))) - ((select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Bask�') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu �ekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7)))) as [�ekim �retilecek Miktar] 
from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = D.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join ProductKindGroup PKG on PKG.Oid = PK.ProductKindGroup 
where D.GCRecord is null and D.SalesOrderStatus < 200 and (PG.Code = 'OM' or PG.Code = 'SM') and O.SalesOrderType in (1, 5, 6, 7) group by PG.Oid, PG.Name, PT.Oid, PT.Name, PK.Oid, PK.Name, PKG.Name