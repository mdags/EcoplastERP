using System;
using System.Text;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Product.Name")]
    [NavigationItem(false)]
    public class CastFilmingWorkOrderReciept : BaseObject
    {
        public CastFilmingWorkOrderReciept(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                if (CastFilmingWorkOrder != null)
                {
                    if (CastFilmingWorkOrder.Station != null) Warehouse = CastFilmingWorkOrder.Station.SourceWarehouse;
                }
            }
            if (CastFilmingWorkOrder != null)
            {
                CastFilmingWorkOrder.CastFilmingWorkOrderReciepts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var item in CastFilmingWorkOrder.CastFilmingWorkOrderParts)
                {
                    CastFilmingWorkOrder.CastFilmingWorkOrderReciepts.Filter = new BinaryOperator("MachinePart.Name", item.MachinePart.Name);
                    foreach (var reciept in CastFilmingWorkOrder.CastFilmingWorkOrderReciepts)
                    {
                        sbReciept.AppendLine(string.Format("{0} - %{1:n2} - {2}  /  ", reciept.MachinePart.Name, reciept.WorkOrderRate, reciept.Product.Name));
                    }
                }
                CastFilmingWorkOrder.RecieptString = sbReciept.ToString();
                CastFilmingWorkOrder.CastFilmingWorkOrderReciepts.Filter = null;
            }
        }
        // Fields...
        private decimal _Quantity;
        private Unit _Unit;
        private decimal _WorkOrderRate;
        private decimal _Rate;
        private MachinePart _MachinePart;
        private Warehouse _Warehouse;
        private Product _Product;

        [Association]
        public CastFilmingWorkOrder CastFilmingWorkOrder { get; set; }

        [RuleRequiredField]
        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                SetPropertyValue("Product", ref _Product, value);
                GetProduct();
            }
        }

        [VisibleInDetailView(false)]
        public Warehouse Warehouse
        {
            get
            {
                return _Warehouse;
            }
            set
            {
                SetPropertyValue("Warehouse", ref _Warehouse, value);
            }
        }

        [RuleRequiredField]
        public MachinePart MachinePart
        {
            get
            {
                return _MachinePart;
            }
            set
            {
                SetPropertyValue("MachinePart", ref _MachinePart, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public decimal Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                SetPropertyValue("Rate", ref _Rate, value);
            }
        }

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal WorkOrderRate
        {
            get
            {
                return _WorkOrderRate;
            }
            set
            {
                SetPropertyValue("WorkOrderRate", ref _WorkOrderRate, value);
                SetRate();
            }
        }

        [RuleRequiredField]
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

        [Appearance("CastFilmingWorkOrderReciept.Quantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
            }
        }

        public decimal StoreQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate<Store>(CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("Warehouse = ? and Product = ?", Warehouse, Product)));
            }
        }

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Unit = Product.Unit;
            }
        }
        void SetRate()
        {
            if (IsLoading) return;
            if (MachinePart != null)
            {
                foreach (var item in CastFilmingWorkOrder.CastFilmingWorkOrderParts)
                {
                    if (item.MachinePart.Name == MachinePart.Name)
                    {
                        if (CastFilmingWorkOrder.Thickness == 0) return;
                        Rate = WorkOrderRate * (item.Thickness * 100 / CastFilmingWorkOrder.Thickness) / 100;
                        Quantity = CastFilmingWorkOrder.Quantity * Rate / 100;
                    }
                }
            }
        }
        #endregion
    }
}
