using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class SalesReturnReportUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;

        public SalesReturnReportUserControl()
        {
            InitializeComponent();
        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        private void SalesReturnReportUserControl_Load(object sender, EventArgs e)
        {
            cbContact.Properties.DataSource = new XPCollection<Contact>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbContact.Properties.DisplayMember = "Name";
            cbContact.Properties.ValueMember = "Oid";

            cbProductType.Properties.DataSource = new XPCollection<ProductType>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductType.Properties.DisplayMember = "Name";
            cbProductType.Properties.ValueMember = "Oid";

            cbProductKind.Properties.DataSource = new XPCollection<ProductKind>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductKind.Properties.DisplayMember = "Name";
            cbProductKind.Properties.ValueMember = "Oid";

            deBeginDate.DateTime = DateTime.Now.AddDays(-1);
            deEndDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            string where = string.Empty;
            string sqlText = string.Format(@"select R.DocumentDate as [Belge Tarihi], C.Name as [Müşteri], P.Name as [Stok Adı], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], D.Barcode as [Barkod], D.Quantity as [Miktar], U.Code as [Birim], D.Note as [Not], RR.Name as [Satış İade Nedeni] from SalesReturnDetail D inner join SalesReturn R on R.Oid = D.SalesReturn inner join Contact C on C.Oid = R.Contact inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind inner join SalesReturnReason RR on RR.Oid = D.SalesReturnReason inner join Unit U on U.Oid = D.Unit where D.GCRecord is null and cast(R.DocumentDate as date) between '{0}' and '{1}'", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));

            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;

            if (!string.IsNullOrEmpty(cbContact.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbContact.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and C.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductType.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductType.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and PT.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductKind.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductKind.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and PK.Oid in ({0})", list.Substring(0, list.Length - 1));
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();

            if (gridView1.Columns["Miktar"] != null)
            {
                gridView1.Columns["Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Miktar"].Summary.Count == 0)
                    gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }
    }
}
