using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.AuditTrail;
using EcoplastERP.Module;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingfast : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }
        [WebMethod]
        public static List<LoadingInfoTable> GetFastShippingList()
        {
            Session session = modal.XpoHelper.GetNewSession();
            List<LoadingInfoTable> list = new List<LoadingInfoTable>();
            LoadingInfoTable table = new LoadingInfoTable();
            table.PaletteCount = Convert.ToInt32(session.ExecuteScalar(@"select isnull((select COUNT(distinct PaletteNumber) from FastShipping where GCRecord is null), 0)"));
            table.BobbinCount = Convert.ToInt32(session.Evaluate<FastShipping>(CriteriaOperator.Parse("COUNT()"), CriteriaOperator.Parse("1 = 1")));
            table.Quantity = Convert.ToDecimal(session.Evaluate<FastShipping>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("1 = 1")));
            list.Add(table);

            return list;
        }

        [WebMethod]
        public static List<BarcodeInfoTable> GetBarcode(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            List<BarcodeInfoTable> list = new List<BarcodeInfoTable>();
            if (barcode.StartsWith("M"))
            {
                Production production = session.FindObject<Production>(new BinaryOperator("Barcode", barcode));
                if (production != null)
                {
                    BarcodeInfoTable barcodeInfo = new BarcodeInfoTable() { BobbinCount = 1, GrossQuantity = production.GrossQuantity, Tare = 0, NetQuantity = production.NetQuantity };
                    list.Add(barcodeInfo);
                }
            }
            else
            {
                BarcodeInfoTable barcodeInfo = new BarcodeInfoTable();
                Production production = session.FindObject<Production>(new BinaryOperator("ProductionPalette.PaletteNumber", barcode));
                if (production != null)
                {
                    barcodeInfo.BobbinCount = Convert.ToInt32(session.ExecuteScalar(@"select count(*) from Production where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber)", new string[] { "@paletteNumber" }, new object[] { barcode }));
                    barcodeInfo.GrossQuantity = Convert.ToDecimal(session.ExecuteScalar(@"select sum(GrossQuantity) from Production where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber)", new string[] { "@paletteNumber" }, new object[] { barcode }));
                    barcodeInfo.Tare = production.ProductionPalette.Tare;
                    barcodeInfo.NetQuantity = Convert.ToDecimal(session.ExecuteScalar(@"select sum(NetQuantity) from Production where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber)", new string[] { "@paletteNumber" }, new object[] { barcode }));
                }
                else
                {
                    Store store = session.FindObject<Store>(new BinaryOperator("PaletteNumber", barcode));
                    if (store != null)
                    {
                        barcodeInfo.BobbinCount = Convert.ToInt32(session.ExecuteScalar(@"select count(*) from Store where GCRecord is null and PaletteNumber = @paletteNumber", new string[] { "@paletteNumber" }, new object[] { barcode }));
                        barcodeInfo.GrossQuantity = Convert.ToDecimal(session.ExecuteScalar(@"select sum(Quantity) from Store where GCRecord is null and PaletteNumber = @paletteNumber", new string[] { "@paletteNumber" }, new object[] { barcode }));
                        barcodeInfo.Tare = 0;
                        barcodeInfo.NetQuantity = Convert.ToDecimal(session.ExecuteScalar(@"select sum(cQuantity) from Store where GCRecord is null and PaletteNumber = @paletteNumber", new string[] { "@paletteNumber" }, new object[] { barcode }));
                    }
                    else
                    {
                        barcodeInfo.BobbinCount = 0;
                        barcodeInfo.GrossQuantity = 0;
                        barcodeInfo.Tare = 0;
                        barcodeInfo.NetQuantity = 0;
                    }
                }
                list.Add(barcodeInfo);
            }

            return list;
        }

        [WebMethod]
        public static string BeginTransfer(string barcode)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(barcode))
            {
                Session session = modal.XpoHelper.GetNewSession();
                AuditTrailService.Instance.EndSessionAudit(session);
                if (barcode.StartsWith("M")) //Barkod ise
                {
                    FastShipping searchFastShipping = session.FindObject<FastShipping>(CriteriaOperator.Parse("Barcode = ?", barcode));
                    if (searchFastShipping != null)
                    {
                        result = "Barkod zaten okutulmuş.";
                    }
                    else
                    {
                        var headerId = Guid.NewGuid();
                        Store store = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.ShippingWarehouse = true", barcode));
                        if (store == null)
                        {
                            result = "Barkod sevk depoda bulunamadı.";
                        }
                        else
                        {
                            Warehouse loadingWarehouse = session.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
                            session.BeginTransaction();
                            try
                            {
                                FastShipping fastShipping = new FastShipping(session) { Barcode = barcode, PaletteNumber = store.PaletteNumber, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity, ShippingUser = session.FindObject<ShippingUser>(new BinaryOperator("Oid", Guid.Parse(HttpContext.Current.Session["userOid"].ToString()))), DeliveryLoadingType = Module.DeliveryLoadingType.WithBarcode, SalesOrderDetail = store.SalesOrderDetail };
                                fastShipping.Save();

                                session.CommitTransaction();

                                modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete Store where Barcode = @barcode and GCRecord is not null", new string[] { "@barcode" }, new object[] { barcode });

                                result = "";
                            }
                            catch
                            {
                                session.RollbackTransaction();
                                result = "İşlem sırasında bir hata oluştu.";
                            }
                        }
                    }

                }
                else //if (barcode.StartsWith("P")) //Palet ise
                {
                    var headerId = Guid.NewGuid();
                    FastShipping searchFastShipping = session.FindObject<FastShipping>(CriteriaOperator.Parse("PaletteNumber = ?", barcode));
                    if (searchFastShipping != null)
                    {
                        result = "Palet zaten okutulmuş.";
                    }
                    else
                    {
                        Store store = session.FindObject<Store>(CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.ShippingWarehouse = true", barcode));
                        if (store == null)
                        {
                            result = "Palet sevk depoda bulunamadı.";
                        }
                        else
                        {
                            session.ExecuteNonQuery(@"insert into FastShipping(Oid, Barcode, PaletteNumber, Unit, Quantity, cUnit, cQuantity, ShippingUser, DeliveryLoadingType, SalesOrderDetail, OptimisticLockField, GCRecord) select NEWID(), Barcode, PaletteNumber, Unit, Quantity, cUnit, cQuantity, @shippingUser, 0, SalesOrderDetail, 0, NULL from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@shippingUser", "@barcode" }, new object[] { Guid.Parse(HttpContext.Current.Session["userOid"].ToString()), barcode });

                            modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete Store where PaletteNumber = @barcode and GCRecord is not null", new string[] { "@barcode" }, new object[] { barcode });

                            result = "";
                        }
                    }

                }
            }
            else result = "Barkod boş olamaz.";

            return result;
        }

        [WebMethod]
        public static string EndFastShipping()
        {
            string result = string.Empty;

            Session session = modal.XpoHelper.GetNewSession();
            AuditTrailService.Instance.EndSessionAudit(session);
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString, CommandType.Text, @"select C.Oid as ContactOid from FastShipping F inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where F.GCRecord is null group by C.Oid").Tables[0];

            session.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Contact contact = session.FindObject<Contact>(new BinaryOperator("Oid", Guid.Parse(dr["ContactOid"].ToString())));
                    Expedition expedition = new Expedition(session)
                    {
                        ExpeditionStatus = ExpeditionStatus.WaitingforDocumentConfirm,
                        Truck = contact.FastShippingTruck,
                        TruckDriver = contact.FastShippingTruckDriver,
                        ExpeditionCompleteDate = DateTime.Now
                    };
                    expedition.Save();

                    Delivery delivery = new Delivery(session)
                    {
                        DeliveryBlockStatus = DeliveryBlockStatus.Documentable,
                        Contact = contact,
                        ShippingContact = contact,
                        Expedition = expedition,
                        DeliveryBlockage = false
                    };
                    delivery.Save();

                    dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString, CommandType.Text, @"select F.SalesOrderDetail, F.Unit, SUM(F.Quantity) as Quantity, F.cUnit, SUM(F.cQuantity) as cQuantity from FastShipping F inner join SalesOrderDetail D on D.Oid = F.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where F.GCRecord is null and C.Oid = @code group by F.SalesOrderDetail, F.Unit, F.cUnit", new SqlParameter("@code", contact.Oid)).Tables[0];

                    int lineNumber = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        lineNumber += 10;
                        SalesOrderDetail salesOrderDetail = session.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", Guid.Parse(row["SalesOrderDetail"].ToString())));

                        ShippingPlan shippingPlan = new ShippingPlan(session)
                        {
                            ShippingPlanStatus = ShippingPlanStatus.Completed,
                            SetupDate = DateTime.Now,
                            LineNumber = lineNumber,
                            SalesOrderDetail = salesOrderDetail,
                            Unit = session.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString()))),
                            Quantity = Convert.ToDecimal(row["Quantity"]),
                            cUnit = session.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["cUnit"].ToString()))),
                            cQuantity = Convert.ToDecimal(row["cQuantity"]),
                            NotifiedUser = session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)) != null ? session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)).NameSurname : string.Empty
                        };
                        shippingPlan.Save();

                        ExpeditionDetail expeditionDetail = new ExpeditionDetail(session)
                        {
                            Expedition = expedition,
                            LineNumber = lineNumber,
                            SalesOrderDetail = salesOrderDetail,
                            Unit = session.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString()))),
                            Quantity = Convert.ToDecimal(row["Quantity"]),
                            cUnit = session.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["cUnit"].ToString()))),
                            cQuantity = Convert.ToDecimal(row["cQuantity"]),
                            ShippingPlan = shippingPlan
                        };
                        expeditionDetail.Save();

                        DeliveryDetail deliveryDetail = new DeliveryDetail(session)
                        {
                            Delivery = delivery,
                            LineNumber = lineNumber,
                            SalesOrderDetail = salesOrderDetail,
                            Unit = session.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["Unit"].ToString()))),
                            Quantity = Convert.ToDecimal(row["Quantity"]),
                            cUnit = session.FindObject<Unit>(new BinaryOperator("Oid", Guid.Parse(row["cUnit"].ToString()))),
                            cQuantity = Convert.ToDecimal(row["cQuantity"]),
                            ReadControl = false,
                            ExpeditionDetail = expeditionDetail
                        };
                        deliveryDetail.Save();

                        delivery.TransportType = salesOrderDetail.SalesOrder.TransportType;
                        delivery.DeliveryDetails.Add(deliveryDetail);

                        int loadingLineNumber = 0;
                        XPCollection<FastShipping> loadingList = new XPCollection<FastShipping>(session, CriteriaOperator.Parse("SalesOrderDetail = ?", salesOrderDetail));
                        foreach (FastShipping loading in loadingList)
                        {
                            loadingLineNumber += 10;
                            DeliveryDetailLoading deliveryDetailLoading = new DeliveryDetailLoading(session)
                            {
                                DeliveryDetail = deliveryDetail,
                                LineNumber = loadingLineNumber,
                                LoadingDate = loading.LoadingDate,
                                Barcode = loading.Barcode,
                                PaletteNumber = loading.PaletteNumber,
                                Unit = session.FindObject<Unit>(new BinaryOperator("Oid", loading.Unit.Oid)),
                                Quantity = loading.Quantity,
                                cUnit = session.FindObject<Unit>(new BinaryOperator("Oid", loading.cUnit.Oid)),
                                cQuantity = loading.cQuantity,
                                ShippingUser = session.FindObject<ShippingUser>(new BinaryOperator("Oid", loading.ShippingUser.Oid)),
                                DeliveryLoadingType = loading.DeliveryLoadingType,
                                Production = session.FindObject<Production>(new BinaryOperator("Barcode", loading.Barcode))
                            };
                            deliveryDetail.DeliveryDetailLoadings.Add(deliveryDetailLoading);
                            loading.ClosedDate = DateTime.Now;
                            deliveryDetailLoading.Save();
                        }

                        expedition.ExpeditionDetails.Add(expeditionDetail);
                    }
                }

                session.CommitTransaction();

                Warehouse loadingWarehouse = session.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
                var headerId = Guid.NewGuid();
                MovementType inMovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P126"));
                MovementType outMovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P127"));
                modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber in (select PaletteNumber from FastShipping where GCRecord is null group by PaletteNumber)", new string[] { "@headerId", "@movementType" }, new object[] { headerId, outMovementType.Oid });
                modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber in (select PaletteNumber from FastShipping where GCRecord is null group by PaletteNumber)", new string[] { "@headerId", "@movementType" }, new object[] { headerId, inMovementType.Oid });

                modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"update Store set Warehouse = @warehouse where PaletteNumber in (select PaletteNumber from FastShipping where GCRecord is null group by PaletteNumber)", new string[] { "@warehouse" }, new object[] { loadingWarehouse.Oid });

                modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete FastShipping");
            }
            catch
            {
                session.RollbackTransaction();
                result = "İşlem sırasında bir hata oluştu.";
            }

            return result;
        }
    }
}