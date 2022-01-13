using System;

namespace EcoplastERP.PTC
{
    public class WarehouseTable
    {
        public string WarehouseCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class BarcodeInfoTable
    {
        public string PaletteNumber { get; set; }
        public string OrderNumber { get; set; }
        public string ContactName { get; set; }
        public string ProductName { get; set; }
        public int BobbinCount { get; set; }
        public decimal GrossQuantity { get; set; }
        public decimal Tare { get; set; }
        public decimal NetQuantity { get; set; }
        public string OperatorName { get; set; }
        public DateTime ProductionDate { get; set; }
    }
    public class OrderInfoTable
    {
        public string ContactName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal FilmingProduction { get; set; }
        public decimal PrintingProduction { get; set; }
        public decimal CuttingProduction { get; set; }
        public decimal ShippingWarehouseQuantity { get; set; }
        public decimal LoadedQuantity { get; set; }
    }
    public class LoadingInfoTable
    {
        public int PaletteCount { get; set; }
        public int BobbinCount { get; set; }
        public decimal Quantity { get; set; }
    }
}