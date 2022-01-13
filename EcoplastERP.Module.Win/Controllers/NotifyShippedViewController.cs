using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.Win.Forms;
using EcoplastERP.Module.Win.UserForms;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class NotifyShippedViewController : ViewController
    {
        public NotifyShippedViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
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
                NotifyShippedUserControl userControl = cvi.Control as NotifyShippedUserControl;
                userControl.RefreshGrid();
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void NotifyShippedCompleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                NotifyShippedUserControl userControl = cvi.Control as NotifyShippedUserControl;
                if (userControl.gridView1.FocusedValue == null) return;
                NotifyShippedParametersForm form = new NotifyShippedParametersForm() { userControl = userControl };
                form.FormClosed += Form_FormClosed;
                form.ShowDialog();
            }
        }

        private void Form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                NotifyShippedUserControl userControl = cvi.Control as NotifyShippedUserControl;
                userControl.RefreshGrid();
            }
        }
    }
}
