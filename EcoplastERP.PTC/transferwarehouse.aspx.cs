using System;
using System.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.PTC
{
    public partial class transferwarehouse : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("transferlogin.aspx");
            }
        }
        [WebMethod]
        public static string GetWarehouseInfo(string warehouseCode)
        {
            string result = string.Empty;
            Session session = modal.XpoHelper.GetNewSession();
            Warehouse warehouse = session.FindObject<Warehouse>(new BinaryOperator("Code", warehouseCode));
            if (warehouse == null) result = "Depo bulunamadı.";
            return result;
        }
        [WebMethod]
        public static List<WarehouseTable> GetWarehouseCellInfo(string warehouseCellName)
        {
            Session session = modal.XpoHelper.GetNewSession();
            List<WarehouseTable> list = new List<WarehouseTable>();
            WarehouseCell warehouseCell = session.FindObject<WarehouseCell>(new BinaryOperator("Name", warehouseCellName));
            WarehouseTable warehouseInfo = new WarehouseTable();
            if (warehouseCell != null)
            {
                warehouseInfo.WarehouseCode = warehouseCell.Warehouse.Code;
                warehouseInfo.ErrorMessage = string.Empty;
                list.Add(warehouseInfo);
            }
            else
            {
                warehouseInfo.WarehouseCode = string.Empty;
                warehouseInfo.ErrorMessage = "Hücre bulunamadı.";
                list.Add(warehouseInfo);
            }
            return list;
        }
        [WebMethod]
        public static string BarcodeGrossInfo(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            if (barcode.StartsWith("M"))
            {
                Store store = session.FindObject<Store>(new BinaryOperator("Barcode", barcode));
                return store != null ? string.Format("{0:n2}", store.Quantity) : "0,00";
            }
            else
            {
                return string.Format("{0:n2}", Convert.ToDecimal(session.Evaluate<Store>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("PaletteNumber = ?", barcode))));
            }
        }
        [WebMethod]
        public static string BarcodeNetInfo(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            if (barcode.StartsWith("M"))
            {
                Store store = session.FindObject<Store>(new BinaryOperator("Barcode", barcode));
                return store != null ? string.Format("{0:n2}", store.cQuantity) : "0,00";
            }
            else
            {
                return string.Format("{0:n2}", Convert.ToDecimal(session.Evaluate<Store>(CriteriaOperator.Parse("SUM(cQuantity)"), CriteriaOperator.Parse("PaletteNumber = ?", barcode))));
            }
        }
        [WebMethod]
        public static string BeginTransfer(string warehouse, string cellName, string barcode)
        {
            string result = string.Empty;
            Session session = modal.XpoHelper.GetNewSession();
            if (!string.IsNullOrEmpty(warehouse))
            {
                Warehouse transferWarehouse = session.FindObject<Warehouse>(new BinaryOperator("Code", warehouse));
                if (transferWarehouse != null)
                {
                    WarehouseCell transferCell = !string.IsNullOrEmpty(cellName) ? session.FindObject<WarehouseCell>(new BinaryOperator("Name", cellName)) : null;
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        MovementType inType = session.FindObject<MovementType>(new BinaryOperator("Code", "P120"));
                        MovementType outType = session.FindObject<MovementType>(new BinaryOperator("Code", "P121"));
                        var headerId = Guid.NewGuid();
                        if (barcode.StartsWith("M"))
                        {
                            session.BeginTransaction();
                            try
                            {
                                session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @outType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode = @barcode
                            insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @inType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, @warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, @warehouseCell from Store where GCRecord is null and Barcode = @barcode
                            update 1Store set Warehouse = @warehouse, WarehouseCell = @warehouseCell where GCRecord is null and Barcode = @barcode", new string[] { "@headerId", "@outType", "@inType", "@warehouse", "@warehouseCell", "@barcode" }, new object[] { headerId, outType.Oid, inType.Oid, transferWarehouse.Oid, transferCell != null ? transferCell.Oid : (Guid?)null, barcode });
                                session.CommitTransaction();
                            }
                            catch
                            {
                                session.RollbackTransaction();
                                throw;
                            }
                        }
                        else if (barcode.StartsWith("P"))
                        {
                            session.BeginTransaction();
                            try
                            {
                                session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @outType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode
                            insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @inType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, @warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, @warehouseCell from Store where GCRecord is null and PaletteNumber = @barcode
                            update Store set Warehouse = @warehouse, WarehouseCell = @warehouseCell where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@outType", "@inType", "@warehouse", "@warehouseCell", "@barcode" }, new object[] { headerId, outType.Oid, inType.Oid, transferWarehouse.Oid, transferCell != null ? transferCell.Oid : (Guid?)null, barcode });
                                session.CommitTransaction();
                            }
                            catch
                            {
                                session.RollbackTransaction();
                                throw;
                            }
                        }
                    }
                    else return "Barkod boş olamaz.";
                }
                else return "Depo bulunamadı.";
            }
            else return "Depo boş olamaz.";

            return result;
        }
    }
}