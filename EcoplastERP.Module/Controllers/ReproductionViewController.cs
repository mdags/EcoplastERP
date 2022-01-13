using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.PlateArchiveObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class ReproductionViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public ReproductionViewController()
        {
            InitializeComponent();

            SetReproductionStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(Reproduction), "ReproductionStatus"), null);
            SetReproductionStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(ReproductionStatus));
        }

        private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)
        {
            foreach (object current in Enum.GetValues(enumType))
            {
                EnumDescriptor ed = new EnumDescriptor(enumType);
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current) { ImageName = ImageLoader.Instance.GetEnumValueImageName(current) };
                parentItem.Items.Add(item);
            }
        }

        private void SetReproductionStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    Reproduction objInNewObjectSpace = (Reproduction)objectSpace.GetObject(obj);
                    objInNewObjectSpace.ReproductionStatus = (ReproductionStatus)e.SelectedChoiceActionItem.Data;
                }
            }
            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if (View is ListView)
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }

        private void PrintReporoductionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //ReportData _reportData = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", "Reprodüksiyon Formu"));
            //if (_reportData != null)
            //{
            //    IObjectSpace objectSpace = View is DetailView ? Application.CreateObjectSpace() : View.ObjectSpace;
            //    ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            //    foreach (Object obj in objectsToProcess)
            //    {
            //        Reproduction objInNewObjectSpace = (Reproduction)objectSpace.GetObject(obj);
            //        ReportServiceController _controller = ((ActionBase)sender).Controller.Frame.GetController<ReportServiceController>();
            //        _controller.ShowPreview(_reportData, CriteriaOperator.TryParse(String.Format("[ReproductionNumber] = '{0}'", objInNewObjectSpace.ReproductionNumber)));
            //    }
            //}
        }
    }
}
