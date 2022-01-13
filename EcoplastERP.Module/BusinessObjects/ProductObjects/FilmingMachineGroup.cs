using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class FilmingMachineGroup : BaseObject
    {
        public FilmingMachineGroup(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _Capacity;
        private string _Name;

        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        public decimal Capacity
        {
            get
            {
                return _Capacity;
            }
            set
            {
                SetPropertyValue("Capacity", ref _Capacity, value);
            }
        }
    }
}
