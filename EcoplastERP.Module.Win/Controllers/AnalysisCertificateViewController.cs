using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using EcoplastERP.Module.Win.Forms;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class AnalysisCertificateViewController : ViewController
    {
        public AnalysisCertificateViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
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

        private void ImportCertificateAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            QualityAnalysisImportForm form = new QualityAnalysisImportForm()
            {
                winApplication = Application
            };
            form.ShowDialog();
        }
    }
}
