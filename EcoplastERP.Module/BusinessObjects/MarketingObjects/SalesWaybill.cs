using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Sale")]
    [DefaultProperty("WaybillNumber")]
    [NavigationItem("MarketingManagement")]
    public class SalesWaybill : BaseObject
    {
        public SalesWaybill(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            WaybillNumber = Helpers.GetDocumentNumber(Session, this.GetType().FullName);
            WaybillDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this) && Session.Connection != null)
            {
                if (Delivery == null)
                {
                    foreach (SalesWaybillDetail detail in SalesWaybillDetails)
                    {
                        Movement outMovement = new Movement(Session)
                        {
                            HeaderId = Guid.NewGuid(),
                            DocumentNumber = WaybillNumber,
                            DocumentDate = Helpers.GetSystemDate(Session),
                            MovementType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P141")),
                            Barcode = string.Empty,
                            SalesOrderDetail = null,
                            Product = detail.Product,
                            PartyNumber = detail.PartyNumber,
                            PaletteNumber = string.Empty,
                            Warehouse = detail.Warehouse,
                            WarehouseCell = null,
                            Unit = detail.Unit,
                            Quantity = detail.Quantity,
                            cUnit = detail.cUnit,
                            cQuantity = detail.cQuantity
                        };
                    }
                }
                else
                {
                    Guid headerId = Guid.NewGuid();
                    MovementType outType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P141"));
                    MovementType inTransferType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P126"));
                    MovementType outTransferType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P127"));
                    foreach (DeliveryDetail detail in Delivery.DeliveryDetails)
                    {
                        if (detail.SalesOrderDetail.SalesOrder.Contact.FastShippingWarehouse != null)
                        {
                            Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, @documentNumber, @documentDate, @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode in (select Barcode from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = @deliveryDetail)", new string[] { "@headerId", "@documentNumber", "@documentDate", "@movementType", "@deliveryDetail" }, new object[] { headerId, WaybillNumber, WaybillDate, outTransferType.Oid, detail.Oid });
                            Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, @documentNumber, @documentDate, @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode in (select Barcode from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = @deliveryDetail)", new string[] { "@headerId", "@documentNumber", "@documentDate", "@movementType", "@deliveryDetail" }, new object[] { headerId, WaybillNumber, WaybillDate, inTransferType.Oid, detail.Oid });

                            Session.ExecuteNonQuery(@"update Store set Warehouse = @warehouse where Barcode in (select Barcode from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = @deliveryDetail)", new string[] { "@warehouse", "@deliveryDetail" }, new object[] { detail.SalesOrderDetail.SalesOrder.Contact.FastShippingWarehouse.Oid, detail.Oid });
                        }
                        else
                        {
                            Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, @documentNumber, @documentDate, @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode in (select Barcode from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = @deliveryDetail)", new string[] { "@headerId", "@documentNumber", "@documentDate", "@movementType", "@deliveryDetail" }, new object[] { headerId, WaybillNumber, WaybillDate, outType.Oid, detail.Oid });

                            Session.ExecuteNonQuery(@"delete Store where Barcode in (select Barcode from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = @deliveryDetail)", new string[] { "@deliveryDetail" }, new object[] { detail.Oid });
                        }

                        Session.ExecuteNonQuery(@"update ShippingPlan set ShippingPlanStatus = 10 where Oid = @oid", new string[] { "@oid" }, new object[] { detail.ExpeditionDetail.ShippingPlan.Oid });

                        decimal shippedcQuantity = Convert.ToDecimal(Session.Evaluate<DeliveryDetailLoading>(CriteriaOperator.Parse("Sum(cQuantity)"), CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ?", detail.SalesOrderDetail)));
                        if (shippedcQuantity >= detail.SalesOrderDetail.Quantity) detail.SalesOrderDetail.SalesOrderStatus = SalesOrderStatus.Completed;
                    }
                    Delivery.SalesWaybill = this;
                    Delivery.DeliveryStatus = DeliveryStatus.Completed;
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
                    throw new UserFriendlyException(@"Sefer kapatma yapmýþ teslimat belgesi silinemez.");
                }
            }

            if (Delivery != null)
            {
                foreach (DeliveryDetail detail in Delivery.DeliveryDetails)
                {
                    foreach (DeliveryDetailLoading loading in detail.DeliveryDetailLoadings)
                    {
                        Movement inMovement = new Movement(Session)
                        {
                            HeaderId = Guid.NewGuid(),
                            DocumentNumber = WaybillNumber,
                            DocumentDate = Helpers.GetSystemDate(Session),
                            MovementType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P140")),
                            Barcode = loading.Barcode,
                            SalesOrderDetail = detail.SalesOrderDetail,
                            Product = detail.SalesOrderDetail.Product,
                            PartyNumber = string.Empty,
                            PaletteNumber = loading.PaletteNumber,
                            Warehouse = Session.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true)),
                            WarehouseCell = null,
                            Unit = loading.Unit,
                            Quantity = loading.Quantity,
                            cUnit = loading.cUnit,
                            cQuantity = loading.cQuantity
                        };
                    }
                    Session.ExecuteNonQuery(@"update ShippingPlan set ShippingPlanStatus = 9 where Oid = @oid", new string[] { "@oid" }, new object[] { detail.ExpeditionDetail.ShippingPlan.Oid });
                    //detail.SalesOrderDetail.ShippedQuantity -= detail.LoadedQuantity;
                    //detail.SalesOrderDetail.ShippedcQuantity -= detail.LoadedcQuantity;
                }
                Delivery.SalesWaybill = null;
                Delivery.DeliveryBlockStatus = DeliveryBlockStatus.DeliveryBlock;
                Delivery.DeliveryBlockage = true;
                foreach (DeliveryDetail detail in Delivery.DeliveryDetails)
                {
                    detail.SalesWaybillDetail = null;
                }
                Delivery.DeliveryStatus = DeliveryStatus.WaitingforWaybill;
            }
        }
        // Fields...
        private Delivery _Delivery;
        private Expedition _Expedition;
        private Contact _Contact;
        private DateTime _ShippingDate;
        private string _Note;
        private string _ReferenceWaybillNumber;
        private DateTime _WaybillDate;
        private string _WaybillNumber;

        [RuleRequiredField]
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ReferenceWaybillNumber
        {
            get
            {
                return _ReferenceWaybillNumber;
            }
            set
            {
                SetPropertyValue("ReferenceWaybillNumber", ref _ReferenceWaybillNumber, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        public DateTime ShippingDate
        {
            get
            {
                return _ShippingDate;
            }
            set
            {
                SetPropertyValue("ShippingDate", ref _ShippingDate, value);
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

        [VisibleInDetailView(false)]
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
        public Delivery Delivery
        {
            get
            {
                return _Delivery;
            }
            set
            {
                SetPropertyValue("Delivery", ref _Delivery, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<SalesWaybillDetail> SalesWaybillDetails
        {
            get { return GetCollection<SalesWaybillDetail>("SalesWaybillDetails"); }
        }

        #region functions
        public void RecalculateNumbers()
        {
            if (IsLoading) return;
            int number = 10;
            foreach (SalesWaybillDetail item in SalesWaybillDetails)
            {
                item.LineNumber = number;
                number = number + 10;
            }
        }
        #endregion
    }
}
