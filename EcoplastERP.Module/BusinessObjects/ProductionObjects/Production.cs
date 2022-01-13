using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.AuditTrail;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Change_State")]
    [DefaultProperty("Barcode")]
    [NavigationItem(false)]
    [Appearance("Production.IsQuarantine", TargetItems = "*", Criteria = "IsQuarantine = true", BackColor = "PowderBlue", Context = "Production_ListView")]
    public class Production : BaseObject
    {
        public Production(Session session)
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
                ProductionPalette activePalette = null;
                decimal productionOption = 0, totalProduction = 0, workOrderQuantity = 0, minWeight = 0, maxWeight = 0, activePaletteLastWeight = 0, thickness = 0, pieceWeight = 0, rollWeight = 0;
                Machine machine = null;
                SalesOrderDetail salesOrderDetail = null;
                Unit unit = null;
                Warehouse productionWarehouse = null;
                WorkOrderStatus status = WorkOrderStatus.Canceled;
                var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (filmingWorkOrder != null)
                {
                    status = filmingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = filmingWorkOrder.SalesOrderDetail;
                    machine = filmingWorkOrder.Machine;
                    unit = filmingWorkOrder.Unit;
                    productionWarehouse = filmingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = filmingWorkOrder.Quantity;
                    thickness = filmingWorkOrder.Thickness;
                    pieceWeight = filmingWorkOrder.GramMetretul;
                    rollWeight = filmingWorkOrder.RollWeight;
                    productionOption = filmingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", filmingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", filmingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = filmingWorkOrder.MinimumRollWeight;
                    maxWeight = filmingWorkOrder.MaximumRollWeight;
                }
                var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castFilmingWorkOrder != null)
                {
                    status = castFilmingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;
                    machine = castFilmingWorkOrder.Machine;
                    unit = castFilmingWorkOrder.Unit;
                    productionWarehouse = castFilmingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = castFilmingWorkOrder.Quantity;
                    thickness = castFilmingWorkOrder.Thickness;
                    pieceWeight = castFilmingWorkOrder.GramMetretul;
                    rollWeight = castFilmingWorkOrder.RollWeight;
                    productionOption = castFilmingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", castFilmingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", castFilmingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = castFilmingWorkOrder.MinimumRollWeight;
                    maxWeight = castFilmingWorkOrder.MaximumRollWeight;
                }
                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(new BinaryOperator("WorkOrderNumber", WorkOrderNumber));
                if (castTransferingWorkOrder != null)
                {
                    status = castTransferingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;
                    machine = castTransferingWorkOrder.Machine;
                    unit = castTransferingWorkOrder.Unit;
                    productionWarehouse = castTransferingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = castTransferingWorkOrder.Quantity;
                    thickness = castTransferingWorkOrder.Thickness;
                    pieceWeight = castTransferingWorkOrder.GramMetretul;
                    rollWeight = castTransferingWorkOrder.RollWeight;
                    productionOption = castTransferingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", castTransferingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", castTransferingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = castTransferingWorkOrder.MinimumRollWeight;
                    maxWeight = castTransferingWorkOrder.MaximumRollWeight;
                }
                var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonFilmingWorkOrder != null)
                {
                    status = balloonFilmingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = balloonFilmingWorkOrder.SalesOrderDetail;
                    machine = balloonFilmingWorkOrder.Machine;
                    unit = balloonFilmingWorkOrder.Unit;
                    productionWarehouse = balloonFilmingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = balloonFilmingWorkOrder.Quantity;
                    thickness = balloonFilmingWorkOrder.Thickness;
                    pieceWeight = balloonFilmingWorkOrder.GramMetretul;
                    rollWeight = balloonFilmingWorkOrder.RollWeight;
                    productionOption = balloonFilmingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", balloonFilmingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", balloonFilmingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = balloonFilmingWorkOrder.MinimumRollWeight;
                    maxWeight = balloonFilmingWorkOrder.MaximumRollWeight;
                }
                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (printingWorkOrder != null)
                {
                    status = printingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = printingWorkOrder.SalesOrderDetail;
                    machine = printingWorkOrder.Machine;
                    unit = printingWorkOrder.Unit;
                    productionWarehouse = printingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = printingWorkOrder.Quantity;
                    thickness = printingWorkOrder.Thickness;
                    pieceWeight = printingWorkOrder.SalesOrderDetail.Product.PieceWeight;
                    rollWeight = printingWorkOrder.RollWeight;
                    productionOption = printingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", printingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", printingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = printingWorkOrder.MinimumRollWeight;
                    maxWeight = printingWorkOrder.MaximumRollWeight;
                }
                var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (laminationWorkOrder != null)
                {
                    status = laminationWorkOrder.WorkOrderStatus;
                    salesOrderDetail = laminationWorkOrder.SalesOrderDetail;
                    machine = laminationWorkOrder.Machine;
                    unit = laminationWorkOrder.Unit;
                    productionWarehouse = laminationWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = laminationWorkOrder.Quantity;
                    thickness = laminationWorkOrder.Thickness;
                    pieceWeight = laminationWorkOrder.SalesOrderDetail.Product.PieceWeight;
                    rollWeight = laminationWorkOrder.RollWeight;
                    productionOption = laminationWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", laminationWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", laminationWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = laminationWorkOrder.MinimumRollWeight;
                    maxWeight = laminationWorkOrder.MaximumRollWeight;
                }
                var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (slicingWorkOrder != null)
                {
                    status = slicingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = slicingWorkOrder.SalesOrderDetail;
                    machine = slicingWorkOrder.Machine;
                    unit = slicingWorkOrder.Unit;
                    productionWarehouse = slicingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = slicingWorkOrder.Quantity;
                    thickness = slicingWorkOrder.Thickness;
                    pieceWeight = slicingWorkOrder.GramMetretul;
                    rollWeight = slicingWorkOrder.RollWeight;
                    productionOption = slicingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", slicingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", slicingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = slicingWorkOrder.MinimumRollWeight;
                    maxWeight = slicingWorkOrder.MaximumRollWeight;
                }
                var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castSlicingWorkOrder != null)
                {
                    status = castSlicingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;
                    machine = castSlicingWorkOrder.Machine;
                    unit = castSlicingWorkOrder.Unit;
                    productionWarehouse = castSlicingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = castSlicingWorkOrder.Quantity;
                    thickness = castSlicingWorkOrder.Thickness;
                    rollWeight = castSlicingWorkOrder.RollWeight;
                    pieceWeight = castSlicingWorkOrder.GramMetretul;
                    productionOption = castSlicingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", castSlicingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", castSlicingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = castSlicingWorkOrder.MinimumRollWeight;
                    maxWeight = castSlicingWorkOrder.MaximumRollWeight;
                }
                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (cuttingWorkOrder != null)
                {
                    status = cuttingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = cuttingWorkOrder.SalesOrderDetail;
                    machine = cuttingWorkOrder.Machine;
                    unit = cuttingWorkOrder.Unit;
                    productionWarehouse = cuttingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = cuttingWorkOrder.Quantity;
                    thickness = cuttingWorkOrder.Thickness;
                    pieceWeight = cuttingWorkOrder.GramMetretul;
                    rollWeight = cuttingWorkOrder.PackageWeight;
                    productionOption = cuttingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", cuttingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", cuttingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = cuttingWorkOrder.MinimumPieceWeight;
                    maxWeight = cuttingWorkOrder.MaximumPieceWeight;
                }
                var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (foldingWorkOrder != null)
                {
                    status = foldingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = foldingWorkOrder.SalesOrderDetail;
                    machine = foldingWorkOrder.Machine;
                    unit = foldingWorkOrder.Unit;
                    productionWarehouse = foldingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = foldingWorkOrder.Quantity;
                    thickness = foldingWorkOrder.Thickness;
                    pieceWeight = foldingWorkOrder.GramMetretul;
                    rollWeight = foldingWorkOrder.PackageWeight;
                    productionOption = foldingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", foldingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", foldingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = foldingWorkOrder.MinimumPieceWeight;
                    maxWeight = foldingWorkOrder.MaximumPieceWeight;
                }
                var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonCuttingWorkOrder != null)
                {
                    status = balloonCuttingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = balloonCuttingWorkOrder.SalesOrderDetail;
                    machine = balloonCuttingWorkOrder.Machine;
                    unit = balloonCuttingWorkOrder.Unit;
                    productionWarehouse = balloonCuttingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = balloonCuttingWorkOrder.Quantity;
                    thickness = balloonCuttingWorkOrder.Thickness;
                    pieceWeight = balloonCuttingWorkOrder.GramMetretul;
                    rollWeight = balloonCuttingWorkOrder.PackageWeight;
                    productionOption = balloonCuttingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", balloonCuttingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", balloonCuttingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = balloonCuttingWorkOrder.MinimumPieceWeight;
                    maxWeight = balloonCuttingWorkOrder.MaximumPieceWeight;
                }
                var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (regeneratedWorkOrder != null)
                {
                    status = regeneratedWorkOrder.WorkOrderStatus;
                    salesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;
                    machine = regeneratedWorkOrder.Machine;
                    unit = regeneratedWorkOrder.Unit;
                    var regeneratedProductionWarehouse = Session.FindObject<Warehouse>(new BinaryOperator("Code", "400"));
                    productionWarehouse = regeneratedProductionWarehouse;
                    workOrderQuantity = regeneratedWorkOrder.Quantity;
                    productionOption = regeneratedWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", regeneratedWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", regeneratedWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = 0;
                    maxWeight = 999999;
                }
                var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castRegeneratedWorkOrder != null)
                {
                    status = castRegeneratedWorkOrder.WorkOrderStatus;
                    salesOrderDetail = castRegeneratedWorkOrder.SalesOrderDetail;
                    machine = castRegeneratedWorkOrder.Machine;
                    unit = castRegeneratedWorkOrder.Unit;
                    var regeneratedProductionWarehouse = Session.FindObject<Warehouse>(new BinaryOperator("Code", "400"));
                    productionWarehouse = regeneratedProductionWarehouse;
                    workOrderQuantity = castRegeneratedWorkOrder.Quantity;
                    productionOption = castRegeneratedWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", castRegeneratedWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", castRegeneratedWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = 0;
                    maxWeight = 999999;
                }
                var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6WorkOrder != null)
                {
                    status = eco6WorkOrder.WorkOrderStatus;
                    salesOrderDetail = eco6WorkOrder.SalesOrderDetail;
                    machine = eco6WorkOrder.Machine;
                    unit = eco6WorkOrder.Unit;
                    productionWarehouse = eco6WorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = eco6WorkOrder.Quantity;
                    thickness = eco6WorkOrder.Thickness;
                    pieceWeight = eco6WorkOrder.GramMetretul;
                    rollWeight = eco6WorkOrder.RollWeight;
                    productionOption = eco6WorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", eco6WorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", eco6WorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = eco6WorkOrder.MinimumRollWeight;
                    maxWeight = eco6WorkOrder.MaximumRollWeight;
                }
                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6CuttingWorkOrder != null)
                {
                    status = eco6CuttingWorkOrder.WorkOrderStatus;
                    salesOrderDetail = eco6CuttingWorkOrder.SalesOrderDetail;
                    machine = eco6CuttingWorkOrder.Machine;
                    unit = eco6CuttingWorkOrder.Unit;
                    productionWarehouse = eco6CuttingWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = eco6CuttingWorkOrder.Quantity;
                    thickness = eco6CuttingWorkOrder.Thickness;
                    pieceWeight = eco6CuttingWorkOrder.GramMetretul;
                    rollWeight = eco6CuttingWorkOrder.PackageWeight;
                    productionOption = eco6CuttingWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", eco6CuttingWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", eco6CuttingWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = eco6CuttingWorkOrder.MinimumPieceWeight;
                    maxWeight = eco6CuttingWorkOrder.MaximumPieceWeight;
                }
                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6LaminationWorkOrder != null)
                {
                    status = eco6LaminationWorkOrder.WorkOrderStatus;
                    salesOrderDetail = eco6LaminationWorkOrder.SalesOrderDetail;
                    machine = eco6LaminationWorkOrder.Machine;
                    unit = eco6LaminationWorkOrder.Unit;
                    productionWarehouse = eco6LaminationWorkOrder.NextStation.SourceWarehouse;
                    workOrderQuantity = eco6LaminationWorkOrder.Quantity;
                    thickness = eco6LaminationWorkOrder.Thickness;
                    pieceWeight = eco6LaminationWorkOrder.SalesOrderDetail.Product.PieceWeight;
                    rollWeight = eco6LaminationWorkOrder.RollWeight;
                    productionOption = eco6LaminationWorkOrder.ProductionOption;
                    totalProduction = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("WorkOrderNumber = ?", eco6LaminationWorkOrder.WorkOrderNumber)));
                    activePalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", eco6LaminationWorkOrder.PaletteNumber));
                    if (activePalette != null) activePaletteLastWeight = activePalette.LastWeight;
                    minWeight = eco6LaminationWorkOrder.MinimumRollWeight;
                    maxWeight = eco6LaminationWorkOrder.MaximumRollWeight;
                }
                if (GrossQuantity > 0)
                {
                    if (status != WorkOrderStatus.ProductionComplete && status != WorkOrderStatus.Canceled)
                    {
                        ShiftStart shiftStart = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("Active = true"));
                        if (shiftStart != null)
                        {
                            var employeeTask = Session.FindObject<EmployeeTask>(CriteriaOperator.Parse("Name = 'Operatör'"));
                            if (employeeTask != null)
                            {
                                var shiftAssignment = Session.FindObject<ShiftAssignment>(CriteriaOperator.Parse("ShiftStart = ? and Machine = ? and EmployeeTask = ?", shiftStart, machine, employeeTask));
                                if (shiftAssignment != null)
                                {
                                    if (activePaletteLastWeight == 0)
                                    {
                                        if (IsLastRoll || GrossQuantity >= minWeight & GrossQuantity <= maxWeight) //Minimum Maksimum Kontrolü
                                        {
                                            if (productionOption == 0 || totalProduction + GrossQuantity <= workOrderQuantity + (workOrderQuantity * productionOption / 100))
                                            {
                                                int actualPaletteBobbinCount = 0, paletteBobbinCount = 999999999, bobbinCount = 999999999, totalBobbinCount = 0;
                                                if (filmingWorkOrder != null)
                                                {
                                                    bobbinCount = filmingWorkOrder.RollCount;
                                                    totalBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("FilmingWorkOrder = ?", filmingWorkOrder));
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", filmingWorkOrder.PaletteNumber));
                                                    if (filmingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (filmingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = filmingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }
                                                if (castFilmingWorkOrder != null)
                                                {
                                                    bobbinCount = castFilmingWorkOrder.RollCount;
                                                    totalBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastFilmingWorkOrder = ?", castFilmingWorkOrder));
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", castFilmingWorkOrder.PaletteNumber));
                                                    if (castFilmingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (castFilmingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = castFilmingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }
                                                if (castTransferingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", castTransferingWorkOrder.PaletteNumber));
                                                    if (castTransferingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (castTransferingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = castTransferingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }
                                                if (balloonFilmingWorkOrder != null)
                                                {
                                                    bobbinCount = balloonFilmingWorkOrder.RollCount;
                                                    totalBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("BalloonFilmingWorkOrder = ?", balloonFilmingWorkOrder));
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", balloonFilmingWorkOrder.PaletteNumber));
                                                    if (balloonFilmingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (balloonFilmingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = balloonFilmingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }
                                                if (printingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", printingWorkOrder.PaletteNumber));
                                                    if (printingWorkOrder.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = printingWorkOrder.PaletteBobbinCount;
                                                }
                                                if (laminationWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", laminationWorkOrder.PaletteNumber));
                                                    if (laminationWorkOrder.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = laminationWorkOrder.PaletteBobbinCount;
                                                }
                                                if (slicingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", slicingWorkOrder.PaletteNumber));
                                                    if (slicingWorkOrder.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = slicingWorkOrder.PaletteBobbinCount;
                                                }
                                                if (castSlicingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", castSlicingWorkOrder.PaletteNumber));
                                                    if (castSlicingWorkOrder.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = castSlicingWorkOrder.PaletteBobbinCount;
                                                }
                                                if (cuttingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", cuttingWorkOrder.PaletteNumber));
                                                    if (cuttingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (cuttingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = cuttingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }
                                                if (eco6WorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", eco6WorkOrder.PaletteNumber));
                                                    if (eco6WorkOrder.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = eco6WorkOrder.PaletteBobbinCount;
                                                }
                                                if (eco6CuttingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", eco6CuttingWorkOrder.PaletteNumber));
                                                    if (eco6CuttingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (eco6CuttingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                            paletteBobbinCount = eco6CuttingWorkOrder.PaletteBobbinCount;
                                                    }
                                                    else
                                                    {
                                                        if (eco6CuttingWorkOrder.SalesOrderDetail.Product.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = eco6CuttingWorkOrder.PaletteBobbinCount;
                                                    }
                                                }
                                                if (eco6LaminationWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", eco6LaminationWorkOrder.PaletteNumber));
                                                    if (eco6LaminationWorkOrder.ShippingPackageType.Name == "PALETLİ") paletteBobbinCount = eco6LaminationWorkOrder.PaletteBobbinCount;
                                                }
                                                if (foldingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", foldingWorkOrder.PaletteNumber));
                                                    if (foldingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (foldingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = foldingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }
                                                if (balloonCuttingWorkOrder != null)
                                                {
                                                    actualPaletteBobbinCount = (int)Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", balloonCuttingWorkOrder.PaletteNumber));
                                                    if (balloonCuttingWorkOrder.ShippingPackageType != null)
                                                    {
                                                        if (balloonCuttingWorkOrder.ShippingPackageType.Name == "PALETLİ")
                                                        {
                                                            paletteBobbinCount = balloonCuttingWorkOrder.PaletteBobbinCount;
                                                        }
                                                    }
                                                }

                                                if (totalBobbinCount > bobbinCount) throw new UserFriendlyException(string.Format("Bu üretim için toplam rulo sayısı tamamlandığından üretim teyidi verilemez. Rulo sayısı: {0:n0} olarak tanımlanmıştır.", bobbinCount));

                                                if (actualPaletteBobbinCount + 1 <= paletteBobbinCount)
                                                {
                                                    decimal netQuantity = GrossQuantity;
                                                    if (filmingWorkOrder != null)
                                                    {
                                                        if (filmingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)filmingWorkOrder.Width * (decimal)filmingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (castFilmingWorkOrder != null)
                                                    {
                                                        if (castFilmingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)castFilmingWorkOrder.Width * (decimal)castFilmingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (castTransferingWorkOrder != null)
                                                    {
                                                        decimal tare = 0;
                                                        if (castTransferingWorkOrder.OuterPacking != null)
                                                        {
                                                            tare += castTransferingWorkOrder.OuterPacking.Weight;
                                                        }
                                                        if (castTransferingWorkOrder.Bobbin != null)
                                                        {
                                                            decimal multiplier = salesOrderDetail.Product.OuterPackingInPiece != 0 ? salesOrderDetail.Product.OuterPackingInPiece : 1;
                                                            tare += ((decimal)castTransferingWorkOrder.Width * (decimal)castTransferingWorkOrder.Bobbin.Weight / 1000000) * multiplier;
                                                        }
                                                        netQuantity = GrossQuantity - tare;
                                                    }
                                                    if (balloonFilmingWorkOrder != null)
                                                    {
                                                        if (balloonFilmingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)balloonFilmingWorkOrder.Width * (decimal)balloonFilmingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (printingWorkOrder != null)
                                                    {
                                                        if (printingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)printingWorkOrder.Width * (decimal)printingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (laminationWorkOrder != null)
                                                    {
                                                        if (laminationWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)laminationWorkOrder.Width * (decimal)laminationWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (slicingWorkOrder != null)
                                                    {
                                                        if (slicingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)slicingWorkOrder.Width * (decimal)slicingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (castSlicingWorkOrder != null)
                                                    {
                                                        if (castSlicingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)castSlicingWorkOrder.Width * (decimal)castSlicingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (cuttingWorkOrder != null)
                                                    {
                                                        decimal tare = 0;
                                                        if (cuttingWorkOrder.OuterPacking != null)
                                                        {
                                                            tare += cuttingWorkOrder.OuterPacking.Weight;
                                                        }
                                                        if (cuttingWorkOrder.Bobbin != null)
                                                        {
                                                            tare += ((decimal)cuttingWorkOrder.Width * (decimal)cuttingWorkOrder.Bobbin.Weight / 1000000);
                                                        }
                                                        netQuantity = GrossQuantity - tare;
                                                    }
                                                    if (eco6WorkOrder != null)
                                                    {
                                                        if (eco6WorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)eco6WorkOrder.Width * (decimal)eco6WorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (eco6CuttingWorkOrder != null)
                                                    {
                                                        if (eco6CuttingWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)eco6CuttingWorkOrder.Width * (decimal)eco6CuttingWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (eco6LaminationWorkOrder != null)
                                                    {
                                                        if (eco6LaminationWorkOrder.Bobbin != null)
                                                            netQuantity = GrossQuantity - ((decimal)eco6LaminationWorkOrder.Width * (decimal)eco6LaminationWorkOrder.Bobbin.Weight / 1000000);
                                                    }
                                                    if (foldingWorkOrder != null)
                                                    {
                                                        if (foldingWorkOrder.OuterPacking != null)
                                                            netQuantity = GrossQuantity - foldingWorkOrder.OuterPacking.Weight;
                                                    }
                                                    if (balloonCuttingWorkOrder != null)
                                                    {
                                                        if (balloonCuttingWorkOrder.OuterPacking != null)
                                                            netQuantity = GrossQuantity - balloonCuttingWorkOrder.OuterPacking.Weight;
                                                    }

                                                    decimal storeQuantity = GrossQuantity;
                                                    if (!IsLastProduction)
                                                    {
                                                        if (filmingWorkOrder != null)
                                                        {
                                                            if (!filmingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (castFilmingWorkOrder != null)
                                                        {
                                                            if (!castFilmingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (castTransferingWorkOrder != null)
                                                        {
                                                            if (!castTransferingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (balloonFilmingWorkOrder != null)
                                                        {
                                                            if (!balloonFilmingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (printingWorkOrder != null)
                                                        {
                                                            if (!printingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (laminationWorkOrder != null)
                                                        {
                                                            if (!laminationWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (slicingWorkOrder != null)
                                                        {
                                                            if (!slicingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (castSlicingWorkOrder != null)
                                                        {
                                                            if (!castSlicingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (cuttingWorkOrder != null)
                                                        {
                                                            if (!cuttingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (eco6WorkOrder != null)
                                                        {
                                                            if (!eco6WorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (eco6CuttingWorkOrder != null)
                                                        {
                                                            if (!eco6CuttingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (eco6LaminationWorkOrder != null)
                                                        {
                                                            if (!eco6LaminationWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (foldingWorkOrder != null)
                                                        {
                                                            if (!foldingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (balloonCuttingWorkOrder != null)
                                                        {
                                                            if (!balloonCuttingWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (regeneratedWorkOrder != null)
                                                        {
                                                            if (!regeneratedWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                        if (castRegeneratedWorkOrder != null)
                                                        {
                                                            if (!castRegeneratedWorkOrder.NextStation.IsLastStation) storeQuantity = netQuantity;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (salesOrderDetail.Unit.Code == "NKG")
                                                        {
                                                            storeQuantity = netQuantity;
                                                        }
                                                    }

                                                    decimal cquantity = 0;
                                                    if (salesOrderDetail.Unit.Code == "AD")
                                                    {
                                                        cquantity = salesOrderDetail.Product.OuterPackingInPiece;
                                                    }
                                                    else if (salesOrderDetail.Unit.Code == "RL")
                                                    {
                                                        cquantity = salesOrderDetail.Product.OuterPackingRollCount;
                                                    }
                                                    else if (salesOrderDetail.Unit.Code == "KL")
                                                    {
                                                        cquantity = 1;
                                                    }
                                                    else if (salesOrderDetail.Unit.Code == "PK")
                                                    {
                                                        cquantity = 1;
                                                    }
                                                    else if (salesOrderDetail.Unit.Code == "M2")
                                                    {
                                                        cquantity = Length != 0 ? salesOrderDetail.Product.Width * Length / 1000 : salesOrderDetail.Product.Width * salesOrderDetail.Product.Lenght / 1000;
                                                    }
                                                    else if (salesOrderDetail.Unit.Code == "MT")
                                                    {
                                                        cquantity = Length != 0 ? Length : salesOrderDetail.Product.Lenght;
                                                    }
                                                    else if (salesOrderDetail.Unit.Code == "NKG")
                                                    {
                                                        if (salesOrderDetail.Product.NetKg != 0) cquantity = salesOrderDetail.Product.NetKg;
                                                        else cquantity = netQuantity;
                                                    }
                                                    else
                                                    {
                                                        cquantity = storeQuantity;
                                                    }

                                                    //Yeni bir malzeme başlık ID alınır.
                                                    var headerId = Guid.NewGuid();
                                                    #region Üretime Çıkış Hareketleri
                                                    //Çıkış hareket türü
                                                    var output = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P111'"));
                                                    if (filmingWorkOrder != null)
                                                    {
                                                        var reciept = filmingWorkOrder.FilmingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit)); ;
                                                                if (store != null)
                                                                {
                                                                    if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
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
                                                                            Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                            cUnit = item.Unit,
                                                                            cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                        };
                                                                    }
                                                                }
                                                                else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                            }
                                                        }
                                                    }
                                                    if (castFilmingWorkOrder != null)
                                                    {
                                                        var reciept = castFilmingWorkOrder.CastFilmingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                if (store != null)
                                                                {
                                                                    if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
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
                                                                            Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                            cUnit = item.Unit,
                                                                            cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                        };
                                                                    }
                                                                }
                                                                else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                            }
                                                        }
                                                    }
                                                    if (castTransferingWorkOrder != null)
                                                    {
                                                        var reciept = castTransferingWorkOrder.CastTransferingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = castTransferingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (balloonFilmingWorkOrder != null)
                                                    {
                                                        var reciept = balloonFilmingWorkOrder.BalloonFilmingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                if (store != null)
                                                                {
                                                                    if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
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
                                                                            Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                            cUnit = item.Unit,
                                                                            cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                        };
                                                                    }
                                                                }
                                                                else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                            }
                                                        }
                                                    }
                                                    else if (printingWorkOrder != null)
                                                    {
                                                        var reciept = printingWorkOrder.PrintingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = printingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (laminationWorkOrder != null)
                                                    {
                                                        var reciept = laminationWorkOrder.LaminationWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = laminationWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (slicingWorkOrder != null)
                                                    {
                                                        var reciept = slicingWorkOrder.SlicingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = slicingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (castSlicingWorkOrder != null)
                                                    {
                                                        var reciept = castSlicingWorkOrder.CastSlicingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = castSlicingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (cuttingWorkOrder != null)
                                                    {
                                                        var reciept = cuttingWorkOrder.CuttingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = cuttingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (eco6WorkOrder != null)
                                                    {
                                                        var reciept = eco6WorkOrder.Eco6WorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                //if (item.ResourceObligatory)
                                                                //{
                                                                //    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                //    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                //    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                //    else
                                                                //    {
                                                                //        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                //        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                //        if (readResourceOid != null)
                                                                //        {
                                                                //            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                //            if (readResource != null)
                                                                //            {
                                                                //                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                //                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                //                {
                                                                //                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                //                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                //                    {
                                                                //                        HeaderId = headerId,
                                                                //                        ProductionBarcode = Barcode,
                                                                //                        ReadResource = readResource,
                                                                //                        Unit = readResource.Unit,
                                                                //                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                //                    };
                                                                //                }
                                                                //                else
                                                                //                {
                                                                //                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                //                    readResource.IsConsume = true;
                                                                //                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                //                    {
                                                                //                        HeaderId = headerId,
                                                                //                        ProductionBarcode = Barcode,
                                                                //                        ReadResource = readResource,
                                                                //                        Unit = readResource.Unit,
                                                                //                        Quantity = readResource.Quantity - rrmt
                                                                //                    };

                                                                //                    if (!IsLastRoll)
                                                                //                    {
                                                                //                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                //                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                //                        if (readResourceOid != null)
                                                                //                        {
                                                                //                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                //                            if (readResource != null)
                                                                //                            {
                                                                //                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                //                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                //                                {
                                                                //                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
                                                                //                                    var rrMovement = new ReadResourceMovement(Session)
                                                                //                                    {
                                                                //                                        HeaderId = headerId,
                                                                //                                        ProductionBarcode = Barcode,
                                                                //                                        ReadResource = readResource,
                                                                //                                        Unit = readResource.Unit,
                                                                //                                        Quantity = remaininQuantity
                                                                //                                    };
                                                                //                                }
                                                                //                            }
                                                                //                        }
                                                                //                    }
                                                                //                }
                                                                //            }
                                                                //        }
                                                                //    }
                                                                //}
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (eco6CuttingWorkOrder != null)
                                                    {
                                                        var reciept = eco6CuttingWorkOrder.Eco6CuttingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = eco6CuttingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (eco6LaminationWorkOrder != null)
                                                    {
                                                        var reciept = eco6LaminationWorkOrder.Eco6LaminationWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = eco6LaminationWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (foldingWorkOrder != null)
                                                    {
                                                        var reciept = foldingWorkOrder.FoldingWorkOrderReciepts;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = foldingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (balloonCuttingWorkOrder != null)
                                                    {
                                                        var reciept = balloonCuttingWorkOrder.BalloonCuttingWorkOrderReciept;
                                                        if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                        {
                                                            foreach (var item in reciept)
                                                            {
                                                                if (item.ResourceObligatory)
                                                                {
                                                                    var readResourceTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("WorkOrderNumber = ? And Production.SalesOrderDetail.Product = ?", WorkOrderNumber, item.Product)));
                                                                    var readResourceMovementTotal = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource.WorkOrderNumber = ?", WorkOrderNumber)));
                                                                    if (readResourceTotal == 0 || readResourceTotal < readResourceMovementTotal) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                    else
                                                                    {
                                                                        var readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@workOrderNumber" }, new object[] { WorkOrderNumber });
                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                        if (readResourceOid != null)
                                                                        {
                                                                            var readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                            if (readResource != null)
                                                                            {
                                                                                decimal rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                if (readResource.Quantity >= rrmt + storeQuantity)
                                                                                {
                                                                                    if (readResource.Quantity == rrmt + (storeQuantity * (decimal)item.Rate / 100)) readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = storeQuantity * (decimal)item.Rate / 100
                                                                                    };
                                                                                }
                                                                                else
                                                                                {
                                                                                    decimal remaininQuantity = (storeQuantity * (decimal)item.Rate / 100) - (readResource.Quantity - rrmt);
                                                                                    readResource.IsConsume = true;
                                                                                    var readResourceMovement = new ReadResourceMovement(Session)
                                                                                    {
                                                                                        HeaderId = headerId,
                                                                                        ProductionBarcode = Barcode,
                                                                                        ReadResource = readResource,
                                                                                        Unit = readResource.Unit,
                                                                                        Quantity = readResource.Quantity - rrmt
                                                                                    };

                                                                                    if (!IsLastRoll)
                                                                                    {
                                                                                        readResourceOid = Session.ExecuteScalar("select top 1 Oid from ReadResource where GCRecord is null and IsConsume = 0 and Barcode != @barcode and WorkOrderNumber = @workOrderNumber order by IncomingDate asc", new string[] { "@barcode", "@workOrderNumber" }, new object[] { readResource.Barcode, WorkOrderNumber });
                                                                                        if (readResourceOid == null) throw new UserFriendlyException("Bu teyidi verebilmek için kaynak okutma yapınız !...");
                                                                                        if (readResourceOid != null)
                                                                                        {
                                                                                            readResource = Session.GetObjectByKey<ReadResource>(readResourceOid);
                                                                                            if (readResource != null)
                                                                                            {
                                                                                                rrmt = Convert.ToDecimal(Session.Evaluate(typeof(ReadResourceMovement), CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("ReadResource = ?", readResource)));
                                                                                                if (remaininQuantity <= rrmt + storeQuantity)
                                                                                                {
                                                                                                    if (remaininQuantity == rrmt + storeQuantity) readResource.IsConsume = true;
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
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!item.Product.ProductGroup.Name.Contains("MAMUL"))
                                                                {
                                                                    var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                                    if (store != null)
                                                                    {
                                                                        if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                                            throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz. Kaynak okutma gerekektedir. !...", item.Product.Code, item.Product.Name));
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
                                                                                Warehouse = balloonCuttingWorkOrder.Station.SourceWarehouse,
                                                                                MovementType = output,
                                                                                Unit = item.Unit,
                                                                                Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                                                cUnit = item.Unit,
                                                                                cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                                            };
                                                                        }
                                                                    }
                                                                    else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //else if (regeneratedWorkOrder != null)
                                                    //{
                                                    //    var reciept = regeneratedWorkOrder.RegeneratedWorkOrderReciepts;
                                                    //    if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                    //    {
                                                    //        foreach (var item in reciept)
                                                    //        {
                                                    //            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                    //            if (store != null)
                                                    //            {
                                                    //                if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                    //                    throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
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
                                                    //                        Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                    //                        cUnit = item.Unit,
                                                    //                        cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                    //                    };
                                                    //                }
                                                    //            }
                                                    //            else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else if (castRegeneratedWorkOrder != null)
                                                    //{
                                                    //    var reciept = castRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts;
                                                    //    if (reciept.Count > 0) //Malzeme ihtiyaçları girilmişse depodan çıkış hareketleri yapılır.
                                                    //    {
                                                    //        foreach (var item in reciept)
                                                    //        {
                                                    //            Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                                                    //            if (store != null)
                                                    //            {
                                                    //                if (store.Quantity - netQuantity * (decimal)item.Rate / 100 < 0)
                                                    //                    throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
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
                                                    //                        Quantity = storeQuantity * (decimal)item.Rate / 100,
                                                    //                        cUnit = item.Unit,
                                                    //                        cQuantity = storeQuantity * (decimal)item.Rate / 100
                                                    //                    };
                                                    //                }
                                                    //            }
                                                    //            else throw new UserFriendlyException(string.Format("{0}-{1} Kaynak depoda stok bakiyesi yetersiz !...", item.Product.Code, item.Product.Name));
                                                    //        }
                                                    //    }
                                                    //}
                                                    #endregion

                                                    #region Üretim Kaydı

                                                    FilmingWorkOrder = filmingWorkOrder;
                                                    CastFilmingWorkOrder = castFilmingWorkOrder;
                                                    CastTransferingWorkOrder = castTransferingWorkOrder;
                                                    PrintingWorkOrder = printingWorkOrder;
                                                    LaminationWorkOrder = laminationWorkOrder;
                                                    SlicingWorkOrder = slicingWorkOrder;
                                                    CastSlicingWorkOrder = castSlicingWorkOrder;
                                                    BalloonFilmingWorkOrder = balloonFilmingWorkOrder;
                                                    CuttingWorkOrder = cuttingWorkOrder;
                                                    FoldingWorkOrder = foldingWorkOrder;
                                                    BalloonCuttingWorkOrder = balloonCuttingWorkOrder;
                                                    RegeneratedWorkOrder = regeneratedWorkOrder;
                                                    CastRegeneratedWorkOrder = castRegeneratedWorkOrder;
                                                    Eco6WorkOrder = eco6WorkOrder;
                                                    Eco6CuttingWorkOrder = eco6CuttingWorkOrder;
                                                    Eco6LaminationWorkOrder = eco6LaminationWorkOrder;
                                                    Machine = machine;
                                                    SalesOrderDetail = salesOrderDetail;
                                                    ProductionPalette = activePalette;
                                                    Shift = shiftStart;
                                                    Employee = shiftAssignment.Employee;
                                                    Unit = unit;
                                                    NetQuantity = netQuantity;
                                                    cUnit = salesOrderDetail.Unit;
                                                    cQuantity = cquantity;

                                                    //Teorik hesaplamalar
                                                    if (IsLastProduction)
                                                    {
                                                        TheoricPieceWeight = pieceWeight;
                                                        TheoricRollWeight = rollWeight;
                                                        if (salesOrderDetail.Unit.Code == "AD")
                                                        {
                                                            TheoricNetQuantity = (pieceWeight * (salesOrderDetail.Product.OuterPackingRollCount * salesOrderDetail.Product.InnerPackingInPiece)) / 1000;
                                                        }
                                                        else if (salesOrderDetail.Unit.Code == "RL")
                                                        {
                                                            TheoricNetQuantity = pieceWeight * salesOrderDetail.Product.OuterPackingRollCount;
                                                        }
                                                        else if (salesOrderDetail.Unit.Code == "KL")
                                                        {
                                                            TheoricNetQuantity = rollWeight;
                                                        }
                                                        else if (salesOrderDetail.Unit.Code == "PK")
                                                        {
                                                            TheoricNetQuantity = rollWeight;
                                                        }
                                                        else if (salesOrderDetail.Unit.Code == "M2")
                                                        {
                                                            TheoricNetQuantity = Length != 0 ? salesOrderDetail.Product.Width * Length / 1000 : salesOrderDetail.Product.Width * salesOrderDetail.Product.Lenght / 1000;
                                                        }
                                                        else if (salesOrderDetail.Unit.Code == "MT")
                                                        {
                                                            TheoricNetQuantity = Length != 0 ? Length : salesOrderDetail.Product.Lenght;
                                                        }
                                                        else
                                                        {
                                                            TheoricNetQuantity = rollWeight;
                                                        }
                                                        TheoricGrossQuantity = TheoricNetQuantity + (GrossQuantity - netQuantity);
                                                    }

                                                    ProductionDate = Helpers.GetSystemDate(Session);
                                                    if (filmingWorkOrder != null) { IsLastProduction = filmingWorkOrder.NextStation.IsLastStation; }
                                                    if (castFilmingWorkOrder != null) { IsLastProduction = castFilmingWorkOrder.NextStation.IsLastStation; }
                                                    if (castTransferingWorkOrder != null) { IsLastProduction = castTransferingWorkOrder.NextStation.IsLastStation; }
                                                    if (balloonFilmingWorkOrder != null) { IsLastProduction = balloonFilmingWorkOrder.NextStation.IsLastStation; }
                                                    if (printingWorkOrder != null) { IsLastProduction = printingWorkOrder.NextStation.IsLastStation; }
                                                    if (laminationWorkOrder != null) { IsLastProduction = laminationWorkOrder.NextStation.IsLastStation; }
                                                    if (slicingWorkOrder != null) { IsLastProduction = slicingWorkOrder.NextStation.IsLastStation; }
                                                    if (castSlicingWorkOrder != null) { IsLastProduction = castSlicingWorkOrder.NextStation.IsLastStation; }
                                                    if (cuttingWorkOrder != null) { IsLastProduction = cuttingWorkOrder.NextStation.IsLastStation; }
                                                    if (foldingWorkOrder != null) { IsLastProduction = foldingWorkOrder.NextStation.IsLastStation; }
                                                    if (balloonCuttingWorkOrder != null) { IsLastProduction = balloonCuttingWorkOrder.NextStation.IsLastStation; }
                                                    if (regeneratedWorkOrder != null) { IsLastProduction = regeneratedWorkOrder.NextStation.IsLastStation; }
                                                    if (castRegeneratedWorkOrder != null) { IsLastProduction = castRegeneratedWorkOrder.NextStation.IsLastStation; }
                                                    if (eco6WorkOrder != null) { IsLastProduction = eco6WorkOrder.NextStation.IsLastStation; }
                                                    if (eco6CuttingWorkOrder != null) { IsLastProduction = eco6CuttingWorkOrder.NextStation.IsLastStation; }
                                                    if (eco6LaminationWorkOrder != null) { IsLastProduction = eco6LaminationWorkOrder.NextStation.IsLastStation; }

                                                    //CalculateThickness
                                                    if (filmingWorkOrder != null)
                                                    {
                                                        int width = 0;
                                                        if (filmingWorkOrder.BellowsStatus == BellowsStatus.None) width = filmingWorkOrder.Width;
                                                        else
                                                        {
                                                            decimal bellow = Convert.ToDecimal(filmingWorkOrder.Bellows);
                                                            width = filmingWorkOrder.ShippingFilmType.Name.Contains("HORTUM") ? filmingWorkOrder.Width + Convert.ToInt32(bellow * 4) : filmingWorkOrder.Width + Convert.ToInt32(bellow * 2);
                                                        }
                                                        Thickness = (netQuantity / ((Convert.ToDecimal(width) / 1000) * Length * filmingWorkOrder.Density)) * 1000;
                                                        ThicknessDeviation = Thickness / filmingWorkOrder.Thickness * 100;
                                                    }

                                                    //Lot No Oluşturma
                                                    if (SalesOrderDetail.StickerDesign.DisplayName.Contains("Lot Numaralı"))
                                                    {
                                                        string shift = Shift.WorkShift.Name == "A" ? "1" : Shift.WorkShift.Name == "B" ? "2" : "3";
                                                        string month = string.Empty;
                                                        switch (Helpers.GetSystemDate(Session).Month)
                                                        {
                                                            case 1:
                                                                month = "A";
                                                                break;
                                                            case 2:
                                                                month = "B";
                                                                break;
                                                            case 3:
                                                                month = "C";
                                                                break;
                                                            case 4:
                                                                month = "D";
                                                                break;
                                                            case 5:
                                                                month = "E";
                                                                break;
                                                            case 6:
                                                                month = "F";
                                                                break;
                                                            case 7:
                                                                month = "G";
                                                                break;
                                                            case 8:
                                                                month = "H";
                                                                break;
                                                            case 9:
                                                                month = "I";
                                                                break;
                                                            case 10:
                                                                month = "J";
                                                                break;
                                                            case 11:
                                                                month = "K";
                                                                break;
                                                            case 12:
                                                                month = "L";
                                                                break;
                                                        }
                                                        if (filmingWorkOrder != null)
                                                        {
                                                            LotNumber = string.Format("#{0}{1}{2}{3}", filmingWorkOrder.Machine.LotNumber, shift, month, Helpers.FillZero(Helpers.GetSystemDate(Session).Day.ToString(), 2));
                                                        }
                                                        if (printingWorkOrder != null)
                                                        {
                                                            LotNumber = string.Format("#{0}{1}", printingWorkOrder.Machine.LotNumber, shift);
                                                        }
                                                        if (cuttingWorkOrder != null)
                                                        {
                                                            LotNumber = string.Format("#{0}{1}{2}{3}{4}", cuttingWorkOrder.Machine.LotNumber, shift, month, Helpers.FillZero(Helpers.GetSystemDate(Session).Day.ToString(), 2), Helpers.GetSystemDate(Session).Year.ToString().Substring(3, 1));
                                                        }
                                                    }

                                                    #endregion

                                                    #region İş Emri Durumları
                                                    if (filmingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(FilmingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", filmingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (FilmingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != filmingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        filmingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (filmingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) filmingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (castFilmingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(CastFilmingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", castFilmingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (CastFilmingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != castFilmingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        castFilmingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (castFilmingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) castFilmingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (castTransferingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(CastTransferingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", castTransferingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (CastTransferingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != castTransferingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        castTransferingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (castTransferingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) castTransferingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (balloonFilmingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(BalloonFilmingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", balloonFilmingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (BalloonFilmingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != balloonFilmingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        balloonFilmingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (balloonFilmingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) balloonFilmingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (printingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(PrintingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", printingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (PrintingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != printingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        printingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (printingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) printingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (laminationWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(LaminationWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", laminationWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (LaminationWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != laminationWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        laminationWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (laminationWorkOrder.BeginDate < new DateTime(2010, 1, 1)) laminationWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (slicingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(SlicingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", slicingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (SlicingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != slicingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        slicingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (slicingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) slicingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (castSlicingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(CastSlicingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", castSlicingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (CastSlicingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != castSlicingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        castSlicingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (castSlicingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) castSlicingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (cuttingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(CuttingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", cuttingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (CuttingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != cuttingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        cuttingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (cuttingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) cuttingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (foldingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(FoldingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", foldingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (FoldingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != foldingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        foldingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                    }
                                                    if (balloonCuttingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(BalloonCuttingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", balloonCuttingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (BalloonCuttingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != balloonCuttingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        balloonCuttingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                    }
                                                    if (regeneratedWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(RegeneratedWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", regeneratedWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (RegeneratedWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != regeneratedWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        regeneratedWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (regeneratedWorkOrder.BeginDate < new DateTime(2010, 1, 1)) regeneratedWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (castRegeneratedWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(CastRegeneratedWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", castRegeneratedWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (CastRegeneratedWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != castRegeneratedWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        castRegeneratedWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                    }
                                                    if (eco6WorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(Eco6WorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", eco6WorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (Eco6WorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != eco6WorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        eco6WorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (eco6WorkOrder.BeginDate < new DateTime(2010, 1, 1)) eco6WorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (eco6CuttingWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(Eco6CuttingWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", eco6CuttingWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (Eco6CuttingWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != eco6CuttingWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        eco6CuttingWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (eco6CuttingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) eco6CuttingWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    if (eco6LaminationWorkOrder != null)
                                                    {
                                                        XPCollection activeWorkOrders = new XPCollection(Session, typeof(Eco6LaminationWorkOrder), CriteriaOperator.Parse("Machine = ? and WorkOrderStatus = 'ProductionStage'", eco6LaminationWorkOrder.Machine));
                                                        if (activeWorkOrders != null)
                                                        {
                                                            foreach (Eco6LaminationWorkOrder item in activeWorkOrders)
                                                            {
                                                                if (item != eco6LaminationWorkOrder)
                                                                {
                                                                    if (item.WorkOrderStatus != WorkOrderStatus.Canceled && item.WorkOrderStatus != WorkOrderStatus.ProductionComplete)
                                                                        item.WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
                                                                }
                                                            }
                                                        }
                                                        eco6LaminationWorkOrder.WorkOrderStatus = WorkOrderStatus.ProductionStage;
                                                        if (eco6LaminationWorkOrder.BeginDate < new DateTime(2010, 1, 1)) eco6LaminationWorkOrder.BeginDate = Helpers.GetSystemDate(Session);
                                                    }
                                                    #endregion

                                                    //Son üreitm ise ve palet açılmamışsa sistem otomatik bir palet numarası üretir.
                                                    string newPaletteNumber = string.Empty;
                                                    if (productionWarehouse.ShippingWarehouse)
                                                    {
                                                        if (activePalette == null)
                                                        {
                                                            newPaletteNumber = Helpers.GetDocumentNumber(Session, "EcoplastERP.Module.BusinessObjects.ProductionObjects.ProductionPalette");
                                                        }
                                                    }

                                                    #region Üretime Giriş Hareketi
                                                    //Üretilen ürünün giriş hareketi yapılır.
                                                    var input = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P110'"));
                                                    var imovement = new Movement(Session)
                                                    {
                                                        HeaderId = headerId,
                                                        DocumentNumber = WorkOrderNumber,
                                                        DocumentDate = Helpers.GetSystemDate(Session),
                                                        Barcode = Barcode,
                                                        SalesOrderDetail = salesOrderDetail,
                                                        Product = salesOrderDetail.Product,
                                                        PartyNumber = string.Empty,
                                                        PaletteNumber = activePalette != null ? activePalette.PaletteNumber : newPaletteNumber,
                                                        Warehouse = productionWarehouse,
                                                        MovementType = input,
                                                        Unit = salesOrderDetail.Unit,
                                                        Quantity = cquantity,
                                                        cUnit = salesOrderDetail.cUnit,
                                                        cQuantity = storeQuantity
                                                    };
                                                    #endregion
                                                }
                                                else throw new UserFriendlyException(string.Format("Bu üretim için paletteki adet sayısı tamamlandığından üretim teyidi verilemez. Yeni bir palet tanımlayınız. Palet bobin sayısı: {0:n0} olarak tanımlanmıştır.", paletteBobbinCount));
                                            }
                                            else throw new UserFriendlyException(string.Format("Bu üretim için verilen opsiyondan fazla üretim yapıldı. Üretim teyidi verilemez. Verilen Opsiyon %{0}", productionOption));
                                        }
                                        else throw new UserFriendlyException(string.Format("Rulo ağırlıkları üretim siparişinde verilen opsiyonlarla uyuşmuyor. Bu sipariş için ağırlıklar {0:n2} kg. ve {1:n2} kg. aralığında olmalıdır.", minWeight, maxWeight));
                                    }
                                    else throw new UserFriendlyException("Üretim siparişinin aktif paleti için palet teyidi verilmiş. Tartıma devam edebilmek için yeni bir palet tanımlaması yapınız.");
                                }
                                else throw new UserFriendlyException("Makineye operatör ataması yapılmamış. Üretim teyidi verilemez.");
                            }
                            else throw new UserFriendlyException("Operatör olarak herhangi bir personel görev tanımı yapılmamış.");
                        }
                        else throw new UserFriendlyException("Vardiya başlatılmamış. Üretim teyidi verilemez.");
                    }
                    else throw new UserFriendlyException("Üretim siparişi durumu Üretimi Tamamlanmış veya Vazgeçilmiş olarak ayarlanmıştır. Üretim teyidi verilemez.");
                }
                else throw new UserFriendlyException("Üretim teyidi alabilmek için basküle bir ürün yerleştiriniz.");
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            AuditTrailService.Instance.EndSessionAudit(Session);
            var store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", Barcode));
            if (store != null)
            {
                if (ProductionPalette != null)
                {
                    if (ProductionPalette.LastWeight > 0)
                    {
                        throw new UserFriendlyException("Palet teyidi verilmiş barkod silinemez.");
                    }
                }
                decimal quantity = GrossQuantity;
                if (!IsLastProduction) quantity = NetQuantity;
                //Yeni bir malzeme başlık ID alınır.
                var headerId = Guid.NewGuid();
                #region Üretime Çıkış İptali
                //Çıkış hareket türü
                var input = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P114'"));
                if (FilmingWorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = FilmingWorkOrder.FilmingWorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = FilmingWorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                if (CastFilmingWorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = CastFilmingWorkOrder.CastFilmingWorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = CastFilmingWorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                if (CastTransferingWorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = CastTransferingWorkOrder.CastTransferingWorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = CastTransferingWorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                if (BalloonFilmingWorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = BalloonFilmingWorkOrder.BalloonFilmingWorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = BalloonFilmingWorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                //else if (PrintingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = PrintingWorkOrder.PrintingWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = PrintingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (LaminationWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = LaminationWorkOrder.LaminationWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = LaminationWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (SlicingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = SlicingWorkOrder.SlicingWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = SlicingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (CastSlicingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = CastSlicingWorkOrder.CastSlicingWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = CastSlicingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (CuttingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = CuttingWorkOrder.CuttingWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = CuttingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (FoldingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = FoldingWorkOrder.FoldingWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = FoldingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (BalloonCuttingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = BalloonCuttingWorkOrder.BalloonCuttingWorkOrderReciept;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = BalloonCuttingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                else if (RegeneratedWorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = RegeneratedWorkOrder.RegeneratedWorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = RegeneratedWorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                else if (CastRegeneratedWorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = CastRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = CastRegeneratedWorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                else if (Eco6WorkOrder != null)
                {
                    //İş emrinde girilen malzeme ihtiyaçları alınır.
                    var reciept = Eco6WorkOrder.Eco6WorkOrderReciepts;
                    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    {
                        foreach (var item in reciept)
                        {
                            var omovement = new Movement(Session)
                            {
                                HeaderId = headerId,
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = Helpers.GetSystemDate(Session),
                                Barcode = string.Empty,
                                Product = item.Product,
                                PartyNumber = string.Empty,
                                PaletteNumber = string.Empty,
                                Warehouse = Eco6WorkOrder.Station.SourceWarehouse,
                                MovementType = input,
                                Unit = item.Unit,
                                Quantity = quantity * (decimal)item.Rate / 100
                            };
                        }
                    }
                }
                //else if (Eco6CuttingWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = Eco6CuttingWorkOrder.Eco6CuttingWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = Eco6CuttingWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                //else if (Eco6LaminationWorkOrder != null)
                //{
                //    //İş emrinde girilen malzeme ihtiyaçları alınır.
                //    var reciept = Eco6LaminationWorkOrder.Eco6LaminationWorkOrderReciepts;
                //    if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                //    {
                //        foreach (var item in reciept)
                //        {
                //            var omovement = new Movement(Session)
                //            {
                //                HeaderId = headerId,
                //                DocumentNumber = WorkOrderNumber,
                //                DocumentDate = Helpers.GetSystemDate(Session),
                //                Barcode = string.Empty,
                //                Product = item.Product,
                //                PartyNumber = string.Empty,
                //                PaletteNumber = string.Empty,
                //                Warehouse = Eco6LaminationWorkOrder.Station.SourceWarehouse,
                //                MovementType = input,
                //                Unit = item.Unit,
                //                Quantity = quantity * (decimal)item.Rate / 100
                //            };
                //        }
                //    }
                //}
                #endregion
                #region Üretim İptali Hareketi
                //Üretilen ürünün iptal hareketi yapılır.
                var output = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P115'"));
                var imovement = new Movement(Session)
                {
                    HeaderId = headerId,
                    DocumentNumber = WorkOrderNumber,
                    DocumentDate = Helpers.GetSystemDate(Session),
                    Barcode = store.Barcode,
                    SalesOrderDetail = store.SalesOrderDetail,
                    Product = store.SalesOrderDetail.Product,
                    PartyNumber = store.PartyNumber,
                    PaletteNumber = store.PaletteNumber,
                    Warehouse = store.Warehouse,
                    MovementType = output,
                    Unit = store.Unit,
                    Quantity = store.Quantity,
                    cUnit = store.cUnit,
                    cQuantity = store.cQuantity
                };
                #endregion
            }
        }
        // Fields...        
        private bool _IsLastRoll;
        private bool _IsLastProduction;
        private DateTime _ProductionDate;
        private decimal _RollDiameter;
        private decimal _TheoricGrossQuantity;
        private decimal _TheoricNetQuantity;
        private decimal _TheoricRollWeight;
        private decimal _TheoricPieceWeight;
        private decimal _ThicknessDeviation;
        private decimal _Thickness;
        private int _Length;
        private decimal _LastQuantity;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _NetQuantity;
        private decimal _GrossQuantity;
        private Unit _Unit;
        private Employee _Employee;
        private ShiftStart _Shift;
        private ProductionPalette _ProductionPalette;
        private SalesOrderDetail _SalesOrderDetail;
        private Machine _Machine;
        private string _LotNumber;
        private string _Barcode;
        private Eco6LaminationWorkOrder _Eco6LaminationWorkOrder;
        private Eco6CuttingWorkOrder _Eco6CuttingWorkOrder;
        private Eco6WorkOrder _Eco6WorkOrder;
        private CastRegeneratedWorkOrder _CastRegeneratedWorkOrder;
        private RegeneratedWorkOrder _RegeneratedWorkOrder;
        private BalloonCuttingWorkOrder _BalloonCuttingWorkOrder;
        private FoldingWorkOrder _FoldingWorkOrder;
        private CuttingWorkOrder _CuttingWorkOrder;
        private CastSlicingWorkOrder _CastSlicingWorkOrder;
        private SlicingWorkOrder _SlicingWorkOrder;
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
        [Association("FilmingWorkOrder-Productions")]
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
        [Association("CastFilmingWorkOrder-Productions")]
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
        [Association("CastTransferingWorkOrder-Productions")]
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
        [Association("BalloonFilmingWorkOrder-Productions")]
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
        [Association("PrintingWorkOrder-Productions")]
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
        [Association("LaminationWorkOrder-Productions")]
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
        [Association("SlicingWorkOrder-Productions")]
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
        [Association("CastSlicingWorkOrder-Productions")]
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
        [Association("CuttingWorkOrder-Productions")]
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
        [Association("FoldingWorkOrder-Productions")]
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
        [Association("BalloonCuttingWorkOrder-Productions")]
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
        [Association("RegeneratedWorkOrder-Productions")]
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
        [Association("CastRegeneratedWorkOrder-Productions")]
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
        [Association("Eco6WorkOrder-Productions")]
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
        [Association("Eco6CuttingWorkOrder-Productions")]
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
        [Association("Eco6LaminationWorkOrder-Productions")]
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
        [Indexed(Unique = true)]
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LotNumber
        {
            get
            {
                return _LotNumber;
            }
            set
            {
                SetPropertyValue("LotNumber", ref _LotNumber, value);
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

        [Association("ProductionPalette-Productions")]
        public ProductionPalette ProductionPalette
        {
            get
            {
                return _ProductionPalette;
            }
            set
            {
                SetPropertyValue("ProductionPalette", ref _ProductionPalette, value);
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

        public decimal LastQuantity
        {
            get
            {
                return _LastQuantity;
            }
            set
            {
                SetPropertyValue("LastQuantity", ref _LastQuantity, value);
            }
        }

        public int Length
        {
            get
            {
                return _Length;
            }
            set
            {
                SetPropertyValue("Length", ref _Length, value);
            }
        }

        public decimal Thickness
        {
            get
            {
                return _Thickness;
            }
            set
            {
                SetPropertyValue("Thickness", ref _Thickness, value);
            }
        }

        public decimal ThicknessDeviation
        {
            get
            {
                return _ThicknessDeviation;
            }
            set
            {
                SetPropertyValue("ThicknessDeviation", ref _ThicknessDeviation, value);
            }
        }

        public decimal TheoricThickness
        {
            get
            {
                decimal thickness = 0;
                if (IsLastProduction) thickness = SalesOrderDetail.Product.Thickness;
                else
                {
                    if (FilmingWorkOrder != null) thickness = FilmingWorkOrder.Thickness;
                    if (CastFilmingWorkOrder != null) thickness = CastFilmingWorkOrder.Thickness;
                    if (CastTransferingWorkOrder != null) thickness = CastTransferingWorkOrder.Thickness;
                    if (PrintingWorkOrder != null) thickness = PrintingWorkOrder.Thickness;
                    if (LaminationWorkOrder != null) thickness = LaminationWorkOrder.Thickness;
                    if (SlicingWorkOrder != null) thickness = SlicingWorkOrder.Thickness;
                    if (CastSlicingWorkOrder != null) thickness = CastSlicingWorkOrder.Thickness;
                    if (CuttingWorkOrder != null) thickness = CuttingWorkOrder.Thickness;
                    if (Eco6WorkOrder != null) thickness = Eco6WorkOrder.Thickness;
                    if (Eco6CuttingWorkOrder != null) thickness = Eco6CuttingWorkOrder.Thickness;
                    if (Eco6LaminationWorkOrder != null) thickness = Eco6LaminationWorkOrder.Thickness;
                }
                return thickness;
            }
        }

        public decimal TheoricPieceWeight
        {
            get
            {
                return _TheoricPieceWeight;
            }
            set
            {
                SetPropertyValue("TheoricPieceWeight", ref _TheoricPieceWeight, value);
            }
        }

        public decimal TheoricRollWeight
        {
            get
            {
                return _TheoricRollWeight;
            }
            set
            {
                SetPropertyValue("TheoricRollWeight", ref _TheoricRollWeight, value);
            }
        }

        public decimal TheoricNetQuantity
        {
            get
            {
                return _TheoricNetQuantity;
            }
            set
            {
                SetPropertyValue("TheoricNetQuantity", ref _TheoricNetQuantity, value);
            }
        }

        public decimal TheoricGrossQuantity
        {
            get
            {
                return _TheoricGrossQuantity;
            }
            set
            {
                SetPropertyValue("TheoricGrossQuantity", ref _TheoricGrossQuantity, value);
            }
        }

        public decimal RollDiameter
        {
            get
            {
                return _RollDiameter;
            }
            set
            {
                SetPropertyValue("RollDiameter", ref _RollDiameter, value);
            }
        }

        [VisibleInDetailView(false)]
        public DateTime ProductionDate
        {
            get
            {
                return _ProductionDate;
            }
            set
            {
                SetPropertyValue("ProductionDate", ref _ProductionDate, value);
            }
        }

        [VisibleInDetailView(false)]
        public bool IsLastProduction
        {
            get
            {
                return _IsLastProduction;
            }
            set
            {
                SetPropertyValue("IsLastProduction", ref _IsLastProduction, value);
            }
        }

        [VisibleInDetailView(false)]
        public bool IsLastRoll
        {
            get
            {
                return _IsLastRoll;
            }
            set
            {
                SetPropertyValue("IsLastRoll", ref _IsLastRoll, value);
            }
        }

        public bool IsQuarantine
        {
            get
            {
                bool quarantine = false;
                Warehouse quarantineWarehouse = null;
                if (FilmingWorkOrder != null) quarantineWarehouse = FilmingWorkOrder.Station.QuarantineWarehouse;
                if (CastFilmingWorkOrder != null) quarantineWarehouse = CastFilmingWorkOrder.Station.QuarantineWarehouse;
                if (CastTransferingWorkOrder != null) quarantineWarehouse = CastTransferingWorkOrder.Station.QuarantineWarehouse;
                if (BalloonFilmingWorkOrder != null) quarantineWarehouse = BalloonFilmingWorkOrder.Station.QuarantineWarehouse;
                if (PrintingWorkOrder != null) quarantineWarehouse = PrintingWorkOrder.Station.QuarantineWarehouse;
                if (LaminationWorkOrder != null) quarantineWarehouse = LaminationWorkOrder.Station.QuarantineWarehouse;
                if (SlicingWorkOrder != null) quarantineWarehouse = SlicingWorkOrder.Station.QuarantineWarehouse;
                if (CastSlicingWorkOrder != null) quarantineWarehouse = CastSlicingWorkOrder.Station.QuarantineWarehouse;
                if (CuttingWorkOrder != null) quarantineWarehouse = CuttingWorkOrder.Station.QuarantineWarehouse;
                if (Eco6WorkOrder != null) quarantineWarehouse = Eco6WorkOrder.Station.QuarantineWarehouse;
                if (Eco6CuttingWorkOrder != null) quarantineWarehouse = Eco6CuttingWorkOrder.Station.QuarantineWarehouse;
                if (Eco6LaminationWorkOrder != null) quarantineWarehouse = Eco6LaminationWorkOrder.Station.QuarantineWarehouse;
                Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse = ?", Barcode, quarantineWarehouse));
                if (store != null) quarantine = true;
                return quarantine;
            }
        }

        [Association, Aggregated]
        public XPCollection<ProductionResource> ProductionResources
        {
            get { return GetCollection<ProductionResource>("ProductionResources"); }
        }
    }
}
