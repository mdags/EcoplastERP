using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Organization")]
    [DefaultProperty("Code")]
    [NavigationItem("ProductionManagement")]
    public class Machine : BaseObject
    {
        public Machine(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Instructions;
        private string _LotNumber;
        private decimal _HeadDiameter;
        private decimal _AcceptableWastage;
        private decimal _Capacity;
        private CuttingMachineGroup _CuttingMachineGroup;
        private MachineGroup _MachineGroup;
        private Station _Station;
        private string _Name;
        private string _Code;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
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

        [ImmediatePostData]
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

        public MachineGroup MachineGroup
        {
            get
            {
                return _MachineGroup;
            }
            set
            {
                SetPropertyValue("MachineGroup", ref _MachineGroup, value);
            }
        }

        //[Appearance("MachinePart.MachineParts", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Station.Name not like '%Kesim%'")]
        public CuttingMachineGroup CuttingMachineGroup
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

        [RuleValueComparison("Machine.Capacity", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        [RuleValueComparison("Machine.AcceptableWastage", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal AcceptableWastage
        {
            get
            {
                return _AcceptableWastage;
            }
            set
            {
                SetPropertyValue("AcceptableWastage", ref _AcceptableWastage, value);
            }
        }

        public decimal HeadDiameter
        {
            get
            {
                return _HeadDiameter;
            }
            set
            {
                SetPropertyValue("HeadDiameter", ref _HeadDiameter, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LotNumber
        {
            get
            {
                return _LotNumber;
            }
            set
            {
                SetPropertyValue("LotNumber", ref _LotNumber, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Instructions
        {
            get
            {
                return _Instructions;
            }
            set
            {
                SetPropertyValue("Instructions", ref _Instructions, value);
            }
        }

        //[Appearance("MachinePart.MachineParts", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Station.Name not like '%Çekim%'")]
        [Association, Aggregated]
        public XPCollection<MachinePart> MachineParts
        {
            get { return GetCollection<MachinePart>("MachineParts"); }
        }

        [Association, Aggregated]
        public XPCollection<MachineCapacityTarget> MachineCapacityTargets
        {
            get { return GetCollection<MachineCapacityTarget>("MachineCapacityTargets"); }
        }
    }
}
