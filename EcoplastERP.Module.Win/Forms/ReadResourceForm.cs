using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class ReadResourceForm : XtraForm
    {
        public IObjectSpace objectSpace;
        DataTable dt;
        public ReadResourceForm()
        {
            InitializeComponent();

            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Barkod", typeof(string)));
            dt.Columns.Add(new DataColumn("Birim", typeof(string)));
            dt.Columns.Add(new DataColumn("Miktar", typeof(decimal)));
            gridControl1.DataSource = dt;
            gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
            if (gridView1.Columns["Miktar"] != null) gridView1.Columns["Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Miktar", "{0:n2}");
        }

        private void ReadResourceForm_Load(object sender, EventArgs e)
        {
            SlicingWorkOrder slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (slicingWorkOrder != null)
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            CastSlicingWorkOrder castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (castSlicingWorkOrder != null)
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }

            XPCollection<ReadResource> readResourceList = new XPCollection<ReadResource>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
            foreach (ReadResource readResource in readResourceList)
            {
                DataRow dr = dt.NewRow();
                dr["Barkod"] = readResource.Barcode;
                dr["Birim"] = readResource.Unit.Code;
                dr["Miktar"] = readResource.Quantity;
                dt.Rows.Add(dr);
            }

            gridControl1.DataSource = dt;
            gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
        }

        private void ReadResourceForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string unitCode = string.Empty;
                decimal quantity = 0;
                Store store = objectSpace.FindObject<Store>(new BinaryOperator("Barcode", txtBarcode.Text));
                Production production = objectSpace.FindObject<Production>(new BinaryOperator("Barcode", txtBarcode.Text));

                if (store != null)
                {
                    unitCode = store.cUnit.Code;
                    quantity = store.cQuantity;
                }
                else
                {
                    if (production != null)
                    {
                        unitCode = production.Unit.Code;
                        quantity = production.NetQuantity;
                    }
                }

                decimal rrquantity = Convert.ToDecimal(objectSpace.Evaluate(typeof(ReadResource), CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("Barcode = ?", txtBarcode.Text)));
                txtQuantity.Text = string.Format("{0:n2}", production != null ? (production.NetQuantity - rrquantity) : store.cQuantity);
                if (layoutControlItem4.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Never)
                {
                    DataRow[] filtered = dt.Select(string.Format("Barkod = '{0}'", txtBarcode.Text));
                    if (filtered.Length == 0)
                    {
                        ReadResource readResource = objectSpace.FindObject<ReadResource>(CriteriaOperator.Parse("WorkOrderNumber = ? and Barcode = ?", txtWorkOrderNumber.Text, txtBarcode.Text));
                        if (readResource == null)
                        {
                            if (store != null)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Barkod"] = store.Barcode;
                                dr["Birim"] = store.Unit.Code;
                                dr["Miktar"] = store != null ? store.cQuantity : production.NetQuantity - rrquantity;
                                dt.Rows.Add(dr);
                                gridControl1.DataSource = dt;
                                gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                                txtBarcode.Text = string.Empty;
                            }
                            else
                            {
                                XtraMessageBox.Show("Barkod depoda bulunamadı !");
                                txtBarcode.Text = string.Empty;
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Barkod bu üretim siparişi için zaten kaynak okutulmuş !");
                            txtBarcode.Text = string.Empty;
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Barkod zaten okutulmuş !");
                        txtBarcode.Text = string.Empty;
                    }
                }
                else
                {
                    txtQuantity.Focus();
                }
            }
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow[] filtered = dt.Select(string.Format("Barkod = '{0}'", txtBarcode.Text));
                if (filtered.Length == 0)
                {
                    ReadResource readResource = objectSpace.FindObject<ReadResource>(CriteriaOperator.Parse("WorkOrderNumber = ? and Barcode = ?", txtWorkOrderNumber.Text, txtBarcode.Text));
                    if (readResource == null)
                    {
                        Production production = objectSpace.FindObject<Production>(new BinaryOperator("Barcode", txtBarcode.Text));
                        Store store = objectSpace.FindObject<Store>(new BinaryOperator("Barcode", txtBarcode.Text));
                        if (production != null)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Barkod"] = production.Barcode;
                            dr["Birim"] = production.Unit.Code;
                            dr["Miktar"] = Convert.ToDecimal(txtQuantity.Text);
                            dt.Rows.Add(dr);
                            gridControl1.DataSource = dt;
                            gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                            txtBarcode.Text = string.Empty;
                            txtQuantity.Text = "0,00";
                            txtBarcode.Focus();
                        }
                        else if (store != null)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Barkod"] = store.Barcode;
                            dr["Birim"] = store.Unit.Code;
                            dr["Miktar"] = Convert.ToDecimal(txtQuantity.Text);
                            dt.Rows.Add(dr);
                            gridControl1.DataSource = dt;
                            gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                            txtBarcode.Text = string.Empty;
                            txtQuantity.Text = "0,00";
                            txtBarcode.Focus();
                        }
                        else
                        {
                            XtraMessageBox.Show("Barkod depoda bulunamadı !");
                            txtBarcode.Text = string.Empty;
                            txtQuantity.Text = "0,00";
                            txtBarcode.Focus();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Barkod bu üretim siparişi için zaten kaynak okutulmuş !");
                        txtBarcode.Text = string.Empty;
                        txtQuantity.Text = "0,00";
                        txtBarcode.Focus();
                    }
                }
                else
                {
                    XtraMessageBox.Show("Barkod zaten okutulmuş !");
                    txtBarcode.Text = string.Empty;
                    txtQuantity.Text = "0,00";
                    txtBarcode.Focus();
                }
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                foreach (DataRow row in dt.Rows)
                {
                    ReadResource existsReadResource = objectSpace.FindObject<ReadResource>(CriteriaOperator.Parse("WorkOrderNumber = ? and Barcode = ?", txtWorkOrderNumber.Text, row["Barkod"].ToString()));
                    if (existsReadResource == null)
                    {
                        ReadResource readResource = objectSpace.CreateObject<ReadResource>();
                        readResource.WorkOrderNumber = txtWorkOrderNumber.Text;
                        readResource.Barcode = row["Barkod"].ToString();
                        readResource.Production = objectSpace.FindObject<Production>(new BinaryOperator("Barcode", row["Barkod"].ToString()));
                        readResource.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Code", row["Birim"].ToString()));
                        readResource.Quantity = Convert.ToDecimal(row["Miktar"]);
                    }
                }
                objectSpace.CommitChanges();
                this.Close();
            }
            else XtraMessageBox.Show("Üretim Siparişi No boş olamaz !");
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnMultipleResource_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null)
            {
                throw new UserFriendlyException("Önce örnek bir barkod okutunuz.");
            }
            else
            {
                Production production = objectSpace.FindObject<Production>(new BinaryOperator("Barcode", gridView1.GetFocusedRowCellValue("Barkod").ToString()));
                if (production != null)
                {
                    Warehouse warehouse = null;
                    var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (filmingWorkOrder != null)
                    {
                        warehouse = filmingWorkOrder.Station.SourceWarehouse;
                    }

                    var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (castFilmingWorkOrder != null)
                    {
                        warehouse = castFilmingWorkOrder.Station.SourceWarehouse;
                    }

                    var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (printingWorkOrder != null)
                    {
                        warehouse = printingWorkOrder.Station.SourceWarehouse;
                    }

                    var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (laminationWorkOrder != null)
                    {
                        warehouse = laminationWorkOrder.Station.SourceWarehouse;
                    }

                    var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (slicingWorkOrder != null)
                    {
                        warehouse = slicingWorkOrder.Station.SourceWarehouse;
                    }

                    var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (castTransferingWorkOrder != null)
                    {
                        warehouse = castTransferingWorkOrder.Station.SourceWarehouse;
                    }

                    var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (castSlicingWorkOrder != null)
                    {
                        warehouse = castSlicingWorkOrder.Station.SourceWarehouse;
                    }

                    var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (cuttingWorkOrder != null)
                    {
                        warehouse = cuttingWorkOrder.Station.SourceWarehouse;
                    }

                    var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (foldingWorkOrder != null)
                    {
                        warehouse = foldingWorkOrder.Station.SourceWarehouse;
                    }

                    var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (balloonCuttingWorkOrder != null)
                    {
                        warehouse = balloonCuttingWorkOrder.Station.SourceWarehouse;
                    }

                    var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (eco6WorkOrder != null)
                    {
                        warehouse = eco6WorkOrder.Station.SourceWarehouse;
                    }

                    var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (eco6CuttingWorkOrder != null)
                    {
                        warehouse = eco6CuttingWorkOrder.Station.SourceWarehouse;
                    }

                    var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                    if (eco6LaminationWorkOrder != null)
                    {
                        warehouse = eco6LaminationWorkOrder.Station.SourceWarehouse;
                    }

                    XPCollection<Store> storeList = new XPCollection<Store>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Warehouse = ? and SalesOrderDetail = ?", warehouse, production.SalesOrderDetail));
                    foreach (Store store in storeList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Barkod"] = store.Barcode;
                        dr["Birim"] = store.cUnit.Code;
                        dr["Miktar"] = store.cQuantity;
                        dt.Rows.Add(dr);
                    }

                    gridControl1.DataSource = dt;
                    gridView1.Columns["Miktar"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView1.Columns["Miktar"].DisplayFormat.FormatString = "n2";
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            ReadResource readResource = objectSpace.FindObject<ReadResource>(CriteriaOperator.Parse("WorkOrderNumber = ? and Barcode = ?", txtWorkOrderNumber.Text, gridView1.GetFocusedRowCellValue("Barkod").ToString()));
            if (readResource != null)
            {
                readResource.Delete();
            }
            objectSpace.CommitChanges();

            DataRow[] filtered = dt.Select(string.Format("Barkod = '{0}'", gridView1.GetFocusedRowCellValue("Barkod").ToString()));
            foreach (DataRow row in filtered)
            {
                row.Delete();
            }
        }
    }
}
