using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("MarketingManagement")]
    public class PaymentMethod : BaseObject
    {
        public PaymentMethod(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private Int16 _TermDay;
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

        public Int16 TermDay
        {
            get
            {
                return _TermDay;
            }
            set
            {
                SetPropertyValue("TermDay", ref _TermDay, value);
            }
        }
    }
}
