using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.PlateArchiveObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Person")]
    [DefaultProperty("NameSurname")]
    [NavigationItem("PlateArchiveManagement")]
    public class Printmaker : BaseObject
    {
        public Printmaker(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
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
    }
}
