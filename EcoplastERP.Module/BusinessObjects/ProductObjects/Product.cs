using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Product")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class Product : BaseObject
    {
        public Product(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TaxRate = 18;
            Unit = Session.FindObject<Unit>(new BinaryOperator("Default", true));
            Logo = false;
            TKBPermission = false;
            PrintStatus = PrintStatus.Unprinted;
            HangingLocation = HangingLocation.Doesnt;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (ProductGroup.Code == "SM")
            {
                if (!Helpers.IsUserInRole("Standart Ürün Yöneticisi"))
                {
                    ProductGroup = null;
                    throw new UserFriendlyException("Standart ürün kartý tanýmý veya düzenlemesi için yetkiniz yok.");
                }
            }

            if (ProductGroup != null)
            {
                if (ProductGroup.Name.Contains("MAMUL"))
                {
                    string bellowCode = string.Empty, bellowName = string.Empty;
                    if (BellowsStatus == BellowsStatus.AboveBellow) bellowCode = ".ÜK";
                    if (BellowsStatus == BellowsStatus.SideBellow) bellowCode = ".YK";
                    if (BellowsStatus == BellowsStatus.UnderBellow) bellowCode = ".AK";
                    if (BellowsStatus != BellowsStatus.None) bellowName = Bellows;

                    string corona = string.Empty;
                    if (Corona == Corona.DubleSideFull) corona = ".ÇTF";
                    if (Corona == Corona.DoubleSidePartial) corona = ".ÇTP";
                    if (Corona == Corona.OneSideFull) corona = ".TTF";
                    if (Corona == Corona.OneSidePartial) corona = ".TTP";
                    decimal bellow = 0;
                    if (!string.IsNullOrEmpty(Bellows)) { bellow = Convert.ToDecimal(Bellows) / 10; }
                    Code = string.Format("{0}{1}{2}{3}{4}X{5}{6}{7}{8}{9}{10}{11}{12}{13}",
                        ProductGroup != null ? ProductGroup.Code + "." : string.Empty,
                        ProductKind != null ? ProductKind.Code + "." : string.Empty,
                        MaterialType != null ? MaterialType.Code + "." : string.Empty,
                        ProductType != null ? ProductType.Code + "." : string.Empty,
                        Width,
                        Height,
                        Thickness != 0 ? string.Format(".{0:n1}", Thickness) : string.Empty,
                        Lenght != 0 ? string.Format(".{0}", Lenght.ToString()) : string.Empty,
                        bellowCode != string.Empty ? string.Format(".{0}", bellowCode) : string.Empty,
                        bellow != 0 ? string.Format("({0}).", bellow) : "",
                        MaterialColor != null ? MaterialColor.Code + "." : string.Empty,
                        PrintName != null ? PrintName.Code + "." : string.Empty,
                        ShippingFilmType != null ? ShippingFilmType.Code + "." : string.Empty,
                        corona);

                    if (Session.IsNewObject(this))
                    {
                        XPCollection<ConversionUnit> cUnitList = new XPCollection<ConversionUnit>(Session);
                        ConversionUnits.Filter = new BinaryOperator("cUnit.Code", "KG");
                        if (ConversionUnits.Count == 0)
                        {
                            ConversionUnit newKGCUnit = new ConversionUnit(Session)
                            {
                                Product = this,
                                BaseQuantity = 1,
                                pUnit = Session.FindObject<Unit>(new BinaryOperator("Code", "KG")),
                                cQuantity = 1,
                                cUnit = Session.FindObject<Unit>(new BinaryOperator("Code", "KG")),
                            };
                            cUnitList.Add(newKGCUnit);
                            ConversionUnits.Add(newKGCUnit);
                        }

                        if (RollWeight > 0)
                        {
                            ConversionUnits.Filter = new BinaryOperator("cUnit.Code", "RL");
                            if (ConversionUnits.Count == 0)
                            {
                                ConversionUnit newRLCUnit = new ConversionUnit(Session)
                                {
                                    Product = this,
                                    BaseQuantity = RollWeight,
                                    pUnit = Session.FindObject<Unit>(new BinaryOperator("Code", "KG")),
                                    cQuantity = 1,
                                    cUnit = Session.FindObject<Unit>(new BinaryOperator("Code", "RL")),
                                };
                                cUnitList.Add(newRLCUnit);
                                ConversionUnits.Add(newRLCUnit);
                            }
                        }

                        Session.Save(cUnitList);
                    }
                }
            }
        }
        // Fields...
        private bool _TKBPermission;
        private bool _Logo;
        private string _BussinesRegistrationNumber;
        private string _Barcode;
        private PrintDirection _PrintDirection;
        private PrintRate _PrintRate;
        private DateTime _ExpirationDate;
        private RollDirection _RollDirection;
        private PrintForm _PrintForm;
        private string _PrintingColors;
        private PrintName _PrintName;
        private int _PalettePackageCount;
        private string _OuterPackingBarcode;
        private OuterPacking _OuterPacking;
        private string _InnerPackingBarcode;
        private InnerPacking _InnerPacking;
        private decimal _PackageWeight;
        private int _OuterPackingInPiece;
        private int _InnerPackingInPiece;
        private int _OuterPackingRollCount;
        private decimal _NetKg;
        private decimal _RollDiameter;
        private decimal _RollWeight;
        private BobbinType _BobbinType;
        private ShippingPackageType _ShippingPackageType;
        private ProductionPackageType _ProductionPackageType;
        private decimal _PieceWeight;
        private int _BladeDeep;
        private int _BladeWidth;
        private MaterialColor _MaterialColor;
        private bool _Embossing;
        private int _Block;
        private PunchType _PunchType;
        private PunchStatus _PunchStatus;
        private ShippingFilmType _ShippingFilmType;
        private decimal _FilmCode2Thickness;
        private Reciept _FilmCode2;
        private decimal _FilmCode1Thickness;
        private Reciept _FilmCode1;
        private Laminated _Laminated;
        private HandsOnType _HandsOnType;
        private HandleType _HandleType;
        private HandleWeld _HandleWeld;
        private AdhesionType _AdhesionType;
        private string _CoronaPartial;
        private CoronaDirection _CoronaDirection;
        private Corona _Corona;
        private HangingLocation _HangingLocation;
        private BandStatus _BandStatus;
        private PerforationStatus _PerforationStatus;
        private decimal _Cap;
        private CapStatus _CapStatus;
        private string _Bellows;
        private BellowsStatus _BellowsStatus;
        private decimal _AdditiveRate;
        private Additive _Additive;
        private decimal _Density;
        private int _Lenght;
        private decimal _Thickness;
        private int _Height;
        private int _Width;
        private PrintStatus _PrintStatus;
        private MaterialType _MaterialType;
        private decimal _SlipFactor;
        private decimal _RawMaterialDensity;
        private decimal _MFI;
        private string _Model;
        private Trademark _Trademark;
        private string _CatalogNumber;
        private CriticalCode _CriticalCode;
        private Contact _ShippingContact;
        private Contact _Contact;
        private CapacityGroup _CapacityGroup;
        private ProductGroup2 _ProductGroup2;
        private ProductGroup1 _ProductGroup1;
        private string _SAPCode;
        private string _EanCode;
        private decimal _TaxRate;
        private Warehouse _Warehouse;
        private ProductStatus _ProductStatus;
        private Unit _Unit;
        private string _Name;
        private string _Code;
        private ProductKind _ProductKind;
        private ProductType _ProductType;
        private ProductGroup _ProductGroup;

        [ImmediatePostData]
        [RuleRequiredField]
        public ProductGroup ProductGroup
        {
            get
            {
                return _ProductGroup;
            }
            set
            {
                SetPropertyValue("ProductGroup", ref _ProductGroup, value);
                GetUnit();
                UpdateCapStatus();
                UpdatePrintStatus();
                UpdateHangingLocation();
                UpdateBandStatus();
                UpdateShippingFilmType();
                CheckPermission();
            }
        }

        [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public ProductType ProductType
        {
            get
            {
                return _ProductType;
            }
            set
            {
                SetPropertyValue("ProductType", ref _ProductType, value);
            }
        }

        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public ProductKind ProductKind
        {
            get
            {
                return _ProductKind;
            }
            set
            {
                SetPropertyValue("ProductKind", ref _ProductKind, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue(TargetCriteria = "IsNewObject(this)")]
        [Appearance("Product.Code", Context = "DetailView", Enabled = false, Criteria = "ProductGroup.Name like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name not like '%MAMUL%'")]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
        [RuleUniqueValue]
        [Size(200)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        [Appearance("Product.Unit", Context = "DetailView", Enabled = false, Criteria = "ProductGroup.Name like '%MAMUL%'")]
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

        public ProductStatus ProductStatus
        {
            get
            {
                return _ProductStatus;
            }
            set
            {
                SetPropertyValue("ProductStatus", ref _ProductStatus, value);
            }
        }

        [Appearance("Product.Warehouse", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name not like '%MAMUL%'")]
        public Warehouse Warehouse
        {
            get
            {
                return _Warehouse;
            }
            set
            {
                SetPropertyValue("Warehouse", ref _Warehouse, value);
            }
        }

        public decimal TaxRate
        {
            get
            {
                return _TaxRate;
            }
            set
            {
                SetPropertyValue("TaxRate", ref _TaxRate, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EanCode
        {
            get
            {
                return _EanCode;
            }
            set
            {
                SetPropertyValue("EanCode", ref _EanCode, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SAPCode
        {
            get
            {
                return _SAPCode;
            }
            set
            {
                SetPropertyValue("SAPCode", ref _SAPCode, value);
            }
        }

        [Appearance("Product.ProductGroup1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public ProductGroup1 ProductGroup1
        {
            get
            {
                return _ProductGroup1;
            }
            set
            {
                SetPropertyValue("ProductGroup1", ref _ProductGroup1, value);
            }
        }

        [Appearance("Product.ProductGroup2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public ProductGroup2 ProductGroup2
        {
            get
            {
                return _ProductGroup2;
            }
            set
            {
                SetPropertyValue("ProductGroup2", ref _ProductGroup2, value);
            }
        }

        public CapacityGroup CapacityGroup
        {
            get
            {
                return _CapacityGroup;
            }
            set
            {
                SetPropertyValue("CapacityGroup", ref _CapacityGroup, value);
            }
        }

        [Appearance("Product.Contact", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code != 'OM'")]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
            }
        }

        [Appearance("Product.ShippingContact", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code != 'OM'")]
        public Contact ShippingContact
        {
            get
            {
                return _ShippingContact;
            }
            set
            {
                SetPropertyValue("ShippingContact", ref _ShippingContact, value);
            }
        }

        [Appearance("Product.CriticalCode", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public CriticalCode CriticalCode
        {
            get
            {
                return _CriticalCode;
            }
            set
            {
                SetPropertyValue("CriticalCode", ref _CriticalCode, value);
            }
        }

        [Appearance("Product.CatalogNumber", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CatalogNumber
        {
            get
            {
                return _CatalogNumber;
            }
            set
            {
                SetPropertyValue("CatalogNumber", ref _CatalogNumber, value);
            }
        }

        [Appearance("Product.Trademark", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public Trademark Trademark
        {
            get
            {
                return _Trademark;
            }
            set
            {
                SetPropertyValue("Trademark", ref _Trademark, value);
            }
        }

        [Appearance("Product.Model", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Model
        {
            get
            {
                return _Model;
            }
            set
            {
                SetPropertyValue("Model", ref _Model, value);
            }
        }

        [Appearance("Product.MFI", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public decimal MFI
        {
            get
            {
                return _MFI;
            }
            set
            {
                SetPropertyValue("MFI", ref _MFI, value);
            }
        }

        [Appearance("Product.RawMaterialDensity", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public decimal RawMaterialDensity
        {
            get
            {
                return _RawMaterialDensity;
            }
            set
            {
                SetPropertyValue("RawMaterialDensity", ref _RawMaterialDensity, value);
            }
        }

        [Appearance("Product.SlipFactor", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'OM' or ProductGroup.Code = 'SM'")]
        public decimal SlipFactor
        {
            get
            {
                return _SlipFactor;
            }
            set
            {
                SetPropertyValue("SlipFactor", ref _SlipFactor, value);
            }
        }

        [ImmediatePostData]
        [Appearance("Product.MaterialType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public MaterialType MaterialType
        {
            get
            {
                return _MaterialType;
            }
            set
            {
                SetPropertyValue("MaterialType", ref _MaterialType, value);
                GetMaterialType();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.PrintStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public PrintStatus PrintStatus
        {
            get
            {
                return _PrintStatus;
            }
            set
            {
                SetPropertyValue("PrintStatus", ref _PrintStatus, value);
            }
        }

        [ImmediatePostData]
        [Appearance("Product.FilmCode1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public Reciept FilmCode1
        {
            get
            {
                return _FilmCode1;
            }
            set
            {
                SetPropertyValue("FilmCode1", ref _FilmCode1, value);
                GetDensity();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.FilmCode1Thickness", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.FilmCode1Thickness1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "FilmCode1 is null")]
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "FilmCode1 is not null")]
        public decimal FilmCode1Thickness
        {
            get
            {
                return _FilmCode1Thickness;
            }
            set
            {
                SetPropertyValue("FilmCode1Thickness", ref _FilmCode1Thickness, value);
            }
        }

        [ImmediatePostData]
        [Appearance("Product.FilmCode2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public Reciept FilmCode2
        {
            get
            {
                return _FilmCode2;
            }
            set
            {
                SetPropertyValue("FilmCode2", ref _FilmCode2, value);
                GetDensity();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.FilmCode2Thickness", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.FilmCode1Thickness2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "FilmCode2 is null")]
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "FilmCode2 is not null")]
        public decimal FilmCode2Thickness
        {
            get
            {
                return _FilmCode2Thickness;
            }
            set
            {
                SetPropertyValue("FilmCode2Thickness", ref _FilmCode2Thickness, value);
            }
        }

        [NonCloneable]
        [ImmediatePostData]
        [Appearance("Product.Width", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                SetPropertyValue("Width", ref _Width, value);
                CalculatePieceWeight();
            }
        }

        [NonCloneable]
        [ImmediatePostData]
        [Appearance("Product.Height", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                SetPropertyValue("Height", ref _Height, value);
                CalculatePieceWeight();
            }
        }

        [NonCloneable]
        [ImmediatePostData]
        [Appearance("Product.Thickness", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
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

        [NonCloneable]
        [Appearance("Product.Lenght", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [ImmediatePostData]
        [Appearance("Product.Density", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public decimal Density
        {
            get
            {
                return _Density;
            }
            set
            {
                SetPropertyValue("Density", ref _Density, value);
                CalculatePieceWeight();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.Additive", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.AdditiveRate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.AdditiveRate2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Additive is null")]
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

        [NonCloneable]
        [ImmediatePostData]
        [Appearance("Product.BellowsStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public BellowsStatus BellowsStatus
        {
            get
            {
                return _BellowsStatus;
            }
            set
            {
                SetPropertyValue("BellowsStatus", ref _BellowsStatus, value);
                CalculatePieceWeight();
            }
        }

        [NonCloneable]
        [Appearance("Product.Bellows", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.BellowsBellowsStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "BellowsStatus = 'None'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%' and BellowsStatus != 'None'")]
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
        [Appearance("Product.CapStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public CapStatus CapStatus
        {
            get
            {
                return _CapStatus;
            }
            set
            {
                SetPropertyValue("CapStatus", ref _CapStatus, value);
                UpdateCap();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.Cap", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.CapCapStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "CapStatus = 'Uncapped'")]
        public decimal Cap
        {
            get
            {
                return _Cap;
            }
            set
            {
                SetPropertyValue("Cap", ref _Cap, value);
                CalculatePieceWeight();
            }
        }

        [Appearance("Product.PerforationStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.BandStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.BandStatusCapStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "CapStatus = 'Uncapped'")]
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

        [ImmediatePostData]
        [Appearance("Product.HangingLocation", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public HangingLocation HangingLocation
        {
            get
            {
                return _HangingLocation;
            }
            set
            {
                SetPropertyValue("HangingLocation", ref _HangingLocation, value);
                CalculatePieceWeight();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.Corona", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.CoronaDirection", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Corona = 'Doesnt'")]
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

        [Appearance("Product.CoronaType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.AdhesionType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public AdhesionType AdhesionType
        {
            get
            {
                return _AdhesionType;
            }
            set
            {
                SetPropertyValue("AdhesionType", ref _AdhesionType, value);
            }
        }

        [Appearance("Product.HandleWeld", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.HandleType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public HandleType HandleType
        {
            get
            {
                return _HandleType;
            }
            set
            {
                SetPropertyValue("HandleType", ref _HandleType, value);
            }
        }

        [Appearance("Product.HandsOnType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public HandsOnType HandsOnType
        {
            get
            {
                return _HandsOnType;
            }
            set
            {
                SetPropertyValue("HandsOnType", ref _HandsOnType, value);
            }
        }

        [Appearance("Product.Laminated", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public Laminated Laminated
        {
            get
            {
                return _Laminated;
            }
            set
            {
                SetPropertyValue("Laminated", ref _Laminated, value);
            }
        }

        [ImmediatePostData]
        [Appearance("Product.ShippingFilmType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
        public ShippingFilmType ShippingFilmType
        {
            get
            {
                return _ShippingFilmType;
            }
            set
            {
                SetPropertyValue("ShippingFilmType", ref _ShippingFilmType, value);
                CalculatePieceWeight();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.PunchStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public PunchStatus PunchStatus
        {
            get
            {
                return _PunchStatus;
            }
            set
            {
                SetPropertyValue("PunchStatus", ref _PunchStatus, value);
            }
        }

        [Appearance("Product.PunchType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PunchTypePunchStatus", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PunchStatus = 'Impunched'")]
        public PunchType PunchType
        {
            get
            {
                return _PunchType;
            }
            set
            {
                SetPropertyValue("PunchType", ref _PunchType, value);
            }
        }

        [Appearance("Product.Block", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.Embossing", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [NonCloneable]
        [Appearance("Product.MaterialColor", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
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

        [ImmediatePostData]
        [Appearance("Product.BladeWidth", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.ProductGroupBladeWidth", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductKind.Name not like '%ATLET POÞET%'")]
        public int BladeWidth
        {
            get
            {
                return _BladeWidth;
            }
            set
            {
                SetPropertyValue("BladeWidth", ref _BladeWidth, value);
                CalculatePieceWeight();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.BladeDeep", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public int BladeDeep
        {
            get
            {
                return _BladeDeep;
            }
            set
            {
                SetPropertyValue("BladeDeep", ref _BladeDeep, value);
                CalculatePieceWeight();
            }
        }

        [Appearance("Product.PieceWeight", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [ImmediatePostData]
        [Appearance("Product.ProductionPackageType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public ProductionPackageType ProductionPackageType
        {
            get
            {
                return _ProductionPackageType;
            }
            set
            {
                SetPropertyValue("ProductionPackageType", ref _ProductionPackageType, value);
            }
        }

        [Appearance("Product.ShippingPackageType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%'")]
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
        [Appearance("Product.BobbinType", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.BobbinType1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'KOLÝ ÝÇÝ KUTU AMBALAJ' Or ProductionPackageType.Name = 'PAKETLÝ AMBALAJ' Or ProductionPackageType.Name = 'KOLÝLÝ AMBALAJ' Or ProductionPackageType.Name = 'PAKET ÝÇÝ DESTE AMBALAJ' Or ProductionPackageType.Name = 'KOLÝ ÝÇÝ DESTE AMBALAJ'")]
        public BobbinType BobbinType
        {
            get
            {
                return _BobbinType;
            }
            set
            {
                SetPropertyValue("BobbinType", ref _BobbinType, value);
                UpdateNetKg();
            }
        }

        [Appearance("Product.RollWeight", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.RollWeight1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'KOLÝ ÝÇÝ DESTE AMBALAJ' or ProductionPackageType.Name = 'PAKETLÝ AMBALAJ'")]
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

        [Appearance("Product.RollDiameter", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.RollDiameter1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'KOLÝ ÝÇÝ DESTE AMBALAJ' or ProductionPackageType.Name = 'PAKETLÝ AMBALAJ'")]
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

        [Appearance("Product.NetKg", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.NetKg1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "BobbinType is not null")]
        public decimal NetKg
        {
            get
            {
                return _NetKg;
            }
            set
            {
                SetPropertyValue("NetKg", ref _NetKg, value);
            }
        }

        [ImmediatePostData]
        [Appearance("Product.OuterPackingRollCount", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.OuterPackingRollCount1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'KOLÝLÝ AMBALAJ' or ProductionPackageType.Name = 'PAKETLÝ AMBALAJ'")]
        public int OuterPackingRollCount
        {
            get
            {
                return _OuterPackingRollCount;
            }
            set
            {
                SetPropertyValue("OuterPackingRollCount", ref _OuterPackingRollCount, value);
                CalculateOuterPackingInPiece();
            }
        }

        [ImmediatePostData]
        [Appearance("Product.InnerPackingInPiece", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.InnerPackingInPiece1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'KOLÝLÝ AMBALAJ' or ProductionPackageType.Name = 'PAKETLÝ AMBALAJ' or ProductionPackageType.Name = 'RULOLU AMBALAJ'")]
        public int InnerPackingInPiece
        {
            get
            {
                return _InnerPackingInPiece;
            }
            set
            {
                SetPropertyValue("InnerPackingInPiece", ref _InnerPackingInPiece, value);
                CalculateOuterPackingInPiece();
            }
        }

        [Appearance("Product.OuterPackingInPiece", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.OuterPackingInPiece1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'RULOLU AMBALAJ'")]
        public int OuterPackingInPiece
        {
            get
            {
                return _OuterPackingInPiece;
            }
            set
            {
                SetPropertyValue("OuterPackingInPiece", ref _OuterPackingInPiece, value);
            }
        }

        [Appearance("Product.PackageWeight", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PackageWeight1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionPackageType.Name = 'PAKETLÝ AMBALAJ' or ProductionPackageType.Name = 'RULOLU AMBALAJ'")]
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

        [Appearance("Product.InnerPacking", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
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

        [Appearance("Product.InnerPackingBarcode", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.InnerPackingBarcode1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "InnerPacking is null")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InnerPackingBarcode
        {
            get
            {
                return _InnerPackingBarcode;
            }
            set
            {
                SetPropertyValue("InnerPackingBarcode", ref _InnerPackingBarcode, value);
            }
        }

        [Appearance("Product.OuterPacking", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductKind.Name like '%STRETCH%'")]
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

        [Appearance("Product.OuterPackingBarcode", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.OuterPackingBarcode1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "OuterPacking is null")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OuterPackingBarcode
        {
            get
            {
                return _OuterPackingBarcode;
            }
            set
            {
                SetPropertyValue("OuterPackingBarcode", ref _OuterPackingBarcode, value);
            }
        }

        [Appearance("Product.PalettePackageCount", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        public int PalettePackageCount
        {
            get
            {
                return _PalettePackageCount;
            }
            set
            {
                SetPropertyValue("PalettePackageCount", ref _PalettePackageCount, value);
            }
        }

        [NonCloneable]
        [Appearance("Product.PrintName", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PrintName2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%' and PrintStatus = 'Printed'")]
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

        [Appearance("Product.PrintingColors", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PrintingColors2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%' and PrintStatus = 'Printed'")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        [Appearance("Product.PrintForm", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PrintForm2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
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

        [Appearance("Product.RollDirection", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.RollDirection2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%' and PrintStatus = 'Printed'")]
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

        [Appearance("Product.ExpirationDate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.ExpirationDate2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        public DateTime ExpirationDate
        {
            get
            {
                return _ExpirationDate;
            }
            set
            {
                SetPropertyValue("ExpirationDate", ref _ExpirationDate, value);
            }
        }

        [Appearance("Product.PrintRate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PrintRate2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        public PrintRate PrintRate
        {
            get
            {
                return _PrintRate;
            }
            set
            {
                SetPropertyValue("PrintRate", ref _PrintRate, value);
            }
        }

        [Appearance("Product.PrintDirection", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.PrintDirection2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductGroup.Name like '%MAMUL%' and PrintStatus = 'Printed'")]
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

        [Appearance("Product.Barcode", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.Barcode2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        [Appearance("Product.BussinesRegistrationNumber", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.BussinesRegistrationNumber2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BussinesRegistrationNumber
        {
            get
            {
                return _BussinesRegistrationNumber;
            }
            set
            {
                SetPropertyValue("BussinesRegistrationNumber", ref _BussinesRegistrationNumber, value);
            }
        }

        [Appearance("Product.Logo", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.Logo2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
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

        [Appearance("Product.TKBPermission", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Name not like '%MAMUL%'")]
        [Appearance("Product.TKBPermission2", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "PrintStatus = 'Unprinted'")]
        public bool TKBPermission
        {
            get
            {
                return _TKBPermission;
            }
            set
            {
                SetPropertyValue("TKBPermission", ref _TKBPermission, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<ConversionUnit> ConversionUnits
        {
            get { return GetCollection<ConversionUnit>("ConversionUnits"); }
        }

        [Association, Aggregated]
        public XPCollection<MinimumQuantity> MinimumQuantities
        {
            get { return GetCollection<MinimumQuantity>("MinimumQuantities"); }
        }

        [Association, Aggregated]
        public XPCollection<ProductPortfolio> ProductPortfolios
        {
            get { return GetCollection<ProductPortfolio>("ProductPortfolios"); }
        }

        [Appearance("Product.OfferDetails", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'MM'")]
        [Association("Product-OfferDetails")]
        public XPCollection<OfferDetail> OfferDetails
        {
            get
            {
                return GetCollection<OfferDetail>("OfferDetails");
            }
        }

        [Appearance("Product.PurchaseOrderDetails", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'MM'")]
        [Association("Product-PurchaseOrderDetails")]
        public XPCollection<PurchaseOrderDetail> PurchaseOrderDetails
        {
            get
            {
                return GetCollection<PurchaseOrderDetail>("PurchaseOrderDetails");
            }
        }

        [Appearance("Product.PurchaseWaybillDetails", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductGroup.Code = 'MM'")]
        [Association("Product-PurchaseWaybillDetails")]
        public XPCollection<PurchaseWaybillDetail> PurchaseWaybillDetails
        {
            get
            {
                return GetCollection<PurchaseWaybillDetail>("PurchaseWaybillDetails");
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

        #region functions
        void CheckPermission()
        {
            if (IsLoading) return;
            if (ProductGroup != null)
            {
                if (ProductGroup.Code == "SM")
                {
                    if (!Helpers.IsUserInRole("Standart Ürün Yöneticisi"))
                    {
                        ProductGroup = null;
                        throw new UserFriendlyException("Standart ürün kartý tanýmý veya düzenlemesi için yetkiniz yok.");
                    }
                }
            }
        }
        void GetUnit()
        {
            if (IsLoading) return;
            if (ProductGroup != null)
            {
                if (ProductGroup.Name.Contains("MAMUL"))
                {
                    Unit = Session.FindObject<Unit>(new BinaryOperator("Code", "KG"));
                }
            }
        }
        void GetMaterialType()
        {
            if (IsLoading) return;
            if (MaterialType != null) Density = MaterialType.Density;
        }
        void CalculateOuterPackingInPiece()
        {
            if (IsLoading) return;
            OuterPackingInPiece = OuterPackingRollCount * InnerPackingInPiece;
        }
        void UpdateCapStatus()
        {
            if (IsLoading) return;
            if (ProductKind != null)
            {
                if (ProductKind.Name.Contains("ATLET POÞET")) CapStatus = CapStatus.Uncapped;
                else if (ProductKind.Name.Contains("RULO")) CapStatus = CapStatus.Uncapped;
                else if (ProductKind.Name.Contains("STRETCH"))
                {
                    CapStatus = CapStatus.Uncapped;
                    PerforationStatus = PerforationStatus.Imperforate;
                }
            }
        }
        void UpdateCap()
        {
            if (IsLoading) return;
            if (CapStatus == CapStatus.Capped) Cap = 0;
        }
        void UpdateHangingLocation()
        {
            if (IsLoading) return;
            if (ProductKind != null)
            {
                if (ProductKind.Name.Contains("TORBA")) HangingLocation = HangingLocation.Doesnt;
            }
        }
        void UpdateBandStatus()
        {
            if (IsLoading) return;
            if (ProductKind != null)
            {
                if (ProductKind.Name.Contains("TORBA")) BandStatus = BandStatus.Tapeless;
            }
        }
        void UpdatePrintStatus()
        {
            if (IsLoading) return;
            if (ProductKind != null)
            {
                if (ProductKind.Name.Contains("STRETCH")) PrintStatus = PrintStatus.Unprinted;
            }
        }
        void UpdateShippingFilmType()
        {
            if (IsLoading) return;
            if (ProductKind != null)
            {
                if (ProductKind.Name.Contains("STRETCH")) ShippingFilmType = Session.FindObject<ShippingFilmType>(new BinaryOperator("Code", "YAP"));
            }
        }
        void UpdateNetKg()
        {
            if (IsLoading) return;
            if (BobbinType != null) NetKg = 0;
        }
        void CalculatePieceWeight()
        {
            if (IsLoading) return;
            if (ProductKind != null)
            {
                if (ShippingFilmType != null)
                {
                    decimal width = 0;
                    if (BellowsStatus == BellowsStatus.None) width = Width;
                    else
                    {
                        decimal bellow = Convert.ToDecimal(Bellows);
                        if (ShippingFilmType.Name == "ATLET POÞET" || ShippingFilmType.Name == "ÇÖP TORBASI" || ShippingFilmType.Name == "KÖRÜKLÜ HORTUM" || ShippingFilmType.Name == "RULO ATLET POÞET" || ShippingFilmType.Name == "RULO ÇÖP")
                        {
                            width = Width + bellow + bellow;
                        }
                        else
                        {
                            width = Width;
                        }
                    }
                    decimal hangingPlaceValue = HangingLocation == HangingLocation.Does ? 0.01M : 0;
                    int thicknessPart = ShippingFilmType.ThicknessDoublePart ? 2 : 1;

                    if (ShippingFilmType.Name == "KARGO TORBASI" || ShippingFilmType.Name == "WÝCKET")
                    {
                        decimal bellow = Convert.ToDecimal(Bellows);
                        PieceWeight = (((Width * (Height + Height + bellow + bellow + Cap) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                    }
                    else if (ShippingFilmType.Name == "TAKVÝYELÝ POÞET")
                    {
                        decimal bellow = Convert.ToDecimal(Bellows);
                        //PieceWeight = (((Width * (Height + Height + bellow + bellow) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                        PieceWeight = (((Width * (Height + bellow) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                    }
                    else if (ShippingFilmType.Name == "YUMUÞAK KULP")
                    {
                        decimal bellow = Convert.ToDecimal(Bellows);
                        PieceWeight = (((Width * (Height + Height + bellow + bellow + Cap + Cap) * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                    }
                    else if (ShippingFilmType.Name == "KÖRÜKLÜ TEK TARAFI AÇIK ")
                    {
                        decimal bellow = Convert.ToDecimal(Bellows);
                        PieceWeight = ((((Width + bellow) * thicknessPart) * Height * Thickness * Density) / 1000000) + hangingPlaceValue;
                    }
                    else
                    {
                        PieceWeight = (((width * Height * Thickness * Density) * thicknessPart) / 1000000) + hangingPlaceValue;
                    }
                }
            }
        }
        void GetDensity()
        {
            if (IsLoading) return;
            if (FilmCode1 != null)
            {
                if (FilmCode2 == null)
                {
                    Density = FilmCode1.Density;
                }
                else
                {
                    Density = (FilmCode1.Density + FilmCode2.Density) / 2;
                }
            }
        }
        #endregion
    }
}
