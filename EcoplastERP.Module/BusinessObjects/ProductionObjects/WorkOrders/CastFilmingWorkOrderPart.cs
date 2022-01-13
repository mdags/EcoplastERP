using System.Text;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("MachinePart.Name")]
    [NavigationItem(false)]
    public class CastFilmingWorkOrderPart : BaseObject
    {
        public CastFilmingWorkOrderPart(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (CastFilmingWorkOrder != null)
            {
                decimal totalThickness = 0;
                foreach (var item in CastFilmingWorkOrder.CastFilmingWorkOrderParts)
                {
                    totalThickness += item.Thickness;
                }
                if (totalThickness > CastFilmingWorkOrder.Thickness) XtraMessageBox.Show("Kat mikronlarý film mikronundan daha büyük olmamalýdýr.", "Bilgilendirme");

                CastFilmingWorkOrder.CastFilmingWorkOrderParts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbPart = new StringBuilder();
                foreach (var item in CastFilmingWorkOrder.CastFilmingWorkOrderParts)
                {
                    sbPart.Append(string.Format("{0} : {1:n1}  /  ", item.MachinePart.Name, item.Thickness));
                }
                CastFilmingWorkOrder.PartString = sbPart.ToString();
            }
        }
        // Fields...
        private decimal _Thickness;
        private MachinePart _MachinePart;

        [Association]
        public CastFilmingWorkOrder CastFilmingWorkOrder { get; set; }

        [RuleRequiredField]
        public MachinePart MachinePart
        {
            get
            {
                return _MachinePart;
            }
            set
            {
                SetPropertyValue("MachinePart", ref _MachinePart, value);
            }
        }

        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Thickness
        {
            get
            {
                return _Thickness;
            }
            set
            {
                SetPropertyValue("Thickness", ref _Thickness, value);
            }
        }
    }
}
