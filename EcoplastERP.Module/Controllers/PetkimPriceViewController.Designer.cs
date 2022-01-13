namespace EcoplastERP.Module.Controllers
{
    partial class PetkimPriceViewController
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
            this.ImportPetkimPriceListAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportPetkimPriceListAction
            // 
            this.ImportPetkimPriceListAction.Caption = "Fiyat Listesi Aktar";
            this.ImportPetkimPriceListAction.ConfirmationMessage = null;
            this.ImportPetkimPriceListAction.Id = "ImportPetkimPriceListAction";
            this.ImportPetkimPriceListAction.ImageName = "Action_Export_ToExcel";
            this.ImportPetkimPriceListAction.ToolTip = null;
            this.ImportPetkimPriceListAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportPetkimPriceListAction_Execute);
            // 
            // PetkimPriceViewController
            // 
            this.Actions.Add(this.ImportPetkimPriceListAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.MarketingObjects.PetkimPrice);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportPetkimPriceListAction;
    }
}
