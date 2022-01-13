using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("DocumentNumber")]
    [NavigationItem("PurchaseManagement")]
    public class DIIB : BaseObject
    {
        public DIIB(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DocumentDate = Helpers.GetSystemDate(Session);
            BeginDate = Helpers.GetSystemDate(Session);
            EndDate = Helpers.GetSystemDate(Session);
        }
        // Fields...
        private string _Notes;
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private string _DocumentNumber;
        private DateTime _DocumentDate;

        [RuleUniqueValue]
        [RuleRequiredField]
        public string DocumentNumber
        {
            get
            {
                return _DocumentNumber;
            }
            set
            {
                SetPropertyValue("DocumentNumber", ref _DocumentNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime DocumentDate
        {
            get
            {
                return _DocumentDate;
            }
            set
            {
                SetPropertyValue("DocumentDate", ref _DocumentDate, value);
            }
        }

        [RuleRequiredField]
        public DateTime BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                SetPropertyValue("BeginDate", ref _BeginDate, value);
            }
        }

        [RuleRequiredField]
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetPropertyValue("EndDate", ref _EndDate, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                SetPropertyValue("Notes", ref _Notes, value);
            }
        }
    }
}
