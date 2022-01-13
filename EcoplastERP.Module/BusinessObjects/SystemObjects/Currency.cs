using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.SystemObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Opportunity")]
    [DefaultProperty("Code")]
    public class Currency : BaseObject
    {
        public Currency(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private bool _IsDefault;
        private string _CurrencyMark;
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CurrencyMark
        {
            get
            {
                return _CurrencyMark;
            }
            set
            {
                SetPropertyValue("CurrencyMark", ref _CurrencyMark, value);
            }
        }

        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }
            set
            {
                SetPropertyValue("IsDefault", ref _IsDefault, value);
            }
        }
    }
}
