using System;
using System.Web.UI;

namespace EcoplastERP.PTC
{
    public partial class shippingstorereporthome : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (txtDocumentNumber.Text.StartsWith("E"))
            {
                Response.Redirect("shippingstorereport.aspx?rt=exp&dn=" + txtDocumentNumber.Text);
            }
            else if (txtDocumentNumber.Text.StartsWith("T"))
            {
                Response.Redirect("shippingstorereport.aspx?rt=del&dn=" + txtDocumentNumber.Text);
            }
            else if (txtDocumentNumber.Text.StartsWith("S"))
            {
                Response.Redirect("shippingstorereport.aspx?rt=ord&dn=" + txtDocumentNumber.Text);
            }
        }
    }
}