using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.AuditTrail;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class FastShippingViewController : ViewController
    {
        public FastShippingViewController()
        {
            InitializeComponent();
        }

        private void CompleteFastShippingAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            AuditTrailService.Instance.EndSessionAudit(((XPObjectSpace)objectSpace).Session);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select C.Oid as ContactOid from FastShipping F inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where F.GCRecord is null group by C.Oid", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                Contact contact = objectSpace.FindObject<Contact>(new BinaryOperator("Oid", Guid.Parse(dr["ContactOid"].ToString())));
                Expedition expedition = objectSpace.CreateObject<Expedition>();
                expedition.ExpeditionStatus = ExpeditionStatus.WaitingforDocumentConfirm;
                expedition.Truck = contact.FastShippingTruck;
                expedition.TruckDriver = contact.FastShippingTruckDriver;
                expedition.ExpeditionCompleteDate = DateTime.Now;

                Delivery delivery = objectSpace.CreateObject<Delivery>();
                delivery.DeliveryBlockStatus = DeliveryBlockStatus.Documentable;
                delivery.Contact = contact;
                delivery.ShippingContact = contact;
                delivery.Expedition = expedition;

                dt = new DataTable();
                da = new SqlDataAdapter(string.Format(@"select F.SalesOrderDetail, F.Unit, SUM(F.Quantity) as Quantity, F.cUnit, SUM(F.cQuantity) as cQuantity from FastShipping F inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where F.GCRecord is null and C.Oid = '{0}' group by F.SalesOrderDetail, F.Unit, F.cUnit", contact.Oid), ((XPObjectSpace)objectSpace).Session.ConnectionString);
                da.Fill(dt);

                int lineNumber = 0;
                foreach (DataRow row in dt.Rows)
                {
                    lineNumber += 10;
                    SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", Guid.Parse(row["SalesOrderDetail"].ToString())));
                    ShippingPlan shippingPlan = objectSpace.CreateObject<ShippingPlan>();
                    shippingPlan.ShippingPlanStatus = ShippingPlanStatus.Completed;
                    shippingPlan.SetupDate = DateTime.Now;
                    shippingPlan.LineNumber = lineNumber;
                    shippingPlan.SalesOrderDetail = salesOrderDetail;
                    shippingPlan.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString())));
                    shippingPlan.Quantity = Convert.ToDecimal(row["Quantity"]);
                    shippingPlan.cUnit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["cUnit"].ToString())));
                    shippingPlan.cQuantity = Convert.ToDecimal(row["cQuantity"]);
                    shippingPlan.NotifiedUser = objectSpace.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)) != null ? objectSpace.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)).NameSurname : string.Empty;

                    ExpeditionDetail expeditionDetail = objectSpace.CreateObject<ExpeditionDetail>();
                    expeditionDetail.Expedition = expedition;
                    expeditionDetail.LineNumber = lineNumber;
                    expeditionDetail.SalesOrderDetail = salesOrderDetail;
                    expeditionDetail.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString())));
                    expeditionDetail.Quantity = Convert.ToDecimal(row["Quantity"]);
                    expeditionDetail.cUnit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["cUnit"].ToString())));
                    expeditionDetail.cQuantity = Convert.ToDecimal(row["cQuantity"]);
                    expeditionDetail.ShippingPlan = shippingPlan;

                    DeliveryDetail deliveryDetail = objectSpace.CreateObject<DeliveryDetail>();
                    deliveryDetail.Delivery = delivery;
                    deliveryDetail.LineNumber = lineNumber;
                    deliveryDetail.SalesOrderDetail = salesOrderDetail;
                    deliveryDetail.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString())));
                    deliveryDetail.Quantity = Convert.ToDecimal(row["Quantity"]);
                    deliveryDetail.cUnit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["cUnit"].ToString())));
                    deliveryDetail.cQuantity = Convert.ToDecimal(row["cQuantity"]);
                    deliveryDetail.ReadControl = false;
                    deliveryDetail.ExpeditionDetail = expeditionDetail;

                    delivery.TransportType = salesOrderDetail.SalesOrder.TransportType;
                    delivery.DeliveryDetails.Add(deliveryDetail);

                    int loadingLineNumber = 0;
                    XPCollection<FastShipping> loadingList = new XPCollection<FastShipping>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("SalesOrderDetail = ?", salesOrderDetail));
                    foreach (FastShipping loading in loadingList)
                    {
                        loadingLineNumber += 10;
                        DeliveryDetailLoading deliveryDetailLoading = objectSpace.CreateObject<DeliveryDetailLoading>();
                        deliveryDetailLoading.DeliveryDetail = deliveryDetail;
                        deliveryDetailLoading.LineNumber = loadingLineNumber;
                        deliveryDetailLoading.LoadingDate = loading.LoadingDate;
                        deliveryDetailLoading.Barcode = loading.Barcode;
                        deliveryDetailLoading.PaletteNumber = loading.PaletteNumber;
                        deliveryDetailLoading.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", loading.Unit.Oid));
                        deliveryDetailLoading.Quantity = loading.Quantity;
                        deliveryDetailLoading.cUnit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", loading.cUnit.Oid));
                        deliveryDetailLoading.cQuantity = loading.cQuantity;
                        deliveryDetailLoading.ShippingUser = objectSpace.FindObject<ShippingUser>(new BinaryOperator("Oid", loading.ShippingUser.Oid));
                        deliveryDetailLoading.DeliveryLoadingType = loading.DeliveryLoadingType;
                        deliveryDetailLoading.Production = objectSpace.FindObject<Production>(new BinaryOperator("Barcode", loading.Barcode));
                        deliveryDetail.DeliveryDetailLoadings.Add(deliveryDetailLoading);
                        loading.ClosedDate = DateTime.Now;
                    }

                    expedition.ExpeditionDetails.Add(expeditionDetail);
                }
            }

            Warehouse loadingWarehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
            var headerId = Guid.NewGuid();
            MovementType inMovementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P126"));
            MovementType outMovementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P127"));
            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber in (select PaletteNumber from FastShipping where GCRecord is null group by PaletteNumber)", new string[] { "@headerId", "@movementType" }, new object[] { headerId, outMovementType.Oid });
            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber in (select PaletteNumber from FastShipping where GCRecord is null group by PaletteNumber)", new string[] { "@headerId", "@movementType" }, new object[] { headerId, inMovementType.Oid });

            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Store set Warehouse = @warehouse where PaletteNumber in (select PaletteNumber from FastShipping where GCRecord is null group by PaletteNumber)", new string[] { "@warehouse" }, new object[] { loadingWarehouse.Oid });

            objectSpace.CommitChanges();
            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"delete FastShipping");
            XtraMessageBox.Show("İşlem tamamlandı.");

            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if (View is ListView)
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }
    }
}