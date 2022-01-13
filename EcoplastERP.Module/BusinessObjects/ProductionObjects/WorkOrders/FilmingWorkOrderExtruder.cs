using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("FilmingWorkOrder.WorkOrderNumber")]
    [NavigationItem(false)]
    public class FilmingWorkOrderExtruder : BaseObject
    {
        public FilmingWorkOrderExtruder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private int _F2;
        private int _F1;
        private int _Z4;
        private int _Z3;
        private int _Z2;
        private int _Z1;
        private int _Amps;
        private int _Temperature;
        private int _Pressure;
        private int _Speed;
        private int _Yield;
        private MachinePart _MachinePart;

        [Association]
        public FilmingWorkOrder FilmingWorkOrder { get; set; }

        [RuleRequiredField]
        public MachinePart MachinePart
        {
            get
            {
                return _MachinePart;
            }
            set
            {
                SetPropertyValue("MachinePart", ref _MachinePart, value);
            }
        }

        public int Yield
        {
            get
            {
                return _Yield;
            }
            set
            {
                SetPropertyValue("Yield", ref _Yield, value);
            }
        }

        public int Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                SetPropertyValue("Speed", ref _Speed, value);
            }
        }

        public int Pressure
        {
            get
            {
                return _Pressure;
            }
            set
            {
                SetPropertyValue("Pressure", ref _Pressure, value);
            }
        }

        public int Temperature
        {
            get
            {
                return _Temperature;
            }
            set
            {
                SetPropertyValue("Temperature", ref _Temperature, value);
            }
        }

        public int Amps
        {
            get
            {
                return _Amps;
            }
            set
            {
                SetPropertyValue("Amps", ref _Amps, value);
            }
        }

        public int Z1
        {
            get
            {
                return _Z1;
            }
            set
            {
                SetPropertyValue("Z1", ref _Z1, value);
            }
        }

        public int Z2
        {
            get
            {
                return _Z2;
            }
            set
            {
                SetPropertyValue("Z2", ref _Z2, value);
            }
        }

        public int Z3
        {
            get
            {
                return _Z3;
            }
            set
            {
                SetPropertyValue("Z3", ref _Z3, value);
            }
        }

        public int Z4
        {
            get
            {
                return _Z4;
            }
            set
            {
                SetPropertyValue("Z4", ref _Z4, value);
            }
        }

        public int F1
        {
            get
            {
                return _F1;
            }
            set
            {
                SetPropertyValue("F1", ref _F1, value);
            }
        }

        public int F2
        {
            get
            {
                return _F2;
            }
            set
            {
                SetPropertyValue("F2", ref _F2, value);
            }
        }
    }
}
