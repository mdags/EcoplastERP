using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_User")]
    [DefaultProperty("UserName")]
    [NavigationItem("ProductManagement")]
    public class WarehouseTransferUser : BaseObject
    {
        public WarehouseTransferUser(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Password;
        private string _UserName;
        private Employee _Employee;

        [RuleRequiredField]
        public Employee Employee
        {
            get
            {
                return _Employee;
            }
            set
            {
                SetPropertyValue("Employee", ref _Employee, value);
            }
        }

        [RuleRequiredField]
        [Size(16)]
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                SetPropertyValue("UserName", ref _UserName, value);
            }
        }

        [RuleRequiredField]
        [Size(16)]
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                SetPropertyValue("Password", ref _Password, value);
            }
        }
    }
}
