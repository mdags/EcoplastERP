using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.BusinessObjects.ParametersObjects
{
    [DomainComponent]
    [ImageName("Action_ParametrizedAction")]
    [NavigationItem(false)]
    public class ExpeditionParameters
    {
        private Truck _Truck;
        [ImmediatePostData]
        public Truck Truck
        {
            get
            {
                return _Truck;
            }
            set
            {
                _Truck = value;
                GetTruck();
            }
        }
        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public decimal Tare { get; set; }
        public string PermitNumber { get; set; }
        public DateTime PermitDate { get; set; }
        public decimal Payload { get; set; }
        public string KDocument { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AdministrationDate { get; set; }

        private TruckDriver _TruckDriver;
        [DataSourceCriteria("Truck = '@this.Truck'")]
        [ImmediatePostData]
        public TruckDriver TruckDriver
        {
            get
            {
                return _TruckDriver;
            }
            set
            {
                _TruckDriver = value;
                GetTruckDriver();
            }
        }
        public string NameSurname { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseDate { get; set; }
        public string CellPhone1 { get; set; }
        public string CellPhone2 { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string Address { get; set; }

        #region functions
        void GetTruck()
        {
            if (Truck != null)
            {
                PlateNumber = Truck.PlateNumber;
                Model = Truck.Model;
                Tare = Truck.Tare;
                PermitNumber = Truck.PermitNumber;
                PermitDate = Truck.PermitDate;
                Payload = Truck.Payload;
                KDocument = Truck.KDocument;
                ExpirationDate = Truck.ExpirationDate;
                AdministrationDate = Truck.AdministrationDate;
            }
        }
        void GetTruckDriver()
        {
            if (TruckDriver != null)
            {
                NameSurname = TruckDriver.NameSurname;
                LicenseNumber = TruckDriver.LicenseNumber;
                LicenseDate = TruckDriver.LicenseDate;
                CellPhone1 = TruckDriver.CellPhone;
                CellPhone2 = TruckDriver.OtherPhone;
                Address = TruckDriver.Address;
            }
        }
        #endregion
    }
}
