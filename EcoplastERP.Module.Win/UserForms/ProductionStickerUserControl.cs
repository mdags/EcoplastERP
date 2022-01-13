using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using EcoplastERP.Module.Win.Forms;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class ProductionStickerUserControl : XtraUserControl, IComplexControl
    {
        public ProductionStickerUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select Ms.Oid, S.Name as [İstasyon], M.Code as [Makine], SC.Name as [Duruş Nedeni], (datediff (minute, Ms.BeginDate, getdate())) as [Dur.Sr.(dk)] from MachineStop Ms inner join Machine M on M.Oid = Ms.Machine inner join Station S on S.Oid = M.Station inner join StopCode SC on SC.Oid = Ms.StopCode where Ms.GCRecord is null and Ms.Active = 1", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;
            gridView1.Columns["İstasyon"].GroupIndex = 0;
        }

        private void btnProductionSticker_Click(object sender, EventArgs e)
        {
            ProductionStickerForm form = new ProductionStickerForm()
            {
                winApplication = application
            };
            form.ShowDialog();
        }

        private void btnWastageSticker_Click(object sender, EventArgs e)
        {
            if (Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Fire Girişi"))
            {
                WastageStickerForm form = new WastageStickerForm()
                {
                    winApplication = application
                };
                form.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("Fire girmek için yetkiniz yok.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnWaste_Click(object sender, EventArgs e)
        {
            WasteStickerForm form = new WasteStickerForm()
            {
                winApplication = application
            };
            form.ShowDialog();
        }
    }
}
