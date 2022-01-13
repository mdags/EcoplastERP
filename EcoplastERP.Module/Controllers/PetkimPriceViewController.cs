using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class PetkimPriceViewController : ViewController
    {
        public PetkimPriceViewController()
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

        private void ImportPetkimPriceListAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel 97-2003 Files|*.xls;", Multiselect = false, Title = "Excel Dosyası Seçiniz..." };
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                IObjectSpace objectSpace = View is DevExpress.ExpressApp.ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
                DataTable dt = (DataTable)ExcelDataBaseHelper.OpenFile(openFileDialog.FileName);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Reciept filmCode = objectSpace.FindObject<Reciept>(new BinaryOperator("Code", dt.Rows[i][0].ToString()));
                    if (filmCode != null)
                    {
                        PetkimPrice petkimPrice = objectSpace.CreateObject<PetkimPrice>();
                        petkimPrice.FilmCode = filmCode;
                        petkimPrice.BeginDate = DateTime.Now;
                        petkimPrice.Price = string.IsNullOrEmpty(dt.Rows[i][1].ToString()) ? 0 : Convert.ToDecimal(dt.Rows[i][1]);
                    }
                }

                if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    objectSpace.CommitChanges();
                }
                if (View is DevExpress.ExpressApp.ListView)
                {
                    objectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                }
            }
        }
    }
}
