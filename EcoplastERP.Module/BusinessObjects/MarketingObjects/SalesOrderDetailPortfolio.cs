using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    public class SalesOrderDetailPortfolio : FileAttachmentBase
    {
        public SalesOrderDetailPortfolio(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Association]
        public SalesOrderDetail SalesOrderDetail { get; set; }
    }
}
