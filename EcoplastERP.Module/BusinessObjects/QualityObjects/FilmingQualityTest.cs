using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Task")]
    [DefaultProperty("Barcode")]
    [NavigationItem("QualityManagement")]
    public class FilmingQualityTest : BaseObject
    {
        public FilmingQualityTest(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TestNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            TestDate = Helpers.GetSystemDate(Session);
            ShiftStart = Session.FindObject<ShiftStart>(CriteriaOperator.Parse("Active = true"));
        }
        // Fields...
        private ShiftStart _ShiftStart;
        private Reciept _Reciept;
        private Production _Production;
        private SalesOrderDetail _SalesOrderDetail;
        private FilmingWorkOrder _FilmingWorkOrder;
        private string _Barcode;
        private DateTime _TestDate;
        private int _TestNumber;

        [RuleUniqueValue]
        [RuleRequiredField]
        [Appearance("FilmingQualityTest.TestNumber", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public int TestNumber
        {
            get
            {
                return _TestNumber;
            }
            set
            {
                SetPropertyValue("TestNumber", ref _TestNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime TestDate
        {
            get
            {
                return _TestDate;
            }
            set
            {
                SetPropertyValue("TestDate", ref _TestDate, value);
            }
        }

        [RuleUniqueValue]
        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        [Association("FilmingWorkOrder-FilmingQualityTests")]
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

        public Reciept Reciept
        {
            get
            {
                return _Reciept;
            }
            set
            {
                SetPropertyValue("Reciept", ref _Reciept, value);
            }
        }

        public SalesOrderDetail SalesOrderDetail
        {
            get
            {
                return _SalesOrderDetail;
            }
            set
            {
                SetPropertyValue("SalesOrderDetail", ref _SalesOrderDetail, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
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

        [Appearance("FilmingQualityTest.ShiftStart", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public ShiftStart ShiftStart
        {
            get
            {
                return _ShiftStart;
            }
            set
            {
                SetPropertyValue("ShiftStart", ref _ShiftStart, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<FilmingQualityTestQuantitative> FilmingQualityTestQuantitatives
        {
            get { return GetCollection<FilmingQualityTestQuantitative>("FilmingQualityTestQuantitatives"); }
        }

        [Association, Aggregated]
        public XPCollection<FilmingQualityTestQualitative> FilmingQualityTestQualitatives
        {
            get { return GetCollection<FilmingQualityTestQualitative>("FilmingQualityTestQualitatives"); }
        }

        #region functions
        void GetBarcode()
        {
            if (IsLoading) return;
            if (!string.IsNullOrEmpty(Barcode))
            {
                Production production = Session.FindObject<Production>(new BinaryOperator("Barcode", Barcode));
                if (production != null)
                {
                    Production = production;
                    FilmingWorkOrder = production.FilmingWorkOrder;
                    Reciept = production.FilmingWorkOrder.Reciept;
                    SalesOrderDetail = production.SalesOrderDetail;

                    AnalysisCertificate analysisCertificate = Session.FindObject<AnalysisCertificate>(new BinaryOperator("Reciept", production.FilmingWorkOrder.Reciept));
                    if (analysisCertificate != null)
                    {
                        XPCollection<AnalysisCertificateDetail> detailList = new XPCollection<AnalysisCertificateDetail>(Session, CriteriaOperator.Parse("AnalysisCertificate = ? and MinimumThickness <= ? and MaximumThickness >= ?", analysisCertificate, production.FilmingWorkOrder.Thickness, production.FilmingWorkOrder.Thickness));
                        foreach (AnalysisCertificateDetail detail in detailList)
                        {
                            decimal certificateValue = detail.Value;
                            if (detail.Value == 0)
                            {
                                if (detail.QuantitativeAttribute.Name == "EN")
                                {
                                    certificateValue = production.FilmingWorkOrder.Width;
                                }
                                if (detail.QuantitativeAttribute.Name.Contains("AÇIK"))
                                {
                                    certificateValue = production.FilmingWorkOrder.OpenedWidth;
                                }
                                if (detail.QuantitativeAttribute.Name.Contains("KAPALI"))
                                {
                                    certificateValue = production.FilmingWorkOrder.ClosedWidth;
                                }
                                if (detail.QuantitativeAttribute.Name.Contains("KALINLIK"))
                                {
                                    certificateValue = production.FilmingWorkOrder.Thickness;
                                }
                                if (detail.QuantitativeAttribute.Name.Contains("GSM"))
                                {
                                    certificateValue = production.FilmingWorkOrder.Gsm;
                                }
                                if (detail.QuantitativeAttribute.Name.Contains("KÖRÜK"))
                                {
                                    certificateValue = Convert.ToDecimal(production.FilmingWorkOrder.Bellows);
                                }
                                if (detail.QuantitativeAttribute.Name.Contains("METRETÜL"))
                                {
                                    certificateValue = production.FilmingWorkOrder.GramMetretul;
                                }
                            }
                            FilmingQualityTestQuantitative FilmingQualityTestQuantitative = new FilmingQualityTestQuantitative(Session)
                            {
                                FilmingQualityTest = this,
                                QuantitativeAttribute = detail.QuantitativeAttribute,
                                CertificateValue = certificateValue,
                                AnalysisCertificateDetail = detail
                            };
                            this.FilmingQualityTestQuantitatives.Add(FilmingQualityTestQuantitative);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Barkod bulunamadı.");
                }
            }
        }
        #endregion
    }
}
