using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class StoreReportUserControl : XtraUserControl, IComplexControl
    {
        public StoreReportUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        public int reportID;
        string sqlText = string.Empty, where = string.Empty, groupby = string.Empty;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

            cbWarehouse.Properties.DataSource = new XPCollection<Warehouse>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Code", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbWarehouse.Properties.DisplayMember = "Code";
            cbWarehouse.Properties.ValueMember = "Oid";

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

            reportID = 1;
            SelectReportType();
        }

        public void SelectReportType()
        {
            if (reportID == 1)
            {
                sqlText = @"select C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.Oid as SalesOrderDetail, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], D.LineDeliveryDate as [Termin Tarihi], G.Name as [Ürün Grubu], T.Name as [Ürün Tipi], K.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], SUM(S.Quantity) as [SÖB Miktarı], (select Code from Unit where Oid = S.Unit) as [SÖB Birimi], SUM(S.cQuantity) as [TÖB Miktarı], (select Code from Unit where Oid = S.cUnit) as [TÖB Birimi], D.PetkimUnitPrice as [Petkim Birim Fiyat ($)], (select NameSurname from Employee where Oid = O.CreatedBy) as [Siparişi Açan] from Store S left outer join SalesOrderDetail D on D.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder left outer join Contact C on C.Oid = O.Contact left outer join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = S.Product left outer join ProductGroup G on G.Oid = P.ProductGroup left outer join ProductType T on T.Oid = P.ProductType left outer join ProductKind K on K.Oid = P.ProductKind left outer join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell where S.GCRecord is null ";
                groupby = " group by C.Name, SC.Name, D.Oid, D.PetkimUnitPrice, O.OrderNumber, D.LineNumber, D.LineDeliveryDate, G.Name, T.Name, K.Name, P.Code, P.Name, W.Code, WC.Name, S.Unit, S.cUnit, O.CreatedBy order by W.Code";
                PaletteNumber.Text = string.Empty;
                PaletteNumber.Enabled = false;
            }
            if (reportID == 2)
            {
                sqlText = @"select C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.Oid as SalesOrderDetail, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], D.LineDeliveryDate as [Termin Tarihi], G.Name as [Ürün Grubu], T.Name as [Ürün Tipi], K.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], S.PaletteNumber as [Palet Numarası], count(S.Oid) as [Paletteki Ür. Ad.], isnull((select top 1 Tare from ProductionPalette where PaletteNumber = S.PaletteNumber), 0) as [Palet Darası], isnull((select top 1 LastWeight from ProductionPalette where PaletteNumber = S.PaletteNumber), 0) as [Palet Son Ağırlık], SUM(S.Quantity) as [SÖB Miktarı], (select Code from Unit where Oid = S.Unit) as [SÖB Birimi], SUM(S.cQuantity) as [TÖB Miktarı], (select Code from Unit where Oid = S.cUnit) as [TÖB Birimi], D.PetkimUnitPrice as [Petkim Birim Fiyat ($)], (select NameSurname from Employee where Oid = O.CreatedBy) as [Siparişi Açan] from Store S left outer join SalesOrderDetail D on D.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder left outer join Contact C on C.Oid = O.Contact left outer join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = S.Product left outer join ProductGroup G on G.Oid = P.ProductGroup left outer join ProductType T on T.Oid = P.ProductType left outer join ProductKind K on K.Oid = P.ProductKind left outer join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell where S.GCRecord is null ";
                groupby = " group by C.Name, SC.Name, D.Oid, D.PetkimUnitPrice, O.OrderNumber, D.LineNumber, D.LineDeliveryDate, G.Name, T.Name, K.Name, P.Code, P.Name, W.Code, WC.Name, S.PaletteNumber, S.Unit, S.cUnit, O.CreatedBy order by W.Code";
                PaletteNumber.Text = string.Empty;
                PaletteNumber.Enabled = true;
            }
            if (reportID == 3)
            {
                sqlText = @"select C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.Oid as SalesOrderDetail, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], D.LineDeliveryDate as [Termin Tarihi], G.Name as [Ürün Grubu], T.Name as [Ürün Tipi], K.Name as [Ürün Cinsi], P.Code as [Stok Kodu], P.Name as [Stok Adı], W.Code as [Depo], WC.Name as [Hücre], S.PaletteNumber as [Palet Numarası], S.Barcode as [Barkod], (select ProductionDate from Production where Barcode = S.Barcode) as [Üretim Tarihi], SUM(S.Quantity) as [SÖB Miktarı], (select Code from Unit where Oid = S.Unit) as [SÖB Birimi], SUM(S.cQuantity) as [TÖB Miktarı], (select Code from Unit where Oid = S.cUnit) as [TÖB Birimi], D.PetkimUnitPrice as [Petkim Birim Fiyat ($)], (select NameSurname from Employee where Oid = O.CreatedBy) as [Siparişi Açan] from Store S left outer join SalesOrderDetail D on D.Oid = S.SalesOrderDetail left outer join SalesOrder O on O.Oid = D.SalesOrder left outer join Contact C on C.Oid = O.Contact left outer join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = S.Product left outer join ProductGroup G on G.Oid = P.ProductGroup left outer join ProductType T on T.Oid = P.ProductType left outer join ProductKind K on K.Oid = P.ProductKind left outer join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell where S.GCRecord is null ";
                groupby = " group by C.Name, SC.Name, D.Oid, D.PetkimUnitPrice, O.OrderNumber, D.LineNumber, D.LineDeliveryDate, G.Name, T.Name, K.Name, P.Code, P.Name, S.PaletteNumber, W.Code, WC.Name, S.Barcode, S.Unit, S.cUnit, O.CreatedBy order by W.Code";
                PaletteNumber.Text = string.Empty;
                PaletteNumber.Enabled = true;
            }
        }
        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;

            if (!string.IsNullOrEmpty(cbWarehouse.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbWarehouse.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and S.Warehouse in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbContact.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbContact.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and C.Oid in ({0})", list.Substring(0, list.Length - 1));
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
            if (!string.IsNullOrEmpty(OrderLineNumber.Text)) where += String.Format(" and D.LineNumber = '{0}'", OrderLineNumber.Text);
            if (!string.IsNullOrEmpty(PaletteNumber.Text))
            {
                string list = string.Empty;
                foreach (string item in PaletteNumber.Text.Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and S.PaletteNumber in ({0})", list.Substring(0, list.Length - 1));
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(dt);
            dt.Columns["SalesOrderDetail"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;

            if (gridView1.Columns["Firma"] != null)
            {
                gridView1.Columns["Firma"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["Firma"].DisplayFormat.FormatString = "n0";
                if (gridView1.Columns["Firma"].Summary.Count == 0)
                    gridView1.Columns["Firma"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Firma", "{0:n0}");
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
            if (reportID == 2)
            {
                if (gridView1.Columns["Palet Darası"] != null)
                {
                    gridView1.Columns["Palet Darası"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Palet Darası"].DisplayFormat.FormatString = "n2";
                    if (gridView1.Columns["Palet Darası"].Summary.Count == 0)
                        gridView1.Columns["Palet Darası"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Palet Darası", "{0:n2}");
                }
            }
            if (gridView1.Columns["TÖB Miktarı"] != null)
            {
                gridView1.Columns["TÖB Miktarı"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                gridView1.Columns["TÖB Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["TÖB Miktarı"].Summary.Count == 0)
                    gridView1.Columns["TÖB Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TÖB Miktarı", "{0:n2}");
            }

            if (reportID == 3)
            {
                gridView1.OptionsBehavior.Editable = true;
                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    if (gridView1.Columns[i].FieldName == "SÖB Miktarı")
                    {
                        if (Helpers.IsUserAdministrator() || Helpers.IsUserInRole("SÖB Miktar Değiştirme"))
                            gridView1.Columns[i].OptionsColumn.AllowEdit = true;
                        else gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    }
                    else gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                }
            }
            else gridView1.OptionsBehavior.Editable = false;

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //if (gridView1.FocusedValue == null) return;
            //StoreDetailReport detailReport = new StoreDetailReport()
            //{
            //    winApplication = application,
            //    PaletteNumber = gridView1.GetFocusedRowCellValue("Palet Numarası").ToString(),
            //    SalesOrderDetail = gridView1.GetFocusedRowCellValue("SalesOrderDetail").ToString()
            //};
            //detailReport.ShowDialog();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "SÖB Miktarı")
            {
                if (Helpers.IsUserAdministrator() || Helpers.IsUserInRole("SÖB Miktar Değiştirme"))
                {
                    decimal Quantity = Convert.ToDecimal(e.Value);
                    ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Store set Quantity = @quantity where GCrecord is null and Barcode = @barcode
                update Production set cQuantity = @quantity where GCRecord is null and Barcode = @barcode", new string[] { "@quantity", "@barcode" }, new object[] { Quantity, gridView1.GetFocusedRowCellValue("Barkod").ToString() });
                }
            }
        }

        public void SalesOrderTransfer(SalesOrderDetail salesOrderDetail)
        {
            if (gridView1.FocusedValue == null) return;
            if (reportID == 2)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Production set SalesOrderDetail = @salesOrderDetail where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber)   update Store set SalesOrderDetail = @salesOrderDetail where GCRecord is null and PaletteNumber = @paletteNumber", new string[] { "@salesOrderDetail", "@paletteNumber" }, new object[] { salesOrderDetail.Oid, row["Palet Numarası"].ToString() });
                    }
                }
            }
            else if (reportID == 3)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Production set SalesOrderDetail = @salesOrderDetail where GCRecord is null and Barcode = @barcode   update Store set SalesOrderDetail = @salesOrderDetail where GCRecord is null and Barcode = @barcode", new string[] { "@salesOrderDetail", "@barcode" }, new object[] { salesOrderDetail.Oid, row["Barkod"].ToString() });
                    }
                }
            }

            RefreshGrid();
            XtraMessageBox.Show("İşlem tamamlandı.");
        }

        public void PaletteTransfer(string paletteNumber)
        {
            if (gridView1.FocusedValue == null) return;
            if (reportID == 2)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Production set ProductionPalette = (select top 1 Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber) where GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @oldPaletteNumber)   update Store set PaletteNumber = @paletteNumber where GCRecord is null and PaletteNumber = @oldPaletteNumber", new string[] { "@paletteNumber", "@oldPaletteNumber" }, new object[] { paletteNumber, row["Palet Numarası"].ToString() });
                    }
                }
            }
            else if (reportID == 3)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Production set ProductionPalette = (select top 1 Oid from ProductionPalette where GCRecord is null and PaletteNumber = @paletteNumber) where GCRecord is null and Barcode = @barcode   update Store set PaletteNumber = @paletteNumber where GCRecord is null and Barcode = @barcode", new string[] { "@paletteNumber", "@barcode" }, new object[] { paletteNumber, row["Barkod"].ToString() });
                    }
                }
            }

            RefreshGrid();
            XtraMessageBox.Show("İşlem tamamlandı.");
        }

        public void WarehouseTransfer(Warehouse warehouse)
        {
            if (gridView1.FocusedValue == null) return;
            if (reportID == 2)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        if (!string.IsNullOrEmpty(row["Palet Numarası"].ToString()))
                        {
                            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Store set Warehouse = @warehouse where GCRecord is null and PaletteNumber = @paletteNumber", new string[] { "@warehouse", "@paletteNumber" }, new object[] { warehouse.Oid, row["Palet Numarası"].ToString() });
                        }
                    }
                }
            }
            else if (reportID == 3)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        if (!string.IsNullOrEmpty(row["Barkod"].ToString()))
                        {
                            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Store set Warehouse = @warehouse where GCRecord is null and Barcode = @barcode", new string[] { "@warehouse", "@barcode" }, new object[] { warehouse.Oid, row["Barkod"].ToString() });
                        }
                    }
                }
            }

            RefreshGrid();
            XtraMessageBox.Show("İşlem tamamlandı.");
        }

        public void UpdateOthers()
        {
            if (gridView1.FocusedValue == null) return;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                decimal Quantity = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("SÖB Miktarı"));
                ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"update Store set Quantity = @quantity where GCrecord is null and Barcode = @barcode
                update Production set cQuantity = @quantity where GCRecord is null and Barcode = @barcode 
                update DeliveryDetailLoading set Quantity = @quantity where GCRecord is null and Barcode = @barcode", new string[] { "@quantity", "@barcode" }, new object[] { Quantity, gridView1.GetRowCellValue(i, "Barkod").ToString() });
            }
            RefreshGrid();
        }

        public void WarehouseExit()
        {
            if (gridView1.FocusedValue == null) return;
            if (reportID == 2)
            {
                Guid headerId = Guid.NewGuid();
                MovementType movementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P131"));
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) select NEWID(), @headerId, '', GETDATE(), @movementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL from Store where GCRecord is null and PaletteNumber = @paletteNumber  
                        update Store set GCRecord = 1 where GCRecord is null and PaletteNumber = @paletteNumber", new string[] { "@headerId", "@movementType", "@paletteNumber" }, new object[] { headerId, movementType.Oid, row["Palet Numarası"].ToString() });
                    }
                }
            }
            else if (reportID == 3)
            {
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    if (gridView1.GetSelectedRows()[i] >= 0)
                    {
                        DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                        Store store = objectSpace.FindObject<Store>(new BinaryOperator("Barcode", row["Barkod"].ToString()));
                        if (store != null)
                        {
                            Movement movement = objectSpace.CreateObject<Movement>();
                            movement.HeaderId = Guid.NewGuid();
                            movement.DocumentNumber = string.Empty;
                            movement.DocumentDate = DateTime.Now;
                            movement.MovementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P131"));
                            movement.Barcode = store.Barcode;
                            movement.SalesOrderDetail = store.SalesOrderDetail;
                            movement.Product = store.Product;
                            movement.PartyNumber = store.PartyNumber;
                            movement.PaletteNumber = store.PaletteNumber;
                            movement.Warehouse = store.Warehouse;
                            movement.WarehouseCell = store.WarehouseCell;
                            movement.Unit = store.Unit;
                            movement.Quantity = store.Quantity;
                            movement.cUnit = store.cUnit;
                            movement.cQuantity = store.cQuantity;

                            objectSpace.CommitChanges();
                        }
                    }
                }
            }

            RefreshGrid();
            XtraMessageBox.Show("İşlem tamamlandı.");
        }

        public void WarehouseEntry(string barcode)
        {
            if (gridView1.FocusedValue == null) return;
            if (barcode.StartsWith("P"))
            {
                Guid headerId = Guid.NewGuid();
                MovementType movementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P130"));
                ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) select NEWID(), @headerId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL from Movement where MovementType = 'BF151A95-F8E3-443F-B11E-254E43F2CD79' and PaletteNumber = @paletteNumber  
                insert into Store(Oid, Product, Barcode, SalesOrderDetail, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord) select NEWID(), Product, Barcode, SalesOrderDetail, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL from Movement where MovementType = 'BF151A95-F8E3-443F-B11E-254E43F2CD79' and PaletteNumber = @paletteNumber", new string[] { "@headerId", "@movementType", "@paletteNumber" }, new object[] { headerId, movementType.Oid, barcode });
            }
            else
            {
                Movement searchMovement = objectSpace.FindObject<Movement>(CriteriaOperator.Parse("MovementType.Code = 'P110' and Barcode = ?", barcode));
                if (searchMovement != null)
                {
                    Movement movement = objectSpace.CreateObject<Movement>();
                    movement.HeaderId = Guid.NewGuid();
                    movement.DocumentNumber = searchMovement.DocumentNumber;
                    movement.DocumentDate = searchMovement.DocumentDate;
                    movement.MovementType = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P130"));
                    movement.Barcode = searchMovement.Barcode;
                    movement.SalesOrderDetail = searchMovement.SalesOrderDetail;
                    movement.Product = searchMovement.Product;
                    movement.PartyNumber = searchMovement.PartyNumber;
                    movement.PaletteNumber = searchMovement.PaletteNumber;
                    movement.Warehouse = searchMovement.Warehouse;
                    movement.WarehouseCell = searchMovement.WarehouseCell;
                    movement.Unit = searchMovement.Unit;
                    movement.Quantity = searchMovement.Quantity;
                    movement.cUnit = searchMovement.cUnit;
                    movement.cQuantity = searchMovement.cQuantity;

                    objectSpace.CommitChanges();
                }
            }

            RefreshGrid();
            XtraMessageBox.Show("İşlem tamamlandı.");
        }

        public void ConsumeBarcode(string workOrderNumber)
        {
            if (gridView1.FocusedValue == null) return;
            Guid headerId = Guid.NewGuid();
            MovementType outType = objectSpace.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P123'"));
            SalesOrderDetail salesOrderDetail = null;
            var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (filmingWorkOrder != null) salesOrderDetail = filmingWorkOrder.SalesOrderDetail;
            var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (castFilmingWorkOrder != null) salesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;
            var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (castTransferingWorkOrder != null) salesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;
            var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (printingWorkOrder != null) salesOrderDetail = printingWorkOrder.SalesOrderDetail;
            var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (laminationWorkOrder != null) salesOrderDetail = laminationWorkOrder.SalesOrderDetail;
            var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (slicingWorkOrder != null) salesOrderDetail = slicingWorkOrder.SalesOrderDetail;
            var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (castSlicingWorkOrder != null) salesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;
            var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (cuttingWorkOrder != null) salesOrderDetail = cuttingWorkOrder.SalesOrderDetail;
            var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (regeneratedWorkOrder != null) salesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;
            var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (eco6WorkOrder != null) salesOrderDetail = eco6WorkOrder.SalesOrderDetail;
            var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (eco6LaminationWorkOrder != null) salesOrderDetail = eco6LaminationWorkOrder.SalesOrderDetail;
            var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", workOrderNumber));
            if (eco6CuttingWorkOrder != null) salesOrderDetail = eco6CuttingWorkOrder.SalesOrderDetail;

            DataRow rowData;
            int[] listRowList = this.gridView1.GetSelectedRows();
            for (int i = 0; i < listRowList.Length; i++)
            {
                rowData = this.gridView1.GetDataRow(listRowList[i]);
                if (rowData != null)
                {
                    if (!string.IsNullOrEmpty(rowData["Barkod"].ToString()))
                    {
                        Store store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", rowData["Barkod"].ToString()));
                        if (store != null)
                        {
                            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"insert into Movement(Oid, HeaderId, DocumentNumber, DocumentDate, MovementType, Barcode, SalesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, PurchaseWaybillDetail, WarehouseCell) select NEWID(), @headerId, @documentNumber, GETDATE(), @movementType, Barcode, @salesOrderDetail, Product, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, 0, NULL, NULL, WarehouseCell from Store where GCRecord is null and Barcode = @barcode", new string[] { "@headerId", "@documentNumber", "@movementType", "@salesOrderDetail", "@barcode" }, new object[] { headerId, workOrderNumber, outType.Oid, salesOrderDetail.Oid, store.Barcode });
                            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"delete Store where GCRecord is null and Barcode = @barcode", new string[] { "@barcode" }, new object[] { store.Barcode });
                        }
                    }
                }
            }

            RefreshGrid();
        }
    }
}