using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Production.Barcode")]
    [NavigationItem(false)]
    public class ProductionResource : BaseObject
    {
        public ProductionResource(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private ReadResourceMovement _ReadResourceMovement;
        private Production _ReadResourceProduction;

        [Association]
        public Production Production { get; set; }

        [RuleRequiredField]
        public Production ReadResourceProduction
        {
            get
            {
                return _ReadResourceProduction;
            }
            set
            {
                SetPropertyValue("ReadResourceProduction", ref _ReadResourceProduction, value);
            }
        }

        public ReadResourceMovement ReadResourceMovement
        {
            get
            {
                return _ReadResourceMovement;
            }
            set
            {
                SetPropertyValue("ReadResourceMovement", ref _ReadResourceMovement, value);
            }
        }
    }
}
