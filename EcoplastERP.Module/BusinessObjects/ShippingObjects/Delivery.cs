using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("DeliveryNumber")]
    [NavigationItem("ShippingManagement")]
    public class Delivery : BaseObject
    {
        public Delivery(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DeliveryStatus = DeliveryStatus.WaitingforLoading;
            DeliveryNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            DeliveryDate = Helpers.GetSystemDate(Session);
            DeliveryBlockage = true;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SalesWaybill == null)
            {
                if (Expedition != null)
                {
                    if (Expedition.ExpeditionStatus == ExpeditionStatus.Completed)
                    {
                        throw new UserFriendlyException("Teslimatın bağlı olduğu sefer kapatılmış. Herhangi bir değişiklik yapılamaz. !");
                    }
                }
                if (DeliveryBlockStatus == DeliveryBlockStatus.Documentable)
                {
                    if (TotalLoadedQuantity >= 0)
                        DeliveryStatus = DeliveryStatus.WaitingforWaybill;
                    else throw new UserFriendlyException("Teslimat için okutulma yapılmamış. Evrak kesilebilir hale getirilemez. !");
                }
                else
                {
                    if (TotalLoadedQuantity >= 0) DeliveryStatus = DeliveryStatus.Loading;
                    else DeliveryStatus = DeliveryStatus.WaitingforLoading;
                }
            }
            bool oldDeliveryBlockage = Convert.ToBoolean(Session.ExecuteScalar("select DeliveryBlockage from Delivery where Oid = @oid", new string[] { "@oid" }, new object[] { this.Oid }));
            if (oldDeliveryBlockage != DeliveryBlockage)
            {
                if (!Helpers.IsUserInRole("Teslimat Blokaj"))
                {
                    DeliveryBlockage = true;
                    throw new UserFriendlyException("Teslimat Blokaj kaldırmak için yetkiniz yok.");
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Expedition != null)
            {
                if (Expedition.ExpeditionStatus == ExpeditionStatus.Completed)
                {
                    throw new UserFriendlyException(@"Sefer kapatma yapmış teslimat belgesi silinemez.");
                }
                //else
                //{
                //    foreach (DeliveryDetail detail in DeliveryDetails)
                //    {
                //        detail.ExpeditionDetail.ShippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforExpedition;
                //    }
                //}
            }
        }
        // Fields...
        private SalesWaybill _SalesWaybill;
        private Expedition _Expedition;
        private decimal _ShippingCost;
        private string _DeliveryBlockNote;
        private TransportType _TransportType;
        private bool _DeliveryBlockage;
        private Route _Route;
        private Contact _ShippingContact;
        private Contact _Contact;
        private DateTime _DeliveryDate;
        private string _DeliveryNumber;
        private DeliveryBlockStatus _DeliveryBlockStatus;
        private DeliveryStatus _DeliveryStatus;

        [Appearance("Delivery.DeliveryStatus", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [VisibleInDetailView(false)]
        public DeliveryStatus DeliveryStatus
        {
            get
            {
                return _DeliveryStatus;
            }
            set
            {
                SetPropertyValue("DeliveryStatus", ref _DeliveryStatus, value);
            }
        }

        [Appearance("Delivery.DeliveryBlockStatus", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [VisibleInDetailView(false)]
        public DeliveryBlockStatus DeliveryBlockStatus
        {
            get
            {
                return _DeliveryBlockStatus;
            }
            set
            {
                SetPropertyValue("DeliveryBlockStatus", ref _DeliveryBlockStatus, value);
            }
        }

        [RuleUniqueValue]
        [Appearance("Delivery.DeliveryNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DeliveryNumber
        {
            get
            {
                return _DeliveryNumber;
            }
            set
            {
                SetPropertyValue("DeliveryNumber", ref _DeliveryNumber, value);
            }
        }

        public DateTime DeliveryDate
        {
            get
            {
                return _DeliveryDate;
            }
            set
            {
                SetPropertyValue("DeliveryDate", ref _DeliveryDate, value);
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
            }
        }

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

        public Route Route
        {
            get
            {
                return _Route;
            }
            set
            {
                SetPropertyValue("Route", ref _Route, value);
            }
        }

        public bool DeliveryBlockage
        {
            get
            {
                return _DeliveryBlockage;
            }
            set
            {
                SetPropertyValue("DeliveryBlockage", ref _DeliveryBlockage, value);
            }
        }

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

        [Size(SizeAttribute.Unlimited)]
        public string DeliveryBlockNote
        {
            get
            {
                return _DeliveryBlockNote;
            }
            set
            {
                SetPropertyValue("DeliveryBlockNote", ref _DeliveryBlockNote, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal TotalLoadedQuantity
        {
            get
            {
                decimal total = 0;
                foreach (DeliveryDetail detail in DeliveryDetails)
                {
                    total += detail.LoadedQuantity;
                }
                return total;
            }
        }

        [VisibleInDetailView(false)]
        public decimal TotalLoadedcQuantity
        {
            get
            {
                decimal total = 0;
                foreach (DeliveryDetail detail in DeliveryDetails)
                {
                    total += detail.LoadedcQuantity;
                }
                return total;
            }
        }

        [VisibleInDetailView(false)]
        public decimal TotalPaletteLastWeight
        {
            get
            {
                decimal total = 0;
                foreach (DeliveryDetail detail in DeliveryDetails)
                {
                    total += detail.PaletteLastWeightTotal;
                }
                return total;
            }
        }

        [Appearance("Delivert.ShippingCost", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal ShippingCost
        {
            get
            {
                return _ShippingCost;
            }
            set
            {
                SetPropertyValue("ShippingCost", ref _ShippingCost, value);
            }
        }

        [VisibleInDetailView(false)]
        [Association("Expedition-Deliveries")]
        public Expedition Expedition
        {
            get
            {
                return _Expedition;
            }
            set
            {
                SetPropertyValue("Expedition", ref _Expedition, value);
            }
        }

        [VisibleInDetailView(false)]
        public SalesWaybill SalesWaybill
        {
            get
            {
                return _SalesWaybill;
            }
            set
            {
                SetPropertyValue("SalesWaybill", ref _SalesWaybill, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<DeliveryDetail> DeliveryDetails
        {
            get { return GetCollection<DeliveryDetail>("DeliveryDetails"); }
        }

        private XPCollection<AuditDataItemPersistent> changeHistory;
        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                {
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return changeHistory;
            }
        }
    }
}
