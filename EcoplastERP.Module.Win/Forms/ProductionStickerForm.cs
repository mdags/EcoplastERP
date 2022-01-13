using System;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.AuditTrail;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.ParametersObjects;
using EcoplastERP.Module.BusinessObjects.MaintenanceObjects;
using AxFIOS2SAPProj1;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class ProductionStickerForm : XtraForm
    {
        public XafApplication winApplication;

        public ProductionStickerForm()
        {
            InitializeComponent();
        }

        private void ProductionStickerForm_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Guard.ArgumentNotNull(((XPObjectSpace)objectSpace).Session, "session");
            RefreshGrid("XXX");

            DataTable prData = new DataTable();
            SqlDataAdapter prAdapter = new SqlDataAdapter(@"select Code from Machine where GCRecord is null order by Code", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            prAdapter.Fill(prData);
            foreach (DataRow row in prData.Rows)
            {
                cmbMachine.Properties.Items.Add(row["Code"]);
            }

            Helpers.axctrl = new Axterazi() { Dock = DockStyle.Fill };
            pnlTerazi.Controls.Add(Helpers.axctrl);

            if (gridView1.Columns["Üretim Siparişi Numarası"] != null)
                gridView1.Columns["Üretim Siparişi Numarası"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Üretim Siparişi Numarası", "{0:n0}");
            if (gridView1.Columns["Brüt Miktar"] != null)
                gridView1.Columns["Brüt Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Brüt Miktar", "{0:n2}");
            if (gridView1.Columns["Net Miktar"] != null)
                gridView1.Columns["Net Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Net Miktar", "{0:n2}");

            timer1.Enabled = true;
            ShiftStart shiftStart = objectSpace.FindObject<ShiftStart>(new BinaryOperator("Active", true));
            if (shiftStart != null)
            {
                double hour = (DateTime.Now - shiftStart.StartTime).TotalHours;
                if (hour >= 8) XtraMessageBox.Show("Önceki vardiya çalışma süresini tamamladı. Yeni vardiya başlatmanız gerekebilir.");
            }
        }

        private void ProductionStickerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void ProductionStickerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;

            foreach (Control c in pnlTerazi.Controls) c.Dispose();
            pnlTerazi.Controls.Clear();
        }

        private void RefreshGrid(string workOrderNumber)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Guard.ArgumentNotNull(((XPObjectSpace)objectSpace).Session, "session");
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(string.Format(@"select P.WorkOrderNumber as [Üretim Siparişi Numarası], P.Barcode as Barkod, M.Name as Makine, l.PaletteNumber as [Palet Numarası]
            , (case when FilmingWorkOrder is not null then (select Width from FilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when CastFilmingWorkOrder is not null then (select Width from CastFilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when CastTransferingWorkOrder is not null then (select Width from CastTransferingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when BalloonFilmingWorkOrder is not null then (select Width from BalloonFilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when PrintingWorkOrder is not null then (select Width from PrintingWorkOrder where Oid = P.PrintingWorkOrder) when LaminationWorkOrder is not null then (select Width from LaminationWorkOrder where Oid = P.LaminationWorkOrder) when SlicingWorkOrder is not null then (select Width from SlicingWorkOrder where Oid = P.SlicingWorkOrder) when CastSlicingWorkOrder is not null then (select Width from CastSlicingWorkOrder where Oid = P.CastSlicingWorkOrder) when CuttingWorkOrder is not null then (select Width from CuttingWorkOrder where Oid = P.CuttingWorkOrder) when FoldingWorkOrder is not null then (select Width from FoldingWorkOrder where Oid = P.FoldingWorkOrder) when BalloonCuttingWorkOrder is not null then (select Width from BalloonCuttingWorkOrder where Oid = P.BalloonCuttingWorkOrder) when Eco6WorkOrder is not null then '0' when Eco6CuttingWorkOrder is not null then (select Width from Eco6CuttingWorkOrder where Oid = P.Eco6CuttingWorkOrder) when Eco6LaminationWorkOrder is not null then (select Width from Eco6LaminationWorkOrder where Oid = P.Eco6LaminationWorkOrder) else '0' end) as En
            , (case when FilmingWorkOrder is not null then (select Height from FilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when CastFilmingWorkOrder is not null then (select Height from CastFilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when CastTransferingWorkOrder is not null then (select Height from CastTransferingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when BalloonFilmingWorkOrder is not null then (select Height from BalloonFilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when PrintingWorkOrder is not null then (select Height from PrintingWorkOrder where Oid = P.PrintingWorkOrder) when LaminationWorkOrder is not null then (select Width from LaminationWorkOrder where Oid = P.LaminationWorkOrder) when SlicingWorkOrder is not null then (select Width from SlicingWorkOrder where Oid = P.SlicingWorkOrder) when CastSlicingWorkOrder is not null then (select Height from CastSlicingWorkOrder where Oid = P.CastSlicingWorkOrder) when CuttingWorkOrder is not null then (select Width from CuttingWorkOrder where Oid = P.CuttingWorkOrder) when FoldingWorkOrder is not null then (select Height from FoldingWorkOrder where Oid = P.FoldingWorkOrder) when BalloonCuttingWorkOrder is not null then (select Height from BalloonCuttingWorkOrder where Oid = P.BalloonCuttingWorkOrder) when Eco6WorkOrder is not null then '0' when Eco6CuttingWorkOrder is not null then (select Height from Eco6CuttingWorkOrder where Oid = P.Eco6CuttingWorkOrder) when Eco6LaminationWorkOrder is not null then (select Height from Eco6LaminationWorkOrder where Oid = P.Eco6LaminationWorkOrder) else '0' end) as Boy
            , (case when FilmingWorkOrder is not null then (select [Length] from FilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when CastFilmingWorkOrder is not null then (select [Length] from CastFilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when CastTransferingWorkOrder is not null then (select [Length] from CastTransferingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when BalloonFilmingWorkOrder is not null then (select [Length] from BalloonFilmingWorkOrder where WorkOrderNumber = P.WorkOrderNumber) when PrintingWorkOrder is not null then (select [Length] from PrintingWorkOrder where Oid = P.PrintingWorkOrder) when LaminationWorkOrder is not null then (select [Length] from LaminationWorkOrder where Oid = P.LaminationWorkOrder) when SlicingWorkOrder is not null then (select [Length] from SlicingWorkOrder where Oid = P.SlicingWorkOrder) when CastSlicingWorkOrder is not null then (select [Length] from CastSlicingWorkOrder where Oid = P.CastSlicingWorkOrder) when CuttingWorkOrder is not null then (select [Length] from CuttingWorkOrder where Oid = P.CuttingWorkOrder) when FoldingWorkOrder is not null then (select [Length] from FoldingWorkOrder where Oid = P.FoldingWorkOrder) when BalloonCuttingWorkOrder is not null then (select [Length] from BalloonCuttingWorkOrder where Oid = P.BalloonCuttingWorkOrder) when Eco6WorkOrder is not null then '0' when Eco6CuttingWorkOrder is not null then (select [Length] from Eco6CuttingWorkOrder where Oid = P.Eco6CuttingWorkOrder) when Eco6LaminationWorkOrder is not null then (select [Length] from Eco6LaminationWorkOrder where Oid = P.Eco6LaminationWorkOrder) else '0' end) as Uzunluk
            , WS.Name as Vardiya, E.NameSurname as [Operatör], U.Code as Birimi, P.GrossQuantity as [Brüt Miktar], P.NetQuantity as [Net Miktar], P.ProductionDate as [Üretim Tarihi], (case when (select Oid from Store where GCRecord is null and Barcode = P.Barcode and Warehouse = (case when P.FilmingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from FilmingWorkOrder where Oid = P.FilmingWorkOrder)) when P.CastFilmingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from CastFilmingWorkOrder where Oid = P.CastFilmingWorkOrder)) when P.CastTransferingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from CastTransferingWorkOrder where Oid = P.CastTransferingWorkOrder)) when P.BalloonFilmingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from BalloonFilmingWorkOrder where Oid = P.BalloonFilmingWorkOrder)) when P.PrintingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from PrintingWorkOrder where Oid = P.PrintingWorkOrder)) when P.LaminationWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from LaminationWorkOrder where Oid = P.LaminationWorkOrder)) when P.SlicingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from SlicingWorkOrder where Oid = P.SlicingWorkOrder)) when P.CastSlicingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from CastSlicingWorkOrder where Oid = P.CastSlicingWorkOrder)) when P.CuttingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from CuttingWorkOrder where Oid = P.CuttingWorkOrder)) when P.Eco6WorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from Eco6WorkOrder where Oid = P.Eco6WorkOrder)) when P.Eco6CuttingWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from Eco6CuttingWorkOrder where Oid = P.Eco6CuttingWorkOrder)) when P.Eco6LaminationWorkOrder is not null then (select QuarantineWarehouse from Station where Oid = (select Station from Eco6LaminationWorkOrder where Oid = P.Eco6LaminationWorkOrder)) else NEWID() end)) is not null then 1 else 0 end) as [Karantina] from Production P inner join Machine M on M.Oid = P.Machine inner join ShiftStart S on S.Oid = P.[Shift] left outer join Employee E on E.Oid = P.Employee inner join Unit U on U.Oid = P.Unit left outer join ProductionPalette l on l.Oid = P.ProductionPalette inner join WorkShift WS on WS.Oid = S.WorkShift where P.GCRecord is null And P.WorkOrderNumber = '{0}' order by P.Barcode", workOrderNumber), ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(data);
            //data.Columns["Karantina"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = data;
            gridControl1.ForceInitialize();
            if (gridView1.Columns.Count > 0)
            {
                gridView1.Columns["Üretim Tarihi"].DisplayFormat.FormatType = FormatType.DateTime;
                gridView1.Columns["Üretim Tarihi"].DisplayFormat.FormatString = "g";
                gridView1.Columns["Brüt Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Brüt Miktar"].DisplayFormat.FormatString = "n2";
                gridView1.Columns["Net Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Net Miktar"].DisplayFormat.FormatString = "n2";
            }

            int totalRollCount = 0;

            var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (filmingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("FilmingWorkOrder = ?", filmingWorkOrder));
            }
            var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (castFilmingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastFilmingWorkOrder = ?", castFilmingWorkOrder));
            }
            var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (castTransferingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastTransferingWorkOrder = ?", castTransferingWorkOrder));
            }
            var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (balloonFilmingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("BalloonFilmingWorkOrder = ?", balloonFilmingWorkOrder));
            }
            var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (printingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("PrintingWorkOrder = ?", printingWorkOrder));
            }
            var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (laminationWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("LaminationWorkOrder = ?", laminationWorkOrder));
            }
            var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (slicingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("SlicingWorkOrder = ?", slicingWorkOrder));
            }
            var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (castSlicingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastSlicingWorkOrder = ?", castSlicingWorkOrder));
            }
            var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (cuttingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CuttingWorkOrder = ?", cuttingWorkOrder));
            }
            var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (foldingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("FoldingWorkOrder = ?", foldingWorkOrder));
            }
            var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (balloonCuttingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("BalloonCuttingWorkOrder = ?", balloonCuttingWorkOrder));
            }
            var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (regeneratedWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("RegeneratedWorkOrder = ?", regeneratedWorkOrder));
            }
            var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (castRegeneratedWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastRegeneratedWorkOrder = ?", castRegeneratedWorkOrder));
            }
            var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (eco6WorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("Eco6WorkOrder = ?", eco6WorkOrder));
            }
            var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (eco6CuttingWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("Eco6CuttingWorkOrder = ?", eco6CuttingWorkOrder));
            }
            var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
            if (eco6LaminationWorkOrder != null)
            {
                totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("Eco6LaminationWorkOrder = ?", eco6LaminationWorkOrder));
            }

            txtTotalRollCount.Text = totalRollCount.ToString();

            if (chkLastProduction.Checked) chkLastProduction.Checked = false;
        }

        private void txtWorkOrderNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtWorkOrderNumber.Text.StartsWith("P"))
                {
                    if (!string.IsNullOrEmpty(cmbMachine.Text))
                    {
                        IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                        var machine = objectSpace.FindObject<Machine>(new BinaryOperator("Code", cmbMachine.Text));

                        var machineStop = objectSpace.FindObject<MachineStop>(CriteriaOperator.Parse("Machine = ? and Active = true", machine));
                        if (machineStop != null) throw new UserFriendlyException("Makine için aktif bir duruş olduğundan teyit veremezsiniz.");

                        SalesOrderDetail salesOrderDetail = null;
                        decimal workOrderQuantity = 0;
                        int rollCount = 0, totalRollCount = 0;
                        string activePaletteNumber = string.Empty;

                        var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (filmingWorkOrder != null)
                        {
                            rollCount = filmingWorkOrder.RollCount;
                            totalRollCount= (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("FilmingWorkOrder = ?", filmingWorkOrder));
                            if (machine != filmingWorkOrder.Machine)
                            {
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");
                            }

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", filmingWorkOrder.SalesOrderDetail, filmingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = filmingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = filmingWorkOrder.Quantity;
                            activePaletteNumber = filmingWorkOrder.PaletteNumber;
                        }
                        var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (castFilmingWorkOrder != null)
                        {
                            rollCount = castFilmingWorkOrder.RollCount;
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastFilmingWorkOrder = ?", castFilmingWorkOrder));
                            if (machine != castFilmingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castFilmingWorkOrder.SalesOrderDetail, castFilmingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = castFilmingWorkOrder.Quantity;
                            activePaletteNumber = castFilmingWorkOrder.PaletteNumber;
                        }
                        var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (castTransferingWorkOrder != null)
                        {
                            rollCount = castTransferingWorkOrder.RollCount;
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastTransferingWorkOrder = ?", castTransferingWorkOrder));
                            if (machine != castTransferingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castTransferingWorkOrder.SalesOrderDetail, castTransferingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = castTransferingWorkOrder.Quantity;
                            activePaletteNumber = castTransferingWorkOrder.PaletteNumber;
                        }
                        var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (balloonFilmingWorkOrder != null)
                        {
                            rollCount = balloonFilmingWorkOrder.RollCount;
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("BalloonFilmingWorkOrder = ?", balloonFilmingWorkOrder));
                            if (machine != balloonFilmingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", balloonFilmingWorkOrder.SalesOrderDetail, balloonFilmingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = balloonFilmingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = balloonFilmingWorkOrder.Quantity;
                            activePaletteNumber = balloonFilmingWorkOrder.PaletteNumber;
                        }
                        var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (printingWorkOrder != null)
                        {
                            rollCount = printingWorkOrder.RollCount;
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("PrintingWorkOrder = ?", printingWorkOrder));
                            if (machine != printingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", printingWorkOrder.SalesOrderDetail, printingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = printingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = printingWorkOrder.Quantity;
                            activePaletteNumber = printingWorkOrder.PaletteNumber;
                        }
                        var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (laminationWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("LaminationWorkOrder = ?", laminationWorkOrder));
                            if (machine != laminationWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", laminationWorkOrder.SalesOrderDetail, laminationWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = laminationWorkOrder.SalesOrderDetail;
                            workOrderQuantity = laminationWorkOrder.Quantity;
                            activePaletteNumber = laminationWorkOrder.PaletteNumber;
                        }
                        var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (slicingWorkOrder != null)
                        {
                            rollCount = slicingWorkOrder.RollCount;
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("SlicingWorkOrder = ?", slicingWorkOrder));
                            if (machine != slicingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", slicingWorkOrder.SalesOrderDetail, slicingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = slicingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = slicingWorkOrder.Quantity;
                            activePaletteNumber = slicingWorkOrder.PaletteNumber;
                        }
                        var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (castSlicingWorkOrder != null)
                        {
                            rollCount = castSlicingWorkOrder.RollCount;
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastSlicingWorkOrder = ?", castSlicingWorkOrder));
                            if (machine != castSlicingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castSlicingWorkOrder.SalesOrderDetail, castSlicingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = castSlicingWorkOrder.Quantity;
                            activePaletteNumber = castSlicingWorkOrder.PaletteNumber;
                        }
                        var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (cuttingWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CuttingWorkOrder = ?", cuttingWorkOrder));
                            if (machine != cuttingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", cuttingWorkOrder.SalesOrderDetail, cuttingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = cuttingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = cuttingWorkOrder.Quantity;
                            activePaletteNumber = cuttingWorkOrder.PaletteNumber;
                        }
                        var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (foldingWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("FoldingWorkOrder = ?", foldingWorkOrder));
                            if (machine != foldingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", foldingWorkOrder.SalesOrderDetail, foldingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = foldingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = foldingWorkOrder.Quantity;
                            activePaletteNumber = foldingWorkOrder.PaletteNumber;
                        }
                        var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (balloonCuttingWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("BalloonCuttingWorkOrder = ?", balloonCuttingWorkOrder));
                            if (machine != balloonCuttingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", balloonCuttingWorkOrder.SalesOrderDetail, balloonCuttingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = balloonCuttingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = balloonCuttingWorkOrder.Quantity;
                            activePaletteNumber = balloonCuttingWorkOrder.PaletteNumber;
                        }
                        var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (regeneratedWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("RegeneratedWorkOrder = ?", regeneratedWorkOrder));
                            if (machine != regeneratedWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", regeneratedWorkOrder.SalesOrderDetail, regeneratedWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;
                            workOrderQuantity = regeneratedWorkOrder.Quantity;
                            activePaletteNumber = regeneratedWorkOrder.PaletteNumber;
                        }
                        var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (castRegeneratedWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("CastRegeneratedWorkOrder = ?", castRegeneratedWorkOrder));
                            if (machine != castRegeneratedWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castRegeneratedWorkOrder.SalesOrderDetail, castRegeneratedWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = castRegeneratedWorkOrder.SalesOrderDetail;
                            workOrderQuantity = castRegeneratedWorkOrder.Quantity;
                            activePaletteNumber = castRegeneratedWorkOrder.PaletteNumber;
                        }
                        var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (eco6WorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("Eco6WorkOrder = ?", eco6WorkOrder));
                            if (machine != eco6WorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", eco6WorkOrder.SalesOrderDetail, eco6WorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = eco6WorkOrder.SalesOrderDetail;
                            workOrderQuantity = eco6WorkOrder.Quantity;
                            activePaletteNumber = eco6WorkOrder.PaletteNumber;
                        }
                        var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (eco6CuttingWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("Eco6CuttingWorkOrder = ?", eco6CuttingWorkOrder));
                            if (machine != eco6CuttingWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", eco6CuttingWorkOrder.SalesOrderDetail, eco6CuttingWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = eco6CuttingWorkOrder.SalesOrderDetail;
                            workOrderQuantity = eco6CuttingWorkOrder.Quantity;
                            activePaletteNumber = eco6CuttingWorkOrder.PaletteNumber;
                        }
                        var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                        if (eco6LaminationWorkOrder != null)
                        {
                            totalRollCount = (int)objectSpace.Evaluate(typeof(Production), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("Eco6LaminationWorkOrder = ?", eco6LaminationWorkOrder));
                            if (machine != eco6LaminationWorkOrder.Machine)
                                throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                            var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", eco6LaminationWorkOrder.SalesOrderDetail, eco6LaminationWorkOrder.PaletteNumber));
                            if (loading != null)
                            {
                                throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                            }

                            salesOrderDetail = eco6LaminationWorkOrder.SalesOrderDetail;
                            workOrderQuantity = eco6LaminationWorkOrder.Quantity;
                            activePaletteNumber = eco6LaminationWorkOrder.PaletteNumber;
                        }

                        if (salesOrderDetail != null)
                        {
                            txtContact.Text = salesOrderDetail.SalesOrder.Contact.Name;
                            txtWorkName.Text = salesOrderDetail.Product.Name;
                            txtWorkOrderQuantity.Text = string.Format("{0:n2}", workOrderQuantity);
                            txtActivePaletteNumber.Text = activePaletteNumber;
                            txtRollCount.Text = rollCount.ToString();
                            txtTotalRollCount.Text = totalRollCount.ToString();
                        }
                        RefreshGrid(txtWorkOrderNumber.Text);
                    }
                    else
                    {
                        cmbMachine.Focus();
                        XtraMessageBox.Show("Önce Makine seçimi yapınız...");
                    }
                }
                else
                {
                    IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                    ProductionPalette productionPalette = ((XPObjectSpace)objectSpace).Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", txtWorkOrderNumber.Text));
                    if (productionPalette != null)
                    {
                        RefreshGrid(productionPalette.WorkOrderNumber);
                    }
                }
            }
        }

        private void chkLastProduction_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLastProduction.Checked)
            {
                //IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                //ShippingPackageType shippingPackageType = null;
                //var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (filmingWorkOrder != null)
                //{
                //    shippingPackageType = filmingWorkOrder.ShippingPackageType;
                //}
                //var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (castFilmingWorkOrder != null)
                //{
                //    shippingPackageType = castFilmingWorkOrder.ShippingPackageType;
                //}
                //var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (castTransferingWorkOrder != null)
                //{
                //    shippingPackageType = castTransferingWorkOrder.ShippingPackageType;
                //}
                //var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (balloonFilmingWorkOrder != null)
                //{
                //    shippingPackageType = balloonFilmingWorkOrder.ShippingPackageType;
                //}
                //var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (printingWorkOrder != null)
                //{
                //    shippingPackageType = printingWorkOrder.ShippingPackageType;
                //}
                //var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (laminationWorkOrder != null)
                //{
                //    shippingPackageType = laminationWorkOrder.ShippingPackageType;
                //}
                //var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (slicingWorkOrder != null)
                //{
                //    shippingPackageType = slicingWorkOrder.ShippingPackageType;
                //}
                //var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (castSlicingWorkOrder != null)
                //{
                //    shippingPackageType = castSlicingWorkOrder.ShippingPackageType;
                //}
                //var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (cuttingWorkOrder != null)
                //{
                //    shippingPackageType = cuttingWorkOrder.ShippingPackageType;
                //}
                //var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (foldingWorkOrder != null)
                //{
                //    shippingPackageType = foldingWorkOrder.ShippingPackageType;
                //}
                //var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (balloonCuttingWorkOrder != null)
                //{
                //    shippingPackageType = balloonCuttingWorkOrder.ShippingPackageType;
                //}
                ////var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                ////if (regeneratedWorkOrder != null)
                ////{
                ////    shippingPackageType = regeneratedWorkOrder.ShippingPackageType;
                ////}
                ////var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                ////if (castRegeneratedWorkOrder != null)
                ////{
                ////    shippingPackageType = castRegeneratedWorkOrder.ShippingPackageType;
                ////}
                //var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (eco6WorkOrder != null)
                //{
                //    shippingPackageType = eco6WorkOrder.ShippingPackageType;
                //}
                //var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (eco6CuttingWorkOrder != null)
                //{
                //    shippingPackageType = eco6CuttingWorkOrder.ShippingPackageType;
                //}
                //var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                //if (eco6LaminationWorkOrder != null)
                //{
                //    shippingPackageType = eco6LaminationWorkOrder.ShippingPackageType;
                //}

                //if (shippingPackageType.Name.Contains("KOLİ"))
                //{
                //    layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //}
                //else layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbMachine.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                AuditTrailService.Instance.EndSessionAudit(((XPObjectSpace)objectSpace).Session);
                var machine = objectSpace.FindObject<Machine>(new BinaryOperator("Code", cmbMachine.Text));

                var machineStop = objectSpace.FindObject<MachineStop>(CriteriaOperator.Parse("Machine = ? and Active = true", machine));
                if (machineStop != null) throw new UserFriendlyException("Makine için aktif bir duruş olduğundan teyit veremezsiniz.");

                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (filmingWorkOrder != null)
                {
                    if (machine != filmingWorkOrder.Machine)
                    {
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");
                    }

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", filmingWorkOrder.SalesOrderDetail, filmingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (castFilmingWorkOrder != null)
                {
                    if (machine != castFilmingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castFilmingWorkOrder.SalesOrderDetail, castFilmingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (castTransferingWorkOrder != null)
                {
                    if (machine != castTransferingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castTransferingWorkOrder.SalesOrderDetail, castTransferingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (balloonFilmingWorkOrder != null)
                {
                    if (machine != balloonFilmingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", balloonFilmingWorkOrder.SalesOrderDetail, balloonFilmingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null)
                {
                    if (machine != printingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", printingWorkOrder.SalesOrderDetail, printingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (laminationWorkOrder != null)
                {
                    if (machine != laminationWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", laminationWorkOrder.SalesOrderDetail, laminationWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (slicingWorkOrder != null)
                {
                    if (machine != slicingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", slicingWorkOrder.SalesOrderDetail, slicingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (castSlicingWorkOrder != null)
                {
                    if (machine != castSlicingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castSlicingWorkOrder.SalesOrderDetail, castSlicingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (cuttingWorkOrder != null)
                {
                    if (machine != cuttingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", cuttingWorkOrder.SalesOrderDetail, cuttingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (foldingWorkOrder != null)
                {
                    if (machine != foldingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", foldingWorkOrder.SalesOrderDetail, foldingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (balloonCuttingWorkOrder != null)
                {
                    if (machine != balloonCuttingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", balloonCuttingWorkOrder.SalesOrderDetail, balloonCuttingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (regeneratedWorkOrder != null)
                {
                    if (machine != regeneratedWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", regeneratedWorkOrder.SalesOrderDetail, regeneratedWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (castRegeneratedWorkOrder != null)
                {
                    if (machine != castRegeneratedWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", castRegeneratedWorkOrder.SalesOrderDetail, castRegeneratedWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (eco6WorkOrder != null)
                {
                    if (machine != eco6WorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", eco6WorkOrder.SalesOrderDetail, eco6WorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (eco6CuttingWorkOrder != null)
                {
                    if (machine != eco6CuttingWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", eco6CuttingWorkOrder.SalesOrderDetail, eco6CuttingWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }
                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", txtWorkOrderNumber.Text));
                if (eco6LaminationWorkOrder != null)
                {
                    if (machine != eco6LaminationWorkOrder.Machine)
                        throw new UserFriendlyException("Bu iş emri bu makine için atanmamış. Makine seçimini kontrol ediniz.");

                    var loading = objectSpace.FindObject<DeliveryDetailLoading>(CriteriaOperator.Parse("DeliveryDetail.SalesOrderDetail = ? and PaletteNumber = ?", eco6LaminationWorkOrder.SalesOrderDetail, eco6LaminationWorkOrder.PaletteNumber));
                    if (loading != null)
                    {
                        throw new UserFriendlyException("İş emrinin aktif paleti için yükleme yapılmış yeni bir üretim paleti tanımlayınız.");
                    }
                }

                if (chkLastProduction.Checked)
                {
                    if (string.IsNullOrEmpty(txtLastQuantity.Text)) txtLastQuantity.Text = "0";
                    if (Convert.ToDecimal(txtLastQuantity.Text) <= 0)
                    {
                        txtLastQuantity.Focus();
                        throw new UserFriendlyException("Son Üretim Kaydı seçildiğinde Son Üretim Miktarı girmek zorundasınız.");
                    }
                }

                decimal grossQuantity = 0;
                if (layoutControlItem3.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                    grossQuantity = Convert.ToDecimal(txtQuantity.Text);
                else grossQuantity = Convert.ToDecimal((Helpers.axctrl as Axterazi).TERAZIOKU.ToString());

                if (txtWorkOrderNumber.Text.StartsWith("P"))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    XPQuery<ProductionPalette> palettes = ((XPObjectSpace)objectSpace).Session.Query<ProductionPalette>();
                    var paletteList = from p in palettes
                                      where p.PaletteNumber == txtWorkOrderNumber.Text
                                      select p;
                    decimal grossWeight = 0, netWeight = 0;
                    foreach (ProductionPalette item in paletteList)
                    {
                        item.LastWeight = grossQuantity;
                        grossWeight += item.GrossWeight;
                        netWeight += item.NetWeight;
                    }
                    ProductionPalette productionPalette = ((XPObjectSpace)objectSpace).Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", txtWorkOrderNumber.Text));
                    if (productionPalette != null)
                    {
                        ReportDataV2 reportData = null;
                        FilmingWorkOrder ppFilmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppFilmingWorkOrder != null) reportData = ppFilmingWorkOrder.PaletteStickerDesign;
                        CastFilmingWorkOrder ppCastFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppCastFilmingWorkOrder != null) reportData = ppCastFilmingWorkOrder.PaletteStickerDesign;
                        CastTransferingWorkOrder ppCastTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppCastTransferingWorkOrder != null) reportData = ppCastTransferingWorkOrder.PaletteStickerDesign;
                        BalloonFilmingWorkOrder ppBalloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppBalloonFilmingWorkOrder != null) reportData = ppBalloonFilmingWorkOrder.PaletteStickerDesign;
                        PrintingWorkOrder ppPrintingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppPrintingWorkOrder != null) reportData = ppPrintingWorkOrder.PaletteStickerDesign;
                        LaminationWorkOrder ppLaminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppLaminationWorkOrder != null) reportData = ppLaminationWorkOrder.PaletteStickerDesign;
                        SlicingWorkOrder ppSlicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppSlicingWorkOrder != null) reportData = ppSlicingWorkOrder.PaletteStickerDesign;
                        CastSlicingWorkOrder ppCastSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppCastSlicingWorkOrder != null) reportData = ppCastSlicingWorkOrder.PaletteStickerDesign;
                        CuttingWorkOrder ppCuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppCuttingWorkOrder != null) reportData = ppCuttingWorkOrder.PaletteStickerDesign;
                        FoldingWorkOrder ppFoldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppFoldingWorkOrder != null) reportData = ppFoldingWorkOrder.PaletteStickerDesign;
                        BalloonCuttingWorkOrder ppBalloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppBalloonCuttingWorkOrder != null) reportData = ppBalloonCuttingWorkOrder.PaletteStickerDesign;
                        RegeneratedWorkOrder ppRegeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppRegeneratedWorkOrder != null) reportData = ppRegeneratedWorkOrder.PaletteStickerDesign;
                        CastRegeneratedWorkOrder ppCastRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppCastRegeneratedWorkOrder != null) reportData = ppCastRegeneratedWorkOrder.PaletteStickerDesign;
                        Eco6WorkOrder ppEco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppEco6WorkOrder != null) reportData = ppEco6WorkOrder.PaletteStickerDesign;
                        Eco6CuttingWorkOrder ppEco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppEco6CuttingWorkOrder != null) reportData = ppEco6CuttingWorkOrder.PaletteStickerDesign;
                        Eco6LaminationWorkOrder ppEco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", productionPalette.WorkOrderNumber));
                        if (ppEco6LaminationWorkOrder != null) reportData = ppEco6LaminationWorkOrder.PaletteStickerDesign;

                        if (reportData == null) throw new UserFriendlyException("Üretim siparişinde palet etiketi seçilmemiş.");

                        productionPalette.LastWeight = grossQuantity;
                        ((XPObjectSpace)objectSpace).Session.CommitTransaction();

                        ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                        if (reportsModule != null && reportsModule.ReportsDataSourceHelper != null)
                        {
                            XtraReport report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
                            report.DataAdapter = null;
                            XPCollection<ProductionPalette> productionList = new XPCollection<ProductionPalette>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("PaletteNumber = ?", productionPalette.PaletteNumber));
                            report.DataSource = productionList;
                            report.CreateDocument(false);
                            reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report);
                            try
                            {
                                if (reportData.DisplayName.Contains("A4"))
                                {
                                    report.Print("A4");
                                    report.Print("A4");
                                }
                                else
                                {
                                    report.Print("MÜŞTERİ");
                                    report.Print("MÜŞTERİ");
                                }
                            }
                            catch { }
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    IObjectSpace objectSpaceForNumber = winApplication.CreateObjectSpace();

                    if (filmingWorkOrder != null)
                    {
                        if(filmingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (castFilmingWorkOrder != null)
                    {
                        if (castFilmingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (balloonFilmingWorkOrder != null)
                    {
                        if (balloonFilmingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (castTransferingWorkOrder != null)
                    {
                        if (castTransferingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (castSlicingWorkOrder != null)
                    {
                        if (castSlicingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (printingWorkOrder != null)
                    {
                        if (printingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (laminationWorkOrder != null)
                    {
                        if (laminationWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (slicingWorkOrder != null)
                    {
                        if (slicingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (cuttingWorkOrder != null)
                    {
                        if (cuttingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (regeneratedWorkOrder != null)
                    {
                        if (regeneratedWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (eco6WorkOrder != null)
                    {
                        if (eco6WorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (eco6LaminationWorkOrder != null)
                    {
                        if (eco6LaminationWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }
                    if (eco6CuttingWorkOrder != null)
                    {
                        if (eco6CuttingWorkOrder.SalesOrderDetail.StickerDesign.DisplayName == "36 - PURETEX  (M2) İng. Logolu Büyük Brüt-Net Etiket")
                        {
                            if ((int)Convert.ToDecimal(txtLength.Text) < 100)
                                throw new UserFriendlyException("M2 ÜRETİMDİR. LÜTFEN UZULUĞU DOĞRU GİRELİM !");
                        }
                    }

                    Cursor.Current = Cursors.WaitCursor;
                    string barcode = Helpers.GetDocumentNumber(((XPObjectSpace)objectSpace).Session, "EcoplastERP.Module.BusinessObjects.ProductionObjects.Production");
                    objectSpaceForNumber.CommitChanges();

                    Production production = objectSpace.CreateObject<Production>();
                    production.WorkOrderNumber = txtWorkOrderNumber.Text;
                    production.Barcode = barcode;
                    production.GrossQuantity = grossQuantity;
                    production.LastQuantity = Convert.ToDecimal(txtLastQuantity.Text);
                    production.Length = (int)Convert.ToDecimal(txtLength.Text);
                    production.RollDiameter = (int)Convert.ToDecimal(txtRollDiameter.Text);
                    production.IsLastRoll = chkLastProduction.Checked;

                    objectSpace.CommitChanges();

                    //Üretim kaynakları
                    IObjectSpace mObjectSpace = winApplication.CreateObjectSpace();
                    if (printingWorkOrder != null || cuttingWorkOrder != null)
                    {
                        Production findedProduction = mObjectSpace.FindObject<Production>(new BinaryOperator("Barcode", barcode));
                        XPCollection<ReadResourceMovement> rrmovementList = new XPCollection<ReadResourceMovement>(((XPObjectSpace)mObjectSpace).Session, CriteriaOperator.Parse("ProductionBarcode = ?", barcode));
                        foreach (ReadResourceMovement item in rrmovementList)
                        {
                            ProductionResource productionResource = mObjectSpace.CreateObject<ProductionResource>();
                            productionResource.Production = findedProduction;
                            productionResource.ReadResourceProduction = item.ReadResource.Production;
                            productionResource.ReadResourceMovement = item;
                            findedProduction.ProductionResources.Add(productionResource);

                            XPCollection<ReadResourceMovement> subMovementList = new XPCollection<ReadResourceMovement>(((XPObjectSpace)mObjectSpace).Session, CriteriaOperator.Parse("ProductionBarcode = ?", item.ReadResource.Barcode));
                            foreach (ReadResourceMovement subitem in subMovementList)
                            {
                                ProductionResource subProductionResource = mObjectSpace.CreateObject<ProductionResource>();
                                subProductionResource.Production = findedProduction;
                                subProductionResource.ReadResourceProduction = subitem.ReadResource.Production;
                                subProductionResource.ReadResourceMovement = subitem;
                                findedProduction.ProductionResources.Add(subProductionResource);
                            }
                        }
                    }
                    mObjectSpace.CommitChanges();

                    bool isLastStation = false;
                    ReportDataV2 reportData = null;
                    if (filmingWorkOrder != null)
                    {
                        isLastStation = filmingWorkOrder.NextStation.IsLastStation;
                        if (filmingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.FilmingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (castFilmingWorkOrder != null)
                    {
                        isLastStation = castFilmingWorkOrder.NextStation.IsLastStation;
                        if (castFilmingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastFilmingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (castTransferingWorkOrder != null)
                    {
                        isLastStation = castTransferingWorkOrder.NextStation.IsLastStation;
                        if (castTransferingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastTransferingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (balloonFilmingWorkOrder != null)
                    {
                        isLastStation = balloonFilmingWorkOrder.NextStation.IsLastStation;
                        if (balloonFilmingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.BalloonFilmingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (printingWorkOrder != null)
                    {
                        isLastStation = printingWorkOrder.NextStation.IsLastStation;
                        if (printingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.PrintingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (laminationWorkOrder != null)
                    {
                        isLastStation = laminationWorkOrder.NextStation.IsLastStation;
                        if (laminationWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.LaminationWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (slicingWorkOrder != null)
                    {
                        isLastStation = slicingWorkOrder.NextStation.IsLastStation;
                        if (slicingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.SlicingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (castSlicingWorkOrder != null)
                    {
                        isLastStation = castSlicingWorkOrder.NextStation.IsLastStation;
                        if (castSlicingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastSlicingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (cuttingWorkOrder != null)
                    {
                        isLastStation = cuttingWorkOrder.NextStation.IsLastStation;
                        if (cuttingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CuttingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (foldingWorkOrder != null)
                    {
                        isLastStation = foldingWorkOrder.NextStation.IsLastStation;
                        if (foldingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.FoldingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (balloonCuttingWorkOrder != null)
                    {
                        isLastStation = balloonCuttingWorkOrder.NextStation.IsLastStation;
                        if (balloonCuttingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.BalloonCuttingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (regeneratedWorkOrder != null)
                    {
                        isLastStation = regeneratedWorkOrder.NextStation.IsLastStation;
                        if (regeneratedWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.RegeneratedWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (castRegeneratedWorkOrder != null)
                    {
                        isLastStation = castRegeneratedWorkOrder.NextStation.IsLastStation;
                        if (castRegeneratedWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastRegeneratedWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (eco6WorkOrder != null)
                    {
                        isLastStation = eco6WorkOrder.NextStation.IsLastStation;
                        if (eco6WorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.Eco6WorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (eco6CuttingWorkOrder != null)
                    {
                        isLastStation = eco6CuttingWorkOrder.NextStation.IsLastStation;
                        if (eco6CuttingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.Eco6CuttingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    if (eco6LaminationWorkOrder != null)
                    {
                        isLastStation = eco6LaminationWorkOrder.NextStation.IsLastStation;
                        if (eco6LaminationWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.Eco6LaminationWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                        else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                    }
                    RefreshGrid(txtWorkOrderNumber.Text);

                    ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                    if (reportsModule != null && reportsModule.ReportsDataSourceHelper != null)
                    {
                        XtraReport report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
                        report.DataAdapter = null;
                        XPCollection<Production> productionList = new XPCollection<Production>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Barcode = ?", production.Barcode));
                        report.DataSource = productionList;
                        report.CreateDocument(false);
                        reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report);
                        try
                        {
                            if (reportData.DisplayName.Contains("Arce"))
                            {
                                report.Print("İŞLETME");
                            }
                            else if (reportData.DisplayName.Contains("Küçük"))
                            {
                                report.Print("İŞLETME");
                                if (!isLastStation)
                                {
                                    report.Print("İŞLETME");
                                }
                            }
                            else if (reportData.DisplayName.Contains("Büyük"))
                            {
                                if (reportData.DisplayName.Contains("A4")) report.Print("A4");
                                else report.Print("MÜŞTERİ");

                                //ReportDataV2 littleSticker = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                                //if (littleSticker != null)
                                //{
                                //    XtraReport littleStickerReport = ReportDataProvider.ReportsStorage.LoadReport(littleSticker);
                                //    littleStickerReport.DataAdapter = null;
                                //    littleStickerReport.DataSource = productionList;
                                //    littleStickerReport.CreateDocument(false);
                                //    reportsModule.ReportsDataSourceHelper.SetupBeforePrint(littleStickerReport);
                                //    littleStickerReport.Print("İŞLETME");
                                //}
                            }
                        }
                        catch { }
                    }
                    Cursor.Current = Cursors.Default;
                }
                
                if (layoutControlItem4.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always) txtQuantity.Text = "0,00";
            }
            else
            {
                cmbMachine.Focus();
                XtraMessageBox.Show("Önce Makine seçimi yapınız...");
            }
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            //if (txtWorkOrderNumber.Text.StartsWith("P"))
            //{
            //    var productionPalette = objectSpace.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", txtWorkOrderNumber.Text));
            //    if (productionPalette != null)
            //    {
            //        if (productionPalette.LastWeight > 0)
            //        {
            //            ReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "50 - Standart Palet Dökümü"));
            //            ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
            //            if (reportsModule != null && reportsModule.ReportsDataSourceHelper != null)
            //            {
            //                XtraReport report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
            //                report.DataAdapter = null;
            //                XPCollection<ProductionPalette> productionList = new XPCollection<ProductionPalette>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("PaletteNumber = ?", productionPalette.PaletteNumber));
            //                report.DataSource = productionList;
            //                report.CreateDocument(false);
            //                reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report);
            //                try
            //                {
            //                    if (reportData.DisplayName.Contains("A4"))
            //                    {
            //                        report.Print("A4");
            //                        report.Print("A4");
            //                    }
            //                    else
            //                    {
            //                        report.Print("MÜŞTERİ");
            //                        report.Print("MÜŞTERİ");
            //                    }
            //                }
            //                catch { }
            //            }
            //        }
            //        else
            //        {
            //            throw new UserFriendlyException(@"Palet teyidi verilmemiş. Palet etiketi yazdıramazsınız.");
            //        }
            //    }
            //}
            //else
            //{
            var production = objectSpace.FindObject<Production>(new BinaryOperator("Barcode", gridView1.GetFocusedRowCellValue("Barkod").ToString()));
            if (production != null)
            {
                ReportDataV2 reportData = null;
                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (filmingWorkOrder != null)
                {
                    if (filmingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.FilmingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (castFilmingWorkOrder != null)
                {
                    if (castFilmingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastFilmingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (castTransferingWorkOrder != null)
                {
                    if (castTransferingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastTransferingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (balloonFilmingWorkOrder != null)
                {
                    if (balloonFilmingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.BalloonFilmingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (printingWorkOrder != null)
                {
                    if (printingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.PrintingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (laminationWorkOrder != null)
                {
                    if (laminationWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.LaminationWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (slicingWorkOrder != null)
                {
                    if (slicingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.SlicingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (castSlicingWorkOrder != null)
                {
                    if (castSlicingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastSlicingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (cuttingWorkOrder != null)
                {
                    if (cuttingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CuttingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (foldingWorkOrder != null)
                {
                    if (foldingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.FoldingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (balloonCuttingWorkOrder != null)
                {
                    if (balloonCuttingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.BalloonCuttingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (regeneratedWorkOrder != null)
                {
                    if (regeneratedWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.RegeneratedWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (castRegeneratedWorkOrder != null)
                {
                    if (castRegeneratedWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.CastRegeneratedWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (eco6WorkOrder != null)
                {
                    if (eco6WorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.Eco6WorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (eco6CuttingWorkOrder != null)
                {
                    if (eco6CuttingWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.Eco6CuttingWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", production.WorkOrderNumber));
                if (eco6LaminationWorkOrder != null)
                {
                    if (eco6LaminationWorkOrder.NextStation.IsLastStation) reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("Oid", production.Eco6LaminationWorkOrder.SalesOrderDetail.StickerDesign.Oid));
                    else reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                }

                ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(winApplication.Modules);
                if (reportsModule != null && reportsModule.ReportsDataSourceHelper != null)
                {
                    XtraReport report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
                    report.DataAdapter = null;
                    XPCollection<Production> productionList = new XPCollection<Production>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("Barcode = ?", production.Barcode));
                    report.DataSource = productionList;
                    report.CreateDocument(false);
                    reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report);
                    try
                    {
                        if (reportData.DisplayName.Contains("Küçük"))
                        {
                            report.Print("İŞLETME");
                            report.Print("İŞLETME");
                        }
                        else if (reportData.DisplayName.Contains("Büyük"))
                        {
                            if (reportData.DisplayName.Contains("A4")) report.Print("A4");
                            else report.Print("MÜŞTERİ");

                            ReportDataV2 littleSticker = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "01 - Standart Küçük Brüt Etiket"));
                            if (littleSticker != null)
                            {
                                XtraReport littleStickerReport = ReportDataProvider.ReportsStorage.LoadReport(littleSticker);
                                littleStickerReport.DataAdapter = null;
                                littleStickerReport.DataSource = productionList;
                                littleStickerReport.CreateDocument(false);
                                reportsModule.ReportsDataSourceHelper.SetupBeforePrint(littleStickerReport);
                                littleStickerReport.Print("İŞLETME");
                            }
                        }
                    }
                    catch { }
                }
            }
            //}
        }

        private void btnReadResource_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();

                ReadResourceForm form = new ReadResourceForm()
                {
                    objectSpace = objectSpace
                };
                form.txtWorkOrderNumber.Text = this.txtWorkOrderNumber.Text;
                form.ShowDialog();
            }
            else throw new UserFriendlyException("Üretim sipariş numarası girilmemiş...");
        }

        private void btnChangePalette_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string barcode = gridView1.FocusedValue != null ? gridView1.GetFocusedRowCellValue("Barkod").ToString() : string.Empty;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            var obj = objectSpace.CreateObject<ChangeProductionPaletteParameters>();
            obj.Barcode = barcode;
            ShowViewParameters svp = new ShowViewParameters()
            {
                CreatedView = winApplication.CreateDetailView(objectSpace, obj),
                CreateAllControllers = true,
                TargetWindow = TargetWindow.NewModalWindow
            };
            winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        private void btnCancelPaletteComplete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                if (txtWorkOrderNumber.Text.StartsWith("P"))
                {
                    IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                    Store store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("PaletteNumber = ? and Warehouse.LoadingWarehouse = true", txtWorkOrderNumber.Text));
                    if(store != null)
                    {
                        throw new UserFriendlyException("Sevkiyat okutma yapılmış paletin teyit iptali yapılamaz.");
                    }

                    XPCollection<ProductionPalette> productionPaletteList = new XPCollection<ProductionPalette>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("PaletteNumber = ?", txtWorkOrderNumber.Text));
                    foreach (ProductionPalette item in productionPaletteList)
                    {
                        item.LastWeight = 0;
                        item.GrossWeight = 0;
                        item.NetWeight = 0;
                        item.ConsumeMaterialWeight = 0;
                    }

                    objectSpace.CommitChanges();
                    XtraMessageBox.Show("İşlem tamamlandı...");
                }
            }
        }

        private void btnWorkOrderDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                DetailView detailView = null;
                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (filmingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, filmingWorkOrder);
                }

                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castFilmingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, castFilmingWorkOrder);
                }

                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castTransferingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, castTransferingWorkOrder);
                }

                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonFilmingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, balloonFilmingWorkOrder);
                }

                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, printingWorkOrder);
                }

                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (laminationWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, laminationWorkOrder);
                }

                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (slicingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, slicingWorkOrder);
                }

                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castSlicingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, castSlicingWorkOrder);
                }

                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (cuttingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, cuttingWorkOrder);
                }

                var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (foldingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, foldingWorkOrder);
                }

                var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonCuttingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, balloonCuttingWorkOrder);
                }

                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (regeneratedWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, regeneratedWorkOrder);
                }

                var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castRegeneratedWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, castRegeneratedWorkOrder);
                }

                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6WorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, eco6WorkOrder);
                }

                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6CuttingWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, eco6CuttingWorkOrder);
                }

                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6LaminationWorkOrder != null)
                {
                    detailView = winApplication.CreateDetailView(objectSpace, eco6LaminationWorkOrder);
                }

                detailView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters svp = new ShowViewParameters()
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    Context = TemplateContext.PopupWindow,
                    CreatedView = detailView
                };
                DialogController dialogController = winApplication.CreateController<DialogController>();
                svp.Controllers.Add(dialogController);
                winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
            else
            {
                throw new UserFriendlyException("İş Emri Numarası giriniz.");
            }
        }

        private void btnProcessForm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();

                //DevExpress.ExpressApp.ListView listView = null;
                //PrintingWorkOrder printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                //if (printingWorkOrder != null)
                //{
                //    string listViewId = winApplication.FindListViewId(typeof(PrintingWorkOrderProcess));
                //    CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(PrintingWorkOrderProcess));
                //    collectionSource.Criteria["filter"] = new BinaryOperator("PrintingWorkOrder", printingWorkOrder);
                //    listView = winApplication.CreateListView(listViewId, winApplication.CreateCollectionSource(objectSpace, typeof(PrintingWorkOrderProcess), listViewId), true);
                //}
                //Form form = new Form();
                //Frame frame = winApplication.CreateFrame(TemplateContext.NestedFrame);
                //frame.CreateTemplate();
                //frame.SetView(listView);
                //form.Controls.Add((Control)frame.Template);
                //form.ShowDialog();

                DetailView detailView = null;
                PrintingWorkOrder printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null)
                {
                    PrintingWorkOrderProcess printingWorkOrderProcessForm = objectSpace.CreateObject<PrintingWorkOrderProcess>();
                    printingWorkOrderProcessForm.PrintingWorkOrder = printingWorkOrder;
                    detailView = winApplication.CreateDetailView(objectSpace, printingWorkOrderProcessForm);
                }

                if (detailView != null)
                {
                    detailView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters svp = new ShowViewParameters()
                    {
                        TargetWindow = TargetWindow.NewModalWindow,
                        Context = TemplateContext.PopupWindow,
                        CreatedView = detailView
                    };
                    DialogController dialogController = winApplication.CreateController<DialogController>();
                    svp.Controllers.Add(dialogController);
                    winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                }
            }
            else
            {
                throw new UserFriendlyException("İş Emri Numarası giriniz.");
            }
        }

        private void btnChangeOption_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                var obj = objectSpace.CreateObject<ChangeWorkOrderProductionOptionParameters>();
                obj.WorkOrderNumber = txtWorkOrderNumber.Text;
                ShowViewParameters svp = new ShowViewParameters()
                {
                    CreatedView = winApplication.CreateDetailView(objectSpace, obj),
                    CreateAllControllers = true,
                    TargetWindow = TargetWindow.NewModalWindow
                };
                winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
            else throw new UserFriendlyException("Üretim sipariş numarası girilmemiş...");
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.GetFocusedRow() == null) return;
            if (XtraMessageBox.Show("Kaydı silmek istiyor musunuz?", "Onaylana", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                AuditTrailService.Instance.EndSessionAudit(((XPObjectSpace)objectSpace).Session);
                var production = objectSpace.FindObject<Production>(CriteriaOperator.Parse("Barcode = ?", gridView1.GetFocusedRowCellValue("Barkod")));
                if (production != null)
                {
                    if (production.ProductionPalette != null)
                    {
                        if (production.ProductionPalette.LastWeight > 0)
                            throw new UserFriendlyException("Palet teyidi verilmiş barkod silinemez.");
                    }
                    #region stok silme hareketi
                    //var store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", production.Barcode));
                    //if (store != null)
                    //{
                    //    //Yeni bir malzeme başlık ID alınır.
                    //    var headerId = Guid.NewGuid();
                    //    #region Üretime Çıkış İptali
                    //    //Çıkış hareket türü
                    //    var input = objectSpace.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P114'"));
                    //    if (production.FilmingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.FilmingWorkOrder.FilmingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.FilmingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    if (production.CastFilmingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.CastFilmingWorkOrder.CastFilmingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.CastFilmingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    if (production.CastTransferingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.CastTransferingWorkOrder.CastTransferingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.CastTransferingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    if (production.BalloonFilmingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.BalloonFilmingWorkOrder.BalloonFilmingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.BalloonFilmingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.PrintingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.PrintingWorkOrder.PrintingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.PrintingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.LaminationWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.LaminationWorkOrder.LaminationWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.LaminationWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.SlicingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.SlicingWorkOrder.SlicingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.SlicingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.CastSlicingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.CastSlicingWorkOrder.CastSlicingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.CastSlicingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.CuttingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.CuttingWorkOrder.CuttingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.CuttingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.FoldingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.FoldingWorkOrder.FoldingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.FoldingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.BalloonCuttingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.BalloonCuttingWorkOrder.BalloonCuttingWorkOrderReciept;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.BalloonCuttingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.RegeneratedWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.RegeneratedWorkOrder.RegeneratedWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.RegeneratedWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.CastRegeneratedWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.CastRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.CastRegeneratedWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.Eco6WorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.Eco6WorkOrder.Eco6WorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.Eco6WorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.Eco6CuttingWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.Eco6CuttingWorkOrder.Eco6CuttingWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.Eco6CuttingWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    else if (production.Eco6LaminationWorkOrder != null)
                    //    {
                    //        //İş emrinde girilen malzeme ihtiyaçları alınır.
                    //        var reciept = production.Eco6LaminationWorkOrder.Eco6LaminationWorkOrderReciepts;
                    //        if (reciept != null) //Malzeme ihtiyaçları girilmişse depoya giriş hareketleri yapılır.
                    //        {
                    //            foreach (var item in reciept)
                    //            {
                    //                var omovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //                {
                    //                    HeaderId = headerId,
                    //                    DocumentNumber = production.WorkOrderNumber,
                    //                    DocumentDate = DateTime.Now,
                    //                    Barcode = "",
                    //                    Product = item.Product,
                    //                    PartyNumber = "",
                    //                    PaletteNumber = "",
                    //                    Warehouse = production.Eco6LaminationWorkOrder.Station.SourceWarehouse,
                    //                    MovementType = input,
                    //                    Unit = item.Unit,
                    //                    Quantity = production.GrossQuantity * (decimal)item.Rate / 100
                    //                };
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //    #region Üretim İptali Hareketi
                    //    //Üretilen ürünün iptal hareketi yapılır.
                    //    var output = objectSpace.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P115'"));
                    //    var imovement = new Movement(((XPObjectSpace)objectSpace).Session)
                    //    {
                    //        HeaderId = headerId,
                    //        DocumentNumber = production.WorkOrderNumber,
                    //        DocumentDate = DateTime.Now,
                    //        Barcode = store.Barcode,
                    //        SalesOrderDetail = production.SalesOrderDetail,
                    //        Product = production.SalesOrderDetail.Product,
                    //        PartyNumber = store.PartyNumber,
                    //        PaletteNumber = store.PaletteNumber,
                    //        Warehouse = store.Warehouse,
                    //        MovementType = output,
                    //        Unit = store.Unit,
                    //        Quantity = store.Quantity,
                    //        cUnit = store.cUnit,
                    //        cQuantity = store.cQuantity
                    //    };
                    //    #endregion
                    //}
                    #endregion

                    XPCollection<ReadResourceMovement> rrmList = new XPCollection<ReadResourceMovement>(((XPObjectSpace)objectSpace).Session, CriteriaOperator.Parse("ProductionBarcode = ?", production.Barcode));
                    foreach (ReadResourceMovement item in rrmList)
                    {
                        item.ReadResource.IsConsume = false;
                    }
                    objectSpace.Delete(rrmList);

                    objectSpace.Delete(production);

                    objectSpace.CommitChanges();
                    //objectSpace.CommitChanges();
                    //((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(String.Format("update Production set OptimisticLockField = 1, GCRecord = 1 where Barcode = '{0}'   update Store set OptimisticLockField = 1, GCRecord = 1 where Barcode = '{0}'", production.Barcode));
                }

                RefreshGrid(txtWorkOrderNumber.Text);
                if (txtQuantity.Visible == true) txtQuantity.Text = "0,00";
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnNewPalette_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            ProductionPalette obj = objectSpace.CreateObject<ProductionPalette>();
            obj.WorkOrderNumber = txtWorkOrderNumber.Text;
            ShowViewParameters svp = new ShowViewParameters()
            {
                CreatedView = winApplication.CreateDetailView(objectSpace, obj),
                CreateAllControllers = true,
                TargetWindow = TargetWindow.NewModalWindow
            };
            winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        private void btnActivatePalette_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                if (txtWorkOrderNumber.Text.StartsWith("P"))
                {
                    IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                    ProductionPalette productionPalette = objectSpace.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", txtWorkOrderNumber.Text));
                    if (productionPalette != null)
                    {
                        productionPalette.Activate();
                        objectSpace.CommitChanges();
                        XtraMessageBox.Show("İşlem tamamlandı...");
                    }
                    else throw new UserFriendlyException("Palet tanımlı değil.");
                }
            }
            else XtraMessageBox.Show("Palet numarası girilmemiş...");
        }

        private void btnSendQuarantine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            if (XtraMessageBox.Show("Seçili barkodu karantinaya göndermek istiyor musunuz?", "Onaylana", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                Warehouse quarantineWarehouse = null;
                string barcode = gridView1.GetFocusedRowCellValue("Barkod").ToString();
                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (filmingWorkOrder != null)
                {
                    quarantineWarehouse = filmingWorkOrder.Station.QuarantineWarehouse;
                }

                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castFilmingWorkOrder != null)
                {
                    quarantineWarehouse = castFilmingWorkOrder.Station.QuarantineWarehouse;
                }

                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castTransferingWorkOrder != null)
                {
                    quarantineWarehouse = castTransferingWorkOrder.Station.QuarantineWarehouse;
                }

                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonFilmingWorkOrder != null)
                {
                    quarantineWarehouse = balloonFilmingWorkOrder.Station.QuarantineWarehouse;
                }

                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null)
                {
                    quarantineWarehouse = printingWorkOrder.Station.QuarantineWarehouse;
                }

                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (laminationWorkOrder != null)
                {
                    quarantineWarehouse = laminationWorkOrder.Station.QuarantineWarehouse;
                }

                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (slicingWorkOrder != null)
                {
                    quarantineWarehouse = slicingWorkOrder.Station.QuarantineWarehouse;
                }

                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castSlicingWorkOrder != null)
                {
                    quarantineWarehouse = castSlicingWorkOrder.Station.QuarantineWarehouse;
                }

                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (cuttingWorkOrder != null)
                {
                    quarantineWarehouse = cuttingWorkOrder.Station.QuarantineWarehouse;
                }

                var foldingWorkOrder = objectSpace.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (foldingWorkOrder != null)
                {
                    quarantineWarehouse = foldingWorkOrder.Station.QuarantineWarehouse;
                }

                var balloonCuttingWorkOrder = objectSpace.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonCuttingWorkOrder != null)
                {
                    quarantineWarehouse = balloonCuttingWorkOrder.Station.QuarantineWarehouse;
                }

                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (regeneratedWorkOrder != null)
                {
                    quarantineWarehouse = regeneratedWorkOrder.Station.QuarantineWarehouse;
                }

                var castRegeneratedWorkOrder = objectSpace.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castRegeneratedWorkOrder != null)
                {
                    quarantineWarehouse = castRegeneratedWorkOrder.Station.QuarantineWarehouse;
                }

                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6WorkOrder != null)
                {
                    quarantineWarehouse = eco6WorkOrder.Station.QuarantineWarehouse;
                }

                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6CuttingWorkOrder != null)
                {
                    quarantineWarehouse = eco6CuttingWorkOrder.Station.QuarantineWarehouse;
                }

                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6LaminationWorkOrder != null)
                {
                    quarantineWarehouse = eco6LaminationWorkOrder.Station.QuarantineWarehouse;
                }

                Store store = objectSpace.FindObject<Store>(new BinaryOperator("Barcode", barcode));
                if (store != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Guid headerId = Guid.NewGuid();
                    MovementType input = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P120"));
                    MovementType output = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P121"));
                    Movement outputMovement = objectSpace.CreateObject<Movement>();
                    outputMovement.HeaderId = headerId;
                    outputMovement.DocumentNumber = string.Empty;
                    outputMovement.DocumentDate = DateTime.Now;
                    outputMovement.Barcode = store.Barcode;
                    outputMovement.SalesOrderDetail = store.SalesOrderDetail;
                    outputMovement.Product = store.Product;
                    outputMovement.PartyNumber = store.PartyNumber;
                    outputMovement.PaletteNumber = store.PaletteNumber;
                    outputMovement.Warehouse = store.Warehouse;
                    outputMovement.MovementType = output;
                    outputMovement.Unit = store.Unit;
                    outputMovement.Quantity = store.Quantity;
                    outputMovement.cUnit = store.cUnit;
                    outputMovement.cQuantity = store.cQuantity;

                    Movement inputMovement = objectSpace.CreateObject<Movement>();
                    inputMovement.HeaderId = headerId;
                    inputMovement.DocumentNumber = string.Empty;
                    inputMovement.DocumentDate = DateTime.Now;
                    inputMovement.Barcode = store.Barcode;
                    inputMovement.SalesOrderDetail = store.SalesOrderDetail;
                    inputMovement.Product = store.Product;
                    inputMovement.PartyNumber = store.PartyNumber;
                    inputMovement.PaletteNumber = store.PaletteNumber;
                    inputMovement.Warehouse = quarantineWarehouse;
                    inputMovement.MovementType = input;
                    inputMovement.Unit = store.Unit;
                    inputMovement.Quantity = store.Quantity;
                    inputMovement.cUnit = store.cUnit;
                    inputMovement.cQuantity = store.cQuantity;

                    Store newStore = objectSpace.CreateObject<Store>();
                    newStore.Product = store.Product;
                    newStore.Barcode = store.Barcode;
                    newStore.SalesOrderDetail = store.SalesOrderDetail;
                    newStore.PartyNumber = store.PartyNumber;
                    newStore.PaletteNumber = store.PaletteNumber;
                    newStore.Warehouse = quarantineWarehouse;
                    newStore.WarehouseCell = null;
                    newStore.Unit = store.Unit;
                    newStore.Quantity = store.Quantity;
                    newStore.cUnit = store.cUnit;
                    newStore.cQuantity = store.cQuantity;

                    objectSpace.CommitChanges();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    throw new UserFriendlyException("Barkod depoda bulunamadı.");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            ShiftStart shiftStart = objectSpace.FindObject<ShiftStart>(new BinaryOperator("Active", true));
            if (shiftStart != null)
            {
                double hour = (DateTime.Now - shiftStart.StartTime).TotalHours;
                if (hour >= 8) XtraMessageBox.Show("Önceki vardiya çalışma süresini tamamladı. Yeni vardiya başlatmanız gerekebilir.");
            }
        }

        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (filmingWorkOrder != null)
                {
                    if (filmingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) filmingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castFilmingWorkOrder != null)
                {
                    if (castFilmingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) castFilmingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castTransferingWorkOrder != null)
                {
                    if (castTransferingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) castTransferingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonFilmingWorkOrder != null)
                {
                    if (balloonFilmingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) balloonFilmingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null)
                {
                    if (printingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) printingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (laminationWorkOrder != null)
                {
                    if (laminationWorkOrder.BeginDate < new DateTime(2010, 1, 1)) laminationWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (slicingWorkOrder != null)
                {
                    if (slicingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) slicingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castSlicingWorkOrder != null)
                {
                    if (castSlicingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) castSlicingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (cuttingWorkOrder != null)
                {
                    if (cuttingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) cuttingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (regeneratedWorkOrder != null)
                {
                    if (regeneratedWorkOrder.BeginDate < new DateTime(2010, 1, 1)) regeneratedWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6WorkOrder != null)
                {
                    if (eco6WorkOrder.BeginDate < new DateTime(2010, 1, 1)) eco6WorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6CuttingWorkOrder != null)
                {
                    if (eco6CuttingWorkOrder.BeginDate < new DateTime(2010, 1, 1)) eco6CuttingWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6LaminationWorkOrder != null)
                {
                    if (eco6LaminationWorkOrder.BeginDate < new DateTime(2010, 1, 1)) eco6LaminationWorkOrder.BeginDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten başlatılmış.");
                }

                objectSpace.CommitChanges();
            }
            else throw new UserFriendlyException("İş emri numarası giriniz.");
        }

        private void btnFinish_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkOrderNumber.Text))
            {
                IObjectSpace objectSpace = winApplication.CreateObjectSpace();
                var filmingWorkOrder = objectSpace.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (filmingWorkOrder != null)
                {
                    if (filmingWorkOrder.EndDate < new DateTime(2010, 1, 1)) filmingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var castFilmingWorkOrder = objectSpace.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castFilmingWorkOrder != null)
                {
                    if (castFilmingWorkOrder.EndDate < new DateTime(2010, 1, 1)) castFilmingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var castTransferingWorkOrder = objectSpace.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castTransferingWorkOrder != null)
                {
                    if (castTransferingWorkOrder.EndDate < new DateTime(2010, 1, 1)) castTransferingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var balloonFilmingWorkOrder = objectSpace.FindObject<BalloonFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (balloonFilmingWorkOrder != null)
                {
                    if (balloonFilmingWorkOrder.EndDate < new DateTime(2010, 1, 1)) balloonFilmingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var printingWorkOrder = objectSpace.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (printingWorkOrder != null)
                {
                    if (printingWorkOrder.EndDate < new DateTime(2010, 1, 1)) printingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var laminationWorkOrder = objectSpace.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (laminationWorkOrder != null)
                {
                    if (laminationWorkOrder.EndDate < new DateTime(2010, 1, 1)) laminationWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var slicingWorkOrder = objectSpace.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (slicingWorkOrder != null)
                {
                    if (slicingWorkOrder.EndDate < new DateTime(2010, 1, 1)) slicingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var castSlicingWorkOrder = objectSpace.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (castSlicingWorkOrder != null)
                {
                    if (castSlicingWorkOrder.EndDate < new DateTime(2010, 1, 1)) castSlicingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var cuttingWorkOrder = objectSpace.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (cuttingWorkOrder != null)
                {
                    if (cuttingWorkOrder.EndDate < new DateTime(2010, 1, 1)) cuttingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var regeneratedWorkOrder = objectSpace.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (regeneratedWorkOrder != null)
                {
                    if (regeneratedWorkOrder.EndDate < new DateTime(2010, 1, 1)) regeneratedWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var eco6WorkOrder = objectSpace.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6WorkOrder != null)
                {
                    if (eco6WorkOrder.EndDate < new DateTime(2010, 1, 1)) eco6WorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var eco6CuttingWorkOrder = objectSpace.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6CuttingWorkOrder != null)
                {
                    if (eco6CuttingWorkOrder.EndDate < new DateTime(2010, 1, 1)) eco6CuttingWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                var eco6LaminationWorkOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", txtWorkOrderNumber.Text));
                if (eco6LaminationWorkOrder != null)
                {
                    if (eco6LaminationWorkOrder.EndDate < new DateTime(2010, 1, 1)) eco6LaminationWorkOrder.EndDate = DateTime.Now;
                    else XtraMessageBox.Show("Bu iş zaten sonlandırılmış.");
                }

                objectSpace.CommitChanges();
            }
            else throw new UserFriendlyException("İş emri numarası giriniz.");
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string karantina = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Karantina"]);
                if (karantina == "1")
                {
                    e.Appearance.BackColor = Color.PowderBlue;
                }
            }
        }
    }
}
