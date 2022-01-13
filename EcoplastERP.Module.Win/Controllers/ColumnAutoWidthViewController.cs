using System.ComponentModel;
using DevExpress.XtraGrid;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid.Views.Grid;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class ColumnAutoWidthViewController : ViewController, IModelExtender
    {
        public const string EnabledKeyColumnAutoWidth = "ColumnAutoWidthListViewController";
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            if (View.Control.GetType() == typeof(GridControl))
            {
                GridControl gridControl = (GridControl)View.Control;
                GridView gridView = (GridView)gridControl.FocusedView;
                gridView.OptionsView.ColumnAutoWidth = ((IModelColumnAutoWidth)View.Model).ColumnAutoWidth;
                gridView.BestFitColumns();
            }
        }
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelViews, IModelDefaultColumnAutoWidth>();
            extenders.Add<IModelListView, IModelColumnAutoWidth>();
        }
    }
    public interface IModelDefaultColumnAutoWidth : IModelNode
    {
        [DefaultValue(true)]
        bool DefaultColumnAutoWidthListView { get; set; }
    }
    public interface IModelColumnAutoWidth : IModelNode
    {
        bool ColumnAutoWidth { get; set; }
    }
    [DomainLogic(typeof(IModelColumnAutoWidth))]
    public static class ModelColumnAutoWidthLogic
    {
        public static bool Get_ColumnAutoWidth(IModelColumnAutoWidth columnAutoWidth)
        {
            IModelDefaultColumnAutoWidth defaultColumnAutoWidthListView = columnAutoWidth.Parent as IModelDefaultColumnAutoWidth;
            if (defaultColumnAutoWidthListView != null)
            {
                return defaultColumnAutoWidthListView.DefaultColumnAutoWidthListView;
            }
            return true;
        }
    }
}
