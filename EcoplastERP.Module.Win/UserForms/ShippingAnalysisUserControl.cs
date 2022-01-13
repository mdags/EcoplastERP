using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class ShippingAnalysisUserControl : XtraUserControl, IComplexControl
    {
        private IObjectSpace objectSpace;

        public ShippingAnalysisUserControl()
        {
            InitializeComponent();
        }

        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.objectSpace = objectSpace;

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select C.Oid, C.Name as [Adı] from Contact C inner join ContactType T on T.Oid = C.ContactType where C.GCRecord is null and T.Code != 'S' order by C.Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            sleContact.Properties.DataSource = dt;
            sleContact.Properties.DisplayMember = "Adı";
            sleContact.Properties.ValueMember = "Oid";

            dt = new DataTable();
            da = new SqlDataAdapter(@"select Oid, Name as [Adı] from ProductGroup where GCRecord is null order by Name", ((XPObjectSpace)objectSpace).Session.ConnectionString);
            da.Fill(dt);
            dt.Columns["Oid"].ColumnMapping = MappingType.Hidden;
            sleProductGroup.Properties.DataSource = dt;
            sleProductGroup.Properties.DisplayMember = "Adı";
            sleProductGroup.Properties.ValueMember = "Oid";

            deBeginDate.DateTime = DateTime.Now.AddDays(-1);
            deEndDate.DateTime = DateTime.Now;
        }

        public void RefreshGrid()
        {
            string where = string.Empty;
            string sqlText = string.Format(@"select W.WaybillNumber as [İrsaliye No], W.WaybillDate as [İrsaliye Tarihi], W.ReferenceWaybillNumber as [İrsaliye Matbu No], W.ReferenceWaybillDate as [İrsaliye Matbu Tarihi], O.OrderNumber+'/'+cast(SD.LineNumber as varchar(5)) as [Sipariş No], C.Code as [Şiparişi Veren Kod], C.Name as [Siparişi Veren], SC.Code as [Malı Teslim Alan Kod],SC.Name as [Malı Teslim Alan], P.Code as [Malzeme Kodu], P.Name as [Malzeme Adı], sum(WD.Quantity) as [SÖB Miktar], U.Code as [SÖB Birim], sum(WD.cQuantity) as [TÖB Miktar], CU.Code as [TÖB Birim], E.ExpeditionNumber+'/'+cast(ED.LineNumber as varchar(5)) as [Sefer No], D.DeliveryNumber+'/'+cast(DD.LineNumber as varchar(5)) as [Teslim No], EM.NameSurname as [Sevkiyatçı], T.PlateNumber as [Plaka], TD.NameSurname as [Şoför], TD.CellPhone as [Telefon], R.Name as [Rota], CT.Name as [İl], PG.Name as [Ürün Grubu], PT.Name as [Ürün Tipi], PK.Name as [Ürün Cinsi], PKG.Name as [Satış Ürün Grubu], (case when O.SalesOrderType = 0 then 'Standart Siparişi' when O.SalesOrderType = 1 then 'Planlama Siparişi' when O.SalesOrderType = 2 then 'İhracat Sipariş' when O.SalesOrderType = 3 then 'İhraç Kayıtlı Sipariş' when O.SalesOrderType = 4 then 'A3 Siparişi' when O.SalesOrderType = 5 then 'Rejenere Siparişi' when O.SalesOrderType = 6 then 'İade Siparişi' when O.SalesOrderType = 7 then 'Arge Siparişi' else '' end) as [Sipariş Türü], DC.Name as [Dağıtım Kanalı], SO.Name as [Satış Ofisi], SG.Name as [Satış Grubu], PM.Name as [Ödeme Metodu] from SalesWaybillDetail WD inner join SalesWaybill W on W.Oid = WD.SalesWaybill inner join SalesOrderDetail SD on SD.Oid = WD.SalesOrderDetail inner join SalesOrder O on O.Oid = SD.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Contact SC on SC.Oid = O.ShippingContact left outer join City CT on CT.Oid = C.City left outer join DistributionChannel DC on DC.Oid = O.DistributionChannel left outer join SalesOffice SO on SO.Oid = O.SalesOffice left outer join SalesGroup SG on SG.Oid = O.SalesGroup left outer join PaymentMethod PM on PM.Oid = O.PaymentMethod inner join Product P on P.Oid = WD.Product left outer join ProductGroup PG on PG.Oid = P.ProductGroup left outer join ProductType PT on PT.Oid = P.ProductType left outer join ProductKind PK on PK.Oid = P.ProductKind left outer join ProductKindGroup PKG on PKG.Oid = PK.ProductKindGroup inner join Unit U on U.Oid = WD.Unit inner join Unit CU on CU.Oid = WD.cUnit inner join DeliveryDetail DD on DD.Oid = WD.DeliveryDetail inner join Delivery D on D.Oid = DD.Delivery inner join ExpeditionDetail ED on ED.Oid = WD.ExpeditionDetail inner join Expedition E on E.Oid = ED.Expedition left outer join ShippingUser SU on SU.Oid = E.ShippingUser left outer join Employee EM on EM.Oid = SU.Employee left outer join Truck T on T.Oid = E.Truck left outer join TruckDriver TD on TD.Oid = E.TruckDriver left outer join [Route] R on R.Oid = E.[Route] where WD.GCRecord is null and cast(W.WaybillDate as date) between '{0}' and '{1}'", deBeginDate.DateTime.ToString("yyyy-MM-dd"), deEndDate.DateTime.ToString("yyyy-MM-dd"));
            const string groupby = @" group by W.WaybillNumber, W.WaybillDate, W.ReferenceWaybillNumber, W.ReferenceWaybillDate, O.OrderNumber, SD.LineNumber, C.Code, C.Name, SC.Code, SC.Name, P.Code, P.Name, U.Code, CU.Code, E.ExpeditionNumber, ED.LineNumber, D.DeliveryNumber, DD.LineNumber, EM.NameSurname, T.PlateNumber, TD.NameSurname, TD.CellPhone, R.Name, CT.Name, PG.Name, PT.Name, PK.Name, PKG.Name, O.SalesOrderType, DC.Name, SO.Name, SG.Name, PM.Name";

            DataTable dt = new DataTable();
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            where = string.Empty;

            if (sleContact.EditValue != null)
            {
                where += string.Format(" and C.Oid = '{0}'", sleContact.EditValue);
            }
            if (sleProductGroup.EditValue != null)
            {
                where += string.Format(" and PG.Oid = '{0}'", sleProductGroup.EditValue);
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sqlText + where + groupby, ((XPObjectSpace)objectSpace).Session.ConnectionString);
            adapter.Fill(dt);
            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.BestFitColumns();

            if (gridView1.Columns["SÖB Miktar"] != null)
            {
                gridView1.Columns["SÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["SÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["SÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["SÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "SÖB Miktar", "{0:n2}");
            }
            if (gridView1.Columns["TÖB Miktar"] != null)
            {
                gridView1.Columns["TÖB Miktar"].DisplayFormat.FormatType = FormatType.Numeric;
                gridView1.Columns["TÖB Miktar"].DisplayFormat.FormatString = "n2";
                if (gridView1.Columns["TÖB Miktar"].Summary.Count == 0)
                    gridView1.Columns["TÖB Miktar"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TÖB Miktar", "{0:n2}");
            }

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }
    }
}
