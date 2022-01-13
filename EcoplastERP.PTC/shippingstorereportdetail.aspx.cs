using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingstorereportdetail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
            if (!IsPostBack)
            {
                rptBarcodeReport.Visible = false;
                rptPaletteReport.Visible = true;
                string orderNumber = Request.QueryString["order"].Substring(0, Request.QueryString["order"].IndexOf("/"));
                string lineNumber = Request.QueryString["order"].Substring(Request.QueryString["order"].IndexOf("/") + 1, (Request.QueryString["order"].Length - (Request.QueryString["order"].IndexOf("/") + 1)));
                Session session = modal.XpoHelper.GetNewSession();
                SalesOrderDetail salesOrderDetail = session.FindObject<SalesOrderDetail>(CriteriaOperator.Parse("SalesOrder.OrderNumber = ? and LineNumber = ?", orderNumber, lineNumber));
                lblTotalQuantity.Text = string.Format("{0:n2}", Convert.ToDecimal(session.Evaluate(typeof(Store), CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("SalesOrderDetail = ?", salesOrderDetail.Oid))));
            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            string orderNumber = Request.QueryString["order"].Substring(0, Request.QueryString["order"].IndexOf("/"));
            string lineNumber = Request.QueryString["order"].Substring(Request.QueryString["order"].IndexOf("/") + 1, (Request.QueryString["order"].Length - (Request.QueryString["order"].IndexOf("/") + 1)));
            Session session = modal.XpoHelper.GetNewSession();
            SalesOrderDetail salesOrderDetail = session.FindObject<SalesOrderDetail>(CriteriaOperator.Parse("SalesOrder.OrderNumber = ? and LineNumber = ?", orderNumber, lineNumber));
            e.Command.Parameters["@salesOrderDetail"].Value = salesOrderDetail != null ? salesOrderDetail.Oid.ToString() : Guid.NewGuid().ToString();
        }

        protected void rblReportType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReportType2.SelectedValue == "palette")
            {
                rptBarcodeReport.Visible = false;
                rptPaletteReport.Visible = true;
                rptPaletteReport.DataSourceID = "SqlDataSource1";
                rptPaletteReport.DataBind();
            }
            else
            {
                rptBarcodeReport.Visible = true;
                rptPaletteReport.Visible = false;
                rptBarcodeReport.DataSourceID = "SqlDataSource2";
                rptBarcodeReport.DataBind();
            }
        }
    }
}