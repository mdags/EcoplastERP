using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcoplastERP.PTC
{
    public partial class shippingstorereport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
            if(Request.QueryString["rt"] != null)
            {
                if (Request.QueryString["rt"].ToString() == "exp")
                {
                    rptRecords.DataSourceID = "SqlDataSource1";
                    rptRecords.DataBind();
                }
                else if (Request.QueryString["rt"].ToString() == "del")
                {
                    rptRecords.DataSourceID = "SqlDataSource2";
                    rptRecords.DataBind();
                }
                else if (Request.QueryString["rt"].ToString() == "ord")
                {
                    rptRecords.DataSourceID = "SqlDataSource3";
                    rptRecords.DataBind();
                }
            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = Request.QueryString["dn"] != null ? Request.QueryString["dn"].ToString() : string.Empty;
        }

        protected void btnDetail_Click(object sender, EventArgs e)
        {
            Response.Redirect("shippingstorereportdetail.aspx?rt=" + Request.QueryString["rt"].ToString() + "&dn=" + Request.QueryString["dn"].ToString() + "&order=" + (sender as Button).CommandArgument.ToString());
        }
    }
}