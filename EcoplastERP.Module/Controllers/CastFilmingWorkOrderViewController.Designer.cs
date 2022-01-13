namespace EcoplastERP.Module.Controllers
{
    partial class CastFilmingWorkOrderViewController
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
            this.SetCastFilmingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CopyCastFilmingWorkOrderRecieptAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewCastFilmingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintEco5StretchMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateCastFilmingRezervationAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateCastFilmingRecieptAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetCastFilmingStatusAction
            // 
            this.SetCastFilmingStatusAction.Caption = "Durumu Değiştir";
            this.SetCastFilmingStatusAction.ConfirmationMessage = null;
            this.SetCastFilmingStatusAction.Id = "SetCastFilmingStatusAction";
            this.SetCastFilmingStatusAction.ImageName = "BO_State";
            this.SetCastFilmingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetCastFilmingStatusAction.ShowItemsOnClick = true;
            this.SetCastFilmingStatusAction.ToolTip = null;
            this.SetCastFilmingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetCastFilmingStatusAction_Execute);
            // 
            // CopyCastFilmingWorkOrderRecieptAction
            // 
            this.CopyCastFilmingWorkOrderRecieptAction.Caption = "Reçete Kopyala";
            this.CopyCastFilmingWorkOrderRecieptAction.ConfirmationMessage = null;
            this.CopyCastFilmingWorkOrderRecieptAction.Id = "CopyCastFilmingWorkOrderRecieptAction";
            this.CopyCastFilmingWorkOrderRecieptAction.ImageName = "BO_List";
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
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem1);
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem2);
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem3);
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem4);
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem5);
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem6);
            this.CopyCastFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem7);
            this.CopyCastFilmingWorkOrderRecieptAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.CopyCastFilmingWorkOrderRecieptAction.ShowItemsOnClick = true;
            this.CopyCastFilmingWorkOrderRecieptAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CopyCastFilmingWorkOrderRecieptAction.ToolTip = null;
            this.CopyCastFilmingWorkOrderRecieptAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CopyCastFilmingWorkOrderRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CopyCastFilmingWorkOrderRecieptAction_Execute);
            // 
            // ViewCastFilmingWorkOrderAction
            // 
            this.ViewCastFilmingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewCastFilmingWorkOrderAction.ConfirmationMessage = null;
            this.ViewCastFilmingWorkOrderAction.Id = "ViewCastFilmingWorkOrderAction";
            this.ViewCastFilmingWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            this.ViewCastFilmingWorkOrderAction.Items.Add(choiceActionItem8);
            this.ViewCastFilmingWorkOrderAction.Items.Add(choiceActionItem9);
            this.ViewCastFilmingWorkOrderAction.Items.Add(choiceActionItem10);
            this.ViewCastFilmingWorkOrderAction.Items.Add(choiceActionItem11);
            this.ViewCastFilmingWorkOrderAction.Items.Add(choiceActionItem12);
            this.ViewCastFilmingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewCastFilmingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewCastFilmingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewCastFilmingWorkOrderAction.ToolTip = null;
            this.ViewCastFilmingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewCastFilmingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewCastFilmingWorkOrderAction_Execute);
            // 
            // PrintEco5StretchMachineLoadAction
            // 
            this.PrintEco5StretchMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintEco5StretchMachineLoadAction.ConfirmationMessage = null;
            this.PrintEco5StretchMachineLoadAction.Id = "PrintEco5StretchMachineLoadAction";
            this.PrintEco5StretchMachineLoadAction.ImageName = "BO_Note";
            this.PrintEco5StretchMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintEco5StretchMachineLoadAction.ToolTip = null;
            this.PrintEco5StretchMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintEco5StretchMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintEco5StretchMachineLoadAction_Execute);
            // 
            // CreateCastFilmingRezervationAction
            // 
            this.CreateCastFilmingRezervationAction.Caption = "Rezervasyon Oluştur";
            this.CreateCastFilmingRezervationAction.ConfirmationMessage = null;
            this.CreateCastFilmingRezervationAction.Id = "CreateCastFilmingRezervationAction";
            this.CreateCastFilmingRezervationAction.ImageName = "Action_Workflow_Activate";
            this.CreateCastFilmingRezervationAction.ToolTip = null;
            this.CreateCastFilmingRezervationAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateCastFilmingRezervationAction_Execute);
            // 
            // UpdateCastFilmingRecieptAction
            // 
            this.UpdateCastFilmingRecieptAction.Caption = "Reçete Güncelle";
            this.UpdateCastFilmingRecieptAction.ConfirmationMessage = null;
            this.UpdateCastFilmingRecieptAction.Id = "a1b2ae11-01dd-4932-904a-9d2d5c784090";
            this.UpdateCastFilmingRecieptAction.ImageName = "UpdateCastFilmingRecieptAction";
            this.UpdateCastFilmingRecieptAction.ToolTip = null;
            this.UpdateCastFilmingRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateCastFilmingRecieptAction_Execute);
            // 
            // CastFilmingWorkOrderViewController
            // 
            this.Actions.Add(this.SetCastFilmingStatusAction);
            this.Actions.Add(this.CopyCastFilmingWorkOrderRecieptAction);
            this.Actions.Add(this.ViewCastFilmingWorkOrderAction);
            this.Actions.Add(this.PrintEco5StretchMachineLoadAction);
            this.Actions.Add(this.CreateCastFilmingRezervationAction);
            this.Actions.Add(this.UpdateCastFilmingRecieptAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.CastFilmingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetCastFilmingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CopyCastFilmingWorkOrderRecieptAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewCastFilmingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintEco5StretchMachineLoadAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateCastFilmingRezervationAction;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateCastFilmingRecieptAction;
    }
}
