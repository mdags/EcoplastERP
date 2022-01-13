using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using EcoplastERP.Module.Win.UserForms;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class SalesOrderAnalysisViewController : ViewController
    {
        public SalesOrderAnalysisViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            
            Frame.GetController<RefreshController>().RefreshAction.Execute += RefreshAction_Execute;
        }

        private void RefreshAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                SalesOrderAnalysisUserControl userControl = cvi.Control as SalesOrderAnalysisUserControl;
                if (userControl.sleSalesOrder.EditValue != null)
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    userControl.RefreshGrid(userControl.sleSalesOrder.EditValue.ToString());
                    SalesOrder salesOrder = objectSpace.FindObject<SalesOrder>(new BinaryOperator("OrderNumber", userControl.sleSalesOrder.EditValue.ToString()));
                    if (salesOrder != null)
                    {
                        if (salesOrder.Contact != null)
                        {
                            userControl.txtContact.Text = salesOrder.Contact.Name;
                            userControl.txtShippingAddress.Text = salesOrder.ShippingContact != null ? salesOrder.ShippingContact.Address : string.Empty;
                            if (salesOrder.Contact.City != null) userControl.txtCity.Text = salesOrder.Contact.City.Name;
                        }
                        userControl.txtSalesOrderType.Text =
                              salesOrder.SalesOrderType == SalesOrderType.CustomerOrder ? "Standart Siparişi" :
                              salesOrder.SalesOrderType == SalesOrderType.PlanningOrder ? "Planlama Siparişi" :
                              salesOrder.SalesOrderType == SalesOrderType.RegeneratedOrder ? "Rejenere Siparişi" :
                              salesOrder.SalesOrderType == SalesOrderType.ExportingOrder ? "İhracat Sipariş" :
                              salesOrder.SalesOrderType == SalesOrderType.ExportRegisteredOrder ? "İhraç Kayıtlı Sipariş" : string.Empty;
                        if (salesOrder.OrderDate != null) userControl.txtSalesOrderDate.Text = salesOrder.OrderDate.ToShortDateString();
                    }
                }
            }
        }
    }
}
