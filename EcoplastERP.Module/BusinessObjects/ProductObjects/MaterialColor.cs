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
    public class MaterialColor : BaseObject
    {
        public MaterialColor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Name;
        private string _Code;

        [RuleUniqueValue]
        [RuleRequiredField]

        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

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

        [RuleFromBoolProperty("", DefaultContexts.Save, "Kod 3 karakter olmalýdýr", SkipNullOrEmptyValues = false, UsedProperties = "Code", ResultType = ValidationResultType.Warning)]
        protected bool IsCodeValue
        {
            get
            {
                if (Code == null) return false;
                if (Code.Length != 3) return false;
                return true;
            }
        }
    }
}
