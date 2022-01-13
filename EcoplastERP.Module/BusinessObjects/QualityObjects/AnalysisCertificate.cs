using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("ActionGroup_EasyTestRecorder")]
    [DefaultProperty("Name")]
    [NavigationItem("QualityManagement")]
    public class AnalysisCertificate : BaseObject
    {
        public AnalysisCertificate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private Reciept _Reciept;

        [RuleUniqueValue]
        [RuleRequiredField]
        public Reciept Reciept
        {
            get
            {
                return _Reciept;
            }
            set
            {
                SetPropertyValue("Reciept", ref _Reciept, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<AnalysisCertificateDetail> AnalysisCertificateDetails
        {
            get { return GetCollection<AnalysisCertificateDetail>("AnalysisCertificateDetails"); }
        }
    }
}
