using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Task")]
    [DefaultProperty("WorkOrderNumber")]
    [NavigationItem("ProductionManagement")]
    [Appearance("CuttingWorkOrder.LineDeliveryDateAppearance", TargetItems = "WorkName", Criteria = "SalesOrderDetail.LineDeliveryDate < LocalDateTimeToday()", BackColor = "MistyRose")]
    [Appearance("CuttingWorkOrder.TotalProductionQuantityAppearance", TargetItems = "TotalProduction", Criteria = "TotalProduction > Quantity", BackColor = "GreenYellow")]
    [Appearance("CuttingWorkOrder.TotalProductionQuantityAppearance1", TargetItems = "TotalProduction", Criteria = "TotalProduction <= Quantity", BackColor = "LemonChiffon")]
    [Appearance("CuttingWorkOrder.CompletedAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'ProductionComplete'", FontStyle = FontStyle.Strikeout, BackColor = "LemonChiffon")]
    [Appearance("CuttingWorkOrder.CanceledAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'Canceled'", FontStyle = FontStyle.Strikeout, BackColor = "MistyRose")]
    [Appearance("CuttingWorkOrder.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "CuttingWorkOrder_ListView")]
    [Appearance("CuttingWorkOrder_MachineLoad.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "CuttingWorkOrder_ListView_MachineLoad")]
    public class CuttingWorkOrder : BaseObject
    {
        public CuttingWorkOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
            SequenceNumber = 99;
            WorkOrderDate = Helpers.GetSystemDate(Session);
            WorkOrderNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            Station = Session.FindObject<Station>(CriteriaOperator.Parse("Name = 'Kesim'"));
            NextStation = Session.FindObject<Station>(CriteriaOperator.Parse("Name = 'Son Üretim'"));
            PaletteStickerDesign = Session.FindObject<ReportDataV2>(CriteriaOperator.Parse("Contains([DisplayName], ?)", "Standart Palet Dökümü"));
        }
        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.IsNewObject(this))
            {
                if (NextStation != null) SalesOrderDetail.SalesOrderStatus = NextStation.SalesOrderStatus;
            }
            if (SalesOrderDetail != null) SalesOrderDetail.WaitingWorkOrder = false;
            if (Machine.CuttingMachineGroup != null)
            {
                if (SalesOrderDetail.CuttingMachineGroup != Machine.CuttingMachineGroup)
                {
                    SalesOrderDetail.CuttingMachineGroup = Machine.CuttingMachineGroup;
                }
            }

            if (WorkOrderStatus == WorkOrderStatus.ProductionComplete || WorkOrderStatus == WorkOrderStatus.Canceled) SequenceNumber = 9000;

            if (Session.Connection != null && Session.IsObjectToSave(this))
            {
                if (!NextStation.IsLastStation)
                {
                    if (Quantity > SalesOrderDetail.cQuantity + (SalesOrderDetail.cQuantity * SalesOrderDetail.SemiProductOption / 100))
                    {
                        throw new UserFriendlyException("Üretim miktarý sipariþteki yarý mamül üretim opsiyonundan fazla olamaz.");
                    }
                }
                else
                {
                    if (Quantity > SalesOrderDetail.cQuantity + (SalesOrderDetail.cQuantity * SalesOrderDetail.LastProductOption / 100))
                    {
                        throw new UserFriendlyException("Üretim miktarý sipariþteki mamül üretim opsiyonundan fazla olamaz.");
                    }
                }
            }

            //CuttingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbReciept = new StringBuilder();
            //foreach (var reciept in CuttingWorkOrderReciepts)
            //{
            //    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));

            //    reciept.Quantity = Quantity * reciept.Rate / 100;
            //}
            //RecieptString = sbReciept.ToString();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforCutting;
        }
        // Fields...
        private DateTime _BeginDate;
        private DateTime _EndDate;
        private string _RecieptString;
        private Reciept _Reciept;
        private string _PaletteNumber;
        private int _InnerPackingPieceCount;
        private int _OuterPackingPackageCount;
        private string _SupportBandNote;
        private decimal _SupportBandSize;
        private decimal _SupportBandThickness;
        private string _SupportBandColor;
        private int _DeckCount;
        private int _PaletteBobbinCount;
        private string _PaletteLayout;
        private Palette _Palette;
        private Bobbin _Bobbin;
        private InnerPacking _InnerPacking;
        private OuterPacking _OuterPacking;
        private bool _Embossing;
        private decimal _GramMetretul;
        private decimal _GramM2;
        private ShippingPackageType _ShippingPackageType;
        private int _WayCount;
        private ShippingFilmType _ShippingFilmType;
        private int _PackageCount;
        private decimal _MaximumPieceWeight;
        private decimal _MinimumPieceWeight;
        private decimal _PieceWeight;
        private decimal _PackageWeight;
        private decimal _Density;
        private int _BladeDeep;
        private int _BladeWidth;
        private PrintName _PrintName;
        private int _Block;
        private HandleWeld _HandleWeld;
        private HangingLocation _HangingLocation;
        private BandStatus _BandStatus;
        private PerforationStatus _PerforationStatus;
        private decimal _Cap;
        private CapStatus _CapStatus;
        private string _Bellows;
        private BellowsStatus _BellowsStatus;
        private decimal _Thickness;
        private int _Height;
        private int _Width;
        private string _QualityNote;
        private string _ProductionNote;
        private decimal _ProductionOption;
        private ReportDataV2 _PaletteStickerDesign;
        private decimal _Quantity;
        private Unit _Unit;
        private Station _NextStation;
        private Machine _Machine;
        private Station _Station;
        private string _WorkName;
        private SalesOrderDetail _SalesOrderDetail;
        private DateTime _WorkOrderDate;
        private string _WorkOrderNumber;
        private Int16 _SequenceNumber;
        private WorkOrderStatus _WorkOrderStatus;

        [NonCloneable]
        [VisibleInDetailView(false)]
        public WorkOrderStatus WorkOrderStatus
        {
            get
            {
                return _WorkOrderStatus;
            }
            set
            {
                SetPropertyValue("WorkOrderStatus", ref _WorkOrderStatus, value);
            }
        }

        [NonCloneable]
        public Int16 SequenceNumber
        {
            get
            {
                return _SequenceNumber;
            }
            set
            {
                SetPropertyValue("SequenceNumber", ref _SequenceNumber, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
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

        [RuleRequiredField]
        public DateTime WorkOrderDate
        {
            get
            {
                return _WorkOrderDate;
            }
            set
            {
                SetPropertyValue("WorkOrderDate", ref _WorkOrderDate, value);
            }
        }

        [VisibleInLookupListView(true)]
        [ImmediatePostData]
        [RuleRequiredField]
        public SalesOrderDetail SalesOrderDetail
        {
            get
            {
                return _SalesOrderDetail;
            }
            set
            {
                SetPropertyValue("SalesOrderDetail", ref _SalesOrderDetail, value);
                GetSalesOrderDetail();
            }
        }

        [VisibleInLookupListView(true)]
        public string Contact
        {
            get { return SalesOrderDetail != null && SalesOrderDetail.SalesOrder != null ? SalesOrderDetail.SalesOrder.Contact.Name : null; }
        }

        [Appearance("CuttingWorkOrder.WorkName", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string WorkName
        {
            get
            {
                return _WorkName;
            }
            set
            {
                SetPropertyValue("WorkName", ref _WorkName, value);
            }
        }

        [VisibleInDetailView(false)]
        public Station Station
        {
            get
            {
                return _Station;
            }
            set
            {
                SetPropertyValue("Station", ref _Station, value);
            }
        }

        [RuleRequiredField]
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

        [ImmediatePostData]
        [RuleRequiredField]
        public Station NextStation
        {
            get
            {
                return _NextStation;
            }
            set
            {
                SetPropertyValue("NextStation", ref _NextStation, value);
                SetMaximumRollWeight();
            }
        }

        [RuleRequiredField]
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

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal TotalProduction
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("CuttingWorkOrder = ?", Oid)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal RemainingProduction
        {
            get
            {
                return Quantity - TotalProduction;
            }
        }

        [VisibleInDetailView(false)]
        public decimal QuarantineQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Sum(cQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse = ?", SalesOrderDetail, Station.QuarantineWarehouse)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal TotalWastage
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Wastage), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("CuttingWorkOrder = ?", Oid)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal ProductStore
        {
            get
            {
                return Station != null ? SalesOrderDetail != null ? Convert.ToDecimal(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Sum(cQuantity)"), CriteriaOperator.Parse("Product = ? and Warehouse = ?", SalesOrderDetail.Product, Station.SourceWarehouse))) : 0 : 0;
            }
        }

        [VisibleInDetailView(false)]
        public decimal SalesOrderProduction
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and IsLastProduction = true", SalesOrderDetail)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal RemainingTime
        {
            get
            {
                return Machine.Capacity != 0 ? (RemainingProduction / (Machine.Capacity / 24)) : 0;
            }
        }

        [Appearance("CuttingWorkOrder.PaletteStickerDesign", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "NextStation.Name = 'Son Üretim'")]
        public ReportDataV2 PaletteStickerDesign
        {
            get
            {
                return _PaletteStickerDesign;
            }
            set
            {
                SetPropertyValue("PaletteStickerDesign", ref _PaletteStickerDesign, value);
            }
        }

        public decimal ProductionOption
        {
            get
            {
                return _ProductionOption;
            }
            set
            {
                SetPropertyValue("ProductionOption", ref _ProductionOption, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string ProductionNote
        {
            get
            {
                return _ProductionNote;
            }
            set
            {
                SetPropertyValue("ProductionNote", ref _ProductionNote, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string QualityNote
        {
            get
            {
                return _QualityNote;
            }
            set
            {
                SetPropertyValue("QualityNote", ref _QualityNote, value);
            }
        }

        [ImmediatePostData]
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                SetPropertyValue("Width", ref _Width, value);
                CalculateGramMetretul();
            }
        }

        [ImmediatePostData]
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                SetPropertyValue("Height", ref _Height, value);
                CalculateGramMetretul();
            }
        }

        [ImmediatePostData]
        public decimal Thickness
        {
            get
            {
                return _Thickness;
            }
            set
            {
                SetPropertyValue("Thickness", ref _Thickness, value);
                CalculateGramMetretul();
            }
        }

        [ImmediatePostData]
        public BellowsStatus BellowsStatus
        {
            get
            {
                return _BellowsStatus;
            }
            set
            {
                SetPropertyValue("BellowsStatus", ref _BellowsStatus, value);
            }
        }

        [Appearance("CuttingWorkOrder.BellowsBellowsStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "BellowsStatus = 'None'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "BellowsStatus != 'None'")]
        public string Bellows
        {
            get
            {
                return _Bellows;
            }
            set
            {
                SetPropertyValue("Bellows", ref _Bellows, value);
            }
        }

        [ImmediatePostData]
        public CapStatus CapStatus
        {
            get
            {
                return _CapStatus;
            }
            set
            {
                SetPropertyValue("CapStatus", ref _CapStatus, value);
            }
        }

        [Appearance("CuttingWorkOrder.Cap", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "CapStatus = 'Uncapped'")]
        public decimal Cap
        {
            get
            {
                return _Cap;
            }
            set
            {
                SetPropertyValue("Cap", ref _Cap, value);
            }
        }

        public PerforationStatus PerforationStatus
        {
            get
            {
                return _PerforationStatus;
            }
            set
            {
                SetPropertyValue("PerforationStatus", ref _PerforationStatus, value);
            }
        }

        public BandStatus BandStatus
        {
            get
            {
                return _BandStatus;
            }
            set
            {
                SetPropertyValue("BandStatus", ref _BandStatus, value);
            }
        }

        public HangingLocation HangingLocation
        {
            get
            {
                return _HangingLocation;
            }
            set
            {
                SetPropertyValue("HangingLocation", ref _HangingLocation, value);
            }
        }

        public HandleWeld HandleWeld
        {
            get
            {
                return _HandleWeld;
            }
            set
            {
                SetPropertyValue("HandleWeld", ref _HandleWeld, value);
            }
        }

        public int Block
        {
            get
            {
                return _Block;
            }
            set
            {
                SetPropertyValue("Block", ref _Block, value);
            }
        }

        public PrintName PrintName
        {
            get
            {
                return _PrintName;
            }
            set
            {
                SetPropertyValue("PrintName", ref _PrintName, value);
            }
        }

        public int BladeWidth
        {
            get
            {
                return _BladeWidth;
            }
            set
            {
                SetPropertyValue("BladeWidth", ref _BladeWidth, value);
            }
        }

        public int BladeDeep
        {
            get
            {
                return _BladeDeep;
            }
            set
            {
                SetPropertyValue("BladeDeep", ref _BladeDeep, value);
            }
        }

        [ImmediatePostData]
        public decimal Density
        {
            get
            {
                return _Density;
            }
            set
            {
                SetPropertyValue("Density", ref _Density, value);
                CalculateGramMetretul();
            }
        }

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "NextStation.Name = 'Son Üretim'")]
        public decimal PackageWeight
        {
            get
            {
                return _PackageWeight;
            }
            set
            {
                SetPropertyValue("PackageWeight", ref _PackageWeight, value);
            }
        }

        public decimal PieceWeight
        {
            get
            {
                return _PieceWeight;
            }
            set
            {
                SetPropertyValue("PieceWeight", ref _PieceWeight, value);
            }
        }

        public decimal MinimumPieceWeight
        {
            get
            {
                return _MinimumPieceWeight;
            }
            set
            {
                SetPropertyValue("MinimumPieceWeight", ref _MinimumPieceWeight, value);
            }
        }

        public decimal MaximumPieceWeight
        {
            get
            {
                return _MaximumPieceWeight;
            }
            set
            {
                SetPropertyValue("MaximumPieceWeight", ref _MaximumPieceWeight, value);
            }
        }

        public int PackageCount
        {
            get
            {
                return _PackageCount;
            }
            set
            {
                SetPropertyValue("PackageCount", ref _PackageCount, value);
            }
        }

        public ShippingPackageType ShippingPackageType
        {
            get
            {
                return _ShippingPackageType;
            }
            set
            {
                SetPropertyValue("ShippingPackageType", ref _ShippingPackageType, value);
            }
        }

        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "NextStation.Name = 'Son Üretim'")]
        public ShippingFilmType ShippingFilmType
        {
            get
            {
                return _ShippingFilmType;
            }
            set
            {
                SetPropertyValue("ShippingFilmType", ref _ShippingFilmType, value);
            }
        }

        public int WayCount
        {
            get
            {
                return _WayCount;
            }
            set
            {
                SetPropertyValue("WayCount", ref _WayCount, value);
            }
        }

        public decimal GramM2
        {
            get
            {
                return _GramM2;
            }
            set
            {
                SetPropertyValue("GramM2", ref _GramM2, value);
            }
        }

        public decimal GramMetretul
        {
            get
            {
                return _GramMetretul;
            }
            set
            {
                SetPropertyValue("GramMetretul", ref _GramMetretul, value);
            }
        }

        public bool Embossing
        {
            get
            {
                return _Embossing;
            }
            set
            {
                SetPropertyValue("Embossing", ref _Embossing, value);
            }
        }

        [RuleRequiredField]
        public OuterPacking OuterPacking
        {
            get
            {
                return _OuterPacking;
            }
            set
            {
                SetPropertyValue("OuterPacking", ref _OuterPacking, value);
            }
        }

        public InnerPacking InnerPacking
        {
            get
            {
                return _InnerPacking;
            }
            set
            {
                SetPropertyValue("InnerPacking", ref _InnerPacking, value);
            }
        }

        public Bobbin Bobbin
        {
            get
            {
                return _Bobbin;
            }
            set
            {
                SetPropertyValue("Bobbin", ref _Bobbin, value);
            }
        }

        public Palette Palette
        {
            get
            {
                return _Palette;
            }
            set
            {
                SetPropertyValue("Palette", ref _Palette, value);
            }
        }

        [VisibleInDetailView(false)]
        public string PaletteLayout
        {
            get
            {
                return _PaletteLayout;
            }
            set
            {
                SetPropertyValue("PaletteLayout", ref _PaletteLayout, value);
            }
        }

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "NextStation.Name = 'Son Üretim'")]
        public int PaletteBobbinCount
        {
            get
            {
                return _PaletteBobbinCount;
            }
            set
            {
                SetPropertyValue("PaletteBobbinCount", ref _PaletteBobbinCount, value);
            }
        }

        [Appearance("CuttingWorkOrder.DeckCount", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "InnerPacking != null")]
        public int DeckCount
        {
            get
            {
                return _DeckCount;
            }
            set
            {
                SetPropertyValue("DeckCount", ref _DeckCount, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SupportBandColor
        {
            get
            {
                return _SupportBandColor;
            }
            set
            {
                SetPropertyValue("SupportBandColor", ref _SupportBandColor, value);
            }
        }

        public decimal SupportBandThickness
        {
            get
            {
                return _SupportBandThickness;
            }
            set
            {
                SetPropertyValue("SupportBandThickness", ref _SupportBandThickness, value);
            }
        }

        public decimal SupportBandSize
        {
            get
            {
                return _SupportBandSize;
            }
            set
            {
                SetPropertyValue("SupportBandSize", ref _SupportBandSize, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SupportBandNote
        {
            get
            {
                return _SupportBandNote;
            }
            set
            {
                SetPropertyValue("SupportBandNote", ref _SupportBandNote, value);
            }
        }

        [Appearance("CuttingWorkOrder.OuterPackingPackageCount", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "OuterPacking = null")]
        public int OuterPackingPackageCount
        {
            get
            {
                return _OuterPackingPackageCount;
            }
            set
            {
                SetPropertyValue("OuterPackingPackageCount", ref _OuterPackingPackageCount, value);
            }
        }

        [Appearance("CuttingWorkOrder.InnerPackingPieceCount", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "InnerPacking = null")]
        public int InnerPackingPieceCount
        {
            get
            {
                return _InnerPackingPieceCount;
            }
            set
            {
                SetPropertyValue("InnerPackingPieceCount", ref _InnerPackingPieceCount, value);
            }
        }

        [Appearance("CuttingWorkOrder.PaletteNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [VisibleInDetailView(false)]
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

        [VisibleInDetailView(false)]
        public Reciept Reciept
        {
            get
            {
                return _Reciept;
            }
            set
            {
                SetPropertyValue("Reciept", ref _Reciept, value);
                GetReciept();
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string RecieptString
        {
            get
            {
                return _RecieptString;
            }
            set
            {
                SetPropertyValue("RecieptString", ref _RecieptString, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                SetPropertyValue("BeginDate", ref _BeginDate, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetPropertyValue("EndDate", ref _EndDate, value);
            }
        }

        [VisibleInDetailView(false)]
        public int WorkTime
        {
            get
            {
                return EndDate != null ? Convert.ToInt32((EndDate - BeginDate).TotalMinutes) : 0;
            }
        }

        [Association, Aggregated]
        public XPCollection<CuttingWorkOrderReciept> CuttingWorkOrderReciepts
        {
            get { return GetCollection<CuttingWorkOrderReciept>("CuttingWorkOrderReciepts"); }
        }

        [Association("CuttingWorkOrder-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }

        }

        [Association("CuttingWorkOrder-Wastages")]
        public XPCollection<Wastage> Wastages
        {
            get
            {
                return GetCollection<Wastage>("Wastages");
            }
        }

        [Association("CuttingWorkOrder-ReadResources")]
        public XPCollection<ReadResource> ReadResources
        {
            get
            {
                return GetCollection<ReadResource>("ReadResources");
            }
        }

        #region functions
        void SetMaximumRollWeight()
        {
            if (IsLoading) return;
            if (NextStation != null)
            {
                if (!NextStation.IsLastStation) MaximumPieceWeight = 1000;
            }
        }
        void GetSalesOrderDetail()
        {
            if (IsLoading) return;
            if (SalesOrderDetail != null)
            {
                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail.Oid));
                if (cuttingWorkOrder != null) XtraMessageBox.Show(string.Format("Bu müþteri sipariþi için daha önce {0} numaralý üretim sipariþi doldurulmuþ. Mükerrer bir kayýt olup olmadýðýný kontrol ediniz...", cuttingWorkOrder.WorkOrderNumber), "Bilgilendirme", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                if (SalesOrderDetail.Unit.Code == "KG")
                {
                    Unit = SalesOrderDetail.Unit;
                    Quantity = SalesOrderDetail.Quantity;
                }
                else if (SalesOrderDetail.cUnit.Code == "KG")
                {
                    Unit = SalesOrderDetail.cUnit;
                    Quantity = SalesOrderDetail.cQuantity;
                }
                WorkName = SalesOrderDetail.Product.Name;
                Width = SalesOrderDetail.Product.Width;
                Height = SalesOrderDetail.Product.Height;
                Thickness = SalesOrderDetail.Product.Thickness;
                BellowsStatus = SalesOrderDetail.Product.BellowsStatus;
                Bellows = SalesOrderDetail.Product.Bellows;
                CapStatus = SalesOrderDetail.Product.CapStatus;
                Cap = SalesOrderDetail.Product.Cap;
                PerforationStatus = SalesOrderDetail.Product.PerforationStatus;
                BandStatus = SalesOrderDetail.Product.BandStatus;
                HangingLocation = SalesOrderDetail.Product.HangingLocation;
                HandleWeld = SalesOrderDetail.Product.HandleWeld;
                Block = SalesOrderDetail.Product.Block;
                PrintName = SalesOrderDetail.Product.PrintName ?? null;
                BladeWidth = SalesOrderDetail.Product.BladeWidth;
                BladeDeep = SalesOrderDetail.Product.BladeDeep;
                Density = SalesOrderDetail.Product.Density;
                ShippingPackageType = SalesOrderDetail.Product.ShippingPackageType ?? null;
                ShippingFilmType = SalesOrderDetail.Product.ShippingFilmType ?? null;
                Embossing = SalesOrderDetail.Product.Embossing;
                ProductionNote = SalesOrderDetail.WorkOrderNote;
                QualityNote = SalesOrderDetail.QualityNote;
                Palette = SalesOrderDetail.Palette ?? null;

                Session.Delete(CuttingWorkOrderReciepts);
                CuttingWorkOrderReciept reciept = new CuttingWorkOrderReciept(Session)
                {
                    CuttingWorkOrder = this,
                    Product = Session.FindObject<Product>(new BinaryOperator("Oid", SalesOrderDetail.Product.Oid)),
                    Warehouse = Session.FindObject<Warehouse>(new BinaryOperator("Oid", Station.SourceWarehouse.Oid)),
                    Rate = 99,
                    Unit = Session.FindObject<Unit>(new BinaryOperator("Oid", SalesOrderDetail.Product.Unit.Oid))
                };
                CuttingWorkOrderReciepts.Add(reciept);
            }
        }
        void GetReciept()
        {
            if (IsLoading) return;
            if (Reciept != null)
            {
                Session.Delete(CuttingWorkOrderReciepts);
                foreach (var item in Reciept.RecieptDetails)
                {
                    var cuttingWorkOrderReciept = new CuttingWorkOrderReciept(Session)
                    {
                        CuttingWorkOrder = this,
                        Product = item.Product,
                        Warehouse = item.Warehouse,
                        Rate = item.Rate,
                        Unit = item.Unit
                    };
                    this.CuttingWorkOrderReciepts.Add(cuttingWorkOrderReciept);
                }

                CuttingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in CuttingWorkOrderReciepts)
                {
                    sbReciept.AppendLine(String.Format("{0} - %{1:n} / ", reciept.Product.Name, reciept.Rate));
                }
                RecieptString = sbReciept.ToString();
            }
        }
        void CalculateGramMetretul()
        {
            if (IsLoading) return;
            if (SalesOrderDetail != null)
            {
                if (SalesOrderDetail.Product != null)
                {
                    if (SalesOrderDetail.Product.ProductKind != null)
                    {
                        if (ShippingFilmType != null)
                        {
                            int width = 0;
                            if (BellowsStatus == BellowsStatus.None) width = Width;
                            else
                            {
                                decimal bellow = Convert.ToDecimal(Bellows);
                                width = ShippingFilmType.Name.Contains("HORTUM") ? Width + Convert.ToInt32(bellow * 4) : Width + Convert.ToInt32(bellow * 2);
                            }
                            decimal hangingPlaceValue = SalesOrderDetail.Product.HangingLocation == HangingLocation.Does ? 0.01M : 0;
                            int thicknessPart = ShippingFilmType.ThicknessDoublePart ? 2 : 1;

                            if (SalesOrderDetail.Product.ProductKind.Name.Contains("TORBA"))
                            {
                                if (ShippingFilmType.Name == "YAPRAK")
                                {
                                    int height = 1000;
                                    decimal en = height + height + width + width + Cap + Cap;
                                    GramMetretul = ((en * height * Density * Thickness) / 100000) + hangingPlaceValue;
                                }
                                else if (ShippingFilmType.Name.Contains("HORTUM"))
                                {
                                    int height = 1000;
                                    GramMetretul = (((width * height * Density * Thickness) * thicknessPart) / 100000) + hangingPlaceValue;
                                }
                                else
                                {
                                    GramMetretul = (((width * Height * Density * Thickness) * thicknessPart) / 100000) + hangingPlaceValue;
                                }
                            }
                            else if (SalesOrderDetail.Product.ProductKind.Name.Contains("ATLET"))
                            {
                                GramMetretul = (((width * Height * Density * 2 * Thickness) -
                                    (width * 2 * Thickness * SalesOrderDetail.Product.BladeDeep * Density) - (BladeDeep * ((BladeWidth - width)) * 4 * Thickness * Density)) / 1000000) + hangingPlaceValue;
                            }
                            else
                            {
                                if (ShippingFilmType.Name == "YAPRAK")
                                {
                                    int height = 1000;
                                    GramMetretul = ((width * height * Density * Thickness) / 100000) + hangingPlaceValue;
                                }
                                else
                                {
                                    GramMetretul = (((width * Height * Density * Thickness) * thicknessPart) / 100000) + hangingPlaceValue;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
