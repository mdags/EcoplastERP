using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.SystemObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class CriteriaController : ObjectViewController
    {
        private SingleChoiceAction filteringCriterionAction;

        public CriteriaController()
        {
            filteringCriterionAction = new SingleChoiceAction(this, "FilteringCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Execute += new SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
        {
            filteringCriterionAction.Items.Clear();
            foreach (FilteringCriterion criterion in ObjectSpace.GetObjects<FilteringCriterion>())
                if (criterion.ObjectType.IsAssignableFrom(View.ObjectTypeInfo.Type))
                {
                    filteringCriterionAction.Items.Add(new ChoiceActionItem(criterion.Description, criterion.Criterion));
                }
            if (filteringCriterionAction.Items.Count > 0)
            {
                filteringCriterionAction.Items.Add(new ChoiceActionItem("Tümü", null));
                //filteringCriterionAction.SelectedItem = (ChoiceActionItem)
            }
        }
        private void FilteringCriterionAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ((ListView)View).CollectionSource.BeginUpdateCriteria();
            ((ListView)View).CollectionSource.Criteria.Clear();
            ((ListView)View).CollectionSource.Criteria[e.SelectedChoiceActionItem.Caption] = 
                CriteriaEditorHelper.GetCriteriaOperator(e.SelectedChoiceActionItem.Data as string, View.ObjectTypeInfo.Type, ObjectSpace);
            ((ListView)View).CollectionSource.EndUpdateCriteria();
        }
    }
}
