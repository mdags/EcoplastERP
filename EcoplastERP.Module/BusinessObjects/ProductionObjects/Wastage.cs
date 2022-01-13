using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;
using DevExpress.Persistent.AuditTrail;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Rules")]
    [DefaultProperty("Barcode")]
    [NavigationItem(false)]
    public class Wastage : BaseObject
    {
        public Wastage(Session session)
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
            if (!Session.IsObjectMarkedDeleted(this))
            {
                AuditTrailService.Instance.EndSessionAudit(Session);
                if (string.IsNullOrEmpty(Barcode)) Barcode = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
                SalesOrderDetail salesOrderDetail = null;
                string paletteNumber = null;
                Machine machine = null;
                Unit unit = null;
                WorkOrderStatus status = WorkOrderStatus.Canceled;
                var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (filmingWorkOrder != null)
                {
                    status = filmingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = filmingWorkOrder.SalesOrderDetail;
                    paletteNumber = filmingWorkOrder.PaletteNumber;
                    machine = filmingWorkOrder.Machine;
                    unit = filmingWorkOrder.Unit;
                }
                var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castFilmingWorkOrder != null)
                {
                    status = castFilmingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;
                    paletteNumber = castFilmingWorkOrder.PaletteNumber;
                    machine = castFilmingWorkOrder.Machine;
                    unit = castFilmingWorkOrder.Unit;
                }
                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castTransferingWorkOrder != null)
                {
                    status = castTransferingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;
                    paletteNumber = castTransferingWorkOrder.PaletteNumber;
                    machine = castTransferingWorkOrder.Machine;
                    unit = castTransferingWorkOrder.Unit;
                }
                var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonFilmingWorkOrder != null)
                {
                    status = balloonFilmingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = balloonFilmingWorkOrder.SalesOrderDetail;
                    paletteNumber = balloonFilmingWorkOrder.PaletteNumber;
                    machine = balloonFilmingWorkOrder.Machine;
                    unit = balloonFilmingWorkOrder.Unit;
                }
                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (printingWorkOrder != null)
                {
                    status = printingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = printingWorkOrder.SalesOrderDetail;
                    paletteNumber = printingWorkOrder.PaletteNumber;
                    machine = printingWorkOrder.Machine;
                    unit = printingWorkOrder.Unit;
                }
                var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (laminationWorkOrder != null)
                {
                    status = laminationWorkOrder.WorkOrderStatus;
                    salesOrderDetail = laminationWorkOrder.SalesOrderDetail;
                    paletteNumber = laminationWorkOrder.PaletteNumber;
                    machine = laminationWorkOrder.Machine;
                    unit = laminationWorkOrder.Unit;
                }
                var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (slicingWorkOrder != null)
                {
                    status = slicingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = slicingWorkOrder.SalesOrderDetail;
                    paletteNumber = slicingWorkOrder.PaletteNumber;
                    machine = slicingWorkOrder.Machine;
                    unit = slicingWorkOrder.Unit;
                }
                var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castSlicingWorkOrder != null)
                {
                    status = castSlicingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;
                    paletteNumber = castSlicingWorkOrder.PaletteNumber;
                    machine = castSlicingWorkOrder.Machine;
                    unit = castSlicingWorkOrder.Unit;
                }
                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (cuttingWorkOrder != null)
                {
                    status = cuttingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = cuttingWorkOrder.SalesOrderDetail;
                    paletteNumber = cuttingWorkOrder.PaletteNumber;
                    machine = cuttingWorkOrder.Machine;
                    unit = cuttingWorkOrder.Unit;
                }
                var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (foldingWorkOrder != null)
                {
                    status = foldingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = foldingWorkOrder.SalesOrderDetail;
                    paletteNumber = foldingWorkOrder.PaletteNumber;
                    machine = foldingWorkOrder.Machine;
                    unit = foldingWorkOrder.Unit;
                }
                var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonCuttingWorkOrder != null)
                {
                    status = balloonCuttingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = balloonCuttingWorkOrder.SalesOrderDetail;
                    paletteNumber = balloonCuttingWorkOrder.PaletteNumber;
                    machine = balloonCuttingWorkOrder.Machine;
                    unit = balloonCuttingWorkOrder.Unit;
                }
                var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (regeneratedWorkOrder != null)
                {
                    status = regeneratedWorkOrder.WorkOrderStatus;
                    salesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;
                    paletteNumber = regeneratedWorkOrder.PaletteNumber;
                    machine = regeneratedWorkOrder.Machine;
                    unit = regeneratedWorkOrder.Unit;
                }
                var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castRegeneratedWorkOrder != null)
                {
                    status = castRegeneratedWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castRegeneratedWorkOrder.SalesOrderDetail;
                    paletteNumber = castRegeneratedWorkOrder.PaletteNumber;
                    machine = castRegeneratedWorkOrder.Machine;
                    unit = castRegeneratedWorkOrder.Unit;
                }
                var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6WorkOrder != null)
                {
                    status = eco6WorkOrder.WorkOrderStatus;
                    salesOrderDetail = eco6WorkOrder.SalesOrderDetail;
                    paletteNumber = eco6WorkOrder.PaletteNumber;
                    machine = eco6WorkOrder.Machine;
                    unit = eco6WorkOrder.Unit;
                }
                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6CuttingWorkOrder != null)
                {
                    status = eco6CuttingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = eco6CuttingWorkOrder.SalesOrderDetail;
                    paletteNumber = eco6CuttingWorkOrder.PaletteNumber;
                    machine = eco6CuttingWorkOrder.Machine;
                    unit = eco6CuttingWorkOrder.Unit;
                }
                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6LaminationWorkOrder != null)
                {
                    status = eco6LaminationWorkOrder.WorkOrderStatus;
                    salesOrderDetail = eco6LaminationWorkOrder.SalesOrderDetail;
                    paletteNumber = eco6LaminationWorkOrder.PaletteNumber;
                    machine = eco6LaminationWorkOrder.Machine;
                    unit = eco6LaminationWorkOrder.Unit;
                }
                if (GrossQuantity > 0)
                {
                    //if (status != WorkOrderStatus.ProductionComplete && status != WorkOrderStatus.Canceled)
                    //{
                    ShiftStart shiftStart = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("Active = true"));
                    if (shiftStart != null)
                    {
                        var employeeTask = Session.FindObject<EmployeeTask>(CriteriaOperator.Parse("Name = 'Operatör'"));
                        if (employeeTask != null)
                        {
                            var shiftAssignment = Session.FindObject<ShiftAssignment>(CriteriaOperator.Parse("ShiftStart = ? and Machine = ? and EmployeeTask = ?", shiftStart, machine, employeeTask)); ;
                            if (shiftAssignment != null)
                            {
                                //Yeni bir malzeme baþlýk ID alýnýr.
                                var headerId = Guid.NewGuid();

                                #region Fire Çýkýþ Hareketleri

                                var output = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P119'"));//Çýkýþ hareket türü
                                if (filmingWorkOrder != null)
                                {
                                    //Ýþ emrinde girilen malzeme ihtiyaçlarý alýnýr.
                                    var reciept = filmingWorkOrder.FilmingWorkOrderReciepts;
                                    if (reciept != null) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                            if (store != null)
                                            {
                                                if (store.Quantity - GrossQuantity * (decimal)item.Rate / 100 < 0)
                                                    throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                                else
                                                {
                                                    var omovement = new Movement(Session)
                                                    {
                                                        HeaderId = headerId,
                                                        DocumentNumber = WorkOrderNumber,
                                                        DocumentDate = Helpers.GetSystemDate(Session),
                                                        Barcode = string.Empty,
                                                        SalesOrderDetail = salesOrderDetail,
                                                        Product = item.Product,
                                                        PartyNumber = string.Empty,
                                                        PaletteNumber = string.Empty,
                                                        Warehouse = filmingWorkOrder.Station.SourceWarehouse,
                                                        MovementType = output,
                                                        Unit = item.Unit,
                                                        Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                    };
                                                }
                                            }
                                            else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                        }
                                    }
                                }
                                if (castFilmingWorkOrder != null)
                                {
                                    //Ýþ emrinde girilen malzeme ihtiyaçlarý alýnýr.
                                    var reciept = castFilmingWorkOrder.CastFilmingWorkOrderReciepts;
                                    if (reciept != null) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                            if (store != null)
                                            {
                                                if (store.Quantity - GrossQuantity * (decimal)item.Rate / 100 < 0)
                                                    throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                                else
                                                {
                                                    var omovement = new Movement(Session)
                                                    {
                                                        HeaderId = headerId,
                                                        DocumentNumber = WorkOrderNumber,
                                                        DocumentDate = Helpers.GetSystemDate(Session),
                                                        Barcode = string.Empty,
                                                        SalesOrderDetail = salesOrderDetail,
                                                        Product = item.Product,
                                                        PartyNumber = string.Empty,
                                                        PaletteNumber = string.Empty,
                                                        Warehouse = castFilmingWorkOrder.Station.SourceWarehouse,
                                                        MovementType = output,
                                                        Unit = item.Unit,
                                                        Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                    };
                                                }
                                            }
                                            else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                        }
                                    }
                                }
                                if (castTransferingWorkOrder != null)
                                {
                                    var reciept = castTransferingWorkOrder.CastTransferingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = castTransferingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                if (balloonFilmingWorkOrder != null)
                                {
                                    //Ýþ emrinde girilen malzeme ihtiyaçlarý alýnýr.
                                    var reciept = balloonFilmingWorkOrder.BalloonFilmingWorkOrderReciepts;
                                    if (reciept != null) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                            if (store != null)
                                            {
                                                if (store.Quantity - GrossQuantity * (decimal)item.Rate / 100 < 0)
                                                    throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                                else
                                                {
                                                    var omovement = new Movement(Session)
                                                    {
                                                        HeaderId = headerId,
                                                        DocumentNumber = WorkOrderNumber,
                                                        DocumentDate = Helpers.GetSystemDate(Session),
                                                        Barcode = string.Empty,
                                                        SalesOrderDetail = salesOrderDetail,
                                                        Product = item.Product,
                                                        PartyNumber = string.Empty,
                                                        PaletteNumber = string.Empty,
                                                        Warehouse = balloonFilmingWorkOrder.Station.SourceWarehouse,
                                                        MovementType = output,
                                                        Unit = item.Unit,
                                                        Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                    };
                                                }
                                            }
                                            else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                        }
                                    }
                                }
                                else if (printingWorkOrder != null)
                                {
                                    var reciept = printingWorkOrder.PrintingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = printingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (laminationWorkOrder != null)
                                {
                                    var reciept = laminationWorkOrder.LaminationWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = laminationWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (slicingWorkOrder != null)
                                {
                                    var reciept = slicingWorkOrder.SlicingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = slicingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (castSlicingWorkOrder != null)
                                {
                                    var reciept = castSlicingWorkOrder.CastSlicingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = castSlicingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (cuttingWorkOrder != null)
                                {
                                    var reciept = cuttingWorkOrder.CuttingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = cuttingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (foldingWorkOrder != null)
                                {
                                    var reciept = foldingWorkOrder.FoldingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = foldingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (balloonCuttingWorkOrder != null)
                                {
                                    var reciept = balloonCuttingWorkOrder.BalloonCuttingWorkOrderReciept;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = balloonCuttingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                //else if (regeneratedWorkOrder != null)
                                //{
                                //    //Ýþ emrinde girilen malzeme ihtiyaçlarý alýnýr.
                                //    var reciept = regeneratedWorkOrder.RegeneratedWorkOrderReciepts;
                                //    if (reciept != null) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                //    {
                                //        foreach (var item in reciept)
                                //        {
                                //            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                //            if (store != null)
                                //            {
                                //                if (store.Quantity - GrossQuantity * (decimal)item.Rate / 100 < 0)
                                //                    throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                //                else
                                //                {
                                //                    var omovement = new Movement(Session)
                                //                    {
                                //                        HeaderId = headerId,
                                //                        DocumentNumber = WorkOrderNumber,
                                //                        DocumentDate = Helpers.GetSystemDate(Session),
                                //                        Barcode = string.Empty,
                                //                        SalesOrderDetail = salesOrderDetail,
                                //                        Product = item.Product,
                                //                        PartyNumber = string.Empty,
                                //                        PaletteNumber = string.Empty,
                                //                        Warehouse = regeneratedWorkOrder.Station.SourceWarehouse,
                                //                        MovementType = output,
                                //                        Unit = item.Unit,
                                //                        Quantity = GrossQuantity * (decimal)item.Rate / 100
                                //                    };
                                //                }
                                //            }
                                //            else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                //        }
                                //    }
                                //}
                                //else if (castRegeneratedWorkOrder != null)
                                //{
                                //    //Ýþ emrinde girilen malzeme ihtiyaçlarý alýnýr.
                                //    var reciept = castRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts;
                                //    if (reciept != null) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                //    {
                                //        foreach (var item in reciept)
                                //        {
                                //            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                //            if (store != null)
                                //            {
                                //                if (store.Quantity - GrossQuantity * (decimal)item.Rate / 100 < 0)
                                //                    throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                //                else
                                //                {
                                //                    var omovement = new Movement(Session)
                                //                    {
                                //                        HeaderId = headerId,
                                //                        DocumentNumber = WorkOrderNumber,
                                //                        DocumentDate = Helpers.GetSystemDate(Session),
                                //                        Barcode = string.Empty,
                                //                        SalesOrderDetail = salesOrderDetail,
                                //                        Product = item.Product,
                                //                        PartyNumber = string.Empty,
                                //                        PaletteNumber = string.Empty,
                                //                        Warehouse = castRegeneratedWorkOrder.Station.SourceWarehouse,
                                //                        MovementType = output,
                                //                        Unit = item.Unit,
                                //                        Quantity = GrossQuantity * (decimal)item.Rate / 100
                                //                    };
                                //                }
                                //            }
                                //            else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                //        }
                                //    }
                                //}
                                else if (eco6WorkOrder != null)
                                {
                                    //Ýþ emrinde girilen malzeme ihtiyaçlarý alýnýr.
                                    var reciept = eco6WorkOrder.Eco6WorkOrderReciepts;
                                    if (reciept != null) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                            if (store != null)
                                            {
                                                if (store.Quantity - GrossQuantity * (decimal)item.Rate / 100 < 0) throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                                else
                                                {
                                                    var omovement = new Movement(Session)
                                                    {
                                                        HeaderId = headerId,
                                                        DocumentNumber = WorkOrderNumber,
                                                        DocumentDate = Helpers.GetSystemDate(Session),
                                                        Barcode = string.Empty,
                                                        SalesOrderDetail = salesOrderDetail,
                                                        Product = item.Product,
                                                        PartyNumber = string.Empty,
                                                        PaletteNumber = string.Empty,
                                                        Warehouse = eco6WorkOrder.Station.SourceWarehouse,
                                                        MovementType = output,
                                                        Unit = item.Unit,
                                                        Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                    };
                                                }
                                            }
                                            else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Name));
                                        }
                                    }
                                }
                                else if (eco6CuttingWorkOrder != null)
                                {
                                    var reciept = eco6CuttingWorkOrder.Eco6CuttingWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = eco6CuttingWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if (eco6LaminationWorkOrder != null)
                                {
                                    var reciept = eco6LaminationWorkOrder.Eco6LaminationWorkOrderReciepts;
                                    if (reciept.Count > 0) //Malzeme ihtiyaçlarý girilmiþse depodan çýkýþ hareketleri yapýlýr.
                                    {
                                        foreach (var item in reciept)
                                        {
                                            if (item.ResourceObligatory)
                                            {
                                                var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                else
                                                {
                                                    var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                    if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                    if (readResourceOid != null)
                                                    {
                                                        var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                        if (readResource != null)
                                                        {
                                                            decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                            if (readResource.Quantity >= rrmt + GrossQuantity)
                                                            {
                                                                if (readResource.Quantity == rrmt + (GrossQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = GrossQuantity * (decimal)item.Rate / 100
                                                                };
                                                            }
                                                            else
                                                            {
                                                                decimal remaininQuantity = (GrossQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                readResource.IsConsume = true;
                                                                var readResourceMovement = new ReadResourceMovement(Session)
                                                                {
                                                                    HeaderId = headerId,
                                                                    ProductionBarcode = Barcode,
                                                                    ReadResource = readResource,
                                                                    Unit = readResource.Unit,
                                                                    Quantity = readResource.Quantity - rrmt
                                                                };

                                                                //if (!IsLastRoll)
                                                                //{
                                                                readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapýnýz !...");
                                                                if (readResourceOid != null)
                                                                {
                                                                    readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                    if (readResource != null)
                                                                    {
                                                                        rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                        if (remaininQuantity <= rrmt + GrossQuantity)
                                                                        {
                                                                            if (remaininQuantity == rrmt + GrossQuantity) readResource.IsConsume = true;
                                                                            var rrMovement = new ReadResourceMovement(Session)
                                                                            {
                                                                                HeaderId = headerId,
                                                                                ProductionBarcode = Barcode,
                                                                                ReadResource = readResource,
                                                                                Unit = readResource.Unit,
                                                                                Quantity = remaininQuantity
                                                                            };
                                                                        }
                                                                    }
                                                                }
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }

                                                //if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                //{
                                                //    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                //    if (store != null)
                                                //    {
                                                //        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                //            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
                                                //        else
                                                //        {
                                                //            var omovement = new Movement(Session)
                                                //            {
                                                //                HeaderId = headerId,
                                                //                DocumentNumber = WorkOrderNumber,
                                                //                DocumentDate = Helpers.GetSystemDate(Session),
                                                //                Barcode = string.Empty,
                                                //                SalesOrderDetail = salesOrderDetail,
                                                //                Product = item.Product,
                                                //                PartyNumber = string.Empty,
                                                //                PaletteNumber = string.Empty,
                                                //                Warehouse = eco6LaminationWorkOrder.Station.SourceWarehouse,
                                                //                MovementType = output,
                                                //                Unit = item.Unit,
                                                //                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                //                cUnit = item.Unit,
                                                //                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                //            };
                                                //        }
                                                //    }
                                                //    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                //}
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region Fire Kaydý

                                var wastageReason = Session.FindObject<WastageReason>(CriteriaOperator.Parse("Code = ?", WastageReasonCode));
                                var _product = Session.FindObject<Product>(CriteriaOperator.Parse("Code = ?", ProductCode));

                                decimal cquantity = 0;
                                var cquanunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", salesOrderDetail.cUnit, salesOrderDetail.Unit, salesOrderDetail.Product));
                                if (cquanunit != null) cquantity = (cquanunit.BaseQuantity * GrossQuantity) / cquanunit.cQuantity;

                                FilmingWorkOrder = filmingWorkOrder;
                                CastFilmingWorkOrder = castFilmingWorkOrder;
                                CastTransferingWorkOrder = castTransferingWorkOrder;
                                BalloonFilmingWorkOrder = balloonFilmingWorkOrder;
                                PrintingWorkOrder = printingWorkOrder;
                                LaminationWorkOrder = laminationWorkOrder;
                                SlicingWorkOrder = slicingWorkOrder;
                                CastSlicingWorkOrder = castSlicingWorkOrder;
                                CuttingWorkOrder = cuttingWorkOrder;
                                FoldingWorkOrder = foldingWorkOrder;
                                BalloonCuttingWorkOrder = balloonCuttingWorkOrder;
                                RegeneratedWorkOrder = regeneratedWorkOrder;
                                CastRegeneratedWorkOrder = castRegeneratedWorkOrder;
                                Eco6WorkOrder = eco6WorkOrder;
                                Eco6CuttingWorkOrder = eco6CuttingWorkOrder;
                                Eco6LaminationWorkOrder = eco6LaminationWorkOrder;
                                WastageReason = wastageReason;
                                Machine = machine;
                                SalesOrderDetail = salesOrderDetail;
                                PaletteNumber = string.Empty;
                                Shift = shiftStart;
                                Employee = shiftAssignment.Employee;
                                Product = _product;
                                Unit = unit;
                                NetQuantity = GrossQuantity;
                                cUnit = salesOrderDetail.cUnit;
                                cQuantity = cquantity;
                                WastageDate = Helpers.GetSystemDate(Session);

                                #endregion

                                #region Fire Giriþ Hareketi
                                Warehouse wastageWarehouse = null;
                                if (filmingWorkOrder != null) wastageWarehouse = filmingWorkOrder.Station.WastageWarehouse;
                                if (castFilmingWorkOrder != null) wastageWarehouse = castFilmingWorkOrder.Station.WastageWarehouse;
                                if (castTransferingWorkOrder != null) wastageWarehouse = castTransferingWorkOrder.Station.WastageWarehouse;
                                if (balloonFilmingWorkOrder != null) wastageWarehouse = balloonFilmingWorkOrder.Station.WastageWarehouse;
                                if (printingWorkOrder != null) wastageWarehouse = printingWorkOrder.Station.WastageWarehouse;
                                if (laminationWorkOrder != null) wastageWarehouse = laminationWorkOrder.Station.WastageWarehouse;
                                if (slicingWorkOrder != null) wastageWarehouse = slicingWorkOrder.Station.WastageWarehouse;
                                if (castSlicingWorkOrder != null) wastageWarehouse = castSlicingWorkOrder.Station.WastageWarehouse;
                                if (cuttingWorkOrder != null) wastageWarehouse = cuttingWorkOrder.Station.WastageWarehouse;
                                if (foldingWorkOrder != null) wastageWarehouse = foldingWorkOrder.Station.WastageWarehouse;
                                if (balloonCuttingWorkOrder != null) wastageWarehouse = balloonCuttingWorkOrder.Station.WastageWarehouse;
                                if (regeneratedWorkOrder != null) wastageWarehouse = regeneratedWorkOrder.Station.WastageWarehouse;
                                if (castRegeneratedWorkOrder != null) wastageWarehouse = castRegeneratedWorkOrder.Station.WastageWarehouse;
                                if (eco6WorkOrder != null) wastageWarehouse = eco6WorkOrder.Station.WastageWarehouse;
                                if (eco6CuttingWorkOrder != null) wastageWarehouse = eco6CuttingWorkOrder.Station.WastageWarehouse;
                                if (eco6LaminationWorkOrder != null) wastageWarehouse = eco6LaminationWorkOrder.Station.WastageWarehouse;

                                //Üretilen ürünün giriþ hareketi yapýlýr.
                                MovementType input = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P118'"));
                                Movement imovement = new Movement(Session)
                                {
                                    HeaderId = headerId,
                                    DocumentNumber = WorkOrderNumber,
                                    DocumentDate = Helpers.GetSystemDate(Session),
                                    Barcode = Barcode,
                                    SalesOrderDetail = salesOrderDetail,
                                    Product = Product,
                                    PartyNumber = string.Empty,
                                    PaletteNumber = string.Empty,
                                    Warehouse = Product.Warehouse != null ? Product.Warehouse : wastageWarehouse,
                                    MovementType = input,
                                    Unit = Unit,
                                    Quantity = GrossQuantity,
                                    cUnit = Unit,
                                    cQuantity = GrossQuantity
                                };
                                #endregion
                            }
                            else throw new UserFriendlyException("Makineye operatör atamasý yapýlmamýþ. Fire teyidi verilemez.");
                        }
                        else throw new UserFriendlyException("Operatör olarak herhangi bir personel görev tanýmý yapýlmamýþ.");
                    }
                    else throw new UserFriendlyException("Vardiya baþlatýlmamýþ. Fire teyidi verilemez.");
                    //}
                    //else throw new UserFriendlyException("Üretim sipariþi durumu Üretimi Tamamlanmýþ veya Vazgeçilmiþ olarak ayarlanmýþtýr. Fire teyidi verilemez.");
                }
                else throw new UserFriendlyException("Fire teyidi alabilmek için basküle bir ürün yerleþtiriniz.");
            }
        }
        // Fields...
        private DateTime _WastageDate;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _NetQuantity;
        private decimal _GrossQuantity;
        private Unit _Unit;
        private Product _Product;
        private string _ProductCode;
        private Employee _Employee;
        private ShiftStart _Shift;
        private string _PaletteNumber;
        private SalesOrderDetail _SalesOrderDetail;
        private Machine _Machine;
        private WastageReason _WastageReason;
        private string _WastageReasonCode;
        private string _Barcode;
        private Eco6LaminationWorkOrder _Eco6LaminationWorkOrder;
        private Eco6CuttingWorkOrder _Eco6CuttingWorkOrder;
        private Eco6WorkOrder _Eco6WorkOrder;
        private CastRegeneratedWorkOrder _CastRegeneratedWorkOrder;
        private RegeneratedWorkOrder _RegeneratedWorkOrder;
        private FoldingWorkOrder _FoldingWorkOrder;
        private BalloonCuttingWorkOrder _BalloonCuttingWorkOrder;
        private CuttingWorkOrder _CuttingWorkOrder;
        private SlicingWorkOrder _SlicingWorkOrder;
        private CastSlicingWorkOrder _CastSlicingWorkOrder;
        private LaminationWorkOrder _LaminationWorkOrder;
        private PrintingWorkOrder _PrintingWorkOrder;
        private BalloonFilmingWorkOrder _BalloonFilmingWorkOrder;
        private CastTransferingWorkOrder _CastTransferingWorkOrder;
        private CastFilmingWorkOrder _CastFilmingWorkOrder;
        private FilmingWorkOrder _FilmingWorkOrder;
        private string _WorkOrderNumber;

        public string WorkOrderNumber
        {
            get
            {
                return _WorkOrderNumber;
            }
            set
            {
                SetPropertyValue("WorkOrderNumber", ref _WorkOrderNumber, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("FilmingWorkOrder-Wastages")]
        public FilmingWorkOrder FilmingWorkOrder
        {
            get
            {
                return _FilmingWorkOrder;
            }
            set
            {
                SetPropertyValue("FilmingWorkOrder", ref _FilmingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("CastFilmingWorkOrder-Wastages")]
        public CastFilmingWorkOrder CastFilmingWorkOrder
        {
            get
            {
                return _CastFilmingWorkOrder;
            }
            set
            {
                SetPropertyValue("CastFilmingWorkOrder", ref _CastFilmingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("CastTransferingWorkOrder-Wastages")]
        public CastTransferingWorkOrder CastTransferingWorkOrder
        {
            get
            {
                return _CastTransferingWorkOrder;
            }
            set
            {
                SetPropertyValue("CastTransferingWorkOrder", ref _CastTransferingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("BalloonFilmingWorkOrder-Wastages")]
        public BalloonFilmingWorkOrder BalloonFilmingWorkOrder
        {
            get
            {
                return _BalloonFilmingWorkOrder;
            }
            set
            {
                SetPropertyValue("BalloonFilmingWorkOrder", ref _BalloonFilmingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("PrintingWorkOrder-Wastages")]
        public PrintingWorkOrder PrintingWorkOrder
        {
            get
            {
                return _PrintingWorkOrder;
            }
            set
            {
                SetPropertyValue("PrintingWorkOrder", ref _PrintingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("LaminationWorkOrder-Wastages")]
        public LaminationWorkOrder LaminationWorkOrder
        {
            get
            {
                return _LaminationWorkOrder;
            }
            set
            {
                SetPropertyValue("LaminationWorkOrder", ref _LaminationWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("SlicingWorkOrder-Wastages")]
        public SlicingWorkOrder SlicingWorkOrder
        {
            get
            {
                return _SlicingWorkOrder;
            }
            set
            {
                SetPropertyValue("SlicingWorkOrder", ref _SlicingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("CastSlicingWorkOrder-Wastages")]
        public CastSlicingWorkOrder CastSlicingWorkOrder
        {
            get
            {
                return _CastSlicingWorkOrder;
            }
            set
            {
                SetPropertyValue("CastSlicingWorkOrder", ref _CastSlicingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("CuttingWorkOrder-Wastages")]
        public CuttingWorkOrder CuttingWorkOrder
        {
            get
            {
                return _CuttingWorkOrder;
            }
            set
            {
                SetPropertyValue("CuttingWorkOrder", ref _CuttingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("FoldingWorkOrder-Wastages")]
        public FoldingWorkOrder FoldingWorkOrder
        {
            get
            {
                return _FoldingWorkOrder;
            }
            set
            {
                SetPropertyValue("FoldingWorkOrder", ref _FoldingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("BalloonCuttingWorkOrder-Wastages")]
        public BalloonCuttingWorkOrder BalloonCuttingWorkOrder
        {
            get
            {
                return _BalloonCuttingWorkOrder;
            }
            set
            {
                SetPropertyValue("BalloonCuttingWorkOrder", ref _BalloonCuttingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("RegeneratedWorkOrder-Wastages")]
        public RegeneratedWorkOrder RegeneratedWorkOrder
        {
            get
            {
                return _RegeneratedWorkOrder;
            }
            set
            {
                SetPropertyValue("RegeneratedWorkOrder", ref _RegeneratedWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("CastRegeneratedWorkOrder-Wastages")]
        public CastRegeneratedWorkOrder CastRegeneratedWorkOrder
        {
            get
            {
                return _CastRegeneratedWorkOrder;
            }
            set
            {
                SetPropertyValue("CastRegeneratedWorkOrder", ref _CastRegeneratedWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("Eco6WorkOrder-Wastages")]
        public Eco6WorkOrder Eco6WorkOrder
        {
            get
            {
                return _Eco6WorkOrder;
            }
            set
            {
                SetPropertyValue("Eco6WorkOrder", ref _Eco6WorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("Eco6CuttingWorkOrder-Wastages")]
        public Eco6CuttingWorkOrder Eco6CuttingWorkOrder
        {
            get
            {
                return _Eco6CuttingWorkOrder;
            }
            set
            {
                SetPropertyValue("Eco6CuttingWorkOrder", ref _Eco6CuttingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("Eco6LaminationWorkOrder-Wastages")]
        public Eco6LaminationWorkOrder Eco6LaminationWorkOrder
        {
            get
            {
                return _Eco6LaminationWorkOrder;
            }
            set
            {
                SetPropertyValue("Eco6LaminationWorkOrder", ref _Eco6LaminationWorkOrder, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        //[Indexed(Unique = true)]
        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                SetPropertyValue("Barcode", ref _Barcode, value);
            }
        }

        [NonPersistent]
        public string WastageReasonCode
        {
            get
            {
                return _WastageReasonCode;
            }
            set
            {
                SetPropertyValue("WastageReasonCode", ref _WastageReasonCode, value);
            }
        }

        public WastageReason WastageReason
        {
            get
            {
                return _WastageReason;
            }
            set
            {
                SetPropertyValue("WastageReason", ref _WastageReason, value);
            }
        }
        
        public Machine Machine
        {
            get
            {
                return _Machine;
            }
            set
            {
                SetPropertyValue("Machine", ref _Machine, value);
            }
        }

        [VisibleInDetailView(false)]
        public SalesOrderDetail SalesOrderDetail
        {
            get
            {
                return _SalesOrderDetail;
            }
            set
            {
                SetPropertyValue("SalesOrderDetail", ref _SalesOrderDetail, value);
            }
        }

        public string PaletteNumber
        {
            get
            {
                return _PaletteNumber;
            }
            set
            {
                SetPropertyValue("PaletteNumber", ref _PaletteNumber, value);
            }
        }

        public ShiftStart Shift
        {
            get
            {
                return _Shift;
            }
            set
            {
                SetPropertyValue("Shift", ref _Shift, value);
            }
        }

        public Employee Employee
        {
            get
            {
                return _Employee;
            }
            set
            {
                SetPropertyValue("Employee", ref _Employee, value);
            }
        }

        [NonPersistent]
        public string ProductCode
        {
            get
            {
                return _ProductCode;
            }
            set
            {
                SetPropertyValue("ProductCode", ref _ProductCode, value);
            }
        }
        
        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                SetPropertyValue("Product", ref _Product, value);
            }
        }
        
        public Unit Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                SetPropertyValue("Unit", ref _Unit, value);
            }
        }

        public decimal GrossQuantity
        {
            get
            {
                return _GrossQuantity;
            }
            set
            {
                SetPropertyValue("GrossQuantity", ref _GrossQuantity, value);
            }
        }

        public decimal NetQuantity
        {
            get
            {
                return _NetQuantity;
            }
            set
            {
                SetPropertyValue("NetQuantity", ref _NetQuantity, value);
            }
        }
        
        public Unit cUnit
        {
            get
            {
                return _CUnit;
            }
            set
            {
                SetPropertyValue("cUnit", ref _CUnit, value);
            }
        }

        public decimal cQuantity
        {
            get
            {
                return _CQuantity;
            }
            set
            {
                SetPropertyValue("cQuantity", ref _CQuantity, value);
            }
        }

        public DateTime WastageDate
        {
            get
            {
                return _WastageDate;
            }
            set
            {
                SetPropertyValue("WastageDate", ref _WastageDate, value);
            }
        }
    }
}
