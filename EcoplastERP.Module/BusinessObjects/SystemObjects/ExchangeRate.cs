using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.SystemObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    public class ExchangeRate : BaseObject
    {
        public ExchangeRate(Session session)
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
            if (Rate < 1) Rate = Rate * 10;
        }
        // Fields...
        private decimal _Rate;
        private CurrencyType _CurrencyType;
        private Currency _Currency;
        private DateTime _RateDate;

        public DateTime RateDate
        {
            get
            {
                return _RateDate;
            }
            set
            {
                SetPropertyValue("RateDate", ref _RateDate, value);
            }
        }
        
        public Currency Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                SetPropertyValue("Currency", ref _Currency, value);
            }
        }
        
        public CurrencyType CurrencyType
        {
            get
            {
                return _CurrencyType;
            }
            set
            {
                SetPropertyValue("CurrencyType", ref _CurrencyType, value);
            }
        }

        public decimal Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                SetPropertyValue("Rate", ref _Rate, value);
            }
        }
    }
}
