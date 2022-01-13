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

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class ErusluMovementReportUserControl : XtraUserControl, IComplexControl
    {
        public ErusluMovementReportUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

            cbWarehouse.Properties.DataSource = new XPCollection<MovementType>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbWarehouse.Properties.DisplayMember = "Name";
            cbWarehouse.Properties.ValueMember = "Oid";

            cbWarehouse.Properties.DataSource = new XPCollection<Warehouse>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Code", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbWarehouse.Properties.DisplayMember = "Code";
            cbWarehouse.Properties.ValueMember = "Oid";

            cbProductGroup.Properties.DataSource = new XPCollection<ProductGroup>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Code = 'SM' or Code = 'OM'"), new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductGroup.Properties.DisplayMember = "Name";
            cbProductGroup.Properties.ValueMember = "Oid";

            cbProductType.Properties.DataSource = new XPCollection<ProductType>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductType.Properties.DisplayMember = "Name";
            cbProductType.Properties.ValueMember = "Oid";

            cbProductKind.Properties.DataSource = new XPCollection<ProductKind>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductKind.Properties.DisplayMember = "Name";
            cbProductKind.Properties.ValueMember = "Oid";

            deBeginDate.DateTime = DateTime.Now.Date.AddDays(-1);
            deEndDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            string sqlText = string.Format(@"select M.DocumentDate as [Hareket Tarihi], T.Name as [Hareket Türü], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], M.PaletteNumber as [Palet Numarası], M.Barcode as [Barkod], M.Quantity as [SÖB Miktarı], U.Code as [SÖB Birim], M.cQuantity as [TÖB Miktarı], CU.Code as [TÖB Birim] from Movement M inner join MovementType T on T.Oid = M.MovementType left outer join SalesOrderDetail D on D.Oid = M.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = M.Product left outer join ProductGroup PG on PG.Oid = P.ProductGroup left outer join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join Warehouse W on W.Oid = M.Warehouse left outer join WarehouseCell WC on WC.Oid = M.WarehouseCell inner join Unit U on U.Oid = M.Unit inner join Unit CU on CU.Oid = M.cUnit where M.GCRecord is null and M.Warehouse in (select Oid from Warehouse where Code in ('900', '901')) and cast(DocumentDate as date) between '{0}' and '{1}' ", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));
            const string orderby = " order by M.DocumentDate desc ";
            string where = string.Empty;

            if (!string.IsNullOrEmpty(cbMovementType.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbMovementType.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and M.MovementType in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbWarehouse.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbWarehouse.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and M.Warehouse in ({0})", list.Substring(0, list.Length - 1));
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
            if (!string.IsNullOrEmpty(OrderNumber.Text))
            {
                string list = string.Empty;
                foreach (string item in OrderNumber.Text.Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and O.OrderNumber in ({0})", list.Substring(0, list.Length - 1));
            }

            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, sqlText + where + orderby).Tables[0];
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Hareket Tarihi"] != null)
            {
                gridView1.Columns["Hareket Tarihi"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView1.Columns["Hareket Tarihi"].DisplayFormat.FormatString = "g";
                if (gridView1.Columns["Hareket Tarihi"].Summary.Count == 0)
                    gridView1.Columns["Hareket Tarihi"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Hareket Tarihi", "{0:n0}");
            }
            if (gridView1.Columns["SÖB Miktarı"] != null)
            {
                if (gridView1.Columns["SÖB Miktarı"].ColumnEdit != null)
                {
                    gridView1.Columns["SÖB Miktarı"].ColumnEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["SÖB Miktarı"].ColumnEdit.EditFormat.FormatString = "n2";
                }
                gridView1.Columns["SÖB Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["SÖB Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["SÖB Miktarı"].Summary.Count == 0)
                    gridView1.Columns["SÖB Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "SÖB Miktarı", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }
    }
}
