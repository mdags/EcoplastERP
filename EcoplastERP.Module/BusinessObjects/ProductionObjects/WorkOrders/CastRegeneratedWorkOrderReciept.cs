using System;
using System.Text;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Product.Name")]
    [NavigationItem(false)]
    public class CastRegeneratedWorkOrderReciept : BaseObject
    {
        public CastRegeneratedWorkOrderReciept(Session session)
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
                if (CastRegeneratedWorkOrder != null)
                {
                    if (CastRegeneratedWorkOrder.Station != null) Warehouse = CastRegeneratedWorkOrder.Station.SourceWarehouse;
                }
            }
            if (CastRegeneratedWorkOrder != null)
            {
                CastRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in CastRegeneratedWorkOrder.CastRegeneratedWorkOrderReciepts)
                {
                    sbReciept.AppendLine(String.Format("{0} - %{1:n2} - {2:n2}", reciept.Product.Name, reciept.Rate, reciept.Quantity));
                }
                CastRegeneratedWorkOrder.RecieptString = sbReciept.ToString();
            }
        }
        // Fields...
        private decimal _Quantity;
        private Unit _Unit;
        private decimal _Rate;
        private Warehouse _Warehouse;
        private Product _Product;

        [Association]
        public CastRegeneratedWorkOrder CastRegeneratedWorkOrder { get; set; }

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

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                SetPropertyValue("Rate", ref _Rate, value);
                UpdateQuantity();
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

        [Appearance("CastRegeneratedWorkOrderReciept.Quantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Unit = Product.Unit;
            }
        }
        void UpdateQuantity()
        {
            if (IsLoading) return;
            Quantity = CastRegeneratedWorkOrder.Quantity * Rate / 100;
        }
        #endregion
    }
}
