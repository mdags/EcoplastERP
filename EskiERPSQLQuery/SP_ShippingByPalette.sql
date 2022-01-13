USE [PERFECT]
GO

/****** Object:  StoredProcedure [dbo].[SP_ShippingByPalette]    Script Date: 12.01.2017 10:53:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ShippingByPalette] 
	@PLANCODE	varchar(100),
	@PALETTEID	int,
	@IsCancel	bit,
	@Status		int,
	@Message	varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	set @Status = 0
	set @Message = 'İşlem Tamamlandı.'
	--declare @HEADERID	TABLEID,
			--@MOVETYPE	TABLEID
	--INSERT INTO ACCMOVEHEADERS(CDATE) VALUES(GETDATE()) SET @HEADERID = @@IDENTITY
	--SET @MOVETYPE = 14

	IF(@IsCancel = 0)
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 P.PLANID FROM STORES S inner join SHIPPINGPLAN P ON P.ORCODE = S.ORCODE AND P.ORDERLINENO = S.ORDERLINENO WHERE S.PALETTENO = @PALETTEID)
		BEGIN SET @status = 1	SET @Message = 'Okutulan paletin siparişi sefer planında değil.' END

		IF((SELECT ISNULL(SUM(AMOUNT), 0) AS Total FROM STORES WHERE ORCODE = (SELECT TOP 1 ORCODE FROM STORES WHERE PALETTENO = @PALETTEID) AND ORDERLINENO = (SELECT TOP 1 ORDERLINENO FROM STORES WHERE PALETTENO = @PALETTEID) and STOREID = 121) > (SELECT SUM(AMOUNT) AS Total FROM SHIPPINGPLAN where ORCODE = (SELECT TOP 1 ORCODE FROM STORES WHERE PALETTENO = @PALETTEID) AND ORDERLINENO = (SELECT TOP 1 ORDERLINENO FROM STORES WHERE PALETTENO = @PALETTEID)))
		BEGIN SET @status = 1	SET @Message = 'Okutulan miktar sevk bildirilen miktardan fazla olamaz.' END

		IF EXISTS (SELECT TOP 1 STORESID from STORES where PALETTENO = @PALETTEID and STOREID = 135)
		BEGIN SET @Status = 1 SET @Message = 'Palet zaten okutulmuş.' END

		IF((SELECT TOP 1 [STATUS] FROM SHIPPINGPLAN WHERE PLANCODE = @PLANCODE) = 200)
		BEGIN SET @Status = 1 SET @Message = 'Sevk Planı için irsaliye kesilmiş.' END

		IF(@Status = 0)
		BEGIN
			--insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) 
			--select NEWID(), @HeaderId, Null, GETDATE(), Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, @OutMovementType, Unit, Quantity, cUnit, cQuantity, 0, null from Store where PaletteNumber = @PaletteNumber and GCRecord is null 
			insert into SHIPPINGS(PLANCODE, ORCODE, ORDERLINENO, PALETTEID, BARCODE, CDATE, STORESID, AMOUNT, PRODUCTID)
			select @PLANCODE, ORCODE, ORDERLINENO, PALETTENO, BARCODE, GETDATE(), STORESID, AMOUNT, (SELECT PRODUCTID FROM PRODUCTS WHERE BARCODE = STORES.BARCODE) FROM STORES WHERE PALETTENO = @PALETTEID AND STOREID = 121 AND STORESID NOT IN (SELECT STORESID FROM SHIPPINGS WHERE PLANCODE = @PLANCODE AND STORESID IS NOT NULL)

			UPDATE STORES SET STOREID = 135, EDATE = GETDATE() WHERE PALETTENO = @PALETTEID AND STOREID = 121 
		END
	END
	ELSE
	BEGIN
		--insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) 
		--select NEWID(), @HeaderId, Null, GETDATE(), Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, @ShippingWarehouse, @OutMovementType, Unit, Quantity, cUnit, cQuantity, 0, null from Store where PaletteNumber = @PaletteNumber and GCRecord is null 

		delete SHIPPINGS where STORESID in (SELECT STORESID FROM STORES WHERE PALETTENO = @PALETTEID AND STOREID = 135)
		UPDATE STORES SET STOREID = 121, EDATE = GETDATE() WHERE PALETTENO = @PALETTEID AND STOREID = 135 
	END

	SELECT @Status AS Status, @Message AS Message
END



GO

