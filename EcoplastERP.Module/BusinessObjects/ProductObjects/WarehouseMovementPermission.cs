using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security.Strategy;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Security_Permission")]
    [DefaultProperty("SecuritySystemUser.UserName")]
    [NavigationItem(false)]
    public class WarehouseMovementPermission : BaseObject
    {
        public WarehouseMovementPermission(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private SecuritySystemUser _SecuritySystemUser;

        [Association]
        public Warehouse Warehouse { get; set; }

        [RuleRequiredField]
        public SecuritySystemUser SecuritySystemUser
        {
            get
            {
                return _SecuritySystemUser;
            }
            set
            {
                SetPropertyValue("SecuritySystemUser", ref _SecuritySystemUser, value);
            }
        }
    }
}
