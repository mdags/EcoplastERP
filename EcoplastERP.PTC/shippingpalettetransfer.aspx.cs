using System;
using System.Web.UI;
using System.Web.Services;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingpalettetransfer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }
        [WebMethod]
        public static string CheckPalette(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            ProductionPalette productionPalette = session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", barcode));
            if (productionPalette == null) return "Palet tanımlı değil.";
            else return string.Empty;
        }
        [WebMethod]
        public static string BarcodeGrossInfo(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            var production = session.FindObject<Production>(new BinaryOperator("Barcode", barcode));
            return production != null ? string.Format("{0:n2}", production.GrossQuantity) : "0,00";
        }
        [WebMethod]
        public static string BarcodeNetInfo(string barcode)
        {
            Session session = modal.XpoHelper.GetNewSession();
            var production = session.FindObject<Production>(new BinaryOperator("Barcode", barcode));
            return production != null ? string.Format("{0:n2}", production.NetQuantity) : "0,00";
        }
        [WebMethod]
        public static string BeginTransfer(string barcode, string palette)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(barcode))
            {
                if (!string.IsNullOrEmpty(palette))
                {
                    Session session = modal.XpoHelper.GetNewSession();
                    var headerId = Guid.NewGuid();
                    var loadingStore = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.LoadingWarehouse = true"));
                    if (loadingStore != null)
                    {
                        result = "Kamyon depoda olan barkod için palet transfer işlemi yapılamaz.";
                    }
                    else
                    {
                        var productionPalette = session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", palette));
                        if (productionPalette != null)
                        {
                            var store = session.FindObject<Store>(new BinaryOperator("Barcode", barcode));
                            if (store != null)
                            {
                                session.BeginTransaction();
                                try
                                {
                                    Movement movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P125")), Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                                    movement.Save();

                                    movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P124")), Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = palette, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                                    movement.Save();

                                    Store newStore = new Store(session) { Product = store.Product, Barcode = store.Barcode, PartyNumber = store.PartyNumber, PaletteNumber = palette, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity, SalesOrderDetail = store.SalesOrderDetail };
                                    newStore.Save();

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
                            else
                            {
                                result = "Barkod depoda bulunamadı.";
                            }
                        }
                        else
                        {
                            result = "Girdiğiniz palet tanımlı değil.";
                        }
                    }
                }
                else
                {
                    result = "Palet boş olamaz.";
                }
            }
            else
            {
                result = "Barkod boş olamaz.";
            }

            return result;
        }
    }
}