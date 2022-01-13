using System;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ParametersObjects;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("ExpeditionNumber")]
    [NavigationItem("ShippingManagement")]
    public class Expedition : BaseObject
    {
        public Expedition(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ExpeditionStatus = ExpeditionStatus.WaitingforTruck;
            ExpeditionNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            ExpeditionDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            foreach (ExpeditionDetail detail in ExpeditionDetails)
            {
                detail.ShippingPlan.ExpeditionDetail = detail;
                if (detail.ShippingPlan.ShippingPlanStatus == ShippingPlanStatus.WaitingforExpedition)
                {
                    detail.ShippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforLoading;
                }
            }

            //Sefer kapatma kontrolü
            if (ExpeditionStatus == ExpeditionStatus.Completed)
            {
                foreach (Delivery delivery in Deliveries)
                {
                    if (delivery.DeliveryStatus != DeliveryStatus.Completed)
                    {
                        throw new UserFriendlyException(string.Format("{0} Nolu Teslimat Belgesinin durumu tamamlandý olarak ayarlanmamýþ. Sefer kapatma iþlemi yapýlamaz.", delivery.DeliveryNumber));
                    }
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            foreach (Delivery delivery in Deliveries)
            {
                if (delivery.DeliveryStatus != DeliveryStatus.Completed) delivery.Delete();
                else throw new UserFriendlyException("Sefere baðlý teslimatlarýn en az biri için irsaliye kesilmiþ. Seferi silebilmek için önce teslimatýn baðlý olduðu irsaliyeyi silmeniz gerekiyor.");
            }
            foreach (ExpeditionDetail detail in ExpeditionDetails)
            {
                //Session.ExecuteNonQuery(@"update ShippingPlan set ExpeditionDetail = NULL where Oid = @oid", new string[] { "@oid" }, new object[] { detail.ShippingPlan.Oid });
                detail.ShippingPlan.ExpeditionDetail = null;
                detail.ShippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforExpedition;
            }
        }
        // Fields...
        private DateTime _ExpeditionCompleteDate;
        private bool _ExpeditionMerge;
        private bool _PictureControl;
        private bool _PlateDriverControl;
        private bool _SalesOrderCancelControl;
        private bool _WeighbridgeControl;
        private string _WeighbridgeDorsePlate;
        private string _WeighbridgeTruckPlate;
        private decimal _WeighbridgeTare;
        private string _LoadingDorsePlate;
        private string _LoadingTruckPlate;
        private TruckDriver _TruckDriver;
        private Truck _Truck;
        private string _Note;
        private decimal _TotalShippingCost;
        private decimal _AdditionalShippingCost;
        private decimal _ShippingCost;
        private Route _Route;
        private string _SealNumber;
        private string _ContainerNumber;
        private Forwarding _Forwarding;
        private ShippingUser _ShippingUser;
        private DateTime _ExpeditionDate;
        private string _ExpeditionNumber;
        private ExpeditionStatus _ExpeditionStatus;

        [Appearance("Expedition.ExpeditionStatus", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [VisibleInDetailView(false)]
        public ExpeditionStatus ExpeditionStatus
        {
            get
            {
                return _ExpeditionStatus;
            }
            set
            {
                SetPropertyValue("ExpeditionStatus", ref _ExpeditionStatus, value);
            }
        }

        //[Appearance("Expedition.ExpeditionNumber", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        [RuleUniqueValue]
        [RuleRequiredField]
        [Appearance("Expedition.ExpeditionNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public string ExpeditionNumber
        {
            get
            {
                return _ExpeditionNumber;
            }
            set
            {
                SetPropertyValue("ExpeditionNumber", ref _ExpeditionNumber, value);
            }
        }

        //[Appearance("Expedition.ExpeditionDate", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        [RuleRequiredField]
        public DateTime ExpeditionDate
        {
            get
            {
                return _ExpeditionDate;
            }
            set
            {
                SetPropertyValue("ExpeditionDate", ref _ExpeditionDate, value);
            }
        }

        //[Appearance("Expedition.ShippingUser", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public ShippingUser ShippingUser
        {
            get
            {
                return _ShippingUser;
            }
            set
            {
                SetPropertyValue("ShippingUser", ref _ShippingUser, value);
            }
        }

        //[Appearance("Expedition.Forwarding", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public Forwarding Forwarding
        {
            get
            {
                return _Forwarding;
            }
            set
            {
                SetPropertyValue("Forwarding", ref _Forwarding, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ContainerNumber
        {
            get
            {
                return _ContainerNumber;
            }
            set
            {
                SetPropertyValue("ContainerNumber", ref _ContainerNumber, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SealNumber
        {
            get
            {
                return _SealNumber;
            }
            set
            {
                SetPropertyValue("SealNumber", ref _SealNumber, value);
            }
        }

        //[Appearance("Expedition.Route", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        [ImmediatePostData]
        public Route Route
        {
            get
            {
                return _Route;
            }
            set
            {
                SetPropertyValue("Route", ref _Route, value);
                GetRoute();
            }
        }

        [ImmediatePostData]
        public decimal ShippingCost
        {
            get
            {
                return _ShippingCost;
            }
            set
            {
                SetPropertyValue("ShippingCost", ref _ShippingCost, value);
                CalculateTotalShippingCost();
            }
        }

        //[Appearance("Expedition.AdditionalShippingCost", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        [ImmediatePostData]
        public decimal AdditionalShippingCost
        {
            get
            {
                return _AdditionalShippingCost;
            }
            set
            {
                SetPropertyValue("AdditionalShippingCost", ref _AdditionalShippingCost, value);
                CalculateTotalShippingCost();
            }
        }

        [Appearance("Expedition.TotalShippingCost", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal TotalShippingCost
        {
            get
            {
                return _TotalShippingCost;
            }
            set
            {
                SetPropertyValue("TotalShippingCost", ref _TotalShippingCost, value);
            }
        }

        public Truck Truck
        {
            get
            {
                return _Truck;
            }
            set
            {
                SetPropertyValue("Truck", ref _Truck, value);
            }
        }

        public TruckDriver TruckDriver
        {
            get
            {
                return _TruckDriver;
            }
            set
            {
                SetPropertyValue("TruckDriver", ref _TruckDriver, value);
            }
        }

        //[Appearance("Expedition.TruckerNameSurname", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public string TruckerNameSurname
        {
            get
            {
                return TruckDriver != null ? TruckDriver.NameSurname : string.Empty;
            }
        }

        //[Appearance("Expedition.TruckerCellPhone", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public string TruckerCellPhone
        {
            get
            {
                return TruckDriver != null ? TruckDriver.CellPhone : string.Empty;
            }
        }

        //[Appearance("Expedition.AssignTruckPlate", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public string AssignTruckPlate
        {
            get
            {
                return Truck != null ? Truck.PlateNumber : string.Empty;
            }
        }

        //[Appearance("Expedition.AssignTruckDorsePlate", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public string AssignTruckDorsePlate
        {
            get
            {
                return Truck != null ? Truck.DorsePlate : string.Empty;
            }
        }

        [Appearance("Expedition.TruckPlate", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LoadingTruckPlate
        {
            get
            {
                return _LoadingTruckPlate;
            }
            set
            {
                SetPropertyValue("LoadingTruckPlate", ref _LoadingTruckPlate, value);
            }
        }

        [Appearance("Expedition.DorsePlate", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LoadingDorsePlate
        {
            get
            {
                return _LoadingDorsePlate;
            }
            set
            {
                SetPropertyValue("LoadingDorsePlate", ref _LoadingDorsePlate, value);
            }
        }

        public decimal WeighbridgeTare
        {
            get
            {
                return _WeighbridgeTare;
            }
            set
            {
                SetPropertyValue("WeighbridgeTare", ref _WeighbridgeTare, value);
            }
        }

        //[Appearance("Expedition.WeighbridgeTruckPlate", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        [Size(10)]
        public string WeighbridgeTruckPlate
        {
            get
            {
                return _WeighbridgeTruckPlate;
            }
            set
            {
                SetPropertyValue("WeighbridgeTruckPlate", ref _WeighbridgeTruckPlate, value);
            }
        }

        //[Appearance("Expedition.WeighbridgeDorsePlate", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        [Size(10)]
        public string WeighbridgeDorsePlate
        {
            get
            {
                return _WeighbridgeDorsePlate;
            }
            set
            {
                SetPropertyValue("WeighbridgeDorsePlate", ref _WeighbridgeDorsePlate, value);
            }
        }

        public decimal TotalLoadedQuantity
        {
            get
            {
                decimal total = 0;
                foreach (Delivery item in Deliveries)
                {
                    total += item.TotalLoadedQuantity;
                }
                return total;
            }
        }

        public decimal TotalLoadedcQuantity
        {
            get
            {
                decimal total = 0;
                foreach (Delivery item in Deliveries)
                {
                    total += item.TotalLoadedcQuantity;
                }
                return total;
            }
        }

        public decimal TotalPaletteLastWeight
        {
            get
            {
                decimal total = 0;
                foreach (Delivery item in Deliveries)
                {
                    total += item.TotalPaletteLastWeight;
                }
                return total;
            }
        }

        //[Appearance("Expedition.WeighbridgeControl", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public bool WeighbridgeControl
        {
            get
            {
                return _WeighbridgeControl;
            }
            set
            {
                SetPropertyValue("WeighbridgeControl", ref _WeighbridgeControl, value);
            }
        }

        //[Appearance("Expedition.SalesOrderCancelControl", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public bool SalesOrderCancelControl
        {
            get
            {
                return _SalesOrderCancelControl;
            }
            set
            {
                SetPropertyValue("SalesOrderCancelControl", ref _SalesOrderCancelControl, value);
            }
        }

        //[Appearance("Expedition.PlateDriverControl", Context = "DetailView", Enabled = false, Criteria = "ExpeditionStatus = 'Completed'")]
        public bool PlateDriverControl
        {
            get
            {
                return _PlateDriverControl;
            }
            set
            {
                SetPropertyValue("PlateDriverControl", ref _PlateDriverControl, value);
            }
        }
        
        public bool PictureControl
        {
            get
            {
                return _PictureControl;
            }
            set
            {
                SetPropertyValue("PictureControl", ref _PictureControl, value);
            }
        }

        public bool ExpeditionMerge
        {
            get
            {
                return _ExpeditionMerge;
            }
            set
            {
                SetPropertyValue("ExpeditionMerge", ref _ExpeditionMerge, value);
            }
        }

        [Appearance("Expedition.ExpeditionCompleteDate", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public DateTime ExpeditionCompleteDate
        {
            get
            {
                return _ExpeditionCompleteDate;
            }
            set
            {
                SetPropertyValue("ExpeditionCompleteDate", ref _ExpeditionCompleteDate, value);
            }
        }

        [Size(200)]
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

        [Association, Aggregated]
        public XPCollection<ExpeditionDetail> ExpeditionDetails
        {
            get { return GetCollection<ExpeditionDetail>("ExpeditionDetails"); }
        }

        [Association("Expedition-Deliveries")]
        public XPCollection<Delivery> Deliveries
        {
            get
            {
                return GetCollection<Delivery>("Deliveries");
            }
        }

        [Association, Aggregated]
        public XPCollection<ExpeditionPortfolio> ExpeditionPortfolios
        {
            get { return GetCollection<ExpeditionPortfolio>("ExpeditionPortfolios"); }
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

        #region functions
        void GetRoute()
        {
            if (IsLoading) return;
            if (Route != null)
            {
                ShippingCost = Route.ShippingCost;
                CalculateTotalShippingCost();
            }
        }
        void CalculateTotalShippingCost()
        {
            if (IsLoading) return;
            TotalShippingCost = ShippingCost + AdditionalShippingCost;
        }
        #endregion

        [Action(PredefinedCategory.View, Caption = "Kamyon Ata", AutoCommit = true, ImageName = "BO_Vendor", TargetObjectsCriteria = "ExpeditionStatus = 'WaitingforTruck'", ToolTip = "Bu iþlem seçili seferler için kamyon atama iþlemi yapar.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        public void SendTruck(ExpeditionParameters parameters)
        {
            if (parameters.Truck != null)
            {
                var truck = Session.FindObject<Truck>(new BinaryOperator("Oid", parameters.Truck.Oid));
                if (truck != null)
                {
                    truck.PlateNumber = parameters.PlateNumber;
                    truck.Model = parameters.Model;
                    truck.Tare = parameters.Tare;
                    truck.PermitNumber = parameters.PermitNumber;
                    truck.PermitDate = parameters.PermitDate;
                    truck.Payload = parameters.Payload;
                    truck.KDocument = parameters.KDocument;
                    truck.ExpirationDate = parameters.ExpirationDate;
                    truck.AdministrationDate = parameters.AdministrationDate;
                }

                if (parameters.TruckDriver != null)
                {
                    var truckDriver = Session.FindObject<TruckDriver>(new BinaryOperator("Oid", parameters.TruckDriver.Oid));
                    if (truckDriver != null)
                    {
                        truckDriver.NameSurname = parameters.NameSurname;
                        truckDriver.LicenseNumber = parameters.LicenseNumber;
                        truckDriver.LicenseDate = parameters.LicenseDate;
                        truckDriver.CellPhone = parameters.CellPhone1;
                        truckDriver.OtherPhone = parameters.CellPhone2;
                        truckDriver.Address = parameters.Address;
                    }
                }

                //Teslimat belgeleri oluþturma
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(@"select O.Contact, O.ShippingContact, O.TransportType from ExpeditionDetail Ed inner join SalesOrderDetail Sd on Sd.Oid = Ed.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder where Ed.GCRecord is null and Ed.Expedition = @expedition group by O.Contact, O.ShippingContact, O.TransportType", Session.ConnectionString);
                da.SelectCommand.Parameters.Add(new SqlParameter("@expedition", this.Oid));
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    Contact contact = Session.FindObject<Contact>(new BinaryOperator("Oid", Guid.Parse(row["Contact"].ToString())));
                    Contact shippingContact = Session.FindObject<Contact>(new BinaryOperator("Oid", Guid.Parse(row["ShippingContact"].ToString())));
                    if (truck.TruckType == TruckType.Truck)
                    {
                        if (shippingContact.TruckBlock)
                            throw new UserFriendlyException("Malý teslim alan firmaya týr giremez. Seçtiðiniz kamyon türünü kontrol edip tekrar deneyiniz.");
                    }

                    Delivery delivery = new Delivery(Session)
                    {
                        Contact = contact,
                        ShippingContact = shippingContact,
                        Route = this.Route,
                        TransportType = (TransportType)Convert.ToInt32(row["TransportType"]),
                        Expedition = this
                    };

                    this.ExpeditionDetails.Filter = CriteriaOperator.Parse("SalesOrderDetail.SalesOrder.Contact = ? and SalesOrderDetail.SalesOrder.ShippingContact = ? and SalesOrderDetail.SalesOrder.TransportType = ?", contact, shippingContact, row["TransportType"]);
                    foreach (ExpeditionDetail detail in this.ExpeditionDetails)
                    {
                        DeliveryDetail deliveryDetail = new DeliveryDetail(Session)
                        {
                            Delivery = delivery,
                            SalesOrderDetail = detail.SalesOrderDetail,
                            Unit = detail.Unit,
                            Quantity = detail.Quantity,
                            cUnit = detail.cUnit,
                            cQuantity = detail.cQuantity,
                            ExpeditionDetail = detail
                        };
                        delivery.DeliveryDetails.Add(deliveryDetail);
                        detail.DeliveryDetail = deliveryDetail;
                    }
                }

                Truck = Session.FindObject<Truck>(new BinaryOperator("Oid", parameters.Truck.Oid));
                TruckDriver = Session.FindObject<TruckDriver>(new BinaryOperator("Oid", parameters.TruckDriver.Oid));
                ExpeditionStatus = ExpeditionStatus.WaitingforLoading;

                foreach (ExpeditionDetail detail in ExpeditionDetails)
                {
                    detail.ShippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforLoading;
                }
            }
        }

        [Action(PredefinedCategory.View, Caption = "Remove From Truck", AutoCommit = true, ImageName = "Action_Deny", TargetObjectsCriteria = "ExpeditionStatus = 'WaitingforLoading'", ToolTip = "This operation will remove from truck the selected expedition.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void RemoveFromTruck()
        {
            bool error = false;
            //XPCollection<Delivery> deliveryList = new XPCollection<Delivery>(Session, CriteriaOperator.Parse("Expedition = ?", this));
            foreach (Delivery item in Deliveries)
            {
                if (item.TotalLoadedQuantity > 0) error = true;
            }

            if (!error)
            {
                Truck = null;
                TruckDriver = null;
                ExpeditionStatus = ExpeditionStatus.WaitingforTruck;

                Session.Delete(Deliveries);
                foreach (ExpeditionDetail detail in ExpeditionDetails)
                {
                    detail.DeliveryDetail = null;
                }
            }
            else
            {
                throw new UserFriendlyException("Teslimatlardan biri için yükleme yapýlmýþ. Kamyon atama iptal edilemez.");
            }
        }
    }
}
