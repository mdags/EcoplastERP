--select Barcode, cQuantity, GrossQuantity from Production where GCRecord is null and ProductionPalette = (select Oid from ProductionPalette where PaletteNumber = 'P1741300401') and Barcode not in (select Barcode from Store where PaletteNumber = 'P1741300401')

--update Store set Product = '5DACC4CB-5D87-4574-A1A7-A6324B0050CA', SalesOrderDetail = '64E7D104-9F7F-4DBC-A517-76E8D98811EE', PaletteNumber = 'P1741300401', GCRecord = null, Quantity = 50, cQuantity = 50 where Barcode in ('M1741300005','M1741300002','M1741300004', 'M1741300026')

--select Barcode, COUNT(*) as Cnt from Production where GCRecord is null group by Barcode having COUNT(*) > 1 order by Cnt desc
select WorkOrderNumber, Barcode, ProductionDate from Production where Barcode in (select Barcode from Production where GCRecord is null group by Barcode having COUNT(*) > 1) order by Barcode, ProductionDate

--select * from Movement where Barcode = 'M1741300001'
--select * from ProductionPalette where PaletteNumber = 'P1741000509'
