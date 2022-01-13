using System;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using DevExpress.ExpressApp.SystemModule;

namespace EcoplastERP.Module.Controllers
{
    public abstract class ShowCustomFormWindowController : WindowController
    {
        private ShowNavigationItemController navigationController;
        public ShowCustomFormWindowController()
        {
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            navigationController = Frame.GetController<ShowNavigationItemController>();
            if (navigationController != null)
                navigationController.CustomShowNavigationItem += navigationController_CustomShowNavigationItem;
        }
        protected override void OnDeactivated()
        {
            if (navigationController != null)
                navigationController.CustomShowNavigationItem -= navigationController_CustomShowNavigationItem;
            base.OnDeactivated();
        }
        private void navigationController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco1MachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco1LaminationMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco2MachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco3MachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco4MachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco4SlicingMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco5CPPMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco5RegeneratedMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco5SlicingMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco5StretchMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco5TransferingMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco6CuttingMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco6LaminationMachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Eco6MachineLoadForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "SalesOrderAnalysisForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "ProductionStickerForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "WastageStickerForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "FindBarcodeForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "FindPaletteForm")
            {
                ShowCustomForm(e.ActionArguments.SelectedChoiceActionItem.Model as IModelNavigationItem);
                e.Handled = true;
            }
        }
        protected abstract void ShowCustomForm(IModelNavigationItem model);
    }
}
