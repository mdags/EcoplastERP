using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class ProductKind : BaseObject
    {
        public ProductKind(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private CuttingMachineGroup _CuttingMachineGroup;
        private FilmingMachineGroup _FilmingMachineGroup;
        private ProductKindGroup _ProductKindGroup;
        private string _Name;
        private string _Code;
        private ProductType _ProductType;

        public ProductType ProductType
        {
            get
            {
                return _ProductType;
            }
            set
            {
                SetPropertyValue("ProductType", ref _ProductType, value);
            }
        }

        [RuleUniqueValue]
        [RuleRequiredField]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
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

        public ProductKindGroup ProductKindGroup
        {
            get
            {
                return _ProductKindGroup;
            }
            set
            {
                SetPropertyValue("ProductKindGroup", ref _ProductKindGroup, value);
            }
        }

        public FilmingMachineGroup FilmingMachineGroup
        {
            get
            {
                return _FilmingMachineGroup;
            }
            set
            {
                SetPropertyValue("FilmingMachineGroup", ref _FilmingMachineGroup, value);
            }
        }

        public CuttingMachineGroup CuttingMachineGroup
        {
            get
            {
                return _CuttingMachineGroup;
            }
            set
            {
                SetPropertyValue("CuttingMachineGroup", ref _CuttingMachineGroup, value);
            }
        }
    }
}
