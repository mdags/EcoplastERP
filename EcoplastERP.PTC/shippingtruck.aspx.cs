using System;
using System.Web.UI;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingtruck : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Session session = modal.XpoHelper.GetNewSession();
            Expedition expedition = session.FindObject<Expedition>(new BinaryOperator("ExpeditionNumber", txtExpeditionNumber.Text));
            if (expedition != null)
            {
                if (!string.IsNullOrEmpty(txtTruckPlate.Text))
                {
                    expedition.LoadingTruckPlate = txtTruckPlate.Text;
                    expedition.LoadingDorsePlate = txtDorsePlate.Text;
                    expedition.Save();
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Kamyon plakası boş olamaz.";
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Sefer bulunamadı.";
            }
        }
    }
}