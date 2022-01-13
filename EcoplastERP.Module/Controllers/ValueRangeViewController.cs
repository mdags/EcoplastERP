using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;

namespace EcoplastERP.Module.Controllers
{
    public class ValueRangeViewController : ViewController<DetailView>, IModelExtender
    {
        protected decimal GetMinValue(IModelMember member)
        {
            return ((IModelMemberRange)member).MinValue;
        }
        protected decimal GetMaxValue(IModelMember member)
        {
            return ((IModelMemberRange)member).MaxValue;
        }
        void IModelExtender.ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelMember, IModelMemberRange>();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
            {
                foreach (PropertyEditor editor in ((DetailView)View).GetItems<PropertyEditor>())
                {
                    SetRange(editor);
                }
            }
        }
        protected virtual void SetRange(PropertyEditor editor) { }
    }

    public interface IModelMemberRange : IModelNode
    {
        decimal MinValue { get; set; }
        decimal MaxValue { get; set; }
    }
}
