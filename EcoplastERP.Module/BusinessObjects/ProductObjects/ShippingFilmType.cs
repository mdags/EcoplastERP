using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class ShippingFilmType : BaseObject
    {
        public ShippingFilmType(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private bool _ThicknessDoublePart;
        private string _Name;
        private string _Code;

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

        public bool ThicknessDoublePart
        {
            get
            {
                return _ThicknessDoublePart;
            }
            set
            {
                SetPropertyValue("ThicknessDoublePart", ref _ThicknessDoublePart, value);
            }
        }
    }
}
