using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_State")]
    [DefaultProperty("Code")]
    [NavigationItem("ProductManagement")]
    public class Unit : BaseObject
    {
        public Unit(Session session)
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
            Unit defaultUnit = Session.FindObject<Unit>(new BinaryOperator("Default", true));
            if (defaultUnit != null) defaultUnit.Default = false;
        }
        // Fields...
        private bool _Default;
        private string _Name;
        private string _Code;

        [NonCloneable]
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

        public bool Default
        {
            get
            {
                return _Default;
            }
            set
            {
                SetPropertyValue("Default", ref _Default, value);
            }
        }
    }
}
