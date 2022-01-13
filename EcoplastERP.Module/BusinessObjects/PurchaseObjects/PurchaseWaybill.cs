using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Resume")]
    [DefaultProperty("WaybillNumber")]
    [NavigationItem("PurchaseManagement")]
    public class PurchaseWaybill : BaseObject
    {
        public PurchaseWaybill(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            PurchaseWaybillStatus = PurchaseWaybillStatus.WaitingForComplete;
            WaybillDate = Helpers.GetSystemDate(Session);
            WaybillNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            foreach (PurchaseWaybillDetail item in PurchaseWaybillDetails)
            {
                if (item.DemandDetail != null)
                {
                    item.PurchaseOrderDetail.PurchaseWaybillDetail = item;
                }
                if (item.DemandDetail != null)
                {
                    item.DemandDetail.PurchaseWaybillDetail = item;
                    if (item.DemandDetail.DemandStatus == DemandStatus.WaitingForOrder || item.DemandDetail.DemandStatus == DemandStatus.WaitingForPurchase)
                        item.DemandDetail.DemandStatus = DemandStatus.WaitingForWarehouseEntry;
                }
                if (item.OfferDetail != null)
                {
                    item.OfferDetail.PurchaseWaybillDetail = item;
                    if (item.OfferDetail.OfferStatus == OfferStatus.WaitingForOrder)
                        item.OfferDetail.OfferStatus = OfferStatus.WaitingForWarehouseEntry;
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            try
            {
                foreach (var item in PurchaseWaybillDetails)
                {
                    if (item.PurchaseOrderDetail != null) item.PurchaseOrderDetail.PurchaseOrderStatus = PurchaseOrderStatus.WaitingForWaybill;

                    if (item.OfferDetail != null) item.OfferDetail.OfferStatus = OfferStatus.WaitingForWarehouseEntry;

                    if (item.DemandDetail != null) item.DemandDetail.DemandStatus = DemandStatus.WaitingForWarehouseEntry;

                    var headerId = Guid.NewGuid();
                    var output = Session.FindObject<MovementType>(new BinaryOperator("Code", "P103"));
                    var outputMovement = new Movement(Session)
                    {
                        HeaderId = headerId,
                        DocumentNumber = WaybillNumber,
                        DocumentDate = WaybillDate,
                        Barcode = string.Empty,
                        SalesOrderDetail = null,
                        Product = item.Product,
                        PartyNumber = string.IsNullOrEmpty(item.PartyNumber) ? string.Empty : item.PartyNumber,
                        PaletteNumber = string.Empty,
                        Warehouse = item.Warehouse,
                        MovementType = output,
                        Unit = item.Unit,
                        Quantity = item.Quantity,
                        cUnit = item.Unit,
                        cQuantity = item.Quantity,
                        PurchaseWaybillDetail = item
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Fields...
        private DIIB _DIIB;
        private Employee _CreatedBy;
        private decimal _Total;
        private decimal _TaxTotal;
        private decimal _SubTotal;
        private PaymentMethod _PaymentMethod;
        private DistributionChannel _DistributionChannel;
        private string _AuthorizedPerson;
        private Contact _Contact;
        private string _ReferenceDocumentNumber;
        private DateTime _WaybillDate;
        private string _WaybillNumber;
        private PurchaseWaybillStatus _PurchaseWaybillStatus;

        [NonCloneable]
        [VisibleInDetailView(false)]
        public PurchaseWaybillStatus PurchaseWaybillStatus
        {
            get
            {
                return _PurchaseWaybillStatus;
            }
            set
            {
                SetPropertyValue("PurchaseWaybillStatus", ref _PurchaseWaybillStatus, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string WaybillNumber
        {
            get
            {
                return _WaybillNumber;
            }
            set
            {
                SetPropertyValue("WaybillNumber", ref _WaybillNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime WaybillDate
        {
            get
            {
                return _WaybillDate;
            }
            set
            {
                SetPropertyValue("WaybillDate", ref _WaybillDate, value);
            }
        }

        public string ReferenceDocumentNumber
        {
            get
            {
                return _ReferenceDocumentNumber;
            }
            set
            {
                SetPropertyValue("ReferenceDocumentNumber", ref _ReferenceDocumentNumber, value);
            }
        }

        [RuleRequiredField]
        [ImmediatePostData]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
                GetContact();
            }
        }

        public string AuthorizedPerson
        {
            get
            {
                return _AuthorizedPerson;
            }
            set
            {
                SetPropertyValue("AuthorizedPerson", ref _AuthorizedPerson, value);
            }
        }

        [RuleRequiredField]
        public DistributionChannel DistributionChannel
        {
            get
            {
                return _DistributionChannel;
            }
            set
            {
                SetPropertyValue("DistributionChannel", ref _DistributionChannel, value);
            }
        }

        public PaymentMethod PaymentMethod
        {
            get
            {
                return _PaymentMethod;
            }
            set
            {
                SetPropertyValue("PaymentMethod", ref _PaymentMethod, value);
            }
        }

        public DIIB DIIB
        {
            get
            {
                return _DIIB;
            }
            set
            {
                SetPropertyValue("DIIB", ref _DIIB, value);
            }
        }

        [Appearance("PurchaseWaybill.SubTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal SubTotal
        {
            get
            {
                return _SubTotal;
            }
            set
            {
                SetPropertyValue("SubTotal", ref _SubTotal, value);
            }
        }

        [Appearance("PurchaseWaybill.TaxTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal TaxTotal
        {
            get
            {
                return _TaxTotal;
            }
            set
            {
                SetPropertyValue("TaxTotal", ref _TaxTotal, value);
            }
        }

        [Appearance("PurchaseWaybill.Total", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetPropertyValue("Total", ref _Total, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<PurchaseWaybillDetail> PurchaseWaybillDetails
        {
            get { return GetCollection<PurchaseWaybillDetail>("PurchaseWaybillDetails"); }
        }

        #region functions
        public void UpdateTotals()
        {
            if (IsLoading) return;
            decimal subTotal = 0, taxTotal = 0;
            foreach (var detail in PurchaseWaybillDetails)
            {
                subTotal += detail.Total;
                taxTotal += detail.Tax;
            }
            SubTotal = subTotal;
            TaxTotal = taxTotal;
            Total = SubTotal + TaxTotal;
        }
        private void GetContact()
        {
            if (IsLoading) return;
            if (Contact != null)
            {
                DistributionChannel = Contact.DistributionChannel;
                PaymentMethod = Contact.PaymentMethod;
            }
        }
        #endregion
    }
}
