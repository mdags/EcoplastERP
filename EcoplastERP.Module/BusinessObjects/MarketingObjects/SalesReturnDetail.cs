using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Order_Item")]
    [DefaultProperty("SalesReturn.DocumentNumber")]
    [NavigationItem("MarketingManagement")]
    public class SalesReturnDetail : BaseObject
    {
        public SalesReturnDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Warehouse = Session.FindObject<Warehouse>(new BinaryOperator("Code", "590"));
        }
        // Fields...
        private string _Note;
        private SalesReturnReason _SalesReturnReason;
        private string _Barcode;
        private decimal _Quantity;
        private Unit _Unit;
        private Warehouse _Warehouse;
        private Product _Product;
        private SalesOrderDetail _SalesOrderDetail;

        [NonCloneable]
        [Association]
        public SalesReturn SalesReturn { get; set; }

        [ImmediatePostData]
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
                GetSalesOrderDetail();
            }
        }

        [ImmediatePostData]
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

        [VisibleInDetailView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                SetPropertyValue("Barcode", ref _Barcode, value);
            }
        }

        public SalesReturnReason SalesReturnReason
        {
            get
            {
                return _SalesReturnReason;
            }
            set
            {
                SetPropertyValue("SalesReturnReason", ref _SalesReturnReason, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                SetPropertyValue("Note", ref _Note, value);
            }
        }

        #region functions
        void GetSalesOrderDetail()
        {
            if (IsLoading) return;
            if(SalesOrderDetail != null)
            {
                Product = SalesOrderDetail.Product;
            }
        }
        public void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Unit = Product.Unit;
            }
        }
        #endregion
    }
}
