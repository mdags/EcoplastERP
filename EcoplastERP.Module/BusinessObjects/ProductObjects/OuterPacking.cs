using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class OuterPacking : BaseObject
    {
        public OuterPacking(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _Weight;
        private string _Name;

        [RuleUniqueValue]
        [RuleRequiredField]
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

        //[RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                SetPropertyValue("Weight", ref _Weight, value);
            }
        }
    }
}
