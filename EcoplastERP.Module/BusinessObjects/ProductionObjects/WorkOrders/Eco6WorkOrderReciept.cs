using System.Text;
using System.ComponentModel;
using DevExpress.Xpo;
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
    public class Eco6WorkOrderReciept : BaseObject
    {
        public Eco6WorkOrderReciept(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ResourceObligatory = false;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                if (Eco6WorkOrder != null)
                {
                    if (Eco6WorkOrder.Station != null) Warehouse = Eco6WorkOrder.Station.SourceWarehouse;
                }
            }
            if (Eco6WorkOrder != null)
            {
                Eco6WorkOrder.Eco6WorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in Eco6WorkOrder.Eco6WorkOrderReciepts)
                {
                    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));
                }
                Eco6WorkOrder.RecieptString = sbReciept.ToString();
            }
        }
        // Fields...
        private bool _ResourceObligatory;
        private decimal _Quantity;
        private Unit _Unit;
        private decimal _Rate;
        private MachinePart _MachinePart;
        private Warehouse _Warehouse;
        private Product _Product;

        [Association]
        public Eco6WorkOrder Eco6WorkOrder { get; set; }

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
                CalculateQuantity();
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

        [Appearance("Eco6WorkOrderReciept.Quantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        public bool ResourceObligatory
        {
            get
            {
                return _ResourceObligatory;
            }
            set
            {
                SetPropertyValue("ResourceObligatory", ref _ResourceObligatory, value);
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
        void CalculateQuantity()
        {
            if (IsLoading) return;
            Quantity = Eco6WorkOrder.Quantity * Rate / 100;
        }
        #endregion
    }
}
