using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using DevExpress.XtraEditors.Repository;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class NotifyShippedUserControl : XtraUserControl, IComplexControl
    {
        public NotifyShippedUserControl()
        {
            InitializeComponent();
        }

        private IObjectSpace objectSpace;
        string sqlText = string.Empty, where = string.Empty;

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        private void NotifyShippedUserControl_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(@"select Oid, Code as [Stok Kodu], Name as [Adı] from Product where GCRecord is null order by Name",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            ProductCode.Properties.DataSource = dt;
            ProductCode.Properties.DisplayMember = "Adı";
            ProductCode.Properties.ValueMember = "Oid";
            ProductCode.ForceInitialize();

            dt = new DataTable();
            adapter = new SqlDataAdapter(@"select Oid, Code as [Kodu], Name as [Adı] from Contact where GCRecord is null order by Code",
                ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            Contact.Properties.DataSource = dt;
            Contact.Properties.DisplayMember = "Adı";
            Contact.Properties.ValueMember = "Oid";
            Contact.ForceInitialize();
        }

        public void RefreshGrid()
        {
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            sqlText = @"select D.Oid, (case when D.SalesOrderStatus = 0 then 'Planlama Onayı Bekliyor' when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Eco1 Bekliyor' when D.SalesOrderStatus = 102 then 'Eco1 Laminasyon Bekliyor' when D.SalesOrderStatus = 103 then 'Eco2 Bekliyor' when D.SalesOrderStatus = 104 then 'Eco3 Bekliyor' when D.SalesOrderStatus = 105 then 'Eco4 Bekliyor' when D.SalesOrderStatus = 106 then 'Eco4 Dilme Bekliyor' when D.SalesOrderStatus = 107 then 'Eco5 CPP Bekliyor' when D.SalesOrderStatus = 108 then 'Eco5 Stretch Bekliyor' when D.SalesOrderStatus = 109 then 'Eco5 Aktarma Bekliyor' when D.SalesOrderStatus = 110 then 'Eco5 Dilme Bekliyor' when D.SalesOrderStatus = 111 then 'Eco5 Rejenere Bekliyor' when D.SalesOrderStatus = 112 then 'Eco6 Bekliyor' when D.SalesOrderStatus = 113 then 'Eco6 Konfeksiyon Bekliyor' when D.SalesOrderStatus = 114 then 'Eco6 Laminasyon Bekliyor' when D.SalesOrderStatus = 120 then 'Üretim Bekliyor' when D.SalesOrderStatus = 130 then 'Sevk Bekliyor' when D.SalesOrderStatus = 131 then 'Yükleme Bekliyor' else '' end) as [Sipariş Durumu], O.OrderNumber as [Sipariş Numarası], D.LineNumber as [Pozisyon], (select isnull(SUM(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) as [Sevk Depo SÖB Miktar], (select isnull(SUM(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) as [Sevk Depo TÖB Miktar], (case when (D.Quantity - (select isnull(SUM(Quantity), 0) from ShippingPlan where GCRecord is null and SalesOrderDetail = D.Oid)) > 0 then (D.Quantity - (select isnull(SUM(Quantity), 0) from ShippingPlan where GCRecord is null and SalesOrderDetail = D.Oid)) else 0 end) as [Sevk Bildirilecek SÖB Miktar], C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], D.LineDeliveryDate as [Termin Tarihi], (select isnull(SUM(Quantity), 0) from ShippingPlan where GCRecord is null and SalesOrderDetail = D.Oid) as [Sevk Bildirilen SÖB Miktar], (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen SÖB Miktar], G.Name as [Ürün Grubu], (case when (select top 1 Oid from Production where GCRecord is null and SalesOrderDetail = D.Oid) is null then 'E' else 'H' end) as [E], P.Name as [İşin Adı], R.Name as [Rota], CT.Name as [İli], D.Quantity as [SÖB Miktar], U.Code as [SÖB Birim], D.cQuantity as [TÖB Miktar], CU.Code as [TÖB Birim], D.CustomerProductCode as [Müşteri Stok Kodu], (case when D.SalesOrderWorkStatus = 0 then 'Yeni' when D.SalesOrderWorkStatus = 1 then 'Tekrar' when D.SalesOrderWorkStatus = 2 then 'Revizyon' else '' end) as [İşin Durumu], D.ShippedOptionPlus as [Sevk Opsiyonu (+%)], D.ShippedOptionMinus as [Sevk Opsiyonu (-%)] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = D.Product inner join ProductGroup G on G.Oid = P.ProductGroup left outer join City CT on CT.Oid = SC.City left outer join [Route] R on R.Oid = CT.[Route] inner join Unit U on U.Oid = D.Unit inner join Unit CU on CU.Oid = D.cUnit where D.GCRecord is null and D.SalesOrderStatus < 200 and O.SalesOrderType in (0, 2, 3, 4) ";
            where = string.Empty;
            if (Contact.EditValue != null) where += string.Format(" and SC.Oid = '{0}'", Contact.EditValue);
            if (ProductGroup.EditValue != null)
            {
                ProductGroup productGroup = objectSpace.FindObject<ProductGroup>(new BinaryOperator("Oid", ProductGroup.EditValue));
                if (productGroup != null) where += string.Format(" and P.ProductGroup = '{0}'", productGroup.Oid);
            }
            if (ProductCode.EditValue != null) where += string.Format(" and P.Oid = '{0}'", ProductCode.EditValue);
            if (rgrpShippingWarehouse.SelectedIndex == 0)
            {
                where += @" and (select isnull(SUM(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) > 0 ";
            }
            else if (rgrpShippingWarehouse.SelectedIndex == 1)
            {
                where += @" and (select isnull(SUM(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) = 0 ";
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();

            if (gridView1.RowCount > 0)
            {
                gridView1.Columns["Rota"].GroupIndex = 0;
                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    if (gridView1.Columns[i].FieldName != "Sevk Bildirilecek SÖB Miktar") gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                }

                gridView1.Columns["Sevk Depo SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Depo SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Depo SÖB Miktar"] != null)
                {
                    if (gridView1.Columns["Sevk Depo SÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["Sevk Depo SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Depo SÖB Miktar", "{0:n2}");
                }

                gridView1.Columns["Sevk Depo TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Depo TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Depo TÖB Miktar"] != null)
                {
                    if (gridView1.Columns["Sevk Depo TÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["Sevk Depo TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Depo TÖB Miktar", "{0:n2}");
                }

                RepositoryItemSpinEdit rep = new RepositoryItemSpinEdit() { Name = "SevkBildirilecek_REP" };
                rep.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                rep.Mask.EditMask = "n2";
                gridView1.Columns["Sevk Bildirilecek SÖB Miktar"].ColumnEdit = rep;
                gridView1.Columns["Sevk Bildirilecek SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Bildirilecek SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Bildirilecek SÖB Miktar"] != null)
                {
                    if (gridView1.Columns["Sevk Bildirilecek SÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["Sevk Bildirilecek SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Bildirilecek SÖB Miktar", "{0:n2}");
                }

                gridView1.Columns["Sevk Bildirilen SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Bildirilen SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Bildirilen SÖB Miktar"] != null)
                {
                    if (gridView1.Columns["Sevk Bildirilen SÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["Sevk Bildirilen SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Bildirilen SÖB Miktar", "{0:n2}");
                }

                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen SÖB Miktar"] != null)
                {
                    if (gridView1.Columns["Sevk Edilen SÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["Sevk Edilen SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen SÖB Miktar", "{0:n2}");
                }

                gridView1.Columns["SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["SÖB Miktar"] != null)
                {
                    if (gridView1.Columns["SÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "SÖB Miktar", "{0:n2}");
                }

                gridView1.Columns["TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["TÖB Miktar"] != null)
                {
                    if (gridView1.Columns["TÖB Miktar"].Summary.Count == 0)
                        gridView1.Columns["TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TÖB Miktar", "{0:n2}");
                }
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Sevk Bildirilecek SÖB Miktar")
            {
                SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", Guid.Parse(gridView1.GetFocusedRowCellValue("Oid").ToString())));
                if (salesOrderDetail != null)
                {
                    if (Convert.ToDecimal(e.Value) + salesOrderDetail.NotifiedQuantity <= salesOrderDetail.cQuantity + (salesOrderDetail.cQuantity * salesOrderDetail.ShippedOptionPlus / 100))
                    {

                    }
                    else
                    {
                        gridView1.SetFocusedRowCellValue("Sevk Bildirilecek SÖB Miktar", salesOrderDetail.Quantity - salesOrderDetail.NotifyShippedQuantity);
                        XtraMessageBox.Show("Sevk bildirilecek miktar sipariş ve sevk opsiyonu miktarından fazla olamaz.");
                    }
                }
            }
        }

        public void NotifyShipped(DateTime shippedDate)
        {
            if (gridView1.FocusedValue == null) return;
            DataRow rowData;
            int[] listRowList = this.gridView1.GetSelectedRows();
            for (int i = 0; i < listRowList.Length; i++)
            {
                rowData = this.gridView1.GetDataRow(listRowList[i]);
                if (rowData != null)
                {
                    SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", Guid.Parse(rowData["Oid"].ToString())));

                    if (salesOrderDetail.SalesOrder.Blockage == Blockage.AskBeforeShipping)
                    {
                        throw new UserFriendlyException("Bu siparişin blokajı Sevkten Önce Sor olarak ayarlanmış. Müşteri temsilcisi ile görüşünüz.");
                    }
                    ShippingPlan shippingPlan = objectSpace.CreateObject<ShippingPlan>();
                    shippingPlan.SetupDate = shippedDate;
                    shippingPlan.SalesOrderDetail = salesOrderDetail;
                    shippingPlan.Unit = salesOrderDetail.Unit;
                    shippingPlan.Quantity = Convert.ToDecimal(rowData["Sevk Bildirilecek SÖB Miktar"]);
                    shippingPlan.cUnit = salesOrderDetail.cUnit;
                    shippingPlan.cQuantity = (salesOrderDetail.cQuantity / salesOrderDetail.Quantity) * Convert.ToDecimal(rowData["Sevk Bildirilecek SÖB Miktar"]);
                    shippingPlan.NotifiedUser = objectSpace.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)) != null ? objectSpace.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName)).NameSurname : string.Empty;

                    if (salesOrderDetail.SalesOrder.PaymentBlockage)
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforPaymentProblem;
                    }
                    else if (salesOrderDetail.SalesOrder.ContactVehicle == ContactVehicle.Yes)
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforCustomerVehicle;
                    }
                    else if (salesOrderDetail.DeliveryBlockType == DeliveryBlockType.DeadlineShipment && DateTime.Now.Date < salesOrderDetail.LineDeliveryDate.Date.AddDays(-5))
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforShippingDelivery;
                    }
                    else if (salesOrderDetail.ShippingBlockType == ShippingBlockType.CompleteShipment)
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforCompleteDelivery;
                    }
                    else if (salesOrderDetail.SalesOrder.Blockage == Blockage.NextMonthInvoice && salesOrderDetail.LineDeliveryDate.Month > DateTime.Now.Month)
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforNextMonthInvoice;
                    }
                    else if (salesOrderDetail.SalesOrder.Blockage == Blockage.CustomerWaybill)
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforCustomerWaybill;
                    }
                    else if (salesOrderDetail.SalesOrder.Blockage == Blockage.StoreProduction)
                    {
                        shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforStoreProduction;
                    }
                    else shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforExpedition;

                    objectSpace.CommitChanges();
                    RefreshGrid();
                }
            }
        }
    }
}