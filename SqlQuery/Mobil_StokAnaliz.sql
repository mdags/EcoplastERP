select Oid, Code, Name from ProductGroup where GCRecord is null and Code = 'SM' or Code = 'OM'

select PT.Oid, PT.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductGroup = '76350298-ED9D-427B-9B24-1F0DD7C2524C' group by PT.Oid, PT.Name order by cQuantity desc

select PK.Oid, PK.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductType = '76048E86-B7E8-4A4D-BEA2-B3221ECD91B3' group by PK.Oid, PK.Name order by cQuantity desc

select top 10 C.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductGroup = '76350298-ED9D-427B-9B24-1F0DD7C2524C' and C.Code is not null and C.Name not like 'ECO%' group by C.Name order by cQuantity desc

select top 10 C.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductKind = '76048E86-B7E8-4A4D-BEA2-B3221ECD91B3' and C.Code is not null and C.Name not like 'ECO%' group by C.Name order by cQuantity desc