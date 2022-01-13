using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Product.Name")]
    [NavigationItem(false)]
    public class RezervationDetail : BaseObject
    {
        public RezervationDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _Quantity;
        private decimal _RecieptQuantity;
        private Unit _Unit;
        private Warehouse _DestinationWarehouse;
        private Warehouse _SourceWarehouse;
        private Product _Product;

        [Association]
        public Rezervation Rezervation { get; set; }

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

        [Appearance("RezervationDetail.StoreQuantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal StoreQuantity
        {
            get
            {
                if (Product != null && DestinationWarehouse != null && Unit != null)
                {
                    return Convert.ToDecimal(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", Product, DestinationWarehouse, Unit)));
                }
                else return 0;
            }
        }

        [Appearance("RezervationDetail.RecieptQuantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal RecieptQuantity
        {
            get
            {
                return _RecieptQuantity;
            }
            set
            {
                SetPropertyValue("RecieptQuantity", ref _RecieptQuantity, value);
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
    }
}
