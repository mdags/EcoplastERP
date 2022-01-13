using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.BusinessObjects.SystemObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    public class City : BaseObject
    {
        public City(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private Route _Route;
        private string _Name;
        private int _Code;
        private Country _Country;

        [RuleRequiredField]
        public Country Country
        {
            get
            {
                return _Country;
            }
            set
            {
                SetPropertyValue("Country", ref _Country, value);
            }
        }

        public int Code
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

        [VisibleInDetailView(false)]
        public Route Route
        {
            get
            {
                return _Route;
            }
            set
            {
                SetPropertyValue("Route", ref _Route, value);
            }
        }

        [Association("City-Routes")]
        public XPCollection<Route> Routes
        {
            get { return GetCollection<Route>("Routes"); }
        }
    }
}
