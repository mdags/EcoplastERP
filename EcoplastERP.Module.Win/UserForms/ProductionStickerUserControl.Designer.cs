namespace EcoplastERP.Module.Win.UserForms
{
    partial class ProductionStickerUserControl
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
            this.btnProductionSticker = new DevExpress.XtraEditors.SimpleButton();
            this.btnWastageSticker = new DevExpress.XtraEditors.SimpleButton();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnWaste = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProductionSticker
            // 
            this.btnProductionSticker.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnProductionSticker.Appearance.Options.UseFont = true;
            this.btnProductionSticker.Location = new System.Drawing.Point(55, 52);
            this.btnProductionSticker.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnProductionSticker.Name = "btnProductionSticker";
            this.btnProductionSticker.Size = new System.Drawing.Size(150, 100);
            this.btnProductionSticker.TabIndex = 0;
            this.btnProductionSticker.Text = "Üretim Teyidi";
            this.btnProductionSticker.Click += new System.EventHandler(this.btnProductionSticker_Click);
            // 
            // btnWastageSticker
            // 
            this.btnWastageSticker.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnWastageSticker.Appearance.Options.UseFont = true;
            this.btnWastageSticker.Location = new System.Drawing.Point(254, 52);
            this.btnWastageSticker.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnWastageSticker.Name = "btnWastageSticker";
            this.btnWastageSticker.Size = new System.Drawing.Size(150, 100);
            this.btnWastageSticker.TabIndex = 1;
            this.btnWastageSticker.Text = "Fire Teyidi";
            this.btnWastageSticker.Click += new System.EventHandler(this.btnWastageSticker_Click);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel1.ID = new System.Guid("a9888d86-3112-4383-b15e-b490506a491e");
            this.dockPanel1.Location = new System.Drawing.Point(530, 0);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(284, 200);
            this.dockPanel1.Size = new System.Drawing.Size(284, 516);
            this.dockPanel1.Text = "Makineler";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.gridControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(3, 21);
            this.dockPanel1_Container.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(278, 492);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(278, 492);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // btnWaste
            // 
            this.btnWaste.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnWaste.Appearance.Options.UseFont = true;
            this.btnWaste.Location = new System.Drawing.Point(443, 52);
            this.btnWaste.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnWaste.Name = "btnWaste";
            this.btnWaste.Size = new System.Drawing.Size(150, 100);
            this.btnWaste.TabIndex = 3;
            this.btnWaste.Text = "Atık Girişi";
            this.btnWaste.Click += new System.EventHandler(this.btnWaste_Click);
            // 
            // ProductionStickerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnWaste);
            this.Controls.Add(this.btnWastageSticker);
            this.Controls.Add(this.btnProductionSticker);
            this.Controls.Add(this.dockPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ProductionStickerUserControl";
            this.Size = new System.Drawing.Size(814, 516);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnProductionSticker;
        private DevExpress.XtraEditors.SimpleButton btnWastageSticker;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btnWaste;
    }
}
