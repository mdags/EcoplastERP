using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ParametersObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Demand.DemandNumber")]
    [NavigationItem("PurchaseManagement")]
    public class DemandDetail : BaseObject
    {
        public DemandDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DemandStatus = DemandStatus.WaitingForWarehouseConfirm;
            var purchaser = Session.FindObject<Purchaser>(new BinaryOperator("DefaultPurchaser", true));
            if (purchaser != null) Purchaser = purchaser;
            var employee = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
            if (employee != null) Demander = employee;
            LineTermDate = DateTime.Now.AddDays(1);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                if (LineTermDate <= DateTime.Now) throw new Exception("Satýr termin tarihi bugünün tarihinden daha büyük olmalýdýr.");
            }
        }
        // Fields...
        private PurchaseWaybillDetail _PurchaseWaybillDetail;
        private PurchaseOrderDetail _PurchaseOrderDetail;
        private OfferDetail _OfferDetail;
        private Employee _Demander;
        private bool _TakeOffer;
        private DateTime _ConfirmDate;
        private string _ConfirmedBy;
        private Purchaser _Purchaser;
        private string _LineInstruction;
        private DateTime _LineTermDate;
        private Priority _Priority;
        private decimal _Quantity;
        private Warehouse _Warehouse;
        private Unit _Unit;
        private Product _Product;
        private DemandStatus _DemandStatus;
        private int _LineNumber;
        private Demand _Demand;

        [Association]
        public Demand Demand
        {
            get { return _Demand; }
            set
            {
                Demand prevHome = _Demand;
                if (SetPropertyValue("Demand", ref _Demand, value) && !IsLoading)
                {
                    if (!IsLoading && _Demand != null)
                    {
                        int lineNumber = 10;
                        if (_Demand.DemandDetails.Count > 0)
                        {
                            _Demand.DemandDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _Demand.DemandDetails[0].LineNumber + 10;
                        }
                        LineNumber = lineNumber;
                    }
                }
            }
        }

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

        [NonCloneable]
        [VisibleInDetailView(false)]
        public DemandStatus DemandStatus
        {
            get
            {
                return _DemandStatus;
            }
            set
            {
                SetPropertyValue("DemandStatus", ref _DemandStatus, value);
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
            }
        }

        [RuleRequiredField]
        public Priority Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                SetPropertyValue("Priority", ref _Priority, value);
            }
        }

        [RuleRequiredField]
        public DateTime LineTermDate
        {
            get
            {
                return _LineTermDate;
            }
            set
            {
                SetPropertyValue("LineTermDate", ref _LineTermDate, value);
            }
        }

        public string LineInstruction
        {
            get
            {
                return _LineInstruction;
            }
            set
            {
                SetPropertyValue("LineInstruction", ref _LineInstruction, value);
            }
        }

        [VisibleInDetailView(false)]
        public Purchaser Purchaser
        {
            get
            {
                return _Purchaser;
            }
            set
            {
                SetPropertyValue("Purchaser", ref _Purchaser, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ConfirmedBy
        {
            get
            {
                return _ConfirmedBy;
            }
            set
            {
                SetPropertyValue("ConfirmedBy", ref _ConfirmedBy, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ConfirmDate
        {
            get
            {
                return _ConfirmDate;
            }
            set
            {
                SetPropertyValue("ConfirmDate", ref _ConfirmDate, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool TakeOffer
        {
            get
            {
                return _TakeOffer;
            }
            set
            {
                SetPropertyValue("TakeOffer", ref _TakeOffer, value);
            }
        }

        [VisibleInDetailView(false)]
        public Employee Demander
        {
            get
            {
                return _Demander;
            }
            set
            {
                SetPropertyValue("Demander", ref _Demander, value);
            }
        }

        [VisibleInDetailView(false)]
        public OfferDetail OfferDetail
        {
            get
            {
                return _OfferDetail;
            }
            set
            {
                SetPropertyValue("OfferDetail", ref _OfferDetail, value);
            }
        }

        [VisibleInDetailView(false)]
        public PurchaseOrderDetail PurchaseOrderDetail
        {
            get
            {
                return _PurchaseOrderDetail;
            }
            set
            {
                SetPropertyValue("PurchaseOrderDetail", ref _PurchaseOrderDetail, value);
            }
        }

        [VisibleInDetailView(false)]
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

        [Association, Aggregated]
        public XPCollection<DemandDetailPortfolio> DemandDetailPortfolios
        {
            get { return GetCollection<DemandDetailPortfolio>("DemandDetailPortfolios"); }
        }

        [Association("DemandDetail-OfferRequests")]
        public XPCollection<Contact> OfferRequests
        {
            get
            {
                return GetCollection<Contact>("OfferRequests");
            }
        }

        [Action(PredefinedCategory.View, Caption = "Satýn Almacý Ata", AutoCommit = true, ImageName = "BO_Person", ToolTip = "Bu iþlem seçili talep kaleminin satýn almacý bilgisini deðiþtirir.", TargetObjectsCriteria = "DemandStatus = 'WaitingForPurchase'", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void ChangePurchaser(AssignPurchaseParameters parameters)
        {
            if (parameters.Purchaser != null)
            {
                Purchaser = Session.FindObject<Purchaser>(new BinaryOperator("Oid", parameters.Purchaser.Oid));
            }
        }

        //[Appearance("DemandDetail.GiveWarehouseConfirm", Context = "ListView", AppearanceItemType = "Action", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Criteria = "SecuritySystem.CurrentUserName = 'Perfect'")]
        [Action(PredefinedCategory.View, Caption = "Depo Onayý Ver", AutoCommit = true, ImageName = "Action_Grant", TargetObjectsCriteria = "DemandStatus = 'WaitingForWarehouseConfirm'", ToolTip = "Bu iþlem seçili talep kalemi için depo onayý verir", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void GiveWarehouseConfirm()
        {
            DemandStatus = DemandStatus.WaitingForAdministratorConfirm;
            var employee = Session.FindObject<Employee>(new BinaryOperator("UserName", SecuritySystem.CurrentUserId));
            if (employee != null) ConfirmedBy = employee.NameSurname;
            ConfirmDate = DateTime.Now;
        }

        [Action(PredefinedCategory.View, Caption = "Yönetim Onayý Ver", AutoCommit = true, ImageName = "Action_Grant", TargetObjectsCriteria = "DemandStatus = 'WaitingForAdministratorConfirm'", ToolTip = "Bu iþlem seçili talep kalemi için yönetim onayý verir", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void GiveAdminConfirm()
        {
            DemandStatus = DemandStatus.WaitingForPurchase;
            var employee = Session.FindObject<Employee>(new BinaryOperator("UserName", SecuritySystem.CurrentUserId));
            if (employee != null) ConfirmedBy = employee.NameSurname;
            ConfirmDate = DateTime.Now;
        }

        [Action(PredefinedCategory.View, Caption = "Ýptal Et", AutoCommit = true, ImageName = "Action_Cancel", ConfirmationMessage = "Bu iþlem seçili talep kalemini iptal edecektir. Devam etmek istiyor musunuz?", ToolTip = "Bu iþlem seçili talep kalemini iptal eder.", TargetObjectsCriteria = "DemandStatus = 'WaitingForWarehouseConfirm' or DemandStatus = 'WaitingForAdministratorConfirm'", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void CancelDemand()
        {
            DemandStatus = DemandStatus.Canceled;
        }

        //[Action(PredefinedCategory.View, Caption = "Teklif Formu", AutoCommit = true, ImageName = "Action_FileAttachment_Attach", TargetObjectsCriteria = "DemandStatus = 'WaitingForPurchase'", ToolTip = "Bu iþlem seçili talebin kalemi için tedarikçilere teklif formu gönderir.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        //public void SendOfferForm()
        //{

        //}

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Unit = Product.Unit;
                Warehouse = Product.Warehouse;
            }
        }
        #endregion
    }
}
