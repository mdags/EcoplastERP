select D.Oid, T.DemandNumber, D.LineNumber, P.Name as [Product], U.Code as [Unit], D.Quantity, (case when D.[Priority] = 0 then 'Düþük' when D.[Priority] = 1 then 'Normal' else 'Yüksek' end) as [Priority], E.NameSurname as [CreatedBy] from DemandDetail D inner join Demand T on T.Oid = D.Demand inner join Product P on P.Oid = D.Product inner join Unit U on U.Oid = D.Unit left outer join Employee E on E.Oid = T.CreatedBy where D.GCRecord is null and D.DemandStatus = 1 order by [Priority] desc

update DemandDetail set DemandStatus = 2 where Oid = @oid
update DemandDetail set DemandStatus = 9 where Oid = @oid

update DemandDetail set DemandStatus = 2 where DemandStatus = 1
update DemandDetail set DemandStatus = 9 where DemandStatus = 1