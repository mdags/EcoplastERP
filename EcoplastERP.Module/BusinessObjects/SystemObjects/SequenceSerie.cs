using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.SystemObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Description")]
    public class SequenceSerie : BaseObject
    {
        public SequenceSerie(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private SerieChangePeriod _SerieChangePeriod;
        private Int16 _Long;
        private string _Serie;
        private string _TypeName;

        [Browsable(false)]
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                SetPropertyValue<string>("TypeName", ref _TypeName, value);
            }
        }

        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [ImmediatePostData, NonPersistent]
        public Type DataType
        {
            get
            {
                if (_TypeName != null)
                {
                    return ReflectionHelper.GetType(_TypeName);
                }
                return null;
            }
            set
            {
                string stringValue = value == null ? null : value.FullName;
                string savedObjectTypeName = TypeName;
                try
                {
                    if (stringValue != _TypeName)
                    {
                        TypeName = stringValue;
                    }
                }
                catch (Exception)
                {
                    TypeName = savedObjectTypeName;
                }
            }
        }

        [RuleRequiredField]
        public string Serie
        {
            get
            {
                return _Serie;
            }
            set
            {
                SetPropertyValue("Serie", ref _Serie, value);
            }
        }

        [RuleRequiredField]
        public Int16 Long
        {
            get
            {
                return _Long;
            }
            set
            {
                SetPropertyValue("Long", ref _Long, value);
            }
        }

        [RuleRequiredField]
        public SerieChangePeriod SerieChangePeriod
        {
            get
            {
                return _SerieChangePeriod;
            }
            set
            {
                SetPropertyValue("SerieChangePeriod", ref _SerieChangePeriod, value);
            }
        }
    }

}
