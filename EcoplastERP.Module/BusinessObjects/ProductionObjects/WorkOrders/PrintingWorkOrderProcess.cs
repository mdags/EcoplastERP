using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("FormNumber")]
    [NavigationItem("ProductionManagement")]
    public class PrintingWorkOrderProcess : BaseObject
    {
        public PrintingWorkOrderProcess(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FormNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            FormDate = Helpers.GetSystemDate(Session);
            ShiftStart shiftStart = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("Active = true"));
            if (shiftStart != null) WorkShift = shiftStart.WorkShift;
        }
        // Fields...
        private string _Note;
        private decimal _StripBladePress8;
        private decimal _StripBladePress7;
        private decimal _StripBladePress6;
        private decimal _StripBladePress5;
        private decimal _StripBladePress4;
        private decimal _StripBladePress3;
        private decimal _StripBladePress2;
        private decimal _StripBladePress1;
        private int _InkViscosity8;
        private int _InkViscosity7;
        private int _InkViscosity6;
        private int _InkViscosity5;
        private int _InkViscosity4;
        private int _InkViscosity3;
        private int _InkViscosity2;
        private int _InkViscosity1;
        private string _PrintingBlockColor8;
        private string _PrintingBlockColor7;
        private string _PrintingBlockColor6;
        private string _PrintingBlockColor5;
        private string _PrintingBlockColor4;
        private string _PrintingBlockColor3;
        private string _PrintingBlockColor2;
        private string _PrintingBlockColor1;
        private int _LineSpeed;
        private int _ChillerTemp;
        private int _TunnelDryingTemp;
        private int _DrumTemp;
        private string _InkType;
        private int _WrapperTension;
        private WorkShift _WorkShift;
        private DateTime _FormDate;
        private int _FormNumber;
        private PrintingWorkOrder _PrintingWorkOrder;

        [RuleRequiredField]
        [Association("PrintingWorkOrder-PrintingWorkOrderProcess")]
        public PrintingWorkOrder PrintingWorkOrder
        {
            get
            {
                return _PrintingWorkOrder;
            }
            set
            {
                SetPropertyValue("PrintingWorkOrder", ref _PrintingWorkOrder, value);
            }
        }

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

        [RuleRequiredField]
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

        public int WrapperTension
        {
            get
            {
                return _WrapperTension;
            }
            set
            {
                SetPropertyValue("WrapperTension", ref _WrapperTension, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InkType
        {
            get
            {
                return _InkType;
            }
            set
            {
                SetPropertyValue("InkType", ref _InkType, value);
            }
        }

        public int DrumTemp
        {
            get
            {
                return _DrumTemp;
            }
            set
            {
                SetPropertyValue("DrumTemp", ref _DrumTemp, value);
            }
        }

        public int TunnelDryingTemp
        {
            get
            {
                return _TunnelDryingTemp;
            }
            set
            {
                SetPropertyValue("TunnelDryingTemp", ref _TunnelDryingTemp, value);
            }
        }

        public int ChillerTemp
        {
            get
            {
                return _ChillerTemp;
            }
            set
            {
                SetPropertyValue("ChillerTemp", ref _ChillerTemp, value);
            }
        }

        public int LineSpeed
        {
            get
            {
                return _LineSpeed;
            }
            set
            {
                SetPropertyValue("LineSpeed", ref _LineSpeed, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor1
        {
            get
            {
                return _PrintingBlockColor1;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor1", ref _PrintingBlockColor1, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor2
        {
            get
            {
                return _PrintingBlockColor2;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor2", ref _PrintingBlockColor2, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor3
        {
            get
            {
                return _PrintingBlockColor3;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor3", ref _PrintingBlockColor3, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor4
        {
            get
            {
                return _PrintingBlockColor4;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor4", ref _PrintingBlockColor4, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor5
        {
            get
            {
                return _PrintingBlockColor5;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor5", ref _PrintingBlockColor5, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor6
        {
            get
            {
                return _PrintingBlockColor6;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor6", ref _PrintingBlockColor6, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor7
        {
            get
            {
                return _PrintingBlockColor7;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor7", ref _PrintingBlockColor7, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PrintingBlockColor8
        {
            get
            {
                return _PrintingBlockColor8;
            }
            set
            {
                SetPropertyValue("PrintingBlockColor8", ref _PrintingBlockColor8, value);
            }
        }

        public int InkViscosity1
        {
            get
            {
                return _InkViscosity1;
            }
            set
            {
                SetPropertyValue("InkViscosity1", ref _InkViscosity1, value);
            }
        }

        public int InkViscosity2
        {
            get
            {
                return _InkViscosity2;
            }
            set
            {
                SetPropertyValue("InkViscosity2", ref _InkViscosity2, value);
            }
        }

        public int InkViscosity3
        {
            get
            {
                return _InkViscosity3;
            }
            set
            {
                SetPropertyValue("InkViscosity3", ref _InkViscosity3, value);
            }
        }

        public int InkViscosity4
        {
            get
            {
                return _InkViscosity4;
            }
            set
            {
                SetPropertyValue("InkViscosity4", ref _InkViscosity4, value);
            }
        }

        public int InkViscosity5
        {
            get
            {
                return _InkViscosity5;
            }
            set
            {
                SetPropertyValue("InkViscosity5", ref _InkViscosity5, value);
            }
        }

        public int InkViscosity6
        {
            get
            {
                return _InkViscosity6;
            }
            set
            {
                SetPropertyValue("InkViscosity6", ref _InkViscosity6, value);
            }
        }

        public int InkViscosity7
        {
            get
            {
                return _InkViscosity7;
            }
            set
            {
                SetPropertyValue("InkViscosity7", ref _InkViscosity7, value);
            }
        }

        public int InkViscosity8
        {
            get
            {
                return _InkViscosity8;
            }
            set
            {
                SetPropertyValue("InkViscosity8", ref _InkViscosity8, value);
            }
        }

        public decimal StripBladePress1
        {
            get
            {
                return _StripBladePress1;
            }
            set
            {
                SetPropertyValue("StripBladePress1", ref _StripBladePress1, value);
            }
        }

        public decimal StripBladePress2
        {
            get
            {
                return _StripBladePress2;
            }
            set
            {
                SetPropertyValue("StripBladePress2", ref _StripBladePress2, value);
            }
        }

        public decimal StripBladePress3
        {
            get
            {
                return _StripBladePress3;
            }
            set
            {
                SetPropertyValue("StripBladePress3", ref _StripBladePress3, value);
            }
        }

        public decimal StripBladePress4
        {
            get
            {
                return _StripBladePress4;
            }
            set
            {
                SetPropertyValue("StripBladePress4", ref _StripBladePress4, value);
            }
        }

        public decimal StripBladePress5
        {
            get
            {
                return _StripBladePress5;
            }
            set
            {
                SetPropertyValue("StripBladePress5", ref _StripBladePress5, value);
            }
        }

        public decimal StripBladePress6
        {
            get
            {
                return _StripBladePress6;
            }
            set
            {
                SetPropertyValue("StripBladePress6", ref _StripBladePress6, value);
            }
        }

        public decimal StripBladePress7
        {
            get
            {
                return _StripBladePress7;
            }
            set
            {
                SetPropertyValue("StripBladePress7", ref _StripBladePress7, value);
            }
        }

        public decimal StripBladePress8
        {
            get
            {
                return _StripBladePress8;
            }
            set
            {
                SetPropertyValue("StripBladePress8", ref _StripBladePress8, value);
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
