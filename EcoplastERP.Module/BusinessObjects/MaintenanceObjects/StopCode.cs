using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductionManagement")]
    public class StopCode : BaseObject
    {
        public StopCode(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private StopGroupCode _StopGroupCode;
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

        [RuleRequiredField]
        public StopGroupCode StopGroupCode
        {
            get
            {
                return _StopGroupCode;
            }
            set
            {
                SetPropertyValue("StopGroupCode", ref _StopGroupCode, value);
            }
        }
    }
}
