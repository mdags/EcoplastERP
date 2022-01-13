using System;
using System.Windows.Forms;
using EcoplastERP.Module.Win.UserForms;
using DevExpress.XtraEditors;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class NotifyShippedParametersForm : Form
    {
        public NotifyShippedUserControl userControl;

        public NotifyShippedParametersForm()
        {
            InitializeComponent();
        }

        private void NotifyShippedParametersForm_Load(object sender, EventArgs e)
        {
            deShippedDate.Properties.MinValue = DateTime.Now;
            deShippedDate.DateTime = DateTime.Now.Hour < 12 ? DateTime.Now : DateTime.Now.AddDays(1);
        }

        private void NotifyShippedParametersForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            userControl.NotifyShipped(deShippedDate.DateTime);
            XtraMessageBox.Show("İşlem tamamlandı.");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
