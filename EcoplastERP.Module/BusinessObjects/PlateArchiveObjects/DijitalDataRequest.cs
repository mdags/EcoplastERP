using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PlateArchiveObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("FormNumber")]
    [NavigationItem("PlateArchiveManagement")]
    public class DijitalDataRequest : BaseObject
    {
        public DijitalDataRequest(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FormNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            FormDate = DateTime.Now;
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        // Fields...
        private Employee _CreatedBy;
        private string _OtherRequestedDijital;
        private RequestedDijital _RequestedDijital;
        private string _OtherIncomingDijital;
        private IncomingDijital _IncomingDijital;
        private string _WorkName;
        private Contact _Contact;
        private DateTime _FormDate;
        private int _FormNumber;
        private DijitalDataRequestStatus _DijitalDataRequestStatus;

        [VisibleInDetailView(false)]
        public DijitalDataRequestStatus DijitalDataRequestStatus
        {
            get
            {
                return _DijitalDataRequestStatus;
            }
            set
            {
                SetPropertyValue("DijitalDataRequestStatus", ref _DijitalDataRequestStatus, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public int FormNumber
        {
            get
            {
                return _FormNumber;
            }
            set
            {
                SetPropertyValue("FormNumber", ref _FormNumber, value);
            }
        }

        public DateTime FormDate
        {
            get
            {
                return _FormDate;
            }
            set
            {
                SetPropertyValue("FormDate", ref _FormDate, value);
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
        public string WorkName
        {
            get
            {
                return _WorkName;
            }
            set
            {
                SetPropertyValue("WorkName", ref _WorkName, value);
            }
        }

        public IncomingDijital IncomingDijital
        {
            get
            {
                return _IncomingDijital;
            }
            set
            {
                SetPropertyValue("IncomingDijital", ref _IncomingDijital, value);
            }
        }

        [Appearance("DijitalDataRequest.OtherIncomingDijital", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "IncomingDijital != 'Other'")]
        public string OtherIncomingDijital
        {
            get
            {
                return _OtherIncomingDijital;
            }
            set
            {
                SetPropertyValue("OtherIncomingDijital", ref _OtherIncomingDijital, value);
            }
        }

        public RequestedDijital RequestedDijital
        {
            get
            {
                return _RequestedDijital;
            }
            set
            {
                SetPropertyValue("RequestedDijital", ref _RequestedDijital, value);
            }
        }

        [Appearance("DijitalDataRequest.OtherRequestedDijital", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "RequestedDijital != 'Other'")]
        public string OtherRequestedDijital
        {
            get
            {
                return _OtherRequestedDijital;
            }
            set
            {
                SetPropertyValue("OtherRequestedDijital", ref _OtherRequestedDijital, value);
            }
        }

        [VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }
    }
}
