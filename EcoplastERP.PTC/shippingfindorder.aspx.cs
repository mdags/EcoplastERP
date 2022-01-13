using System;
using System.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingfindorder : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }
        [WebMethod]
        public static List<OrderInfoTable> GetOrderInfo(string ordernumber, string linenumber)
        {
            List<OrderInfoTable> list = new List<PTC.OrderInfoTable>();
            Session session = modal.XpoHelper.GetNewSession();
            var salesOrderDetail = session.FindObject<SalesOrderDetail>(CriteriaOperator.Parse("SalesOrder.OrderNumber = ? and LineNumber = ?", ordernumber, linenumber));
            if (salesOrderDetail != null)
            {
                OrderInfoTable table = new OrderInfoTable();
                table.ContactName = salesOrderDetail.SalesOrder.Contact.Name;
                table.ProductCode = salesOrderDetail.Product.Code;
                table.ProductName = salesOrderDetail.Product.Name;
                table.FilmingProduction = Convert.ToDecimal(session.Evaluate<Production>(CriteriaOperator.Parse("SUM(GrossQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and FilmingWorkOrder != null", salesOrderDetail)));
                table.PrintingProduction = Convert.ToDecimal(session.Evaluate<Production>(CriteriaOperator.Parse("SUM(GrossQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and PrintingWorkOrder != null", salesOrderDetail)));
                table.CuttingProduction = Convert.ToDecimal(session.Evaluate<Production>(CriteriaOperator.Parse("SUM(GrossQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and CuttingWorkOrder != null", salesOrderDetail)));
                table.ShippingWarehouseQuantity = Convert.ToDecimal(session.Evaluate<Store>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.ShippingWarehouse = true", salesOrderDetail)));
                table.LoadedQuantity = Convert.ToDecimal(session.Evaluate<DeliveryDetailLoading>(CriteriaOperator.Parse("SUM(Quantity)"), CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ?", salesOrderDetail)));
                list.Add(table);
            }
            return list;
        }
    }
}