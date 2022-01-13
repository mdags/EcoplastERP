using System;
using System.Globalization;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.ReportObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Report")]
    [DefaultProperty("CuttingMachineGroup")]
    [NavigationItem(false)]
    public class CuttingMachineGroupReport : BaseObject
    {
        public CuttingMachineGroupReport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ReportDate = Helpers.GetSystemDate(Session);
            ThisMonthName = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.MonthNames[DateTime.Now.Month - 1];
            NextMonthName = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.MonthNames[DateTime.Now.Month];
        }
        // Fields...
        private decimal _NextProductionDay;
        private decimal _NextProduction;
        private decimal _NextMontProductionDay;
        private decimal _NextMonthProduction;
        private string _NextMonthName;
        private decimal _ThisMonthProductionDay;
        private decimal _ThisMonthProduction;
        private string _ThisMonthName;
        private decimal _PastProductionDay;
        private decimal _PastProduction;
        private decimal _StoreDay;
        private decimal _StoreQuantity;
        private decimal _CapacityDay;
        private decimal _Capacity;
        private decimal _ProduceQuantity;
        private string _CuttingMachineGroup;
        private string _ProductGroup;
        private DateTime _ReportDate;

        public DateTime ReportDate
        {
            get
            {
                return _ReportDate;
            }
            set
            {
                SetPropertyValue("ReportDate", ref _ReportDate, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductGroup
        {
            get
            {
                return _ProductGroup;
            }
            set
            {
                SetPropertyValue("ProductGroup", ref _ProductGroup, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CuttingMachineGroup
        {
            get
            {
                return _CuttingMachineGroup;
            }
            set
            {
                SetPropertyValue("CuttingMachineGroup", ref _CuttingMachineGroup, value);
            }
        }

        public decimal ProduceQuantity
        {
            get
            {
                return _ProduceQuantity;
            }
            set
            {
                SetPropertyValue("ProduceQuantity", ref _ProduceQuantity, value);
            }
        }

        public decimal Capacity
        {
            get
            {
                return _Capacity;
            }
            set
            {
                SetPropertyValue("Capacity", ref _Capacity, value);
            }
        }

        public decimal CapacityDay
        {
            get
            {
                return _CapacityDay;
            }
            set
            {
                SetPropertyValue("CapacityDay", ref _CapacityDay, value);
            }
        }

        public decimal StoreQuantity
        {
            get
            {
                return _StoreQuantity;
            }
            set
            {
                SetPropertyValue("StoreQuantity", ref _StoreQuantity, value);
            }
        }

        public decimal StoreDay
        {
            get
            {
                return _StoreDay;
            }
            set
            {
                SetPropertyValue("StoreDay", ref _StoreDay, value);
            }
        }

        public decimal PastProduction
        {
            get
            {
                return _PastProduction;
            }
            set
            {
                SetPropertyValue("PastProduction", ref _PastProduction, value);
            }
        }

        public decimal PastProductionDay
        {
            get
            {
                return _PastProductionDay;
            }
            set
            {
                SetPropertyValue("PastProductionDay", ref _PastProductionDay, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ThisMonthName
        {
            get
            {
                return _ThisMonthName;
            }
            set
            {
                SetPropertyValue("ThisMonthName", ref _ThisMonthName, value);
            }
        }

        public decimal ThisMonthProduction
        {
            get
            {
                return _ThisMonthProduction;
            }
            set
            {
                SetPropertyValue("ThisMonthProduction", ref _ThisMonthProduction, value);
            }
        }

        public decimal ThisMonthProductionDay
        {
            get
            {
                return _ThisMonthProductionDay;
            }
            set
            {
                SetPropertyValue("ThisMonthProductionDay", ref _ThisMonthProductionDay, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NextMonthName
        {
            get
            {
                return _NextMonthName;
            }
            set
            {
                SetPropertyValue("NextMonthName", ref _NextMonthName, value);
            }
        }

        public decimal NextMonthProduction
        {
            get
            {
                return _NextMonthProduction;
            }
            set
            {
                SetPropertyValue("NextMonthProduction", ref _NextMonthProduction, value);
            }
        }

        public decimal NextMontProductionDay
        {
            get
            {
                return _NextMontProductionDay;
            }
            set
            {
                SetPropertyValue("NextMontProductionDay", ref _NextMontProductionDay, value);
            }
        }

        public decimal NextProduction
        {
            get
            {
                return _NextProduction;
            }
            set
            {
                SetPropertyValue("NextProduction", ref _NextProduction, value);
            }
        }

        public decimal NextProductionDay
        {
            get
            {
                return _NextProductionDay;
            }
            set
            {
                SetPropertyValue("NextProductionDay", ref _NextProductionDay, value);
            }
        }
    }
}
