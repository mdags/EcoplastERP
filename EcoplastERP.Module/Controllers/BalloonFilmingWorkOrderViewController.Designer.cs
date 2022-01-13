namespace EcoplastERP.Module.Controllers
{
    partial class BalloonFilmingWorkOrderViewController
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
            this.SetBalloonFilmingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CopyBalloonFilmingWorkOrderRecieptAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewBalloonFilmingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PrintEco5CPPMachineLoadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateBalloonFilmingRezervationAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.UpdateBalloonFilmingRecieptAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetBalloonFilmingStatusAction
            // 
            this.SetBalloonFilmingStatusAction.Caption = "Durumu Değiştir";
            this.SetBalloonFilmingStatusAction.ConfirmationMessage = null;
            this.SetBalloonFilmingStatusAction.Id = "SetBalloonFilmingStatusAction";
            this.SetBalloonFilmingStatusAction.ImageName = "BO_State";
            this.SetBalloonFilmingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetBalloonFilmingStatusAction.ShowItemsOnClick = true;
            this.SetBalloonFilmingStatusAction.ToolTip = null;
            this.SetBalloonFilmingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetBalloonFilmingStatusAction_Execute);
            // 
            // CopyBalloonFilmingWorkOrderRecieptAction
            // 
            this.CopyBalloonFilmingWorkOrderRecieptAction.Caption = "Reçete Kopyala";
            this.CopyBalloonFilmingWorkOrderRecieptAction.ConfirmationMessage = null;
            this.CopyBalloonFilmingWorkOrderRecieptAction.Id = "CopyBalloonFilmingWorkOrderRecieptAction";
            this.CopyBalloonFilmingWorkOrderRecieptAction.ImageName = "BO_List";
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
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem1);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem2);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem3);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem4);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem5);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem6);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Items.Add(choiceActionItem7);
            this.CopyBalloonFilmingWorkOrderRecieptAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.CopyBalloonFilmingWorkOrderRecieptAction.ShowItemsOnClick = true;
            this.CopyBalloonFilmingWorkOrderRecieptAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CopyBalloonFilmingWorkOrderRecieptAction.ToolTip = null;
            this.CopyBalloonFilmingWorkOrderRecieptAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CopyBalloonFilmingWorkOrderRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CopyBalloonFilmingWorkOrderRecieptAction_Execute);
            // 
            // ViewBalloonFilmingWorkOrderAction
            // 
            this.ViewBalloonFilmingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewBalloonFilmingWorkOrderAction.ConfirmationMessage = null;
            this.ViewBalloonFilmingWorkOrderAction.Id = "ViewBalloonFilmingWorkOrderAction";
            this.ViewBalloonFilmingWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            this.ViewBalloonFilmingWorkOrderAction.Items.Add(choiceActionItem8);
            this.ViewBalloonFilmingWorkOrderAction.Items.Add(choiceActionItem9);
            this.ViewBalloonFilmingWorkOrderAction.Items.Add(choiceActionItem10);
            this.ViewBalloonFilmingWorkOrderAction.Items.Add(choiceActionItem11);
            this.ViewBalloonFilmingWorkOrderAction.Items.Add(choiceActionItem12);
            this.ViewBalloonFilmingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewBalloonFilmingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewBalloonFilmingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ViewBalloonFilmingWorkOrderAction.ToolTip = null;
            this.ViewBalloonFilmingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ViewBalloonFilmingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewBalloonFilmingWorkOrderAction_Execute);
            // 
            // PrintEco5CPPMachineLoadAction
            // 
            this.PrintEco5CPPMachineLoadAction.Caption = "Makine Yükü Yazdır";
            this.PrintEco5CPPMachineLoadAction.ConfirmationMessage = null;
            this.PrintEco5CPPMachineLoadAction.Id = "PrintEco5CPPMachineLoadAction";
            this.PrintEco5CPPMachineLoadAction.ImageName = "BO_Note";
            this.PrintEco5CPPMachineLoadAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintEco5CPPMachineLoadAction.ToolTip = null;
            this.PrintEco5CPPMachineLoadAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintEco5CPPMachineLoadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintEco5CPPMachineLoadAction_Execute);
            // 
            // CreateBalloonFilmingRezervationAction
            // 
            this.CreateBalloonFilmingRezervationAction.Caption = "Rezervasyon Oluştur";
            this.CreateBalloonFilmingRezervationAction.ConfirmationMessage = null;
            this.CreateBalloonFilmingRezervationAction.Id = "CreateBalloonFilmingRezervationAction";
            this.CreateBalloonFilmingRezervationAction.ImageName = "Action_Workflow_Activate";
            this.CreateBalloonFilmingRezervationAction.ToolTip = null;
            this.CreateBalloonFilmingRezervationAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateBalloonFilmingRezervationAction_Execute);
            // 
            // UpdateBalloonFilmingRecieptAction
            // 
            this.UpdateBalloonFilmingRecieptAction.Caption = "Reçete Güncelle";
            this.UpdateBalloonFilmingRecieptAction.ConfirmationMessage = null;
            this.UpdateBalloonFilmingRecieptAction.Id = "UpdateBalloonFilmingRecieptAction";
            this.UpdateBalloonFilmingRecieptAction.ToolTip = null;
            this.UpdateBalloonFilmingRecieptAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateBalloonFilmingRecieptAction_Execute);
            // 
            // BalloonFilmingWorkOrderViewController
            // 
            this.Actions.Add(this.SetBalloonFilmingStatusAction);
            this.Actions.Add(this.CopyBalloonFilmingWorkOrderRecieptAction);
            this.Actions.Add(this.ViewBalloonFilmingWorkOrderAction);
            this.Actions.Add(this.PrintEco5CPPMachineLoadAction);
            this.Actions.Add(this.CreateBalloonFilmingRezervationAction);
            this.Actions.Add(this.UpdateBalloonFilmingRecieptAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.BalloonFilmingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetBalloonFilmingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CopyBalloonFilmingWorkOrderRecieptAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewBalloonFilmingWorkOrderAction;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintEco5CPPMachineLoadAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateBalloonFilmingRezervationAction;
        private DevExpress.ExpressApp.Actions.SimpleAction UpdateBalloonFilmingRecieptAction;
    }
}
