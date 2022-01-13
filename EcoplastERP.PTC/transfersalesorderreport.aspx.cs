using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcoplastERP.PTC
{
    public partial class transfersalesorderreport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["userOid"] == null)
            //{
            //    Response.Redirect("transferlogin.aspx");
            //}
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@salesOrderNumber"].Value = txtSalesOrderNumber.Text;
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            rptRecords.DataBind();
        }
    }
}