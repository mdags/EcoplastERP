using System.Text;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [NavigationItem(false)]
    public class BalloonFilmingWorkOrderPart : BaseObject
    {
        public BalloonFilmingWorkOrderPart(Session session)
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
            if (BalloonFilmingWorkOrder != null)
            {
                decimal totalThickness = 0;
                foreach (var item in BalloonFilmingWorkOrder.BalloonFilmingWorkOrderParts)
                {
                    totalThickness += item.Thickness;
                }
                if (totalThickness > BalloonFilmingWorkOrder.Thickness) XtraMessageBox.Show("Kat mikronlarý film mikronundan daha büyük olmamalýdýr.", "Bilgilendirme");

                BalloonFilmingWorkOrder.BalloonFilmingWorkOrderParts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbPart = new StringBuilder();
                foreach (var item in BalloonFilmingWorkOrder.BalloonFilmingWorkOrderParts)
                {
                    sbPart.Append(string.Format("{0} : {1:n1}  /  ", item.MachinePart.Name, item.Thickness));
                }
                BalloonFilmingWorkOrder.PartString = sbPart.ToString();
            }
        }
        // Fields...
        private decimal _Thickness;
        private MachinePart _MachinePart;

        [Association]
        public BalloonFilmingWorkOrder BalloonFilmingWorkOrder { get; set; }

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
