using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("AnalysisCertificate.Name")]
    [NavigationItem(false)]
    public class AnalysisCertificateDetail : BaseObject
    {
        public AnalysisCertificateDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _ProductionToleranceMax;
        private decimal _ProductionToleranceMin;
        private decimal _ContactToleranceMax;
        private decimal _ContactToleranceMin;
        private decimal _Value;
        private QuantitativeAttribute _QuantitativeAttribute;
        private int _MaximumThickness;
        private int _MinimumThickness;

        [Association]
        public AnalysisCertificate AnalysisCertificate { get; set; }

        public int MinimumThickness
        {
            get
            {
                return _MinimumThickness;
            }
            set
            {
                SetPropertyValue("MinimumThickness", ref _MinimumThickness, value);
            }
        }

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public int MaximumThickness
        {
            get
            {
                return _MaximumThickness;
            }
            set
            {
                SetPropertyValue("MaximumThickness", ref _MaximumThickness, value);
            }
        }

        [RuleRequiredField]
        public QuantitativeAttribute QuantitativeAttribute
        {
            get
            {
                return _QuantitativeAttribute;
            }
            set
            {
                SetPropertyValue("QuantitativeAttribute", ref _QuantitativeAttribute, value);
            }
        }

        public decimal Value
        {
            get
            {
                return _Value;
            }
            set
            {
                SetPropertyValue("Value", ref _Value, value);
            }
        }

        public decimal ContactToleranceMin
        {
            get
            {
                return _ContactToleranceMin;
            }
            set
            {
                SetPropertyValue("ContactToleranceMin", ref _ContactToleranceMin, value);
            }
        }

        public decimal ContactToleranceMax
        {
            get
            {
                return _ContactToleranceMax;
            }
            set
            {
                SetPropertyValue("ContactToleranceMax", ref _ContactToleranceMax, value);
            }
        }

        public decimal ProductionToleranceMin
        {
            get
            {
                return _ProductionToleranceMin;
            }
            set
            {
                SetPropertyValue("ProductionToleranceMin", ref _ProductionToleranceMin, value);
            }
        }

        public decimal ProductionToleranceMax
        {
            get
            {
                return _ProductionToleranceMax;
            }
            set
            {
                SetPropertyValue("ProductionToleranceMax", ref _ProductionToleranceMax, value);
            }
        }
    }
}
