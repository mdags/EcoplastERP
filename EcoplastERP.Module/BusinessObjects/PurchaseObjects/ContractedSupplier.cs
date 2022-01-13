using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("ContractNumber")]
    [NavigationItem("PurchaseManagement")]
    public class ContractedSupplier : BaseObject
    {
        public ContractedSupplier(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ContractNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            BeginDate = DateTime.Now;
        }
        // Fields...
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private Contact _Contact;
        private int _ContractNumber;

        [RuleRequiredField]
        public int ContractNumber
        {
            get
            {
                return _ContractNumber;
            }
            set
            {
                SetPropertyValue("ContractNumber", ref _ContractNumber, value);
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

        [Association, Aggregated]
        public XPCollection<ContractedSupplierDetail> ContractedSupplierDetails
        {
            get { return GetCollection<ContractedSupplierDetail>("ContractedSupplierDetails"); }
        }
    }
}
