namespace EcoplastERP.Module.Controllers
{
    partial class FilmingWorkOrderViewController
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
            this.SetFilmingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CopyFilmingWorkOrderRecieptAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewFilmingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintFilmingMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateFilmingRezervationAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateFilmingRecieptAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetFilmingStatusAction
            // 
            this.SetFilmingStatusAction.Caption = "Set Filming Status";
            this.SetFilmingStatusAction.ConfirmationMessage = null;
            this.SetFilmingStatusAction.Id = "SetFilmingStatusAction";
            this.SetFilmingStatusAction.ImageName = "BO_State";
            this.SetFilmingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetFilmingStatusAction.ShowItemsOnClick = true;
            this.SetFilmingStatusAction.ToolTip = null;
            this.SetFilmingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetStatusAction_Execute);
            // 
            // CopyFilmingWorkOrderRecieptAction
            // 
            this.CopyFilmingWorkOrderRecieptAction.Caption = "Copy Filming Work Order Reciept";
            this.CopyFilmingWorkOrderRecieptAction.ConfirmationMessage = null;
            this.CopyFilmingWorkOrderRecieptAction.Id = "CopyFilmingWorkOrderRecieptAction";
            this.CopyFilmingWorkOrderRecieptAction.ImageName = "BO_List";
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
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem1);
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem2);
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem3);
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem4);
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem5);
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem6);
            this.CopyFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem7);
            this.CopyFilmingWorkOrderRecieptAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.CopyFilmingWorkOrderRecieptAction.ShowItemsOnClick = true;
            this.CopyFilmingWorkOrderRecieptAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CopyFilmingWorkOrderRecieptAction.ToolTip = null;
            this.CopyFilmingWorkOrderRecieptAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CopyFilmingWorkOrderRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CopyFilmingWorkOrderRecieptAction_Execute);
            // 
            // ViewFilmingWorkOrderAction
            // 
            this.ViewFilmingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewFilmingWorkOrderAction.ConfirmationMessage = null;
            this.ViewFilmingWorkOrderAction.Id = "ViewFilmingWorkOrderAction";
            this.ViewFilmingWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            this.ViewFilmingWorkOrderAction.Items.Add(choiceActionItem8);
            this.ViewFilmingWorkOrderAction.Items.Add(choiceActionItem9);
            this.ViewFilmingWorkOrderAction.Items.Add(choiceActionItem10);
            this.ViewFilmingWorkOrderAction.Items.Add(choiceActionItem11);
            this.ViewFilmingWorkOrderAction.Items.Add(choiceActionItem12);
            this.ViewFilmingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewFilmingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewFilmingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewFilmingWorkOrderAction.ToolTip = null;
            this.ViewFilmingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewFilmingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewFilmingWorkOrderAction_Execute);
            // 
            // PrintFilmingMachineLoadAction
            // 
            this.PrintFilmingMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintFilmingMachineLoadAction.ConfirmationMessage = null;
            this.PrintFilmingMachineLoadAction.Id = "PrintFilmingMachineLoadAction";
            this.PrintFilmingMachineLoadAction.ImageName = "BO_Note";
            this.PrintFilmingMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintFilmingMachineLoadAction.ToolTip = null;
            this.PrintFilmingMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintFilmingMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintFilmingMachineLoadAction_Execute);
            // 
            // CreateFilmingRezervationAction
            // 
            this.CreateFilmingRezervationAction.Caption = "Rezervasyon Oluştur";
            this.CreateFilmingRezervationAction.ConfirmationMessage = null;
            this.CreateFilmingRezervationAction.Id = "CreateFilmingRezervationAction";
            this.CreateFilmingRezervationAction.ImageName = "Action_Workflow_Activate";
            this.CreateFilmingRezervationAction.ToolTip = null;
            this.CreateFilmingRezervationAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateFilmingRezervationAction_Execute);
            // 
            // UpdateFilmingRecieptAction
            // 
            this.UpdateFilmingRecieptAction.Caption = "Reçete Güncelle";
            this.UpdateFilmingRecieptAction.ConfirmationMessage = null;
            this.UpdateFilmingRecieptAction.Id = "UpdateFilmingRecieptAction";
            this.UpdateFilmingRecieptAction.ToolTip = null;
            this.UpdateFilmingRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateFilmingRecieptAction_Execute);
            // 
            // FilmingWorkOrderViewController
            // 
            this.Actions.Add(this.SetFilmingStatusAction);
            this.Actions.Add(this.CopyFilmingWorkOrderRecieptAction);
            this.Actions.Add(this.ViewFilmingWorkOrderAction);
            this.Actions.Add(this.PrintFilmingMachineLoadAction);
            this.Actions.Add(this.CreateFilmingRezervationAction);
            this.Actions.Add(this.UpdateFilmingRecieptAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.FilmingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetFilmingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CopyFilmingWorkOrderRecieptAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewFilmingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintFilmingMachineLoadAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateFilmingRezervationAction;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateFilmingRecieptAction;
    }
}
