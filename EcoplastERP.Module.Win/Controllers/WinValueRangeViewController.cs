using System;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Win.Editors;
using EcoplastERP.Module.Controllers;

namespace EcoplastERP.Module.Win.Controllers
{
    public class WinValueRangeViewController : ValueRangeViewController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            foreach (IntegerPropertyEditor propertyEditor in View.GetItems<IntegerPropertyEditor>())
            {
                propertyEditor.ControlCreated += new EventHandler<EventArgs>(propertyEditor_ControlCreated);
            }
        }
        void propertyEditor_ControlCreated(object sender, EventArgs e)
        {
            SetRange((IntegerPropertyEditor)sender);
        }
        protected override void SetRange(PropertyEditor editor)
        {
            if (editor.Control is SpinEdit)
            {
                SpinEdit spinEdit = (SpinEdit)editor.Control;
                spinEdit.Properties.MinValue = GetMinValue(editor.Model.ModelMember);
                spinEdit.Properties.MaxValue = GetMaxValue(editor.Model.ModelMember);
                if (!((spinEdit.Properties.MaxValue == 0) && (spinEdit.Properties.MaxValue == 0)))
                    spinEdit.TextChanged += new EventHandler(spinEdit_TextChanged);
            }
        }
        void spinEdit_TextChanged(object sender, EventArgs e)
        {
            SpinEdit spinEdit = (SpinEdit)sender;
            if (spinEdit.Value < spinEdit.Properties.MinValue)
            {
                spinEdit.Value = spinEdit.Properties.MinValue;
                spinEdit.Refresh();
            }
            if (spinEdit.Value > spinEdit.Properties.MaxValue)
            {
                spinEdit.Value = spinEdit.Properties.MaxValue;
                spinEdit.Refresh();
            }
        }
    }
}
