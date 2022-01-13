using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;

namespace EcoplastERP.Module.Controllers
{
    public partial class HideItemWindowController : WindowController
    {
        private ShowNavigationItemController navigationController;
        public HideItemWindowController()
        {
            InitializeComponent();

            TargetWindowType = WindowType.Main;
        }
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            navigationController = Frame.GetController<ShowNavigationItemController>();
            if (navigationController != null)
            {
                navigationController.ItemsInitialized += new EventHandler<EventArgs>(HideItemWindowController_ItemsInitialized);
            }
        }
        protected override void OnActivated()
        {
            if (navigationController != null)
            {
                navigationController.ItemsInitialized -= new EventHandler<EventArgs>(HideItemWindowController_ItemsInitialized);
                navigationController = null;
            }

            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private void HideItemWindowController_ItemsInitialized(object sender, EventArgs e)
        {
            if (!Helpers.IsUserAdministrator() && !Helpers.IsUserInRole("Rapor Kullanıcısı"))
            {
                HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "Reports");
            }
            if (!Helpers.IsUserAdministrator() && !Helpers.IsUserInRole("Sevk Bildir"))
            {
                HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "NewNotifyShipped");
            }
            if (!Helpers.IsUserAdministrator() && !Helpers.IsUserInRole("Eruslu Rapor Kullanıcısı"))
            {
                HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "ErusluReports");
            }
        }
        private void HideItemByCaption(ChoiceActionItemCollection items, string navigationItemId)
        {
            foreach (ChoiceActionItem item in items)
            {
                if (item.Id == navigationItemId)
                {
                    item.Active["InactiveForUsersRole"] = false;
                    return;
                }
                HideItemByCaption(item.Items, navigationItemId);
            }
        }
    }
}