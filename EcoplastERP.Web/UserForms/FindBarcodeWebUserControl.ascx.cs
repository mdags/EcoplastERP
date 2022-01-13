using System;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Web.UserForms
{
    public partial class FindBarcodeWebUserControl : UserControl, IComplexControl
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

        void RefreshGrid(string barcode)
        {
            IObjectSpace objectSpace = application.CreateObjectSpace();
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select C.Name as Contact, P.WorkOrderNumber, M.Code, WS.Name as [Shift], L.PaletteNumber, H.Code as Machine, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as OrderNumber, E.NameSurname as Employee, P.ProductionDate, P.GrossQuantity, P.NetQuantity from Production P inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join Product M on M.Oid = D.Product left outer join ProductionPalette L on L.Oid = P.ProductionPalette inner join Machine H on H.Oid = P.Machine inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Employee E on E.Oid = P.Employee inner join ShiftStart SS on SS.Oid = P.[Shift] inner join WorkShift WS on WS.Oid = SS.WorkShift where P.Barcode = '{0}'", barcode), WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            adapter.Fill(data);
            if (data.Rows.Count > 0)
            {
                lblContact.Text = data.Rows[0]["Contact"].ToString();
                lblWorkOrderNumber.Text = data.Rows[0]["WorkOrderNumber"].ToString();
                lblProductCode.Text = data.Rows[0]["Code"].ToString();
                lblShift.Text = data.Rows[0]["Shift"].ToString();
                lblPaletteNumber.Text = data.Rows[0]["PaletteNumber"].ToString();
                lblMachine.Text = data.Rows[0]["Machine"].ToString();
                lblOrderNumber.Text = data.Rows[0]["OrderNumber"].ToString();
                lblEmployee.Text = data.Rows[0]["Employee"].ToString();
                lblProductionDate.Text = data.Rows[0]["ProductionDate"].ToString();
                lblGrossQuantity.Text = string.Format("{0:n2}", data.Rows[0]["GrossQuantity"]);
                lblNetQuantity.Text = string.Format("{0:n2}", data.Rows[0]["NetQuantity"]);
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            RefreshGrid(txtBarcode.Text);
        }

        protected void SqlDataSource1_Selecting(object sender, System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@barcode"].Value = txtBarcode.Text;
        }
    }
}