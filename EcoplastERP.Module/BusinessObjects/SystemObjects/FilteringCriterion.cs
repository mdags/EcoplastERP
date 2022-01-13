using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.SystemObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Filter")]
    [DefaultProperty("Description")]
    public class FilteringCriterion : BaseObject
    {
        public FilteringCriterion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        public string Description
        {
            get { return GetPropertyValue<string>("Description"); }
            set { SetPropertyValue<string>("Description", value); }
        }

        [ValueConverter(typeof(TypeToStringConverter)), ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        public Type ObjectType
        {
            get { return GetPropertyValue<Type>("ObjectType"); }
            set
            {
                SetPropertyValue<Type>("ObjectType", value);
                Criterion = String.Empty;
            }
        }

        [CriteriaOptions("ObjectType"), Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CriteriaPropertyEditor)]
        public string Criterion
        {
            get { return GetPropertyValue<string>("Criterion"); }
            set { SetPropertyValue<string>("Criterion", value); }
        }
    }
}
