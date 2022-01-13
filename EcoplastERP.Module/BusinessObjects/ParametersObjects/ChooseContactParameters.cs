using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DefaultClassOptions]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]
    public class ChooseContactParameters : BaseObject
    {
        public ChooseContactParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public Contact Contact { get; set; }
    }
}
