using System;
using System.Windows.Forms;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Debug_Step")]
    [DefaultProperty("WorkOrderNumber")]
    [NavigationItem("ProductionManagement")]
    public class ReadResource : BaseObject
    {
        public ReadResource(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            IncomingDate = Helpers.GetSystemDate(Session);
            IsConsume = false;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                Warehouse warehouse = null;
                SalesOrderDetail salesOrderDetail = null;
                var filmingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (filmingWorkOrder != null)
                {
                    warehouse = filmingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = filmingWorkOrder.SalesOrderDetail;
                }

                var castFilmingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castFilmingWorkOrder != null)
                {
                    warehouse = castFilmingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = castFilmingWorkOrder.SalesOrderDetail;
                }

                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (printingWorkOrder != null)
                {
                    warehouse = printingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = printingWorkOrder.SalesOrderDetail;
                }

                var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (laminationWorkOrder != null)
                {
                    warehouse = laminationWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = laminationWorkOrder.SalesOrderDetail;
                }

                var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (slicingWorkOrder != null)
                {
                    warehouse = slicingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = slicingWorkOrder.SalesOrderDetail;
                }

                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castTransferingWorkOrder != null)
                {
                    warehouse = castTransferingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = castTransferingWorkOrder.SalesOrderDetail;
                }

                var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castSlicingWorkOrder != null)
                {
                    warehouse = castSlicingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = castSlicingWorkOrder.SalesOrderDetail;
                }

                var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castRegeneratedWorkOrder != null)
                {
                    warehouse = castRegeneratedWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = castRegeneratedWorkOrder.SalesOrderDetail;
                }

                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (cuttingWorkOrder != null)
                {
                    warehouse = cuttingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = cuttingWorkOrder.SalesOrderDetail;
                }

                var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (foldingWorkOrder != null)
                {
                    warehouse = foldingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = foldingWorkOrder.SalesOrderDetail;
                }

                var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonCuttingWorkOrder != null)
                {
                    warehouse = balloonCuttingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = balloonCuttingWorkOrder.SalesOrderDetail;
                }

                var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6WorkOrder != null)
                {
                    warehouse = eco6WorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = eco6WorkOrder.SalesOrderDetail;
                }

                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6CuttingWorkOrder != null)
                {
                    warehouse = eco6CuttingWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = eco6CuttingWorkOrder.SalesOrderDetail;
                }

                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6LaminationWorkOrder != null)
                {
                    warehouse = eco6LaminationWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = eco6LaminationWorkOrder.SalesOrderDetail;
                }

                var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (regeneratedWorkOrder != null)
                {
                    warehouse = regeneratedWorkOrder.Station.SourceWarehouse;
                    salesOrderDetail = regeneratedWorkOrder.SalesOrderDetail;
                }

                var production = Session.FindObject<Production>(new BinaryOperator("Barcode", Barcode));
                if (production != null) Production = production;

                if (slicingWorkOrder != null)
                {
                    decimal readResourceTotal = Convert.ToDecimal(Session.Evaluate<ReadResource>(CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("Barcode = ?", Barcode)));
                    if ((readResourceTotal + Quantity) > (production.NetQuantity + 1))
                        throw new UserFriendlyException("Kaynak okutulan miktar depo miktarýný geçti. Kaynak okutulamaz.");
                }
                if (castSlicingWorkOrder != null)
                {
                    decimal readResourceTotal = Convert.ToDecimal(Session.Evaluate<ReadResource>(CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("Barcode = ?", Barcode)));
                    if ((readResourceTotal + Quantity) > production.NetQuantity)
                        throw new UserFriendlyException("Kaynak okutulan miktar depo miktarýný geçti. Kaynak okutulamaz.");
                }

                if (castFilmingWorkOrder != null)
                {
                    Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", Barcode));
                    if (store != null)
                    {
                        Movement inputMovement = new Movement(Session)
                        {
                            HeaderId = Guid.NewGuid(),
                            DocumentNumber = WorkOrderNumber,
                            DocumentDate = DateTime.Now,
                            Barcode = string.Empty,
                            SalesOrderDetail = null,
                            Product = store.Product,
                            PartyNumber = string.Empty,
                            PaletteNumber = string.Empty,
                            Warehouse = warehouse,
                            MovementType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P120")),
                            Unit = Unit,
                            Quantity = Quantity,
                            cUnit = Unit,
                            cQuantity = Quantity
                        };

                        store.Delete();
                    }
                }
                else if (filmingWorkOrder != null)
                {
                    Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", Barcode));
                    if (store != null)
                    {
                        Movement inputMovement = new Movement(Session)
                        {
                            HeaderId = Guid.NewGuid(),
                            DocumentNumber = WorkOrderNumber,
                            DocumentDate = DateTime.Now,
                            Barcode = string.Empty,
                            SalesOrderDetail = null,
                            Product = store.Product,
                            PartyNumber = string.Empty,
                            PaletteNumber = string.Empty,
                            Warehouse = warehouse,
                            MovementType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P120")),
                            Unit = Unit,
                            Quantity = Quantity,
                            cUnit = Unit,
                            cQuantity = Quantity
                        };

                        store.Delete();
                    }
                }
                else if (CastRegeneratedWorkOrder != null)
                {
                    Store store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ?", Barcode));
                    if (store != null)
                    {
                        Movement inputMovement = new Movement(Session)
                        {
                            HeaderId = Guid.NewGuid(),
                            DocumentNumber = WorkOrderNumber,
                            DocumentDate = DateTime.Now,
                            Barcode = string.Empty,
                            SalesOrderDetail = null,
                            Product = store.Product,
                            PartyNumber = string.Empty,
                            PaletteNumber = string.Empty,
                            Warehouse = warehouse,
                            MovementType = Session.FindObject<MovementType>(new BinaryOperator("Code", "P120")),
                            Unit = Unit,
                            Quantity = Quantity,
                            cUnit = Unit,
                            cQuantity = Quantity
                        };

                        store.Delete();
                    }
                }
                else
                {
                    if (salesOrderDetail.Product == production.SalesOrderDetail.Product)
                    {
                        var store = Session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse = ?", Barcode, warehouse));
                        if (store != null)
                        {
                            Unit = store.cUnit;
                            Quantity = store.cQuantity;
                            var output = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P123'"));
                            var omovement = new Movement(Session)
                            {
                                HeaderId = Guid.NewGuid(),
                                DocumentNumber = WorkOrderNumber,
                                DocumentDate = DateTime.Now,
                                Barcode = Barcode,
                                Product = store.Product,
                                PartyNumber = string.IsNullOrEmpty(store.PartyNumber) ? string.Empty : store.PartyNumber,
                                PaletteNumber = store.PaletteNumber,
                                Warehouse = store.Warehouse,
                                MovementType = output,
                                Unit = store.Unit,
                                Quantity = store.Quantity,
                                cUnit = store.cUnit,
                                cQuantity = store.cQuantity
                            };
                        }
                        else if (SlicingWorkOrder == null)
                        {
                            throw new UserFriendlyException("Barkod kaynak depoda bulunamadý. Baþka bir üretim için kaynak okutulmuþ olabilir.");
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Okutulan kaynak ve Üretim Sipariþindeki ürün farklý. Kaynak okutamazsýnýz.");
                    }
                }
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            //if (ReadResourceMovements.Count > 0)
            //{
            //    throw new UserFriendlyException("Bu kaynak hareket görmüþ silinemez.");
            //}
            var production = Session.FindObject<Production>(CriteriaOperator.Parse("Barcode = ?", Barcode));
            if (production != null)
            {
                bool returnToStore = true;
                var input = Session.FindObject<MovementType>(CriteriaOperator.Parse("Code = 'P122'"));
                Warehouse warehouse = null;
                if (FilmingWorkOrder != null) warehouse = FilmingWorkOrder.Station.SourceWarehouse;
                if (CastFilmingWorkOrder != null) warehouse = CastFilmingWorkOrder.Station.SourceWarehouse;
                if (PrintingWorkOrder != null) warehouse = PrintingWorkOrder.Station.SourceWarehouse;
                else if (LaminationWorkOrder != null) warehouse = LaminationWorkOrder.Station.SourceWarehouse;
                else if (SlicingWorkOrder != null) warehouse = SlicingWorkOrder.Station.SourceWarehouse;
                else if (CastTransferingWorkOrder != null) warehouse = CastTransferingWorkOrder.Station.SourceWarehouse;
                else if (CastSlicingWorkOrder != null) warehouse = CastSlicingWorkOrder.Station.SourceWarehouse;
                else if (CastRegeneratedWorkOrder != null) warehouse = CastRegeneratedWorkOrder.Station.SourceWarehouse;
                else if (CuttingWorkOrder != null) warehouse = CuttingWorkOrder.Station.SourceWarehouse;
                else if (FoldingWorkOrder != null) warehouse = FoldingWorkOrder.Station.SourceWarehouse;
                else if (BalloonCuttingWorkOrder != null) warehouse = BalloonCuttingWorkOrder.Station.SourceWarehouse;
                else if (Eco6WorkOrder != null) warehouse = Eco6WorkOrder.Station.SourceWarehouse;
                else if (Eco6CuttingWorkOrder != null) warehouse = Eco6CuttingWorkOrder.Station.SourceWarehouse;
                else if (Eco6LaminationWorkOrder != null) warehouse = Eco6LaminationWorkOrder.Station.SourceWarehouse;
                else if (RegeneratedWorkOrder != null) warehouse = RegeneratedWorkOrder.Station.SourceWarehouse;

                if (SlicingWorkOrder != null)
                {
                    ReadResource existsReadResource = Session.FindObject<ReadResource>(new BinaryOperator("Barcode", Barcode));
                    if (existsReadResource != null)
                    {
                        if (XtraMessageBox.Show("Okutulan diðer tüm kaynaklar silinip depoya iade edilsin mi?", "Onaylama", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            returnToStore = true;
                        else returnToStore = false;
                    }
                }
                if (CastSlicingWorkOrder != null)
                {
                    ReadResource existsReadResource = Session.FindObject<ReadResource>(new BinaryOperator("Barcode", Barcode));
                    if (existsReadResource != null)
                    {
                        if (XtraMessageBox.Show("Okutulan diðer tüm kaynaklar silinip depoya iade edilsin mi?", "Onaylama", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            returnToStore = true;
                        else returnToStore = false;
                    }
                }

                if (returnToStore)
                {
                    decimal quantity = 0, cquantity = 0;
                    decimal rrquantity = 0;
                    foreach (ReadResourceMovement movement in ReadResourceMovements)
                    {
                        rrquantity += movement.Quantity;
                    }
                    if (production.SalesOrderDetail.Unit.Code == "KG")
                    {
                        quantity = production.NetQuantity - rrquantity;
                        cquantity = production.NetQuantity - rrquantity;
                    }
                    else
                    {
                        quantity = production.cQuantity;
                        cquantity = production.NetQuantity - rrquantity;
                    }
                    var imovement = new Movement(Session)
                    {
                        HeaderId = Guid.NewGuid(),
                        DocumentNumber = WorkOrderNumber,
                        DocumentDate = DateTime.Now,
                        Barcode = Barcode,
                        SalesOrderDetail = production.SalesOrderDetail,
                        Product = production.SalesOrderDetail.Product,
                        PartyNumber = string.Empty,
                        PaletteNumber = production.ProductionPalette != null ? production.ProductionPalette.PaletteNumber : null,
                        Warehouse = warehouse,
                        MovementType = input,
                        Unit = production.SalesOrderDetail.Unit,
                        Quantity = quantity,
                        cUnit = production.SalesOrderDetail.cUnit,
                        cQuantity = cquantity
                    };

                    Session.ExecuteNonQuery(@"update ReadResource set GCRecord = 1 where GCRecord is null and Barcode = @barcode", new string[] { "@barcode" }, new object[] { Barcode });
                }
            }
        }
        // Fields...
        private bool _IsConsume;
        private DateTime _IncomingDate;
        private decimal _Quantity;
        private Unit _Unit;
        private Production _Production;
        private string _Barcode;
        private RegeneratedWorkOrder _RegeneratedWorkOrder;
        private Eco6LaminationWorkOrder _Eco6LaminationWorkOrder;
        private Eco6CuttingWorkOrder _Eco6CuttingWorkOrder;
        private Eco6WorkOrder _Eco6WorkOrder;
        private BalloonCuttingWorkOrder _BalloonCuttingWorkOrder;
        private FoldingWorkOrder _FoldingWorkOrder;
        private CuttingWorkOrder _CuttingWorkOrder;
        private CastRegeneratedWorkOrder _CastRegeneratedWorkOrder;
        private CastSlicingWorkOrder _CastSlicingWorkOrder;
        private CastTransferingWorkOrder _CastTransferingWorkOrder;
        private SlicingWorkOrder _SlicingWorkOrder;
        private LaminationWorkOrder _LaminationWorkOrder;
        private PrintingWorkOrder _PrintingWorkOrder;
        private CastFilmingWorkOrder _CastFilmingWorkOrder;
        private FilmingWorkOrder _FilmingWorkOrder;
        private string _WorkOrderNumber;

        [RuleRequiredField]
        public string WorkOrderNumber
        {
            get
            {
                return _WorkOrderNumber;
            }
            set
            {
                SetPropertyValue("WorkOrderNumber", ref _WorkOrderNumber, value);
                GetWorkOrder();
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public FilmingWorkOrder FilmingWorkOrder
        {
            get
            {
                return _FilmingWorkOrder;
            }
            set
            {
                SetPropertyValue("FilmingWorkOrder", ref _FilmingWorkOrder, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public CastFilmingWorkOrder CastFilmingWorkOrder
        {
            get
            {
                return _CastFilmingWorkOrder;
            }
            set
            {
                SetPropertyValue("CastFilmingWorkOrder", ref _CastFilmingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("PrintingWorkOrder-ReadResources")]
        public PrintingWorkOrder PrintingWorkOrder
        {
            get
            {
                return _PrintingWorkOrder;
            }
            set
            {
                SetPropertyValue("PrintingWorkOrder", ref _PrintingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("LaminationWorkOrder-ReadResources")]
        public LaminationWorkOrder LaminationWorkOrder
        {
            get
            {
                return _LaminationWorkOrder;
            }
            set
            {
                SetPropertyValue("LaminationWorkOrder", ref _LaminationWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("SlicingWorkOrder-ReadResources")]
        public SlicingWorkOrder SlicingWorkOrder
        {
            get
            {
                return _SlicingWorkOrder;
            }
            set
            {
                SetPropertyValue("SlicingWorkOrder", ref _SlicingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("CastTransferingWorkOrder-ReadResources")]
        public CastTransferingWorkOrder CastTransferingWorkOrder
        {
            get
            {
                return _CastTransferingWorkOrder;
            }
            set
            {
                SetPropertyValue("CastTransferingWorkOrder", ref _CastTransferingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("CastSlicingWorkOrder-ReadResources")]
        public CastSlicingWorkOrder CastSlicingWorkOrder
        {
            get
            {
                return _CastSlicingWorkOrder;
            }
            set
            {
                SetPropertyValue("CastSlicingWorkOrder", ref _CastSlicingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("CastRegeneratedWorkOrder-ReadResources")]
        public CastRegeneratedWorkOrder CastRegeneratedWorkOrder
        {
            get
            {
                return _CastRegeneratedWorkOrder;
            }
            set
            {
                SetPropertyValue("CastRegeneratedWorkOrder", ref _CastRegeneratedWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("CuttingWorkOrder-ReadResources")]
        public CuttingWorkOrder CuttingWorkOrder
        {
            get
            {
                return _CuttingWorkOrder;
            }
            set
            {
                SetPropertyValue("CuttingWorkOrder", ref _CuttingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FoldingWorkOrder-ReadResources")]
        public FoldingWorkOrder FoldingWorkOrder
        {
            get
            {
                return _FoldingWorkOrder;
            }
            set
            {
                SetPropertyValue("FoldingWorkOrder", ref _FoldingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("BalloonCuttingWorkOrder-ReadResources")]
        public BalloonCuttingWorkOrder BalloonCuttingWorkOrder
        {
            get
            {
                return _BalloonCuttingWorkOrder;
            }
            set
            {
                SetPropertyValue("BalloonCuttingWorkOrder", ref _BalloonCuttingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Eco6WorkOrder-ReadResources")]
        public Eco6WorkOrder Eco6WorkOrder
        {
            get
            {
                return _Eco6WorkOrder;
            }
            set
            {
                SetPropertyValue("Eco6WorkOrder", ref _Eco6WorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Eco6CuttingWorkOrder-ReadResources")]
        public Eco6CuttingWorkOrder Eco6CuttingWorkOrder
        {
            get
            {
                return _Eco6CuttingWorkOrder;
            }
            set
            {
                SetPropertyValue("Eco6CuttingWorkOrder", ref _Eco6CuttingWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Eco6LaminationWorkOrder-ReadResources")]
        public Eco6LaminationWorkOrder Eco6LaminationWorkOrder
        {
            get
            {
                return _Eco6LaminationWorkOrder;
            }
            set
            {
                SetPropertyValue("Eco6LaminationWorkOrder", ref _Eco6LaminationWorkOrder, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("RegeneratedWorkOrder-ReadResources")]
        public RegeneratedWorkOrder RegeneratedWorkOrder
        {
            get
            {
                return _RegeneratedWorkOrder;
            }
            set
            {
                SetPropertyValue("RegeneratedWorkOrder", ref _RegeneratedWorkOrder, value);
            }
        }

        [RuleRequiredField]
        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                SetPropertyValue("Barcode", ref _Barcode, value);
                GetBarcode();
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Production Production
        {
            get
            {
                return _Production;
            }
            set
            {
                SetPropertyValue("Production", ref _Production, value);
            }
        }

        [Appearance("ReadResource.Unit", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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

        [Appearance("ReadResource.Quantity", Context = "DetailView", Enabled = false, Criteria = "Not Contains([WorkOrderNumber], 'D') and Not Contains([WorkOrderNumber], 'I')")]
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

        [VisibleInDetailView(false)]
        public DateTime IncomingDate
        {
            get
            {
                return _IncomingDate;
            }
            set
            {
                SetPropertyValue("IncomingDate", ref _IncomingDate, value);
            }
        }

        [VisibleInDetailView(false)]
        public bool IsConsume
        {
            get
            {
                return _IsConsume;
            }
            set
            {
                SetPropertyValue("IsConsume", ref _IsConsume, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<ReadResourceMovement> ReadResourceMovements
        {
            get { return GetCollection<ReadResourceMovement>("ReadResourceMovements"); }
        }

        #region functions
        void GetWorkOrder()
        {
            if (IsLoading) return;
            if (!string.IsNullOrEmpty(WorkOrderNumber))
            {
                var flimingWorkOrder = Session.FindObject<FilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (flimingWorkOrder != null) FilmingWorkOrder = flimingWorkOrder;

                var castFlimingWorkOrder = Session.FindObject<CastFilmingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castFlimingWorkOrder != null) CastFilmingWorkOrder = castFlimingWorkOrder;

                var printingWorkOrder = Session.FindObject<PrintingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (printingWorkOrder != null) PrintingWorkOrder = printingWorkOrder;

                var laminationWorkOrder = Session.FindObject<LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (laminationWorkOrder != null) LaminationWorkOrder = laminationWorkOrder;

                var slicingWorkOrder = Session.FindObject<SlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (slicingWorkOrder != null) SlicingWorkOrder = slicingWorkOrder;

                var castTransferingWorkOrder = Session.FindObject<CastTransferingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castTransferingWorkOrder != null) CastTransferingWorkOrder = castTransferingWorkOrder;

                var castSlicingWorkOrder = Session.FindObject<CastSlicingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castSlicingWorkOrder != null) CastSlicingWorkOrder = castSlicingWorkOrder;

                var castRegeneratedWorkOrder = Session.FindObject<CastRegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (castRegeneratedWorkOrder != null) CastRegeneratedWorkOrder = castRegeneratedWorkOrder;

                var cuttingWorkOrder = Session.FindObject<CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (cuttingWorkOrder != null) CuttingWorkOrder = cuttingWorkOrder;

                var foldingWorkOrder = Session.FindObject<FoldingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (foldingWorkOrder != null) FoldingWorkOrder = foldingWorkOrder;

                var balloonCuttingWorkOrder = Session.FindObject<BalloonCuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (balloonCuttingWorkOrder != null) BalloonCuttingWorkOrder = balloonCuttingWorkOrder;

                var eco6WorkOrder = Session.FindObject<Eco6WorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6WorkOrder != null) Eco6WorkOrder = eco6WorkOrder;

                var eco6CuttingWorkOrder = Session.FindObject<Eco6CuttingWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6CuttingWorkOrder != null) Eco6CuttingWorkOrder = eco6CuttingWorkOrder;

                var eco6LaminationWorkOrder = Session.FindObject<Eco6LaminationWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (eco6LaminationWorkOrder != null) Eco6LaminationWorkOrder = eco6LaminationWorkOrder;

                var regeneratedWorkOrder = Session.FindObject<RegeneratedWorkOrder>(CriteriaOperator.Parse("WorkOrderNumber = ?", WorkOrderNumber));
                if (regeneratedWorkOrder != null) RegeneratedWorkOrder = regeneratedWorkOrder;
            }
        }
        void GetBarcode()
        {
            if (IsLoading) return;
            if (!string.IsNullOrEmpty(Barcode))
            {
                Store store = Session.FindObject<Store>(new BinaryOperator("Barcode", Barcode));
                if (store != null)
                {
                    Unit = store.cUnit;
                    Quantity = store.cQuantity;
                }
                else
                {
                    Production production = Session.FindObject<Production>(new BinaryOperator("Barcode", Barcode));
                    if (production != null)
                    {
                        decimal rrquantity = Convert.ToDecimal(Session.Evaluate<ReadResource>(CriteriaOperator.Parse("sum(Quantity)"), CriteriaOperator.Parse("Barcode = ?", Barcode)));
                        Unit = production.Unit;
                        Quantity = production.NetQuantity - rrquantity;
                    }
                    else throw new UserFriendlyException("Barkod depoda bulunamadý. Baþka bir üretim için kaynak okutulmuþ olabilir.");
                }
            }
        }
        #endregion
    }
}
