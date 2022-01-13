namespace EcoplastERP.Module.Win.UserForms
{
    partial class NotifyShippedUserControl
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
            this.rgrpShippingWarehouse = new DevExpress.XtraEditors.RadioGroup();
            this.Contact = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit5View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProductCode = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit4View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ProductGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit3View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgrpShippingWarehouse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Contact.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit5View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit4View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit3View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.xtraTabControl1.Size = new System.Drawing.Size(1402, 926);
            this.xtraTabControl1.TabIndex = 16;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.layoutControl1);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1391, 878);
            this.xtraTabPage1.Text = "Seçim";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.rgrpShippingWarehouse);
            this.layoutControl1.Controls.Add(this.Contact);
            this.layoutControl1.Controls.Add(this.ProductCode);
            this.layoutControl1.Controls.Add(this.ProductGroup);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1039, 528, 437, 612);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1391, 878);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // rgrpShippingWarehouse
            // 
            this.rgrpShippingWarehouse.Location = new System.Drawing.Point(22, 136);
            this.rgrpShippingWarehouse.Name = "rgrpShippingWarehouse";
            this.rgrpShippingWarehouse.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Sevk Depoda Olan"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Sevk Depoda Olmayan"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Tümü")});
            this.rgrpShippingWarehouse.Properties.ItemsLayout = DevExpress.XtraEditors.RadioGroupItemsLayout.Flow;
            this.rgrpShippingWarehouse.Size = new System.Drawing.Size(1347, 720);
            this.rgrpShippingWarehouse.StyleController = this.layoutControl1;
            this.rgrpShippingWarehouse.TabIndex = 13;
            // 
            // Contact
            // 
            this.Contact.Location = new System.Drawing.Point(161, 22);
            this.Contact.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Contact.Name = "Contact";
            this.Contact.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Contact.Properties.NullText = "...Seçiniz...";
            this.Contact.Properties.View = this.searchLookUpEdit5View;
            this.Contact.Size = new System.Drawing.Size(1208, 30);
            this.Contact.StyleController = this.layoutControl1;
            this.Contact.TabIndex = 12;
            // 
            // searchLookUpEdit5View
            // 
            this.searchLookUpEdit5View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit5View.Name = "searchLookUpEdit5View";
            this.searchLookUpEdit5View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit5View.OptionsView.ShowGroupPanel = false;
            // 
            // ProductCode
            // 
            this.ProductCode.Location = new System.Drawing.Point(161, 98);
            this.ProductCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductCode.Properties.NullText = "...Seçiniz...";
            this.ProductCode.Properties.View = this.searchLookUpEdit4View;
            this.ProductCode.Size = new System.Drawing.Size(1208, 30);
            this.ProductCode.StyleController = this.layoutControl1;
            this.ProductCode.TabIndex = 10;
            // 
            // searchLookUpEdit4View
            // 
            this.searchLookUpEdit4View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit4View.Name = "searchLookUpEdit4View";
            this.searchLookUpEdit4View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit4View.OptionsView.ShowGroupPanel = false;
            // 
            // ProductGroup
            // 
            this.ProductGroup.Location = new System.Drawing.Point(161, 60);
            this.ProductGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ProductGroup.Name = "ProductGroup";
            this.ProductGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductGroup.Properties.NullText = "...Seçiniz...";
            this.ProductGroup.Properties.View = this.searchLookUpEdit3View;
            this.ProductGroup.Size = new System.Drawing.Size(1208, 30);
            this.ProductGroup.StyleController = this.layoutControl1;
            this.ProductGroup.TabIndex = 8;
            // 
            // searchLookUpEdit3View
            // 
            this.searchLookUpEdit3View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit3View.Name = "searchLookUpEdit3View";
            this.searchLookUpEdit3View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit3View.OptionsView.ShowGroupPanel = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem9,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(1391, 878);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.ProductGroup;
            this.layoutControlItem5.CustomizationFormText = "Ürün Grubu";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 38);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(1355, 38);
            this.layoutControlItem5.Text = "Ürün Grubu";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(134, 23);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.ProductCode;
            this.layoutControlItem7.CustomizationFormText = "Stok Kodu";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 76);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(1355, 38);
            this.layoutControlItem7.Text = "Stok Kodu";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(134, 23);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.Contact;
            this.layoutControlItem9.CustomizationFormText = "Firma Adı";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(1355, 38);
            this.layoutControlItem9.Text = "Malı Teslim Alan";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(134, 23);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.rgrpShippingWarehouse;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 114);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1355, 728);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gridControl1);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1391, 878);
            this.xtraTabPage2.Text = "Liste";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1391, 878);
            this.gridControl1.TabIndex = 8;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // NotifyShippedUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "NotifyShippedUserControl";
            this.Size = new System.Drawing.Size(1402, 926);
            this.Load += new System.EventHandler(this.NotifyShippedUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgrpShippingWarehouse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Contact.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit5View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit4View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit3View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        public DevExpress.XtraEditors.SearchLookUpEdit Contact;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit5View;
        public DevExpress.XtraEditors.SearchLookUpEdit ProductCode;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit4View;
        public DevExpress.XtraEditors.SearchLookUpEdit ProductGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit3View;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.RadioGroup rgrpShippingWarehouse;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}
