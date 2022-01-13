using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.Win.Forms;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class GenProdStatusReportUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;
        public int reportID = 1;
        string sqlText = string.Empty, where = string.Empty, groupby = string.Empty;

        public GenProdStatusReportUserControl()
        {
            InitializeComponent();
        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;
        }

        public void SelectReportType()
        {
            if (reportID == 1)
            {
                sqlText = @"select PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Oid as [ProductKindOid], PK.Name as [Ürün Cinsi], PKG.Name as [Satış Ürün Grubu] 
, SUM(D.cQuantity) as [Sipariş TÖB Miktarı], (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Sevk Edilen TÖB Miktar]
, SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Bekleyen TÖB Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) as [Depo TÖB Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) as [Baskı Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) as [Kesim Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) as [Dilme Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) as [Laminasyon Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu Çekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) as [Cast Depo]
, (SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid)))) - ((select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200)) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu Çekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select Oid from SalesOrderDetail where GCRecord is null and SalesOrderStatus < 200))) as [Çekim Üretilecek Miktar] 
from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = D.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join ProductKindGroup PKG on PKG.Oid = PK.ProductKindGroup 
where D.GCRecord is null and D.SalesOrderStatus < 200 and (PG.Code = 'OM' or PG.Code = 'SM') ";
                groupby = @" group by PG.Oid, PG.Name, PT.Oid, PT.Name, PK.Oid, PK.Name, PKG.Name";
            }
            else if (reportID == 2)
            {
                sqlText = @"select PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Oid as [ProductKindOid], PK.Name as [Ürün Cinsi], PKG.Name as [Satış Ürün Grubu] 
, SUM(D.cQuantity) as [Sipariş TÖB Miktarı], (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Sevk Edilen TÖB Miktar]
, SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Bekleyen TÖB Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) as [Depo TÖB Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) as [Baskı Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) as [Kesim Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) as [Dilme Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) as [Laminasyon Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu Çekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) as [Cast Depo]
, (SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid)))) - ((select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu Çekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (0, 2, 3, 4)))) as [Çekim Üretilecek Miktar] 
from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = D.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join ProductKindGroup PKG on PKG.Oid = PK.ProductKindGroup 
where D.GCRecord is null and D.SalesOrderStatus < 200 and (PG.Code = 'OM' or PG.Code = 'SM') and O.SalesOrderType in (0, 2, 3, 4) ";
                groupby = @" group by PG.Oid, PG.Name, PT.Oid, PT.Name, PK.Oid, PK.Name, PKG.Name";
            }
            else if (reportID == 3)
            {
                sqlText = @"select PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Oid as [ProductKindOid], PK.Name as [Ürün Cinsi], PKG.Name as [Satış Ürün Grubu] 
, SUM(D.cQuantity) as [Sipariş TÖB Miktarı], (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Sevk Edilen TÖB Miktar]
, SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid))) as [Bekleyen TÖB Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Depo TÖB Miktar]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Baskı Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Kesim Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Dilme Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Laminasyon Depo]
, (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu Çekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) as [Cast Depo]
, (SUM(D.cQuantity) - (select isnull(sum(cQuantity), 0) from DeliveryDetailLoading where GCRecord is null and DeliveryDetail in (select Oid from DeliveryDetail where GCRecord is null and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder inner join Product P1 on P1.Oid = D1.Product where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7) and P1.ProductGroup = PG.Oid and P1.ProductType = PT.Oid and P1.ProductKind = PK.Oid)))) - ((select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse in (select Oid from Warehouse where GCRecord is null and ShippingWarehouse = 1) and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Baskı') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Kesim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Dilme') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Laminasyon') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7))) + (select isnull(SUM(cQuantity), 0) from Store where GCRecord is null and Warehouse = (select SourceWarehouse from Station where GCRecord is null and Name = 'Balonlu Çekim') and Product in (select Oid from Product where GCRecord is null and ProductGroup = PG.Oid and ProductType = PT.Oid and ProductKind = PK.Oid) and SalesOrderDetail in (select D1.Oid from SalesOrderDetail D1 inner join SalesOrder O1 on O1.Oid = D1.SalesOrder where D1.GCRecord is null and D1.SalesOrderStatus < 200 and O1.SalesOrderType in (1, 5, 6, 7)))) as [Çekim Üretilecek Miktar] 
from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Product P on P.Oid = D.Product inner join ProductGroup PG on PG.Oid = P.ProductGroup inner join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join ProductKindGroup PKG on PKG.Oid = PK.ProductKindGroup 
where D.GCRecord is null and D.SalesOrderStatus < 200 and (PG.Code = 'OM' or PG.Code = 'SM') and O.SalesOrderType in (1, 5, 6, 7) ";
                groupby = @" group by PG.Oid, PG.Name, PT.Oid, PT.Name, PK.Oid, PK.Name, PKG.Name";
            }
        }

        public void RefreshGrid()
        {
            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.SelectCommand.CommandTimeout = 300;
            adapter.Fill(dt);
            dt.Columns["ProductKindOid"].ColumnMapping = MappingType.Hidden;
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();

            if (gridView1.Columns["Sipariş TÖB Miktarı"] != null)
            {
                gridView1.Columns["Sipariş TÖB Miktarı"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sipariş TÖB Miktarı"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sipariş TÖB Miktarı"].Summary.Count == 0)
                    gridView1.Columns["Sipariş TÖB Miktarı"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sipariş TÖB Miktarı", "{0:n2}");
            }
            if (gridView1.Columns["Sevk Edilen TÖB Miktar"] != null)
            {
                gridView1.Columns["Sevk Edilen TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Sevk Edilen TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Sevk Edilen TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Sevk Edilen TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Sevk Edilen TÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["Bekleyen TÖB Miktar"] != null)
            {
                gridView1.Columns["Bekleyen TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Bekleyen TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Bekleyen TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Bekleyen TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Bekleyen TÖB Miktar", "{0:n2}");
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
            if (gridView1.Columns["Cast Depo"] != null)
            {
                gridView1.Columns["Cast Depo"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Cast Depo"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Cast Depo"].Summary.Count == 0)
                    gridView1.Columns["Cast Depo"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Cast Depo", "{0:n2}");
            }
            if (gridView1.Columns["Üretilecek TÖB Miktar"] != null)
            {
                gridView1.Columns["Üretilecek TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["Üretilecek TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["Üretilecek TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["Üretilecek TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Üretilecek TÖB Miktar", "{0:n2}");
            }
        }

        private void gridView1_DoubleClick(object sender, System.EventArgs e)
        {
            if (gridView1.FocusedValue == null) return;
            GenProdStatusReportDetailForm form = new GenProdStatusReportDetailForm()
            {
                objectSpace = objectSpace,
                reportID = this.reportID,
                productKindOid = gridView1.GetFocusedRowCellValue("ProductKindOid").ToString()
            };
            form.ShowDialog();
        }
    }
}
