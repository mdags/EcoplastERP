using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [NonPersistent]
    [DefaultClassOptions]
    [ImageName("Action_Change_State")]
    [DefaultProperty("SalesOrderDetail.OrderNumber")]
    [NavigationItem(false)]
    public class TransferStoreParameters : BaseObject
    {
        public TransferStoreParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        [RuleRequiredField]
        public SalesOrderDetail SalesOrderDetail { get; set; }
        [RuleRequiredField]
        public Warehouse Warehouse { get; set; }
    }
}
