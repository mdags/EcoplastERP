select CAST(S.StoreDate as date) as [Tarih], O.OrderNumber+'/'+cast(SD.LineNumber as varchar(5)) as [Sipari� No], PG.Name as [�r�n Grubu], PT.Name as [�r�n Tipi], PK.Name as [�r�n Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Ad�], W.Code as [Depo Kodu], W.Name as [Depo Ad�], WC.Name as [H�cre], S.PaletteNumber as [Palet Numaras�], SUM(S.Quantity) as [Miktar], U.Code as [Birim], SUM(S.cQuantity) as [�evrim Miktar�], CU.Code as [�evrim Birimi] from StorePast S inner join Product P on P.Oid = S.Product left outer join SalesOrderDetail SD on SD.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = SD.SalesOrder inner join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell inner join Unit U on U.Oid = S.Unit inner join Unit CU on CU.Oid = S.cUnit inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where S.GCRecord is null group by CAST(S.StoreDate as date), O.OrderNumber, SD.LineNumber, PG.Name, PT.Name, PK.Name, P.Code, P.Name, W.Code, W.Name, WC.Name, S.PaletteNumber, U.Code, CU.Code