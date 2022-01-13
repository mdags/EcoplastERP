using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Quote")]
    [DefaultProperty("DocumentNumber")]
    [NavigationItem("MarketingManagement")]
    public class SalesReturn : BaseObject
    {
        public SalesReturn(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SalesReturnStatus = SalesReturnStatus.WaitingForComplete;
            DocumentDate = Helpers.GetSystemDate(Session);
            DocumentNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
        }
        protected override void OnSaving()
        {
            base.OnSaving();

        }
        // Fields...
        private Contact _Contact;
        private string _ReferenceDocumentNumber;
        private DateTime _DocumentDate;
        private string _DocumentNumber;
        private SalesReturnStatus _SalesReturnStatus;

        [VisibleInDetailView(false)]
        public SalesReturnStatus SalesReturnStatus
        {
            get
            {
                return _SalesReturnStatus;
            }
            set
            {
                SetPropertyValue("SalesReturnStatus", ref _SalesReturnStatus, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ReferenceDocumentNumber
        {
            get
            {
                return _ReferenceDocumentNumber;
            }
            set
            {
                SetPropertyValue("ReferenceDocumentNumber", ref _ReferenceDocumentNumber, value);
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

        [Association, Aggregated]
        public XPCollection<SalesReturnDetail> SalesReturnDetails
        {
            get { return GetCollection<SalesReturnDetail>("SalesReturnDetails"); }
        }
    }
}
