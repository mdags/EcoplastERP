using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using AxFIOS2SAPProj1;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraReports.UI;
using DevExpress.Persistent.AuditTrail;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class WastageStickerForm : XtraForm
    {
        public XafApplication winApplication;

        public WastageStickerForm()
        {
            InitializeComponent();
        }

        private void WastageStickerForm_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select W.WorkOrderNumber as [Üretim Siparişi Numarası], W.Barcode as Barkod, M.Name as Makine, W.PaletteNumber as [Palet Numarası], WS.Name as Vardiya, E.NameSurname as [Operatör], U.Code as Birimi, W.GrossQuantity as [Brüt Miktar], W.NetQuantity as [Net Miktar], R.Name as [Fire Nedeni], W.WastageDate as [Fire Tarihi] from Wastage W inner join Machine M on M.Oid = W.Machine inner join ShiftStart S on S.Oid = W.Shift inner join Employee E on E.Oid = W.Employee inner join Unit U on U.Oid = W.Unit inner join WastageReason R on R.Oid = W.WastageReason inner join WorkShift WS on WS.Oid = S.WorkShift where W.OptimisticLockField = 0 And W.GCRecord is null And  W.WorkOrderNumber = 'XXX' order by W.Barcode", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            gridControl1.DataSource = data;
            gridControl1.ForceInitialize();

            Helpers.axctrl = new Axterazi() { Dock = DockStyle.Fill };
            pnlTerazi.Controls.Add(Helpers.axctrl);

            if (gridView1.Columns["Fire Tarihi"] != null)
            {
                gridView1.Columns["Fire Tarihi"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView1.Columns["Fire Tarihi"].DisplayFormat.FormatString = "g";
            }
            if (gridView1.Columns["Üretim Siparişi Numarası"] != null)
                gridView1.Columns["Üretim Siparişi Numarası"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Üretim Siparişi Numarası", "{0:n0}");
            if (gridView1.Columns["Brüt Miktar"] != null)
                gridView1.Columns["Brüt Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Brüt Miktar", "{0:n2}");
            if (gridView1.Columns["Net Miktar"] != null)
                gridView1.Columns["Net Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Net Miktar", "{0:n2}");
        }

        private void WastageStickerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void WastageStickerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Control c in pnlTerazi.Controls) c.Dispose();
            pnlTerazi.Controls.Clear();
        }

        private void RefreshGrid(string workOrderNumber)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Guard.ArgumentNotNull(((XPObjectSpace)objectSpace).Session, "session");
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select W.WorkOrderNumber as [Üretim Siparişi Numarası], W.Barcode as Barkod, M.Name as Makine, W.PaletteNumber as [Palet Numarası], WS.Name as Vardiya, E.NameSurname as [Operatör], U.Code as Birimi, W.GrossQuantity as [Brüt Miktar], W.NetQuantity as [Net Miktar], R.Name as [Fire Nedeni], W.WastageDate as [Fire Tarihi] from Wastage W inner join Machine M on M.Oid = W.Machine inner join ShiftStart S on S.Oid = W.Shift inner join Employee E on E.Oid = W.Employee inner join Unit U on U.Oid = W.Unit inner join WastageReason R on R.Oid = W.WastageReason inner join WorkShift WS on WS.Oid = S.WorkShift where W.OptimisticLockField = 0 And W.GCRecord is null And  W.WorkOrderNumber = '{0}' order by W.Barcode", workOrderNumber), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            gridControl1.DataSource = data;
            gridControl1.ForceInitialize();
            if (gridView1.Columns["Fire Tarihi"] != null)
            {
                gridView1.Columns["Fire Tarihi"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView1.Columns["Fire Tarihi"].DisplayFormat.FormatString = "g";
            }
        }

        private void txtWorkOrderNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                RefreshGrid(txtWorkOrderNumber.Text);

                Station station = null;
                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (filmingWorkOrder != null) station = filmingWorkOrder.Station;

                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castFilmingWorkOrder != null) station = castFilmingWorkOrder.Station;

                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castTransferingWorkOrder != null) station = castTransferingWorkOrder.Station;

                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonFilmingWorkOrder != null) station = balloonFilmingWorkOrder.Station;

                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null) station = printingWorkOrder.Station;

                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (laminationWorkOrder != null) station = laminationWorkOrder.Station;

                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (slicingWorkOrder != null) station = slicingWorkOrder.Station;

                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castSlicingWorkOrder != null) station = castSlicingWorkOrder.Station;

                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (cuttingWorkOrder != null) station = cuttingWorkOrder.Station;

                var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (foldingWorkOrder != null) station = foldingWorkOrder.Station;

                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6WorkOrder != null) station = eco6WorkOrder.Station;

                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6CuttingWorkOrder != null) station = eco6CuttingWorkOrder.Station;

                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6LaminationWorkOrder != null) station = eco6LaminationWorkOrder.Station;

                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (regeneratedWorkOrder != null) station = regeneratedWorkOrder.Station;

                var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castRegeneratedWorkOrder != null) station = castRegeneratedWorkOrder.Station;

                Guard.ArgumentNotNull(((XPObjectSpace)objectSpace).Session, "session");
                DataTable prData = new DataTable();
                SqlDataAdapter prAdapter = new SqlDataAdapter(@"select P.Code as Kod, P.Name as [Adı] from Product P inner join ProductGroup G on P.ProductGroup = G.Oid where G.Code = 'FR'", ((XPObjectSpace)objectSpace).Session.ConnectionString);
                prAdapter.Fill(prData);
                sleWastageCode.Properties.DataSource = prData;
                sleWastageCode.Properties.DisplayMember = "Adı";
                sleWastageCode.Properties.ValueMember = "Kod";
                sleWastageCode.ForceInitialize();

                DataTable wrData = new DataTable();
                SqlDataAdapter wrAdapter = new SqlDataAdapter(String.Format(@"select Code as Kod, Name as [Adı] from WastageReason where Station = '{0}'", station.Oid), ((XPObjectSpace)objectSpace).Session.ConnectionString);
                wrAdapter.Fill(wrData);
                sleWastageReason.Properties.DataSource = wrData;
                sleWastageReason.Properties.DisplayMember = "Adı";
                sleWastageReason.Properties.ValueMember = "Kod";
                sleWastageReason.ForceInitialize();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            AuditTrailService.Instance.EndSessionAudit(((XPObjectSpace)objectSpace).Session);
            IObjectSpace objectSpaceForNumber = winApplication.CreateObjectSpace();
            string barcode = Helpers.GetDocumentNumber(((XPObjectSpace)objectSpace).Session, "EcoplastERP.Module.BusinessObjects.ProductionObjects.Wastage");
            objectSpaceForNumber.CommitChanges();

            using (UnitOfWork uow = new UnitOfWork(((XPObjectSpace)objectSpace).Session.DataLayer))
            {
                decimal grossQuantity = 0;
                if (layoutControlItem5.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                    grossQuantity = Convert.ToDecimal(txtQuantity.Text);
                else grossQuantity = Convert.ToDecimal((Helpers.axctrl as Axterazi).TERAZIOKU.ToString());

                var wastage = new Wastage(uow)
                {
                    WorkOrderNumber = txtWorkOrderNumber.Text,
                    Barcode = barcode,
                    WastageReasonCode = (sleWastageReason.Properties.View).GetFocusedRowCellValue("Kod").ToString(),
                    ProductCode = (sleWastageCode.Properties.View).GetFocusedRowCellValue("Kod").ToString(),
                    GrossQuantity = grossQuantity
                };
                if (uow.InTransaction)
                {
                    uow.CommitTransaction();
                }

                ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                ReportDataV2 sticker = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Fire Etiketi"));
                if (sticker != null)
                {
                    XtraReport stickerReport = ReportDataProvider.ReportsStorage.LoadReport(sticker);
                    stickerReport.DataAdapter = null;
                    XPCollection<Wastage> productionList = new XPCollection<Wastage>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Barcode = ?", barcode));
                    stickerReport.DataSource = productionList;
                    stickerReport.CreateDocument(false);
                    reportsModule.ReportsDataSourceHelper.SetupBeforePrint(stickerReport);
                    try
                    {
                        stickerReport.Print("İŞLETME");
                    }
                    catch { }
                }
            }
            RefreshGrid(txtWorkOrderNumber.Text);
            if (txtQuantity.Visible == true) txtQuantity.Text = "0,00";
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            if (XtraMessageBox.Show("Kaydı silmek istiyor musunuz?", "Onaylana", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                AuditTrailService.Instance.EndSessionAudit(((XPObjectSpace)objectSpace).Session);
                var wastage = objectSpace.FindObject<Wastage>(CriteriaOperator.Parse("Barcode = ?", gridView1.GetFocusedRowCellValue("Barkod")));
                if (wastage != null)
                {
                    var store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", wastage.Barcode));
                    if (store != null)
                    {
                        //Yeni bir malzeme başlık ID alınır.
                        var headerId = Guid.NewGuid();
                        #region Üretime Çıkış İptali
                        //Çıkış hareket türü
                        var input = objectSpace.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P114'"));
                        if (wastage.FilmingWorkOrder != null)
                        {
                            //İş emrinde girilen malzeme ihtiyaçları alınır.
                            var reciept = wastage.FilmingWorkOrder.FilmingWorkOrderReciepts;
                            if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                            {
                                foreach (var item in reciept)
                                {
                                    var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                                    {
                                        HeaderId = headerId,
                                        DocumentNumber = wastage.WorkOrderNumber,
                                        DocumentDate = DateTime.Now,
                                        Barcode = "",
                                        Product = item.Product,
                                        PartyNumber = "",
                                        PaletteNumber = "",
                                        Warehouse = wastage.FilmingWorkOrder.Station.SourceWarehouse,
                                        MovementType = input,
                                        Unit = item.Unit,
                                        Quantity = wastage.GrossQuantity * (decimal)item.Rate / 100
                                    };
                                }
                            }
                        }
                        if (wastage.CastFilmingWorkOrder != null)
                        {
                            //İş emrinde girilen malzeme ihtiyaçları alınır.
                            var reciept = wastage.CastFilmingWorkOrder.CastFilmingWorkOrderReciepts;
                            if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                            {
                                foreach (var item in reciept)
                                {
                                    var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                                    {
                                        HeaderId = headerId,
                                        DocumentNumber = wastage.WorkOrderNumber,
                                        DocumentDate = DateTime.Now,
                                        Barcode = "",
                                        Product = item.Product,
                                        PartyNumber = "",
                                        PaletteNumber = "",
                                        Warehouse = wastage.CastFilmingWorkOrder.Station.SourceWarehouse,
                                        MovementType = input,
                                        Unit = item.Unit,
                                        Quantity = wastage.GrossQuantity * (decimal)item.Rate / 100
                                    };
                                }
                            }
                        }
                        if (wastage.BalloonFilmingWorkOrder != null)
                        {
                            //İş emrinde girilen malzeme ihtiyaçları alınır.
                            var reciept = wastage.BalloonFilmingWorkOrder.BalloonFilmingWorkOrderReciepts;
                            if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                            {
                                foreach (var item in reciept)
                                {
                                    var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                                    {
                                        HeaderId = headerId,
                                        DocumentNumber = wastage.WorkOrderNumber,
                                        DocumentDate = DateTime.Now,
                                        Barcode = "",
                                        Product = item.Product,
                                        PartyNumber = "",
                                        PaletteNumber = "",
                                        Warehouse = wastage.BalloonFilmingWorkOrder.Station.SourceWarehouse,
                                        MovementType = input,
                                        Unit = item.Unit,
                                        Quantity = wastage.GrossQuantity * (decimal)item.Rate / 100
                                    };
                                }
                            }
                        }
                        else if (wastage.RegeneratedWorkOrder != null)
                        {
                            //İş emrinde girilen malzeme ihtiyaçları alınır.
                            var reciept = wastage.RegeneratedWorkOrder.RegeneratedWorkOrderReciepts;
                            if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                            {
                                foreach (var item in reciept)
                                {
                                    var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                                    {
                                        HeaderId = headerId,
                                        DocumentNumber = wastage.WorkOrderNumber,
                                        DocumentDate = DateTime.Now,
                                        Barcode = "",
                                        Product = item.Product,
                                        PartyNumber = "",
                                        PaletteNumber = "",
                                        Warehouse = wastage.RegeneratedWorkOrder.Station.SourceWarehouse,
                                        MovementType = input,
                                        Unit = item.Unit,
                                        Quantity = wastage.GrossQuantity * (decimal)item.Rate / 100
                                    };
                                }
                            }
                        }
                        else if (wastage.CastRegeneratedWorkOrder != null)
                        {
                            //İş emrinde girilen malzeme ihtiyaçları alınır.
                            var reciept = wastage.CastRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts;
                            if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                            {
                                foreach (var item in reciept)
                                {
                                    var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                                    {
                                        HeaderId = headerId,
                                        DocumentNumber = wastage.WorkOrderNumber,
                                        DocumentDate = DateTime.Now,
                                        Barcode = "",
                                        Product = item.Product,
                                        PartyNumber = "",
                                        PaletteNumber = "",
                                        Warehouse = wastage.CastRegeneratedWorkOrder.Station.SourceWarehouse,
                                        MovementType = input,
                                        Unit = item.Unit,
                                        Quantity = wastage.GrossQuantity * (decimal)item.Rate / 100
                                    };
                                }
                            }
                        }
                        else if (wastage.Eco6WorkOrder != null)
                        {
                            //İş emrinde girilen malzeme ihtiyaçları alınır.
                            var reciept = wastage.Eco6WorkOrder.Eco6WorkOrderReciepts;
                            if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                            {
                                foreach (var item in reciept)
                                {
                                    var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                                    {
                                        HeaderId = headerId,
                                        DocumentNumber = wastage.WorkOrderNumber,
                                        DocumentDate = DateTime.Now,
                                        Barcode = "",
                                        Product = item.Product,
                                        PartyNumber = "",
                                        PaletteNumber = "",
                                        Warehouse = wastage.Eco6WorkOrder.Station.SourceWarehouse,
                                        MovementType = input,
                                        Unit = item.Unit,
                                        Quantity = wastage.GrossQuantity * (decimal)item.Rate / 100
                                    };
                                }
                            }
                        }
                        #endregion
                        #region Üretim İptali Hareketi
                        //Üretilen ürünün iptal hareketi yapılır.
                        var output = objectSpace.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P115'"));
                        var imovement = new Movement(((XPObjectSpace)objectSpace).Session)
                        {
                            HeaderId = headerId,
                            DocumentNumber = wastage.WorkOrderNumber,
                            DocumentDate = DateTime.Now,
                            Barcode = store.Barcode,
                            SalesOrderDetail = wastage.SalesOrderDetail,
                            Product = wastage.SalesOrderDetail.Product,
                            PartyNumber = store.PartyNumber,
                            PaletteNumber = store.PaletteNumber,
                            Warehouse = store.Warehouse,
                            MovementType = output,
                            Unit = store.Unit,
                            Quantity = store.Quantity,
                            cUnit = store.cUnit,
                            cQuantity = store.cQuantity
                        };
                        #endregion
                    }

                    XPCollection<ReadResourceMovement> rrmList = new XPCollection<ReadResourceMovement>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("ProductionBarcode = ?", wastage.Barcode));
                    foreach (ReadResourceMovement item in rrmList)
                    {
                        item.ReadResource.IsConsume = false;
                    }
                    objectSpace.Delete(rrmList);

                    objectSpace.Delete(wastage);

                    objectSpace.CommitChanges();
                }
                RefreshGrid(txtWorkOrderNumber.Text);
                if (txtQuantity.Visible == true) txtQuantity.Text = "0,00";
            }
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Wastage wastage = objectSpace.FindObject<Wastage>(new BinaryOperator("Barcode", gridView1.GetFocusedRowCellValue("Barkod").ToString()));
            if (wastage != null)
            {
                ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                ReportDataV2 sticker = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Fire Etiketi"));
                if (sticker != null)
                {
                    XtraReport stickerReport = ReportDataProvider.ReportsStorage.LoadReport(sticker);
                    stickerReport.DataAdapter = null;
                    XPCollection<Wastage> productionList = new XPCollection<Wastage>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Barcode = ?", wastage.Barcode));
                    stickerReport.DataSource = productionList;
                    stickerReport.CreateDocument(false);
                    reportsModule.ReportsDataSourceHelper.SetupBeforePrint(stickerReport);
                    try
                    {
                        stickerReport.Print("İŞLETME");
                    }
                    catch { }
                }
            }
        }
    }
}