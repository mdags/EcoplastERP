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
    [Appearance("Eco6CuttingWorkOrder.LineDeliveryDateAppearance", TargetItems = "WorkName", Criteria = "SalesOrderDetail.LineDeliveryDate < LocalDateTimeToday()", BackColor = "MistyRose")]
    [Appearance("Eco6CuttingWorkOrder.TotalProductionQuantityAppearance", TargetItems = "TotalProduction", Criteria = "TotalProduction > Quantity", BackColor = "GreenYellow")]
    [Appearance("Eco6CuttingWorkOrder.TotalProductionQuantityAppearance1", TargetItems = "TotalProduction", Criteria = "TotalProduction <= Quantity", BackColor = "LemonChiffon")]
    [Appearance("Eco6CuttingWorkOrder.CompletedAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'ProductionComplete'", FontStyle = FontStyle.Strikeout, BackColor = "LemonChiffon")]
    [Appearance("Eco6CuttingWorkOrder.CanceledAppearance", TargetItems = "*", Criteria = "WorkOrderStatus = 'Canceled'", FontStyle = FontStyle.Strikeout, BackColor = "MistyRose")]
    [Appearance("Eco6CuttingWorkOrder.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "Eco6CuttingWorkOrder_ListView")]
    [Appearance("Eco6CuttingWorkOrder_MachineLoad.QuarantineQuantityAppearance", TargetItems = "*", Criteria = "QuarantineQuantity > 0", BackColor = "PowderBlue", Context = "Eco6CuttingWorkOrder_ListView_MachineLoad")]
    public class Eco6CuttingWorkOrder : BaseObject
    {
        public Eco6CuttingWorkOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            WorkOrderStatus = WorkOrderStatus.WaitingforProduction;
            SequenceNumber = 99;
            WorkOrderNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            WorkOrderDate = Helpers.GetSystemDate(Session);
            Station = Session.FindObject<Station>(new BinaryOperator("Name", "Eco6 Kesim"));
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

            //Eco6CuttingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            //var sbReciept = new StringBuilder();
            //foreach (var reciept in Eco6CuttingWorkOrderReciepts)
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
            //    if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforPrinting;
            //    else if (SalesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforProduction) SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.WaitingforEco6;
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
        private string _PaletteLayout;
        private Palette _Palette;
        private OuterPacking _OuterPacking;
        private ShippingFilmType _ShippingFilmType;
        private ShippingPackageType _ShippingPackageType;
        private Bobbin _Bobbin;
        private decimal _RollDiameter;
        private MaterialColor _MaterialColor;
        private decimal _MaximumPieceWeight;
        private decimal _MinimumPieceWeight;
        private decimal _PackageWeight;
        private decimal _GramM2;
        private decimal _M2;
        private decimal _GramMetretul;
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

        [Appearance("Eco6CuttingWorkOrder.WorkName", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Production), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("Eco6CuttingWorkOrder = ?", Oid)));
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
                return Convert.ToDecimal(Session.Evaluate(typeof(Wastage), CriteriaOperator.Parse("Sum(GrossQuantity)"), CriteriaOperator.Parse("Eco6CuttingWorkOrder = ?", Oid)));
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

        [Appearance("Eco6CuttingWorkOrder.PaletteStickerDesign", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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
                CalculateRollDiameter();
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
                CalculateRollDiameter();
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

        public decimal M2
        {
            get
            {
                return _M2;
            }
            set
            {
                SetPropertyValue("M2", ref _M2, value);
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

        [RuleValueComparison("Eco6CuttingWorkOrder.MaximumPieceWeight", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        [Appearance("Eco6CuttingWorkOrder.ShippingPackageType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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
        [Appearance("Eco6CuttingWorkOrder.ShippingFilmType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("Eco6CuttingWorkOrder.Palette", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
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

        [Appearance("Eco6CuttingWorkOrder.PaletteLayout", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "NextStation.Name != 'Son Üretim'")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        [Appearance("Eco6CuttingWorkOrder.PaletteNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [VisibleInDetailView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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
        public XPCollection<Eco6CuttingWorkOrderReciept> Eco6CuttingWorkOrderReciepts
        {
            get { return GetCollection<Eco6CuttingWorkOrderReciept>("Eco6CuttingWorkOrderReciepts"); }
        }

        [Association("Eco6CuttingWorkOrder-Productions")]
        public XPCollection<Production> Productions
        {
            get
            {
                return GetCollection<Production>("Productions");
            }
        }

        [Association("Eco6CuttingWorkOrder-Wastages")]
        public XPCollection<Wastage> Wastages
        {
            get
            {
                return GetCollection<Wastage>("Wastages");
            }
        }

        [Association("Eco6CuttingWorkOrder-ReadResources")]
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
                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail.Oid));
                if (eco6CuttingWorkOrder != null) XtraMessageBox.Show(string.Format("Bu müşteri siparişi için daha önce {0} numaralı üretim siparişi doldurulmuş. Mükerrer bir kayıt olup olmadığını kontrol ediniz...", eco6CuttingWorkOrder.WorkOrderNumber), "Bilgilendirme", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

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
                ShippingPackageType = SalesOrderDetail.Product.ShippingPackageType;
                ProductionNote = SalesOrderDetail.WorkOrderNote;
                QualityNote = SalesOrderDetail.QualityNote;
                Palette = SalesOrderDetail.Palette ?? null;

                Session.Delete(Eco6CuttingWorkOrderReciepts);
                Eco6CuttingWorkOrderReciept reciept = new Eco6CuttingWorkOrderReciept(Session)
                {
                    Eco6CuttingWorkOrder = this,
                    Product = Session.FindObject<Product>(new BinaryOperator("Oid", SalesOrderDetail.Product.Oid)),
                    Warehouse = Session.FindObject<Warehouse>(new BinaryOperator("Oid", Station.SourceWarehouse.Oid)),
                    Rate = 99,
                    Unit = Session.FindObject<Unit>(new BinaryOperator("Oid", SalesOrderDetail.Product.Unit.Oid))
                };
                Eco6CuttingWorkOrderReciepts.Add(reciept);
            }
        }

        void GetReciept()
        {
            if (IsLoading) return;
            if (Reciept != null)
            {
                Session.Delete(Eco6CuttingWorkOrderReciepts);
                foreach (var item in Reciept.RecieptDetails)
                {
                    var eco6CuttingWorkOrderReciept = new Eco6CuttingWorkOrderReciept(Session)
                    {
                        Eco6CuttingWorkOrder = this,
                        Product = item.Product,
                        Warehouse = item.Warehouse,
                        Rate = item.Rate,
                        Unit = item.Unit
                    };
                    this.Eco6CuttingWorkOrderReciepts.Add(eco6CuttingWorkOrderReciept);
                }

                Eco6CuttingWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in Eco6CuttingWorkOrderReciepts)
                {
                    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));
                }
                RecieptString = sbReciept.ToString();
            }
        }
        void CalculateGramMetretul()
        {
            if (IsLoading) return;
            //if (SalesOrderDetail != null)
            //{
            //    if (SalesOrderDetail.Product != null)
            //    {
            //        if (SalesOrderDetail.Product.ProductType != null)
            //        {
            //            int en = Width;
            //            if (SalesOrderDetail.Product.ProductType.Name.Contains("JELATİN"))
            //                GramMetretul = ((en + (SalesOrderDetail.Product.Cap / 2)) * Thickness * 2 * Density) / 1000;
            //            else
            //            {
            //                if (SalesOrderDetail.Product.CapStatus == CapStatus.Capped)
            //                    GramMetretul = ((Height + (SalesOrderDetail.Product.Cap / 2)) * (Thickness * 2) * Density) / 1000;
            //                else
            //                {
            //                    if (SalesOrderDetail.Product.ShippingFilmType != null)
            //                    {
            //                        if (SalesOrderDetail.Product.ShippingFilmType.Code != "YAP")
            //                            GramMetretul = (en * (Thickness * 2) * Density) / 1000;
            //                        else GramMetretul = (en * Thickness * Density) / 1000;
            //                    }
            //                    else GramMetretul = (en * (Thickness * 2) * Density) / 1000;
            //                }
            //            }

            //            if (Bobbin != null) RollWeight = (GramMetretul / 1000 * Lenght) + ((((decimal)Width / 10) + 1) * (decimal)Bobbin.Weight / 100000);
            //        }
            //    }
            //}
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
        #endregion
    }
}
