using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Category")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class ProductGroup : BaseObject
    {
        public ProductGroup(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private bool _AutomaticPurchaseConfirmation;
        private bool _SupplierEvaluation;
        private string _Name;
        private string _Code;

        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        public bool SupplierEvaluation
        {
            get
            {
                return _SupplierEvaluation;
            }
            set
            {
                SetPropertyValue("SupplierEvaluation", ref _SupplierEvaluation, value);
            }
        }

        public bool AutomaticPurchaseConfirmation
        {
            get
            {
                return _AutomaticPurchaseConfirmation;
            }
            set
            {
                SetPropertyValue("AutomaticPurchaseConfirmation", ref _AutomaticPurchaseConfirmation, value);
            }
        }
    }
}
