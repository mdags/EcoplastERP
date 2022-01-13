using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_User")]
    [DefaultProperty("NameSurname")]
    [NavigationItem(false)]
    public class TruckDriver : BaseObject
    {
        public TruckDriver(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Address;
        private string _OtherPhone;
        private string _CellPhone;
        private DateTime _LicenseDate;
        private string _LicenseNumber;
        private string _NameSurname;

        [Association]
        public Truck Truck { get; set; }

        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NameSurname
        {
            get
            {
                return _NameSurname;
            }
            set
            {
                SetPropertyValue("NameSurname", ref _NameSurname, value);
            }
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LicenseNumber
        {
            get
            {
                return _LicenseNumber;
            }
            set
            {
                SetPropertyValue("LicenseNumber", ref _LicenseNumber, value);
            }
        }
        
        public DateTime LicenseDate
        {
            get
            {
                return _LicenseDate;
            }
            set
            {
                SetPropertyValue("LicenseDate", ref _LicenseDate, value);
            }
        }

        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CellPhone
        {
            get
            {
                return _CellPhone;
            }
            set
            {
                SetPropertyValue("CellPhone", ref _CellPhone, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OtherPhone
        {
            get
            {
                return _OtherPhone;
            }
            set
            {
                SetPropertyValue("OtherPhone", ref _OtherPhone, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                SetPropertyValue("Address", ref _Address, value);
            }
        }
    }
}
