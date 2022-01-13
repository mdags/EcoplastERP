using System;
using System.Windows.Forms;
using System.ComponentModel;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.Editors;
using EcoplastERP.Module.Controllers;

namespace EcoplastERP.Module.Win.Controllers
{
    public interface IModelWinCustomFormPathNavigationItem
    {
        [Category("Data")]
        string CustomFormTypeName { get; set; }
    }
    public class WinShowCustomFormWindowController : ShowCustomFormWindowController
    {
        protected override void ShowCustomForm(IModelNavigationItem model)
        {
            string customFormTypeName = ((IModelWinCustomFormPathNavigationItem)model).CustomFormTypeName;
            if (String.IsNullOrEmpty(customFormTypeName)) customFormTypeName = "EcoplastERP.Module.Win.Forms." + model.Id;
            Form form = DevExpress.Persistent.Base.ReflectionHelper.CreateObject(customFormTypeName) as Form;
            // Initializing a form when it is invoked from a controller.
            XpoSessionAwareControlInitializer.Initialize(form as IXpoSessionAwareControl, Application);
            //form.MdiParent = (Form)Application.MainWindow.Template;
            //form.Show();
            form.ShowDialog();
        }
    }
}
