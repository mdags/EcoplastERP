using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("PUnit.Code")]
    [NavigationItem(false)]
    public class ConversionUnit : BaseObject
    {
        public ConversionUnit(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            pUnit = Session.FindObject<Unit>(new BinaryOperator("Code", "KG"));
        }
        // Fileds...
        private Unit _CUnit;
        private decimal _CQuantity;
        private Unit _PUnit;
        private decimal _BaseQuantity;

        [Association]
        public Product Product { get; set; }

        [RuleRequiredField]
        public decimal BaseQuantity
        {
            get
            {
                return _BaseQuantity;
            }
            set
            {
                SetPropertyValue("BaseQuantity", ref _BaseQuantity, value);
            }
        }

        [Appearance("ConversionUnit.pUnit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [RuleRequiredField]
        public Unit pUnit
        {
            get
            {
                return _PUnit;
            }
            set
            {
                SetPropertyValue("pUnit", ref _PUnit, value);
            }
        }

        [RuleRequiredField]
        public decimal cQuantity
        {
            get
            {
                return _CQuantity;
            }
            set
            {
                SetPropertyValue("cQuantity", ref _CQuantity, value);
            }
        }

        [RuleRequiredField]
        public Unit cUnit
        {
            get
            {
                return _CUnit;
            }
            set
            {
                SetPropertyValue("cUnit", ref _CUnit, value);
            }
        }
    }
}
