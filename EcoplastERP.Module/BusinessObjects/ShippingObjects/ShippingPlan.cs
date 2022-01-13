using System;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
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
    [ImageName("BO_Task")]
    [DefaultProperty("SalesOrderDetail.LineNumber")]
    [NavigationItem("ShippingManagement")]
    [Appearance("ShippingPlan.GreenYellow", TargetItems = "ShippingPlanStatus", Criteria = "ShippingPlanStatus = 'WaitingforExpedition'", BackColor = "GreenYellow")]
    [Appearance("ShippingPlan.Tomato", TargetItems = "ShippingPlanStatus", Criteria = "ShippingPlanStatus != 'WaitingforExpedition'", BackColor = "Tomato")]
    [Appearance("ShippingPlan.MistyRose", TargetItems = "ShippingPlanStatus", Criteria = "ShippingPlanStatus = 'WaitingforCustomerVehicle' or ShippingPlanStatus = 'WaitingforShippingAddress'", BackColor = "MistyRose")]
    [Appearance("ShippingPlan.SteelBlue", TargetItems = "ShippingPlanStatus", Criteria = "ShippingPlanStatus = 'WaitingforLoading'", BackColor = "SteelBlue")]
    public class ShippingPlan : BaseObject
    {
        public ShippingPlan(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ShippingPlanDate = Helpers.GetSystemDate(Session);
            NotifiedUser = SecuritySystem.CurrentUserName;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (ExpeditionDetail != null)
            {
                if (ExpeditionDetail.Expedition != null)
                {
                    int expeditionStatus = Convert.ToInt32(Session.ExecuteScalar(@"select ExpeditionStatus from Expedition where GCRecord is null and Oid = @oid", new string[] { "@oid" }, new object[] { ExpeditionDetail.Expedition.Oid }));
                    if ((ExpeditionStatus)Enum.ToObject(typeof(ExpeditionStatus), expeditionStatus) == ExpeditionStatus.Completed)
                    {
                        throw new UserFriendlyException("Sevk planýnýn baðlý olduðu sefer kapatýlmýþ. Herhangi bir deðiþiklik yapamazsýnýz. !");
                    }
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (ExpeditionDetail != null) throw new UserFriendlyException("Sefere atanmýþ sevk planý silinemez.");
        }
        // Fields...
        private string _NotifiedUser;
        private ExpeditionDetail _ExpeditionDetail;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private SalesOrderDetail _SalesOrderDetail;
        private int _LineNumber;
        private DateTime _SetupDate;
        private DateTime _ShippingPlanDate;
        private ShippingPlanStatus _ShippingPlanStatus;

        //[Appearance("ShippingPlan.ShippingPlanStatus", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public ShippingPlanStatus ShippingPlanStatus
        {
            get
            {
                return _ShippingPlanStatus;
            }
            set
            {
                SetPropertyValue("ShippingPlanStatus", ref _ShippingPlanStatus, value);
            }
        }

        public DateTime ShippingPlanDate
        {
            get
            {
                return _ShippingPlanDate;
            }
            set
            {
                SetPropertyValue("ShippingPlanDate", ref _ShippingPlanDate, value);
            }
        }

        public DateTime SetupDate
        {
            get
            {
                return _SetupDate;
            }
            set
            {
                SetPropertyValue("SetupDate", ref _SetupDate, value);
            }
        }

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
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        [ImmediatePostData]
        public Unit cUnit
        {
            get
            {
                return _CUnit;
            }
            set
            {
                SetPropertyValue("cUnit", ref _CUnit, value);
                CalculatecQuantity();
            }
        }

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
        public string NotifiedUser
        {
            get
            {
                return _NotifiedUser;
            }
            set
            {
                SetPropertyValue("NotifiedUser", ref _NotifiedUser, value);
            }
        }

        [VisibleInDetailView(false)]
        public string E
        {
            get
            {
                Production production = Session.FindObject<Production>(new BinaryOperator("SalesOrderDetail", SalesOrderDetail));
                return production != null ? "E" : "H";
            }
        }

        [VisibleInDetailView(false)]
        public decimal StoreQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.Code = '800'", SalesOrderDetail)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal StorecQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Sum(cQuantity)"), CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.Code = '800'", SalesOrderDetail)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal PaletteLastWeightTotal
        {
            get
            {
                decimal total = 0;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(string.Format(@"select PaletteNumber from Store where GCrecord is null and SalesOrderDetail = '{0}' and Warehouse in (select Oid from Warehouse where GCRecord is null and Code like '8%') group by PaletteNumber", SalesOrderDetail.Oid), Session.ConnectionString);
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
        public int PaletteCount
        {
            get
            {
                return Convert.ToInt32(Session.ExecuteScalar("select count(distinct palettenumber) from Store where SalesOrderDetail = @1 and Warehouse in (select Oid from Warehouse where Code like '8%')", new string[] { "@1" }, new object[] { SalesOrderDetail.Oid }));
            }
        }

        [VisibleInDetailView(false)]
        public int RollCount
        {
            get
            {
                return Convert.ToInt32(Session.Evaluate(typeof(Store), CriteriaOperator.Parse("Count"), CriteriaOperator.Parse("SalesOrderDetail = ? and Contains(Warehouse.Code, ?)", SalesOrderDetail)));
            }
        }

        #region functions
        void CalculatecQuantity()
        {
            if (IsLoading) return;
            if (cUnit != null)
            {
                if (cUnit == Unit) cQuantity = Quantity;
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

    [NonPersistent]
    [DomainComponent]
    public class SelectExpeditionParameters
    {
        [ImmediatePostData]
        public Expedition Expedition { get; set; }
    }
}
