namespace EcoplastERP.Module.Controllers
{
    partial class CastTransferingWorkOrderViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem10 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem11 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem12 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.SetCastTransferingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CopyCastTransferingWorkOrderRecieptAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewCastTransferingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintCastTransferingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetCastTransferingStatusAction
            // 
            this.SetCastTransferingStatusAction.Caption = "Durumu Değiştir";
            this.SetCastTransferingStatusAction.ConfirmationMessage = null;
            this.SetCastTransferingStatusAction.Id = "SetCastTransferingStatusAction";
            this.SetCastTransferingStatusAction.ImageName = "BO_State";
            this.SetCastTransferingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetCastTransferingStatusAction.ShowItemsOnClick = true;
            this.SetCastTransferingStatusAction.ToolTip = null;
            this.SetCastTransferingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetCastTransferingStatusAction_Execute);
            // 
            // CopyCastTransferingWorkOrderRecieptAction
            // 
            this.CopyCastTransferingWorkOrderRecieptAction.Caption = "Reçete Kopyala";
            this.CopyCastTransferingWorkOrderRecieptAction.ConfirmationMessage = null;
            this.CopyCastTransferingWorkOrderRecieptAction.Id = "CopyCastTransferingWorkOrderRecieptAction";
            this.CopyCastTransferingWorkOrderRecieptAction.ImageName = "BO_List";
            choiceActionItem1.Caption = "Aynı Firmanın Aynı Stoğu";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Aynı Stoktan";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Aynı Firmanın Son Üretim Siparişi";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "Aynı Makine Yükünden";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "Farklı Firmadan";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "Aynı Siparişten";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "Farklı Makineden";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem1);
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem2);
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem3);
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem4);
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem5);
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem6);
            this.CopyCastTransferingWorkOrderRecieptAction.Items.Add(choiceActionItem7);
            this.CopyCastTransferingWorkOrderRecieptAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.CopyCastTransferingWorkOrderRecieptAction.ShowItemsOnClick = true;
            this.CopyCastTransferingWorkOrderRecieptAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CopyCastTransferingWorkOrderRecieptAction.ToolTip = null;
            this.CopyCastTransferingWorkOrderRecieptAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CopyCastTransferingWorkOrderRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CopyCastTransferingWorkOrderRecieptAction_Execute);
            // 
            // ViewCastTransferingWorkOrderAction
            // 
            this.ViewCastTransferingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewCastTransferingWorkOrderAction.ConfirmationMessage = null;
            this.ViewCastTransferingWorkOrderAction.Id = "ViewCastTransferingWorkOrderAction";
            this.ViewCastTransferingWorkOrderAction.ImageName = "Action_Printing_Preview";
            choiceActionItem8.Caption = "Aynı Firmanın Aynı Stoğu";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            choiceActionItem9.Caption = "Aynı Stoktan";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
            choiceActionItem10.Caption = "Aynı Firmanın Son Üretim Siparişi";
            choiceActionItem10.ImageName = null;
            choiceActionItem10.Shortcut = null;
            choiceActionItem10.ToolTip = null;
            choiceActionItem11.Caption = "Aynı Makine Yükünden";
            choiceActionItem11.ImageName = null;
            choiceActionItem11.Shortcut = null;
            choiceActionItem11.ToolTip = null;
            choiceActionItem12.Caption = "Aynı Siparişten";
            choiceActionItem12.ImageName = null;
            choiceActionItem12.Shortcut = null;
            choiceActionItem12.ToolTip = null;
            this.ViewCastTransferingWorkOrderAction.Items.Add(choiceActionItem8);
            this.ViewCastTransferingWorkOrderAction.Items.Add(choiceActionItem9);
            this.ViewCastTransferingWorkOrderAction.Items.Add(choiceActionItem10);
            this.ViewCastTransferingWorkOrderAction.Items.Add(choiceActionItem11);
            this.ViewCastTransferingWorkOrderAction.Items.Add(choiceActionItem12);
            this.ViewCastTransferingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewCastTransferingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewCastTransferingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewCastTransferingWorkOrderAction.ToolTip = null;
            this.ViewCastTransferingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewCastTransferingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewCastTransferingWorkOrderAction_Execute);
            // 
            // PrintCastTransferingMachineLoadAction
            // 
            this.PrintCastTransferingMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintCastTransferingMachineLoadAction.ConfirmationMessage = null;
            this.PrintCastTransferingMachineLoadAction.Id = "PrintCastTransferingMachineLoadAction";
            this.PrintCastTransferingMachineLoadAction.ImageName = "BO_Note";
            this.PrintCastTransferingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintCastTransferingMachineLoadAction.ToolTip = null;
            this.PrintCastTransferingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintCastTransferingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintCastTransferingMachineLoadAction_Execute);
            // 
            // CastTransferingWorkOrderViewController
            // 
            this.Actions.Add(this.SetCastTransferingStatusAction);
            this.Actions.Add(this.CopyCastTransferingWorkOrderRecieptAction);
            this.Actions.Add(this.ViewCastTransferingWorkOrderAction);
            this.Actions.Add(this.PrintCastTransferingMachineLoadAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.CastTransferingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetCastTransferingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CopyCastTransferingWorkOrderRecieptAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewCastTransferingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintCastTransferingMachineLoadAction;
    }
}
