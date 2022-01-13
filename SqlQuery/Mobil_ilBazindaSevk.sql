select City, sum(Quantity) as Quantity, Unit, sum(Total) as Total from (select CT.Name as City, sum(D.cQuantity) as Quantity, U.Code as Unit, sum(D.cQuantity) * (case when SD.CurrencyPrice > 0 then (SD.CurrencyPrice / (case when SD.ExchangeRate > 0 then SD.ExchangeRate else 1 end)) else SD.Price end) as Total from SalesWaybillDetail D inner join SalesWaybill W on W.Oid = D.SalesWaybill inner join SalesOrderDetail SD on SD.Oid = D.SalesOrderDetail inner join Unit U on U.Oid = D.cUnit inner join SalesOrder O on O.Oid = SD.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join City CT on CT.Oid = C.City where D.GCRecord is null and cast(W.WaybillDate as date) between '2017-05-17' and '2017-05-18' group by CT.Name, U.Code, SD.CurrencyPrice, SD.Price, SD.ExchangeRate) T group by City, Unit order by Total desc