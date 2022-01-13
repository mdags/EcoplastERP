using System;
using System.Web.UI;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.PTC
{
    public partial class transferlogin : Page
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
                    var warehouseTransferUser = session.FindObject<WarehouseTransferUser>(CriteriaOperator.Parse("Oid = ? and Password = ?", lstUser.SelectedValue, txtPassword.Text));
                    if (warehouseTransferUser != null)
                    {
                        Session["userOid"] = lstUser.SelectedValue;
                        Response.Redirect("transferwarehouse.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Kullanıcı adı veya şifre yanlış.');", true);
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