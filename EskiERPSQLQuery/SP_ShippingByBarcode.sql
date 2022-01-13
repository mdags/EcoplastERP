USE [PERFECT]
GO

/****** Object:  StoredProcedure [dbo].[SP_ShippingByBarcode]    Script Date: 12.01.2017 10:53:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ShippingByBarcode] 
	@PLANCODE	varchar(100),
	@BARCODE	varchar(100),
	@IsCancel	bit,
	@Status		int,
	@Message	varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	SET @Status = 0
	SET @Message = 'İşlem Tamamlandı.'
	--DECLARE @HEADERID	TABLEID,
			--@MOVETYPE	TABLEID
	--INSERT INTO ACCMOVEHEADERS(CDATE) VALUES(GETDATE()) SET @HEADERID = @@IDENTITY
	--SET @MOVETYPE = 14

	IF(@IsCancel = 0)
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 P.PLANID FROM STORES S inner join SHIPPINGPLAN P ON P.ORCODE = S.ORCODE AND P.ORDERLINENO = S.ORDERLINENO WHERE S.BARCODE = @BARCODE)
		BEGIN SET @status = 1	SET @Message = 'Okutulan barkodun siparişi sefer planında değil.' END

		IF((SELECT ISNULL(SUM(AMOUNT), 0) AS Total FROM STORES WHERE ORCODE = (SELECT TOP 1 ORCODE FROM STORES WHERE BARCODE = @BARCODE) AND ORDERLINENO = (SELECT TOP 1 ORDERLINENO FROM STORES WHERE BARCODE = @BARCODE) and STOREID = 121) > (SELECT SUM(AMOUNT) AS Total FROM SHIPPINGPLAN where ORCODE = (SELECT TOP 1 ORCODE FROM STORES WHERE BARCODE = @BARCODE) AND ORDERLINENO = (SELECT TOP 1 ORDERLINENO FROM STORES WHERE BARCODE = @BARCODE)))
		BEGIN SET @status = 1	SET @Message = 'Okutulan miktar sevk bildirilen miktardan fazla olamaz.' END

		IF EXISTS (SELECT TOP 1 STORESID from STORES where BARCODE = @BARCODE and STOREID = 135)
		BEGIN SET @Status = 1 SET @Message = 'Barkod zaten okutulmuş.' END

		IF((SELECT TOP 1 [STATUS] FROM SHIPPINGPLAN WHERE PLANCODE = @PLANCODE) = 200)
		BEGIN SET @Status = 1 set @Message = 'Sevk Planı için irsaliye kesilmiş.' END

		IF(@Status = 0)
		BEGIN
			--insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) 
			--select NEWID(), @HeaderId, Null, GETDATE(), Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, @OutMovementType, Unit, Quantity, cUnit, cQuantity, 0, null from Store where Barcode = @Barcode and GCRecord is null 

			UPDATE STORES SET STOREID = 135, EDATE = GETDATE() WHERE BARCODE = @BARCODE

			insert into SHIPPINGS(PLANCODE, ORCODE, ORDERLINENO, PALETTEID, BARCODE, CDATE, STORESID, AMOUNT, PRODUCTID)
			select @PLANCODE, ORCODE, ORDERLINENO, PALETTENO, BARCODE, GETDATE(), STORESID, AMOUNT, (SELECT PRODUCTID FROM PRODUCTS WHERE BARCODE = STORES.BARCODE) FROM STORES WHERE BARCODE = @BARCODE AND STORESID NOT IN (SELECT STORESID FROM SHIPPINGS WHERE PLANCODE = @PLANCODE AND STORESID IS NOT NULL)
		END
	END
	ELSE
	BEGIN
		--insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) 
		--select NEWID(), @HeaderId, Null, GETDATE(), Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, @ShippingWarehouse, @OutMovementType, Unit, Quantity, cUnit, cQuantity, 0, null from Store where Barcode = @Barcode and GCRecord is null 

		DELETE SHIPPINGS WHERE STORESID IN (SELECT STORESID FROM STORES WHERE BARCODE = @BARCODE AND STOREID = 135)
		UPDATE STORES SET STOREID = 121, EDATE = GETDATE() WHERE BARCODE = @BARCODE
	END

	SELECT @Status AS Status, @Message AS Message
END


GO

