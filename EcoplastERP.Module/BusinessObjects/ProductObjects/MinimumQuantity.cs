using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Product.Code")]
    [NavigationItem(false)]
    public class MinimumQuantity : BaseObject
    {
        public MinimumQuantity(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private MinimumQuantityProcess _Process;
        private decimal _Quantity;
        private Warehouse _Warehouse;

        [Association]
        public Product Product { get; set; }

        [RuleRequiredField]
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

        [RuleRequiredField]
        public MinimumQuantityProcess Process
        {
            get
            {
                return _Process;
            }
            set
            {
                SetPropertyValue("Process", ref _Process, value);
            }
        }
    }
}
