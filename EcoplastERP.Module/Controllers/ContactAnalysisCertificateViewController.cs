using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.QualityObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class ContactAnalysisCertificateViewController : ViewController
    {
        public ContactAnalysisCertificateViewController()
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

        private void CreateAnalysisCertificateAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            const string salesOrderDetail_ListView = "SalesOrderDetail_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(SalesOrderDetail), salesOrderDetail_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(salesOrderDetail_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController salesOrderDetailDialogController = Application.CreateController<DialogController>();
            salesOrderDetailDialogController.Accepting += SalesOrderDetailDialogController_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(salesOrderDetailDialogController);
        }

        private void SalesOrderDetailDialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            SalesOrderDetail salesOrderDetail = (SalesOrderDetail)objectSpace.GetObject(e.AcceptActionArgs.SelectedObjects[0]);

            ContactAnalysisCertificate contactAnalysisCertificate = objectSpace.CreateObject<ContactAnalysisCertificate>();
            contactAnalysisCertificate.SalesOrderDetail = salesOrderDetail;
            FilmingWorkOrder filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("SalesOrderDetail", salesOrderDetail));
            if (filmingWorkOrder != null)
            {
                contactAnalysisCertificate.Reciept = filmingWorkOrder.Reciept;

                XPCollection<QuantitativeAttribute> QuantitativeAttributeList = new XPCollection<QuantitativeAttribute>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Station.Code = ?", "Eco2"));
                foreach (QuantitativeAttribute attribute in QuantitativeAttributeList)
                {
                    ContactAnalysisCertificateDetail detail = objectSpace.CreateObject<ContactAnalysisCertificateDetail>();
                    detail.ContactAnalysisCertificate = contactAnalysisCertificate;
                    detail.QuantitativeAttribute = attribute;
                    AnalysisCertificateDetail analysisCertificateDetail = objectSpace.FindObject<AnalysisCertificateDetail>(CriteriaOperator.Parse("AnalysisCertificate.Reciept = ? and QuantitativeAttribute = ?", filmingWorkOrder.Reciept, attribute));
                    detail.AnalysisCertificateDetail = analysisCertificateDetail;
                    detail.CertificateValue = analysisCertificateDetail.Value;
                    detail.MinTestValue = Convert.ToDecimal(objectSpace.Evaluate(typeof(FilmingQualityTestQuantitative), CriteriaOperator.Parse("Min(Value)"), CriteriaOperator.Parse("FilmingQualityTest.SalesOrderDetail = ? and QuantitativeAttribute = ?", salesOrderDetail, attribute)));
                    detail.MaxTestValue = Convert.ToDecimal(objectSpace.Evaluate(typeof(FilmingQualityTestQuantitative), CriteriaOperator.Parse("Max(Value)"), CriteriaOperator.Parse("FilmingQualityTest.SalesOrderDetail = ? and QuantitativeAttribute = ?", salesOrderDetail, attribute)));
                    contactAnalysisCertificate.ContactAnalysisCertificateDetails.Add(detail);
                }

                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contactAnalysisCertificate);
            }
            else throw new UserFriendlyException("Çekim Üretim Siparişi bulunamadı.");
        }
    }
}
