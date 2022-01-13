using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("Contact.Name")]
    [NavigationItem("QualityManagement")]
    public class ConfirmedSupplier : BaseObject
    {
        public ConfirmedSupplier(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Year = Helpers.GetSystemDate(Session).Year.ToString();
            Period = Helpers.GetSystemDate(Session).Month >= 1 & Helpers.GetSystemDate(Session).Month <= 6 ? Period.First : Period.Second;
        }
        // Fields...
        private int _Score;
        private SupplierClass _SupplierClass;
        private Contact _Contact;
        private Period _Period;
        private string _Year;

        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Year
        {
            get
            {
                return _Year;
            }
            set
            {
                SetPropertyValue("Year", ref _Year, value);
            }
        }

        [RuleRequiredField]
        public Period Period
        {
            get
            {
                return _Period;
            }
            set
            {
                SetPropertyValue("Period", ref _Period, value);
            }
        }

        [RuleRequiredField]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
            }
        }

        [RuleRequiredField]
        public SupplierClass SupplierClass
        {
            get
            {
                return _SupplierClass;
            }
            set
            {
                SetPropertyValue("SupplierClass", ref _SupplierClass, value);
            }
        }

        public int Score
        {
            get
            {
                return _Score;
            }
            set
            {
                SetPropertyValue("Score", ref _Score, value);
            }
        }

        //[Action(PredefinedCategory.View, Caption = "Create Confirmed Supplier List", AutoCommit = true, ImageName = "Action_Grant", ToolTip = "This operation will be exit department the selected employee.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        //public void CreateConfirmedSupplierList()
        //{
        //    XPCollection confirmedSuppliers = new XPCollection(Session, typeof(ConfirmedSupplier), CriteriaOperator.Parse("Year = ? and Period = ?", DateTime.Now.Year.ToString(), DateTime.Now.Month >= 1 & DateTime.Now.Month <= 6 ? Module.Period.First : Module.Period.Second));
        //    foreach (ConfirmedSupplier item in confirmedSuppliers)
        //    {
        //        item.Delete();
        //    }

        //    var beginDate = DateTime.Now.Month >= 1 & DateTime.Now.Month <= 6 ? new DateTime(DateTime.Now.Year, 1, 1) : new DateTime(DateTime.Now.Year, 7, 1);
        //    var enddDate = DateTime.Now.Month >= 1 & DateTime.Now.Month <= 6 ? new DateTime(DateTime.Now.Year, 6, 30) : new DateTime(DateTime.Now.Year, 12, 31);
        //    XPQuery<SupplierEvaluation> supplierEvaluations = new XPQuery<SupplierEvaluation>(Session);
        //    var list = from t in supplierEvaluations
        //               where (t.EvaluationDate >= beginDate && t.EvaluationDate <= enddDate)
        //               group t by t.Contact into gt
        //               select gt;

        //    foreach (SupplierEvaluation item in list)
        //    {
        //        XPQuery<SupplierInspection> supplierInspection = new XPQuery<SupplierInspection>(Session);
        //        int qualityManagementSystemScore = Convert.ToInt32((from t in supplierInspection
        //                                                            where t.Contact == item.Contact
        //                                                            orderby t.InspectionDate descending
        //                                                            select t.QualityManagementSystemScore).Take(1));
        //        int foodSafetyScore = Convert.ToInt32((from t in supplierInspection
        //                                               where t.Contact == item.Contact
        //                                               orderby t.InspectionDate descending
        //                                               select t.FoodSafetyScore).Take(1));

        //        int score = (int)Session.Evaluate(typeof(SupplierEvaluation), CriteriaOperator.Parse("Avg(Quality)"), CriteriaOperator.Parse("EvaluationDate >= ? and EvaluationDate <= ?", beginDate, enddDate)) +
        //            (int)Session.Evaluate(typeof(SupplierEvaluation), CriteriaOperator.Parse("Avg(Delivery)"), CriteriaOperator.Parse("EvaluationDate >= ? and EvaluationDate <= ?", beginDate, enddDate)) +
        //            (int)Session.Evaluate(typeof(SupplierEvaluation), CriteriaOperator.Parse("Avg(Price)"), CriteriaOperator.Parse("EvaluationDate >= ? and EvaluationDate <= ?", beginDate, enddDate)) + qualityManagementSystemScore + foodSafetyScore;
        //        var newConfirmedSuppliers = new ConfirmedSupplier(Session)
        //        {
        //            Contact = item.Contact,
        //            Score = score,
        //            SupplierClass = Session.FindObject<SupplierClass>(CriteriaOperator.Parse("Score >= ?", score))
        //        };
        //    }
        //}
    }
}
