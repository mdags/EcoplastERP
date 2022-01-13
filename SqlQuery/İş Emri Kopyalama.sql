declare	@oid				uniqueidentifier,
		@workordernumber	nvarchar(100),
		@station			uniqueidentifier,
		@machine			uniqueidentifier

set @oid = NEWID()
set @workordernumber = 'C16316023'
set @station = (select Oid from Station where GCRecord is null and Name = 'Cast Çekim')
set @machine = (select Oid from Machine where GCRecord is null and Code = 'SC03')

insert into CastFilmingWorkOrder (Oid, WorkOrderStatus, SequenceNumber, WorkOrderNumber, WorkOrderDate, SalesOrderDetail, 
	WorkName, Station, Machine, NextStation, Unit, Quantity, StickerDesign, ProductionOption, ProductionNote, QualityNote, 
	Width, Height, Thickness, Lenght, BellowsStatus, Bellows, CapStatus, Cap, Corona, CoronaPartial, PrintName, Density, RollWeight, RollDiameter, RollCount, MinimumRollWeight, MaximumRollWeight, WayCount, FilmingWidth, GramM2, GramMetretul, InflationRate, ShippingPackageType, ShippingFilmType, MaterialColor, Embossing, WeighingWithPalette, OuterPacking, Bobbin, Palette, PaletteLayout, PaletteBobbinCount, Reciept, PaletteNumber, PartString, RecieptString, OptimisticLockField, GCRecord)
select @oid, 100, 99, 'CC16324001', GETDATE(), SalesOrderDetail, 
	WorkName, @station, @machine, NextStation, Unit, Quantity, StickerDesign, ProductionOption, ProductionNote, QualityNote, 
	Width, Height, Thickness, Lenght, BellowsStatus, Bellows, CapStatus, Cap, Corona, CoronaPartial, PrintName, Density, RollWeight, RollDiameter, RollCount, MinimumRollWeight, MaximumRollWeight, WayCount, FilmingWidth, GramM2, GramMetretul, InflationRate, ShippingPackageType, ShippingFilmType, MaterialColor, Embossing, WeighingWithPalette, OuterPacking, Bobbin, Palette, PaletteLayout, PaletteBobbinCount, Reciept, PaletteNumber, PartString, RecieptString, OptimisticLockField, GCRecord 
from FilmingWorkOrder where GCRecord is null and WorkOrderNumber = @workordernumber

insert into CastFilmingWorkOrderPart (Oid, CastFilmingWorkOrder, MachinePart, Thickness, OptimisticLockField, GCRecord)
select NEWID(), @oid, MachinePart, Thickness, OptimisticLockField, GCRecord 
from FilmingWorkOrderPart where GCRecord is null and FilmingWorkOrder = (select Oid from FilmingWorkOrder where GCRecord is null and WorkOrderNumber = @workordernumber)

insert into CastFilmingWorkOrderReciept (Oid, CastFilmingWorkOrder, Product, Warehouse, MachinePart, Rate, WorkOrderRate, Unit, Quantity, OptimisticLockField, GCRecord)
select NEWID(), @oid, Product, Warehouse, MachinePart, Rate, WorkOrderRate, Unit, Quantity, OptimisticLockField, GCRecord 
from FilmingWorkOrderReciept where GCRecord is null and FilmingWorkOrder = (select Oid from FilmingWorkOrder where GCRecord is null and WorkOrderNumber = @workordernumber)
