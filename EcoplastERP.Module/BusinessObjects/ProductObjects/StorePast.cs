using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_State")]
    [DefaultProperty("Product.Code")]
    [NavigationItem(false)]
    public class StorePast : BaseObject
    {
        public StorePast(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private WarehouseCell _WarehouseCell;
        private Warehouse _Warehouse;
        private string _PaletteNumber;
        private string _PartyNumber;
        private SalesOrderDetail _SalesOrderDetail;
        private string _Barcode;
        private Product _Product;
        private DateTime _StoreDate;

        public DateTime StoreDate
        {
            get
            {
                return _StoreDate;
            }
            set
            {
                SetPropertyValue("StoreDate", ref _StoreDate, value);
            }
        }

        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                SetPropertyValue("Product", ref _Product, value);
            }
        }

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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string PartyNumber
        {
            get
            {
                return _PartyNumber;
            }
            set
            {
                SetPropertyValue("PartyNumber", ref _PartyNumber, value);
            }
        }

        public string PaletteNumber
        {
            get
            {
                return _PaletteNumber;
            }
            set
            {
                SetPropertyValue("PaletteNumber", ref _PaletteNumber, value);
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

        public WarehouseCell WarehouseCell
        {
            get
            {
                return _WarehouseCell;
            }
            set
            {
                SetPropertyValue("WarehouseCell", ref _WarehouseCell, value);
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
    }
}
