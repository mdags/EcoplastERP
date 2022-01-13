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
    [Appearance("Eco6LaminationWorkOrder.LineDeliveryDateAppearance", TargetItems = "WorkName", Criteria = "SalesOrderDetail.LineDeliveryDate < LocalDateTimeToday()", BackColor = "MistyRose")]
    [Appearance("Eco6LaminationWorkOrder.TotalProductionQuantityAppearance", TargetItems = "TotalProduction", Criteria = "TotalProduction > Quantity", BackColor = "GreenYellow")]
    [Appearance("Eco6LaminationWorkOrder.TotalProductionQuantityAppearance1", TargetItems = "TotalProduction", Criteria = "TotalProduction <= Quantity", BackColor = "LemonChiffon")]
    [Appearance("Eco6LaminationWorkOrder.CompletedAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'ProductionComplete'", FontStyle = FontStyle.Strikeout, BackColor = "LemonChiffon")]
    [Appearance("Eco6LaminationWorkOrder.CanceledAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'Canceled'", FontStyle = FontStyle.Strikeout, BackColor = "MistyRose")]
    [Appearance("Eco6LaminationWorkOrder.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "Eco6LaminationWorkOrder_ListView")]
    [Appearance("Eco6LaminationWorkOrder_MachineLoad.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "Eco6LaminationWorkOrder_ListView_MachineLoad")]
    public class Eco6LaminationWorkOrder : BaseObject
    {
        public Eco6LaminationWorkOrder(Session session)
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
            Station = Session.FindObject<Station>(CriteriaOperator.Parse("Name = 'Eco6 Laminasyon'"));
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

            //Eco6LaminationWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbReciept = new StringBuilder();
            //foreach (var reciept in Eco6LaminationWorkOrderReciepts)
            //{
            //    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));

            //    reciept.Quantity = Quantity * reciept.Rate / 100;
            //}
            //RecieptString = sbReciept.ToString();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforEco6Lamination;
        }
        // Fields...
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private int _ShippingBobbinLength;
        private int _BandCount;
        private int _WayCount;
        private string _RecieptString;
        private Reciept _Reciept;
        private string _PaletteNumber;
        private Int16 _PaletteBobbinCount;
        private Palette _Palette;
        private ShippingPackageType _ShippingPackageType;
        private string _Tension;
        private int _LineSpeed;
        private string _Performance;
        private string _DoubleComponentBSprayGun;
        private string _DoubleComponentBHose;
        private string _DoubleComponentBReservoir;
        private int _DoubleComponentB;
        private string _DoubleComponentASprayGun;
        private string _DoubleComponentAHose;
        private string _DoubleComponentAReservoir;
        private int _DoubleComponentA;
        private decimal _DoubleComponentGramM2;
        private string _DoubleComponent;
        private string _SingleComponentSprayGun;
        private string _SingleComponentHose;
        private string _SingleComponentReservoir;
        private decimal _SingleComponentGramM2;
        private string _SingleComponent;
        private int _CoatingRollerSize;
        private string _TopFilmTension;
        private int _TopFilmLength;
        private int _TopFilmWidth;
        private decimal _TopFilmThickness;
        private string _TopFilm;
        private string _SubFilmTension;
        private int _SubFilmLength;
        private int _SubFilmWidth;
        private decimal _SubFilmThickness;
        private string _SubFilm;
        private Bobbin _Bobbin;
        private decimal _MaximumRollWeight;
        private decimal _MinimumRollWeight;
        private decimal _RollWeight;
        private decimal _RollDiameter;
        private decimal _Density;
        private int _Length;
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
        private int _SequenceNumber;
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
        public int SequenceNumber
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

        [Appearance("Eco6LaminationWorkOrder.WorkName", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("Eco6LaminationWorkOrder = ?", Oid)));
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Wastage), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("Eco6LaminationWorkOrder = ?", Oid)));
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

        [Appearance("Eco6LaminationWorkOrder.PaletteStickerDesign", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                SetPropertyValue("Width", ref _Width, value);
            }
        }

        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                SetPropertyValue("Height", ref _Height, value);
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

        public decimal Density
        {
            get
            {
                return _Density;
            }
            set
            {
                SetPropertyValue("Density", ref _Density, value);
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

        [RuleValueComparison("Eco6LaminationWorkOrder.MaximumRollWeight", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        public int BandCount
        {
            get
            {
                return _BandCount;
            }
            set
            {
                SetPropertyValue("BandCount", ref _BandCount, value);
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

        public int ShippingBobbinLength
        {
            get
            {
                return _ShippingBobbinLength;
            }
            set
            {
                SetPropertyValue("ShippingBobbinLength", ref _ShippingBobbinLength, value);
            }
        }

        public string SubFilm
        {
            get
            {
                return _SubFilm;
            }
            set
            {
                SetPropertyValue("SubFilm", ref _SubFilm, value);
            }
        }

        public decimal SubFilmThickness
        {
            get
            {
                return _SubFilmThickness;
            }
            set
            {
                SetPropertyValue("SubFilmThickness", ref _SubFilmThickness, value);
            }
        }

        public int SubFilmWidth
        {
            get
            {
                return _SubFilmWidth;
            }
            set
            {
                SetPropertyValue("SubFilmWidth", ref _SubFilmWidth, value);
            }
        }

        public int SubFilmLength
        {
            get
            {
                return _SubFilmLength;
            }
            set
            {
                SetPropertyValue("SubFilmLength", ref _SubFilmLength, value);
            }
        }

        public string SubFilmTension
        {
            get
            {
                return _SubFilmTension;
            }
            set
            {
                SetPropertyValue("SubFilmTension", ref _SubFilmTension, value);
            }
        }

        public string TopFilm
        {
            get
            {
                return _TopFilm;
            }
            set
            {
                SetPropertyValue("TopFilm", ref _TopFilm, value);
            }
        }

        public decimal TopFilmThickness
        {
            get
            {
                return _TopFilmThickness;
            }
            set
            {
                SetPropertyValue("TopFilmThickness", ref _TopFilmThickness, value);
            }
        }

        public int TopFilmWidth
        {
            get
            {
                return _TopFilmWidth;
            }
            set
            {
                SetPropertyValue("TopFilmWidth", ref _TopFilmWidth, value);
            }
        }

        public int TopFilmLength
        {
            get
            {
                return _TopFilmLength;
            }
            set
            {
                SetPropertyValue("TopFilmLength", ref _TopFilmLength, value);
            }
        }

        public string TopFilmTension
        {
            get
            {
                return _TopFilmTension;
            }
            set
            {
                SetPropertyValue("TopFilmTension", ref _TopFilmTension, value);
            }
        }

        public int CoatingRollerSize
        {
            get
            {
                return _CoatingRollerSize;
            }
            set
            {
                SetPropertyValue("CoatingRollerSize", ref _CoatingRollerSize, value);
            }
        }

        public string SingleComponent
        {
            get
            {
                return _SingleComponent;
            }
            set
            {
                SetPropertyValue("SingleComponent", ref _SingleComponent, value);
            }
        }

        public decimal SingleComponentGramM2
        {
            get
            {
                return _SingleComponentGramM2;
            }
            set
            {
                SetPropertyValue("SingleComponentGramM2", ref _SingleComponentGramM2, value);
            }
        }

        public string SingleComponentReservoir
        {
            get
            {
                return _SingleComponentReservoir;
            }
            set
            {
                SetPropertyValue("SingleComponentReservoir", ref _SingleComponentReservoir, value);
            }
        }

        public string SingleComponentHose
        {
            get
            {
                return _SingleComponentHose;
            }
            set
            {
                SetPropertyValue("SingleComponentHose", ref _SingleComponentHose, value);
            }
        }

        public string SingleComponentSprayGun
        {
            get
            {
                return _SingleComponentSprayGun;
            }
            set
            {
                SetPropertyValue("SingleComponentSprayGun", ref _SingleComponentSprayGun, value);
            }
        }

        public string DoubleComponent
        {
            get
            {
                return _DoubleComponent;
            }
            set
            {
                SetPropertyValue("DoubleComponent", ref _DoubleComponent, value);
            }
        }

        public decimal DoubleComponentGramM2
        {
            get
            {
                return _DoubleComponentGramM2;
            }
            set
            {
                SetPropertyValue("DoubleComponentGramM2", ref _DoubleComponentGramM2, value);
            }
        }

        public int DoubleComponentA
        {
            get
            {
                return _DoubleComponentA;
            }
            set
            {
                SetPropertyValue("DoubleComponentA", ref _DoubleComponentA, value);
            }
        }

        public string DoubleComponentAReservoir
        {
            get
            {
                return _DoubleComponentAReservoir;
            }
            set
            {
                SetPropertyValue("DoubleComponentAReservoir", ref _DoubleComponentAReservoir, value);
            }
        }

        public string DoubleComponentAHose
        {
            get
            {
                return _DoubleComponentAHose;
            }
            set
            {
                SetPropertyValue("DoubleComponentAHose", ref _DoubleComponentAHose, value);
            }
        }

        public string DoubleComponentASprayGun
        {
            get
            {
                return _DoubleComponentASprayGun;
            }
            set
            {
                SetPropertyValue("DoubleComponentASprayGun", ref _DoubleComponentASprayGun, value);
            }
        }

        public int DoubleComponentB
        {
            get
            {
                return _DoubleComponentB;
            }
            set
            {
                SetPropertyValue("DoubleComponentB", ref _DoubleComponentB, value);
            }
        }

        public string DoubleComponentBReservoir
        {
            get
            {
                return _DoubleComponentBReservoir;
            }
            set
            {
                SetPropertyValue("DoubleComponentBReservoir", ref _DoubleComponentBReservoir, value);
            }
        }

        public string DoubleComponentBHose
        {
            get
            {
                return _DoubleComponentBHose;
            }
            set
            {
                SetPropertyValue("DoubleComponentBHose", ref _DoubleComponentBHose, value);
            }
        }

        public string DoubleComponentBSprayGun
        {
            get
            {
                return _DoubleComponentBSprayGun;
            }
            set
            {
                SetPropertyValue("DoubleComponentBSprayGun", ref _DoubleComponentBSprayGun, value);
            }
        }

        public string Performance
        {
            get
            {
                return _Performance;
            }
            set
            {
                SetPropertyValue("Performance", ref _Performance, value);
            }
        }

        public int LineSpeed
        {
            get
            {
                return _LineSpeed;
            }
            set
            {
                SetPropertyValue("LineSpeed", ref _LineSpeed, value);
            }
        }

        public string Tension
        {
            get
            {
                return _Tension;
            }
            set
            {
                SetPropertyValue("Tension", ref _Tension, value);
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

        [Appearance("Eco6LaminatingWorkOrder.PaletteNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
        [ImmediatePostData]
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
        public XPCollection<Eco6LaminationWorkOrderReciept> Eco6LaminationWorkOrderReciepts
        {
            get { return GetCollection<Eco6LaminationWorkOrderReciept>("Eco6LaminationWorkOrderReciepts"); }
        }

        [Association("Eco6LaminationWorkOrder-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }
        }

        [Association("Eco6LaminationWorkOrder-Wastages")]
        public XPCollection<Wastage> Wastages
        {
            get
            {
                return GetCollection<Wastage>("Wastages");
            }
        }

        [Association("Eco6LaminationWorkOrder-ReadResources")]
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
                if (!NextStation.IsLastStation) MaximumRollWeight = 1000;
            }
        }
        void GetSalesOrderDetail()
        {
            if (IsLoading) return;
            if (SalesOrderDetail != null)
            {
                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail.Oid));
                if (eco6LaminationWorkOrder != null) XtraMessageBox.Show(string.Format("Bu müşteri siparişi için daha önce {0} numaralı üretim siparişi doldurulmuş. Mükerrer bir kayıt olup olmadığını kontrol ediniz...", eco6LaminationWorkOrder.WorkOrderNumber), "Bilgilendirme", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

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
                Density = SalesOrderDetail.Product.Density;
                ShippingPackageType = SalesOrderDetail.Product.ShippingPackageType;
                ProductionNote = SalesOrderDetail.WorkOrderNote;
                QualityNote = SalesOrderDetail.QualityNote;
                Palette = SalesOrderDetail.Palette ?? null;

                Session.Delete(Eco6LaminationWorkOrderReciepts);
                Eco6LaminationWorkOrderReciept reciept = new Eco6LaminationWorkOrderReciept(Session)
                {
                    Eco6LaminationWorkOrder = this,
                    Product = Session.FindObject<Product>(new BinaryOperator("Oid", SalesOrderDetail.Product.Oid)),
                    Warehouse = Session.FindObject<Warehouse>(new BinaryOperator("Oid", Station.SourceWarehouse.Oid)),
                    Rate = 99,
                    Unit = Session.FindObject<Unit>(new BinaryOperator("Oid", SalesOrderDetail.Product.Unit.Oid))
                };
                Eco6LaminationWorkOrderReciepts.Add(reciept);
            }
        }
        void GetReciept()
        {
            if (IsLoading) return;
            if (Reciept != null)
            {
                Session.Delete(Eco6LaminationWorkOrderReciepts);
                foreach (var item in Reciept.RecieptDetails)
                {
                    var reciept = new Eco6LaminationWorkOrderReciept(Session)
                    {
                        Eco6LaminationWorkOrder = this,
                        Product = item.Product,
                        Warehouse = item.Warehouse,
                        Rate = item.Rate,
                        Unit = item.Unit
                    };
                    this.Eco6LaminationWorkOrderReciepts.Add(reciept);
                }

                Eco6LaminationWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in Eco6LaminationWorkOrderReciepts)
                {
                    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));
                }
                RecieptString = sbReciept.ToString();
            }
        }
        #endregion
    }
}
