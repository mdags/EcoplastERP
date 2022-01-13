using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class Warehouse : BaseObject
    {
        public Warehouse(Session session)
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
            if (LoadingWarehouse)
            {
                var loadingWarehouse = Session.FindObject<Warehouse>(new BinaryOperator("LoadingWarehouse", true));
                if (loadingWarehouse != null) loadingWarehouse.LoadingWarehouse = false;
            }
        }
        // Fields...
        private bool _CheckPermission;
        private bool _LoadingWarehouse;
        private bool _ShippingWarehouse;
        private decimal _PriceCapacity;
        private decimal _Capacity;
        private Employee _Manager;
        private WarehouseGroupCode _WarehouseGroupCode;
        private string _Name;
        private string _Code;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }
        
        public WarehouseGroupCode WarehouseGroupCode
        {
            get
            {
                return _WarehouseGroupCode;
            }
            set
            {
                SetPropertyValue("WarehouseGroupCode", ref _WarehouseGroupCode, value);
            }
        }
        
        public Employee Manager
        {
            get
            {
                return _Manager;
            }
            set
            {
                SetPropertyValue("Manager", ref _Manager, value);
            }
        }

        public decimal Capacity
        {
            get
            {
                return _Capacity;
            }
            set
            {
                SetPropertyValue("Capacity", ref _Capacity, value);
            }
        }

        public decimal PriceCapacity
        {
            get
            {
                return _PriceCapacity;
            }
            set
            {
                SetPropertyValue("PriceCapacity", ref _PriceCapacity, value);
            }
        }

        public bool ShippingWarehouse
        {
            get
            {
                return _ShippingWarehouse;
            }
            set
            {
                SetPropertyValue("ShippingWarehouse", ref _ShippingWarehouse, value);
            }
        }

        public bool LoadingWarehouse
        {
            get
            {
                return _LoadingWarehouse;
            }
            set
            {
                SetPropertyValue("LoadingWarehouse", ref _LoadingWarehouse, value);
            }
        }

        public bool CheckPermission
        {
            get
            {
                return _CheckPermission;
            }
            set
            {
                SetPropertyValue("CheckPermission", ref _CheckPermission, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<WarehouseMovementPermission> WarehouseMovementPermissions
        {
            get { return GetCollection<WarehouseMovementPermission>("WarehouseMovementPermissions"); }
        }

        private XPCollection<AuditDataItemPersistent> changeHistory;
        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                {
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return changeHistory;
            }
        }
    }
}
