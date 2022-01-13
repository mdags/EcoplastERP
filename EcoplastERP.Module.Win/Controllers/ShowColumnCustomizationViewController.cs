using System.ComponentModel;
using DevExpress.XtraGrid;
using DevExpress.Utils.Menu;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Localization;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class ShowColumnCustomizationViewController : ViewController, IModelExtender
    {
        public const string EnabledKeyShowColumnCustomization = "ShowColumnCustomizationViewController";
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            if (View.Control.GetType() == typeof(GridControl))
            {
                GridControl gridControl = (GridControl)View.Control;
                GridView gridView = (GridView)gridControl.FocusedView;
                gridView.PopupMenuShowing += GridView_PopupMenuShowing;
            }
        }
        private void GridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == GridMenuType.Column)
            {
                DXMenuItem miCustomize = GetItemByStringId(e.Menu, GridStringId.MenuColumnColumnCustomization);
                if (miCustomize != null)
                {
                    miCustomize.Visible = ((IModelShowColumnCustomization)View.Model).ShowColumnCustomization;
                }
            }
        }
        private DXMenuItem GetItemByStringId(DXPopupMenu menu, GridStringId id)
        {
            foreach (DXMenuItem item in menu.Items)
                if (item.Caption == GridLocalizer.Active.GetLocalizedString(id))
                    return item;
            return null;
        }
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelViews, IModelDefaultShowColumnCustomization>();
            extenders.Add<IModelListView, IModelShowColumnCustomization>();
        }
    }
    public interface IModelDefaultShowColumnCustomization : IModelNode
    {
        [DefaultValue(true)]
        bool DefaultShowColumnCustomizationListView { get; set; }
    }
    public interface IModelShowColumnCustomization : IModelNode
    {
        bool ShowColumnCustomization { get; set; }
    }
    [DomainLogic(typeof(IModelShowColumnCustomization))]
    public static class ModelShowColumnCustomizationLogic
    {
        public static bool Get_ShowColumnCustomization(IModelShowColumnCustomization showColumnCustomization)
        {
            IModelDefaultShowColumnCustomization defaultShowColumnCustomizationListView = showColumnCustomization.Parent as IModelDefaultShowColumnCustomization;
            if (defaultShowColumnCustomizationListView != null)
            {
                return defaultShowColumnCustomizationListView.DefaultShowColumnCustomizationListView;
            }
            return true;
        }
    }
}
