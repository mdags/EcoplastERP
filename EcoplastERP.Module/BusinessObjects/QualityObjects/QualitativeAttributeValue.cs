using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.QualityObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Name")]
    [NavigationItem("QualityManagement")]
    public class QualitativeAttributeValue : BaseObject
    {
        public QualitativeAttributeValue(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Name;
        private QualitativeAttributeDescription _QualitativeAttributeDescription;

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
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }
    }
}
