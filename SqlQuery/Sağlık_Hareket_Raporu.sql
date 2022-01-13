select M.DocumentDate as [Hareket Tarihi], T.Name as [Hareket T�r�], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipari� No], PG.Name as [�r�n Grubu], PT.Name as [�r�n Tipi], PK.Name as [�r�n Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Ad�], W.Code as [Depo], WC.Name as [H�cre], M.Quantity as [S�B Miktar], U.Code as [S�B Birim], M.cQuantity as [T�B Miktar], CU.Code as [T�B Birim] from Movement M inner join MovementType T on T.Oid = M.MovementType left outer join SalesOrderDetail D on D.Oid = M.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = M.Product left outer join ProductGroup PG on PG.Oid = P.ProductGroup left outer join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join Warehouse W on W.Oid = M.Warehouse left outer join WarehouseCell WC on WC.Oid = M.WarehouseCell inner join Unit U on U.Oid = M.Unit inner join Unit CU on CU.Oid = M.cUnit where M.GCRecord is null and M.Warehouse in (select Oid from Warehouse where Code in ('900', '901')) order by M.DocumentDate desc