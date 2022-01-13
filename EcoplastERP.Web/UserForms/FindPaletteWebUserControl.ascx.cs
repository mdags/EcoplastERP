using System;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Web.UserForms
{
    public partial class FindPaletteWebUserControl : UserControl, IComplexControl
    {
        private XafApplication application;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
        }
        void IComplexControl.Refresh() { }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select LastWeight, Tare, GrossWeight, NetWeight from ProductionPalette where PaletteNumber = '{0}'", txtBarcode.Text), WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            adapter.Fill(data);
            if (data.Rows.Count > 0)
            {
                lblLastWeight.Text = string.Format("{0:n2}", data.Rows[0]["LastWeight"]);
                lblTare.Text = string.Format("{0:n2}", data.Rows[0]["Tare"]);
                lblGross.Text = string.Format("{0:n2}", data.Rows[0]["GrossWeight"]);
                lblNet.Text = string.Format("{0:n2}", data.Rows[0]["NetWeight"]);
            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@barcode"].Value = txtBarcode.Text;
        }
    }
}