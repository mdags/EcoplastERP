using System;

namespace EcoplastERP.WebMobile.model
{
    public class UserTable
    {
        public string Oid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class WarehouseTable
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class StoreTable
    {
        public string Code { get; set; }
        public string ProductType { get; set; }
        public string ProductKindOid { get; set; }
        public string ProductKind { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
    }
    public class DemandTable
    {
        public string Oid { get; set; }
        public int DemandNumber { get; set; }
        public int LineNumber { get; set; }
        public string Product { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public string Priority { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ContactAnalysisReportTable
    {
        public string Oid { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public decimal TLTotal { get; set; }
        public decimal USDTotal { get; set; }
        public decimal EURTotal { get; set; }
    }
    public class ProductGroupReportTable
    {
        public string Oid { get; set; }
        public string Name { get; set; }
        public decimal cQuantity { get; set; }
    }
    public class ContactTable
    {
        public string Oid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class WaitingSalesOrderTable
    {
        public string ContactOrderNumber { get; set; }
        public DateTime ContactOrderDate { get; set; }
        public string SalesOrderStatus { get; set; }
        public string Contact { get; set; }
        public string ShippingContact { get; set; }
        public string OrderNumber { get; set; }
        public string WorkName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime LineDeliveryDate { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal ShippedQuantity { get; set; }
        public decimal StoreQuantity { get; set; }
        public string Unit { get; set; }
        public decimal OrderedcQuantity { get; set; }
        public decimal ShippedcQuantity { get; set; }
        public decimal StorecQuantity { get; set; }
        public string WorkStatus { get; set; }
        public string City { get; set; }
        public decimal PetkimPrice { get; set; }
        public decimal GeneralCost { get; set; }
        public decimal PetkimUnitPrice { get; set; }
        public string PetkimExtra { get; set; }
        public string TransportType { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PrintingStore { get; set; }
        public decimal CuttingStore { get; set; }
        public decimal SlicingStore { get; set; }
        public decimal LaminationStore { get; set; }
    }
    public class ProductionTable
    {
        public string StationCode { get; set; }
        public string MachineCode { get; set; }
        public string Shift { get; set; }
        public string Operator { get; set; }
        public decimal ProductionQuantity { get; set; }
        public decimal WastageQuantity { get; set; }
    }
    public class MachineLoadMainTable
    {
        public string StationCode { get; set; }
        public string TableName { get; set; }
        public string MachineCode { get; set; }
    }
    public class MachineLoadTable
    {
        public string MachineCode { get; set; }
        public Int16 SequenceNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public string WorkName { get; set; }
        public string ContactName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime LineDeliveryDate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public decimal Thickness { get; set; }
        public int Lenght { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal WorkOrderQuantity { get; set; }
        public decimal StoreQuantity { get; set; }
        public decimal ProductionQuantity { get; set; }
        public decimal RemainingProductionQuantity { get; set; }
        public decimal WastageQuantity { get; set; }
        public decimal SalesOrderQuantity { get; set; }
        public decimal RemainingTime { get; set; }
        public decimal US { get; set; }
        public decimal SS { get; set; }
    }
    public class MachineStopTable
    {
        public string GroupName { get; set; }
        public string StopName { get; set; }
        public string MachineCode { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StopTime { get; set; }
        public string Note { get; set; }
    }
    public class DepartmentTable
    {
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
    }
    public class EmployeeTable
    {
        public string DepartmentName { get; set; }
        public string NameSurname { get; set; }
        public string DepartmentPartName { get; set; }
        public string TaskName { get; set; }
        public string WorkPlaceName { get; set; }
    }
    public class ShippingTable
    {
        public string ProductKind { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Total { get; set; }
    }
}