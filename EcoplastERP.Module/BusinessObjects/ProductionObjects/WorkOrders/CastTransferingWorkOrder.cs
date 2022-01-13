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
    [Appearance("CastTransferingWorkOrder.LineDeliveryDateAppearance", TargetItems = "WorkName", Criteria = "SalesOrderDetail.LineDeliveryDate < LocalDateTimeToday()", BackColor = "MistyRose")]
    [Appearance("CastTransferingWorkOrder.TotalProductionQuantityAppearance", TargetItems = "TotalProduction", Criteria = "TotalProduction > Quantity", BackColor = "GreenYellow")]
    [Appearance("CastTransferingWorkOrder.TotalProductionQuantityAppearance1", TargetItems = "TotalProduction", Criteria = "TotalProduction <= Quantity", BackColor = "LemonChiffon")]
    [Appearance("CastTransferingWorkOrder.CompletedAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'ProductionComplete'", FontStyle = FontStyle.Strikeout, BackColor = "LemonChiffon")]
    [Appearance("CastTransferingWorkOrder.CanceledAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'Canceled'", FontStyle = FontStyle.Strikeout, BackColor = "MistyRose")]
    [Appearance("CastTransferingWorkOrder.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "CastTransferingWorkOrder_ListView")]
    [Appearance("CastTransferingWorkOrder_MachineLoad.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "CastTransferingWorkOrder_ListView_MachineLoad")]
    public class CastTransferingWorkOrder : BaseObject
    {
        public CastTransferingWorkOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
            SequenceNumber = 99;
            WorkOrderDate = DateTime.Now;
            WorkOrderNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            Station = Session.FindObject<Station>(CriteriaOperator.Parse("Name = 'Cast Aktarma'"));
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

            if (WorkOrderStatus == WorkOrderStatus.ProductionComplete || WorkOrderStatus == WorkOrderStatus.Canceled) SequenceNumber = 9000;

            if (Session.Connection != null && Session.IsObjectToSave(this))
            {
                if (!NextStation.IsLastStation)
                {
                    if (Quantity > SalesOrderDetail.cQuantity + (SalesOrderDetail.cQuantity * SalesOrderDetail.SemiProductOption / 100))
                    {
                        throw new UserFriendlyException("Üretim miktarı siparişteki yarı mamül üretim opsiyonundan fazla olamaz.");
                    }
                }
                else
                {
                    if (Quantity > SalesOrderDetail.cQuantity + (SalesOrderDetail.cQuantity * SalesOrderDetail.LastProductOption / 100))
                    {
                        throw new UserFriendlyException("Üretim miktarı siparişteki mamül üretim opsiyonundan fazla olamaz.");
                    }
                }
            }

            //CastTransferingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbReciept = new StringBuilder();
            //foreach (var reciept in CastTransferingWorkOrderReciepts)
            //{
            //    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));

            //    reciept.Quantity = Quantity * reciept.Rate / 100;
            //}
            //RecieptString = sbReciept.ToString();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            //var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("SalesOrderDetail = ?", SalesOrderDetail));
            //if (cuttingWorkOrder == null)
            //{
            //    var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("SalesOrderDetail = ?", SalesOrderDetail));
            //    if (printingWorkOrder == null)
            //    {
            //        if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforCastTransfering;
            //        if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforCastTransfering;
            //        else if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforProduction) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforCastTransfering;
            //    }
            //    else throw new UserFriendlyException("Bu sipariş için Baskı Üretim Siparişi açılmış. Çekim Üretim Siparişini silebilmek için öncelikle Baskı Üretim Siparişini silmeniz gerekmekterdir.");
            //}
            //else throw new UserFriendlyException("Bu sipariş için Kesim Üretim Siparişi açılmış. Çekim Üretim Siparişini silebilmek için öncelikle Kesim Üretim Siparişini silmeniz gerekmekterdir.");
        }
        // Fields...
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private string _RecieptString;
        private string _PartString;
        private Reciept _Reciept;
        private string _PaletteNumber;
        private Int16 _PaletteBobbinCount;
        private string _PaletteLayout;
        private Palette _Palette;
        private Bobbin _Bobbin;
        private OuterPacking _OuterPacking;
        private bool _Embossing;
        private MaterialColor _MaterialColor;
        private ShippingFilmType _ShippingFilmType;
        private ShippingPackageType _ShippingPackageType;
        private decimal _InflationRate;
        private decimal _GramMetretul;
        private decimal _GramM2;
        private Int16 _WayCount;
        private decimal _MaximumRollWeight;
        private decimal _MinimumRollWeight;
        private int _RollCount;
        private int _RollDiameter;
        private decimal _RollWeight;
        private decimal _Density;
        private PrintName _PrintName;
        private string _CoronaPartial;
        private Corona _Corona;
        private decimal _Cap;
        private CapStatus _CapStatus;
        private string _Bellows;
        private BellowsStatus _BellowsStatus;
        private int _Lenght;
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

        [Appearance("CastTransferingWorkOrder.WorkName", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
        
        [ImmediatePostData]
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("CastTransferingWorkOrder = ?", Oid)));
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Wastage), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("CastTransferingWorkOrder = ?", Oid)));
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

        [Appearance("CastTransferingWorkOrder.PaletteStickerDesign", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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
        [RuleRequiredField]
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
        public int Lenght
        {
            get
            {
                return _Lenght;
            }
            set
            {
                SetPropertyValue("Lenght", ref _Lenght, value);
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

        [Appearance("CastTransferingWorkOrder.BellowsBellowsStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "BellowsStatus = 'None'")]
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

        [Appearance("CastTransferingWorkOrder.Cap", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "CapStatus = 'Uncapped'")]
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

        public Corona Corona
        {
            get
            {
                return _Corona;
            }
            set
            {
                SetPropertyValue("Corona", ref _Corona, value);
            }
        }

        [Appearance("Product.CoronaCoronaType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Corona != 'OneSidePartial' and Corona != 'DoubleSidePartial'")]
        public string CoronaPartial
        {
            get
            {
                return _CoronaPartial;
            }
            set
            {
                SetPropertyValue("CoronaPartial", ref _CoronaPartial, value);
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
        public decimal RollWeight
        {
            get
            {
                return _RollWeight;
            }
            set
            {
                SetPropertyValue("RollWeight", ref _RollWeight, value);
            }
        }

        public int RollDiameter
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

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "NextStation.Name = 'Son Üretim'")]
        public int RollCount
        {
            get
            {
                return _RollCount;
            }
            set
            {
                SetPropertyValue("RollCount", ref _RollCount, value);
            }
        }

        [RuleRequiredField]
        public decimal MinimumRollWeight
        {
            get
            {
                return _MinimumRollWeight;
            }
            set
            {
                SetPropertyValue("MinimumRollWeight", ref _MinimumRollWeight, value);
            }
        }

        [RuleRequiredField]
        public decimal MaximumRollWeight
        {
            get
            {
                return _MaximumRollWeight;
            }
            set
            {
                SetPropertyValue("MaximumRollWeight", ref _MaximumRollWeight, value);
            }
        }

        public Int16 WayCount
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

        public decimal InflationRate
        {
            get
            {
                return _InflationRate;
            }
            set
            {
                SetPropertyValue("InflationRate", ref _InflationRate, value);
            }
        }

        [Appearance("CastTransferingWorkOrder.ShippingPackageType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("CastTransferingWorkOrder.ShippingFilmType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        public MaterialColor MaterialColor
        {
            get
            {
                return _MaterialColor;
            }
            set
            {
                SetPropertyValue("MaterialColor", ref _MaterialColor, value);
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

        [RuleRequiredField]
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

        [Appearance("CastTransferingWorkOrder.Palette", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("CastTransferingWorkOrder.PaletteLayout", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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
        public Int16 PaletteBobbinCount
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

        [Appearance("CastTransferingWorkOrder.PaletteNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string PartString
        {
            get
            {
                return _PartString;
            }
            set
            {
                SetPropertyValue("PartString", ref _PartString, value);
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
        public XPCollection<CastTransferingWorkOrderReciept> CastTransferingWorkOrderReciepts
        {
            get { return GetCollection<CastTransferingWorkOrderReciept>("CastTransferingWorkOrderReciepts"); }
        }

        [Association("CastTransferingWorkOrder-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }
        }

        [Association("CastTransferingWorkOrder-Wastages")]
        public XPCollection<Wastage> Wastages
        {
            get
            {
                return GetCollection<Wastage>("Wastages");
            }
        }

        [Association("CastTransferingWorkOrder-ReadResources")]
        public XPCollection<ReadResource> ReadResources
        {
            get
            {
                return GetCollection<ReadResource>("ReadResources");
            }
        }

        [Action(PredefinedCategory.View, Caption = "Rezervasyon Oluştur", AutoCommit = true, ImageName = "Action_Workflow_Activate", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void CreateRezervation()
        {
            var _rezervation = new Rezervation(Session)
            {
                Description = String.Format("Cast Aktarma {0} tarihli rezervasyon", DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
            };
            foreach (var item in CastTransferingWorkOrderReciepts)
            {
                decimal quantity = item.Quantity;
                var store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                if (store != null)
                {
                    if (store.Quantity < quantity)
                    {
                        var rezervationDetail = new RezervationDetail(Session)
                        {
                            Rezervation = _rezervation,
                            Product = item.Product,
                            SourceWarehouse = item.Product.Warehouse,
                            DestinationWarehouse = item.Warehouse,
                            Unit = item.Unit,
                            RecieptQuantity = quantity,
                            Quantity = quantity - store.Quantity
                        };
                    }
                }
                else
                {
                    var rezervationDetail = new RezervationDetail(Session)
                    {
                        Rezervation = _rezervation,
                        Product = item.Product,
                        SourceWarehouse = item.Product.Warehouse,
                        DestinationWarehouse = item.Warehouse,
                        Unit = item.Unit,
                        RecieptQuantity = quantity,
                        Quantity = quantity
                    };
                }
            }
        }

        #region functions
        void SetMaximumRollWeight()
        {
            if (IsLoading) return;
            if (NextStation != null)
            {
                if (!NextStation.IsLastStation) MaximumRollWeight = 1000;
            }
        }
        void GetSalesOrderDetail()
        {
            if (IsLoading) return;
            if (SalesOrderDetail != null)
            {
                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail.Oid));
                if (castTransferingWorkOrder != null) XtraMessageBox.Show(string.Format("Bu müşteri siparişi için daha önce {0} numaralı üretim siparişi doldurulmuş. Mükerrer bir kayıt olup olmadığını kontrol ediniz...", castTransferingWorkOrder.WorkOrderNumber), "Bilgilendirme", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

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
                Lenght = SalesOrderDetail.Product.Lenght;
                BellowsStatus = SalesOrderDetail.Product.BellowsStatus;
                Bellows = SalesOrderDetail.Product.Bellows;
                CapStatus = SalesOrderDetail.Product.CapStatus;
                Cap = SalesOrderDetail.Product.Cap;
                Corona = SalesOrderDetail.Product.Corona;
                CoronaPartial = SalesOrderDetail.Product.CoronaPartial;
                PrintName = SalesOrderDetail.Product.PrintName;
                Density = SalesOrderDetail.Product.Density;
                RollWeight = SalesOrderDetail.Product.RollWeight;
                ShippingPackageType = SalesOrderDetail.Product.ShippingPackageType;
                ShippingFilmType = SalesOrderDetail.Product.ShippingFilmType;
                MaterialColor = SalesOrderDetail.Product.MaterialColor;
                Embossing = SalesOrderDetail.Product.Embossing;
                ProductionNote = SalesOrderDetail.WorkOrderNote;
                QualityNote = SalesOrderDetail.QualityNote;
                Palette = SalesOrderDetail.Palette ?? null;
            }
        }
        void GetReciept()
        {
            if (IsLoading) return;
            if (Reciept != null)
            {
                XPCollection<CastTransferingWorkOrderReciept> castTransferingReciepts = CastTransferingWorkOrderReciepts;
                Session.Delete(castTransferingReciepts);

                foreach (var item in Reciept.RecieptDetails)
                {
                    var _part = Session.FindObject<MachinePart>(CriteriaOperator.Parse("Machine = ? And Name = ?", Machine, item.MachinePartCode));
                    var castTransferingWorkOrderReciept = new CastTransferingWorkOrderReciept(Session)
                    {
                        CastTransferingWorkOrder = this,
                        Product = item.Product,
                        Warehouse = item.Warehouse,
                        Rate = item.Rate,
                        Unit = item.Unit
                    };
                    this.CastTransferingWorkOrderReciepts.Add(castTransferingWorkOrderReciept);
                }

                CastTransferingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in CastTransferingWorkOrderReciepts)
                {
                    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));
                }
                RecieptString = sbReciept.ToString();
            }
            CastTransferingWorkOrderReciepts.Filter = null;
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
                            decimal width = 0;
                            if (BellowsStatus == BellowsStatus.None) width = Width;
                            else
                            {
                                decimal bellow = Convert.ToDecimal(Bellows);
                                if (ShippingFilmType.Name == "ATLET POŞET" || ShippingFilmType.Name == "ÇÖP TORBASI" || ShippingFilmType.Name == "KÖRÜKLÜ HORTUM" || ShippingFilmType.Name == "RULO ATLET POŞET" || ShippingFilmType.Name == "RULO ÇÖP")
                                {
                                    width = Width + bellow + bellow;
                                }
                                else
                                {
                                    width = Width;
                                }
                            }
                            decimal hangingPlaceValue = SalesOrderDetail.Product.HangingLocation == HangingLocation.Does ? 0.01M : 0;
                            int thicknessPart = ShippingFilmType.ThicknessDoublePart ? 2 : 1;

                            if (ShippingFilmType.Name == "KARGO TORBASI" || ShippingFilmType.Name == "WİCKET")
                            {
                                decimal bellow = Convert.ToDecimal(Bellows);
                                GramMetretul = (((Width * (1000 + 1000 + bellow + bellow + Cap) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                            }
                            //else if (ShippingFilmType.Name.Contains("ATLET"))
                            //{
                            //    GramMetretul = (((width * 1000 * Density * 2 * Thickness) -
                            //        (width * 2 * Thickness * SalesOrderDetail.Product.BladeDeep * Density) - (BladeDeep * ((BladeWidth - width)) * 4 * Thickness * Density)) / 1000000) + hangingPlaceValue;
                            //}
                            else if (ShippingFilmType.Name == "TAKVİYELİ POŞET")
                            {
                                decimal bellow = Convert.ToDecimal(Bellows);
                                GramMetretul = (((Width * (1000 + 1000 + bellow + bellow) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                            }
                            else if (ShippingFilmType.Name == "YUMUŞAK KULP")
                            {
                                decimal bellow = Convert.ToDecimal(Bellows);
                                GramMetretul = (((Width * (1000 + 1000 + bellow + bellow + Cap + Cap) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                            }
                            else if (ShippingFilmType.Name == "KÖRÜKLÜ TEK TARAFI AÇIK ")
                            {
                                decimal bellow = Convert.ToDecimal(Bellows);
                                GramMetretul = ((((Width + bellow) * thicknessPart) * 1000 * Thickness * Density) / 1000000) + hangingPlaceValue;
                            }
                            else
                            {
                                GramMetretul = (((width * 1000 * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
