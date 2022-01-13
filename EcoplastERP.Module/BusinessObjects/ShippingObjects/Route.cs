using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.SystemObjects;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Change_State")]
    [DefaultProperty("Name")]
    [NavigationItem("ShippingManagement")]
    public class Route : BaseObject
    {
        public Route(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            foreach (var item in Cities)
            {
                item.Route = this;
            }
        }
        // Fields...
        private decimal _ShippingCost;
        private string _Name;

        [RuleUniqueValue]
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

        public decimal ShippingCost
        {
            get
            {
                return _ShippingCost;
            }
            set
            {
                SetPropertyValue("ShippingCost", ref _ShippingCost, value);
            }
        }

        [Association("City-Routes")]
        public XPCollection<City> Cities
        {
            get { return GetCollection<City>("Cities"); }
        }
    }
}
