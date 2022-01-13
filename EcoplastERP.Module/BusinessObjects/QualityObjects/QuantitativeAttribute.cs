using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Totals_Row")]
    [DefaultProperty("Name")]
    [NavigationItem("QualityManagement")]
    public class QuantitativeAttribute : BaseObject
    {
        public QuantitativeAttribute(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Unit;
        private string _TestDirection;
        private string _TestMethod;
        private string _Name;
        private Station _Station;

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

        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TestMethod
        {
            get
            {
                return _TestMethod;
            }
            set
            {
                SetPropertyValue("TestMethod", ref _TestMethod, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TestDirection
        {
            get
            {
                return _TestDirection;
            }
            set
            {
                SetPropertyValue("TestDirection", ref _TestDirection, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                SetPropertyValue("Unit", ref _Unit, value);
            }
        }
    }
}
