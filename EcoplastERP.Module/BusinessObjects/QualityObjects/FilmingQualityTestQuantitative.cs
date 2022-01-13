using System.Drawing;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Totals_Row")]
    [DefaultProperty("FilmingQualityTest.Barcode")]
    [NavigationItem(false)]
    public class FilmingQualityTestQuantitative : BaseObject
    {
        public FilmingQualityTestQuantitative(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private AnalysisCertificateDetail _AnalysisCertificateDetail;
        private decimal _Value;
        private decimal _CertificateValue;
        private QuantitativeAttribute _QuantitativeAttribute;

        [Association]
        public FilmingQualityTest FilmingQualityTest { get; set; }

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

        [Appearance("FilmingQualityTestQuantitative.CertificateValue", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
                decimal value = 0;
                if (AnalysisCertificateDetail.Value == 0) value = CertificateValue - AnalysisCertificateDetail.ContactToleranceMin;
                else value = AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ContactToleranceMin : 0;
                return value;
                //return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ContactToleranceMin : 0;
            }
        }

        public decimal ContactMaxValue
        {
            get
            {
                decimal value = 0;
                if (AnalysisCertificateDetail.Value == 0) value = CertificateValue + AnalysisCertificateDetail.ContactToleranceMax;
                else value = AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ContactToleranceMax : 0;
                return value;
                //return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ContactToleranceMax : 0;
            }
        }

        public decimal ProductionMinValue
        {
            get
            {
                decimal value = 0;
                if (AnalysisCertificateDetail.Value == 0) value = CertificateValue - AnalysisCertificateDetail.ProductionToleranceMin;
                else value = AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ProductionToleranceMin : 0;
                return value;
                //return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ProductionToleranceMin : 0;
            }
        }

        public decimal ProductionMaxValue
        {
            get
            {
                decimal value = 0;
                if (AnalysisCertificateDetail.Value == 0) value = CertificateValue + AnalysisCertificateDetail.ProductionToleranceMax;
                else value = AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ProductionToleranceMax : 0;
                return value;
                //return AnalysisCertificateDetail != null ? AnalysisCertificateDetail.ProductionToleranceMax : 0;
            }
        }

        [ImmediatePostData]
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

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
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

        [Appearance("FilmingQualityTestQuantitative_DetailView_Valid", AppearanceItemType = "ViewItem", TargetItems = "CertificateValue", BackColor = "LawnGreen", FontColor = "Black", FontStyle = FontStyle.Bold)]
        public bool ValidRuleMethod()
        {
            if (Value == CertificateValue) return true;
            else return false;
        }
        [Appearance("FilmingQualityTestQuantitative_ListView_Invalid", AppearanceItemType = "ViewItem", TargetItems = "CertificateValue", BackColor = "Tomato", FontColor = "Black", FontStyle = FontStyle.Bold)]
        public bool InValidRuleMethod()
        {
            if (Value < ProductionMinValue || Value > ProductionMaxValue) return true;
            else return false;
        }
        [Appearance("FilmingQualityTestQuantitative_ListView_Critic", AppearanceItemType = "ViewItem", TargetItems = "CertificateValue", BackColor = "LemonChiffon", FontColor = "Black", FontStyle = FontStyle.Bold)]
        public bool CriticRuleMethod()
        {
            if (Value >= ProductionMinValue && Value <= ProductionMaxValue) return true;
            else return false;
        }
    }
}
