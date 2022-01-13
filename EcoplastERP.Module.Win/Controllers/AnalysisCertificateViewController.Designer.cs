namespace EcoplastERP.Module.Win.Controllers
{
    partial class AnalysisCertificateViewController
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
            this.ImportCertificateAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportCertificateAction
            // 
            this.ImportCertificateAction.Caption = "Sertifika İçeri Aktar";
            this.ImportCertificateAction.ConfirmationMessage = null;
            this.ImportCertificateAction.Id = "ImportCertificateAction";
            this.ImportCertificateAction.ImageName = "Action_Export_ToExcel";
            this.ImportCertificateAction.ToolTip = null;
            this.ImportCertificateAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportCertificateAction_Execute);
            // 
            // AnalysisCertificateViewController
            // 
            this.Actions.Add(this.ImportCertificateAction);
            this.TargetObjectType = typeof(EcoplastERP.Module.BusinessObjects.QualityObjects.AnalysisCertificate);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ImportCertificateAction;
    }
}
