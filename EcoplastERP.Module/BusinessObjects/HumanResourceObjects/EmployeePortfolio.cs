using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.HumanResourceObjects
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    public class EmployeePortfolio : FileAttachmentBase
    {
        public EmployeePortfolio(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Association]
        public Employee Employee { get; set; }
    }
}
