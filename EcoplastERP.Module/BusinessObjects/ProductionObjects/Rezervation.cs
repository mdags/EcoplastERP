using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("RezervationNumber")]
    [NavigationItem("ProductionManagement")]
    public class Rezervation : BaseObject
    {
        public Rezervation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            RezervationDate = Helpers.GetSystemDate(Session);
            RezervationNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
        }
        // Fields...
        private string _Description;
        private DateTime _RezervationDate;
        private string _RezervationNumber;
        private WarehouseTransferStatus _Status;

        [VisibleInDetailView(false)]
        public WarehouseTransferStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                SetPropertyValue("Status", ref _Status, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public string RezervationNumber
        {
            get
            {
                return _RezervationNumber;
            }
            set
            {
                SetPropertyValue("RezervationNumber", ref _RezervationNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime RezervationDate
        {
            get
            {
                return _RezervationDate;
            }
            set
            {
                SetPropertyValue("RezervationDate", ref _RezervationDate, value);
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetPropertyValue("Description", ref _Description, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<RezervationDetail> RezervationDetails
        {
            get { return GetCollection<RezervationDetail>("RezervationDetails"); }
        }
    }
}
