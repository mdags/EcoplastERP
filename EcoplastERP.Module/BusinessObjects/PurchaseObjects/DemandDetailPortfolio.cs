using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    public class DemandDetailPortfolio : FileAttachmentBase
    {
        public DemandDetailPortfolio(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Association]
        public DemandDetail DemandDetail { get; set; }
    }
}
