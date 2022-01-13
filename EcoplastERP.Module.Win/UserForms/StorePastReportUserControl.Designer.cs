namespace EcoplastERP.Module.Win.UserForms
{
    partial class StorePastReportUserControl
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
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.deStoreDate = new DevExpress.XtraEditors.DateEdit();
            this.cbProductKind = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.cbProductType = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.cbProductGroup = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.cbWarehouse = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deStoreDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStoreDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbProductKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbProductType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbProductGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbWarehouse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1425, 986);
            this.xtraTabControl1.TabIndex = 18;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.layoutControl1);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1414, 938);
            this.xtraTabPage1.Text = "Seçim";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.deStoreDate);
            this.layoutControl1.Controls.Add(this.cbProductKind);
            this.layoutControl1.Controls.Add(this.cbProductType);
            this.layoutControl1.Controls.Add(this.cbProductGroup);
            this.layoutControl1.Controls.Add(this.cbWarehouse);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(926, 200, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1414, 938);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // deStoreDate
            // 
            this.deStoreDate.EditValue = null;
            this.deStoreDate.Location = new System.Drawing.Point(127, 22);
            this.deStoreDate.Name = "deStoreDate";
            this.deStoreDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStoreDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStoreDate.Size = new System.Drawing.Size(576, 30);
            this.deStoreDate.StyleController = this.layoutControl1;
            this.deStoreDate.TabIndex = 25;
            // 
            // cbProductKind
            // 
            this.cbProductKind.Location = new System.Drawing.Point(816, 98);
            this.cbProductKind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbProductKind.Name = "cbProductKind";
            this.cbProductKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbProductKind.Size = new System.Drawing.Size(576, 30);
            this.cbProductKind.StyleController = this.layoutControl1;
            this.cbProductKind.TabIndex = 23;
            // 
            // cbProductType
            // 
            this.cbProductType.Location = new System.Drawing.Point(816, 60);
            this.cbProductType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbProductType.Name = "cbProductType";
            this.cbProductType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbProductType.Size = new System.Drawing.Size(576, 30);
            this.cbProductType.StyleController = this.layoutControl1;
            this.cbProductType.TabIndex = 22;
            // 
            // cbProductGroup
            // 
            this.cbProductGroup.Location = new System.Drawing.Point(816, 22);
            this.cbProductGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbProductGroup.Name = "cbProductGroup";
            this.cbProductGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbProductGroup.Properties.IncrementalSearch = true;
            this.cbProductGroup.Size = new System.Drawing.Size(576, 30);
            this.cbProductGroup.StyleController = this.layoutControl1;
            this.cbProductGroup.TabIndex = 20;
            // 
            // cbWarehouse
            // 
            this.cbWarehouse.Location = new System.Drawing.Point(127, 60);
            this.cbWarehouse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbWarehouse.Name = "cbWarehouse";
            this.cbWarehouse.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbWarehouse.Properties.IncrementalSearch = true;
            this.cbWarehouse.Size = new System.Drawing.Size(576, 30);
            this.cbWarehouse.StyleController = this.layoutControl1;
            this.cbWarehouse.TabIndex = 17;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem7,
            this.layoutControlItem9});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(1414, 938);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.cbWarehouse;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 38);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(689, 864);
            this.layoutControlItem4.Text = "Depo Kodu";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(100, 23);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.deStoreDate;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(689, 38);
            this.layoutControlItem3.Text = "Tarih";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(100, 23);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.cbProductGroup;
            this.layoutControlItem2.Location = new System.Drawing.Point(689, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(689, 38);
            this.layoutControlItem2.Text = "Ürün Grubu";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(100, 23);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cbProductType;
            this.layoutControlItem7.Location = new System.Drawing.Point(689, 38);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(689, 38);
            this.layoutControlItem7.Text = "Ürün Tipi";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(100, 23);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.cbProductKind;
            this.layoutControlItem9.Location = new System.Drawing.Point(689, 76);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(689, 826);
            this.layoutControlItem9.Text = "Ürün Cinsi";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(100, 23);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gridControl1);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1414, 938);
            this.xtraTabPage2.Text = "Liste";
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1414, 938);
            this.gridControl1.TabIndex = 8;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // StorePastReportUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "StorePastReportUserControl";
            this.Size = new System.Drawing.Size(1425, 986);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deStoreDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStoreDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbProductKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbProductType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbProductGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbWarehouse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbProductKind;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbProductType;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbProductGroup;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbWarehouse;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.DateEdit deStoreDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}
