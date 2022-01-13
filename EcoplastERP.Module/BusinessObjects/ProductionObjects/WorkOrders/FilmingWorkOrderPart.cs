using System.ComponentModel;
using System.Text;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("MachinePart.Name")]
    [NavigationItem(false)]
    public class FilmingWorkOrderPart : BaseObject
    {
        public FilmingWorkOrderPart(Session session)
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
            if (FilmingWorkOrder != null)
            {
                decimal totalThickness = 0;
                foreach (var item in FilmingWorkOrder.FilmingWorkOrderParts)
                {
                    totalThickness += item.Thickness;
                }
                if (totalThickness > FilmingWorkOrder.Thickness) XtraMessageBox.Show("Kat mikronlarý film mikronundan daha büyük olmamalýdýr.", "Bilgilendirme");

                FilmingWorkOrder.FilmingWorkOrderParts.Sorting.Add(new SortProperty("MachinePart.Name", DevExpress.Xpo.DB.SortingDirection.Ascending));
                var sbPart = new StringBuilder();
                foreach (var item in FilmingWorkOrder.FilmingWorkOrderParts)
                {
                    sbPart.Append(string.Format("{0} : {1:n1}  /  ", item.MachinePart.Name, item.Thickness));
                }
                FilmingWorkOrder.PartString = sbPart.ToString();
            }
        }
        // Fields...
        private decimal _Thickness;
        private MachinePart _MachinePart;

        [Association]
        public FilmingWorkOrder FilmingWorkOrder { get; set; }

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

        [ImmediatePostData]
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
                UpdateRecieptRate();
            }
        }

        #region functions
        void UpdateRecieptRate()
        {
            if (IsLoading) return;
            if (FilmingWorkOrder != null)
            {
                foreach (var item in FilmingWorkOrder.FilmingWorkOrderParts)
                {
                    FilmingWorkOrder.FilmingWorkOrderReciepts.Filter = new BinaryOperator("MachinePart.Name", item.MachinePart.Name);
                    foreach (var reciept in FilmingWorkOrder.FilmingWorkOrderReciepts)
                    {
                        if (item.MachinePart.Name == reciept.MachinePart.Name)
                        {
                            reciept.Rate = reciept.WorkOrderRate * (item.Thickness * 100 / Thickness) / 100;
                            reciept.Quantity = FilmingWorkOrder.Quantity * reciept.Rate / 100;
                        }
                    }
                }
                FilmingWorkOrder.FilmingWorkOrderReciepts.Filter = null;
            }
        }
        #endregion
    }
}
