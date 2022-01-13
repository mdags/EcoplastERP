using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("DocumentNumber")]
    [NavigationItem("ProductManagement")]
    public class ProductionConsume : BaseObject
    {
        public ProductionConsume(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DocumentNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            DocumentDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this) && Session.Connection != null)
            {
                bool workOrderControl = false;
                var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (filmingWorkOrder != null) workOrderControl = true;
                var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castFilmingWorkOrder != null) workOrderControl = true;
                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castTransferingWorkOrder != null) workOrderControl = true;
                var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonFilmingWorkOrder != null) workOrderControl = true;
                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (printingWorkOrder != null) workOrderControl = true;
                var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (laminationWorkOrder != null) workOrderControl = true;
                var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (slicingWorkOrder != null) workOrderControl = true;
                var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castSlicingWorkOrder != null) workOrderControl = true;
                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (cuttingWorkOrder != null) workOrderControl = true;
                var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (foldingWorkOrder != null) workOrderControl = true;
                var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonCuttingWorkOrder != null) workOrderControl = true;
                var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (regeneratedWorkOrder != null) workOrderControl = true;
                var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castRegeneratedWorkOrder != null) workOrderControl = true;
                var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6WorkOrder != null) workOrderControl = true;
                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6CuttingWorkOrder != null) workOrderControl = true;
                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6LaminationWorkOrder != null) workOrderControl = true;

                if (!workOrderControl)
                {
                    throw new UserFriendlyException("Üretim Siparişi bulunamadı.");
                }

                foreach (ProductionConsumeDetail item in ProductionConsumeDetails)
                {
                    Guid headerId = Guid.NewGuid();
                    MovementType output = Session.FindObject<MovementType>(new BinaryOperator("Code", "P111"));
                    if (!string.IsNullOrEmpty(item.Barcode))
                    {
                        Store store = Session.FindObject<Store>(new BinaryOperator("Barcode", item.Barcode));
                        if (store != null)
                        {
                            Movement outputMovement = new Movement(Session);
                            outputMovement.HeaderId = headerId;
                            outputMovement.DocumentNumber = WorkOrderNumber;
                            outputMovement.DocumentDate = DocumentDate;
                            outputMovement.Barcode = item.Barcode;
                            outputMovement.SalesOrderDetail = store.SalesOrderDetail;
                            outputMovement.Product = store.Product;
                            outputMovement.PartyNumber = store.PartyNumber;
                            outputMovement.PaletteNumber = store.PaletteNumber;
                            outputMovement.Warehouse = store.Warehouse;
                            outputMovement.WarehouseCell = store.WarehouseCell;
                            outputMovement.MovementType = output;
                            outputMovement.Unit = store.Unit;
                            outputMovement.Quantity = store.Quantity;
                            outputMovement.cUnit = store.cUnit;
                            outputMovement.cQuantity = store.cQuantity;
                        }
                    }
                    else
                    {
                        SalesOrderDetail salesOrderDetail = null;
                        if (filmingWorkOrder != null) salesOrderDetail = filmingWorkOrder.SalesOrderDetail;
                        if (castFilmingWorkOrder != null) salesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;
                        if (castTransferingWorkOrder != null) salesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;
                        if (printingWorkOrder != null) salesOrderDetail = printingWorkOrder.SalesOrderDetail;
                        if (laminationWorkOrder != null) salesOrderDetail = laminationWorkOrder.SalesOrderDetail;
                        if (slicingWorkOrder != null) salesOrderDetail = slicingWorkOrder.SalesOrderDetail;
                        if (castSlicingWorkOrder != null) salesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;
                        if (cuttingWorkOrder != null) salesOrderDetail = cuttingWorkOrder.SalesOrderDetail;
                        if (regeneratedWorkOrder != null) salesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;

                        Movement outputMovement = new Movement(Session);
                        outputMovement.HeaderId = headerId;
                        outputMovement.DocumentNumber = WorkOrderNumber;
                        outputMovement.DocumentDate = DocumentDate;
                        outputMovement.Barcode = item.Barcode;
                        outputMovement.SalesOrderDetail = salesOrderDetail;
                        outputMovement.Product = item.Product;
                        outputMovement.PartyNumber = string.Empty;
                        outputMovement.PaletteNumber = string.Empty;
                        outputMovement.Warehouse = item.Warehouse;
                        outputMovement.WarehouseCell = item.WarehouseCell;
                        outputMovement.MovementType = output;
                        outputMovement.Unit = item.Unit;
                        outputMovement.Quantity = item.Quantity;
                        outputMovement.cUnit = item.Unit;
                        outputMovement.cQuantity = item.Quantity;
                    }
                }
            }
        }
        // Fields...
        private string _WorkOrderNumber;
        private DateTime _DocumentDate;
        private int _DocumentNumber;

        [RuleUniqueValue]
        [RuleRequiredField]
        public int DocumentNumber
        {
            get
            {
                return _DocumentNumber;
            }
            set
            {
                SetPropertyValue("DocumentNumber", ref _DocumentNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime DocumentDate
        {
            get
            {
                return _DocumentDate;
            }
            set
            {
                SetPropertyValue("DocumentDate", ref _DocumentDate, value);
            }
        }

        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        [Association, Aggregated]
        public XPCollection<ProductionConsumeDetail> ProductionConsumeDetails
        {
            get { return GetCollection<ProductionConsumeDetail>("ProductionConsumeDetails"); }
        }
    }
}
