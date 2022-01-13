using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingfindpalette : Page
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

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = string.IsNullOrEmpty(txtBarcode.Text) ? "x" : txtBarcode.Text;
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                Session session = modal.XpoHelper.GetNewSession();
                lblTotalGross.Text = string.Format("{0:n2}", Convert.ToDecimal(session.ExecuteScalar(@"select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber)", new string[] { "@paletteNumber" }, new object[] { txtBarcode.Text })));

                ProductionPalette palette = session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", txtBarcode.Text));
                if (palette != null)
                {
                    lblTare.Text = string.Format("{0:n2}", palette.Tare);
                }

                lblTotalNet.Text = string.Format("{0:n2}", Convert.ToDecimal(session.ExecuteScalar(@"select isnull(sum(NetQuantity), 0) from Production where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber)", new string[] { "@paletteNumber" }, new object[] { txtBarcode.Text })));

                rptRecords.DataBind();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Palet No boş olamaz.');", true);
            }
        }
    }
}