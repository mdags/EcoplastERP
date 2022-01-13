using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("MaintenanceManagement")]
    public class PeriodicMaintenance : BaseObject
    {
        public PeriodicMaintenance(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Note;
        private int _MaintenanceTime;
        private DateTime _FirstMaintenanceDate;
        private MaintenanceStopType _MaintenanceStopType;
        private MaintenancePeriod _MaintenancePeriod;
        private Asset _Asset;
        private MaintenanceType _MaintenanceType;
        private string _Name;

        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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
        public MaintenancePeriod MaintenancePeriod
        {
            get
            {
                return _MaintenancePeriod;
            }
            set
            {
                SetPropertyValue("MaintenancePeriod", ref _MaintenancePeriod, value);
            }
        }

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

        public DateTime FirstMaintenanceDate
        {
            get
            {
                return _FirstMaintenanceDate;
            }
            set
            {
                SetPropertyValue("FirstMaintenanceDate", ref _FirstMaintenanceDate, value);
            }
        }

        public int MaintenanceTime
        {
            get
            {
                return _MaintenanceTime;
            }
            set
            {
                SetPropertyValue("MaintenanceTime", ref _MaintenanceTime, value);
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

        [Association("PeriodicMaintenance-Employees")]
        public XPCollection<Employee> Employees
        {
            get
            {
                return GetCollection<Employee>("Employees");
            }
        }

        [Association("PeriodicMaintenance-Subcontractors")]
        public XPCollection<Contact> Subcontractors
        {
            get
            {
                return GetCollection<Contact>("Subcontractors");
            }
        }
    }
}
