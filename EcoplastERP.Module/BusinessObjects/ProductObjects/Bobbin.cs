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
    public class Bobbin : BaseObject
    {
        public Bobbin(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _BobbinOutDiameter;
        private int _BobbinInDiameter;
        private int _Weight;
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

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public int Weight
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

        public int BobbinInDiameter
        {
            get
            {
                return _BobbinInDiameter;
            }
            set
            {
                SetPropertyValue("BobbinInDiameter", ref _BobbinInDiameter, value);
            }
        }

        public decimal BobbinOutDiameter
        {
            get
            {
                return _BobbinOutDiameter;
            }
            set
            {
                SetPropertyValue("BobbinOutDiameter", ref _BobbinOutDiameter, value);
            }
        }
    }
}
