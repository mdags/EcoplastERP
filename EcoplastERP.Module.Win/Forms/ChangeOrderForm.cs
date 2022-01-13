using System;
using System.Windows.Forms;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class ChangeOrderForm : XtraForm
    {
        public XafApplication winApplication;
        public string oldOrderNumber = string.Empty, barcode = string.Empty;
        public int oldOrderLineNumber;

        public ChangeOrderForm()
        {
            InitializeComponent();
        }

        private void ChangeOrderForm_Load(object sender, EventArgs e)
        {
            lblOldOrderInfo.Text = string.Format("Değiştirilecek Sipariş No : {0} ve Kalem Numarası: {1}.\nPalet/Barkod Numarası: {2}", oldOrderNumber, oldOrderLineNumber, barcode);
        }

        private void ChangeOrderForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOrderNumber.Text))
            {
                if (!string.IsNullOrEmpty(txtLineNumber.Text))
                {
                    IObjectSpace objectSpace = winApplication.CreateObjectSpace();

                    SalesOrder salesOrder = objectSpace.FindObject<SalesOrder>(new BinaryOperator("OrderNumber", txtOrderNumber.Text));
                    if (salesOrder != null)
                    {
                        SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(CriteriaOperator.Parse("SalesOrder = ? and LineNumber = ?", salesOrder, txtLineNumber.Text));
                        if (salesOrderDetail != null)
                        {
                            ((XPObjectSpace)objectSpace).Session.ExecuteSprocParametrized("SP_ChangeOrder", new SprocParameter("@oldSalesOrderNumber", oldOrderNumber), new SprocParameter("@oldSalesOrderLineNumber", oldOrderLineNumber), new SprocParameter("@newSalesOrderDetail", salesOrderDetail.Oid), new SprocParameter("@barcode", barcode));
                            XtraMessageBox.Show("İşlem tamamlandı.");
                            this.Close();
                        }
                        else throw new Exception("Sipariş satırı bulunamadı.");
                    }
                    else throw new Exception("Sipariş bulunamadı.");
                }
                else throw new Exception("Kalem No giriniz.");
            }
            else throw new Exception("Sipariş No giriniz.");
        }
    }
}
