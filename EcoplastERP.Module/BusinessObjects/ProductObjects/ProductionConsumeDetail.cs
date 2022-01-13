using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("ProductionConsume.DocumentNumber")]
    [NavigationItem(false)]
    public class ProductionConsumeDetail : BaseObject
    {
        public ProductionConsumeDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private WarehouseCell _WarehouseCell;
        private Warehouse _Warehouse;
        private decimal _Quantity;
        private Unit _Unit;
        private Product _Product;
        private string _Barcode;
        private ProductionConsumeType _ProductionConsumeType;

        [Association]
        public ProductionConsume ProductionConsume { get; set; }

        [ImmediatePostData]
        [RuleRequiredField]
        public ProductionConsumeType ProductionConsumeType
        {
            get
            {
                return _ProductionConsumeType;
            }
            set
            {
                SetPropertyValue("ProductionConsumeType", ref _ProductionConsumeType, value);
            }
        }

        [Appearance("ProductionConsumeDetail.Barcode", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionConsumeType != 'WithBarcode'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductionConsumeType = 'WithBarcode'")]
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
                GetBarcode();
            }
        }

        [ImmediatePostData]
        [Appearance("ProductionConsumeDetail.Product", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionConsumeType = 'WithBarcode'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductionConsumeType != 'WithBarcode'")]
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
            }
        }

        [Appearance("ProductionConsumeDetail.Warehouse", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionConsumeType = 'WithBarcode'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "ProductionConsumeType != 'WithBarcode'")]
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

        [Appearance("ProductionConsumeDetail.WarehouseCell", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "ProductionConsumeType = 'WithBarcode'")]
        public WarehouseCell WarehouseCell
        {
            get
            {
                return _WarehouseCell;
            }
            set
            {
                SetPropertyValue("WarehouseCell", ref _WarehouseCell, value);
            }
        }

        [Appearance("ProductionConsumeDetail.Unit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Appearance("ProductionConsumeDetail.Quantity", Context = "DetailView", Enabled = false, Criteria = "ProductionConsumeType = 'WithBarcode'")]
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

        #region functions
        void GetBarcode()
        {
            if (IsLoading) return;
            if (!string.IsNullOrEmpty(Barcode))
            {
                Store store = Session.FindObject<Store>(new BinaryOperator("Barcode", Barcode));
                if (store != null)
                {
                    Warehouse = store.Warehouse;
                    Unit = store.cUnit;
                    Quantity = store.cQuantity;
                }
            }
        }
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Warehouse = Product.Warehouse;
                Unit = Product.Unit;
            }
        }
        #endregion
    }
}