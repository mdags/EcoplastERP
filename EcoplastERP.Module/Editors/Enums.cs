using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
namespace EcoplastERP.Module
{
    public enum SerieChangePeriod
    {
        [XafDisplayName("Günlük")]
        Daily = 0,
        [XafDisplayName("Aylık")]
        Monthly = 1,
        [XafDisplayName("Yıllık")]
        Yearly = 2
    }
    public enum Priority
    {
        [XafDisplayName("Düşük")]
        [ImageName("State_Priority_Low")]
        Low = 0,
        [XafDisplayName("Normal")]
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [XafDisplayName("Yüksek")]
        [ImageName("State_Priority_High")]
        High = 2
    }
    public enum InputControlPlace
    {
        [XafDisplayName("Depoda")]
        InWarehouse = 0,
        [XafDisplayName("Araç Üzerinde")]
        OnVehicle = 1
    }
    public enum Period
    {
        [XafDisplayName("1. Dönem")]
        First = 0,
        [XafDisplayName("2. Dönem")]
        Second = 1
    }
    public enum DemandStatus
    {
        [XafDisplayName("Depo Onayı Bekliyor")]
        WaitingForWarehouseConfirm = 0,
        [XafDisplayName("Yönetim Onayı Bekliyor")]
        WaitingForAdministratorConfirm = 1,
        [XafDisplayName("Satın Alma Bekliyor")]
        WaitingForPurchase = 2,
        [XafDisplayName("Sipariş Bekliyor")]
        WaitingForOrder = 3,
        [XafDisplayName("Depo Girişi Bekliyor")]
        WaitingForWarehouseEntry = 4,
        [XafDisplayName("Depoya Girdi")]
        EntredtoWarehouse = 5,
        [XafDisplayName("Tamamlandı")]
        Completed = 6,
        [XafDisplayName("Reddedildi")]
        Canceled = 9
    }
    public enum OfferStatus
    {
        [XafDisplayName("Onay Bekliyor")]
        WaitingForConfirm = 0,
        [XafDisplayName("Sipariş Bekliyor")]
        WaitingForOrder = 1,
        [XafDisplayName("Depo Girişi Bekliyor")]
        WaitingForWarehouseEntry = 2,
        [XafDisplayName("Tamamlandı")]
        Completed = 3,
        [XafDisplayName("Reddedildi")]
        Canceled = 9
    }
    public enum PurchaseOrderStatus
    {
        [XafDisplayName("TD Bekliyor")]
        WaitingForSupplierEvaluation = 0,
        [XafDisplayName("İrsaliye/Fatura Bekliyor")]
        WaitingForWaybill = 1,
        [XafDisplayName("Girişi Yapıldı")]
        Receipted = 2
    }
    public enum PurchaseWaybillStatus
    {
        [XafDisplayName("Tamamlama Bekliyor")]
        WaitingForComplete = 0,
        [XafDisplayName("Tamamlandı")]
        Completed = 1
    }
    public enum WarehouseTransferStatus
    {
        [XafDisplayName("Transfer Bekliyor")]
        Waiting = 0,
        [XafDisplayName("Transfer Edildi")]
        Completed = 1,
    }
    public enum ProductionConsumeType
    {
        [XafDisplayName("Barkodlu Sarfiyat")]
        WithBarcode = 1,
        [XafDisplayName("Barkodsuz Sarfiyat")]
        WithoutBarcode = 2,
    }
    public enum ProductStatus
    {
        [XafDisplayName("Kullanımda")]
        Used = 0,
        [XafDisplayName("Hurda")]
        Scrap = 1,
        [XafDisplayName("Tamirde")]
        Repaired = 2,
        [XafDisplayName("Kullanım Dışı")]
        OutOfUse = 3
    }
    public enum MinimumQuantityProcess
    {
        [XafDisplayName("Devam Et")]
        GoOn = 0,
        [XafDisplayName("Uyarı Ver")]
        Warn = 1,
        [XafDisplayName("İptal Et")]
        Abort = 2
    }
    public enum BellowsStatus
    {
        [XafDisplayName("Yok")]
        None = 0,
        [XafDisplayName("Yan Körük")]
        SideBellow = 1,
        [XafDisplayName("Alt Körük")]
        UnderBellow = 2,
        [XafDisplayName("Üst Körük")]
        AboveBellow = 3
    }
    public enum PrintStatus
    {
        [XafDisplayName("Baskılı")]
        Printed = 0,
        [XafDisplayName("Baskısız")]
        Unprinted = 1
    }
    public enum PrintForm
    {
        [XafDisplayName("Ters")]
        Reverse = 0,
        [XafDisplayName("Düz")]
        Flat = 1
    }
    public enum PrintDirection
    {
        [XafDisplayName("Ön")]
        Front = 0,
        [XafDisplayName("Ön/Arka")]
        FrontBack = 1,
        [XafDisplayName("İç")]
        Internal = 2,
        [XafDisplayName("Dış")]
        External = 3,
        [XafDisplayName("Çift Taraf")]
        Double = 4,
        [XafDisplayName("Tek Taraf")]
        Single = 5
    }
    public enum RollDirection
    {
        [ImageName("OutFlatA")]
        OutFlat = 1,
        [ImageName("OutReverseA")]
        OutReverse = 2,
        [ImageName("OutLeftA")]
        OutLeft = 3,
        [ImageName("OutRightA")]
        OutRight = 4,
        [ImageName("InFlatA")]
        InFlat = 5,
        [ImageName("InReverseA")]
        InReverse = 6,
        [ImageName("InLeftA")]
        InLeft = 7,
        [ImageName("InRightA")]
        InRight = 8,
        [ImageName("FotoselCift")]
        FotoselCift = 9,
        [ImageName("FotoselSag")]
        FotoselSag = 10,
        [ImageName("FotoselSol")]
        FotoselSol = 11
    }
    public enum PrintWorkStatus
    {
        [XafDisplayName("Tekrar")]
        Repeat = 0,
        [XafDisplayName("Yeni")]
        New = 1,
        [XafDisplayName("Revizyon")]
        Revision = 2,
        [XafDisplayName("Tasarım")]
        Design = 3,
        [XafDisplayName("İlk")]
        First = 4,
        [XafDisplayName("İade")]
        Return = 5
    }
    public enum Corona
    {
        [XafDisplayName("Yok")]
        Doesnt = 0,
        [XafDisplayName("Tek Taraf Full")]
        OneSideFull = 1,
        [XafDisplayName("Çift Taraf Full")]
        DubleSideFull = 2,
        [XafDisplayName("Tek Taraf Parsiyel")]
        OneSidePartial = 3,
        [XafDisplayName("Çift Taraf Parsiyel")]
        DoubleSidePartial = 4
    }
    public enum CoronaDirection
    {
        [XafDisplayName("İç")]
        In = 1,
        [XafDisplayName("Dış")]
        Out = 2,
        [XafDisplayName("İç-Dış")]
        InOut = 3
    }
    public enum PerforationStatus
    {
        [XafDisplayName("Perforesiz")]
        Imperforate = 0,
        [XafDisplayName("Perforeli")]
        Perforated = 1
    }
    public enum PunchStatus
    {
        [XafDisplayName("Deliksiz")]
        Impunched = 0,
        [XafDisplayName("Delikli")]
        Punched = 1
    }
    public enum BandStatus
    {
        [XafDisplayName("Bantsız")]
        Tapeless = 0,
        [XafDisplayName("Bantlı")]
        Banded = 1
        
    }
    public enum CapStatus
    {
        [XafDisplayName("Kapaksız")]
        Uncapped = 0,
        [XafDisplayName("Kapaklı")]
        Capped = 1
    }
    public enum HangingLocation
    {
        [XafDisplayName("Var")]
        Does = 0,
        [XafDisplayName("Yok")]
        Doesnt = 1
    }
    public enum HandleWeld
    {
        [XafDisplayName("Yok")]
        None = 0,
        [XafDisplayName("Oval")]
        Oval = 1,
        [XafDisplayName("Çavuş")]
        Sergeant = 2,
        [XafDisplayName("Böbrek")]
        Kidney = 3,
        [XafDisplayName("Çift Çene")]
        DuobleSeal = 4
    }
    public enum HandsOnType
    {
        [XafDisplayName("Yok")]
        None = 0,
        [XafDisplayName("Oval")]
        Oval = 1,
        [XafDisplayName("Böbrek")]
        Kidney = 2
    }
    public enum AdhesionType
    {
        [XafDisplayName("Yok")]
        None = 0,
        [XafDisplayName("AA")]
        AA = 1,
        [XafDisplayName("AB")]
        AB = 2,
        [XafDisplayName("BB")]
        BB = 3
    }
    public enum HandleType
    {
        [XafDisplayName("Yok")]
        None = 0,
        [XafDisplayName("El Geçme")]
        Handle = 1,
        [XafDisplayName("Yumuşak")]
        Soft = 2,
        [XafDisplayName("Crosshandle")]
        Crosshandle = 3,
        [XafDisplayName("Takviye")]
        Support = 4
    }
    public enum Laminated
    {
        [XafDisplayName("Yok")]
        No = 0,
        [XafDisplayName("Var")]
        Yes = 1
        
    }
    public enum SalesOrderType
    {
        [XafDisplayName("Standart Sipariş")]
        CustomerOrder = 0,
        [XafDisplayName("Planlama Siparişi")]
        PlanningOrder = 1,
        [XafDisplayName("İhracat Sipariş")]
        ExportingOrder = 2,
        [XafDisplayName("İhraç Kayıtlı Sipariş")]
        ExportRegisteredOrder = 3,
        [XafDisplayName("A3 Siparişi")]
        A3Order = 4,
        [XafDisplayName("Rejenere Siparişi")]
        RegeneratedOrder = 5,
        [XafDisplayName("İade Siparişi")]
        SalesReturnOrder = 6,
        [XafDisplayName("Arge Siparişi")]
        ArgeOrder = 7,
        [XafDisplayName("Numune Siparişi")]
        SampleOrder = 8
    }
    public enum SalesOrderWorkStatus
    {
        [XafDisplayName("Yeni")]
        New = 0,
        [XafDisplayName("Tekrar")]
        Repeat = 1,
        [XafDisplayName("Revizyon")]
        Revision = 2,
        [XafDisplayName("İade")]
        Return = 3
    }
    public enum ContactVehicle
    {
        [XafDisplayName("Hayır")]
        No = 0,
        [XafDisplayName("Evet")]
        Yes = 1
    }
    public enum Blockage
    {
        [XafDisplayName("Sevk Edilebilir")]
        MayShipping = 0,
        [XafDisplayName("Sevkden Önce Sor")]
        AskBeforeShipping = 1,
        [XafDisplayName("Faturadan Önce Sor")]
        AskBeforeInvoice = 2,
        [XafDisplayName("Sonraki Ay Fatura")]
        NextMonthInvoice = 3,
        [XafDisplayName("Stok Üretim")]
        StoreProduction = 4,
        [XafDisplayName("Müşteri İrsaliyesi Kesilecek")]
        CustomerWaybill = 5
    }
    public enum ShippingBlockType
    {
        [XafDisplayName("Parçalı Sevk")]
        PieceShipment = 0,
        [XafDisplayName("Komple Sevk")]
        CompleteShipment = 1
    }
    public enum DeliveryBlockType
    {
        [XafDisplayName("Hazır Sevk")]
        ReadyShipment = 0,
        [XafDisplayName("Termininde Sevk")]
        DeadlineShipment = 1
    }
    public enum SalesOrderStatus
    {
        [XafDisplayName("Planlama Onayı Bekliyor")]
        WaitingforPlanningConfirm = 0,
        [XafDisplayName("Onay Bekliyor")]
        WaitingforApproval = 100,
        [XafDisplayName("Eco1 Bekliyor")]
        WaitingforPrinting = 101,
        [XafDisplayName("Eco1 Laminasyon Bekliyor")]
        WaitingforLamination = 102,
        [XafDisplayName("Eco2 Bekliyor")]
        WaitingforFilming = 103,
        [XafDisplayName("Eco3 Bekliyor")]
        WaitingforRegenerated = 104,
        [XafDisplayName("Eco4 Bekliyor")]
        WaitingforCutting = 105,
        [XafDisplayName("Eco4 Dilme Bekliyor")]
        WaitingforSlicing = 106,
        [XafDisplayName("Eco5 Cpp Bekliyor")]
        WaitingforBalloonFilming = 107,
        [XafDisplayName("Eco5 Stretch Bekliyor")]
        WaitingforCastFilming = 108,
        [XafDisplayName("Eco5 Aktarma Bekliyor")]
        WaitingforCastTransfering = 109,
        [XafDisplayName("Eco5 Dilme Bekliyor")]
        WaitingforCastSlicing = 110,
        [XafDisplayName("Eco5 Rejenere Bekliyor")]
        WaitingforCastRegenerated = 111,
        [XafDisplayName("Eco6 Bekliyor")]
        WaitingforEco6 = 112,
        [XafDisplayName("Eco6 Konfeksiyon Bekliyor")]
        WaitingforEco6Cutting = 113,
        [XafDisplayName("Eco6 Laminasyon Bekliyor")]
        WaitingforEco6Lamination = 114,
        //[XafDisplayName("Katlama Bekliyor")]
        //WaitingforFolding = 109,
        //[XafDisplayName("Balonlu Kesim Bekliyor")]
        //WaitingforBalloonCutting = 111,
        [XafDisplayName("Üretim Bekliyor")]
        WaitingforProduction = 120,
        [XafDisplayName("Sevk Bekliyor")]
        WaitingforShipping = 130,
        [XafDisplayName("Yükleme Bekliyor")]
        WaitingforLoading = 131,
        [XafDisplayName("Sevk Edildi")]
        Completed = 200,
        [XafDisplayName("Vazgeçildi")]
        Canceled = 900
    }
    public enum TransportType
    {
        [XafDisplayName("Nakliye Bize")]
        BelongToUs = 0,
        [XafDisplayName("Nakliye Karşıya")]
        DoesNotBelongToUs = 1
    }
    public enum ShippingStatus
    {
        [XafDisplayName("Yükleme Bekliyor")]
        WaitingforLoading = 100,
        [XafDisplayName("Sevk Edildi")]
        Shipped = 200,
        [XafDisplayName("İptal")]
        Canceled = 900
    }
    public enum WaybillType
    {
        [XafDisplayName("Satın Alma Siparişi")]
        PurchaseWaybill = 1,
        [XafDisplayName("Satış Siparişi")]
        SalesWaybill = 2
    }
    public enum WorkOrderStatus
    {
        [XafDisplayName("Üretim Bekliyor")]
        WaitingforProduction = 100,
        [XafDisplayName("Üretimde")]
        ProductionStage = 101,
        [XafDisplayName("Üretim Durduruldu")]
        ProductionStopped = 102,
        [XafDisplayName("Üretim Tamamlandı")]
        ProductionComplete = 110,
        [XafDisplayName("İptal")]
        Canceled = 900
    }
    public enum WorkType
    {
        [XafDisplayName("Sabit")]
        Fixed = 0,
        [XafDisplayName("Vardiyalı")]
        Shift = 1,
        [XafDisplayName("Part-Time")]
        PartTime = 2
    }
    public enum WorkClass
    {
        [XafDisplayName("Beyaz Yaka")]
        WhiteCollar = 0,
        [XafDisplayName("Mavi Yaka")]
        BlueCollar = 1
    }
    public enum SalesReturnStatus
    {
        [XafDisplayName("Giriş Bekliyor")]
        WaitingForComplete = 0,
        [XafDisplayName("Tamamlandı")]
        Completed = 1
    }
    public enum WorkStatus
    {
        [XafDisplayName("Normal")]
        Normal = 0,
        [XafDisplayName("Emekli")]
        Retired = 1,
        [XafDisplayName("Engelli")]
        Disabled = 2
    }
    public enum Gender
    {
        [XafDisplayName("Erkek")]
        Man = 1,
        [XafDisplayName("Bayan")]
        Woman = 2
    }
    public enum MaritalStatus
    {
        [XafDisplayName("Bekar")]
        Single = 0,
        [XafDisplayName("Evli")]
        Married = 1,
        [XafDisplayName("Dul")]
        Widow = 2
    }
    public enum BloodGroup
    {
        ORhPozitive = 0,
        ORhnegative = 1,
        ARhPozitive = 2,
        ARhnegative = 3,
        BRhPozitive = 4,
        BRhNegative = 5,
        ABRhPozitive = 6,
        ABRhNegative = 7,
    }
    public enum TruckType
    {
        [XafDisplayName("Kamyonet")]
        Van = 1,
        [XafDisplayName("On Teker")]
        TenWheel = 2,
        [XafDisplayName("Kırk Ayak")]
        FortyWheel = 3,
        [XafDisplayName("Elli Ayak")]
        FiftyWheel = 4,
        [XafDisplayName("Tır")]
        Truck = 5,
        [XafDisplayName("Diğer")]
        Other = 6
    }
    public enum ShippingPlanStatus
    {
        [XafDisplayName("Sefer Bekliyor")]
        WaitingforExpedition = 0,
        [XafDisplayName("Sevk Termini Bekliyor")]
        WaitingforShippingDelivery = 1,
        [XafDisplayName("Ödeme Problemi Bekliyor")]
        WaitingforPaymentProblem = 2,
        [XafDisplayName("Müşteri Aracı Bekliyor")]
        WaitingforCustomerVehicle = 3,
        [XafDisplayName("Sonraki Ay Fatura Bekliyor")]
        WaitingforNextMonthInvoice = 4,
        [XafDisplayName("Tam Teslimat Bekliyor")]
        WaitingforCompleteDelivery = 5,
        [XafDisplayName("Stok Üretim Bekliyor")]
        WaitingforStoreProduction = 6,
        [XafDisplayName("Müşteri İrsaliyesi Bekliyor")]
        WaitingforCustomerWaybill = 7,
        [XafDisplayName("Sevk Adresi Bekliyor")]
        WaitingforShippingAddress = 8,
        [XafDisplayName("Yükleme Bekliyor")]
        WaitingforLoading = 9,
        [XafDisplayName("Evrak Kesildi")]
        Documented = 10,
        [XafDisplayName("Sefer Kapandı")]
        Completed = 11
    }
    public enum ExpeditionStatus
    {
        [XafDisplayName("Kamyon Bekliyor")]
        WaitingforTruck = 0,
        [XafDisplayName("Yükleme Bekliyor")]
        WaitingforLoading = 1,
        [XafDisplayName("Sevkiyat Onayı Bekliyor")]
        WaitingforShippingConfirm = 2,
        [XafDisplayName("Evrak Onayı Bekliyor")]
        WaitingforDocumentConfirm = 3,
        [XafDisplayName("Sefer Kapandı")]
        Completed = 4
    }
    public enum DeliveryStatus
    {
        [XafDisplayName("Yükleme Bekliyor")]
        WaitingforLoading = 0,
        [XafDisplayName("Yükleniyor")]
        Loading = 1,
        [XafDisplayName("İrsaliye Bekliyor")]
        WaitingforWaybill = 2,
        [XafDisplayName("İrsaliye Kesildi")]
        Completed = 3
    }
    public enum DeliveryBlockStatus
    {
        [XafDisplayName("Teslimat Blokajlı")]
        DeliveryBlock = 0,
        [XafDisplayName("Okutulmayan Sipariş Var")]
        MissingOrder = 1,
        [XafDisplayName("Depoda Sipariş Ürünü Mevcut")]
        OrderStore = 2,
        [XafDisplayName("Müşterinin Depoda Başka Ürünü Mevcut")]
        ContactStore = 3,
        [XafDisplayName("Evrak Kesilebilir")]
        Documentable = 4,
    }
    public enum DeliveryLoadingType
    {
        [XafDisplayName("Paletli")]
        WithPalette = 0,
        [XafDisplayName("Barkodlu")]
        WithBarcode = 1
    }
    public enum DijitalDataRequestStatus
    {
        [XafDisplayName("Grafikte")]
        WaitingForGraph = 0,
        [XafDisplayName("Beklemede")]
        Waiting = 1,
        [XafDisplayName("Çalışıldı")]
        Finished = 2,
        [XafDisplayName("İptal Edildi")]
        Canceled = 9
    }
    public enum IncomingDijital
    {
        [XafDisplayName("Cd")]
        Cd = 0,
        [XafDisplayName("Eposta")]
        Email = 1,
        [XafDisplayName("Ftp")]
        Ftp = 2,
        [XafDisplayName("Flash Bellek")]
        FlashDrive = 3,
        [XafDisplayName("Diğer")]
        Other = 9
    }
    public enum RequestedDijital
    {
        [XafDisplayName("A4 Renkli Çıktı")]
        A4 = 0,
        [XafDisplayName("Eposta")]
        Email = 1,
        [XafDisplayName("Cd")]
        Cd = 2,
        [XafDisplayName("Diğer")]
        Other = 9
    }
    public enum ReproductionStatus
    {
        [XafDisplayName("Satış Temsilcisinde")]
        InSaleDepartment = 0,
        [XafDisplayName("Grafikte")]
        InGraph = 1,
        [XafDisplayName("Beklemede")]
        Waiting = 2,
        [XafDisplayName("Üretim Bekliyor")]
        WaitingforProduction = 3,
        [XafDisplayName("Üretildi")]
        Produced = 4,
        [XafDisplayName("İptal Edildi")]
        Canceled = 5
    }
    public enum PlateStatus
    {
        [XafDisplayName("Kullanılabilir")]
        Usable = 0,
        [XafDisplayName("Arızalı")]
        Defective = 1,
        [XafDisplayName("Müşteriye İade")]
        Return = 2
    }
    public enum MaterialFilmingType
    {
        [XafDisplayName("Yaprak")]
        Leaf = 0,
        [XafDisplayName("Hortum")]
        Pipe = 1,
        [XafDisplayName("Tek Tarafı Açık")]
        OneSideOpen = 2
    }
    public enum PrintSide
    {
        [XafDisplayName("Tek")]
        Single = 0,
        [XafDisplayName("Çift")]
        Couple = 1
    }
    public enum MaintenanceDemandStatus
    {
        [XafDisplayName("Bakım Bekliyor")]
        WaitingMaintenance = 0,
        [XafDisplayName("Bakım Emri Düzenlendi")]
        Ordered = 1,
        [XafDisplayName("Bakım Yapıldı")]
        Completed = 2,
    }
    public enum MaintenanceWorkOrderStatus
    {
        [XafDisplayName("Bakım Bekliyor")]
        WaitingMaintenance = 0,
        [XafDisplayName("Bakım Başlatıldı")]
        Started = 1,
        [XafDisplayName("Bakım Tamamlama Bekliyor")]
        WaitingCompleted = 2,
        [XafDisplayName("Bakım Tamamlandı")]
        Completed = 3,
        [XafDisplayName("İptal Edildi")]
        Canceled = 9
    }
    public enum MaintenanceStopType
    {
        [XafDisplayName("Duruş Gerektiren Bakım")]
        WithStop = 0,
        [XafDisplayName("Duruş Gerektirmeyen Bakım")]
        WithoutStop = 1
    }
}