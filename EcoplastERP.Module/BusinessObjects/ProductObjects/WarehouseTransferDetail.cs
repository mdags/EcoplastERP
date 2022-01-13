using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("WarehouseTransfer.DocumentNumber")]
    [NavigationItem(false)]
    public class WarehouseTransferDetail : BaseObject
    {
        public WarehouseTransferDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _Quantity;
        private Unit _Unit;
        private Warehouse _DestinationWarehouse;
        private Warehouse _SourceWarehouse;
        private Product _Product;

        [Association]
        public WarehouseTransfer WarehouseTransfer { get; set; }

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
            }
        }

        [RuleRequiredField]
        public Warehouse SourceWarehouse
        {
            get
            {
                return _SourceWarehouse;
            }
            set
            {
                SetPropertyValue("SourceWarehouse", ref _SourceWarehouse, value);
            }
        }

        [RuleRequiredField]
        public Warehouse DestinationWarehouse
        {
            get
            {
                return _DestinationWarehouse;
            }
            set
            {
                SetPropertyValue("DestinationWarehouse", ref _DestinationWarehouse, value);
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

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Unit = Product.Unit;
            }
        }
        #endregion
    }
}
