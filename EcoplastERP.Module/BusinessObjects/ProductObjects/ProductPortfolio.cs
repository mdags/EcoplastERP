using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    public class ProductPortfolio : FileAttachmentBase
    {
        public ProductPortfolio(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Association]
        public Product Product { get; set; }
    }
}
