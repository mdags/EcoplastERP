using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Person")]
    [DefaultProperty("NameSurname")]
    [NavigationItem("PurchaseManagement")]
    public class Purchaser : BaseObject
    {
        public Purchaser(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private bool _DefaultPurchaser;
        private string _NameSurname;

        [RuleUniqueValue]
        [RuleRequiredField]
        public string NameSurname
        {
            get
            {
                return _NameSurname;
            }
            set
            {
                SetPropertyValue("NameSurname", ref _NameSurname, value);
            }
        }

        public bool DefaultPurchaser
        {
            get
            {
                return _DefaultPurchaser;
            }
            set
            {
                SetPropertyValue("DefaultPurchaser", ref _DefaultPurchaser, value);
            }
        }
    }
}
