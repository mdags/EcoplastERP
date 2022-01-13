using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Change_State")]
    [DefaultProperty("DocumentNumber")]
    [NavigationItem("ProductManagement")]
    public class WarehouseTransfer : BaseObject
    {
        public WarehouseTransfer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DocumentNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            DocumentDate = Helpers.GetSystemDate(Session);
        }
        // Fields...
        private string _Note;
        private DateTime _DocumentDate;
        private int _DocumentNumber;
        private WarehouseTransferStatus _Status;

        [VisibleInDetailView(false)]
        public WarehouseTransferStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                SetPropertyValue("Status", ref _Status, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public int DocumentNumber
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

        [RuleRequiredField]
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

        [Association, Aggregated]
        public XPCollection<WarehouseTransferDetail> WarehouseTransferDetails
        {
            get { return GetCollection<WarehouseTransferDetail>("WarehouseTransferDetails"); }
        }
    }
}
