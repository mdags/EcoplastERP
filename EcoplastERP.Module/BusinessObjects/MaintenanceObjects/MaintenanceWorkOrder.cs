using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Task")]
    [DefaultProperty("WorkOrderNumber")]
    [NavigationItem("MaintenanceManagement")]
    public class MaintenanceWorkOrder : BaseObject
    {
        public MaintenanceWorkOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = MaintenanceWorkOrderStatus.WaitingMaintenance;
            SequenceNumber = 99;
            WorkOrderDate = Helpers.GetSystemDate(Session);
            WorkOrderNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (MaintenanceDemand != null)
            {
                if(MaintenanceDemand.Status == MaintenanceDemandStatus.WaitingMaintenance)
                {
                    MaintenanceDemand.Status = MaintenanceDemandStatus.Ordered;
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (MaintenanceDemand != null)
            {
                MaintenanceDemand.Status = MaintenanceDemandStatus.WaitingMaintenance;
            }
        }
        // Fields...
        private string _MaintenanceEndNote;
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private string _ISGNote;
        private string _MaintenanceNote;
        private Priority _Priority;
        private MaintenanceStopType _MaintenanceStopType;
        private MalfunctionReason _MalfunctionReason;
        private Asset _Asset;
        private MaintenanceType _MaintenanceType;
        private MaintenanceDemand _MaintenanceDemand;
        private DateTime _WorkOrderDate;
        private int _WorkOrderNumber;
        private Int16 _SequenceNumber;
        private MaintenanceWorkOrderStatus _Status;

        [VisibleInDetailView(false)]
        public MaintenanceWorkOrderStatus Status
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

        [VisibleInDetailView(false)]
        public Int16 SequenceNumber
        {
            get
            {
                return _SequenceNumber;
            }
            set
            {
                SetPropertyValue("SequenceNumber", ref _SequenceNumber, value);
            }
        }

        [RuleRequiredField]
        public int WorkOrderNumber
        {
            get
            {
                return _WorkOrderNumber;
            }
            set
            {
                SetPropertyValue("WorkOrderNumber", ref _WorkOrderNumber, value);
            }
        }

        public DateTime WorkOrderDate
        {
            get
            {
                return _WorkOrderDate;
            }
            set
            {
                SetPropertyValue("WorkOrderDate", ref _WorkOrderDate, value);
            }
        }

        [RuleRequiredField]
        public MaintenanceType MaintenanceType
        {
            get
            {
                return _MaintenanceType;
            }
            set
            {
                SetPropertyValue("MaintenanceType", ref _MaintenanceType, value);
            }
        }

        [ImmediatePostData]
        public MaintenanceDemand MaintenanceDemand
        {
            get
            {
                return _MaintenanceDemand;
            }
            set
            {
                SetPropertyValue("MaintenanceDemand", ref _MaintenanceDemand, value);
                GetMaintenanceDemand();
            }
        }

        [RuleRequiredField]
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

        [RuleRequiredField]
        public MaintenanceStopType MaintenanceStopType
        {
            get
            {
                return _MaintenanceStopType;
            }
            set
            {
                SetPropertyValue("MaintenanceStopType", ref _MaintenanceStopType, value);
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

        [Size(SizeAttribute.Unlimited)]
        public string MaintenanceNote
        {
            get
            {
                return _MaintenanceNote;
            }
            set
            {
                SetPropertyValue("MaintenanceNote", ref _MaintenanceNote, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string ISGNote
        {
            get
            {
                return _ISGNote;
            }
            set
            {
                SetPropertyValue("ISGNote", ref _ISGNote, value);
            }
        }

        [VisibleInDetailView(false)]
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

        [VisibleInDetailView(false)]
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

        [VisibleInDetailView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string MaintenanceEndNote
        {
            get
            {
                return _MaintenanceEndNote;
            }
            set
            {
                SetPropertyValue("MaintenanceEndNote", ref _MaintenanceEndNote, value);
            }
        }

        [Association("MaintenanceWorkOrder-Employees")]
        public XPCollection<Employee> Employees
        {
            get
            {
                return GetCollection<Employee>("Employees");
            }
        }

        [Association("MaintenanceWorkOrder-Subcontractors")]
        public XPCollection<Contact> Subcontractors
        {
            get
            {
                return GetCollection<Contact>("Subcontractors");
            }
        }

        [Association, Aggregated]
        public XPCollection<MaintenanceWorkOrderReciept> MaintenanceWorkOrderReciepts
        {
            get { return GetCollection<MaintenanceWorkOrderReciept>("MaintenanceWorkOrderReciepts"); }
        }

        [Association, Aggregated]
        public XPCollection<MaintenanceWorkOrderPortfolio> MaintenanceWorkOrderPortfolios
        {
            get { return GetCollection<MaintenanceWorkOrderPortfolio>("MaintenanceWorkOrderPortfolios"); }
        }

        #region functions
        void GetMaintenanceDemand()
        {
            if (IsLoading) return;
            if (MaintenanceDemand != null)
            {
                Asset = MaintenanceDemand.Asset;
                MalfunctionReason = MaintenanceDemand.MalfunctionReason;
                Priority = MaintenanceDemand.Priority;
            }
        }
        #endregion
    }
}
