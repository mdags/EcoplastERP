using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class SalesOrderDetailListViewController : ObjectViewController<ListView, SalesOrderDetail>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            if (Helpers.IsUserInRole("Satış"))
            {
                Employee employee = View.ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
                if (employee != null)
                {
                    View.CollectionSource.Criteria["SalesOfficeFilter"] = CriteriaOperator.Parse("[SalesOrder.SalesOffice.Oid] = ?", employee.SalesOffice.Oid);
                }
            }
        }
    }
}
