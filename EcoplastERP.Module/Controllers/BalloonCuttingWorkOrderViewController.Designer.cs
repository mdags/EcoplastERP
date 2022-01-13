namespace EcoplastERP.Module.Controllers
{
    partial class BalloonCuttingWorkOrderViewController
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
            this.SetBalloonCuttingStatusAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewBalloonCuttingWorkOrderAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // SetBalloonCuttingStatusAction
            // 
            this.SetBalloonCuttingStatusAction.Caption = "Durumu Değiştir";
            this.SetBalloonCuttingStatusAction.ConfirmationMessage = null;
            this.SetBalloonCuttingStatusAction.Id = "SetBalloonCuttingStatusAction";
            this.SetBalloonCuttingStatusAction.ImageName = "BO_State";
            this.SetBalloonCuttingStatusAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.SetBalloonCuttingStatusAction.ShowItemsOnClick = true;
            this.SetBalloonCuttingStatusAction.ToolTip = null;
            this.SetBalloonCuttingStatusAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SetBalloonCuttingStatusAction_Execute);
            // 
            // ViewBalloonCuttingWorkOrderAction
            // 
            this.ViewBalloonCuttingWorkOrderAction.Caption = "İncele/Değiştir";
            this.ViewBalloonCuttingWorkOrderAction.ConfirmationMessage = null;
            this.ViewBalloonCuttingWorkOrderAction.Id = "ViewBalloonCuttingWorkOrderAction";
            this.ViewBalloonCuttingWorkOrderAction.ImageName = "Action_Printing_Preview";
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
            choiceActionItem5.Caption = "Aynı Siparişten";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.ViewBalloonCuttingWorkOrderAction.Items.Add(choiceActionItem1);
            this.ViewBalloonCuttingWorkOrderAction.Items.Add(choiceActionItem2);
            this.ViewBalloonCuttingWorkOrderAction.Items.Add(choiceActionItem3);
            this.ViewBalloonCuttingWorkOrderAction.Items.Add(choiceActionItem4);
            this.ViewBalloonCuttingWorkOrderAction.Items.Add(choiceActionItem5);
            this.ViewBalloonCuttingWorkOrderAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ViewBalloonCuttingWorkOrderAction.ShowItemsOnClick = true;
            this.ViewBalloonCuttingWorkOrderAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewBalloonCuttingWorkOrderAction.ToolTip = null;
            this.ViewBalloonCuttingWorkOrderAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewBalloonCuttingWorkOrderAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ViewBalloonCuttingWorkOrderAction_Execute);
            // 
            // BalloonCuttingWorkOrderViewController
            // 
            this.Actions.Add(this.SetBalloonCuttingStatusAction);
            this.Actions.Add(this.ViewBalloonCuttingWorkOrderAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.BalloonCuttingWorkOrder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SetBalloonCuttingStatusAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ViewBalloonCuttingWorkOrderAction;
    }
}
