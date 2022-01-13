using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DefaultClassOptions]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]
    public class ChangeProductionPaletteParameters : BaseObject
    {
        public ChangeProductionPaletteParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            var productionPalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", NewPaletteNumber));
            if (productionPalette != null)
            {
                var loading = Session.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("PaletteNumber = ?", NewPaletteNumber));
                if (loading == null)
                {
                    var store = Session.FindObject<Store>(new BinaryOperator("Barcode", Barcode));
                    if (store != null)
                    {
                        SalesOrderDetail newSalesOrderDetail = null;
                        SalesOrderDetail salesOrderDetail = store.SalesOrderDetail;
                        var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (filmingWorkOrder != null) newSalesOrderDetail = filmingWorkOrder.SalesOrderDetail;

                        var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castFilmingWorkOrder != null) newSalesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;

                        var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castTransferingWorkOrder != null) newSalesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;

                        var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (balloonFilmingWorkOrder != null) newSalesOrderDetail = balloonFilmingWorkOrder.SalesOrderDetail;

                        var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (printingWorkOrder != null) newSalesOrderDetail = printingWorkOrder.SalesOrderDetail;

                        var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (laminationWorkOrder != null) newSalesOrderDetail = laminationWorkOrder.SalesOrderDetail;

                        var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (slicingWorkOrder != null) newSalesOrderDetail = slicingWorkOrder.SalesOrderDetail;

                        var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castSlicingWorkOrder != null) newSalesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;

                        var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (cuttingWorkOrder != null) newSalesOrderDetail = cuttingWorkOrder.SalesOrderDetail;

                        var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (foldingWorkOrder != null) newSalesOrderDetail = foldingWorkOrder.SalesOrderDetail;

                        var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (balloonCuttingWorkOrder != null) newSalesOrderDetail = balloonCuttingWorkOrder.SalesOrderDetail;

                        var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (regeneratedWorkOrder != null) newSalesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;

                        var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castRegeneratedWorkOrder != null) newSalesOrderDetail = castRegeneratedWorkOrder.SalesOrderDetail;

                        var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (eco6WorkOrder != null) newSalesOrderDetail = eco6WorkOrder.SalesOrderDetail;

                        var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (eco6CuttingWorkOrder != null) newSalesOrderDetail = eco6CuttingWorkOrder.SalesOrderDetail;

                        var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (eco6LaminationWorkOrder != null) newSalesOrderDetail = eco6LaminationWorkOrder.SalesOrderDetail;

                        if (newSalesOrderDetail == salesOrderDetail)
                        {
                            var headerId = Guid.NewGuid();

                            Product product = store.Product;
                            Warehouse warehouse = store.Warehouse;
                            Unit unit = store.Unit;
                            decimal quantity = store.Quantity;
                            Unit cUnit = store.cUnit;
                            decimal cQuantity = store.cQuantity;

                            var output = Session.FindObject<MovementType>(new BinaryOperator("Code", "P125"));
                            var input = Session.FindObject<MovementType>(new BinaryOperator("Code", "P124"));
                            //var oMovement = new Movement(Session)
                            //{
                            //    HeaderId = headerId,
                            //    DocumentNumber = string.Empty,
                            //    DocumentDate = Helpers.GetSystemDate(Session),
                            //    Barcode = store.Barcode,
                            //    SalesOrderDetail = salesOrderDetail,
                            //    Product = product,
                            //    PartyNumber = partyNumber,
                            //    PaletteNumber = store.PaletteNumber,
                            //    Warehouse = warehouse,
                            //    MovementType = output,
                            //    Unit = unit,
                            //    Quantity = quantity,
                            //    cUnit = cUnit,
                            //    cQuantity = cQuantity
                            //};
                            //var iMovement = new Movement(Session)
                            //{
                            //    HeaderId = headerId,
                            //    DocumentNumber = string.Empty,
                            //    DocumentDate = Helpers.GetSystemDate(Session),
                            //    Barcode = store.Barcode,
                            //    SalesOrderDetail = salesOrderDetail,
                            //    Product = product,
                            //    PartyNumber = partyNumber,
                            //    PaletteNumber = NewPaletteNumber,
                            //    Warehouse = warehouse,
                            //    MovementType = input,
                            //    Unit = unit,
                            //    Quantity = quantity,
                            //    cUnit = cUnit,
                            //    cQuantity = cQuantity
                            //};
                            if (!Helpers.IsUserAdministrator())
                            {
                                Warehouse entryWarehouse = Session.FindObject<Warehouse>(new BinaryOperator("Oid", warehouse.Oid));
                                if (entryWarehouse.CheckPermission)
                                {
                                    WarehouseMovementPermission warehousePermission = Session.FindObject<WarehouseMovementPermission>(CriteriaOperator.Parse("Warehouse = ? and SecuritySystemUser.Name = ?", entryWarehouse, SecuritySystem.CurrentUserName));
                                    if (warehousePermission == null)
                                    {
                                        throw new UserFriendlyException(string.Format("{0} nolu depo için giriş/çıkış yetkiniz yok.", entryWarehouse.Code));
                                    }
                                }
                            }

                            Session.ExecuteNonQuery(@"insert into Movement (Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) values (NEWID(), @1, '', GETDATE(), @2, @3, @4, '', @5, @6, @7, @8, @9, @10, @11, 0, null)", new string[] { "@1", "@2", "@3", "@4", "@5", "@6", "@7", "@8", "@9", "@10", "@11" }, new object[] { headerId, store.Barcode, salesOrderDetail.Oid, product.Oid, store.PaletteNumber, warehouse.Oid, output.Oid, unit.Oid, quantity, cUnit.Oid, cQuantity });
                            Session.ExecuteNonQuery("update Store set GCRecord = 2 where Barcode = @barcode and PaletteNumber = @paletteNumber", new string[] { "@barcode", "@paletteNumber" }, new object[] { store.Barcode, store.PaletteNumber });

                            Session.ExecuteNonQuery(@"insert into Movement (Oid, HeaderId, DocumentNumber, DocumentDate, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, MovementType, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) values (NEWID(), @1, '', GETDATE(), @2, @3, @4, '', @5, @6, @7, @8, @9, @10, @11, 0, null)", new string[] { "@1", "@2", "@3", "@4", "@5", "@6", "@7", "@8", "@9", "@10", "@11" }, new object[] { headerId, store.Barcode, salesOrderDetail.Oid, product.Oid, NewPaletteNumber, warehouse.Oid, input.Oid, unit.Oid, quantity, cUnit.Oid, cQuantity });
                            Session.ExecuteNonQuery("insert into Store (Oid, Product, Barcode, SalesOrderDetail, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) values (NEWID(), @1, @2, @3, '', @4, @5, @6, @7, @8, @9, 0, null)", new string[] { "@1", "@2", "@3", "@4", "@5", "@6", "@7", "@8", "@9" }, new object[] { product.Oid, store.Barcode, salesOrderDetail.Oid, NewPaletteNumber, warehouse.Oid, unit.Oid, quantity, cUnit.Oid, cQuantity });

                            var production = Session.FindObject<Production>(new BinaryOperator("Barcode", Barcode));
                            if (production != null)
                            {
                                Session.ExecuteNonQuery(string.Format("update Production set ProductionPalette = '{0}' where Barcode = '{1}'", productionPalette.Oid, Barcode));
                            }
                        }
                        else throw new UserFriendlyException("Transfer edilecek paletin siparişi ile ürün siparişleri farklı. Palet transferini yapamazsınız.");
                    }
                    else throw new UserFriendlyException("Kamyon depodaki palet için transfer işlemi yapamazsınız.");
                }
                else throw new UserFriendlyException("Kamyon depodaki palet için transfer işlemi yapamazsınız.");
            }
            else throw new UserFriendlyException("Yeni girilen palet numarası tanımlanmamış.");
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public string Barcode { get; set; }

        public string OldPaletteNumber { get { return Barcode != string.Empty ? Session.FindObject<Production>(new BinaryOperator("Barcode", Barcode)).ProductionPalette.PaletteNumber : string.Empty; } }

        [RuleRequiredField]
        public string NewPaletteNumber { get; set; }
    }
}
