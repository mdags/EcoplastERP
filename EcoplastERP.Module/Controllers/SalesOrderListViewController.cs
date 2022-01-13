using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class SalesOrderListViewController : ObjectViewController<ListView, SalesOrder>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            if (Helpers.IsUserInRole("Satış"))
            {
                Employee employee = View.ObjectSpace.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
                if (employee != null)
                {
                    View.CollectionSource.Criteria["SalesOfficeFilter"] = CriteriaOperator.Parse("SalesOffice = ?", employee.SalesOffice);
                }
            }
        }
    }
}
