using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class SalesOrderReportUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;

        public SalesOrderReportUserControl()
        {
            InitializeComponent();
        }
        
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        private void SalesOrderReportUserControl_Load(object sender, EventArgs e)
        {
            List<EnumTable> list = new List<EnumTable>();
            foreach (object current in Enum.GetValues(typeof(SalesOrderStatus)))
            {
                EnumDescriptor ed = new EnumDescriptor(typeof(SalesOrderStatus));
                EnumTable enumTable = new EnumTable()
                {
                    Key = (int)current,
                    Value = ed.GetCaption(current)
                };
                list.Add(enumTable);
            }

            cbSalesOrderStatus.Properties.DataSource = list;
            cbSalesOrderStatus.Properties.DisplayMember = "Value";
            cbSalesOrderStatus.Properties.ValueMember = "Key";

            cbContact.Properties.DataSource = new XPCollection<Contact>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbContact.Properties.DisplayMember = "Name";
            cbContact.Properties.ValueMember = "Oid";

            cbShippingContact.Properties.DataSource = new XPCollection<Contact>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbShippingContact.Properties.DisplayMember = "Name";
            cbShippingContact.Properties.ValueMember = "Oid";

            cbProductKindGroup.Properties.DataSource = new XPCollection<ProductKindGroup>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductKindGroup.Properties.DisplayMember = "Name";
            cbProductKindGroup.Properties.ValueMember = "Oid";

            cbProductGroup.Properties.DataSource = new XPCollection<ProductGroup>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbProductGroup.Properties.DisplayMember = "Name";
            cbProductGroup.Properties.ValueMember = "Oid";

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
            #region sqltext
            string where = string.Empty;
            string sqlText = string.Format(@"select (case when D.SalesOrderStatus = 0 then 'Planlama Onayı Bekliyor' when D.SalesOrderStatus = 100 then 'Onay Bekliyor' when D.SalesOrderStatus = 101 then 'Eco1 Bekliyor' when D.SalesOrderStatus = 102 then 'Eco1 Laminasyon Bekliyor' when D.SalesOrderStatus = 103 then 'Eco2 Bekliyor' when D.SalesOrderStatus = 104 then 'Eco3 Bekliyor' when D.SalesOrderStatus = 105 then 'Eco4 Bekliyor' when D.SalesOrderStatus = 106 then 'Eco4 Dilme Bekliyor' when D.SalesOrderStatus = 107 then 'Eco5 Cpp Bekliyor' when D.SalesOrderStatus = 108 then 'Eco5 Stretch Bekliyor' when D.SalesOrderStatus = 109 then 'Eco5 Aktarma Bekliyor' when D.SalesOrderStatus = 110 then 'Eco5 Dilme Bekliyor' when D.SalesOrderStatus = 111 then 'Eco5 Rejenere Bekliyor' when D.SalesOrderStatus = 112 then 'Eco6 Bekliyor' when D.SalesOrderStatus = 113 then 'Eco6 Kesim Bekliyor' when D.SalesOrderStatus = 114 then 'Eco6 Laminasyon Bekliyor' when D.SalesOrderStatus = 120 then 'Üretim Bekliyor' when D.SalesOrderStatus = 130 then 'Sevkiyat Bekliyor' when D.SalesOrderStatus = 131 then 'Yükleme Bekliyor' when D.SalesOrderStatus = 200 then 'Sevk Edildi' when D.SalesOrderStatus = 900 then 'İptal Edildi' else '' end) as [Durumu], C.Name as [Siparişi Veren], SC.Name as [Malı Teslim Alan], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], O.OrderDate as [Sipariş Tarihi], D.LineDeliveryDate as [Termin Tarihi], P.Code as [Malzeme Kodu], P.Name as [İşin Adı], PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], PKG.Name as [Satış Ürün Grubu], P.Thickness as [Kalınlık], D.Quantity as [Sipariş SÖB Miktar], (select isnull(sum(Quantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen SÖB Miktar], (select isnull(SUM(Quantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) as [Depo SÖB Miktar], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1)) as [Depo TÖB Miktar], U.Code as [SÖB Birim], D.cQuantity as [Sipariş TÖB Miktar], (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail = D.Oid)) as [Sevk Edilen TÖB Miktar], D.CurrencyPrice as [Döviz Fiyatı], CR.Code as [Döviz Tipi], D.ExchangeRate as [Kur], D.Parity as [Parite], D.Price as [Fiyatı], D.PetkimPrice as [Petkim Fiyatı], D.GeneralCost as [Genel Gider], D.PetkimUnitPrice as [Petkim Birim Fiyat], D.PlateCost as [Klişe Bedeli], P.PieceWeight as [Gr/Adet], MT.Name as [Malzeme Türü], MC.Name as [Malzeme Rengi], R1.Code as [Film Kodu1], P.FilmCode1Thickness as [Film Kod1 Kalınlığı], R2.Code as [Film Kodu2], P.FilmCode2Thickness as [Film Kod2 Kalınlığı], (case when P.PrintStatus = 0 then 'Baskılı' else 'Baskısız' end) as [Baskı Durumu], (case when D.SalesOrderWorkStatus = 0 then 'Yeni' when D.SalesOrderWorkStatus = 1 then 'Tekrar' else 'Revizyon' end) as [İş Durumu], SO.Name as [Satış Ofisi], SG.Name as [Satış Grubu], R.Name as [Rota], CT.Name as [İl], (case when O.SalesOrderType = 0 then 'Standart Siparişi' when O.SalesOrderType = 1 then 'Planlama Siparişi' when O.SalesOrderType = 2 then 'İhracat Siparişi' when O.SalesOrderType = 3 then 'İhraç Kayıtlı Sipariş' when O.SalesOrderType = 4 then 'A3 Siparişi' when O.SalesOrderType = 5 then 'Rejenere Siparişi' when O.SalesOrderType = 6 then 'İade Siparişi' when O.SalesOrderType = 7 then 'Arge Siparişi' else '' end) as [Sipariş Türü], PZ.Name as [Petkim Zammı], (case when O.TransportType = 0 then 'Nakliye Bize' else 'Nakliye Karşıya' end) as [Nakliye Türü], DC.Name as [Dağıtım Kanalı], PM.Name as [Ödeme Methodu], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı')) as [Baskı Depo], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim')) as [Konfeksiyon Depo], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme')) as [Dilme Depo], (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and SalesOrderDetail = D.Oid and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon')) as [Laminasyon Depo], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco1 Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco1 Laminasyon Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco2 Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and RegeneratedWorkOrder in (select Oid from RegeneratedWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from RegeneratedWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and RegeneratedWorkOrder in (select Oid from RegeneratedWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from RegeneratedWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco3 Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CuttingWorkOrder in (select Oid from CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CuttingWorkOrder in (select Oid from CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco4 Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and SlicingWorkOrder in (select Oid from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and SlicingWorkOrder in (select Oid from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco4 Dilme Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CastTransferingWorkOrder in (select Oid from CastTransferingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CastTransferingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CastTransferingWorkOrder in (select Oid from CastTransferingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CastTransferingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco5 Aktarma Kalan Üretim], isnull((case when (select sum(Quantity) - (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and BalloonFilmingWorkOrder in (select Oid from BalloonFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from BalloonFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) from BalloonFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and BalloonFilmingWorkOrder in (select Oid from BalloonFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from BalloonFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco5 CPP Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco5 Dilme Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CastFilmingWorkOrder in (select Oid from CastFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CastFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and CastFilmingWorkOrder in (select Oid from CastFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from CastFilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco5 Çekim Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and Eco6WorkOrder in (select Oid from Eco6WorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from Eco6WorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and Eco6WorkOrder in (select Oid from Eco6WorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from Eco6WorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) from Eco6WorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco6 Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and Eco6CuttingWorkOrder in (select Oid from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and Eco6CuttingWorkOrder in (select Oid from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco6 Konfeksiyon Kalan Üretim], isnull((case when (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and Eco6LaminationWorkOrder in (select Oid from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) >= 0 then (select sum(Quantity) - (select isnull(sum(GrossQuantity), 0) from Production where GCRecord is null and Eco6LaminationWorkOrder in (select Oid from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid)) from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid) else 0 end), 0) as [Eco6 Laminasyon Kalan Üretim], (case when (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6CuttingWorkOrder in (select Oid from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0)) >= 0 then (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6CuttingWorkOrder in (select Oid from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı'))) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Baskı')), 0)) else 0 end) as [Baskı Depoya Girecek], (case when (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and SlicingWorkOrder in (select Oid from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0)) >= 0 then (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and SlicingWorkOrder in (select Oid from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim'))) from SlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Kesim')), 0)) else 0 end) as [Kesim Depoya Girecek], (case when (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6CuttingWorkOrder in (select Oid from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6LaminationWorkOrder in (select Oid from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0)) >= 0 then (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6CuttingWorkOrder in (select Oid from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from Eco6CuttingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6LaminationWorkOrder in (select Oid from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and LaminationWorkOrder in (select Oid from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme'))) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Dilme')), 0)) else 0 end) as [Dilme Depoya Girecek], (case when (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6LaminationWorkOrder in (select Oid from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0)) >= 0 then (isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and FilmingWorkOrder in (select Oid from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from FilmingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and CastSlicingWorkOrder in (select Oid from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from CastSlicingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and Eco6LaminationWorkOrder in (select Oid from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from Eco6LaminationWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0) + isnull((select sum(Quantity) - (select sum(GrossQuantity) from Production where GCRecord is null and PrintingWorkOrder in (select Oid from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon'))) from PrintingWorkOrder where GCRecord is null and SalesOrderDetail = D.Oid and NextStation = (select Oid from Station where Name = 'Laminasyon')), 0)) else 0 end) as [Laminasyon Depoya Girecek] from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join City CT on CT.Oid = C.City left outer join [Route] R on R.Oid = CT.[Route] inner join Contact SC on SC.Oid = O.ShippingContact inner join Product P on P.Oid = D.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType inner join ProductKind PK on PK.Oid = P.ProductKind inner join ProductKindGroup PKG on PKG.Oid = PK.ProductKindGroup left outer join MaterialType MT on MT.Oid = P.MaterialType left outer join MaterialColor MC on MC.Oid = P.MaterialColor left outer join Reciept R1 on R1.Oid = P.FilmCode1 left outer join Reciept R2 on R2.Oid = P.FilmCode2 inner join Unit U on U.Oid = D.Unit inner join Unit CU on CU.Oid = D.cUnit inner join SalesOffice SO on SO.Oid = O.SalesOffice inner join SalesGroup SG on SG.Oid = O.SalesGroup left outer join ContactGroup2 PZ on PZ.Oid = O.ContactGroup2 left outer join DistributionChannel DC on DC.Oid = O.DistributionChannel left outer join PaymentMethod PM on PM.Oid = O.PaymentMethod inner join Currency CR on CR.Oid = D.Currency where D.GCRecord is null and cast(O.OrderDate as date) between '{0}' and '{1}'", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));
            #endregion

            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;

            if (!string.IsNullOrEmpty(cbSalesOrderStatus.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbSalesOrderStatus.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and D.SalesOrderStatus in ({0})", list.Substring(0, list.Length - 1));
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
            if (!string.IsNullOrEmpty(cbShippingContact.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbShippingContact.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and SC.Oid in ({0})", list.Substring(0, list.Length - 1));
            }
            if (!string.IsNullOrEmpty(cbProductGroup.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductGroup.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and PG.Oid in ({0})", list.Substring(0, list.Length - 1));
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
            if (!string.IsNullOrEmpty(cbProductKindGroup.EditValue.ToString()))
            {
                string list = string.Empty;
                foreach (string item in cbProductKindGroup.EditValue.ToString().Split(','))
                {
                    list += string.Format("'{0}',", item.Trim());
                }
                where += string.Format(" and PKG.Oid in ({0})", list.Substring(0, list.Length - 1));
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();

            if (gridView1.Columns["Sipariş SÖB Miktar"] != null)
            {
                gridView1.Columns["Sipariş SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sipariş SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sipariş SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sipariş SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sipariş SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Sipariş TÖB Miktar"] != null)
            {
                gridView1.Columns["Sipariş TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sipariş TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sipariş TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sipariş TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sipariş TÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Edilen TÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Edilen TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Edilen TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen TÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Edilen SÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Edilen SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Depo SÖB Miktar"] != null)
            {
                gridView1.Columns["Depo SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Depo SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Depo SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Depo SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Depo SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Depo TÖB Miktar"] != null)
            {
                gridView1.Columns["Depo TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Depo TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Depo TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Depo TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Depo TÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Baskı Depo"] != null)
            {
                gridView1.Columns["Baskı Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Baskı Depo"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Baskı Depo"].Summary.Count == 0)
                    gridView1.Columns["Baskı Depo"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Baskı Depo", "{0:n2}");
            }
            if (gridView1.Columns["Konfeksiyon Depo"] != null)
            {
                gridView1.Columns["Konfeksiyon Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Konfeksiyon Depo"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Konfeksiyon Depo"].Summary.Count == 0)
                    gridView1.Columns["Konfeksiyon Depo"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Konfeksiyon Depo", "{0:n2}");
            }
            if (gridView1.Columns["Dilme Depo"] != null)
            {
                gridView1.Columns["Dilme Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Dilme Depo"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Dilme Depo"].Summary.Count == 0)
                    gridView1.Columns["Dilme Depo"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Dilme Depo", "{0:n2}");
            }
            if (gridView1.Columns["Laminasyon Depo"] != null)
            {
                gridView1.Columns["Laminasyon Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Laminasyon Depo"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Laminasyon Depo"].Summary.Count == 0)
                    gridView1.Columns["Laminasyon Depo"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Laminasyon Depo", "{0:n2}");
            }
            if (gridView1.Columns["Eco1 Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco1 Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco1 Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco1 Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco1 Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco1 Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco1 Laminasyon Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco1 Laminasyon Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco1 Laminasyon Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco1 Laminasyon Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco1 Laminasyon Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco1 Laminasyon Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco2 Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco2 Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco2 Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco2 Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco2 Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco2 Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco3 Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco3 Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco3 Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco3 Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco3 Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco3 Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco4 Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco4 Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco4 Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco4 Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco4 Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco4 Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco4 Dilme Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco4 Dilme Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco4 Dilme Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco4 Dilme Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco4 Dilme Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco4 Dilme Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco5 Aktarma Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco5 Aktarma Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco5 Aktarma Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco5 Aktarma Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco5 Aktarma Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco5 Aktarma Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco5 CPP Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco5 CPP Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco5 CPP Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco5 CPP Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco5 CPP Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco5 CPP Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco5 Dilme Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco5 Dilme Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco5 Dilme Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco5 Dilme Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco5 Dilme Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco5 Dilme Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco5 Çekim Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco5 Çekim Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco5 Çekim Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco5 Çekim Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco5 Çekim Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco5 Çekim Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco6 Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco6 Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco6 Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco6 Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco6 Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco6 Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco6 Konfeksiyon Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco6 Konfeksiyon Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco6 Konfeksiyon Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco6 Konfeksiyon Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco6 Konfeksiyon Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco6 Konfeksiyon Kalan Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Eco6 Laminasyon Kalan Üretim"] != null)
            {
                gridView1.Columns["Eco6 Laminasyon Kalan Üretim"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Eco6 Laminasyon Kalan Üretim"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Eco6 Laminasyon Kalan Üretim"].Summary.Count == 0)
                    gridView1.Columns["Eco6 Laminasyon Kalan Üretim"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Eco6 Laminasyon Kalan Üretim", "{0:n2}");
            }
            if (gridView1.Columns["Baskı Depoya Girecek"] != null)
            {
                gridView1.Columns["Baskı Depoya Girecek"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Baskı Depoya Girecek"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Baskı Depoya Girecek"].Summary.Count == 0)
                    gridView1.Columns["Baskı Depoya Girecek"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Baskı Depoya Girecek", "{0:n2}");
            }
            if (gridView1.Columns["Kesim Depoya Girecek"] != null)
            {
                gridView1.Columns["Kesim Depoya Girecek"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Kesim Depoya Girecek"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Kesim Depoya Girecek"].Summary.Count == 0)
                    gridView1.Columns["Kesim Depoya Girecek"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Kesim Depoya Girecek", "{0:n2}");
            }
            if (gridView1.Columns["Dilme Depoya Girecek"] != null)
            {
                gridView1.Columns["Dilme Depoya Girecek"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Dilme Depoya Girecek"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Dilme Depoya Girecek"].Summary.Count == 0)
                    gridView1.Columns["Dilme Depoya Girecek"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Dilme Depoya Girecek", "{0:n2}");
            }
            if (gridView1.Columns["Laminasyon Depoya Girecek"] != null)
            {
                gridView1.Columns["Laminasyon Depoya Girecek"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Laminasyon Depoya Girecek"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Laminasyon Depoya Girecek"].Summary.Count == 0)
                    gridView1.Columns["Laminasyon Depoya Girecek"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Laminasyon Depoya Girecek", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }
    }
}
