using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using EcoplastERP.Module.BusinessObjects.QualityObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class ProductionViewController : ViewController
    {
        public ProductionViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
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

        private void CreateFilmingQualityTestAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Production production = (Production)objectSpace.GetObject(e.CurrentObject);

            FilmingQualityTest filmingQualityTest = objectSpace.CreateObject<FilmingQualityTest>();
            filmingQualityTest.Barcode = production.Barcode;
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, filmingQualityTest);
        }
    }
}
