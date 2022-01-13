using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Project")]
    [DefaultProperty("Year")]
    [NavigationItem(false)]
    public class MachineCapacityTarget : BaseObject
    {
        public MachineCapacityTarget(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Year = Helpers.GetSystemDate(Session).Year;
            Month = Helpers.GetSystemDate(Session).Month;
        }
        // Fields...
        private decimal _Rate;
        private FilmKind _FilmKind;
        private CapacityGroup _CapacityGroup;
        private int _Month;
        private int _Year;

        [Association]
        public Machine Machine { get; set; }

        [RuleRequiredField]
        public int Year
        {
            get
            {
                return _Year;
            }
            set
            {
                SetPropertyValue("Year", ref _Year, value);
            }
        }

        [RuleRequiredField]
        public int Month
        {
            get
            {
                return _Month;
            }
            set
            {
                SetPropertyValue("Month", ref _Month, value);
            }
        }

        [RuleRequiredField]
        public FilmKind FilmKind
        {
            get
            {
                return _FilmKind;
            }
            set
            {
                SetPropertyValue("FilmKind", ref _FilmKind, value);
            }
        }

        [RuleRequiredField]
        public CapacityGroup CapacityGroup
        {
            get
            {
                return _CapacityGroup;
            }
            set
            {
                SetPropertyValue("CapacityGroup", ref _CapacityGroup, value);
            }
        }

        //[RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                SetPropertyValue("Rate", ref _Rate, value);
            }
        }
    }
}
