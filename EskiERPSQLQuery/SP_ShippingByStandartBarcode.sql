USE [PERFECT]
GO

/****** Object:  StoredProcedure [dbo].[SP_ShippingByStandartBarcode]    Script Date: 12.01.2017 10:54:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_ShippingByStandartBarcode] 
	@PLANCODE	varchar(100),
	@BARCODE	varchar(100),
	@ORCODE		nvarchar(100),
	@IsCancel	bit,
	@Status		int,
	@Message	varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    set @Status = 0
	set @Message = 'İşlem Tamamlandı.'

	IF(@IsCancel = 0)
	BEGIN
		IF((SELECT ISNULL(SUM(AMOUNT), 0) AS Total FROM STORES WHERE ORCODE = @ORCODE and STOREID = 121) > (SELECT SUM(AMOUNT) as Total from SHIPPINGPLAN where ORCODE = @ORCODE))
		BEGIN SET @Status = 1 SET @Message = 'Okutulan miktar sevk bildirilen miktardan fazla olamaz.' END

		IF((SELECT TOP 1 MATERIALID from STORES where BARCODE = @BARCODE) != (SELECT TOP 1 MATERIALID FROM ORDERLINES where ORCODE = @ORCODE AND ORDERLINENO = 0))
		BEGIN SET @Status = 1 SET @Message = 'Okutulan mamul ve siparişteki mamul eşleşmiyor.' END

		IF EXISTS (SELECT TOP 1 STORESID FROM STORES where BARCODE = @BARCODE and STOREID = 135)
		BEGIN SET @Status = 1 SET @Message = 'Palet zaten okutulmuş.' END

		IF((SELECT TOP 1 [STATUS] from SHIPPINGPLAN where PLANCODE = @PLANCODE) = 200)
		BEGIN SET @Status = 1 SET @Message = 'Sefer Planı için irsaliye kesilmiş.' END

		IF(@Status = 0)
		BEGIN
			--insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) select NEWID(), @HeaderId, null, GETDATE(), Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, @OutMovementType, Unit, Quantity, cUnit, cQuantity, 0, null from Store where PaletteNumber = @PaletteNumber and GCRecord is null
			INSERT INTO SHIPPINGS(PLANCODE, ORCODE, ORDERLINENO, PALETTEID, BARCODE, CDATE, STORESID, AMOUNT, PRODUCTID)
			SELECT @PLANCODE, ORCODE, ORDERLINENO, PALETTENO, BARCODE, GETDATE(), STORESID, AMOUNT, (SELECT PRODUCTID FROM PRODUCTS WHERE BARCODE = STORES.BARCODE) FROM STORES WHERE BARCODE = @BARCODE AND STOREID = 121 AND STORESID NOT IN (SELECT STORESID FROM SHIPPINGS WHERE PLANCODE = @PLANCODE AND STORESID IS NOT NULL)

			UPDATE STORES SET ORCODE = @ORCODE, STOREID = 135, EDATE = GETDATE() WHERE BARCODE = @BARCODE AND STOREID = 121 
		END
	END
	ELSE
	BEGIN
		 --insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) 
        --select NEWID(), @HeaderId, Null, GETDATE(), Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, (select Oid from Warehouse where Code = '600'), @InMovementType, Unit, Quantity, cUnit, cQuantity, 0, null from Store where PaletteNumber = @PaletteNumber and GCRecord is null
		DECLARE @OLDORCODE	NVARCHAR(100)
		SELECT @OLDORCODE = ORCODE FROM PRODUCTS WHERE PRODUCTID = (SELECT TOP 1 PRODUCTID FROM SHIPPINGS WHERE PLANCODE = @PLANCODE AND BARCODE = @BARCODE AND ORCODE = @ORCODE)
        delete SHIPPINGS where STORESID in (SELECT STORESID FROM STORES WHERE BARCODE = @BARCODE AND STOREID = 135)
		UPDATE STORES SET ORCODE = @OLDORCODE, STOREID = 121, EDATE = GETDATE() WHERE BARCODE = @BARCODE AND STOREID = 135 
	END

	SELECT @Status AS Status, @Message AS Message
END


GO

