using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.PlateArchiveObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("PlateNumber")]
    [NavigationItem("PlateArchiveManagement")]
    public class Plate : BaseObject
    {
        public Plate(Session session)
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
                if (PlateNumber == null) Reproduction.ReproductionStatus = ReproductionStatus.Produced;
                else Reproduction.ReproductionStatus = ReproductionStatus.WaitingforProduction;
            }
            else if (PlateNumber != null) Reproduction.ReproductionStatus = ReproductionStatus.Produced;
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            Reproduction.ReproductionStatus = ReproductionStatus.Waiting;
        }
        // Fields...
        private string _DefectiveInstructions;
        private string _RollerLength;
        private Int16 _BandCount;
        private Int16 _ColorCount;
        private int _UsedRawMaterials;
        private RawMaterialType _RawMaterialType;
        private string _WorkSize;
        private string _WorkName;
        private Contact _Contact;
        private Reproduction _Reproduction;
        private string _PlateNumber;
        private PlateStatus _PlateStatus;

        public PlateStatus PlateStatus
        {
            get
            {
                return _PlateStatus;
            }
            set
            {
                SetPropertyValue("PlateStatus", ref _PlateStatus, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public string PlateNumber
        {
            get
            {
                return _PlateNumber;
            }
            set
            {
                SetPropertyValue("PlateNumber", ref _PlateNumber, value);
            }
        }

        [Association("Reproduction-Plates")]
        [DataSourceCriteria("ReproductionStatus != 'InSaleDepartment' and ReproductionStatus != 'Produced'")]
        public Reproduction Reproduction
        {
            get
            {
                return _Reproduction;
            }
            set
            {
                SetPropertyValue("Reproduction", ref _Reproduction, value);
                GetReproduction();
            }
        }

        [RuleRequiredField]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
            }
        }

        [RuleRequiredField]
        public string WorkName
        {
            get
            {
                return _WorkName;
            }
            set
            {
                SetPropertyValue("WorkName", ref _WorkName, value);
            }
        }

        [RuleRequiredField]
        public string WorkSize
        {
            get
            {
                return _WorkSize;
            }
            set
            {
                SetPropertyValue("WorkSize", ref _WorkSize, value);
            }
        }

        public RawMaterialType RawMaterialType
        {
            get
            {
                return _RawMaterialType;
            }
            set
            {
                SetPropertyValue("RawMaterialType", ref _RawMaterialType, value);
            }
        }

        public int UsedRawMaterials
        {
            get
            {
                return _UsedRawMaterials;
            }
            set
            {
                SetPropertyValue("UsedRawMaterials", ref _UsedRawMaterials, value);
            }
        }

        [ModelDefault("MinValue", "0"), ModelDefault("MaxValue", "100")]
        public Int16 ColorCount
        {
            get
            {
                return _ColorCount;
            }
            set
            {
                SetPropertyValue("ColorCount", ref _ColorCount, value);
            }
        }

        [ModelDefault("MinValue", "0"), ModelDefault("MaxValue", "100")]
        public Int16 BandCount
        {
            get
            {
                return _BandCount;
            }
            set
            {
                SetPropertyValue("BandCount", ref _BandCount, value);
            }
        }

        public string RollerLength
        {
            get
            {
                return _RollerLength;
            }
            set
            {
                SetPropertyValue("RollerLength", ref _RollerLength, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string DefectiveInstructions
        {
            get
            {
                return _DefectiveInstructions;
            }
            set
            {
                SetPropertyValue("DefectiveInstructions", ref _DefectiveInstructions, value);
            }
        }

        #region functions
        void GetReproduction()
        {
            if (IsLoading) return;
            if (Reproduction != null)
            {
                var reproduction = Session.FindObject<Reproduction>(CriteriaOperator.Parse("Oid = ?", Reproduction.Oid));
                if (reproduction != null)
                {
                    Contact = reproduction.Contact;
                    WorkName = reproduction.WorkName;
                    WorkSize = reproduction.WorkSize;
                    BandCount = reproduction.BandCount;
                    RollerLength = reproduction.RollerLength;
                }
            }
        }
        #endregion
    }
}
