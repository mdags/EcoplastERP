using System;
using System.Web.UI;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingfindbarcode : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
            if (!IsPostBack)
            {
                txtBarcode.Focus();
            }
        }

        protected void SqlDataSource1_Selecting(object sender, System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@barcode"].Value = string.IsNullOrEmpty(txtBarcode.Text) ? "x" : txtBarcode.Text;
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Production production = session.FindObject<Production>(CriteriaOperator.Parse("Barcode = ?", txtBarcode.Text));
            if (production != null)
            {
                lblPaletteNumber.InnerHtml = production.ProductionPalette != null ? production.ProductionPalette.PaletteNumber : string.Empty;
                lblOrderNumber.InnerHtml = production.SalesOrderDetail.SalesOrder.OrderNumber;
                lblContactName.InnerHtml = production.SalesOrderDetail.SalesOrder.Contact.Name;
                lblProductName.InnerHtml = production.SalesOrderDetail.Product.Name;
                lblGrossQuantity.InnerHtml = string.Format("{0:n2}", production.GrossQuantity);
                lblNetQuantity.InnerHtml = string.Format("{0:n2}", production.NetQuantity);
                lblOperatorName.InnerHtml = production.Employee.NameSurname;
                lblProductionDate.InnerHtml = production.ProductionDate.ToShortDateString();
            }
            rptRecords.DataBind();
        }
    }
}