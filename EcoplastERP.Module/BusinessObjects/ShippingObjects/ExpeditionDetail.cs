using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Expedition.ExpeditionNumber")]
    [NavigationItem(false)]
    public class ExpeditionDetail : BaseObject
    {
        public ExpeditionDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (DeliveryDetail != null)
            {
                DeliveryDetail.Delete();
            }
            if (ShippingPlan != null)
            {
                ShippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforExpedition;
            }
            if (Expedition.ExpeditionDetails.Count == 1)
            {
                Expedition.Delete();
            }
        }
        // Fields...
        private DeliveryDetail _DeliveryDetail;
        private ShippingPlan _ShippingPlan;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private SalesOrderDetail _SalesOrderDetail;
        private int _LineNumber;
        private Expedition _Expedition;

        [NonCloneable]
        [Association]
        public Expedition Expedition
        {
            get { return _Expedition; }
            set
            {
                Expedition prevHome = _Expedition;
                if (SetPropertyValue("Expedition", ref _Expedition, value) && !IsLoading)
                {
                    if (!IsLoading && _Expedition != null)
                    {
                        int lineNumber = 10;
                        if (_Expedition.ExpeditionDetails.Count > 0)
                        {
                            _Expedition.ExpeditionDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _Expedition.ExpeditionDetails[0].LineNumber + 10;
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

        [Appearance("ExpeditionDetail.Unit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
        [Appearance("ExpeditionDetail.cUnit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Appearance("ExpeditionDetail.cQuantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
        public ShippingPlan ShippingPlan
        {
            get
            {
                return _ShippingPlan;
            }
            set
            {
                SetPropertyValue("ShippingPlan", ref _ShippingPlan, value);
            }
        }

        [VisibleInDetailView(false)]
        public DeliveryDetail DeliveryDetail
        {
            get
            {
                return _DeliveryDetail;
            }
            set
            {
                SetPropertyValue("DeliveryDetail", ref _DeliveryDetail, value);
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

        public decimal TotalLoadedQuantity
        {
            get
            {
                return DeliveryDetail != null ? DeliveryDetail.LoadedQuantity : 0;
            }
        }

        public decimal TotalLoadedcQuantity
        {
            get
            {
                return DeliveryDetail != null ? DeliveryDetail.LoadedcQuantity : 0;
            }
        }

        public decimal TotalPaletteLastWeight
        {
            get
            {
                return DeliveryDetail != null ? DeliveryDetail.PaletteLastWeightTotal : 0;
            }
        }

        #region functions
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
