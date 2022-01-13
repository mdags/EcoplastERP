using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Order")]
    [DefaultProperty("OrderNumber")]
    [NavigationItem("PurchaseManagement")]
    public class PurchaseOrder : BaseObject
    {
        public PurchaseOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OrderDate = Helpers.GetSystemDate(Session);
            OrderNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            foreach (PurchaseOrderDetail item in PurchaseOrderDetails)
            {
                if (item.DemandDetail != null)
                {
                    item.DemandDetail.PurchaseOrderDetail = item;
                    if (item.DemandDetail.DemandStatus == DemandStatus.WaitingForOrder || item.DemandDetail.DemandStatus == DemandStatus.WaitingForPurchase)
                        item.DemandDetail.DemandStatus = DemandStatus.WaitingForWarehouseEntry;
                }
                if (item.OfferDetail != null)
                {
                    item.OfferDetail.PurchaseOrderDetail = item;
                    if (item.OfferDetail.OfferStatus == OfferStatus.WaitingForOrder)
                        item.OfferDetail.OfferStatus = OfferStatus.WaitingForWarehouseEntry;
                }
            }
            //if (Session.Connection != null & Session.IsNewObject(this))
            //{
            //    foreach (var item in PurchaseOrderDetail)
            //    {
            //        var newMyEvent = new MyEvent(Session)
            //        {
            //            Subject = String.Format("{0}/{1} nolu sipariþ termini", OrderNumber, item.LineNumber),
            //            Description = String.Format("{0} firmasýndan {1} tarihinde {2} malzemesi gelecektir.", Contact.Name, item.LineDeliveryDate.ToShortDateString(), item.DemandDetail.Product.Name),
            //            AllDay = false,
            //            StartOn = new DateTime(item.LineDeliveryDate.Year, item.LineDeliveryDate.Month, item.LineDeliveryDate.Day, 8, 50, 0).AddMinutes(item.LineNumber * 10),
            //            EndOn = new DateTime(item.LineDeliveryDate.Year, item.LineDeliveryDate.Month, item.LineDeliveryDate.Day, 8, 50, 0).AddMinutes((item.LineNumber * 10) + 10),
            //            Employee = item.DemandDetail.Demander
            //        };
            //    }
            //}
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            foreach (var item in PurchaseOrderDetails)
            {
                var purchaseWaybillDetail = Session.FindObject<PurchaseWaybillDetail>(new BinaryOperator("PurchaseOrderDetail", item.Oid));
                if (purchaseWaybillDetail != null) throw new Exception("Bu tedarikçi sipariþinin kalemlerinden en az biri için irsaliye giriþi yapýlmýþ. Kaydý silmek için öncelikle irsaliyesini silmeniz gerekmektedir.");
                else
                {
                    if (item.OfferDetail != null) item.OfferDetail.OfferStatus = OfferStatus.WaitingForOrder;
                    if (item.DemandDetail != null) item.DemandDetail.DemandStatus = DemandStatus.WaitingForOrder;
                }
            }
        }
        // Fields...
        private Employee _CreatedBy;
        private decimal _Total;
        private decimal _TaxTotal;
        private decimal _SubTotal;
        private PaymentMethod _PaymentMethod;
        private DistributionChannel _DistributionChannel;
        private string _AuthorizedPerson;
        private Contact _Contact;
        private PurchaseOrderType _PurchaseOrderType;
        private DateTime _OrderDate;
        private int _OrderNumber;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public int OrderNumber
        {
            get
            {
                return _OrderNumber;
            }
            set
            {
                SetPropertyValue("OrderNumber", ref _OrderNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime OrderDate
        {
            get
            {
                return _OrderDate;
            }
            set
            {
                SetPropertyValue("OrderDate", ref _OrderDate, value);
            }
        }

        public PurchaseOrderType PurchaseOrderType
        {
            get
            {
                return _PurchaseOrderType;
            }
            set
            {
                SetPropertyValue("PurchaseOrderType", ref _PurchaseOrderType, value);
            }
        }

        [ImmediatePostData]
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
                GetContact();
                GetContractedSupplier();
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

        [Appearance("PurchaseOrder.SubTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Appearance("PurchaseOrder.TaxTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Appearance("PurchaseOrder.Total", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
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
        public XPCollection<PurchaseOrderDetail> PurchaseOrderDetails
        {
            get { return GetCollection<PurchaseOrderDetail>("PurchaseOrderDetails"); }
        }

        #region functions
        public void UpdateTotals()
        {
            if (IsLoading) return;
            decimal subTotal = 0, taxTotal = 0;
            foreach (var detail in PurchaseOrderDetails)
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
        void GetContractedSupplier()
        {
            if (IsLoading) return;
            if (Contact != null)
            {
                foreach (PurchaseOrderDetail item in PurchaseOrderDetails)
                {
                    ContractedSupplierDetail contractedSupplierDetail = Session.FindObject<ContractedSupplierDetail>(CriteriaOperator.Parse("ContractedSupplier.Contact = ? and ContractedSupplier.BeginDate <= ? and ContractedSupplier.EndDate >= ? and Product = ?", Contact, OrderDate, OrderDate, item.Product));
                    if (contractedSupplierDetail != null)
                    {
                        item.Currency = contractedSupplierDetail.Currency;
                        if (!contractedSupplierDetail.Currency.IsDefault) item.CurrencyPrice = contractedSupplierDetail.CurrencyPrice;
                        else item.Price = contractedSupplierDetail.Price;
                    }
                }
            }
        }
        #endregion
    }
}
