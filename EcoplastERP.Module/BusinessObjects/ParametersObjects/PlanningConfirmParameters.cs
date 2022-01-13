using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DomainComponent]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]

    public class PlanningConfirmParameters
    {
        public PlanningConfirmParameters()
        {
            DeliveryDate = DateTime.Now.AddDays(2);
        }
        public DateTime DeliveryDate { get; set; }
    }
}
