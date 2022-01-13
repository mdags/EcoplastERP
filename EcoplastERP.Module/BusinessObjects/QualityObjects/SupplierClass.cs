using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("QualityManagement")]
    public class SupplierClass : BaseObject
    {
        public SupplierClass(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private int _MaxScore;
        private int _MinScore;
        private string _Name;

        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        public int MinScore
        {
            get
            {
                return _MinScore;
            }
            set
            {
                SetPropertyValue("MinScore", ref _MinScore, value);
            }
        }

        public int MaxScore
        {
            get
            {
                return _MaxScore;
            }
            set
            {
                SetPropertyValue("MaxScore", ref _MaxScore, value);
            }
        }
    }
}
