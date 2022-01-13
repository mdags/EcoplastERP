using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [NavigationItem("QualityManagement")]
    public class ContactAnalysisCertificate : BaseObject
    {
        public ContactAnalysisCertificate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private Reciept _Reciept;
        private SalesOrderDetail _SalesOrderDetail;

        [RuleRequiredField]
        public SalesOrderDetail SalesOrderDetail
        {
            get
            {
                return _SalesOrderDetail;
            }
            set
            {
                SetPropertyValue("SalesOrderDetail", ref _SalesOrderDetail, value);
            }
        }

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
        public XPCollection<ContactAnalysisCertificateDetail> ContactAnalysisCertificateDetails
        {
            get { return GetCollection<ContactAnalysisCertificateDetail>("ContactAnalysisCertificateDetails"); }
        }
    }
}
