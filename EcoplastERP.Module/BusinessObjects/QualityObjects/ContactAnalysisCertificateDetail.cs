using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("QuantitativeAttribute.Name")]
    [NavigationItem(false)]
    public class ContactAnalysisCertificateDetail : BaseObject
    {
        public ContactAnalysisCertificateDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _MaxQualityValue;
        private decimal _MinQualityValue;
        private decimal _MaxTestValue;
        private decimal _MinTestValue;
        private decimal _CertificateValue;
        private AnalysisCertificateDetail _AnalysisCertificateDetail;
        private QuantitativeAttribute _QuantitativeAttribute;

        [Association]
        public ContactAnalysisCertificate ContactAnalysisCertificate { get; set; }

        [RuleRequiredField]
        [Appearance("ContactAnalysisCertificateDetail.QuantitativeAttribute", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("ContactAnalysisCertificateDetail.AnalysisCertificateDetail", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public AnalysisCertificateDetail AnalysisCertificateDetail
        {
            get
            {
                return _AnalysisCertificateDetail;
            }
            set
            {
                SetPropertyValue("AnalysisCertificateDetail", ref _AnalysisCertificateDetail, value);
            }
        }

        [Appearance("ContactAnalysisCertificateDetail.CertificateValue", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal CertificateValue
        {
            get
            {
                return _CertificateValue;
            }
            set
            {
                SetPropertyValue("CertificateValue", ref _CertificateValue, value);
            }
        }

        public decimal ContactMinValue
        {
            get
            {
                return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ContactToleranceMin : 0;
            }
        }

        public decimal ContactMaxValue
        {
            get
            {
                return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ContactToleranceMax : 0;
            }
        }

        public decimal ProductionMinValue
        {
            get
            {
                return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ProductionToleranceMin : 0;
            }
        }

        public decimal ProductionMaxValue
        {
            get
            {
                return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ProductionToleranceMax : 0;
            }
        }

        [Appearance("ContactAnalysisCertificateDetail.MinTestValue", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal MinTestValue
        {
            get
            {
                return _MinTestValue;
            }
            set
            {
                SetPropertyValue("MinTestValue", ref _MinTestValue, value);
            }
        }

        [Appearance("ContactAnalysisCertificateDetail.MaxTestValue", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal MaxTestValue
        {
            get
            {
                return _MaxTestValue;
            }
            set
            {
                SetPropertyValue("MaxTestValue", ref _MaxTestValue, value);
            }
        }

        public decimal MinQualityValue
        {
            get
            {
                return _MinQualityValue;
            }
            set
            {
                SetPropertyValue("MinQualityValue", ref _MinQualityValue, value);
            }
        }

        public decimal MaxQualityValue
        {
            get
            {
                return _MaxQualityValue;
            }
            set
            {
                SetPropertyValue("MaxQualityValue", ref _MaxQualityValue, value);
            }
        }
    }
}
