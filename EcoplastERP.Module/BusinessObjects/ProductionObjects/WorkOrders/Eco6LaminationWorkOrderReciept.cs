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
    public class Eco6LaminationWorkOrderReciept : BaseObject
    {
        public Eco6LaminationWorkOrderReciept(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ResourceObligatory = true;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                if (Eco6LaminationWorkOrder != null)
                {
                    if (Eco6LaminationWorkOrder.Station != null) Warehouse = Eco6LaminationWorkOrder.Station.SourceWarehouse;
                }
            }
            if (Eco6LaminationWorkOrder != null)
            {
                Eco6LaminationWorkOrder.Eco6LaminationWorkOrderReciepts.Sorting.Add(new SortProperty("Product.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbReciept = new StringBuilder();
                foreach (var reciept in Eco6LaminationWorkOrder.Eco6LaminationWorkOrderReciepts)
                {
                    sbReciept.AppendLine(string.Format("{0} - %{1:n2} / ", reciept.Product.Name, reciept.Rate));
                }
                Eco6LaminationWorkOrder.RecieptString = sbReciept.ToString();
            }
        }
        // Fields...
        private bool _ResourceObligatory;
        private decimal _Quantity;
        private Unit _Unit;
        private decimal _Rate;
        private Warehouse _Warehouse;
        private Product _Product;

        [Association]
        public Eco6LaminationWorkOrder Eco6LaminationWorkOrder { get; set; }

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

        [Appearance("Eco6LaminationWorkOrderReciept.Quantity", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
            Quantity = Eco6LaminationWorkOrder.Quantity * Rate / 100;
        }
        #endregion
    }
}
