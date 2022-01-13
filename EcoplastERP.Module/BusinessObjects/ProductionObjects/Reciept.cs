using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("Code")]
    [NavigationItem("ProductionManagement")]
    public class Reciept : BaseObject
    {
        public Reciept(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private WastageKind _WastageKind;
        private FilmKind _FilmKind;
        private decimal _Density;
        private Station _Station;
        private string _Code;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
        public Station Station
        {
            get
            {
                return _Station;
            }
            set
            {
                SetPropertyValue("Station", ref _Station, value);
            }
        }

        public decimal Density
        {
            get
            {
                return _Density;
            }
            set
            {
                SetPropertyValue("Density", ref _Density, value);
            }
        }

        public FilmKind FilmKind
        {
            get
            {
                return _FilmKind;
            }
            set
            {
                SetPropertyValue("FilmKind", ref _FilmKind, value);
            }
        }

        public WastageKind WastageKind
        {
            get
            {
                return _WastageKind;
            }
            set
            {
                SetPropertyValue("WastageKind", ref _WastageKind, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<RecieptDetail> RecieptDetails
        {
            get { return GetCollection<RecieptDetail>("RecieptDetails"); }
        }

        [Action(PredefinedCategory.View, Caption = "Hammadde Deðiþtir", AutoCommit = true, ImageName = "Action_Printing_PageSetup", SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        public void ChangeRate(ChangeRecieptProductParametersObject parameters)
        {
            XPCollection recieptDetailList = new XPCollection(Session, typeof(RecieptDetail), new BinaryOperator("Product", parameters.OldProduct.Oid));
            if (recieptDetailList != null)
            {
                foreach (RecieptDetail item in recieptDetailList)
                {
                    item.Product = Session.GetObjectByKey<Product>(parameters.Product.Oid);
                    if (parameters.Rate > 0) item.Rate = parameters.Rate;
                }
            }

            XPCollection recieptFilmingWorkOrderList = new XPCollection(Session, typeof(FilmingWorkOrder), new BinaryOperator("Reciept", Oid));
            if (recieptFilmingWorkOrderList != null)
            {
                foreach (FilmingWorkOrder item in recieptFilmingWorkOrderList)
                {
                    foreach (var obj in item.FilmingWorkOrderReciepts)
                    {
                        if (obj.Product.Oid == parameters.OldProduct.Oid)
                        {
                            obj.Product = Session.GetObjectByKey<Product>(parameters.Product.Oid);
                            if (parameters.Rate > 0) obj.WorkOrderRate = parameters.Rate;
                        }
                    }
                }
            }
            XPCollection cuttingWorkOrderList = new XPCollection(Session, typeof(CuttingWorkOrder), new BinaryOperator("Reciept", Oid));
            if (cuttingWorkOrderList != null)
            {
                foreach (CuttingWorkOrder item in cuttingWorkOrderList)
                {
                    foreach (var obj in item.CuttingWorkOrderReciepts)
                    {
                        if (obj.Product == parameters.OldProduct)
                        {
                            obj.Product = Session.GetObjectByKey<Product>(parameters.Product.Oid);
                            if (parameters.Rate > 0) obj.Rate = parameters.Rate;
                        }
                    }
                }
            }
            XPCollection regeneratedWorkOrderList = new XPCollection(Session, typeof(RegeneratedWorkOrder), new BinaryOperator("Reciept", Oid));
            if (regeneratedWorkOrderList != null)
            {
                foreach (RegeneratedWorkOrder item in regeneratedWorkOrderList)
                {
                    foreach (var obj in item.RegeneratedWorkOrderReciepts)
                    {
                        if (obj.Product == parameters.OldProduct)
                        {
                            obj.Product = Session.GetObjectByKey<Product>(parameters.Product.Oid);
                            if (parameters.Rate > 0) obj.Rate = parameters.Rate;
                        }
                    }
                }
            }
        }
    }

    [NonPersistent]
    public class ChangeRecieptProductParametersObject
    {
        public ChangeRecieptProductParametersObject()
        {

        }
        public Product OldProduct { get; set; }
        public Product Product { get; set; }
        public decimal Rate { get; set; }
    }
}
