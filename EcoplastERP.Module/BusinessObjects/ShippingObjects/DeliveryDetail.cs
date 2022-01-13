using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Delivery.DeliveryNumber")]
    [NavigationItem(false)]
    public class DeliveryDetail : BaseObject
    {
        public DeliveryDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ReadControl = true;
        }
        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.Connection != null)
            {
                decimal oldQuantity = Convert.ToDecimal(Session.ExecuteScalar("select Quantity from DeliveryDetail where Oid = @oid", new string[] { "@oid" }, new object[] { this.Oid }));
                if (oldQuantity != Quantity)
                {
                    ExpeditionDetail.Quantity = Quantity;
                    ExpeditionDetail.cQuantity = cQuantity;
                    ExpeditionDetail.ShippingPlan.Quantity = Quantity;
                    ExpeditionDetail.ShippingPlan.cQuantity = cQuantity;
                }
            }
            if (!Session.IsObjectMarkedDeleted(this))
            {
                bool oldReadControl = Convert.ToBoolean(Session.ExecuteScalar("select ReadControl from DeliveryDetail where Oid = @oid", new string[] { "@oid" }, new object[] { this.Oid }));
                if (oldReadControl != ReadControl)
                {
                    if (!Helpers.IsUserInRole("Teslimat Blokaj"))
                    {
                        ReadControl = true;
                        throw new UserFriendlyException("Okuma kontrolü kaldırmak için yetkiniz yok. Teslimat blokaj yetkisine sahip değilsiniz.");
                    }
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (DeliveryDetailLoadings.Count > 0)
            {
                throw new UserFriendlyException("Okutma yapılmış teslimat kalemi silinemez.");
            }
            if (Delivery.DeliveryDetails.Count == 1)
            {
                Delivery.Delete();
            }
        }
        // Fields...
        private SalesWaybillDetail _SalesWaybillDetail;
        private ExpeditionDetail _ExpeditionDetail;
        private bool _ReadControl;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private SalesOrderDetail _SalesOrderDetail;
        private int _LineNumber;
        private Delivery _Delivery;

        [NonCloneable]
        [Association]
        public Delivery Delivery
        {
            get { return _Delivery; }
            set
            {
                Delivery prevHome = _Delivery;
                if (SetPropertyValue("Delivery", ref _Delivery, value) && !IsLoading)
                {
                    if (!IsLoading && _Delivery != null)
                    {
                        int lineNumber = 10;
                        if (_Delivery.DeliveryDetails.Count > 0)
                        {
                            _Delivery.DeliveryDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _Delivery.DeliveryDetails[0].LineNumber + 10;
                        }
                        LineNumber = lineNumber;
                    }
                }
            }
        }

        [NonCloneable]
        [VisibleInDetailView(false)]
        public int LineNumber
        {
            get
            {
                return _LineNumber;
            }
            set
            {
                SetPropertyValue("LineNumber", ref _LineNumber, value);
            }
        }

        [RuleRequiredField]
        public SalesOrderDetail SalesOrderDetail
        {
            get
            {
                return _SalesOrderDetail;
            }
            set
            {
                SetPropertyValue("SalesOrderDetail", ref _SalesOrderDetail, value);
            }
        }

        [Appearance("DeliveryDetail.Unit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public Unit Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                SetPropertyValue("Unit", ref _Unit, value);
            }
        }

        [ImmediatePostData]
        [RuleValueComparison("DeliveryDetail.Quantity", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
                CalculatecQuantity();
            }
        }

        [Appearance("DeliveryDetail.cUnit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public Unit cUnit
        {
            get
            {
                return _CUnit;
            }
            set
            {
                SetPropertyValue("cUnit", ref _CUnit, value);
            }
        }

        [Appearance("DeliveryDetail.cQuantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal cQuantity
        {
            get
            {
                return _CQuantity;
            }
            set
            {
                SetPropertyValue("cQuantity", ref _CQuantity, value);
            }
        }

        public bool ReadControl
        {
            get
            {
                return _ReadControl;
            }
            set
            {
                SetPropertyValue("ReadControl", ref _ReadControl, value);
            }
        }

        [VisibleInDetailView(false)]
        public decimal LoadedQuantity
        {
            get
            {
                decimal total = 0;
                foreach (DeliveryDetailLoading detail in DeliveryDetailLoadings)
                {
                    total += detail.Quantity;
                }
                return total;
            }
        }

        [VisibleInDetailView(false)]
        public decimal LoadedcQuantity
        {
            get
            {
                decimal total = 0;
                foreach (DeliveryDetailLoading detail in DeliveryDetailLoadings)
                {
                    total += detail.cQuantity;
                }
                return total;
            }
        }

        [VisibleInDetailView(false)]
        public decimal PaletteLastWeightTotal
        {
            get
            {
                decimal total = 0;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(string.Format(@"select PaletteNumber from DeliveryDetailLoading where GCRecord is null and DeliveryDetail = '{0}' group by PaletteNumber", this.Oid), Session.ConnectionString);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PaletteNumber"].ToString() != "")
                    {
                        ProductionPalette productionPalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", dr["PaletteNumber"].ToString()));
                        if (productionPalette != null) total += productionPalette.LastWeight;
                    }
                }
                return total;
            }
        }

        [VisibleInDetailView(false)]
        public ExpeditionDetail ExpeditionDetail
        {
            get
            {
                return _ExpeditionDetail;
            }
            set
            {
                SetPropertyValue("ExpeditionDetail", ref _ExpeditionDetail, value);
            }
        }

        [VisibleInDetailView(false)]
        public SalesWaybillDetail SalesWaybillDetail
        {
            get
            {
                return _SalesWaybillDetail;
            }
            set
            {
                SetPropertyValue("SalesWaybillDetail", ref _SalesWaybillDetail, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<DeliveryDetailLoading> DeliveryDetailLoadings
        {
            get { return GetCollection<DeliveryDetailLoading>("DeliveryDetailLoadings"); }
        }

        private XPCollection<Store> storeList;
        public XPCollection<Store> StoreList
        {
            get
            {
                if (storeList == null)
                {
                    storeList = new XPCollection<Store>(Session, CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.ShippingWarehouse = true", SalesOrderDetail));
                }
                return storeList;
            }
        }

        #region functions
        public void RecalculateNumbers()
        {
            if (IsLoading) return;
            int number = 10;
            foreach (DeliveryDetailLoading item in DeliveryDetailLoadings)
            {
                item.LineNumber = number;
                number = number + 10;
            }
        }
        void CalculatecQuantity()
        {
            if (IsLoading) return;
            if (cUnit != null)
            {
                if (cUnit == Unit)
                {
                    cQuantity = Quantity;
                }
                else
                {
                    var cunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", Unit, cUnit, SalesOrderDetail.Product));
                    if (cunit != null) cQuantity = (Quantity * cunit.cQuantity) / cunit.BaseQuantity;
                    else
                    {
                        cunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", cUnit, Unit, SalesOrderDetail.Product));
                        if (cunit != null) cQuantity = (cunit.BaseQuantity * Quantity) / cunit.cQuantity;
                    }
                }
            }
            else cQuantity = Quantity;
        }
        #endregion
    }
}
