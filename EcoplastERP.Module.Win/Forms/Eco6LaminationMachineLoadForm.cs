using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.XtraReports.UI;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.BaseImpl;
using EcoplastERP.Module.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ParametersObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.MaintenanceObjects;

namespace EcoplastERP.Module.Win.Forms
{
    public partial class Eco6LaminationMachineLoadForm : XtraForm, IXpoSessionAwareControl
    {
        public XafApplication winApplication;

        public Eco6LaminationMachineLoadForm()
        {
            InitializeComponent();
        }

        public void UpdateDataSource(XafApplication xafWinApplication)
        {
            winApplication = xafWinApplication;
        }

        private void Eco2MachineLoadForm_Load(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select M.Code from Machine M inner join Station S on S.Oid = M.Station where M.GCRecord is null and S.Code = 'Eco6 Laminasyon' order by M.Code", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                cmbMachine.Properties.Items.Add(row["Code"]);
            }
            RefreshGrid("xxx");

            if (gridView1.Columns["Durumu"] != null)
                gridView1.Columns["Durumu"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Durumu", "{0:n0}");
            if (gridView1.Columns["Sipariş Miktarı"] != null)
                gridView1.Columns["Sipariş Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Üretim Siparişi Miktarı", "{0:n2}");
            if (gridView1.Columns["Üretim Siparişi Miktarı"] != null)
                gridView1.Columns["Üretim Siparişi Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Üretim Siparişi Miktarı", "{0:n2}");
            if (gridView1.Columns["Toplam Üretim"] != null)
                gridView1.Columns["Toplam Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Toplam Üretim", "{0:n2}");
            if (gridView1.Columns["Kalan Üretim"] != null)
                gridView1.Columns["Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Kalan Üretim", "{0:n2}");
            if (gridView1.Columns["Kalan Süre (saat)"] != null)
                gridView1.Columns["Kalan Süre (saat)"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Kalan Süre (saat)", "{0:n2}");
            if (gridView1.Columns["Sipariş Üretilen"] != null)
                gridView1.Columns["Sipariş Üretilen"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sipariş Üretilen", "{0:n2}");

            FillItemWithEnumValues(bsiChangeStatus, typeof(WorkOrderStatus));
        }

        private void Eco2MachineLoadForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        void RefreshGrid(string machineCode)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            DataTable dt = new DataTable();
            string sqlText = string.Format(@"select (case when W.WorkOrderStatus = 100 then 'Üretim Bekliyor' when W.WorkOrderStatus = 101 then 'Aktif' when W.WorkOrderStatus = 102 then 'Üretim Durduruldu' when W.WorkOrderStatus = 110 then 'Üretim Tamamlandı' when W.WorkOrderStatus = 900 then 'İptal Edildi' else 'NULL' end) as Durumu, W.SequenceNumber as [Sıra No], C.Name as [Müşteri], M.Code as [Stok Kodu], W.WorkName as [İşin Adı], W.Width as [En], W.Height as [Boy], W.Thickness as [Kalınlık], M.Cap as [Kapak], W.WorkOrderNumber as [Üretim Siparişi No], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], O.OrderDate as [Sipariş Tarihi], D.LineDeliveryDate as [Termin Tarihi], D.Quantity as [Sipariş Miktarı], W.Quantity as [Üretim Siparişi Miktarı], sum(case when P.GCRecord is null then isnull(P.GrossQuantity, 0) else 0 end) as [Toplam Üretim], W.Quantity - sum(case when P.GCRecord is null then isnull(P.GrossQuantity, 0) else 0 end) as [Kalan Üretim], (select isnull(SUM(GrossQuantity), 0) from Production where GCRecord is null and SalesOrderDetail = D.Oid and IsLastProduction = 1) as [Sipariş Üretilen], MC.Name as [Renk], (W.Quantity - sum(case when P.GCRecord is null then isnull(P.GrossQuantity, 0) else 0 end)) / ((case when A.Capacity = 0 then 1 else A.Capacity end)/24) as [Kalan Süre (saat)], M.CapStatus as [Kapak Durumu], W.WorkOrderDate as [Üretim Siparişi Tarihi]  from Eco6LaminationWorkOrder W inner join SalesOrderDetail D on D.Oid = W.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product M on M.Oid = D.Product left outer join Production P on P.WorkOrderNumber = W.WorkOrderNumber inner join Machine A on A.Oid = W.Machine left outer join MaterialColor MC on MC.Oid = M.MaterialColor where W.GCRecord is null And W.WorkOrderStatus != 110 And W.WorkOrderStatus != 900 And A.Code = '{0}' group by W.Oid, W.WorkOrderStatus, W.SequenceNumber, W.WorkOrderNumber, W.WorkOrderDate, C.Name, O.OrderNumber, O.OrderDate, D.LineDeliveryDate, M.Code, D.Quantity, W.Quantity, A.Capacity, D.LineNumber, W.Width, W.Height, W.Thickness, M.CapStatus, M.Cap, W.WorkName, MC.Name, D.Oid order by W.SequenceNumber", machineCode);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlText, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();

            gridView1.Columns["Sipariş Miktarı"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Sipariş Miktarı"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Üretim Siparişi Miktarı"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Üretim Siparişi Miktarı"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Toplam Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Toplam Üretim"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Kalan Üretim"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Sipariş Üretilen"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Sipariş Üretilen"].DisplayFormat.FormatString = "n2";

            if (gridView1.Columns["Sıra No"] != null) gridView1.Columns["Sıra No"].Width = 30;
            if (gridView1.Columns["Müşteri"] != null) gridView1.Columns["Müşteri"].Width = 100;
            if (gridView1.Columns["Stok Kodu"] != null) gridView1.Columns["Stok Kodu"].Width = 60;
            if (gridView1.Columns["İşin Adı"] != null) gridView1.Columns["İşin Adı"].Width = 150;
            if (gridView1.Columns["En"] != null) gridView1.Columns["En"].Width = 30;
            if (gridView1.Columns["Boy"] != null) gridView1.Columns["Boy"].Width = 30;
            if (gridView1.Columns["Kalınlık"] != null)
            {
                gridView1.Columns["Kalınlık"].Width = 30;
                gridView1.Columns["Kalınlık"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Kalınlık"].DisplayFormat.FormatString = "n1";
            }
            if (gridView1.Columns["Sipariş No"] != null) gridView1.Columns["Sipariş No"].Width = 90;
            if (gridView1.Columns["Sipariş Miktarı"] != null) gridView1.Columns["Sipariş Miktarı"].Width = 60;
            if (gridView1.Columns["Üretim Siparişi Miktarı"] != null) gridView1.Columns["Üretim Siparişi Miktarı"].Width = 60;
            if (gridView1.Columns["Toplam Üretim"] != null) gridView1.Columns["Toplam Üretim"].Width = 60;
            if (gridView1.Columns["Kalan Üretim"] != null) gridView1.Columns["Kalan Üretim"].Width = 60;
            if (gridView1.Columns["Sipariş Üretilen"] != null) gridView1.Columns["Sipariş Üretilen"].Width = 60;
        }

        private void FillItemWithEnumValues(BarSubItem parentItem, Type enumType)
        {
            foreach (object current in Enum.GetValues(enumType))
            {
                EnumDescriptor ed = new EnumDescriptor(enumType);
                var item = new BarButtonItem() { Caption = ed.GetCaption(current), Name = current.ToString(), Tag = current };
                item.ItemClick += item_ItemClick;
                parentItem.AddItem(item);
            }
        }

        void item_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                var row = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                Eco6LaminationWorkOrder workOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", row["Üretim Siparişi No"].ToString()));
                if (workOrder != null)
                {
                    if (workOrder.WorkOrderStatus != WorkOrderStatus.ProductionStage)
                    {
                        workOrder.WorkOrderStatus = (WorkOrderStatus)e.Item.Tag;
                        objectSpace.CommitChanges();
                    }
                }
            }
            RefreshGrid(cmbMachine.Text);
        }

        private void cmbMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            var machine = objectSpace.FindObject<Machine>(new BinaryOperator("Code", cmbMachine.Text));
            if (machine != null)
            {
                RefreshGrid(cmbMachine.Text);

                var machineStop = objectSpace.FindObject<MachineStop>(CriteriaOperator.Parse("Machine = ? and Active = true", machine));
                if (machineStop != null)
                {
                    lblMachineStatus.Text = string.Format("Makinede {0} tarihinden itibaren {1} nedenli aktif bir duruş var.", machineStop.BeginDate.ToString(), machineStop.StopCode.Name);
                }
                else lblMachineStatus.Text = string.Empty;
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            if (e.Column.FieldName == "Sıra No")
            {
                ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(string.Format("update Eco6LaminationWorkOrder set SequenceNumber = {0} where WorkOrderNumber = '{1}'", e.Value, gridView1.GetRowCellValue(e.RowHandle, "Üretim Siparişi No")));
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Eco6LaminationWorkOrder obj = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", gridView1.GetFocusedRowCellValue("Üretim Siparişi No")));
            if (obj != null)
            {
                ShowViewParameters svp = new ShowViewParameters()
                {
                    CreatedView = winApplication.CreateDetailView(objectSpace, obj),
                    CreateAllControllers = true
                };
                winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            Eco6LaminationWorkOrder obj = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", gridView1.GetFocusedRowCellValue("Üretim Siparişi No")));
            if (obj != null)
            {
                ShowViewParameters svp = new ShowViewParameters()
                {
                    CreatedView = winApplication.CreateDetailView(objectSpace, obj),
                    CreateAllControllers = true
                };
                svp.CreatedView.Closed += CreatedView_Closed;
                winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }

        void CreatedView_Closed(object sender, EventArgs e)
        {
            btnRefresh_ItemClick(null, null);
        }

        private void btnClone_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            var workOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", gridView1.GetFocusedRowCellValue("Üretim Siparişi No")));
            if (workOrder != null)
            {
                Eco6LaminationWorkOrder obj = objectSpace.CreateObject<Eco6LaminationWorkOrder>();
                obj.WorkName = workOrder.WorkName;
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", workOrder.SalesOrderDetail.Oid));
                if (workOrder.Station != null) obj.Station = objectSpace.FindObject<Station>(new BinaryOperator("Oid", workOrder.Station.Oid));
                if (workOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", workOrder.Machine.Oid));
                if (workOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", workOrder.NextStation.Oid));
                obj.ProductionOption = workOrder.ProductionOption;
                obj.ProductionNote = workOrder.ProductionNote;
                obj.QualityNote = workOrder.QualityNote;
                obj.RollDiameter = workOrder.RollDiameter;
                obj.RollWeight = workOrder.RollWeight;
                obj.MinimumRollWeight = workOrder.MinimumRollWeight;
                obj.MaximumRollWeight = workOrder.MaximumRollWeight;
                if (workOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", workOrder.Bobbin.Oid));
                obj.SubFilm = workOrder.SubFilm;
                obj.SubFilmThickness = workOrder.SubFilmThickness;
                obj.SubFilmLength = workOrder.SubFilmLength;
                obj.SubFilmTension = workOrder.SubFilmTension;
                obj.TopFilm = workOrder.TopFilm;
                obj.TopFilmThickness = workOrder.TopFilmThickness;
                obj.TopFilmLength = workOrder.TopFilmLength;
                obj.TopFilmTension = workOrder.TopFilmTension;
                obj.CoatingRollerSize = workOrder.CoatingRollerSize;
                obj.SingleComponent = workOrder.SingleComponent;
                obj.SingleComponentReservoir = workOrder.SingleComponentReservoir;
                obj.SingleComponentHose = workOrder.SingleComponentHose;
                obj.SingleComponentSprayGun = workOrder.SingleComponentSprayGun;
                obj.DoubleComponent = workOrder.DoubleComponent;
                obj.DoubleComponentA = workOrder.DoubleComponentA;
                obj.DoubleComponentAReservoir = workOrder.DoubleComponentAReservoir;
                obj.DoubleComponentAHose = workOrder.DoubleComponentAHose;
                obj.DoubleComponentASprayGun = workOrder.DoubleComponentASprayGun;
                obj.DoubleComponentB = workOrder.DoubleComponentB;
                obj.DoubleComponentBReservoir = workOrder.DoubleComponentBReservoir;
                obj.DoubleComponentBHose = workOrder.DoubleComponentBHose;
                obj.DoubleComponentBSprayGun = workOrder.DoubleComponentBSprayGun;
                obj.Performance = workOrder.Performance;
                obj.LineSpeed = workOrder.LineSpeed;
                obj.Tension = workOrder.Tension;
                if (workOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", workOrder.Palette.Oid));
                obj.PaletteBobbinCount = workOrder.PaletteBobbinCount;

                //Sipariş ile gelen bilgiler
                obj.Width = workOrder.Width;
                obj.Height = workOrder.Height;
                obj.Thickness = workOrder.Thickness;
                obj.Length = workOrder.Length;
                obj.Density = workOrder.Density;
                obj.ShippingPackageType = workOrder.ShippingPackageType;

                obj.RecieptString = workOrder.RecieptString;
                foreach (var item in workOrder.Eco6LaminationWorkOrderReciepts)
                {
                    if (item.Product != workOrder.SalesOrderDetail.Product)
                    {
                        Eco6LaminationWorkOrderReciept workOrderReciept = objectSpace.CreateObject<Eco6LaminationWorkOrderReciept>();
                        workOrderReciept.Eco6LaminationWorkOrder = obj;
                        workOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                        workOrderReciept.Quantity = item.Quantity;
                        workOrderReciept.Rate = item.Rate;
                        workOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                        workOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    }
                }
                ShowViewParameters svp = new ShowViewParameters()
                {
                    CreatedView = winApplication.CreateDetailView(objectSpace, obj),
                    CreateAllControllers = true
                };
                svp.CreatedView.Closed += CreatedView_Closed;
                winApplication.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshGrid(cmbMachine.Text);
        }

        private void btnRezervation_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                var workOrder = objectSpace.FindObject<Eco6LaminationWorkOrder>(new BinaryOperator("WorkOrderNumber", gridView1.GetRowCellValue(i, "Üretim Siparişi No")));
                var rezervation = new Rezervation(((XPObjectSpace)objectSpace).Session)
                {
                    Description = string.Format("Çekim {0} tarihli rezervasyon", DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                };
                foreach (var item in workOrder.Eco6LaminationWorkOrderReciepts)
                {
                    decimal quantity = item.Quantity;
                    var store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ?", item.Product, item.Warehouse, item.Unit));
                    if (store != null)
                    {
                        if (store.Quantity < quantity)
                        {
                            var rezervationDetail = new RezervationDetail(((XPObjectSpace)objectSpace).Session)
                            {
                                Rezervation = rezervation,
                                Product = item.Product,
                                SourceWarehouse = item.Product.Warehouse,
                                DestinationWarehouse = item.Warehouse,
                                Unit = item.Unit,
                                RecieptQuantity = quantity,
                                Quantity = quantity - store.Quantity
                            };
                        }
                    }
                    else
                    {
                        var rezervationDetail = new RezervationDetail(((XPObjectSpace)objectSpace).Session)
                        {
                            Rezervation = rezervation,
                            Product = item.Product,
                            SourceWarehouse = item.Product.Warehouse,
                            DestinationWarehouse = item.Warehouse,
                            Unit = item.Unit,
                            RecieptQuantity = quantity,
                            Quantity = quantity
                        };
                    }
                }
            }
        }

        short copycount = 1;
        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            IObjectSpace objectSpace = winApplication.CreateObjectSpace();
            ReportData reportData = objectSpace.FindObject<ReportData>(new BinaryOperator("Name", "Eco6 Laminasyon Üretim Siparişi Formu"));
            if (reportData != null)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Çıktı:", "Çıktı Sayısı", "1", 100, 100);
                copycount = Convert.ToInt16(input);
                for (int i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    var report = reportData.LoadReport(objectSpace);
                    report.FilterString = string.Format("WorkOrderNumber = '{0}'", gridView1.GetRowCellValue(gridView1.GetSelectedRows()[i], "Üretim Siparişi No"));
                    report.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    if (report.PrintingSystem.Document.ScaleFactor > 1) report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    report.Print();
                }
            }
        }

        void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrinterSettings.Copies = copycount;
        }
    }
}
