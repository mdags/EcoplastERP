using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Note")]
    [DefaultProperty("DemandNumber")]
    [NavigationItem("MaintenanceManagement")]
    public class MaintenanceDemand : BaseObject
    {
        public MaintenanceDemand(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = MaintenanceDemandStatus.WaitingMaintenance;
            DemandDate = Helpers.GetSystemDate(Session);
            DemandNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        // Fields...
        private string _Note;
        private Employee _CreatedBy;
        private Priority _Priority;
        private MalfunctionReason _MalfunctionReason;
        private Asset _Asset;
        private DateTime _DemandDate;
        private int _DemandNumber;
        private MaintenanceDemandStatus _Status;

        [VisibleInDetailView(false)]
        public MaintenanceDemandStatus Status
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

        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public int DemandNumber
        {
            get
            {
                return _DemandNumber;
            }
            set
            {
                SetPropertyValue("DemandNumber", ref _DemandNumber, value);
            }
        }

        public DateTime DemandDate
        {
            get
            {
                return _DemandDate;
            }
            set
            {
                SetPropertyValue("DemandDate", ref _DemandDate, value);
            }
        }

        public Asset Asset
        {
            get
            {
                return _Asset;
            }
            set
            {
                SetPropertyValue("Asset", ref _Asset, value);
            }
        }

        [RuleRequiredField]
        public MalfunctionReason MalfunctionReason
        {
            get
            {
                return _MalfunctionReason;
            }
            set
            {
                SetPropertyValue("MalfunctionReason", ref _MalfunctionReason, value);
            }
        }

        public Priority Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                SetPropertyValue("Priority", ref _Priority, value);
            }
        }

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
