using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class SalesReturnMovementReportUserControl : XtraUserControl, IComplexControl
    {
        public SalesReturnMovementReportUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

            cbContact.Properties.DataSource = new XPCollection<Contact>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbContact.Properties.DisplayMember = "Name";
            cbContact.Properties.ValueMember = "Oid";

            cbProductGroup.Properties.DataSource = new XPCollection<ProductGroup>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Code = 'SM' or Code = 'OM'"), new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductGroup.Properties.DisplayMember = "Name";
            cbProductGroup.Properties.ValueMember = "Oid";

            cbProductType.Properties.DataSource = new XPCollection<ProductType>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductType.Properties.DisplayMember = "Name";
            cbProductType.Properties.ValueMember = "Oid";

            cbProductKind.Properties.DataSource = new XPCollection<ProductKind>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductKind.Properties.DisplayMember = "Name";
            cbProductKind.Properties.ValueMember = "Oid";

            deBeginDate.DateTime = DateTime.Now.Date.AddDays(-30);
            deEndDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            string sqlText = string.Format(@"select D.Barcode as [Barkod], R.DocumentNumber as [Belge No], R.DocumentDate as [İade Tarihi], C.Name as [Cari], SRR.Name as [İade Nedeni], D.Note as [Açıklama], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], D.Quantity as [İade Gelen Miktar], (select isnull(sum(Quantity), 0) from ReadResourceMovement where GCRecord is null and ProductionBarcode like 'M%' and ReadResource in (select Oid from ReadResource where GCRecord is null and Barcode = D.Barcode)) as [Değerlendirme Miktarı] , (select isnull(sum(Quantity), 0) from ReadResourceMovement where GCRecord is null and ProductionBarcode like 'F%' and ReadResource in (select Oid from ReadResource where GCRecord is null and Barcode = D.Barcode)) as [Fire Miktarı] , (select isnull(sum(Quantity), 0) from Store where GCRecord is null and Barcode = D.Barcode) as [Depo Miktarı] from SalesReturnDetail D inner join SalesReturn R on R.Oid = D.SalesReturn inner join Contact C on C.Oid = R.Contact inner join SalesReturnReason SRR on SRR.Oid = D.SalesReturnReason inner join Product P on P.Oid = D.Product inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where D.GCRecord is null and cast(R.DocumentDate as date) between '{0}' and '{1}' ", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));
            string where = string.Empty;

            if (!string.IsNullOrEmpty(cbContact.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbContact.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and R.Contact in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductGroup.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductGroup.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and P.ProductGroup in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductType.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductType.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and P.ProductType in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductKind.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductKind.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and P.ProductKind in ({0})", list.Substring(0, list.Length - 1));
            }

            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, sqlText + where).Tables[0];
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Barkod"] != null)
            {
                gridView1.Columns["Barkod"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Barkod"].DisplayFormat.FormatString = "n0";
                if (gridView1.Columns["Barkod"].Summary.Count == 0)
                    gridView1.Columns["Barkod"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Barkod", "{0:n0}");
            }
            if (gridView1.Columns["İade Gelen Miktar"] != null)
            {
                gridView1.Columns["İade Gelen Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["İade Gelen Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["İade Gelen Miktar"].Summary.Count == 0)
                    gridView1.Columns["İade Gelen Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "İade Gelen Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Değerlendirme Miktarı"] != null)
            {
                gridView1.Columns["Değerlendirme Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Değerlendirme Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Değerlendirme Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Değerlendirme Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Değerlendirme Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["Fire Miktarı"] != null)
            {
                gridView1.Columns["Fire Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Fire Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Fire Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Fire Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Fire Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["Depo Miktarı"] != null)
            {
                gridView1.Columns["Depo Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Depo Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Depo Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Depo Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Depo Miktarı", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }
    }
}
