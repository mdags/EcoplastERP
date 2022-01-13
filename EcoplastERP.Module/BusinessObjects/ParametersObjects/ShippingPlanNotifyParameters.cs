using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DomainComponent]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]

    public class ShippingPlanNotifyParametersObject
    {
        public ShippingPlanNotifyParametersObject()
        {
            SetupDate = DateTime.Now.Hour < 12 ? DateTime.Now : DateTime.Now.AddDays(1);
        }

        //[RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.LessThan, "12/12/05")]
        public DateTime SetupDate { get; set; }
    }
}
