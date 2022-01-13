using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Contact.Name")]
    [NavigationItem("QualityManagement")]
    public class SupplierEvaluation : BaseObject
    {
        public SupplierEvaluation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            EvaluationDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (PurchaseOrderDetail != null)
            {
                PurchaseOrderDetail.PurchaseOrderStatus = PurchaseOrderStatus.WaitingForWaybill;
            }
        }
        // Fields...
        private string _Note;
        private int _ShipmentComplianceScore;
        private int _PackageTypeScore;
        private decimal _RejectedQuantity;
        private decimal _ConditionallyAcceptedQuantity;
        private decimal _AcceptedQuantity;
        private decimal _IncomingQuantity;
        private PurchaseOrderDetail _PurchaseOrderDetail;
        private Contact _Contact;
        private DateTime _EvaluationDate;

        public DateTime EvaluationDate
        {
            get
            {
                return _EvaluationDate;
            }
            set
            {
                SetPropertyValue("EvaluationDate", ref _EvaluationDate, value);
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
        public PurchaseOrderDetail PurchaseOrderDetail
        {
            get
            {
                return _PurchaseOrderDetail;
            }
            set
            {
                SetPropertyValue("PurchaseOrderDetail", ref _PurchaseOrderDetail, value);
            }
        }

        public decimal IncomingQuantity
        {
            get
            {
                return _IncomingQuantity;
            }
            set
            {
                SetPropertyValue("IncomingQuantity", ref _IncomingQuantity, value);
            }
        }

        public decimal AcceptedQuantity
        {
            get
            {
                return _AcceptedQuantity;
            }
            set
            {
                SetPropertyValue("AcceptedQuantity", ref _AcceptedQuantity, value);
            }
        }

        public decimal ConditionallyAcceptedQuantity
        {
            get
            {
                return _ConditionallyAcceptedQuantity;
            }
            set
            {
                SetPropertyValue("ConditionallyAcceptedQuantity", ref _ConditionallyAcceptedQuantity, value);
            }
        }

        public decimal RejectedQuantity
        {
            get
            {
                return _RejectedQuantity;
            }
            set
            {
                SetPropertyValue("RejectedQuantity", ref _RejectedQuantity, value);
            }
        }

        public int PackageTypeScore
        {
            get
            {
                return _PackageTypeScore;
            }
            set
            {
                SetPropertyValue("PackageTypeScore", ref _PackageTypeScore, value);
            }
        }

        public int ShipmentComplianceScore
        {
            get
            {
                return _ShipmentComplianceScore;
            }
            set
            {
                SetPropertyValue("ShipmentComplianceScore", ref _ShipmentComplianceScore, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                SetPropertyValue("Note", ref _Note, value);
            }
        }
    }
}
