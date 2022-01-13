using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.PlateArchiveObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Reproduction.ReproductionNumber")]
    [NavigationItem("PlateArchiveManagement")]
    public class ReproductionAccept : BaseObject
    {
        public ReproductionAccept(Session session)
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
            if (Session.IsNewObject(this))
            {
                if (Reproduction != null)
                {
                    Reproduction.ReproductionStatus = ReproductionStatus.InGraph;
                    Reproduction.DesignedBy = Printmaker.NameSurname;
                }
            }
            else
            {
                if (OutgoingDate != null)
                {
                    if (Reproduction != null)
                    {
                        Reproduction.ReproductionStatus = ReproductionStatus.InSaleDepartment;
                        Reproduction.DesignedBy = Printmaker.NameSurname;
                    }
                }
                //if (Reproduction.ReproductionStatus == ReproductionStatus.Produced)
                //    throw new Exception("Kliþesi üretilmiþ reprodüksiyon formunda düzenleme yapýlamaz.");
            }
        }
        // Fields...
        private DateTime _ProductionGoingDate;
        private string _Instructions;
        private DateTime _OutgoingDate;
        private Printmaker _Printmaker;
        private DateTime _IncomingDate;
        private Reproduction _Reproduction;

        [RuleRequiredField]
        [DataSourceCriteria("ReproductionStatus != 'Produced'")]
        [Association("Reproduction-ReproductionAccepts")]
        public Reproduction Reproduction
        {
            get
            {
                return _Reproduction;
            }
            set
            {
                SetPropertyValue("Reproduction", ref _Reproduction, value);
            }
        }

        [RuleRequiredField]
        public DateTime IncomingDate
        {
            get
            {
                return _IncomingDate;
            }
            set
            {
                SetPropertyValue("IncomingDate", ref _IncomingDate, value);
            }
        }

        [RuleRequiredField]
        public Printmaker Printmaker
        {
            get
            {
                return _Printmaker;
            }
            set
            {
                SetPropertyValue("Printmaker", ref _Printmaker, value);
            }
        }

        public DateTime OutgoingDate
        {
            get
            {
                return _OutgoingDate;
            }
            set
            {
                SetPropertyValue("OutgoingDate", ref _OutgoingDate, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Instructions
        {
            get
            {
                return _Instructions;
            }
            set
            {
                SetPropertyValue("Instructions", ref _Instructions, value);
            }
        }


        public DateTime ProductionGoingDate
        {
            get
            {
                return _ProductionGoingDate;
            }
            set
            {
                SetPropertyValue("ProductionGoingDate", ref _ProductionGoingDate, value);
            }
        }
    }
}
