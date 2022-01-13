using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Opportunity")]
    [DefaultProperty("OrderNumber")]
    [NavigationItem("MarketingManagement")]
    public class PetkimPrice : BaseObject
    {
        public PetkimPrice(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            BeginDate = Helpers.GetSystemDate(Session);
            EndDate = new DateTime(2099, 12, 31);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            PetkimPrice petkimPrice = Session.FindObject<PetkimPrice>(CriteriaOperator.Parse("FilmCode = ? and EndDate = ?", FilmCode, new DateTime(2099, 12, 31)));
            DateTime endDate = this.BeginDate.AddDays(-1);
            if (petkimPrice != null)
            {
                if (petkimPrice.EndDate == new DateTime(2099, 12, 31))
                {
                    petkimPrice.EndDate = endDate;
                }
            }
        }
        // Fields...
        private decimal _Price;
        private DateTime _EndDate;
        private DateTime _BeginDate;
        private Reciept _FilmCode;
        //private MaterialType _MaterialType;

        public Reciept FilmCode
        {
            get
            {
                return _FilmCode;
            }
            set
            {
                SetPropertyValue("FilmCode", ref _FilmCode, value);
            }
        }

        //public MaterialType MaterialType
        //{
        //    get
        //    {
        //        return _MaterialType;
        //    }
        //    set
        //    {
        //        SetPropertyValue("MaterialType", ref _MaterialType, value);
        //    }
        //}

        public DateTime BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                SetPropertyValue("BeginDate", ref _BeginDate, value);
            }
        }

        [VisibleInDetailView(false)]
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetPropertyValue("EndDate", ref _EndDate, value);
            }
        }

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Price
        {
            get
            {
                return _Price;
            }
            set
            {
                SetPropertyValue("Price", ref _Price, value);
            }
        }
    }
}
