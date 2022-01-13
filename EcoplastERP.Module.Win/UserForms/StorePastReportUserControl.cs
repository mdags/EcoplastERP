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
    public partial class StorePastReportUserControl : XtraUserControl, IComplexControl
    {
        public StorePastReportUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

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

            deStoreDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            string where = string.Empty;

            string sqlText = string.Format(@"select CAST(S.StoreDate as date) as [Tarih], O.OrderNumber+'/'+cast(SD.LineNumber as varchar(5)) as [Sipariş No], PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo Kodu], W.Name as [Depo Adı], WC.Name as [Hücre], S.PaletteNumber as [Palet Numarası], SUM(S.Quantity) as [Miktar], U.Code as [Birim], SUM(S.cQuantity) as [Çevrim Miktarı], CU.Code as [Çevrim Birimi] from StorePast S inner join Product P on P.Oid = S.Product left outer join SalesOrderDetail SD on SD.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = SD.SalesOrder inner join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell inner join Unit U on U.Oid = S.Unit inner join Unit CU on CU.Oid = S.cUnit inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind where S.GCRecord is null and CAST(S.StoreDate as date) = '{0}' ", deStoreDate.DateTime.ToString("yyyy-MM-dd"));
            const string groupby = " group by CAST(S.StoreDate as date), O.OrderNumber, SD.LineNumber, PG.Name, PT.Name, PK.Name, P.Code, P.Name, W.Code, W.Name, WC.Name, S.PaletteNumber, U.Code, CU.Code";

            if (!string.IsNullOrEmpty(cbWarehouse.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbWarehouse.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and S.Warehouse in ({0})", list.Substring(0, list.Length - 1));
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
            
            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, sqlText + where + groupby).Tables[0];
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Tarih"] != null)
            {
                gridView1.Columns["Tarih"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Tarih"].DisplayFormat.FormatString = "n0";
                if (gridView1.Columns["Tarih"].Summary.Count == 0)
                    gridView1.Columns["Tarih"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Tarih", "{0:n0}");
            }
            if (gridView1.Columns["Miktar"] != null)
            {
                if (gridView1.Columns["Miktar"].ColumnEdit != null)
                {
                    gridView1.Columns["Miktar"].ColumnEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Miktar"].ColumnEdit.EditFormat.FormatString = "n2";
                }
                gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Miktar"].Summary.Count == 0)
                    gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Çevrim Miktarı"] != null)
            {
                gridView1.Columns["Çevrim Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Çevrim Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Çevrim Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Çevrim Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Çevrim Miktarı", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }
    }
}