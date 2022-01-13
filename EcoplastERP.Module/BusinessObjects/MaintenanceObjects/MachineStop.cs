using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Debug_Breakpoint_Toggle")]
    [DefaultProperty("Machine.Code")]
    [NavigationItem("ProductionManagement")]
    public class MachineStop : BaseObject
    {
        public MachineStop(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Active = true;
            BeginDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.IsNewObject(this))
            {
                var existStop = Session.FindObject<MachineStop>(CriteriaOperator.Parse("Machine = ? and Active = true", Machine));
                if (existStop != null) throw new UserFriendlyException("Bu makine için zaten aktif bir duruş kaydı var.");
            }
            if (EndDate > new DateTime(2010, 1, 1)) Active = false;
        }
        // Fields...
        private string _Note;
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private StopCode _StopCode;
        private Machine _Machine;
        private string _WorkOrderNumber;
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
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string WorkOrderNumber
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

        [RuleRequiredField]
        public StopCode StopCode
        {
            get
            {
                return _StopCode;
            }
            set
            {
                SetPropertyValue("StopCode", ref _StopCode, value);
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

        public int StopTime
        {
            get
            {
                return (EndDate - BeginDate).Minutes;
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

        [Action(PredefinedCategory.View, Caption = "Duruşu Bitir", AutoCommit = true, ImageName = "Action_Grant", TargetObjectsCriteria = "Active = true", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void EndStop()
        {
            Active = false;
            EndDate = DateTime.Now;
        }
    }
}
