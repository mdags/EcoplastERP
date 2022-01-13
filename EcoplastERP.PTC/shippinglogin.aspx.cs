using System;
using System.Web.UI;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.PTC
{
    public partial class shippinglogin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (lstUser.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    Session session = modal.XpoHelper.GetNewSession();
                    var shippingUser = session.FindObject<ShippingUser>(CriteriaOperator.Parse("Oid = ? and Password = ?", lstUser.SelectedValue, txtPassword.Text));
                    if (shippingUser != null)
                    {
                        Session["userOid"] = lstUser.SelectedValue;
                        Response.Redirect("shippinghome.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Şifre yanlış.');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Şifre boş olamaz.');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Kullanıcı seçiniz.');", true);
            }
        }
    }
}