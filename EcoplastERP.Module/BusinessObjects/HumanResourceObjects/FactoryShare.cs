using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.HumanResourceObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [NavigationItem("HumanResourceManagement")]
    [DefaultProperty("Name")]
    public class FactoryShare : BaseObject
    {
        public FactoryShare(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
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
    }
}
