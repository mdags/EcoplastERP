using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    public class MaintenanceWorkOrderPortfolio : FileAttachmentBase
    {
        public MaintenanceWorkOrderPortfolio(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Association]
        public MaintenanceWorkOrder MaintenanceWorkOrder { get; set; }
    }
}
