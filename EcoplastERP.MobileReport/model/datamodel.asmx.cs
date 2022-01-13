using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace EcoplastERP.MobileReport.model
{
    /// <summary>
    /// Summary description for datamodel
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class datamodel : WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Login(string username, string password)
        {
            List<UserTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select * from MobileUser where UserName = @username and Password = @password", new SqlParameter("@username", username), new SqlParameter("@password", password)).Tables[0].AsEnumerable().Select(t => new UserTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                UserName = t.Field<string>("UserName"),
                Password = t.Field<string>("Password"),
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            string data = js.Serialize(list);
            Context.Response.Write(data);
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWarehouseList(string tokenKey)
        {
            List<WarehouseTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select Code, Name from Warehouse where GCRecord is null order by Code").Tables[0].AsEnumerable().Select(t => new WarehouseTable
            {
                Code = t.Field<string>("Code"),
                Name = t.Field<string>("Name")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetStoreReportByWarehouse(string tokenKey, string warehouseCode)
        {
            List<StoreTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select W.Code, PT.Name as [ProductType], PK.Oid as [ProductKindOid], PK.Name as [ProductKind], U.Code as [Unit], SUM(S.cQuantity) as Quantity from Store S inner join Warehouse W on W.Oid = S.Warehouse inner join Product P on P.Oid = S.Product inner join Unit U on U.Oid = S.cUnit inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where S.GCRecord is null and W.Code = @warehouseCode group by W.Code, PT.Name, PK.Oid, PK.Name, U.Code order by PT.Name, PK.Name", new SqlParameter("@warehouseCode", warehouseCode)).Tables[0].AsEnumerable().Select(t => new StoreTable
            {
                Code = t.Field<string>("Code"),
                ProductType = t.Field<string>("ProductType"),
                ProductKindOid = t.Field<Guid>("ProductKindOid").ToString(),
                ProductKind = t.Field<string>("ProductKind"),
                Unit = t.Field<string>("Unit"),
                Quantity = t.Field<decimal>("Quantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetStoreReportByKind(string tokenKey, string warehouseCode, string productKind)
        {
            List<StoreTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select W.Code, P.Name as [ProductName], U.Code as [Unit], SUM(S.cQuantity) as [Quantity] from Store S inner join Warehouse W on W.Oid = S.Warehouse inner join Product P on P.Oid = S.Product inner join Unit U on U.Oid = S.cUnit where S.GCRecord is null and W.Code = @warehouseCode and P.ProductKind = @productKind group by W.Code, P.Name, U.Code order by P.Name", new SqlParameter("@warehouseCode", warehouseCode), new SqlParameter("@productKind", Guid.Parse(productKind))).Tables[0].AsEnumerable().Select(t => new StoreTable
            {
                ProductName = t.Field<string>("ProductName"),
                Unit = t.Field<string>("Unit"),
                Quantity = t.Field<decimal>("Quantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWaitingDemandList(string tokenKey)
        {
            List<DemandTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select D.Oid, T.DemandNumber, D.LineNumber, P.Name as [Product], U.Code as [Unit], D.Quantity, (case when D.[Priority] = 0 then 'Düşük' when D.[Priority] = 1 then 'Normal' else 'Yüksek' end) as [Priority], E.NameSurname as [CreatedBy] from DemandDetail D inner join Demand T on T.Oid = D.Demand inner join Product P on P.Oid = D.Product inner join Unit U on U.Oid = D.Unit left outer join Employee E on E.Oid = T.CreatedBy where D.GCRecord is null and D.DemandStatus = 1 order by [Priority] desc").Tables[0].AsEnumerable().Select(t => new DemandTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                DemandNumber = t.Field<int>("DemandNumber"),
                LineNumber = t.Field<int>("LineNumber"),
                Product = t.Field<string>("Product"),
                Unit = t.Field<string>("Unit"),
                Quantity = t.Field<decimal>("Quantity"),
                Priority = t.Field<string>("Priority"),
                CreatedBy = t.Field<string>("CreatedBy")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string ConfirmDemand(string tokenKey, string oid)
        {
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(SqlProvider.ConnectionString, CommandType.Text, @"update DemandDetail set DemandStatus = 2 where Oid = @oid", new SqlParameter("@oid", Guid.Parse(oid)));
            }
            return "islem tamamlandi.";
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string DenyDemand(string tokenKey, string oid)
        {
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(SqlProvider.ConnectionString, CommandType.Text, @"update DemandDetail set DemandStatus = 9 where Oid = @oid", new SqlParameter("@oid", Guid.Parse(oid)));
            }
            return "islem tamamlandi.";
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string ConfirmAllDemand(string tokenKey)
        {
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(SqlProvider.ConnectionString, CommandType.Text, @"update DemandDetail set DemandStatus = 2 where DemandStatus = 1");
            }
            return "islem tamamlandi.";
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string DenyAllDemand(string tokenKey)
        {
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(SqlProvider.ConnectionString, CommandType.Text, @"update DemandDetail set DemandStatus = 9 where DemandStatus = 1");
            }
            return "islem tamamlandi.";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetContactAnalysisList(string tokenKey)
        {
            List<ContactAnalysisReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select C.Oid, C.Name, (select isnull(SUM(D1.Total + D1.Tax), 0) from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 900 and D1.Currency = (select Oid from Currency where GCRecord is null and Code = 'TL') and O1.Contact = C.Oid) as [TLTotal], (select isnull(SUM(D1.CurrencyTotal + D1.CurrencyTax), 0) from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 900 and D1.Currency = (select Oid from Currency where GCRecord is null and Code = 'USD') and O1.Contact = C.Oid) as [USDTotal], (select isnull(SUM(D1.CurrencyTotal + D1.CurrencyTax), 0) from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 900 and D1.Currency = (select Oid from Currency where GCRecord is null and Code = 'EUR') and O1.Contact = C.Oid) as [EURTotal] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where D.GCRecord is null and D.SalesOrderStatus < 900 and C.Code is not null and C.Name not like 'ECO%' group by C.Oid, C.Name order by C.Name").Tables[0].AsEnumerable().Select(t => new ContactAnalysisReportTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                Name = t.Field<string>("Name"),
                TLTotal = t.Field<decimal>("TLTotal"),
                USDTotal = t.Field<decimal>("USDTotal"),
                EURTotal = t.Field<decimal>("EURTotal")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetContactAnalysisDetailList(string tokenKey, string contactOid)
        {
            List<ContactAnalysisReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select PT.Name as [GroupName], PK.Name, (select isnull(SUM(D1.Total + D1.Tax), 0) from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 900 and D1.Currency = (select Oid from Currency where GCRecord is null and Code = 'TL') and O1.Contact = O.Contact and P1.ProductKind = PK.Oid) as [TLTotal], (select isnull(SUM(D1.CurrencyTotal + D1.CurrencyTax), 0) from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 900 and D1.Currency = (select Oid from Currency where GCRecord is null and Code = 'USD') and O1.Contact = O.Contact and P1.ProductKind = PK.Oid) as [USDTotal], (select isnull(SUM(D1.CurrencyTotal + D1.CurrencyTax), 0) from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 900 and D1.Currency = (select Oid from Currency where GCRecord is null and Code = 'EUR') and O1.Contact = O.Contact and P1.ProductKind = PK.Oid) as [EURTotal] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and C.Oid = @contactOid group by O.Contact, PT.Name, PK.Oid, PK.Name order by PT.Name, PK.Name", new SqlParameter("@contactOid", Guid.Parse(contactOid))).Tables[0].AsEnumerable().Select(t => new ContactAnalysisReportTable
            {
                GroupName = t.Field<string>("GroupName"),
                Name = t.Field<string>("Name"),
                TLTotal = t.Field<decimal>("TLTotal"),
                USDTotal = t.Field<decimal>("USDTotal"),
                EURTotal = t.Field<decimal>("EURTotal")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProductGroupAnalysisList(string tokenKey)
        {
            List<ProductGroupReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select Oid, Code, Name from ProductGroup where GCRecord is null and Code = 'SM' or Code = 'OM'").Tables[0].AsEnumerable().Select(t => new ProductGroupReportTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                Name = t.Field<string>("Name")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProductTypeAnalysisList(string tokenKey, string productGroup)
        {
            List<ProductGroupReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select PT.Oid, PT.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductGroup = @productGroup group by PT.Oid, PT.Name order by cQuantity desc", new SqlParameter("@productGroup", Guid.Parse(productGroup))).Tables[0].AsEnumerable().Select(t => new ProductGroupReportTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                Name = t.Field<string>("Name"),
                cQuantity = t.Field<decimal>("cQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProductTypeAnalysisDetailList(string tokenKey, string productType)
        {
            List<ProductGroupReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select top 10 C.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductType = @productType and C.Code is not null and C.Name not like 'ECO%' group by C.Name order by cQuantity desc", new SqlParameter("@productType", Guid.Parse(productType))).Tables[0].AsEnumerable().Select(t => new ProductGroupReportTable
            {
                Name = t.Field<string>("Name"),
                cQuantity = t.Field<decimal>("cQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProductKindAnalysisList(string tokenKey, string productType)
        {
            List<ProductGroupReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select PK.Oid, PK.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductType = @productType group by PK.Oid, PK.Name order by cQuantity desc", new SqlParameter("@productType", Guid.Parse(productType))).Tables[0].AsEnumerable().Select(t => new ProductGroupReportTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                Name = t.Field<string>("Name"),
                cQuantity = t.Field<decimal>("cQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProductKindAnalysisDetailList(string tokenKey, string productKind)
        {
            List<ProductGroupReportTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select top 10 C.Name, SUM(D.cQuantity) as [cQuantity] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and D.SalesOrderStatus < 900 and P.ProductKind = @productKind and C.Code is not null and C.Name not like 'ECO%' group by C.Name order by cQuantity desc", new SqlParameter("@productKind", Guid.Parse(productKind))).Tables[0].AsEnumerable().Select(t => new ProductGroupReportTable
            {
                Name = t.Field<string>("Name"),
                cQuantity = t.Field<decimal>("cQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWaitingOrderContactList(string tokenKey)
        {
            List<ContactTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select C.Oid, C.Code, C.Name from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact where D.GCRecord is null and D.SalesOrderStatus < 200 group by C.Oid, C.Code, C.Name order by C.Name").Tables[0].AsEnumerable().Select(t => new ContactTable
            {
                Oid = t.Field<Guid>("Oid").ToString(),
                Code = t.Field<string>("Code"),
                Name = t.Field<string>("Name")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWaitingOrderList(string tokenKey, string contactOid)
        {
            List<WaitingSalesOrderTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select O.ContactOrderNumber, O.ContactOrderDate, (case when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Eco1 Bekliyor' when D.SalesOrderStatus = 102 then 'Eco1 Laminasyon Bekliyor' when D.SalesOrderStatus = 103 then 'Eco2 Bekliyor' when D.SalesOrderStatus = 104 then 'Eco3 Bekliyor' when D.SalesOrderStatus = 105 then 'Eco4 Bekliyor' when D.SalesOrderStatus = 106 then 'Eco4 Dilme Bekliyor' when D.SalesOrderStatus = 107 then 'Eco5 Cpp Bekliyor' when D.SalesOrderStatus = 108 then 'Eco5 Stretch Bekliyor' when D.SalesOrderStatus = 109 then 'Eco5 Aktarma Bekliyor' when D.SalesOrderStatus = 110 then 'Eco5 Dilme Bekliyor' when D.SalesOrderStatus = 111 then 'Eco5 Rejenere Bekliyor' when D.SalesOrderStatus = 112 then 'Eco6 Bekliyor' when D.SalesOrderStatus = 113 then 'Eco6 Kesim Bekliyor' when D.SalesOrderStatus = 114 then 'Eco6 Laminasyon Bekliyor' when D.SalesOrderStatus = 120 then 'Üretim Bekliyor' when D.SalesOrderStatus = 130 then 'Sevkiyat Bekliyor' when D.SalesOrderStatus = 131 then 'Yükleme Bekliyor' when D.SalesOrderStatus = 200 then 'Sevk Edildi' when D.SalesOrderStatus = 900 then 'İptal Edildi' else '' end) as [SalesOrderStatus], C.Name as [Contact], SC.Name as [ShippingContact], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [OrderNumber], P.Name as [WorkName], O.OrderDate, D.LineDeliveryDate, D.Quantity as [OrderedQuantity], D.ShippedQuantity as [ShippedQuantity], (select isnull(SUM(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null) and SalesOrderDetail = D.Oid) as [StoreQuantity], U.Code as [Unit], D.cQuantity as [OrderedcQuantity], D.ShippedcQuantity as [ShippedcQuantity], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) as [StorecQuantity], (case when D.SalesOrderWorkStatus = 0 then 'Yeni' when D.SalesOrderWorkStatus = 1 then 'Tekrar' else 'Revizyon' end) as [WorkStatus], CT.Name as [City], D.PetkimPrice, D.GeneralCost, D.PetkimUnitPrice, PZ.Name as [PetkimExtra], (case when O.TransportType = 0 then 'Nakliye Bize' else 'Nakliye Karşıya' end) as [TransportType], PM.Name as [PaymentMethod], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı')) as [PrintingStore], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim')) as [CuttingStore], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme')) as [SlicingStore], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon')) as [LaminationStore] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join City CT on CT.Oid = C.City inner join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = D.Product inner join Unit U on U.Oid = D.Unit inner join Unit CU on CU.Oid = D.cUnit inner join ContactGroup2 PZ on PZ.Oid = O.ContactGroup2 inner join PaymentMethod PM on PM.Oid = O.PaymentMethod where D.GCRecord is null and D.SalesOrderStatus < 200 and C.Oid = @contactOid", new SqlParameter("@contactOid", Guid.Parse(contactOid))).Tables[0].AsEnumerable().Select(t => new WaitingSalesOrderTable
            {
                SalesOrderStatus = t.Field<string>("SalesOrderStatus"),
                Contact = t.Field<string>("Contact"),
                ShippingContact = t.Field<string>("ShippingContact"),
                ContactOrderNumber = t.Field<string>("ContactOrderNumber"),
                ContactOrderDate = t.Field<DateTime>("ContactOrderDate"),
                OrderNumber = t.Field<string>("OrderNumber"),
                WorkName = t.Field<string>("WorkName"),
                OrderDate = t.Field<DateTime>("OrderDate"),
                LineDeliveryDate = t.Field<DateTime>("LineDeliveryDate"),
                OrderedQuantity = t.Field<decimal>("OrderedQuantity"),
                ShippedQuantity = t.Field<decimal>("ShippedQuantity"),
                StoreQuantity = t.Field<decimal>("StoreQuantity"),
                Unit = t.Field<string>("Unit"),
                OrderedcQuantity = t.Field<decimal>("OrderedcQuantity"),
                ShippedcQuantity = t.Field<decimal>("ShippedcQuantity"),
                StorecQuantity = t.Field<decimal>("StorecQuantity"),
                WorkStatus = t.Field<string>("WorkStatus"),
                City = t.Field<string>("City"),
                PetkimPrice = t.Field<decimal>("PetkimPrice"),
                GeneralCost = t.Field<decimal>("GeneralCost"),
                PetkimUnitPrice = t.Field<decimal>("PetkimUnitPrice"),
                PetkimExtra = t.Field<string>("PetkimExtra"),
                TransportType = t.Field<string>("TransportType"),
                PaymentMethod = t.Field<string>("PaymentMethod"),
                PrintingStore = t.Field<decimal>("PrintingStore"),
                CuttingStore = t.Field<decimal>("CuttingStore"),
                SlicingStore = t.Field<decimal>("SlicingStore"),
                LaminationStore = t.Field<decimal>("LaminationStore")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetStationProductionList(string tokenKey, string beginDate, string endDate)
        {
            DateTime begindate = new DateTime(Convert.ToDateTime(beginDate).Year, Convert.ToDateTime(beginDate).Month, Convert.ToDateTime(beginDate).Day, 8, 15, 0);
            DateTime enddate = new DateTime(Convert.ToDateTime(endDate).Year, Convert.ToDateTime(endDate).Month, Convert.ToDateTime(endDate).Day, 8, 15, 0);
            List<ProductionTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select S.Code as [StationCode], isnull(sum(P.GrossQuantity), 0) as [ProductionQuantity], (select isnull(SUM(W.GrossQuantity), 0) from Wastage W inner join Machine M1 on M1.Oid = W.Machine inner join Station S1 on S1.Oid = M1.Station where W.GCRecord is null and S1.Code = S.Code and WastageDate between @beginDate and @endDate) as [WastageQuantity] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and P.ProductionDate between @beginDate and @endDate group by S.Code order by S.Code", new SqlParameter("@beginDate", begindate), new SqlParameter("@endDate", enddate)).Tables[0].AsEnumerable().Select(t => new ProductionTable
            {
                StationCode = t.Field<string>("StationCode"),
                ProductionQuantity = t.Field<decimal>("ProductionQuantity"),
                WastageQuantity = t.Field<decimal>("WastageQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetStationMachineList(string tokenKey, string beginDate, string endDate, string station)
        {
            DateTime begindate = new DateTime(Convert.ToDateTime(beginDate).Year, Convert.ToDateTime(beginDate).Month, Convert.ToDateTime(beginDate).Day, 8, 15, 0);
            DateTime enddate = new DateTime(Convert.ToDateTime(endDate).Year, Convert.ToDateTime(endDate).Month, Convert.ToDateTime(endDate).Day, 8, 15, 0);
            List<ProductionTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select M.Code as [MachineCode], sum(P.GrossQuantity) as [ProductionQuantity], (select isnull(SUM(W.GrossQuantity), 0) from Wastage W inner join Machine M1 on M1.Oid = W.Machine where W.GCRecord is null and M1.Code = M.Code and WastageDate between @beginDate and @endDate) as [WastageQuantity] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and S.Code = @station and P.ProductionDate between @beginDate and @endDate group by M.Code order by M.Code", new SqlParameter("@beginDate", begindate), new SqlParameter("@endDate", enddate), new SqlParameter("@station", station)).Tables[0].AsEnumerable().Select(t => new ProductionTable
            {
                MachineCode = t.Field<string>("MachineCode"),
                ProductionQuantity = t.Field<decimal>("ProductionQuantity"),
                WastageQuantity = t.Field<decimal>("WastageQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetStationMachineDetailList(string tokenKey, string beginDate, string endDate, string machine)
        {
            DateTime begindate = new DateTime(Convert.ToDateTime(beginDate).Year, Convert.ToDateTime(beginDate).Month, Convert.ToDateTime(beginDate).Day, 8, 15, 0);
            DateTime enddate = new DateTime(Convert.ToDateTime(endDate).Year, Convert.ToDateTime(endDate).Month, Convert.ToDateTime(endDate).Day, 8, 15, 0);
            List<ProductionTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select WS.Name as [Shift], E.NameSurname as [Operator], sum(P.GrossQuantity) as [ProductionQuantity], (select isnull(SUM(W.GrossQuantity), 0) from Wastage W inner join Machine M1 on M1.Oid = W.Machine inner join ShiftStart SS1 on SS1.Oid = W.[Shift] left outer join Employee E1 on E1.Oid = W.Employee inner join WorkShift WS1 on WS1.Oid = SS1.WorkShift where W.GCRecord is null and M1.Code = M.Code and WS1.Oid = WS.Oid and E1.Oid = E.Oid and WastageDate between @beginDate and @endDate) as [WastageQuantity] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station inner join ShiftStart SS on SS.Oid = P.[Shift] left outer join Employee E on E.Oid = P.Employee inner join WorkShift WS on WS.Oid = SS.WorkShift where P.GCRecord is null and M.Code = @machine and P.ProductionDate between @beginDate and @endDate group by M.Code, WS.Oid, WS.Name, E.Oid, E.NameSurname order by WS.Name", new SqlParameter("@beginDate", begindate), new SqlParameter("@endDate", enddate), new SqlParameter("@machine", machine)).Tables[0].AsEnumerable().Select(t => new ProductionTable
            {
                Shift = t.Field<string>("Shift"),
                Operator = t.Field<string>("Operator"),
                ProductionQuantity = t.Field<decimal>("ProductionQuantity"),
                WastageQuantity = t.Field<decimal>("WastageQuantity")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetMachineLoadMainList(string tokenKey)
        {
            List<MachineLoadMainTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select S.Code as [StationCode], S.TableName, M.Code as [MachineCode] from Machine M inner join Station S on S.Oid = M.Station where M.GCRecord is null order by S.Code, M.Code").Tables[0].AsEnumerable().Select(t => new MachineLoadMainTable
            {
                StationCode = t.Field<string>("StationCode"),
                TableName = t.Field<string>("TableName"),
                MachineCode = t.Field<string>("MachineCode")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetMachineLoadList(string tokenKey, string machine)
        {
            string tableName = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(SqlProvider.ConnectionString, CommandType.Text, @"select TableName from Station where Oid = (select Station from Machine where GCRecord is null and Code = @machine)", new SqlParameter("@machine", machine)).ToString();
            List<MachineLoadTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, string.Format(@"select M.Code as [MachineCode], W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, P.Name as [WorkName], C.Name as [ContactName], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [OrderNumber], O.OrderDate as [OrderDate], D.LineDeliveryDate, P.Width, P.Height, P.Thickness, P.Lenght, D.cQuantity as [OrderedQuantity], W.Quantity as [WorkOrderQuantity], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = S.SourceWarehouse and Product = P.Oid) as [StoreQuantity], (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [ProductionQuantity], (case when W.Quantity - (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) > 0 then W.Quantity - (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) else 0 end) as [RemainingProductionQuantity], (select isnull(SUM(GrossQuantity), 0) from Wastage where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) as [WastageQuantity], (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) as [SalesOrderQuantity], ((case when W.Quantity - (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) > 0 then W.Quantity - (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) else 0 end) / (M.Capacity / 24)) as [RemainingTime], (case when (((select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) * 100) / W.Quantity) <= 100 then (((select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and WorkOrderNumber = W.WorkOrderNumber) * 100) / W.Quantity) else 100 end) as [US], (case when (((select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) * 100) / D.cQuantity) <= 100 then (((select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) * 100) / D.cQuantity) else 0 end) as [SS] from {0} W inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product inner join Station S on S.Oid = W.Station inner join Machine M on M.Oid = W.Machine where W.GCRecord is null and M.Code = @machine and W.SequenceNumber between 1 and 100 order by W.SequenceNumber", tableName), new SqlParameter("@machine", machine)).Tables[0].AsEnumerable().Select(t => new MachineLoadTable
            {
                MachineCode = t.Field<string>("MachineCode"),
                SequenceNumber = t.Field<Int16>("SequenceNumber"),
                WorkOrderNumber = t.Field<string>("WorkOrderNumber"),
                WorkOrderDate = t.Field<DateTime>("WorkOrderDate"),
                WorkName = t.Field<string>("WorkName"),
                ContactName = t.Field<string>("ContactName"),
                OrderNumber = t.Field<string>("OrderNumber"),
                OrderDate = t.Field<DateTime>("OrderDate"),
                LineDeliveryDate = t.Field<DateTime>("LineDeliveryDate"),
                Width = t.Field<int>("Width"),
                Height = t.Field<int>("Height"),
                Thickness = t.Field<decimal>("Thickness"),
                Lenght = t.Field<int>("Lenght"),
                OrderedQuantity = t.Field<decimal>("OrderedQuantity"),
                WorkOrderQuantity = t.Field<decimal>("WorkOrderQuantity"),
                StoreQuantity = t.Field<decimal>("StoreQuantity"),
                ProductionQuantity = t.Field<decimal>("ProductionQuantity"),
                RemainingProductionQuantity = t.Field<decimal>("RemainingProductionQuantity"),
                WastageQuantity = t.Field<decimal>("WastageQuantity"),
                SalesOrderQuantity = t.Field<decimal>("SalesOrderQuantity"),
                RemainingTime = t.Field<decimal>("RemainingTime"),
                US = t.Field<decimal>("US"),
                SS = t.Field<decimal>("SS")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetActiveMachineStopList(string tokenKey)
        {
            List<MachineStopTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select G.Name as [GroupName], C.Name as [StopName], M.Code as [MachineCode], S.BeginDate, S.Note from MachineStop S inner join Machine M on M.Oid = S.Machine inner join StopCode C on C.Oid = S.StopCode inner join StopGroupCode G on G.Oid = C.StopGroupCode where S.GCRecord is null and S.Active = 1 order by G.Name, C.Name, M.Code").Tables[0].AsEnumerable().Select(t => new MachineStopTable
            {
                GroupName = t.Field<string>("GroupName"),
                StopName = t.Field<string>("StopName"),
                MachineCode = t.Field<string>("MachineCode"),
                BeginDate = t.Field<DateTime>("BeginDate"),
                Note = t.Field<string>("Note")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetEndedMachineStopList(string tokenKey, string beginDate, string endDate)
        {
            DateTime begindate = new DateTime(Convert.ToDateTime(beginDate).Year, Convert.ToDateTime(beginDate).Month, Convert.ToDateTime(beginDate).Day, 8, 15, 0);
            DateTime enddate = new DateTime(Convert.ToDateTime(endDate).Year, Convert.ToDateTime(endDate).Month, Convert.ToDateTime(endDate).Day, 8, 15, 0);
            List<MachineStopTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select G.Name as [GroupName], C.Name as [StopName], M.Code as [MachineCode], S.BeginDate, S.EndDate, datediff(minute, S.BeginDate, S.EndDate) as [StopTime], S.Note from MachineStop S inner join Machine M on M.Oid = S.Machine inner join StopCode C on C.Oid = S.StopCode inner join StopGroupCode G on G.Oid = C.StopGroupCode where S.GCRecord is null and S.Active = 0 and S.BeginDate between @beginDate and @endDate order by G.Name, C.Name, M.Code", new SqlParameter("@beginDate", begindate), new SqlParameter("@endDate", enddate)).Tables[0].AsEnumerable().Select(t => new MachineStopTable
            {
                GroupName = t.Field<string>("GroupName"),
                StopName = t.Field<string>("StopName"),
                MachineCode = t.Field<string>("MachineCode"),
                BeginDate = t.Field<DateTime>("BeginDate"),
                EndDate = t.Field<DateTime>("EndDate"),
                StopTime = t.Field<int>("StopTime"),
                Note = t.Field<string>("Note")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetDepartmentList(string tokenKey)
        {
            List<DepartmentTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select D.Name, (select COUNT(Oid) from Employee where GCRecord is null and LeaveDate is null and Department = D.Oid) as [EmployeeCount] from Department D where D.GCRecord is null order by D.Name").Tables[0].AsEnumerable().Select(t => new DepartmentTable
            {
                Name = t.Field<string>("Name"),
                EmployeeCount = t.Field<int>("EmployeeCount")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetEmployeeList(string tokenKey, string department)
        {
            List<EmployeeTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select D.Name as [DepartmentName], E.NameSurname, P.Name as [DepartmentPartName], T.Name as [TaskName], WP.Name as [WorkPlaceName] from Employee E inner join Department D on D.Oid = E.Department inner join DepartmentPart P on P.Oid = E.DepartmentPart inner join EmployeeTask T on T.Oid = E.EmployeeTask inner join WorkPlace WP on WP.Oid = E.WorkPlace where E.GCRecord is null and E.LeaveDate is null and D.Name = @department order by D.Name, E.NameSurname", new SqlParameter("@department", department)).Tables[0].AsEnumerable().Select(t => new EmployeeTable
            {
                DepartmentName = t.Field<string>("DepartmentName"),
                NameSurname = t.Field<string>("NameSurname"),
                DepartmentPartName = t.Field<string>("DepartmentPartName"),
                TaskName = t.Field<string>("TaskName"),
                WorkPlaceName = t.Field<string>("WorkPlaceName")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetShippingByProductKind(string tokenKey, string begindate, string enddate)
        {

            List<ShippingTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select ProductKind, sum(Quantity) as Quantity, Unit, sum(Total) as Total from (select PK.Name as ProductKind, sum(D.cQuantity) as Quantity, U.Code as Unit, sum(D.cQuantity) * (case when SD.CurrencyPrice > 0 then (SD.CurrencyPrice / (case when SD.ExchangeRate > 0 then SD.ExchangeRate else 1 end)) else SD.Price end) as Total from SalesWaybillDetail D inner join SalesWaybill W on W.Oid = D.SalesWaybill inner join SalesOrderDetail SD on SD.Oid = D.SalesOrderDetail inner join Unit U on U.Oid = D.cUnit inner join Product P on P.Oid = SD.Product inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and cast(W.WaybillDate as date) between @begindate and @enddate group by PK.Name, U.Code, SD.CurrencyPrice, SD.Price, SD.ExchangeRate) T group by ProductKind, Unit order by Total desc", new SqlParameter("@begindate", Convert.ToDateTime(begindate)), new SqlParameter("@enddate", Convert.ToDateTime(enddate))).Tables[0].AsEnumerable().Select(t => new ShippingTable
            {
                ProductKind = t.Field<string>("ProductKind"),
                Quantity = t.Field<decimal>("Quantity"),
                Unit = t.Field<string>("Unit"),
                Total = t.Field<decimal>("Total")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetShippingByContact(string tokenKey, string begindate, string enddate)
        {
            List<ShippingTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select Contact, sum(Quantity) as Quantity, Unit, sum(Total) as Total from (select C.Name as Contact, sum(D.cQuantity) as Quantity, U.Code as Unit, sum(D.cQuantity) * (case when SD.CurrencyPrice > 0 then (SD.CurrencyPrice / (case when SD.ExchangeRate > 0 then SD.ExchangeRate else 1 end)) else SD.Price end) as Total from SalesWaybillDetail D inner join SalesWaybill W on W.Oid = D.SalesWaybill inner join SalesOrderDetail SD on SD.Oid = D.SalesOrderDetail inner join Unit U on U.Oid = D.cUnit inner join SalesOrder O on O.Oid = SD.SalesOrder inner join Contact C on C.Oid = O.Contact where D.GCRecord is null and cast(W.WaybillDate as date) between @begindate and @enddate group by C.Name, U.Code, SD.CurrencyPrice, SD.Price, SD.ExchangeRate) T group by Contact, Unit order by Total desc", new SqlParameter("@begindate", Convert.ToDateTime(begindate)), new SqlParameter("@enddate", Convert.ToDateTime(enddate))).Tables[0].AsEnumerable().Select(t => new ShippingTable
            {
                Contact = t.Field<string>("Contact"),
                Quantity = t.Field<decimal>("Quantity"),
                Unit = t.Field<string>("Unit"),
                Total = t.Field<decimal>("Total")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetShippingByCity(string tokenKey, string begindate, string enddate)
        {
            List<ShippingTable> list = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionString, CommandType.Text, @"select City, sum(Quantity) as Quantity, Unit, sum(Total) as Total from (select CT.Name as City, sum(D.cQuantity) as Quantity, U.Code as Unit, sum(D.cQuantity) * (case when SD.CurrencyPrice > 0 then (SD.CurrencyPrice / (case when SD.ExchangeRate > 0 then SD.ExchangeRate else 1 end)) else SD.Price end) as Total from SalesWaybillDetail D inner join SalesWaybill W on W.Oid = D.SalesWaybill inner join SalesOrderDetail SD on SD.Oid = D.SalesOrderDetail inner join Unit U on U.Oid = D.cUnit inner join SalesOrder O on O.Oid = SD.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join City CT on CT.Oid = C.City where D.GCRecord is null and cast(W.WaybillDate as date) between @begindate and @enddate group by CT.Name, U.Code, SD.CurrencyPrice, SD.Price, SD.ExchangeRate) T group by City, Unit order by Total desc", new SqlParameter("@begindate", Convert.ToDateTime(begindate)), new SqlParameter("@enddate", Convert.ToDateTime(enddate))).Tables[0].AsEnumerable().Select(t => new ShippingTable
            {
                City = t.Field<string>("City"),
                Quantity = t.Field<decimal>("Quantity"),
                Unit = t.Field<string>("Unit"),
                Total = t.Field<decimal>("Total")
            }).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            if (tokenKey == HttpContext.Current.Request.Cookies["tokenKey"].Value.Replace("%22", ""))
            {
                string data = js.Serialize(list);
                Context.Response.Write(data);
            }
            else
            {
                Context.Response.Write(string.Empty);
            }
            Context.Response.End();
        }
    }
}
