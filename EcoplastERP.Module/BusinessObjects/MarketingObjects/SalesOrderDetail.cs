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
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.ParametersObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Order_Item")]
    [DefaultProperty("SalesOrderDetailAlias")]
    [NavigationItem("MarketingManagement")]
    [Appearance("SalesOrderDetail.WaitingWorkOrder", TargetItems = "SalesOrderStatus, SalesOrder.OrderNumber, LineNumber", Criteria = "WaitingWorkOrder = true", BackColor = "Tomato")]
    public class SalesOrderDetail : BaseObject
    {
        public SalesOrderDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Currency = Session.FindObject<Currency>(CriteriaOperator.Parse("IsDefault = true"));
            SalesOrderStatus = SalesOrderStatus.WaitingforPlanningConfirm;
            LineDeliveryDate = Helpers.GetSystemDate(Session).AddDays(2);
            UnitMultiplier = 1;
            Parity = 1;
            WaitingWorkOrder = true;
            ShippedOptionPlus = 10;
            ShippedOptionMinus = 10;
            SemiProductOption = 20;
            LastProductOption = 15;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.Connection != null)
            {
                //    decimal oldPrice = Convert.ToDecimal(Session.ExecuteScalar("select Price from SalesOrderDetail where Oid = @oid", new string[] { "@oid" }, new object[] { this.Oid }));
                //    if (oldPrice != Price)
                //    {
                //        //Fiyat deðiþirse mail gönderecek
                //        DevExpress.XtraEditors.XtraMessageBox.Show(string.Format("Eski fiyat {0}", oldPrice));
                //    }
            }
            if (!Session.IsObjectMarkedDeleted(this))
            {
                if (Unit != null)
                {
                    if (Unit != Product.Unit)
                    {
                        if (Unit.Code != "NKG")
                        {
                            var conversionUnit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("Product = ? and cUnit = ?", Product, Unit));
                            if (conversionUnit == null) throw new UserFriendlyException("Stok kartýnda çevrim birimi tanýmý yapýlmamýþ. Stok kartýna gidip çevrim birimi tanýmý yapýnýz.");
                        }
                        if (Unit.Code == "AD")
                        {
                            if (Product != null)
                            {
                                if (Product.OuterPackingInPiece == 0)
                                    throw new UserFriendlyException("Malzeme ana verisinde dýþ ambalaj iç adet sayýsý girilmemiþ.");
                            }
                        }
                        if (Unit.Code == "RL")
                        {
                            if (Product != null)
                            {
                                if (Product.OuterPackingRollCount == 0)
                                    throw new UserFriendlyException("Malzeme ana verisinde dýþ ambalaj deste/rulo sayýsý girilmemiþ.");
                            }
                        }
                    }
                }
            }
            if (SalesOrder != null) SalesOrder.UpdateTotals();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (printingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco1 üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", printingWorkOrder.WorkOrderNumber));

            var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (laminationWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco1 Laminasyon üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", laminationWorkOrder.WorkOrderNumber));

            var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (filmingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco2 üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", filmingWorkOrder.WorkOrderNumber));

            var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (regeneratedWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco3 üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", regeneratedWorkOrder.WorkOrderNumber));

            var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (cuttingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco4 üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (slicingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco4 Dilme üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", slicingWorkOrder.WorkOrderNumber));

            var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (castSlicingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco5 Dilme üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", castSlicingWorkOrder.WorkOrderNumber));

            var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (castFilmingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco5 CPP üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", castFilmingWorkOrder.WorkOrderNumber));

            var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (balloonFilmingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco5 Stretch üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", balloonFilmingWorkOrder.WorkOrderNumber));

            var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (castTransferingWorkOrder != null) throw new UserFriendlyException(string.Format("Bu sipariþ kalemi için {0} nolu Eco5 Aktarma üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", castTransferingWorkOrder.WorkOrderNumber));

            var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (eco6WorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariþ için {0} nolu Eco6 üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", regeneratedWorkOrder.WorkOrderNumber));

            var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("SalesOrderDetail", this));
            if (eco6LaminationWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariþ için {0} nolu Eco6 Laminasyon üretim sipariþ emri oluþturulmuþtur. Sipariþi silebilmek için öncelikle üretim sipariþini silmeniz gerekmektedir.", regeneratedWorkOrder.WorkOrderNumber));
        }
        // Fields...
        private bool _WaitingWorkOrder;
        private decimal _NotifyShippedQuantity;
        private string _InvoiceNote;
        private string _QualityNote;
        private string _ShippingNote;
        private string _WorkOrderNote;
        private string _SAPLineNumber;
        private string _SAPOrderNumber;
        private CuttingMachineGroup _CuttingMachineGroup;
        private ContactGroup4 _AnalysisCertificate;
        private ReportDataV2 _StickerDesign;
        private Cargo _Cargo;
        private decimal _PlateCost;
        private SalesOrderWorkStatus _SalesOrderWorkStatus;
        private bool _QualityBlock;
        private DeliveryBlockType _DeliveryBlockType;
        private ShippingBlockType _ShippingBlockType;
        private decimal _LastProductOption;
        private decimal _SemiProductOption;
        private decimal _ShippedOptionMinus;
        private decimal _ShippedOptionPlus;
        private Palette _Palette;
        private string _CustomerProductCode;
        private DateTime _LineDeliveryDate;
        private decimal _Tax;
        private decimal _CurrencyTax;
        private decimal _Total;
        private decimal _CurrencyTotal;
        private decimal _GeneralCost;
        private decimal _PetkimUnitPrice;
        private decimal _PetkimPrice;
        private decimal _Price;
        private decimal _Parity;
        private decimal _ExchangeRate;
        private decimal _CurrencyPrice;
        private Currency _Currency;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _UnitMultiplier;
        private decimal _Quantity;
        private Unit _Unit;
        private Product _Product;
        private SalesOrderStatus _SalesOrderStatus;
        private int _LineNumber;
        private SalesOrder _SalesOrder;

        [NonCloneable]
        [Association]
        public SalesOrder SalesOrder
        {
            get { return _SalesOrder; }
            set
            {
                SalesOrder prevHome = _SalesOrder;
                if (SetPropertyValue("SalesOrder", ref _SalesOrder, value) && !IsLoading)
                {
                    if (!IsLoading && _SalesOrder != null)
                    {
                        int lineNumber = 10;
                        if (_SalesOrder.SalesOrderDetails.Count > 0)
                        {
                            _SalesOrder.SalesOrderDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _SalesOrder.SalesOrderDetails[0].LineNumber + 10;
                        }
                        LineNumber = lineNumber;
                    }
                }
            }
        }

        [NonPersistent]
        [VisibleInDetailView(false)]
        public string SalesOrderDetailAlias
        {
            get
            {
                return SalesOrder != null ? string.Format("{0}/{1}", SalesOrder.OrderNumber, LineNumber) : LineNumber.ToString();
            }
        }

        [NonCloneable]
        [VisibleInDetailView(false)]
        public int LineNumber
        {
            get
            {
                return _LineNumber;
            }
            set
            {
                SetPropertyValue("LineNumber", ref _LineNumber, value);
            }
        }

        [Appearance("SalesOrderDetail.SalesOrderStatus", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [NonCloneable]
        [VisibleInDetailView(false)]
        public SalesOrderStatus SalesOrderStatus
        {
            get
            {
                return _SalesOrderStatus;
            }
            set
            {
                SetPropertyValue("SalesOrderStatus", ref _SalesOrderStatus, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                SetPropertyValue("Product", ref _Product, value);
                GetProduct();
                GetPetkimPrice();
                GetExchangeRate();
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        [Appearance("SalesOrderDetail.Unit", Context = "DetailView", Enabled = false, Criteria = "StoreQuantity > 0")]
        public Unit Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                SetPropertyValue("Unit", ref _Unit, value);
                GetProductUnit();
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
                CalculatecQuantity();
                UpdateCurrencyTotal();
                UpdateTotal();
            }
        }

        [ImmediatePostData]
        public decimal UnitMultiplier
        {
            get
            {
                return _UnitMultiplier;
            }
            set
            {
                SetPropertyValue("UnitMultiplier", ref _UnitMultiplier, value);
                CalculatePetkimUnitPrice();
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.cUnit", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Unit.Code = Product.Unit.Code")]
        [Appearance("SalesOrderDetail.cUnit.Enable", Enabled = false, Criteria = "Unit.Code != Product.Unit.Code", Context = "DetailView")]
        public Unit cUnit
        {
            get
            {
                return _CUnit;
            }
            set
            {
                SetPropertyValue("cUnit", ref _CUnit, value);
                CalculatecQuantity();
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.cQuantity", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Unit.Code = Product.Unit.Code")]
        [Appearance("SalesOrderDetail.cQuantity.Enable", Enabled = false, Criteria = "Unit.Code != Product.Unit.Code", Context = "DetailView")]
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "Unit.Code != Product.Unit.Code")]
        public decimal cQuantity
        {
            get
            {
                return _CQuantity;
            }
            set
            {
                SetPropertyValue("cQuantity", ref _CQuantity, value);
                CalculatePetkimUnitPrice();
            }
        }

        [RuleRequiredField]
        [ImmediatePostData]
        public Currency Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                SetPropertyValue("Currency", ref _Currency, value);
                GetExchangeRate();
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.CurrencyPrice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyPrice
        {
            get
            {
                return _CurrencyPrice;
            }
            set
            {
                SetPropertyValue("CurrencyPrice", ref _CurrencyPrice, value);
                UpdatePrice();
                UpdateCurrencyTotal();
            }
        }

        [NonCloneable]
        [ImmediatePostData]
        [Appearance("SalesOrderDetail.ExchangeRate", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal ExchangeRate
        {
            get
            {
                return _ExchangeRate;
            }
            set
            {
                SetPropertyValue("ExchangeRate", ref _ExchangeRate, value);
                UpdatePrice();
                CalculatePetkimUnitPrice();
            }
        }

        [Appearance("SalesOrderDetail.Parity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Parity
        {
            get
            {
                return _Parity;
            }
            set
            {
                SetPropertyValue("Parity", ref _Parity, value);
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.Price", Context = "DetailView", Enabled = false, Criteria = "Currency.IsDefault = false")]
        public decimal Price
        {
            get
            {
                return _Price;
            }
            set
            {
                SetPropertyValue("Price", ref _Price, value);
                UpdateTotal();
                CalculatePetkimUnitPrice();
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.PetkimPrice", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal PetkimPrice
        {
            get
            {
                return _PetkimPrice;
            }
            set
            {
                SetPropertyValue("PetkimPrice", ref _PetkimPrice, value);
                CalculatePetkimUnitPrice();
            }
        }

        [Appearance("SalesOrderDetail.PetkimUnitPrice", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal PetkimUnitPrice
        {
            get
            {
                return _PetkimUnitPrice;
            }
            set
            {
                SetPropertyValue("PetkimUnitPrice", ref _PetkimUnitPrice, value);
            }
        }

        [Appearance("SalesOrderDetail.GeneralCost", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal GeneralCost
        {
            get
            {
                return _GeneralCost;
            }
            set
            {
                SetPropertyValue("GeneralCost", ref _GeneralCost, value);
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.CurrencyTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [Appearance("SalesOrderDetail.CurrencyTotal1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyTotal
        {
            get
            {
                return _CurrencyTotal;
            }
            set
            {
                SetPropertyValue("CurrencyTotal", ref _CurrencyTotal, value);
                UpdateCurrencyTax();
            }
        }

        [ImmediatePostData]
        [Appearance("SalesOrderDetail.Total", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetPropertyValue("Total", ref _Total, value);
                UpdateTax();
            }
        }

        [Appearance("SalesOrderDetail.CurrencyTax", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [Appearance("SalesOrderDetail.CurrencyTax1", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyTax
        {
            get
            {
                return _CurrencyTax;
            }
            set
            {
                SetPropertyValue("CurrencyTax", ref _CurrencyTax, value);
            }
        }

        [Appearance("SalesOrderDetail.Tax", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Tax
        {
            get
            {
                return _Tax;
            }
            set
            {
                SetPropertyValue("Tax", ref _Tax, value);
            }
        }

        public DateTime LineDeliveryDate
        {
            get
            {
                return _LineDeliveryDate;
            }
            set
            {
                SetPropertyValue("LineDeliveryDate", ref _LineDeliveryDate, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CustomerProductCode
        {
            get
            {
                return _CustomerProductCode;
            }
            set
            {
                SetPropertyValue("CustomerProductCode", ref _CustomerProductCode, value);
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

        public decimal ShippedOptionPlus
        {
            get
            {
                return _ShippedOptionPlus;
            }
            set
            {
                SetPropertyValue("ShippedOptionPlus", ref _ShippedOptionPlus, value);
            }
        }

        public decimal ShippedOptionMinus
        {
            get
            {
                return _ShippedOptionMinus;
            }
            set
            {
                SetPropertyValue("ShippedOptionMinus", ref _ShippedOptionMinus, value);
            }
        }

        public decimal SemiProductOption
        {
            get
            {
                return _SemiProductOption;
            }
            set
            {
                SetPropertyValue("SemiProductOption", ref _SemiProductOption, value);
            }
        }

        public decimal LastProductOption
        {
            get
            {
                return _LastProductOption;
            }
            set
            {
                SetPropertyValue("LastProductOption", ref _LastProductOption, value);
            }
        }

        public ShippingBlockType ShippingBlockType
        {
            get
            {
                return _ShippingBlockType;
            }
            set
            {
                SetPropertyValue("ShippingBlockType", ref _ShippingBlockType, value);
            }
        }

        public DeliveryBlockType DeliveryBlockType
        {
            get
            {
                return _DeliveryBlockType;
            }
            set
            {
                SetPropertyValue("DeliveryBlockType", ref _DeliveryBlockType, value);
            }
        }

        public bool QualityBlock
        {
            get
            {
                return _QualityBlock;
            }
            set
            {
                SetPropertyValue("QualityBlock", ref _QualityBlock, value);
            }
        }

        public SalesOrderWorkStatus SalesOrderWorkStatus
        {
            get
            {
                return _SalesOrderWorkStatus;
            }
            set
            {
                SetPropertyValue("SalesOrderWorkStatus", ref _SalesOrderWorkStatus, value);
            }
        }

        public decimal PlateCost
        {
            get
            {
                return _PlateCost;
            }
            set
            {
                SetPropertyValue("PlateCost", ref _PlateCost, value);
            }
        }

        public Cargo Cargo
        {
            get
            {
                return _Cargo;
            }
            set
            {
                SetPropertyValue("Cargo", ref _Cargo, value);
            }
        }

        [RuleRequiredField]
        public ReportDataV2 StickerDesign
        {
            get
            {
                return _StickerDesign;
            }
            set
            {
                SetPropertyValue("StickerDesign", ref _StickerDesign, value);
            }
        }

        public ContactGroup4 AnalysisCertificate
        {
            get
            {
                return _AnalysisCertificate;
            }
            set
            {
                SetPropertyValue("AnalysisCertificate", ref _AnalysisCertificate, value);
            }
        }

        [Appearance("SalesOrderDetail.CuttingMachineGroup", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public CuttingMachineGroup CuttingMachineGroup
        {
            get
            {
                return _CuttingMachineGroup;
            }
            set
            {
                SetPropertyValue("CuttingMachineGroup", ref _CuttingMachineGroup, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SAPOrderNumber
        {
            get
            {
                return _SAPOrderNumber;
            }
            set
            {
                SetPropertyValue("SAPOrderNumber", ref _SAPOrderNumber, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SAPLineNumber
        {
            get
            {
                return _SAPLineNumber;
            }
            set
            {
                SetPropertyValue("SAPLineNumber", ref _SAPLineNumber, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string WorkOrderNote
        {
            get
            {
                return _WorkOrderNote;
            }
            set
            {
                SetPropertyValue("WorkOrderNote", ref _WorkOrderNote, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string ShippingNote
        {
            get
            {
                return _ShippingNote;
            }
            set
            {
                SetPropertyValue("ShippingNote", ref _ShippingNote, value);
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

        [Size(SizeAttribute.Unlimited)]
        public string InvoiceNote
        {
            get
            {
                return _InvoiceNote;
            }
            set
            {
                SetPropertyValue("InvoiceNote", ref _InvoiceNote, value);
            }
        }

        [VisibleInDetailView(false)]
        public string E
        {
            get
            {
                Production production = Session.FindObject<Production>(new BinaryOperator("SalesOrderDetail", this));
                return production != null ? "E" : "H";
            }
        }

        [VisibleInDetailView(false)]
        public decimal ProductionQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate<Production>(CriteriaOperator.Parse("sum(GrossQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and IsLastProduction = ?", this, true)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal StoreQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate<Store>(CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.ShippingWarehouse = ?", this, true)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal StorecQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate<Store>(CriteriaOperator.Parse("sum(cQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.ShippingWarehouse = ?", this, true)));
            }
        }

        [Persistent("ShippedQuantity")]
        private decimal _ShippedQuantity;

        [PersistentAlias("_ShippedQuantity")]
        [VisibleInDetailView(false)]
        public decimal ShippedQuantity
        {
            get
            {
                decimal quantity = 0;
                XPCollection<DeliveryDetail> deliveryDetails = new XPCollection<DeliveryDetail>(Session, CriteriaOperator.Parse("SalesOrderDetail = ?", this.Oid));
                foreach (DeliveryDetail detail in deliveryDetails)
                {
                    quantity += detail.LoadedQuantity;
                }
                return quantity;
            }
        }

        [Persistent("ShippedcQuantity")]
        private decimal _ShippedcQuantity;

        [PersistentAlias("_ShippedcQuantity")]
        [VisibleInDetailView(false)]
        public decimal ShippedcQuantity
        {
            get
            {
                decimal quantity = 0;
                XPCollection<DeliveryDetail> deliveryDetails = new XPCollection<DeliveryDetail>(Session, CriteriaOperator.Parse("SalesOrderDetail = ?", this.Oid));
                foreach (DeliveryDetail detail in deliveryDetails)
                {
                    quantity += detail.LoadedcQuantity;
                }
                return quantity;
            }
        }

        [VisibleInDetailView(false)]
        public decimal NotifiedQuantity
        {
            get
            {
                decimal notifiedQuantity = Convert.ToDecimal(Session.Evaluate(typeof(ShippingPlan), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("SalesOrderDetail = ?", Oid)));
                return notifiedQuantity;
            }
        }

        [VisibleInDetailView(false)]
        public decimal NotifyShippedQuantity
        {
            get
            {
                return _NotifyShippedQuantity;
            }
            set
            {
                SetPropertyValue("NotifyShippedQuantity", ref _NotifyShippedQuantity, value);
            }
        }

        [VisibleInDetailView(false)]
        public ShippingPlanStatus ShippingPlanStatus
        {
            get
            {
                if (SalesOrder != null)
                {
                    if (SalesOrder.PaymentBlockage)
                    {
                        return ShippingPlanStatus.WaitingforPaymentProblem;
                    }
                    else if (LineDeliveryDate > Helpers.GetSystemDate(Session))
                    {
                        return ShippingPlanStatus.WaitingforShippingDelivery;
                    }
                    else if (SalesOrder.Blockage == Blockage.NextMonthInvoice)
                    {
                        return ShippingPlanStatus.WaitingforNextMonthInvoice;
                    }
                    else if (SalesOrder.Blockage == Blockage.CustomerWaybill)
                    {
                        return ShippingPlanStatus.WaitingforCustomerWaybill;
                    }
                    else if (SalesOrder.Blockage == Blockage.StoreProduction)
                    {
                        return ShippingPlanStatus.WaitingforStoreProduction;
                    }
                    else if (SalesOrder.ContactVehicle == ContactVehicle.Yes)
                    {
                        return ShippingPlanStatus.WaitingforCustomerVehicle;
                    }
                    else return ShippingPlanStatus.WaitingforExpedition;
                }
                else return ShippingPlanStatus.WaitingforExpedition;
            }
        }

        [NonCloneable]
        [Browsable(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public bool WaitingWorkOrder
        {
            get
            {
                return _WaitingWorkOrder;
            }
            set
            {
                SetPropertyValue("WaitingWorkOrder", ref _WaitingWorkOrder, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<SalesOrderDetailPortfolio> SalesOrderDetailPortfolios
        {
            get { return GetCollection<SalesOrderDetailPortfolio>("SalesOrderDetailPortfolios"); }
        }

        [VisibleInDetailView(false)]
        [Association("SalesOrderDetail-Stores")]
        public XPCollection<Store> Stores
        {
            get
            {
                return GetCollection<Store>("Stores");
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

        [Action(PredefinedCategory.View, Caption = "Planlama Onayý Ver", AutoCommit = true, ImageName = "Action_Grant_Set", TargetObjectsCriteria = "SalesOrderStatus = 'WaitingforPlanningConfirm'", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void PlanningConfirm(PlanningConfirmParameters parameters)
        {
            LineDeliveryDate = parameters.DeliveryDate;
            if (SalesOrder.SalesOrderType == SalesOrderType.SalesReturnOrder) SalesOrderStatus = SalesOrderStatus.WaitingforFilming;
            else SalesOrderStatus = SalesOrderStatus.WaitingforApproval;
        }

        [Action(PredefinedCategory.View, Caption = "Müþteri Onayý Ver", AutoCommit = true, ImageName = "Action_Grant", ConfirmationMessage = "Seçili sipariþ(ler) onaylansýn mý?", TargetObjectsCriteria = "SalesOrderStatus = 'WaitingforApproval'", ToolTip = "Bu iþlem seçili sipariþ kalemlerine onay verecektir.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void ConfirmOrder()
        {
            if (SalesOrder.SalesOrderType == SalesOrderType.RegeneratedOrder)
            {
                SalesOrderStatus = SalesOrderStatus.WaitingforRegenerated;
            }
            else
            {
                if (Product.ProductGroup.Name.Contains("STANDART"))
                {
                    SalesOrderStatus = SalesOrderStatus.WaitingforShipping;
                }
                else
                {
                    if (Product.ProductType.Name.Contains("STRETCH"))
                    {
                        SalesOrderStatus = SalesOrderStatus.WaitingforCastFilming;
                    }
                    else if (Product.ProductType.Name.Contains("BALONLU"))
                    {
                        SalesOrderStatus = SalesOrderStatus.WaitingforBalloonFilming;
                    }
                    else SalesOrderStatus = SalesOrderStatus.WaitingforFilming;
                }
            }

            if (SalesOrder != null)
            {
                Product.Contact = SalesOrder.Contact;
                Product.ShippingContact = SalesOrder.ShippingContact;
            }
        }

        [Action(PredefinedCategory.View, Caption = "Sevk Bildir", AutoCommit = true, ImageName = "BO_Vendor", ToolTip = "Bu iþlem seçili sipariþ kalemleri için sevk bildirme iþlemi yapar.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void ShippingPlanNotify(ShippingPlanNotifyParametersObject parameters)
        {
            if (parameters.SetupDate.Date < Helpers.GetSystemDate(Session).Date)
            {
                throw new UserFriendlyException("Sevke hazýr olacaðý tarih bugünün tarihinden küçük olamaz.");
            }
            if(SalesOrder.Blockage== Blockage.AskBeforeShipping)
            {
                throw new UserFriendlyException("Bu sipariþin blokajý Sevkten Önce Sor olarak ayarlanmýþ. Müþteri temsilcisi ile görüþünüz.");
            }
            if ((NotifyShippedQuantity + NotifiedQuantity) > (Quantity + (Quantity * ShippedOptionPlus) / 100))
            {
                throw new UserFriendlyException("Sevk bildirilecek miktar sipariþ ve sevk opsiyonu miktarýndan fazla olamaz.");
            }
            ShippingPlan shippingPlan = new ShippingPlan(Session)
            {
                SetupDate = parameters.SetupDate,
                SalesOrderDetail = this,
                Unit = Unit,
                Quantity = NotifyShippedQuantity,
                cUnit = cUnit,
                cQuantity = (cQuantity / Quantity) * NotifyShippedQuantity,
                NotifiedUser = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)) != null ? Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)).NameSurname : string.Empty
            };
            if (SalesOrder.PaymentBlockage)
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforPaymentProblem;
            }
            else if (SalesOrder.ContactVehicle == ContactVehicle.Yes)
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforCustomerVehicle;
            }
            else if (DeliveryBlockType == DeliveryBlockType.DeadlineShipment && Helpers.GetSystemDate(Session).Date < LineDeliveryDate.Date.AddDays(-5))
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforShippingDelivery;
            }
            else if (ShippingBlockType == ShippingBlockType.CompleteShipment)
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforCompleteDelivery;
            }
            else if (SalesOrder.Blockage == Blockage.NextMonthInvoice && LineDeliveryDate.Month > Helpers.GetSystemDate(Session).Month)
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforNextMonthInvoice;
            }
            else if (SalesOrder.Blockage == Blockage.CustomerWaybill)
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforCustomerWaybill;
            }
            else if (SalesOrder.Blockage == Blockage.StoreProduction)
            {
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforStoreProduction;
            }
            else shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforExpedition;

            NotifyShippedQuantity = 0;
        }

        [Action(PredefinedCategory.View, Caption = "Petkim Fiyat Güncelle", AutoCommit = true, ImageName = "BO_Opportunity", TargetObjectsCriteria = "SalesOrderStatus != 'Completed'", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void UpdatePetkimPrice()
        {
            if (SalesOrder != null)
            {
                if (Product != null)
                {
                    if (Product.FilmCode2 == null)
                    {
                        PetkimPrice petkimPrice = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and BeginDate <= ? and EndDate >= ?", Product.FilmCode1, SalesOrder.OrderDate, SalesOrder.OrderDate));
                        if (petkimPrice != null)
                        {
                            PetkimPrice = petkimPrice.Price;
                        }
                    }
                    else
                    {
                        PetkimPrice filmCode1Price = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and BeginDate <= ? and EndDate >= ?", Product.FilmCode1, SalesOrder.OrderDate, SalesOrder.OrderDate));
                        PetkimPrice filmCode2Price = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and BeginDate <= ? and EndDate >= ?", Product.FilmCode2, SalesOrder.OrderDate, SalesOrder.OrderDate));
                        decimal filmcCode1ThicknessRate = (Product.FilmCode1Thickness * 100) / Product.Thickness;
                        decimal filmcCode2ThicknessRate = (Product.FilmCode2Thickness * 100) / Product.Thickness;

                        PetkimPrice = (filmCode1Price.Price * filmcCode1ThicknessRate / 100) + (filmCode2Price.Price * filmcCode2ThicknessRate / 100);
                    }
                }
            }
        }

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                CuttingMachineGroup = Product.ProductKind.CuttingMachineGroup;
                Unit = Product.Unit;
            }
        }
        void GetPetkimPrice()
        {
            if (IsLoading) return;
            if (SalesOrder != null)
            {
                if (Product != null)
                {
                    if (Product.FilmCode2 == null)
                    {
                        PetkimPrice petkimPrice = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and BeginDate <= ? and EndDate >= ?", Product.FilmCode1, SalesOrder.OrderDate, SalesOrder.OrderDate));
                        if (petkimPrice != null)
                        {
                            PetkimPrice = petkimPrice.Price;
                        }
                    }
                    else
                    {
                        PetkimPrice filmCode1Price = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and BeginDate <= ? and EndDate >= ?", Product.FilmCode1, SalesOrder.OrderDate, SalesOrder.OrderDate));
                        PetkimPrice filmCode2Price = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and BeginDate <= ? and EndDate >= ?", Product.FilmCode2, SalesOrder.OrderDate, SalesOrder.OrderDate));
                        decimal filmcCode1ThicknessRate = (Product.FilmCode1Thickness * 100) / Product.Thickness;
                        decimal filmcCode2ThicknessRate = (Product.FilmCode2Thickness * 100) / Product.Thickness;

                        PetkimPrice = (filmCode1Price.Price * filmcCode1ThicknessRate / 100) + (filmCode2Price.Price * filmcCode2ThicknessRate / 100);
                    }
                }
            }
        }
        void GetProductUnit()
        {
            if (IsLoading) return;
            if (Unit != null)
            {
                if (Product != null)
                {
                    if (Unit.Code == "AD")
                    {
                        if (Product.OuterPackingInPiece == 0) throw new UserFriendlyException("Malzeme Ana Verisinde Dýþ Ambalaj Ýç Adet Sayýsý 0'dan büyük olmalýdýr. Yoksa Adetli satýþ yapamazsýnýz");
                    }
                    if (Unit.Code == "RL")
                    {
                        if (Product.OuterPackingRollCount == 0) throw new UserFriendlyException("Malzeme Ana Verisinde Dýþ Ambalaj Deste/Rulo Sayýsý 0'dan büyük olmalýdýr. Yoksa Rulo satýþ yapamazsýnýz");
                    }
                }

                if (Unit != Product.Unit) cUnit = Product.Unit;
                else cUnit = Unit;
            }
        }
        void CalculatecQuantity()
        {
            if (IsLoading) return;
            if (cUnit != null)
            {
                if (cUnit == Unit || Unit.Code == "NKG")
                {
                    cQuantity = Quantity;
                }
                else
                {
                    var cunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", Unit, cUnit, Product));
                    if (cunit != null) cQuantity = (Quantity * cunit.cQuantity) / cunit.BaseQuantity;
                    else
                    {
                        cunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", cUnit, Unit, Product));
                        if (cunit != null) cQuantity = (cunit.BaseQuantity * Quantity) / cunit.cQuantity;
                    }
                }
            }
            else cQuantity = Quantity;
        }
        void GetExchangeRate()
        {
            if (IsLoading) return;
            if (Currency != null)
            {
                if (SalesOrder != null)
                {
                    var currencyType = Session.FindObject<CurrencyType>(new BinaryOperator("ForSales", true));

                    var exchangeRate = Session.FindObject<ExchangeRate>(CriteriaOperator.Parse("Currency = ? and CurrencyType.ForSales == true and RateDate = ?", Currency, Convert.ToDateTime(SalesOrder.OrderDate.ToShortDateString())));
                    if (exchangeRate != null)
                    {
                        ExchangeRate = exchangeRate.Rate;
                    }
                    else
                    {
                        var currency = Currency.Code == "TL" ? Session.FindObject<Currency>(new BinaryOperator("Code", "USD")) : Currency;
                        ExchangeRate = Helpers.GetExchangeRate(Session, SalesOrder != null ? SalesOrder.OrderDate : Helpers.GetSystemDate(Session), currency, currencyType);
                    }
                    Currency usdCurrency = Session.FindObject<Currency>(new BinaryOperator("Code", "USD"));
                    decimal usdRate = Helpers.GetExchangeRate(Session, SalesOrder != null ? SalesOrder.OrderDate : Helpers.GetSystemDate(Session), usdCurrency, currencyType);
                    Parity = ExchangeRate / usdRate;
                }
            }
        }
        void CalculatePetkimUnitPrice()
        {
            if (IsLoading) return;
            try
            {
                decimal exchangeRate = (ExchangeRate / Parity) != 0 ? (ExchangeRate / Parity) : 1;
                PetkimUnitPrice = ((Price / UnitMultiplier) / exchangeRate / (cQuantity / Quantity)) * 1000;
                GeneralCost = PetkimUnitPrice - PetkimPrice;
            }
            catch
            {

            }
        }
        void UpdatePrice()
        {
            if (IsLoading) return;
            if (Currency != null)
            {
                if (Currency.IsDefault == false) Price = (CurrencyPrice * ExchangeRate) / UnitMultiplier;
            }
        }
        void UpdateCurrencyTotal()
        {
            if (IsLoading) return;
            CurrencyTotal = (Quantity * CurrencyPrice) / UnitMultiplier;
        }
        void UpdateTotal()
        {
            if (IsLoading) return;
            Total = (Quantity * Price) / UnitMultiplier;
        }
        void UpdateCurrencyTax()
        {
            if (IsLoading) return;
            if (SalesOrder != null)
            {
                if (SalesOrder.SalesOrderType == SalesOrderType.ExportingOrder || SalesOrder.SalesOrderType == SalesOrderType.ExportRegisteredOrder) CurrencyTax = 0;
                else if (Product != null) CurrencyTax = (CurrencyTotal * Product.TaxRate) / 100;
            }
            else if (Product != null) CurrencyTax = (CurrencyTotal * Product.TaxRate) / 100;
        }
        void UpdateTax()
        {
            if (IsLoading) return;
            if (SalesOrder != null)
            {
                if (SalesOrder.SalesOrderType == SalesOrderType.ExportingOrder || SalesOrder.SalesOrderType == SalesOrderType.ExportRegisteredOrder) Tax = 0;
                else if (Product != null) Tax = (Total * Product.TaxRate) / 100;
            }
            else if (Product != null) Tax = (Total * Product.TaxRate) / 100;
        }
        #endregion
    }
}
