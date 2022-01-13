using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.AuditTrail;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingtransfer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }
        [WebMethod]
        public static string CheckDelivery(string expeditionNumber, string deliveryNumber)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Delivery delivery = session.FindObject<Delivery>(CriteriaOperator.Parse("DeliveryNumber = ?", deliveryNumber));
            if (delivery != null)
            {
                if (delivery.Expedition.ExpeditionNumber != expeditionNumber)
                {
                    return "Bu teslimat belgesi girilen sefere ait değil.";
                }
                else return string.Empty;
            }
            else return "Teslimat belgesi bulunamadı.";
        }

        [WebMethod]
        public static List<LoadingInfoTable> GetLoading(string deliverynumber)
        {
            Session session = modal.XpoHelper.GetNewSession();
            List<LoadingInfoTable> list = new List<LoadingInfoTable>();
            LoadingInfoTable table = new LoadingInfoTable();
            Delivery delivery = session.FindObject<Delivery>(new BinaryOperator("DeliveryNumber", deliverynumber));
            if (delivery != null)
            {
                int paletteCount = 0, bobbinCount = 0;
                decimal quantity = 0;
                foreach (DeliveryDetail deliveryDetail in delivery.DeliveryDetails)
                {
                    paletteCount += Convert.ToInt32(session.ExecuteScalar(@"select isnull((select COUNT(distinct PaletteNumber) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = @deliveryDetail), 0)", new string[] { "@deliveryDetail" }, new object[] { deliveryDetail.Oid }));
                    bobbinCount += Convert.ToInt32(session.Evaluate<DeliveryDetailLoading>(CriteriaOperator.Parse("COUNT()"), CriteriaOperator.Parse("DeliveryDetail = ?", deliveryDetail)));
                    quantity += Convert.ToDecimal(session.Evaluate<DeliveryDetailLoading>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("DeliveryDetail = ?", deliveryDetail)));
                }
                table.PaletteCount = paletteCount;
                table.BobbinCount = bobbinCount;
                table.Quantity = quantity;
            }
            else
            {
                table.PaletteCount = 0;
                table.BobbinCount = 0;
                table.Quantity = 0;
            }
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
                else
                {
                    BarcodeInfoTable barcodeInfo = new BarcodeInfoTable() { BobbinCount = 0, GrossQuantity = 0, Tare = 0, NetQuantity = 0 };
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
        public static string BeginTransfer(string deliveryNumber, string barcode)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(deliveryNumber))
            {
                if (!string.IsNullOrEmpty(barcode))
                {
                    Session session = modal.XpoHelper.GetNewSession();
                    AuditTrailService.Instance.EndSessionAudit(session);
                    Delivery delivery = session.FindObject<Delivery>(new BinaryOperator("DeliveryNumber", deliveryNumber));
                    if (delivery.DeliveryStatus == Module.DeliveryStatus.Completed)
                    {
                        result = "Teslimat için irsaliye kesilmiş okutma yapılamaz.";
                    }
                    else
                    {
                        if (barcode.StartsWith("M")) //Barkod ise
                        {
                            Store searchStore = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.LoadingWarehouse = true", barcode));
                            if (searchStore != null)
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
                                    if (store.Product.ProductGroup.Name.Contains("STANDART"))
                                    {
                                        DeliveryDetail searchDeliveryDetail = session.FindObject<DeliveryDetail>(CriteriaOperator.Parse("Delivery = ? and SalesOrderDetail.Product = ?", delivery, store.Product));
                                        if (searchDeliveryDetail != null)
                                        {
                                            if (searchDeliveryDetail.SalesOrderDetail != store.SalesOrderDetail)
                                            {
                                                MovementType inType = session.FindObject<MovementType>(new BinaryOperator("Code", "P132"));
                                                MovementType outType = session.FindObject<MovementType>(new BinaryOperator("Code", "P133"));
                                                decimal quantity = 0;
                                                if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "AD")
                                                {
                                                    quantity = searchDeliveryDetail.SalesOrderDetail.Product.OuterPackingInPiece;
                                                }
                                                else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "RL")
                                                {
                                                    quantity = searchDeliveryDetail.SalesOrderDetail.Product.OuterPackingRollCount;
                                                }
                                                else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "KL")
                                                {
                                                    quantity = 1;
                                                }
                                                else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "PK")
                                                {
                                                    quantity = 1;
                                                }
                                                else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "M2")
                                                {
                                                    quantity = searchDeliveryDetail.SalesOrderDetail.Product.Width * searchDeliveryDetail.SalesOrderDetail.Product.Lenght / 1000;
                                                }
                                                else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "MT")
                                                {
                                                    quantity = searchDeliveryDetail.SalesOrderDetail.Product.Lenght;
                                                }
                                                if (quantity == 0)
                                                {
                                                    session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, inType.Oid, searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                    session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, outType.Oid, store.SalesOrderDetail.Oid, barcode });
                                                    session.ExecuteNonQuery(@"update Store set SalesOrderDetail = @salesOrderDetail where GCRecord is null and Barcode = @barcode", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                    session.ExecuteNonQuery(@"update Production set SalesOrderDetail = @salesOrderDetail where GCRecord is null and Barcode = @barcode", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                }
                                                else
                                                {
                                                    session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, (select Unit from SalesOrderDetail where Oid = @salesOrderDetail), @quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@quantity", "@barcode" }, new object[] { headerId, inType.Oid, searchDeliveryDetail.SalesOrderDetail.Oid, quantity, barcode });
                                                    session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, outType.Oid, store.SalesOrderDetail.Oid, barcode });
                                                    session.ExecuteNonQuery(@"update Store set SalesOrderDetail = @salesOrderDetail, Unit = (select Unit from SalesOrderDetail where Oid = @salesOrderDetail), Quantity = @quantity where GCRecord is null and Barcode = @barcode", new string[] { "@salesOrderDetail", "@quantity", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, quantity, barcode });
                                                    session.ExecuteNonQuery(@"update Production set SalesOrderDetail = @salesOrderDetail, ProductionPalette = null where GCRecord is null and Barcode = @barcode", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                    store.Quantity = quantity;
                                                    store.Unit = searchDeliveryDetail.SalesOrderDetail.Unit;
                                                }

                                                store.SalesOrderDetail = searchDeliveryDetail.SalesOrderDetail;
                                            }
                                        }
                                    }

                                    DeliveryDetail deliveryDetail = session.FindObject<DeliveryDetail>(CriteriaOperator.Parse("Delivery = ? and SalesOrderDetail = ?", delivery, store.SalesOrderDetail));
                                    if (deliveryDetail == null)
                                    {
                                        result = "Okutulan barkodun siparişi seçilen teslimat belgesinde değil.";
                                    }
                                    else
                                    {
                                        decimal totalLoadedQuantity = Convert.ToDecimal(session.Evaluate<DeliveryDetailLoading>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("DeliveryDetail = ?", deliveryDetail)));
                                        if (deliveryDetail.ReadControl && deliveryDetail.Quantity < (totalLoadedQuantity + store.Quantity))
                                        {
                                            result = "Okutulan miktar sevk bildirilen miktardan fazla olamaz.";
                                        }
                                        else
                                        {
                                            Warehouse loadingWarehouse = session.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
                                            //var headerId = Guid.NewGuid();
                                            session.BeginTransaction();
                                            try
                                            {
                                                Movement movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P127")), Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                                                movement.Save();

                                                movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P126")), Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = loadingWarehouse, WarehouseCell = null, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                                                movement.Save();

                                                Store newStore = new Store(session) { Product = store.Product, Barcode = store.Barcode, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = loadingWarehouse, WarehouseCell = null, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity, SalesOrderDetail = store.SalesOrderDetail };
                                                newStore.Save();

                                                DeliveryDetailLoading deliveryDetailLoading = new DeliveryDetailLoading(session) { DeliveryDetail = deliveryDetail, Barcode = barcode, PaletteNumber = store.PaletteNumber, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity, ShippingUser = session.FindObject<ShippingUser>(new BinaryOperator("Oid", Guid.Parse(HttpContext.Current.Session["userOid"].ToString()))), DeliveryLoadingType = Module.DeliveryLoadingType.WithBarcode, Production = session.FindObject<Production>(new BinaryOperator("Barcode", barcode)) };
                                                deliveryDetailLoading.Save();

                                                deliveryDetail.DeliveryDetailLoadings.Add(deliveryDetailLoading);
                                                deliveryDetail.Save();

                                                session.ExecuteNonQuery(@"update Production set ProductionPalette = null where GCRecord is null and Barcode = @barcode  update Store set PaletteNumber = null where Barcode = @barcode  update Movement set PaletteNumber = null where Barcode = @barcode", new string[] { "@barcode" }, new object[] { barcode });

                                                if (delivery.DeliveryStatus == Module.DeliveryStatus.WaitingforLoading)
                                                {
                                                    delivery.DeliveryStatus = Module.DeliveryStatus.Loading;
                                                    delivery.Save();
                                                }

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
                            }
                        }
                        else //if (barcode.StartsWith("P")) //Palet ise
                        {
                            var headerId = Guid.NewGuid();
                            ProductionPalette productionPalette = session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", barcode));
                            if (productionPalette == null)
                            {
                                result = "Palet tanımlı değil.";
                            }
                            else
                            {
                                if (productionPalette.LastWeight == 0)
                                {
                                    result = "Palet teyidi verilmemiş.";
                                }
                                else
                                {
                                    Store searchStore = session.FindObject<Store>(CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.LoadingWarehouse = true", barcode));
                                    if (searchStore != null)
                                    {
                                        result = "Palet zaten okutulmuş.";
                                    }
                                    else
                                    {
                                        searchStore = session.FindObject<Store>(CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.ShippingWarehouse = true", barcode));
                                        if (searchStore == null)
                                        {
                                            result = "Palet sevk depoda bulunamadı.";
                                        }
                                        else
                                        {
                                            if (searchStore.Product.ProductGroup.Name.Contains("STANDART"))
                                            {
                                                DeliveryDetail searchDeliveryDetail = session.FindObject<DeliveryDetail>(CriteriaOperator.Parse("Delivery = ? and SalesOrderDetail.Product = ?", delivery, searchStore.Product));
                                                if (searchDeliveryDetail != null)
                                                {
                                                    if (searchDeliveryDetail.SalesOrderDetail != searchStore.SalesOrderDetail)
                                                    {
                                                        MovementType inType = session.FindObject<MovementType>(new BinaryOperator("Code", "P132"));
                                                        MovementType outType = session.FindObject<MovementType>(new BinaryOperator("Code", "P133"));
                                                        decimal quantity = 0;
                                                        if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "AD")
                                                        {
                                                            quantity = searchDeliveryDetail.SalesOrderDetail.Product.OuterPackingInPiece;
                                                        }
                                                        else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "RL")
                                                        {
                                                            quantity = searchDeliveryDetail.SalesOrderDetail.Product.OuterPackingRollCount;
                                                        }
                                                        else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "KL")
                                                        {
                                                            quantity = 1;
                                                        }
                                                        else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "PK")
                                                        {
                                                            quantity = 1;
                                                        }
                                                        else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "M2")
                                                        {
                                                            quantity = searchDeliveryDetail.SalesOrderDetail.Product.Width * searchDeliveryDetail.SalesOrderDetail.Product.Lenght / 1000;
                                                        }
                                                        else if (searchDeliveryDetail.SalesOrderDetail.Unit.Code == "MT")
                                                        {
                                                            quantity = searchDeliveryDetail.SalesOrderDetail.Product.Lenght;
                                                        }
                                                        if (quantity == 0)
                                                        {
                                                            session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, inType.Oid, searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                            session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, outType.Oid, searchStore.SalesOrderDetail.Oid, barcode });
                                                            session.ExecuteNonQuery(@"update Store set SalesOrderDetail = @salesOrderDetail where GCRecord is null and PaletteNumber = @barcode", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                            session.ExecuteNonQuery(@"update Production set SalesOrderDetail = @salesOrderDetail where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where PaletteNumber = @barcode)", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                        }
                                                        else
                                                        {
                                                            session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, @quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@quantity", "@barcode" }, new object[] { headerId, inType.Oid, searchDeliveryDetail.SalesOrderDetail.Oid, quantity, barcode });
                                                            session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, (select Unit from SalesOrderDetail where Oid = @salesOrderDetail), Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, outType.Oid, searchStore.SalesOrderDetail.Oid, barcode });
                                                            session.ExecuteNonQuery(@"update Store set SalesOrderDetail = @salesOrderDetail, Unit = (select Unit from SalesOrderDetail where Oid = @salesOrderDetail), Quantity = @quantity where GCRecord is null and PaletteNumber = @barcode", new string[] { "@salesOrderDetail", "@quantity", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, quantity, barcode });
                                                            session.ExecuteNonQuery(@"update Production set SalesOrderDetail = @salesOrderDetail where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where PaletteNumber = @barcode)", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { searchDeliveryDetail.SalesOrderDetail.Oid, barcode });
                                                        }

                                                        searchStore.SalesOrderDetail = searchDeliveryDetail.SalesOrderDetail;
                                                    }
                                                }
                                            }

                                            DeliveryDetail deliveryDetail = session.FindObject<DeliveryDetail>(CriteriaOperator.Parse("Delivery = ? and SalesOrderDetail = ?", delivery, searchStore.SalesOrderDetail));
                                            if (deliveryDetail == null)
                                            {
                                                result = "Okutulan barkodun siparişi seçilen teslimat belgesinde değil.";
                                            }
                                            else
                                            {
                                                decimal totalLoadedQuantity = Convert.ToDecimal(session.Evaluate<DeliveryDetailLoading>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("DeliveryDetail = ?", deliveryDetail)));
                                                decimal paletteTotal = Convert.ToDecimal(session.Evaluate<Store>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("PaletteNumber = ?", barcode)));
                                                if (deliveryDetail.ReadControl && deliveryDetail.Quantity < (totalLoadedQuantity + paletteTotal))
                                                {
                                                    result = "Okutulan miktar sevk bildirilen miktardan fazla olamaz.";
                                                }
                                                else
                                                {
                                                    Session session1 = modal.XpoHelper.GetNewSession();
                                                    DeliveryDetail selectedDeliveryDetail = session1.FindObject<DeliveryDetail>(new BinaryOperator("Oid", deliveryDetail.Oid));
                                                    ShippingUser shippingUser = session1.FindObject<ShippingUser>(new BinaryOperator("Oid", Guid.Parse(HttpContext.Current.Session["userOid"].ToString())));
                                                    Warehouse loadingWarehouse = session1.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
                                                    MovementType inType = session.FindObject<MovementType>(new BinaryOperator("Code", "P126"));
                                                    MovementType outType = session.FindObject<MovementType>(new BinaryOperator("Code", "P127"));
                                                    session1.BeginTransaction();
                                                    try
                                                    {
                                                        session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, @warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@warehouse", "@barcode" }, new object[] { headerId, inType.Oid, loadingWarehouse.Oid, barcode });
                                                        session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@barcode" }, new object[] { headerId, outType.Oid, barcode });

                                                        session.ExecuteNonQuery(@"insert into Store(Oid, Product, Barcode, SalesOrderDetail, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) select NEWID(), Product, Barcode, SalesOrderDetail, PartyNumber, PaletteNumber, @warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@warehouse", "@barcode" }, new object[] { loadingWarehouse.Oid, barcode });
                                                        session.ExecuteNonQuery(@"delete Store where GCRecord is null and PaletteNumber = @barcode and Warehouse != @warehouse", new string[] { "@warehouse", "@barcode" }, new object[] { loadingWarehouse.Oid, barcode });

                                                        session.ExecuteNonQuery(@"insert into DeliveryDetailLoading(Oid, DeliveryDetail, LoadingDate, Barcode, PaletteNumber, Unit, Quantity, cUnit, cQuantity, ShippingUser, DeliveryLoadingType, Production, OptimisticLockField, GCRecord) select NEWID(), @selectedDeliveryDetail, GETDATE(), Barcode, PaletteNumber, Unit, Quantity, cUnit, cQuantity, @shippingUser, 0, (select Oid from Production where Barcode = Store.Barcode), 0, NULL from Store where GCRecord is null and PaletteNumber = @barcode and Warehouse = @warehouse", new string[] { "@selectedDeliveryDetail", "@shippingUser", "@barcode", "@warehouse" }, new object[] { selectedDeliveryDetail.Oid, shippingUser.Oid, barcode, loadingWarehouse.Oid });

                                                        session1.CommitTransaction();
                                                    }
                                                    catch
                                                    {
                                                        session1.RollbackTransaction();
                                                        result = "İşlem sırasında bir hata oluştu.";
                                                    }

                                                    modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete Store where PaletteNumber = @barcode and GCRecord is not null", new string[] { "@barcode" }, new object[] { barcode });

                                                    result = "";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else result = "Barkod boş olamaz.";
            }
            else result = "Teslimat No boş olamaz.";


            return result;
        }
    }
}