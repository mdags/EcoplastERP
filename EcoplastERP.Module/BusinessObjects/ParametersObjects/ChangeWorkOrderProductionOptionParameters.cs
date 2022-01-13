using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DefaultClassOptions]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]
    public class ChangeWorkOrderProductionOptionParameters : BaseObject
    {
        public ChangeWorkOrderProductionOptionParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", WorkOrderNumber));
            var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var balloonFilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("WorkOrderNumber", WorkOrderNumber));
            var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", WorkOrderNumber));
            var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", WorkOrderNumber));
            var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", WorkOrderNumber));
            var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
            var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));

            if (filmingWorkOrder != null) filmingWorkOrder.ProductionOption = ProductionOption;
            else if (castFilmingWorkOrder != null) castFilmingWorkOrder.ProductionOption = ProductionOption;
            else if (castTransferingWorkOrder != null) castTransferingWorkOrder.ProductionOption = ProductionOption;
            else if (balloonFilmingWorkOrder != null) balloonFilmingWorkOrder.ProductionOption = ProductionOption;
            else if (printingWorkOrder != null) printingWorkOrder.ProductionOption = ProductionOption;
            else if (laminationWorkOrder != null) laminationWorkOrder.ProductionOption = ProductionOption;
            else if (slicingWorkOrder != null) slicingWorkOrder.ProductionOption = ProductionOption;
            else if (castSlicingWorkOrder != null) castSlicingWorkOrder.ProductionOption = ProductionOption;
            else if (cuttingWorkOrder != null) cuttingWorkOrder.ProductionOption = ProductionOption;
            else if (foldingWorkOrder != null) foldingWorkOrder.ProductionOption = ProductionOption;
            else if (balloonCuttingWorkOrder != null) balloonCuttingWorkOrder.ProductionOption = ProductionOption;
            else if (regeneratedWorkOrder != null) regeneratedWorkOrder.ProductionOption = ProductionOption;
            else if (castRegeneratedWorkOrder != null) castRegeneratedWorkOrder.ProductionOption = ProductionOption;
            else if (eco6WorkOrder != null) eco6WorkOrder.ProductionOption = ProductionOption;
            else if (eco6CuttingWorkOrder != null) eco6CuttingWorkOrder.ProductionOption = ProductionOption;
            else if (eco6LaminationWorkOrder != null) eco6LaminationWorkOrder.ProductionOption = ProductionOption;
            else throw new UserFriendlyException("Yanlýþ bir üretim sipariþi numarasý giriþi yaptýnýz. Bu numarada kayýtlý bir üretim sipariþi bulunamadý.");
        }

        [RuleRequiredField]
        public string WorkOrderNumber { get; set; }

        [RuleRequiredField]
        public decimal ProductionOption { get; set; }
    }
}
