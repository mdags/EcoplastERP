namespace EcoplastERP.Module.Controllers
{
    partial class ProductionViewController
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
            this.CreateFilmingQualityTestAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateFilmingQualityTestAction
            // 
            this.CreateFilmingQualityTestAction.Caption = "Çekim Kalite Testi Oluştur";
            this.CreateFilmingQualityTestAction.ConfirmationMessage = null;
            this.CreateFilmingQualityTestAction.Id = "CreateFilmingQualityTestAction";
            this.CreateFilmingQualityTestAction.ImageName = "BO_Task";
            this.CreateFilmingQualityTestAction.ToolTip = null;
            this.CreateFilmingQualityTestAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateFilmingQualityTestAction_Execute);
            // 
            // ProductionViewController
            // 
            this.Actions.Add(this.CreateFilmingQualityTestAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.ProductionObjects.Production);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateFilmingQualityTestAction;
    }
}
