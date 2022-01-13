using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("WorkShift.Name")]
    [NavigationItem("ProductionManagement")]
    public class ShiftStart : BaseObject
    {
        public ShiftStart(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Active = true;
            StartTime = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                //aktif vardiyanýn atanan personelleri için atama bitiþ tarihi düzenler
                ShiftStart shift = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("Active = true"));
                if (shift != null)
                {
                    shift.Active = false;
                    shift.EndTime = Helpers.GetSystemDate(Session);
                    XPCollection<ShiftAssignment> shiftAssignmentCollection = new XPCollection<ShiftAssignment>(Session, CriteriaOperator.Parse("ShiftStart = ?", shift));
                    if (shiftAssignmentCollection != null)
                    {
                        foreach (ShiftAssignment item in shiftAssignmentCollection)
                        {
                            item.EndDate = Helpers.GetSystemDate(Session);
                        }
                    }
                }

                //Kapatýlan ayný son vardiyanýn atanan personelleri kopyalanýr.
                ShiftStart oldShiftStart = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("WorkShift = ? and Active = false and EndTime = [<ShiftStart>].Max(EndTime)", WorkShift));
                if (oldShiftStart != null)
                {
                    XPCollection<ShiftAssignment> shiftAssignmentList = new XPCollection<ShiftAssignment>(Session, CriteriaOperator.Parse("ShiftStart = ?", oldShiftStart));
                    foreach (ShiftAssignment item in shiftAssignmentList)
                    {
                        ShiftAssignment newShiftAssignment = new ShiftAssignment(Session)
                        {
                            ShiftStart = this,
                            Station = item.Station,
                            Machine = item.Machine,
                            Employee = item.Employee,
                            BeginDate = item.BeginDate,
                            EmployeeTask = item.EmployeeTask
                        };
                    }
                }
            }
        }
        // Fields...
        private DateTime _EndTime;
        private DateTime _StartTime;
        private WorkShift _WorkShift;
        private bool _Active;

        [VisibleInDetailView(false)]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                SetPropertyValue("Active", ref _Active, value);
            }
        }

        [RuleRequiredField]
        public WorkShift WorkShift
        {
            get
            {
                return _WorkShift;
            }
            set
            {
                SetPropertyValue("WorkShift", ref _WorkShift, value);
            }
        }

        [VisibleInDetailView(false)]
        public DateTime StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                SetPropertyValue("StartTime", ref _StartTime, value);
            }
        }

        [VisibleInDetailView(false)]
        public DateTime EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                SetPropertyValue("EndTime", ref _EndTime, value);
            }
        }
    }
}
