using System;
using System.Linq;
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
using EcoplastERP.Module.BusinessObjects.QualityObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Task")]
    [DefaultProperty("WorkOrderNumber")]
    [NavigationItem("ProductionManagement")]
    [Appearance("FilmingWorkOrder.LineDeliveryDateAppearance", TargetItems = "WorkName", Criteria = "SalesOrderDetail.LineDeliveryDate < LocalDateTimeToday()", BackColor = "MistyRose")]
    [Appearance("FilmingWorkOrder.TotalProductionQuantityAppearance", TargetItems = "TotalProduction", Criteria = "TotalProduction > Quantity", BackColor = "GreenYellow")]
    [Appearance("FilmingWorkOrder.TotalProductionQuantityAppearance1", TargetItems = "TotalProduction", Criteria = "TotalProduction <= Quantity", BackColor = "LemonChiffon")]
    [Appearance("FilmingWorkOrder.CompletedAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'ProductionComplete'", FontStyle = FontStyle.Strikeout, BackColor = "LemonChiffon")]
    [Appearance("FilmingWorkOrder.CanceledAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'Canceled'", FontStyle = FontStyle.Strikeout, BackColor = "MistyRose")]
    [Appearance("FilmingWorkOrder.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "FilmingWorkOrder_ListView")]
    [Appearance("FilmingWorkOrder_MachineLoad.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "FilmingWorkOrder_ListView_MachineLoad")]
    public class FilmingWorkOrder : BaseObject
    {
        public FilmingWorkOrder(Session session)
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
            Station = Session.FindObject<Station>(CriteriaOperator.Parse("Name = 'Çekim'"));
            PaletteStickerDesign = Session.FindObject<ReportDataV2>(CriteriaOperator.Parse("Contains([DisplayName], ?)", "Standart Palet Dökümü"));
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
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

                decimal totalPartThickness = 0;
                foreach (var item in FilmingWorkOrderParts)
                {
                    totalPartThickness += item.Thickness;
                }
                if (totalPartThickness != Thickness) throw new UserFriendlyException("Kat mikronları toplamı film mikronuna eşit değildir. Kat miktornlarını kontrol edin...");

                //Ortalama Yoğunluk ve Slip Factor Hesaplama
                decimal totalDensity = 0, totalSlipFactor = 0;
                foreach (FilmingWorkOrderReciept reciept in FilmingWorkOrderReciepts)
                {
                    decimal partThickness = 0;
                    foreach (FilmingWorkOrderPart part in FilmingWorkOrderParts)
                    {
                        if (part.MachinePart.Name == reciept.MachinePart.Name) partThickness = part.Thickness;
                    }
                    totalDensity += (partThickness / Thickness) * (reciept.WorkOrderRate * reciept.Product.RawMaterialDensity);
                    totalSlipFactor += (partThickness * reciept.WorkOrderRate * reciept.Product.SlipFactor) / 100;
                }
                //Density = totalDensity / 100;
                AvgDensity = totalDensity / 100;
                AvgSlipFactor = totalSlipFactor;

                //Mfi Hesaplama
                const double ee = 2.71828182845904;//5235360287471352662497757247;
                double totalMfi = 0;
                var recieptProductGroupList = from t in FilmingWorkOrderReciepts group t by t.Product into tt select new { Product = tt.Key };
                foreach (var item in recieptProductGroupList)
                {
                    XPCollection<FilmingWorkOrderReciept> recieptList = FilmingWorkOrderReciepts;
                    recieptList.Filter = new BinaryOperator("Product", item.Product);
                    double rateTotal = 0, lnTotal = 0;
                    foreach (var reciept in recieptList)
                    {
                        decimal partThickness = 0;
                        foreach (FilmingWorkOrderPart part in FilmingWorkOrderParts)
                        {
                            if (part.MachinePart.Name == reciept.MachinePart.Name) partThickness = part.Thickness;
                        }
                        rateTotal += ((double)partThickness / (double)Thickness) * (double)reciept.WorkOrderRate;
                        lnTotal += Math.Log((double)reciept.Product.MFI, ee);
                    }
                    totalMfi += (rateTotal * lnTotal) / 100;
                }
                AvgMFI = Convert.ToDecimal(Math.Pow(ee, totalMfi));

                if (NextStation != null)
                {
                    if (SalesOrderDetail != null)
                    {
                        if (!NextStation.IsLastStation) ProductionOption = SalesOrderDetail.SemiProductOption;
                        else ProductionOption = SalesOrderDetail.LastProductOption;
                    }
                }

                UpdateRecieptRate();
            }

            //FilmingWorkOrderParts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbPart = new StringBuilder();
            //foreach (var item in FilmingWorkOrderParts)
            //{
            //    sbPart.Append(string.Format("{0} : {1:n1}  /  ", item.MachinePart.Name, item.Thickness));
            //}
            //PartString = sbPart.ToString();

            //FilmingWorkOrderReciepts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbReciept = new StringBuilder();
            //foreach (var item in FilmingWorkOrderParts)
            //{
            //    FilmingWorkOrderReciepts.Filter = new BinaryOperator("MachinePart.Name", item.MachinePart.Name);
            //    foreach (var reciept in FilmingWorkOrderReciepts)
            //    {
            //        sbReciept.AppendLine(string.Format("{0} - %{1:n2} - {2}  /  ", reciept.MachinePart.Name, reciept.WorkOrderRate, reciept.Product.Name));

            //        if (item.MachinePart.Name == reciept.MachinePart.Name)
            //        {
            //            reciept.Rate = reciept.WorkOrderRate * (item.Thickness * 100 / Thickness) / 100;
            //            reciept.Quantity = Quantity * reciept.Rate / 100;
            //        }
            //    }
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
            //        if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforFilming;
            //        if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforFilming;
            //        else if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforProduction) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforFilming;
            //    }
            //    else throw new UserFriendlyException("Bu sipariş için Baskı Üretim Siparişi açılmış. Çekim Üretim Siparişini silebilmek için öncelikle Baskı Üretim Siparişini silmeniz gerekmekterdir.");
            //}
            //else throw new UserFriendlyException("Bu sipariş için Kesim Üretim Siparişi açılmış. Çekim Üretim Siparişini silebilmek için öncelikle Kesim Üretim Siparişini silmeniz gerekmekterdir.");
        }
        // Fields...
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private int _ClosedWidth;
        private int _OpenedWidth;
        private int _FilmingWidth;
        private int _Lip;
        private int _Head2;
        private int _Head1;
        private int _Neck;
        private int _TowerSpeed;
        private int _IBCExtruder;
        private int _IBCFeed;
        private int _BubbleCooling;
        private int _WrapperTension;
        private int _FilmingTension;
        private int _CoolingLine;
        private int _LineSpeed;
        private string _RecieptString;
        private string _PartString;
        private Reciept _Reciept;
        private decimal _AvgSlipFactor;
        private decimal _AvgDensity;
        private decimal _AvgMFI;
        private string _PaletteNumber;
        private Int16 _PaletteBobbinCount;
        private string _PaletteLayout;
        private Palette _Palette;
        private Bobbin _Bobbin;
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
        private decimal _AdditiveRate;
        private Additive _Additive;
        private decimal _Density;
        private PrintName _PrintName;
        private string _CoronaPartial;
        private CoronaDirection _CoronaDirection;
        private Corona _Corona;
        private decimal _Cap;
        private CapStatus _CapStatus;
        private string _Bellows;
        private BellowsStatus _BellowsStatus;
        private int _Lenght;
        private decimal _Gsm;
        private decimal _Thickness;
        private int _Height;
        private int _Width;
        private FilmingFilmType _FilmingFilmType;
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

        [Appearance("FilmingWorkOrder.WorkName", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [ImmediatePostData]
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

        [ImmediatePostData]
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
                CalculateInflationRate();
                CreateWorkOrderParts();
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
                SetProductionOption();
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
                UpdateRecieptRate();
            }
        }

        [VisibleInDetailView(false)]
        public decimal TotalProduction
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("FilmingWorkOrder = ?", Oid)));
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Wastage), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("FilmingWorkOrder = ?", Oid)));
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
                return Machine != null ? Machine.Capacity != 0 ? (RemainingProduction / (Machine.Capacity / 24)) : 0 : 0;
            }
        }

        [Appearance("FilmingWorkOrder.PaletteStickerDesign", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("FilmingWorkOrder.ProductionOption", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Station.Name = 'Son Üretim'")]
        //[ModelDefault("MinValue", "0"), ModelDefault("MaxValue", "20")]
        public decimal ProductionOption
        {
            get
            {
                return _ProductionOption;
            }
            set
            {
                SetPropertyValue("ProductionOption", ref _ProductionOption, value);
                CheckOption();
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
        public FilmingFilmType FilmingFilmType
        {
            get
            {
                return _FilmingFilmType;
            }
            set
            {
                SetPropertyValue("FilmingFilmType", ref _FilmingFilmType, value);
                SetWidth();
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
                //CalculateGramMetretul();
                CalculateInflationRate();
            }
        }

        public int FilmingWidth
        {
            get
            {
                return _FilmingWidth;
            }
            set
            {
                SetPropertyValue("FilmingWidth", ref _FilmingWidth, value);
            }
        }

        public int OpenedWidth
        {
            get
            {
                return _OpenedWidth;
            }
            set
            {
                SetPropertyValue("OpenedWidth", ref _OpenedWidth, value);
            }
        }

        public int ClosedWidth
        {
            get
            {
                return _ClosedWidth;
            }
            set
            {
                SetPropertyValue("ClosedWidth", ref _ClosedWidth, value);
            }
        }

        //public int FilmingWidth
        //{
        //    get
        //    {
        //        int width = 0;
        //        if (FilmingFilmType != null)
        //        {
        //            if (FilmingFilmType.Name == "HORTUM")
        //            {
        //                width = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
        //            }
        //            else if (FilmingFilmType.Name == "KÖRÜKLÜ HORTUM")
        //            {
        //                width = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
        //            }
        //            else if (FilmingFilmType.Name == "YAPRAK")
        //            {
        //                if (SalesOrderDetail != null)
        //                {
        //                    if (SalesOrderDetail.Product.PrintStatus == PrintStatus.Printed)
        //                    {
        //                        PrintingWorkOrder printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
        //                        width = printingWorkOrder != null ? printingWorkOrder.Width : 0;
        //                    }
        //                    else
        //                    {
        //                        SlicingWorkOrder slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
        //                        width = slicingWorkOrder != null ? slicingWorkOrder.Width : 0;
        //                    }
        //                }
        //            }
        //        }
        //        return width;
        //    }
        //}

        //public int OpenedWidth
        //{
        //    get
        //    {
        //        int width = 0;
        //        if (FilmingFilmType != null)
        //        {
        //            if (FilmingFilmType.Name == "HORTUM")
        //            {
        //                width = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
        //            }
        //            else if (FilmingFilmType.Name == "KÖRÜKLÜ HORTUM")
        //            {
        //                width = SalesOrderDetail != null ? SalesOrderDetail.Product.Width + (Convert.ToInt32(SalesOrderDetail.Product.Bellows) * 2) : Width + (Convert.ToInt32(Bellows) * 2);
        //            }
        //            else if (FilmingFilmType.Name == "YAPRAK")
        //            {
        //                if (SalesOrderDetail != null)
        //                {
        //                    if (SalesOrderDetail.Product.PrintStatus == PrintStatus.Printed)
        //                    {
        //                        PrintingWorkOrder printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
        //                        width = printingWorkOrder != null ? printingWorkOrder.Width : 0;
        //                    }
        //                    else
        //                    {
        //                        SlicingWorkOrder slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
        //                        width = slicingWorkOrder != null ? slicingWorkOrder.Width : 0;
        //                    }
        //                }
        //            }
        //        }
        //        return width;
        //    }
        //}

        //public int ClosedWidth
        //{
        //    get
        //    {
        //        int width = 0;
        //        if (FilmingFilmType != null)
        //        {
        //            if (FilmingFilmType.Name == "HORTUM")
        //            {
        //                width = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
        //            }
        //            else if (FilmingFilmType.Name == "KÖRÜKLÜ HORTUM")
        //            {
        //                width = SalesOrderDetail != null ? SalesOrderDetail.Product.Width - (Convert.ToInt32(SalesOrderDetail.Product.Bellows) * 2) : Width - (Convert.ToInt32(Bellows) * 2);
        //            }
        //            else if (FilmingFilmType.Name == "YAPRAK")
        //            {
        //                if (SalesOrderDetail != null)
        //                {
        //                    if (SalesOrderDetail.Product.PrintStatus == PrintStatus.Printed)
        //                    {
        //                        PrintingWorkOrder printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
        //                        width = printingWorkOrder != null ? printingWorkOrder.Width : 0;
        //                    }
        //                    else
        //                    {
        //                        SlicingWorkOrder slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
        //                        width = slicingWorkOrder != null ? slicingWorkOrder.Width : 0;
        //                    }
        //                }
        //            }
        //        }
        //        return width;
        //    }
        //}

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
                //CalculateGramMetretul();
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
                CalculateGramM2();
                //CalculateGramMetretul();
                CalculateRollDiameter();
            }
        }

        public decimal Gsm
        {
            get
            {
                return _Gsm;
            }
            set
            {
                SetPropertyValue("Gsm", ref _Gsm, value);
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
                CalculateRollDiameter();
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

        [Appearance("FilmingWorkOrder.BellowsBellowsStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "BellowsStatus = 'None'")]
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

        [Appearance("FilmingWorkOrder.Cap", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "CapStatus = 'Uncapped'")]
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

        [ImmediatePostData]
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

        [Appearance("FilmingWorkOrder.CoronaDirection", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Corona = 'Doesnt'")]
        public CoronaDirection CoronaDirection
        {
            get
            {
                return _CoronaDirection;
            }
            set
            {
                SetPropertyValue("CoronaDirection", ref _CoronaDirection, value);
            }
        }

        [Appearance("FilmingWorkOrder.CoronaCoronaType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Corona != 'OneSidePartial' and Corona != 'DoubleSidePartial'")]
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
                CalculateGramM2();
                //CalculateGramMetretul();
            }
        }

        public Additive Additive
        {
            get
            {
                return _Additive;
            }
            set
            {
                SetPropertyValue("Additive", ref _Additive, value);
            }
        }

        public decimal AdditiveRate
        {
            get
            {
                return _AdditiveRate;
            }
            set
            {
                SetPropertyValue("AdditiveRate", ref _AdditiveRate, value);
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

        [Appearance("FilmingWorkOrder.ShippingPackageType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [ImmediatePostData]
        [RuleRequiredField]
        public ShippingFilmType ShippingFilmType
        {
            get
            {
                return _ShippingFilmType;
            }
            set
            {
                SetPropertyValue("ShippingFilmType", ref _ShippingFilmType, value);
                CalculateRollDiameter();
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

        [ImmediatePostData]
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
                CalculateRollDiameter();
            }
        }

        [Appearance("FilmingWorkOrder.Palette", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("FilmingWorkOrder.PaletteLayout", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("FilmingWorkOrder.PaletteNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        public decimal AvgMFI
        {
            get
            {
                return _AvgMFI;
            }
            set
            {
                SetPropertyValue("AvgMFI", ref _AvgMFI, value);
            }
        }

        public decimal AvgDensity
        {
            get
            {
                return _AvgDensity;
            }
            set
            {
                SetPropertyValue("AvgDensity", ref _AvgDensity, value);
            }
        }

        public decimal AvgSlipFactor
        {
            get
            {
                return _AvgSlipFactor;
            }
            set
            {
                SetPropertyValue("AvgSlipFactor", ref _AvgSlipFactor, value);
            }
        }

        [RuleRequiredField]
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

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
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

        #region Isıl Değerler
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

        public int CoolingLine
        {
            get
            {
                return _CoolingLine;
            }
            set
            {
                SetPropertyValue("CoolingLine", ref _CoolingLine, value);
            }
        }

        public int FilmingTension
        {
            get
            {
                return _FilmingTension;
            }
            set
            {
                SetPropertyValue("FilmingTension", ref _FilmingTension, value);
            }
        }

        public int WrapperTension
        {
            get
            {
                return _WrapperTension;
            }
            set
            {
                SetPropertyValue("WrapperTension", ref _WrapperTension, value);
            }
        }

        public int BubbleCooling
        {
            get
            {
                return _BubbleCooling;
            }
            set
            {
                SetPropertyValue("BubbleCooling", ref _BubbleCooling, value);
            }
        }

        public int IBCFeed
        {
            get
            {
                return _IBCFeed;
            }
            set
            {
                SetPropertyValue("IBCFeed", ref _IBCFeed, value);
            }
        }

        public int IBCExtruder
        {
            get
            {
                return _IBCExtruder;
            }
            set
            {
                SetPropertyValue("IBCExtruder", ref _IBCExtruder, value);
            }
        }

        public int TowerSpeed
        {
            get
            {
                return _TowerSpeed;
            }
            set
            {
                SetPropertyValue("TowerSpeed", ref _TowerSpeed, value);
            }
        }

        public int Neck
        {
            get
            {
                return _Neck;
            }
            set
            {
                SetPropertyValue("Neck", ref _Neck, value);
            }
        }

        public int Head1
        {
            get
            {
                return _Head1;
            }
            set
            {
                SetPropertyValue("Head1", ref _Head1, value);
            }
        }

        public int Head2
        {
            get
            {
                return _Head2;
            }
            set
            {
                SetPropertyValue("Head2", ref _Head2, value);
            }
        }

        public int Lip
        {
            get
            {
                return _Lip;
            }
            set
            {
                SetPropertyValue("Lip", ref _Lip, value);
            }
        }
        #endregion

        [Association, Aggregated]
        public XPCollection<FilmingWorkOrderPart> FilmingWorkOrderParts
        {
            get { return GetCollection<FilmingWorkOrderPart>("FilmingWorkOrderParts"); }
        }

        [Association, Aggregated]
        public XPCollection<FilmingWorkOrderReciept> FilmingWorkOrderReciepts
        {
            get { return GetCollection<FilmingWorkOrderReciept>("FilmingWorkOrderReciepts"); }
        }

        [Association, Aggregated]
        public XPCollection<FilmingWorkOrderExtruder> FilmingWorkOrderExtruders
        {
            get { return GetCollection<FilmingWorkOrderExtruder>("FilmingWorkOrderExtruders"); }
        }

        [Association("FilmingWorkOrder-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }
        }

        [Association("FilmingWorkOrder-Wastages")]
        public XPCollection<Wastage> Wastages
        {
            get
            {
                return GetCollection<Wastage>("Wastages");
            }
        }
        [Association("FilmingWorkOrder-FilmingQualityTests")]
        public XPCollection<FilmingQualityTest> FilmingQualityTests
        {
            get
            {
                return GetCollection<FilmingQualityTest>("FilmingQualityTests");
            }
        }

        #region functions
        void SetWidth()
        {
            if (IsLoading) return;
            if (FilmingFilmType != null)
            {
                if (FilmingFilmType.Name == "HORTUM")
                {
                    FilmingWidth = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
                    OpenedWidth = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
                    ClosedWidth = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
                }
                else if (FilmingFilmType.Name == "KÖRÜKLÜ HORTUM")
                {
                    FilmingWidth = SalesOrderDetail != null ? SalesOrderDetail.Product.Width : Width;
                    OpenedWidth = SalesOrderDetail != null ? SalesOrderDetail.Product.Width + (Convert.ToInt32(SalesOrderDetail.Product.Bellows) * 2) : Width + (Convert.ToInt32(Bellows) * 2);
                    ClosedWidth = SalesOrderDetail != null ? SalesOrderDetail.Product.Width - (Convert.ToInt32(SalesOrderDetail.Product.Bellows) * 2) : Width - (Convert.ToInt32(Bellows) * 2);
                }
                else if (FilmingFilmType.Name == "YAPRAK")
                {
                    if (SalesOrderDetail != null)
                    {
                        if (SalesOrderDetail.Product.PrintStatus == PrintStatus.Printed)
                        {
                            PrintingWorkOrder printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
                            FilmingWidth = printingWorkOrder != null ? printingWorkOrder.Width : 0;
                            OpenedWidth = printingWorkOrder != null ? printingWorkOrder.Width : 0;
                            ClosedWidth = printingWorkOrder != null ? printingWorkOrder.Width : 0;
                        }
                        else
                        {
                            SlicingWorkOrder slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
                            FilmingWidth = slicingWorkOrder != null ? slicingWorkOrder.Width : 0;
                            OpenedWidth = slicingWorkOrder != null ? slicingWorkOrder.Width : 0;
                            ClosedWidth = slicingWorkOrder != null ? slicingWorkOrder.Width : 0;
                        }
                    }
                }
            }
        }
        void SetMaximumRollWeight()
        {
            if (IsLoading) return;
            if (NextStation != null)
            {
                if (!NextStation.IsLastStation) MaximumRollWeight = 1000;
            }
        }
        void SetProductionOption()
        {
            if (IsLoading) return;
            if (NextStation != null)
            {
                if (SalesOrderDetail != null)
                {
                    if (!NextStation.IsLastStation) ProductionOption = SalesOrderDetail.SemiProductOption;
                    else ProductionOption = SalesOrderDetail.LastProductOption;
                }
            }
        }
        void CheckOption()
        {
            if (IsLoading) return;
            if (NextStation != null)
            {
                if (SalesOrderDetail != null)
                {
                    if (!NextStation.IsLastStation)
                    {
                        if(ProductionOption > SalesOrderDetail.SemiProductOption)
                        {
                            ProductionOption = SalesOrderDetail.SemiProductOption;
                            XtraMessageBox.Show("Siparişteki üretim opsiyonundan fazla değer giremezsiniz.");
                        }
                    }
                    else
                    {
                        if (ProductionOption > SalesOrderDetail.LastProductOption)
                        {
                            ProductionOption = SalesOrderDetail.LastProductOption;
                            XtraMessageBox.Show("Siparişteki üretim opsiyonundan fazla değer giremezsiniz.");
                        }
                    }
                }
            }
        }
        void GetSalesOrderDetail()
        {
            if (IsLoading) return;
            if (SalesOrderDetail != null)
            {
                var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail.Oid));
                if (filmingWorkOrder != null) XtraMessageBox.Show(string.Format("Bu müşteri siparişi için daha önce {0} numaralı üretim siparişi doldurulmuş. Mükerrer bir kayıt olup olmadığını kontrol ediniz...", filmingWorkOrder.WorkOrderNumber), "Bilgilendirme", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

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
                CoronaDirection = SalesOrderDetail.Product.CoronaDirection;
                CoronaPartial = SalesOrderDetail.Product.CoronaPartial;
                PrintName = SalesOrderDetail.Product.PrintName;
                Density = SalesOrderDetail.Product.Density;
                Additive = SalesOrderDetail.Product.Additive;
                AdditiveRate = SalesOrderDetail.Product.AdditiveRate;
                RollWeight = SalesOrderDetail.Product.RollWeight;
                ShippingPackageType = SalesOrderDetail.Product.ShippingPackageType;
                ShippingFilmType = SalesOrderDetail.Product.ShippingFilmType;
                MaterialColor = SalesOrderDetail.Product.MaterialColor;
                Embossing = SalesOrderDetail.Product.Embossing;
                ProductionNote = SalesOrderDetail.WorkOrderNote;
                QualityNote = SalesOrderDetail.QualityNote;
                Palette = SalesOrderDetail.Palette ?? null;

                if (NextStation != null)
                {
                    if (!NextStation.IsLastStation) ProductionOption = SalesOrderDetail.SemiProductOption;
                    else ProductionOption = SalesOrderDetail.LastProductOption;
                }
            }
        }
        void GetReciept()
        {
            if (IsLoading) return;
            if (Reciept != null)
            {
                XPCollection<FilmingWorkOrderReciept> filmingReciepts = FilmingWorkOrderReciepts;
                Session.Delete(filmingReciepts);

                foreach (var item in Reciept.RecieptDetails)
                {
                    var part = Session.FindObject<MachinePart>(CriteriaOperator.Parse("Machine = ? and Name = ?", Machine, item.MachinePartCode));
                    var filmingWorkOrderReciept = new FilmingWorkOrderReciept(Session)
                    {
                        FilmingWorkOrder = this,
                        Product = item.Product,
                        Warehouse = item.Warehouse,
                        MachinePart = part != null ? part : null,
                        WorkOrderRate = item.Rate,
                        Unit = item.Unit
                    };
                    this.FilmingWorkOrderReciepts.Add(filmingWorkOrderReciept);
                }

                FilmingWorkOrderParts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbPart = new StringBuilder();
                foreach (var item in FilmingWorkOrderParts)
                {
                    sbPart.AppendLine(string.Format("{0} : {1:n1}", item.MachinePart.Name, item.Thickness));
                }
                PartString = sbPart.ToString();

                FilmingWorkOrderReciepts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var item in FilmingWorkOrderParts)
                {
                    FilmingWorkOrderReciepts.Filter = new BinaryOperator("MachinePart.Name", item.MachinePart.Name);
                    foreach (var reciept in FilmingWorkOrderReciepts)
                    {
                        sbReciept.AppendLine(string.Format("{0} - %{1:n2} - {2}  /  ", reciept.MachinePart.Name, reciept.WorkOrderRate, reciept.Product.Name));
                    }
                }
                RecieptString = sbReciept.ToString();
            }
            FilmingWorkOrderReciepts.Filter = null;
        }
        void CalculateGramM2()
        {
            if (IsLoading) return;
            GramM2 = Thickness * Density;
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
        void CalculateInflationRate()
        {
            if (IsLoading) return;
            int en = 0;
            if (BellowsStatus == BellowsStatus.None) en = Width;
            else
            {
                decimal bellow = Convert.ToDecimal(Bellows);
                en = Convert.ToInt32(bellow) * 4;
                if (WayCount > 1) en = (en * WayCount) + 10;
            }
            decimal headDiameter = Machine != null ? Machine.HeadDiameter : 1;
            InflationRate = headDiameter == 0 ? 0 : en / headDiameter * 0.637m;
        }
        void CalculateRollDiameter()
        {
            if (IsLoading) return;
            decimal f = 0, result = 0;
            if (Bobbin != null)
            {
                if (Bobbin.BobbinInDiameter == 2) f = 6.1m;
                else if (Bobbin.BobbinInDiameter == 3) f = 10.3m;
                else f = 50m;
            }
            if (ShippingFilmType != null)
            {
                if (ShippingFilmType.Code != "YAP" & ShippingFilmType.Code != "VNT")
                {
                    result = (Lenght * Thickness * 2 * 1.37m) + f;
                }
                else
                {
                    result = (Lenght * Thickness * 1.37m) + f;
                }
            }
            else result = Lenght * Thickness * 2 * 1.37m;
            RollDiameter = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(result)) + Convert.ToDouble(f));
        }
        void CreateWorkOrderParts()
        {
            if (IsLoading) return;
            if (Machine != null)
            {
                if (Session.IsNewObject(this))
                {
                    Session.Delete(FilmingWorkOrderParts);
                    foreach (MachinePart part in Machine.MachineParts)
                    {
                        FilmingWorkOrderPart filmingWorkOrderPart = new FilmingWorkOrderPart(Session);
                        filmingWorkOrderPart.FilmingWorkOrder = this;
                        filmingWorkOrderPart.MachinePart = part;
                        if (Machine.MachineParts.Count == 1)
                        {
                            filmingWorkOrderPart.Thickness = Thickness;
                        }
                        FilmingWorkOrderParts.Add(filmingWorkOrderPart);
                    }
                }
            }
        }
        public void UpdateRecieptRate()
        {
            if (IsLoading) return;
            foreach (var item in FilmingWorkOrderParts)
            {
                if (Thickness > 0)
                {
                    FilmingWorkOrderReciepts.Filter = new BinaryOperator("MachinePart.Name", item.MachinePart.Name);
                    foreach (var reciept in FilmingWorkOrderReciepts)
                    {
                        if (item.MachinePart.Name == reciept.MachinePart.Name)
                        {
                            reciept.Rate = reciept.WorkOrderRate * (item.Thickness * 100 / Thickness) / 100;
                            reciept.Quantity = Quantity * reciept.Rate / 100;
                        }
                    }
                }
            }
            FilmingWorkOrderReciepts.Filter = null;
        }
        #endregion
    }
}