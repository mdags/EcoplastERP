using System;
using System.Web.UI;
using System.Web.Services;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.PTC
{
    public partial class erusluproduction : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string BarcodeGrossInfo(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            if (barcode.StartsWith("M"))
            {
                Store store = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.Code = '901'", barcode));
                return store != null ? string.Format("{0:n2}", store.Quantity) : "0,00";
            }
            else
            {
                return string.Format("{0:n2}", Convert.ToDecimal(session.Evaluate<Store>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.Code = '901'", barcode))));
            }
        }
        [WebMethod]
        public static string BarcodeNetInfo(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            if (barcode.StartsWith("M"))
            {
                Store store = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.Code = '901'", barcode));
                return store != null ? string.Format("{0:n2}", store.cQuantity) : "0,00";
            }
            else
            {
                return string.Format("{0:n2}", Convert.ToDecimal(session.Evaluate<Store>(CriteriaOperator.Parse("SUM(cQuantity)"), CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.Code = '901'", barcode))));
            }
        }
        [WebMethod]
        public static string BeginTransfer(string barcode)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(barcode))
            {
                Session session = modal.XpoHelper.GetNewSession();
                var headerId = Guid.NewGuid();
                MovementType outType = session.FindObject<MovementType>(new BinaryOperator("Code", "P111"));
                if (barcode.StartsWith("M")) //Barkod ise
                {
                    var store = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.Code = '901'", barcode));
                    if (store != null)
                    {
                        session.BeginTransaction();
                        try
                        {
                            Movement movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = outType, Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                            movement.Save();

                            session.CommitTransaction();
                        }
                        catch
                        {
                            session.RollbackTransaction();
                            result = "İşlem sırasında bir hata oluştu.";
                        }
                    }
                    else
                    {
                        result = "Barkod depoda bulunamadı.";
                    }
                }
                else if (barcode.StartsWith("P")) //Palet ise
                {
                    var store = session.FindObject<Store>(CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.Code = '901'", barcode));
                    if (store != null)
                    {
                        session.BeginTransaction();
                        try
                        {
                            session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, NULL, GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@headerId", "@movementType", "@barcode" }, new object[] { headerId, outType.Oid, barcode });
                            modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete Store where GCRecord is null and PaletteNumber = @barcode", new string[] { "@barcode" }, new object[] { barcode });
                            session.CommitTransaction();
                        }
                        catch
                        {
                            session.RollbackTransaction();
                            throw;
                        }
                    }
                    else
                    {
                        result = "Palet depoda bulunamadı.";
                    }
                }
            }
            return result;
        }
    }
}