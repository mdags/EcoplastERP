using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Employee.NameSurname")]
    [NavigationItem("ProductionManagement")]
    public class ShiftAssignment : BaseObject
    {
        public ShiftAssignment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            BeginDate = Helpers.GetSystemDate(Session);
            if (Session.IsNewObject(this))
            {
                ShiftStart shift = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("Active = true"));
                if (shift != null) ShiftStart = shift;
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                var assignment = Session.FindObject<ShiftAssignment>(CriteriaOperator.Parse("ShiftStart = ? and Station = ? and Machine = ? and EmployeeTask.Name = 'OPERATÖR'", ShiftStart, Station, Machine));
                if (assignment != null)
                {
                    throw new UserFriendlyException("Ayný makineye birden fazla operatör atayamazsýnýz.");
                }
                else
                {
                    assignment = Session.FindObject<ShiftAssignment>(CriteriaOperator.Parse("ShiftStart = ? and Station = ? and Machine = ? and Employee = ?", ShiftStart, Station, Machine, Employee));
                    if (assignment != null) throw new UserFriendlyException("Ayný operatörü ayný makineye birden fazla atayamazsýnýz.");
                }
            }
        }
        // Fields...
        private EmployeeTask _EmployeeTask;
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private Employee _Employee;
        private Machine _Machine;
        private Station _Station;
        private ShiftStart _ShiftStart;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public ShiftStart ShiftStart
        {
            get
            {
                return _ShiftStart;
            }
            set
            {
                SetPropertyValue("ShiftStart", ref _ShiftStart, value);
            }
        }
        
        [RuleRequiredField]
        public Station Station
        {
            get
            {
                return _Station;
            }
            set
            {
                SetPropertyValue("Station", ref _Station, value);
            }
        }

        [RuleRequiredField]
        public Machine Machine
        {
            get
            {
                return _Machine;
            }
            set
            {
                SetPropertyValue("Machine", ref _Machine, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public Employee Employee
        {
            get
            {
                return _Employee;
            }
            set
            {
                SetPropertyValue("Employee", ref _Employee, value);
                GetEmployeeTask();
            }
        }

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
        
        public EmployeeTask EmployeeTask
        {
            get
            {
                return _EmployeeTask;
            }
            set
            {
                SetPropertyValue("EmployeeTask", ref _EmployeeTask, value);
            }
        }

        #region functions
        void GetEmployeeTask()
        {
            if (IsLoading) return;
            if (Employee != null) EmployeeTask = Employee.EmployeeTask;
        }
        #endregion
    }
}
