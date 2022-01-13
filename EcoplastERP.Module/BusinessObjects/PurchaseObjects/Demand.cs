using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Note")]
    [DefaultProperty("DemandNumber")]
    [NavigationItem("PurchaseManagement")]
    public class Demand : BaseObject
    {
        public Demand(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DemandDate = Helpers.GetSystemDate(Session);
            DemandNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            foreach (var item in DemandDetails)
            {
                var offerDetail = Session.FindObject<OfferDetail>(new BinaryOperator("DemandDetail", item.Oid));
                if (offerDetail != null) throw new Exception("Bu talebin kalemlerinden en az biri için teklif oluþturulmuþ. Kaydý silmek için öncelikle teklifin silinmesi gerek.");
            }
        }
        // Fields...
        private Employee _CreatedBy;
        private string _Instruction;
        private InputControlPlace _InputControlPlace;
        private Employee _InputControlPerson;
        private ExpenseType _ExpenseType;
        private ExpenseCenter _ExpenseCenter;
        private DateTime _DemandDate;
        private int _DemandNumber;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public int DemandNumber
        {
            get
            {
                return _DemandNumber;
            }
            set
            {
                SetPropertyValue("DemandNumber", ref _DemandNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime DemandDate
        {
            get
            {
                return _DemandDate;
            }
            set
            {
                SetPropertyValue("DemandDate", ref _DemandDate, value);
            }
        }

        [RuleRequiredField]
        public ExpenseCenter ExpenseCenter
        {
            get
            {
                return _ExpenseCenter;
            }
            set
            {
                SetPropertyValue("ExpenseCenter", ref _ExpenseCenter, value);
            }
        }

        [RuleRequiredField]
        public ExpenseType ExpenseType
        {
            get
            {
                return _ExpenseType;
            }
            set
            {
                SetPropertyValue("ExpenseType", ref _ExpenseType, value);
            }
        }

        [RuleRequiredField]
        public Employee InputControlPerson
        {
            get
            {
                return _InputControlPerson;
            }
            set
            {
                SetPropertyValue("InputControlPerson", ref _InputControlPerson, value);
            }
        }

        [RuleRequiredField]
        public InputControlPlace InputControlPlace
        {
            get
            {
                return _InputControlPlace;
            }
            set
            {
                SetPropertyValue("InputControlPlace", ref _InputControlPlace, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Instruction
        {
            get
            {
                return _Instruction;
            }
            set
            {
                SetPropertyValue("Instruction", ref _Instruction, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<DemandDetail> DemandDetails
        {
            get { return GetCollection<DemandDetail>("DemandDetails"); }
        }

        [Action(PredefinedCategory.View, Caption = "Tüm Satýrlara Yönetim Onayý Ver", AutoCommit = true, ImageName = "Action_Grant", ConfirmationMessage = "Bu iþlem seçili talepteki tüm kalemleri onaylayacaktýr. Devam etmek istiyor musunuz?", ToolTip = "Bu iþlem seçili telepteki tüm kalemleri onaylar.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void GiveAdminConfirmToAll()
        {
            foreach (DemandDetail item in DemandDetails)
            {
                if (item.DemandStatus == DemandStatus.WaitingForAdministratorConfirm)
                {
                    item.DemandStatus = DemandStatus.WaitingForPurchase;
                    var employee = Session.FindObject<Employee>(new BinaryOperator("UserName", SecuritySystem.CurrentUserId));
                    if (employee != null) item.ConfirmedBy = employee.NameSurname;
                    item.ConfirmDate = DateTime.Now;
                }
            }
        }

        [Action(PredefinedCategory.View, Caption = "Talebi Ýptal Et", AutoCommit = true, ImageName = "Action_Cancel", ConfirmationMessage = "Bu iþlem seçili telepteki tüm kalemleri iptal edecektir. Devam etmek istiyor musunuz?", ToolTip = "Bu iþlem seçili telepteki tüm kalemleri iptal eder.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void CancelAllDemand()
        {
            foreach (DemandDetail item in DemandDetails)
            {
                if (item.DemandStatus == DemandStatus.WaitingForAdministratorConfirm)
                    item.DemandStatus = DemandStatus.Canceled;
            }
        }
    }
}
