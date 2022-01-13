using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Contact.Name")]
    [NavigationItem("QualityManagement")]
    public class SupplierSupervision : BaseObject
    {
        public SupplierSupervision(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SupervisionDate = Helpers.GetSystemDate(Session);
        }
        // Fields...
        private string _Note;
        private int _FoodSafetyScore;
        private int _KYSScore;
        private Contact _Contact;
        private DateTime _SupervisionDate;

        public DateTime SupervisionDate
        {
            get
            {
                return _SupervisionDate;
            }
            set
            {
                SetPropertyValue("SupervisionDate", ref _SupervisionDate, value);
            }
        }

        [RuleRequiredField]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
            }
        }

        public int KYSScore
        {
            get
            {
                return _KYSScore;
            }
            set
            {
                SetPropertyValue("KYSScore", ref _KYSScore, value);
            }
        }

        public int FoodSafetyScore
        {
            get
            {
                return _FoodSafetyScore;
            }
            set
            {
                SetPropertyValue("FoodSafetyScore", ref _FoodSafetyScore, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                SetPropertyValue("Note", ref _Note, value);
            }
        }
    }
}
