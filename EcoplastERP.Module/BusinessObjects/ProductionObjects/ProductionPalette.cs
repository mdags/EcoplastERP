using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("PaletteNumber")]
    [NavigationItem("ProductionManagement")]
    public class ProductionPalette : BaseObject
    {
        public ProductionPalette(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            PaletteNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                if (!PaletteNumber.StartsWith("P")) throw new UserFriendlyException("Palet numarasý P ile baþlamalýdýr.");

                var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));

                var productionPalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", PaletteNumber));
                if (productionPalette != null)
                {
                    if (filmingWorkOrder != null)
                    {
                        var eFilmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (filmingWorkOrder.SalesOrderDetail.Product != eFilmingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (castFilmingWorkOrder != null)
                    {
                        var eCastFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castFilmingWorkOrder.SalesOrderDetail.Product != eCastFilmingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (castTransferingWorkOrder != null)
                    {
                        var eCastTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castTransferingWorkOrder.SalesOrderDetail.Product != eCastTransferingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (balloonFilmingWorkOrder != null)
                    {
                        var eBalloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (balloonFilmingWorkOrder.SalesOrderDetail.Product != eBalloonFilmingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (printingWorkOrder != null)
                    {
                        var ePrintingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (printingWorkOrder.SalesOrderDetail.Product != ePrintingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (laminationWorkOrder != null)
                    {
                        var eLaminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (laminationWorkOrder.SalesOrderDetail.Product != eLaminationWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (slicingWorkOrder != null)
                    {
                        var eSlicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (slicingWorkOrder.SalesOrderDetail.Product != eSlicingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (castSlicingWorkOrder != null)
                    {
                        var eCastSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castSlicingWorkOrder.SalesOrderDetail.Product != eCastSlicingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (cuttingWorkOrder != null)
                    {
                        var eCuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (cuttingWorkOrder.SalesOrderDetail.Product != eCuttingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (foldingWorkOrder != null)
                    {
                        var eFoldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (foldingWorkOrder.SalesOrderDetail.Product != eFoldingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (balloonCuttingWorkOrder != null)
                    {
                        var eBalloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (balloonCuttingWorkOrder.SalesOrderDetail.Product != eBalloonCuttingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (regeneratedWorkOrder != null)
                    {
                        var eRegeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (regeneratedWorkOrder.SalesOrderDetail.Product != eRegeneratedWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (castRegeneratedWorkOrder != null)
                    {
                        var eCastRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (castRegeneratedWorkOrder.SalesOrderDetail.Product != eCastRegeneratedWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (eco6WorkOrder != null)
                    {
                        var eEco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (eco6WorkOrder.SalesOrderDetail.Product != eEco6WorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (eco6CuttingWorkOrder != null)
                    {
                        var eEco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (eco6CuttingWorkOrder.SalesOrderDetail.Product != eEco6CuttingWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                    if (eco6LaminationWorkOrder != null)
                    {
                        var eEco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (eco6LaminationWorkOrder.SalesOrderDetail.Product != eEco6LaminationWorkOrder.SalesOrderDetail.Product)
                        {
                            throw new UserFriendlyException("Girilen üretim sipariþi ile daha önce palete tanýmlý üretim sipariþi kalemleri uyuþmuyor.");
                        }
                    }
                }

                Active = true;
                var activePalette = Session.FindObject<ProductionPalette>(CriteriaOperator.Parse("WorkOrderNumber = ? And Active = true", WorkOrderNumber));
                if (activePalette != null)
                {
                    Session.ExecuteNonQuery(@"update ProductionPalette set Active = 0 where Oid = @1", new string[] { "@1" }, new object[] { activePalette.Oid });
                }

                if (filmingWorkOrder != null) filmingWorkOrder.PaletteNumber = PaletteNumber;
                if (castFilmingWorkOrder != null) castFilmingWorkOrder.PaletteNumber = PaletteNumber;
                if (castTransferingWorkOrder != null) castTransferingWorkOrder.PaletteNumber = PaletteNumber;
                if (balloonFilmingWorkOrder != null) balloonFilmingWorkOrder.PaletteNumber = PaletteNumber;
                if (printingWorkOrder != null) printingWorkOrder.PaletteNumber = PaletteNumber;
                if (laminationWorkOrder != null) laminationWorkOrder.PaletteNumber = PaletteNumber;
                if (slicingWorkOrder != null) slicingWorkOrder.PaletteNumber = PaletteNumber;
                if (castSlicingWorkOrder != null) castSlicingWorkOrder.PaletteNumber = PaletteNumber;
                if (cuttingWorkOrder != null) cuttingWorkOrder.PaletteNumber = PaletteNumber;
                if (foldingWorkOrder != null) foldingWorkOrder.PaletteNumber = PaletteNumber;
                if (balloonCuttingWorkOrder != null) balloonCuttingWorkOrder.PaletteNumber = PaletteNumber;
                if (regeneratedWorkOrder != null) regeneratedWorkOrder.PaletteNumber = PaletteNumber;
                if (castRegeneratedWorkOrder != null) castRegeneratedWorkOrder.PaletteNumber = PaletteNumber;
                if (eco6WorkOrder != null) eco6WorkOrder.PaletteNumber = PaletteNumber;
                if (eco6CuttingWorkOrder != null) eco6CuttingWorkOrder.PaletteNumber = PaletteNumber;
                if (eco6LaminationWorkOrder != null) eco6LaminationWorkOrder.PaletteNumber = PaletteNumber;
            }
            else
            {
                if (!Session.IsObjectMarkedDeleted(this) & LastWeight != 0)
                {
                    GrossWeight = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", PaletteNumber)));
                    NetWeight = Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(NetQuantity)"), CriteriaOperator.Parse("ProductionPalette.PaletteNumber = ?", PaletteNumber)));

                    if (LastWeight < ((GrossWeight + Tare) - Palette.Option) || LastWeight > ((GrossWeight + Tare) + Palette.Option))
                    {
                        throw new UserFriendlyException("Son aðýrlýk rulolarýn toplamý ve palet darasýndan daha küçük olamaz. Rulolarý kontrol edip tekrar tartým yapýnýz.");
                    }
                    else
                    {
                        ConsumeMaterialWeight = LastWeight - (GrossWeight + Tare);
                    }
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            Production production = Session.FindObject<Production>(new BinaryOperator("ProductionPalette", this));
            if (production != null)
            {
                throw new UserFriendlyException("Ýçerisinde ürün olan palet silinemez. Öncelikle tartýlan ürünleri silmeniz gerekiyor.");
            }

            var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (filmingWorkOrder != null)
            {
                if (filmingWorkOrder.PaletteNumber == PaletteNumber) filmingWorkOrder.PaletteNumber = string.Empty;
            }
            var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castFilmingWorkOrder != null)
            {
                if (castFilmingWorkOrder.PaletteNumber == PaletteNumber) castFilmingWorkOrder.PaletteNumber = string.Empty;
            }
            var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castTransferingWorkOrder != null)
            {
                if (castTransferingWorkOrder.PaletteNumber == PaletteNumber) castTransferingWorkOrder.PaletteNumber = string.Empty;
            }
            var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (balloonFilmingWorkOrder != null)
            {
                if (balloonFilmingWorkOrder.PaletteNumber == PaletteNumber) balloonFilmingWorkOrder.PaletteNumber = string.Empty;
            }
            var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (printingWorkOrder != null)
            {
                if (printingWorkOrder.PaletteNumber == PaletteNumber) printingWorkOrder.PaletteNumber = string.Empty;
            }
            var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (laminationWorkOrder != null)
            {
                if (laminationWorkOrder.PaletteNumber == PaletteNumber) laminationWorkOrder.PaletteNumber = string.Empty;
            }
            var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (slicingWorkOrder != null)
            {
                if (slicingWorkOrder.PaletteNumber == PaletteNumber) slicingWorkOrder.PaletteNumber = string.Empty;
            }
            var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castSlicingWorkOrder != null)
            {
                if (castSlicingWorkOrder.PaletteNumber == PaletteNumber) castSlicingWorkOrder.PaletteNumber = string.Empty;
            }
            var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (cuttingWorkOrder != null)
            {
                if (cuttingWorkOrder.PaletteNumber == PaletteNumber) cuttingWorkOrder.PaletteNumber = string.Empty;
            }
            var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (foldingWorkOrder != null)
            {
                if (foldingWorkOrder.PaletteNumber == PaletteNumber) foldingWorkOrder.PaletteNumber = string.Empty;
            }
            var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (balloonCuttingWorkOrder != null)
            {
                if (balloonCuttingWorkOrder.PaletteNumber == PaletteNumber) balloonCuttingWorkOrder.PaletteNumber = string.Empty;
            }
            var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (regeneratedWorkOrder != null)
            {
                if (regeneratedWorkOrder.PaletteNumber == PaletteNumber) regeneratedWorkOrder.PaletteNumber = string.Empty;
            }
            var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castRegeneratedWorkOrder != null)
            {
                if (castRegeneratedWorkOrder.PaletteNumber == PaletteNumber) castRegeneratedWorkOrder.PaletteNumber = string.Empty;
            }
            var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (eco6WorkOrder != null)
            {
                if (eco6WorkOrder.PaletteNumber == PaletteNumber) eco6WorkOrder.PaletteNumber = string.Empty;
            }
            var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (eco6CuttingWorkOrder != null)
            {
                if (eco6CuttingWorkOrder.PaletteNumber == PaletteNumber) eco6CuttingWorkOrder.PaletteNumber = string.Empty;
            }
            var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (eco6LaminationWorkOrder != null)
            {
                if (eco6LaminationWorkOrder.PaletteNumber == PaletteNumber) eco6LaminationWorkOrder.PaletteNumber = string.Empty;
            }
        }
        // Fields...
        private decimal _ConsumeMaterialWeight;
        private decimal _LastWeight;
        private decimal _NetWeight;
        private decimal _GrossWeight;
        private decimal _Tare;
        private string _WorkOrderNumber;
        private string _PaletteNumber;
        private Palette _Palette;
        private bool _Active;

        [VisibleInDetailView(false)]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                SetPropertyValue("Active", ref _Active, value);
            }
        }

        [RuleRequiredField]
        public Palette Palette
        {
            get
            {
                return _Palette;
            }
            set
            {
                SetPropertyValue("Palette", ref _Palette, value);
                GetPalette();
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
                GetTare();
            }
        }

        [NonCloneable]
        [RuleRequiredField]
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

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Tare
        {
            get
            {
                return _Tare;
            }
            set
            {
                SetPropertyValue("Tare", ref _Tare, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal GrossWeight
        {
            get
            {
                return _GrossWeight;
            }
            set
            {
                SetPropertyValue("GrossWeight", ref _GrossWeight, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal NetWeight
        {
            get
            {
                return _NetWeight;
            }
            set
            {
                SetPropertyValue("NetWeight", ref _NetWeight, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal LastWeight
        {
            get
            {
                return _LastWeight;
            }
            set
            {
                SetPropertyValue("LastWeight", ref _LastWeight, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal ConsumeMaterialWeight
        {
            get
            {
                return _ConsumeMaterialWeight;
            }
            set
            {
                SetPropertyValue("ConsumeMaterialWeight", ref _ConsumeMaterialWeight, value);
            }
        }

        public int RollCount
        {
            get
            {
                return Productions.Count;
            }
        }

        public int TotalLength
        {
            get
            {
                int total = 0;
                foreach (Production item in Productions)
                {
                    total += item.Length;
                }
                return total;
            }
        }

        [Association("ProductionPalette-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }
        }

        private XPCollection<AuditDataItemPersistent> changeHistory;
        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                {
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return changeHistory;
            }
        }

        [Action(PredefinedCategory.View, Caption = "Activate", AutoCommit = true, ImageName = "Action_Grant", ConfirmationMessage = "This operation will be activated the selected palette. Do you want to continue?", TargetObjectsCriteria = "Active = false", ToolTip = "This operation will be activated the selected palette.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        public void Activate()
        {
            var loading = Session.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("PaletteNumber = ?", PaletteNumber));
            if (loading != null) throw new UserFriendlyException("Yükleme yapýlmýþ paleti tekrar aktife alamazsýnýz.");

            var activePalette = Session.FindObject<ProductionPalette>(CriteriaOperator.Parse("WorkOrderNumber = ? And Active = true", WorkOrderNumber));
            if (activePalette != null) activePalette.Active = false;
            Active = true;
            GrossWeight = 0;
            NetWeight = 0;
            LastWeight = 0;
            ConsumeMaterialWeight = 0;

            var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (filmingWorkOrder != null) filmingWorkOrder.PaletteNumber = PaletteNumber;

            var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castFilmingWorkOrder != null) castFilmingWorkOrder.PaletteNumber = PaletteNumber;

            var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castTransferingWorkOrder != null) castTransferingWorkOrder.PaletteNumber = PaletteNumber;

            var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (balloonFilmingWorkOrder != null) balloonFilmingWorkOrder.PaletteNumber = PaletteNumber;

            var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (printingWorkOrder != null) printingWorkOrder.PaletteNumber = PaletteNumber;

            var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (laminationWorkOrder != null) laminationWorkOrder.PaletteNumber = PaletteNumber;

            var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (slicingWorkOrder != null) slicingWorkOrder.PaletteNumber = PaletteNumber;

            var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castSlicingWorkOrder != null) castSlicingWorkOrder.PaletteNumber = PaletteNumber;

            var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (cuttingWorkOrder != null) cuttingWorkOrder.PaletteNumber = PaletteNumber;

            var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (foldingWorkOrder != null) foldingWorkOrder.PaletteNumber = PaletteNumber;

            var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (balloonCuttingWorkOrder != null) balloonCuttingWorkOrder.PaletteNumber = PaletteNumber;

            var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (regeneratedWorkOrder != null) regeneratedWorkOrder.PaletteNumber = PaletteNumber;

            var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (castRegeneratedWorkOrder != null) castRegeneratedWorkOrder.PaletteNumber = PaletteNumber;

            var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (eco6WorkOrder != null) eco6WorkOrder.PaletteNumber = PaletteNumber;

            var eco6CUttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (eco6CUttingWorkOrder != null) eco6CUttingWorkOrder.PaletteNumber = PaletteNumber;

            var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            if (eco6LaminationWorkOrder != null) eco6LaminationWorkOrder.PaletteNumber = PaletteNumber;
        }

        #region functions
        void GetPalette()
        {
            if (IsLoading) return;
            if (Palette != null)
            {
                var palette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", PaletteNumber));
                if (palette == null) Tare = Palette.Weight;
            }
        }
        void GetTare()
        {
            if (IsLoading) return;
            var palette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", PaletteNumber));
            if (palette != null)
            {
                Tare = palette.Tare;
            }
        }
        #endregion
    }
}
