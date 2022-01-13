using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Customer")]
    [DefaultProperty("NameSurname")]
    [NavigationItem(false)]
    public class ContactPerson : BaseObject
    {
        public ContactPerson(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Email;
        private string _Phone;
        private string _NameSurname;

        [Association]
        public Contact Contact { get; set; }

        [NonCloneable]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NameSurname
        {
            get
            {
                return _NameSurname;
            }
            set
            {
                SetPropertyValue("NameSurname", ref _NameSurname, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                SetPropertyValue("Phone", ref _Phone, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                SetPropertyValue("Email", ref _Email, value);
            }
        }
    }
}
