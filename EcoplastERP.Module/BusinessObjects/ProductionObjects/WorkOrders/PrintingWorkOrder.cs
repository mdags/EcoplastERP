using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.PlateArchiveObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [OptimisticLocking(false)]
    [ImageName("BO_Task")]
    [DefaultProperty("WorkOrderNumber")]
    [NavigationItem("ProductionManagement")]
    [Appearance("PrintingWorkOrder.LineDeliveryDateAppearance", TargetItems = "WorkName", Criteria = "SalesOrderDetail.LineDeliveryDate < LocalDateTimeToday()", BackColor = "MistyRose")]
    [Appearance("PrintingWorkOrder.TotalProductionQuantityAppearance", TargetItems = "TotalProduction", Criteria = "TotalProduction > Quantity", BackColor = "GreenYellow")]
    [Appearance("PrintingWorkOrder.TotalProductionQuantityAppearance1", TargetItems = "TotalProduction", Criteria = "TotalProduction <= Quantity", BackColor = "LemonChiffon")]
    [Appearance("PrintingWorkOrder.CompletedAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'ProductionComplete'", FontStyle = FontStyle.Strikeout, BackColor = "LemonChiffon")]
    [Appearance("PrintingWorkOrder.CanceledAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'Canceled'", FontStyle = FontStyle.Strikeout, BackColor = "MistyRose")]
    [Appearance("PrintingWorkOrder.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "PrintingWorkOrder_ListView")]
    [Appearance("PrintingWorkOrder_MachineLoad.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "PrintingWorkOrder_ListView_MachineLoad")]
    public class PrintingWorkOrder : BaseObject
    {
        public PrintingWorkOrder(Session session)
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
            Station = Session.FindObject<Station>(CriteriaOperator.Parse("Name = 'Baskı'"));
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

            //PrintingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbReciept = new StringBuilder();
            //foreach (var reciept in PrintingWorkOrderReciepts)
            //{
            //    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));

            //    reciept.Quantity = Quantity * reciept.Rate / 100;
            //}
            //RecieptString = sbReciept.ToString();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            //var _cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("SalesOrderDetail = ?", SalesOrderDetail));
            //if (_cuttingWorkOrder == null)
            //{
            //    if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforPrinting;
            //    else if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforProduction) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforPrinting;
            //}
            //else throw new UserFriendlyException("Bu sipariş için Kesim Üretim Siparişi açılmış. Çekim Üretim Siparişini silebilmek için öncelikle Kesim Üretim Siparişini silmeniz gerekmekterdir.");
        }
        // Fields...
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private string _RecieptString;
        private Reciept _Reciept;
        private string _PaletteNumber;
        private Int16 _PaletteBobbinCount;
        private Palette _Palette;
        private OuterPacking _OuterPacking;
        private ShippingPackageType _ShippingPackageType;
        private string _Barcode;
        private bool _Logo;
        private string _CylinderRepetition;
        private string _ClicheCode;
        private decimal _ClicheThickness;
        private string _ClicheBand;
        private RollDirection _RollDirection;
        private PrintWorkStatus _PrintWorkStatus;
        private PrintDirection _PrintDirection;
        private PrintForm _PrintForm;
        private string _PrintingColors;
        private int _LineSpeed;
        private Int16 _ColorCount;
        private Plate _Plate;
        private string _Anilox;
        private PrintName _PrintName;
        private MaterialColor _MaterialColor;
        private Int16 _BandCount;
        private int _RollCount;
        private int _RollDiameter;
        private decimal _MaximumRollWeight;
        private decimal _MinimumRollWeight;
        private decimal _RollWeight;
        private Bobbin _Bobbin;
        private string _Size;
        private decimal _Density;
        private int _Lenght;
        private decimal _Thickness;
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

        [Appearance("PrintingWorkOrder.WorkName", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("PrintingWorkOrder = ?", Oid)));
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Wastage), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("PrintingWorkOrder = ?", Oid)));
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

        [Appearance("PrintingWorkOrder.PaletteStickerDesign", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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
            }
        }

        public int Lenght
        {
            get
            {
                return _Lenght;
            }
            set
            {
                SetPropertyValue("Lenght", ref _Lenght, value);
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Size
        {
            get
            {
                return _Size;
            }
            set
            {
                SetPropertyValue("Size", ref _Size, value);
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

        public Int16 BandCount
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

        public string Anilox
        {
            get
            {
                return _Anilox;
            }
            set
            {
                SetPropertyValue("Anilox", ref _Anilox, value);
            }
        }

        public Plate Plate
        {
            get
            {
                return _Plate;
            }
            set
            {
                SetPropertyValue("Plate", ref _Plate, value);
            }
        }

        public Int16 ColorCount
        {
            get
            {
                return _ColorCount;
            }
            set
            {
                SetPropertyValue("ColorCount", ref _ColorCount, value);
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

        public string PrintingColors
        {
            get
            {
                return _PrintingColors;
            }
            set
            {
                SetPropertyValue("PrintingColors", ref _PrintingColors, value);
            }
        }

        public PrintForm PrintForm
        {
            get
            {
                return _PrintForm;
            }
            set
            {
                SetPropertyValue("PrintForm", ref _PrintForm, value);
            }
        }

        public PrintDirection PrintDirection
        {
            get
            {
                return _PrintDirection;
            }
            set
            {
                SetPropertyValue("PrintDirection", ref _PrintDirection, value);
            }
        }

        public PrintWorkStatus PrintWorkStatus
        {
            get
            {
                return _PrintWorkStatus;
            }
            set
            {
                SetPropertyValue("PrintWorkStatus", ref _PrintWorkStatus, value);
            }
        }

        public RollDirection RollDirection
        {
            get
            {
                return _RollDirection;
            }
            set
            {
                SetPropertyValue("RollDirection", ref _RollDirection, value);
            }
        }

        public string ClicheBand
        {
            get
            {
                return _ClicheBand;
            }
            set
            {
                SetPropertyValue("ClicheBand", ref _ClicheBand, value);
            }
        }

        public decimal ClicheThickness
        {
            get
            {
                return _ClicheThickness;
            }
            set
            {
                SetPropertyValue("ClicheThickness", ref _ClicheThickness, value);
            }
        }

        public string ClicheCode
        {
            get
            {
                return _ClicheCode;
            }
            set
            {
                SetPropertyValue("ClicheCode", ref _ClicheCode, value);
            }
        }

        public string CylinderRepetition
        {
            get
            {
                return _CylinderRepetition;
            }
            set
            {
                SetPropertyValue("CylinderRepetition", ref _CylinderRepetition, value);
            }
        }

        public bool Logo
        {
            get
            {
                return _Logo;
            }
            set
            {
                SetPropertyValue("Logo", ref _Logo, value);
            }
        }

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

        [Appearance("PrintingWorkOrder.PaletteNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Size(-1)]
        [Delayed(true)]
        [VisibleInListView(false)]
        [ValueConverter(typeof(ImageValueConverter))]
        [ImageEditor(DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
        public Image PrintingVisual
        {
            get { return GetPropertyValue<Image>("PrintingVisual"); }
            set { SetPropertyValue<Image>("PrintingVisual", value); }
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

        public decimal FilmingRollCount
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse = ?", SalesOrderDetail, Station.SourceWarehouse)));
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
        public XPCollection<PrintingWorkOrderReciept> PrintingWorkOrderReciepts
        {
            get { return GetCollection<PrintingWorkOrderReciept>("PrintingWorkOrderReciepts"); }
        }

        [Association("PrintingWorkOrder-PrintingWorkOrderProcess")]
        public XPCollection<PrintingWorkOrderProcess> PrintingWorkOrderProcess
        {
            get
            {
                return GetCollection<PrintingWorkOrderProcess>("PrintingWorkOrderProcess");
            }
        }

        [Association("PrintingWorkOrder-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }
        }

        [Association("PrintingWorkOrder-Wastages")]
        public XPCollection<Wastage> Wastages
        {
            get
            {
                return GetCollection<Wastage>("Wastages");
            }
        }

        [Association("PrintingWorkOrder-ReadResources")]
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
                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail.Oid));
                if (printingWorkOrder != null) XtraMessageBox.Show(string.Format("Bu müşteri siparişi için daha önce {0} numaralı üretim siparişi doldurulmuş. Mükerrer bir kayıt olup olmadığını kontrol ediniz...", printingWorkOrder.WorkOrderNumber), "Bilgilendirme", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

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
                Thickness = SalesOrderDetail.Product.Thickness;
                Density = SalesOrderDetail.Product.Density;
                MaterialColor = SalesOrderDetail.Product.MaterialColor ?? null;
                PrintName = SalesOrderDetail.Product.PrintName ?? null;
                PrintingColors = SalesOrderDetail.Product.PrintingColors;
                PrintForm = SalesOrderDetail.Product.PrintForm;
                PrintDirection = SalesOrderDetail.Product.PrintDirection;
                RollDirection = SalesOrderDetail.Product.RollDirection;
                Logo = SalesOrderDetail.Product.Logo;
                Barcode = SalesOrderDetail.Product.Barcode;
                ProductionNote = SalesOrderDetail.WorkOrderNote;
                QualityNote = SalesOrderDetail.QualityNote;
                ShippingPackageType = SalesOrderDetail.Product.ShippingPackageType ?? null;
                Palette = SalesOrderDetail.Palette ?? null;

                Session.Delete(PrintingWorkOrderReciepts);
                PrintingWorkOrderReciept reciept = new PrintingWorkOrderReciept(Session)
                {
                    PrintingWorkOrder = this,
                    Product = Session.FindObject<Product>(new BinaryOperator("Oid", SalesOrderDetail.Product.Oid)),
                    Warehouse = Session.FindObject<Warehouse>(new BinaryOperator("Oid", Station.SourceWarehouse.Oid)),
                    Rate = 99,
                    Unit = Session.FindObject<Unit>(new BinaryOperator("Oid", SalesOrderDetail.Product.Unit.Oid))
                };
                PrintingWorkOrderReciepts.Add(reciept);
            }
        }
        void GetReciept()
        {
            if (IsLoading) return;
            if (Reciept != null)
            {
                Session.Delete(PrintingWorkOrderReciepts);
                foreach (var item in Reciept.RecieptDetails)
                {
                    var printingWorkOrderReciept = new PrintingWorkOrderReciept(Session)
                    {
                        PrintingWorkOrder = this,
                        Product = item.Product,
                        Warehouse = item.Warehouse,
                        Rate = item.Rate,
                        Unit = item.Unit
                    };
                    this.PrintingWorkOrderReciepts.Add(printingWorkOrderReciept);
                }

                PrintingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in PrintingWorkOrderReciepts)
                {
                    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));
                }
                RecieptString = sbReciept.ToString();
            }
        }
        #endregion
    }
}
