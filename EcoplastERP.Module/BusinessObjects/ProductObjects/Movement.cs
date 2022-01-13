using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Transition")]
    [DefaultProperty("Product.Code")]
    [NavigationItem("ProductManagement")]
    public class Movement : BaseObject
    {
        public Movement(Session session)
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
            try
            {
                if (Product == null) return;
                string partyNumber = PartyNumber == null ? partyNumber = "" : PartyNumber;
                Store store = null;
                if (!string.IsNullOrEmpty(Barcode)) store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", Barcode));
                else if (WarehouseCell == null) store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ? and PartyNumber = ? and PaletteNumber = ?", Product, Warehouse, Unit, partyNumber, PaletteNumber));
                else store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and WarehouseCell = ? and Unit = ? and PartyNumber = ? and PalletNumber = ?", Product, Warehouse, WarehouseCell, Unit, partyNumber, PaletteNumber));
                if (store != null)
                {
                    if (SecuritySystem.CurrentUser != null)
                    {
                        if (!Helpers.IsUserAdministrator())
                        {
                            if (Warehouse.CheckPermission)
                            {
                                WarehouseMovementPermission warehousePermission = Session.FindObject<WarehouseMovementPermission>(CriteriaOperator.Parse("Warehouse = ? and SecuritySystemUser.Name = ?", Warehouse, SecuritySystem.CurrentUserName));
                                if (warehousePermission == null)
                                {
                                    throw new UserFriendlyException(string.Format("{0} nolu depo için giriþ/çýkýþ yetkiniz yok.", Warehouse.Code));
                                }
                            }
                        }
                    }

                    if (MovementType.Input)
                    {
                        store.Quantity = store.Quantity + Quantity;
                        store.cQuantity = store.cQuantity + cQuantity;
                    }
                    else if (MovementType.Output)
                    {
                        if (!string.IsNullOrEmpty(Barcode)) store.Delete();
                        else
                        {
                            if (store.Quantity - Quantity > 0)
                            {
                                //var minQuan = Session.FindObject<MinimumQuantity>(CriteriaOperator.Parse("Product = ? and Warehouse = ?", Product, Warehouse));
                                //if (minQuan != null)
                                //{
                                //    if (minQuan.Process == MinimumQuantityProcess.Warn)
                                //    {
                                //        if (store.Quantity - Quantity < minQuan.Quantity)
                                //            XtraMessageBox.Show("Stok bakiyesi minimum stok miktarýnýn altýna düþecek.", "Bilgilendirme...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    }
                                //    else if (minQuan.Process == MinimumQuantityProcess.Abort)
                                //    {
                                //        if (store.Quantity - Quantity < minQuan.Quantity)
                                //            throw new UserFriendlyException("Stok bakiyesi minimum stok miktarýnýn altýna düþecek. Bu iþlemi gerçekleþtiremezsiniz.");
                                //    }
                                //}
                                store.Quantity = store.Quantity - Quantity;
                                store.cQuantity = store.cQuantity - cQuantity;
                            }
                            else if (store.Quantity - Quantity == 0) store.Delete();
                            else if (store.Quantity - Quantity < 0) throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", Product.Name));
                        }
                    }
                }
                else
                {
                    if (MovementType.Input)
                    {
                        store = new Store(Session);
                        store.Product = Product;
                        store.Barcode = Barcode;
                        store.SalesOrderDetail = SalesOrderDetail;
                        store.PartyNumber = partyNumber;
                        store.PaletteNumber = PaletteNumber;
                        store.Warehouse = Warehouse;
                        store.WarehouseCell = WarehouseCell;
                        store.Unit = Unit;
                        store.Quantity = Quantity;
                        store.cUnit = cUnit;
                        store.cQuantity = cQuantity;
                    }
                    else throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", Product.Name));
                }
            }
            catch
            {
                throw new UserFriendlyException(string.Format("{0} Kaynak depoda stok bakiyesi yetersiz !...", Product.Name));
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            try
            {
                if (Product == null) return;
                string partyNumber = PartyNumber == null ? partyNumber = "" : PartyNumber;
                Store store = null;
                if (!string.IsNullOrEmpty(Barcode)) store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", Barcode));
                else if (WarehouseCell == null) store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and Unit = ? and PartyNumber = ? and PaletteNumber = ?", Product, Warehouse, Unit, partyNumber, PaletteNumber));
                else store = Session.FindObject<Store>(CriteriaOperator.Parse("Product = ? and Warehouse = ? and WarehouseCell = ? and Unit = ? and PartyNumber = ? and PalletNumber = ?", Product, Warehouse, WarehouseCell, Unit, partyNumber, PaletteNumber));
                if (store != null)
                {
                    if (store.Quantity - Quantity > 0)
                    {
                        store.Quantity = store.Quantity - Quantity;
                        store.cQuantity = store.cQuantity - cQuantity;
                    }
                    else store.Delete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        //// Fields...
        private PurchaseWaybillDetail _PurchaseWaybillDetail;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private WarehouseCell _WarehouseCell;
        private Warehouse _Warehouse;
        private string _PaletteNumber;
        private string _PartyNumber;
        private Product _Product;
        private SalesOrderDetail _SalesOrderDetail;
        private string _Barcode;
        private MovementType _MovementType;
        private DateTime _DocumentDate;
        private string _DocumentNumber;
        private Guid _HeaderId;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Guid HeaderId
        {
            get
            {
                return _HeaderId;
            }
            set
            {
                SetPropertyValue("HeaderId", ref _HeaderId, value);
            }
        }

        public string DocumentNumber
        {
            get
            {
                return _DocumentNumber;
            }
            set
            {
                SetPropertyValue("DocumentNumber", ref _DocumentNumber, value);
            }
        }

        public DateTime DocumentDate
        {
            get
            {
                return _DocumentDate;
            }
            set
            {
                SetPropertyValue("DocumentDate", ref _DocumentDate, value);
            }
        }

        [RuleRequiredField]
        public MovementType MovementType
        {
            get
            {
                return _MovementType;
            }
            set
            {
                SetPropertyValue("MovementType", ref _MovementType, value);
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

        [RuleRequiredField]
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

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public PurchaseWaybillDetail PurchaseWaybillDetail
        {
            get
            {
                return _PurchaseWaybillDetail;
            }
            set
            {
                SetPropertyValue("PurchaseWaybillDetail", ref _PurchaseWaybillDetail, value);
            }
        }

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                var product = Session.FindObject<Product>(CriteriaOperator.Parse("Oid = ?", Product.Oid));
                if (product != null)
                {
                    Warehouse = product.Warehouse;
                    Unit = Product.Unit;
                }
            }
        }
        #endregion
    }
}
