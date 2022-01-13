using System;
using System.Web.UI;

namespace EcoplastERP.PTC
{
    public partial class shippinghome : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }
    }
}