using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DomainComponent]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]
    public class AssignPurchaseParameters
    {
        public AssignPurchaseParameters() { }
        public Purchaser Purchaser { get; set; }
    }
}
