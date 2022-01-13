using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Category")]
    [DefaultProperty("FilmingQualityTest.Barcode")]
    [NavigationItem(false)]
    public class FilmingQualityTestQualitative : BaseObject
    {
        public FilmingQualityTestQualitative(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private QualitativeAttributeValue _QualitativeAttributeValue;
        private QualitativeAttributeDescription _QualitativeAttributeDescription;

        [Association]
        public FilmingQualityTest FilmingQualityTest { get; set; }

        [RuleRequiredField]
        public QualitativeAttributeDescription QualitativeAttributeDescription
        {
            get
            {
                return _QualitativeAttributeDescription;
            }
            set
            {
                SetPropertyValue("QualitativeAttributeDescription", ref _QualitativeAttributeDescription, value);
            }
        }

        [RuleRequiredField]
        public QualitativeAttributeValue QualitativeAttributeValue
        {
            get
            {
                return _QualitativeAttributeValue;
            }
            set
            {
                SetPropertyValue("QualitativeAttributeValue", ref _QualitativeAttributeValue, value);
            }
        }
    }
}
