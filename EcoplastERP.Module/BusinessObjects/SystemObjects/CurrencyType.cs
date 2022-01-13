using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.SystemObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    public class CurrencyType : BaseObject
    {
        public CurrencyType(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private bool _ForSales;
        private bool _ForPurchase;
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

        public bool ForPurchase
        {
            get
            {
                return _ForPurchase;
            }
            set
            {
                SetPropertyValue("ForPurchase", ref _ForPurchase, value);
            }
        }

        public bool ForSales
        {
            get
            {
                return _ForSales;
            }
            set
            {
                SetPropertyValue("ForSales", ref _ForSales, value);
            }
        }
    }
}
