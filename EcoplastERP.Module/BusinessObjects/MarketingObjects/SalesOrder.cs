using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Order")]
    [DefaultProperty("OrderNumber")]
    [NavigationItem("MarketingManagement")]
    [Appearance("SalesOrder.Contact", AppearanceItemType = "ViewItem", TargetItems = "Contact", Criteria = "Contact.SalesOrderBlock = true", Context = "SalesOrder_DetailView", BackColor = "Red", FontColor = "White")]
    public class SalesOrder : BaseObject
    {
        public SalesOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OrderDate = Helpers.GetSystemDate(Session);
            OrderNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            Employee employee = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
            if (employee != null)
            {
                SalesOrganization = employee.SalesOrganization ?? null;
                SalesOffice = employee.SalesOffice ?? null;
                SalesGroup = employee.SalesGroup ?? null;
            }
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
            ContactOrderDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (!Session.IsNewObject(this))
            {
                foreach (SalesOrderDetail salesOrderDetail in SalesOrderDetails)
                {
                    DeliveryDetail deliveryDetail = Session.FindObject<DeliveryDetail>(new BinaryOperator("SalesOrderDetail", salesOrderDetail));
                    if (deliveryDetail != null)
                    {
                        if(deliveryDetail.Delivery.Contact != this.Contact) deliveryDetail.Delivery.Contact = this.Contact;
                        if(deliveryDetail.Delivery.ShippingContact != this.ShippingContact) deliveryDetail.Delivery.ShippingContact = this.ShippingContact;
                        if(deliveryDetail.Delivery.TransportType != this.TransportType) deliveryDetail.Delivery.TransportType = this.TransportType;
                    }
                }
                if(SalesOrderType== SalesOrderType.A3Order)
                {
                    foreach (SalesOrderDetail item in SalesOrderDetails)
                    {
                        item.ShippingNote = "A3 SİPARİŞİ";
                    }
                }
            }
            if (Contact != null)
            {
                if (Contact.SalesOrderBlock)
                {
                    throw new UserFriendlyException("Seçtiğiniz cari kart sipariş blokajlıdır.");
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (filmingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco2 üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", filmingWorkOrder.WorkOrderNumber));

            var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (printingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco1 üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", filmingWorkOrder.WorkOrderNumber));

            var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (laminationWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco1 Lamine üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", filmingWorkOrder.WorkOrderNumber));

            var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (cuttingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco4 üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (slicingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco4 Dilme üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (castSlicingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco5 Dilme üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var castfilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (castfilmingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco5 CPP üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var balloonfilmingWorkOrder = Session.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (balloonfilmingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco5 Stretch üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (castTransferingWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco5 Aktarma üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", cuttingWorkOrder.WorkOrderNumber));

            var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (regeneratedWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco3 üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", regeneratedWorkOrder.WorkOrderNumber));

            var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (eco6WorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco6 üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", regeneratedWorkOrder.WorkOrderNumber));

            var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("SalesOrderDetail.SalesOrder.Oid", this.Oid));
            if (eco6LaminationWorkOrder != null) throw new UserFriendlyException(String.Format("Bu sipariş için {0} nolu Eco6 Laminasyon üretim sipariş emri oluşturulmuştur. Siparişi silebilmek için öncelikle üretim siparişini silmeniz gerekmektedir.", regeneratedWorkOrder.WorkOrderNumber));
        }
        // Fields...
        private Employee _CreatedBy;
        private decimal _Total;
        private decimal _Tax;
        private decimal _SubTotal;
        private bool _PaymentBlockage;
        private Blockage _Blockage;
        private SalesOffice _SalesOffice;
        private SalesGroup _SalesGroup;
        private SalesOrganization _SalesOrganization;
        private PaymentMethod _PaymentMethod;
        private ContactVehicle _ContactVehicle;
        private TransportType _TransportType;
        private DateTime _ContactOrderDate;
        private string _ContactOrderNumber;
        private Incoterms _Incoterms;
        private DistributionChannel _DistributionChannel;
        private ContactGroup2 _ContactGroup2;
        private Contact _ShippingContact;
        private Contact _Contact;
        private SalesOrderType _SalesOrderType;
        private DateTime _OrderDate;
        private string _OrderNumber;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        [Appearance("SalesOrder.OrderNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public string OrderNumber
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

        [NonCloneable]
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

        [ImmediatePostData]
        [RuleRequiredField]
        public SalesOrderType SalesOrderType
        {
            get
            {
                return _SalesOrderType;
            }
            set
            {
                SetPropertyValue("SalesOrderType", ref _SalesOrderType, value);
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
            }
        }

        [RuleRequiredField]
        [ImmediatePostData]
        [DataSourceCriteria("MainContact = '@this.Contact' or Oid = '@this.Contact.Oid'")]
        public Contact ShippingContact
        {
            get
            {
                return _ShippingContact;
            }
            set
            {
                SetPropertyValue("ShippingContact", ref _ShippingContact, value);
            }
        }

        [VisibleInListView(false)]
        public string ShippingAddress
        {
            get
            {
                return ShippingContact != null ? ShippingContact.Address : string.Empty;
            }
        }

        public string ContactOrderNumber
        {
            get
            {
                return _ContactOrderNumber;
            }
            set
            {
                SetPropertyValue("ContactOrderNumber", ref _ContactOrderNumber, value);
            }
        }

        [NonCloneable]
        public DateTime ContactOrderDate
        {
            get
            {
                return _ContactOrderDate;
            }
            set
            {
                SetPropertyValue("ContactOrderDate", ref _ContactOrderDate, value);
            }
        }

        public ContactGroup2 ContactGroup2
        {
            get
            {
                return _ContactGroup2;
            }
            set
            {
                SetPropertyValue("ContactGroup2", ref _ContactGroup2, value);
            }
        }
        
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

        [Appearance("SalesOrder.Incoterms", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "SalesOrderType != 'ExportingOrder' and SalesOrderType != 'ExportRegisteredOrder'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "SalesOrderType = 'ExportingOrder' or SalesOrderType = 'ExportRegisteredOrder'")]
        public Incoterms Incoterms
        {
            get
            {
                return _Incoterms;
            }
            set
            {
                SetPropertyValue("Incoterms", ref _Incoterms, value);
            }
        }

        [RuleRequiredField]
        public TransportType TransportType
        {
            get
            {
                return _TransportType;
            }
            set
            {
                SetPropertyValue("TransportType", ref _TransportType, value);
            }
        }

        public ContactVehicle ContactVehicle
        {
            get
            {
                return _ContactVehicle;
            }
            set
            {
                SetPropertyValue("ContactVehicle", ref _ContactVehicle, value);
            }
        }

        [RuleRequiredField]
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

        [RuleRequiredField]
        public SalesOrganization SalesOrganization
        {
            get
            {
                return _SalesOrganization;
            }
            set
            {
                SetPropertyValue("SalesOrganization", ref _SalesOrganization, value);
            }
        }

        [RuleRequiredField]
        [DataSourceCriteria("SalesOrganization = '@this.SalesOrganization'")]
        public SalesOffice SalesOffice
        {
            get
            {
                return _SalesOffice;
            }
            set
            {
                SetPropertyValue("SalesOffice", ref _SalesOffice, value);
            }
        }

        [RuleRequiredField]
        [DataSourceCriteria("SalesOffice = '@this.SalesOffice'")]
        public SalesGroup SalesGroup
        {
            get
            {
                return _SalesGroup;
            }
            set
            {
                SetPropertyValue("SalesGroup", ref _SalesGroup, value);
            }
        }

        public Blockage Blockage
        {
            get
            {
                return _Blockage;
            }
            set
            {
                SetPropertyValue("Blockage", ref _Blockage, value);
            }
        }

        public bool PaymentBlockage
        {
            get
            {
                return _PaymentBlockage;
            }
            set
            {
                SetPropertyValue("PaymentBlockage", ref _PaymentBlockage, value);
            }
        }

        [Appearance("SalesOrder.SubTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Appearance("SalesOrder.Tax", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Tax
        {
            get
            {
                return _Tax;
            }
            set
            {
                SetPropertyValue("Tax", ref _Tax, value);
            }
        }

        [Appearance("SalesOrder.Total", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [NonCloneable]
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
        public XPCollection<SalesOrderDetail> SalesOrderDetails
        {
            get { return GetCollection<SalesOrderDetail>("SalesOrderDetails"); }
        }

        #region functions
        public void UpdateTotals()
        {
            if (IsLoading) return;
            decimal subtotal = 0, taxtotal = 0;
            foreach (var detail in SalesOrderDetails)
            {
                subtotal += detail.Total;
                taxtotal += detail.Tax;
            }
            SubTotal = subtotal;
            Tax = taxtotal;
            Total = SubTotal + Tax;
        }

        void GetContact()
        {
            if (IsLoading) return;
            if (Contact != null)
            {
                DistributionChannel = Contact.DistributionChannel;
                PaymentMethod = Contact.PaymentMethod;
                ContactGroup2 = Contact.ContactGroup2;
            }
        }
        #endregion
    }
}