namespace EcoplastERP.Module.Win.UserForms
{
    partial class FindPaletteUserControl
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtNet = new DevExpress.XtraEditors.TextEdit();
            this.txtTare = new DevExpress.XtraEditors.TextEdit();
            this.txtGross = new DevExpress.XtraEditors.TextEdit();
            this.txtLastWeight = new DevExpress.XtraEditors.TextEdit();
            this.txtPaletteNumber = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTare.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGross.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastWeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaletteNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridControl1);
            this.layoutControl1.Controls.Add(this.txtNet);
            this.layoutControl1.Controls.Add(this.txtTare);
            this.layoutControl1.Controls.Add(this.txtGross);
            this.layoutControl1.Controls.Add(this.txtLastWeight);
            this.layoutControl1.Controls.Add(this.txtPaletteNumber);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1388, 936);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.gridControl1.Location = new System.Drawing.Point(22, 136);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1344, 778);
            this.gridControl1.TabIndex = 17;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // txtNet
            // 
            this.txtNet.Location = new System.Drawing.Point(791, 98);
            this.txtNet.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtNet.Name = "txtNet";
            this.txtNet.Properties.ReadOnly = true;
            this.txtNet.Size = new System.Drawing.Size(575, 30);
            this.txtNet.StyleController = this.layoutControl1;
            this.txtNet.TabIndex = 8;
            this.txtNet.TabStop = false;
            // 
            // txtTare
            // 
            this.txtTare.Location = new System.Drawing.Point(791, 60);
            this.txtTare.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtTare.Name = "txtTare";
            this.txtTare.Properties.ReadOnly = true;
            this.txtTare.Size = new System.Drawing.Size(575, 30);
            this.txtTare.StyleController = this.layoutControl1;
            this.txtTare.TabIndex = 7;
            this.txtTare.TabStop = false;
            // 
            // txtGross
            // 
            this.txtGross.Location = new System.Drawing.Point(116, 98);
            this.txtGross.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtGross.Name = "txtGross";
            this.txtGross.Properties.ReadOnly = true;
            this.txtGross.Size = new System.Drawing.Size(573, 30);
            this.txtGross.StyleController = this.layoutControl1;
            this.txtGross.TabIndex = 6;
            this.txtGross.TabStop = false;
            // 
            // txtLastWeight
            // 
            this.txtLastWeight.Location = new System.Drawing.Point(116, 60);
            this.txtLastWeight.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtLastWeight.Name = "txtLastWeight";
            this.txtLastWeight.Properties.ReadOnly = true;
            this.txtLastWeight.Size = new System.Drawing.Size(573, 30);
            this.txtLastWeight.StyleController = this.layoutControl1;
            this.txtLastWeight.TabIndex = 5;
            this.txtLastWeight.TabStop = false;
            // 
            // txtPaletteNumber
            // 
            this.txtPaletteNumber.Location = new System.Drawing.Point(116, 22);
            this.txtPaletteNumber.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtPaletteNumber.Name = "txtPaletteNumber";
            this.txtPaletteNumber.Size = new System.Drawing.Size(1250, 30);
            this.txtPaletteNumber.StyleController = this.layoutControl1;
            this.txtPaletteNumber.TabIndex = 4;
            this.txtPaletteNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaletteNumber_KeyDown);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(1388, 936);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtPaletteNumber;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1352, 38);
            this.layoutControlItem1.Text = "Palet No";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(89, 23);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtLastWeight;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 38);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(675, 38);
            this.layoutControlItem2.Text = "Son Ağırlık";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(89, 23);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtGross;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 76);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(675, 38);
            this.layoutControlItem3.Text = "Brüt";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(89, 23);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtTare;
            this.layoutControlItem4.Location = new System.Drawing.Point(675, 38);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(677, 38);
            this.layoutControlItem4.Text = "Dara";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(89, 23);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.txtNet;
            this.layoutControlItem5.Location = new System.Drawing.Point(675, 76);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(677, 38);
            this.layoutControlItem5.Text = "Net";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(89, 23);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.gridControl1;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 114);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(1352, 786);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // FindPaletteUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FindPaletteUserControl";
            this.Size = new System.Drawing.Size(1388, 936);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTare.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGross.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastWeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaletteNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtNet;
        private DevExpress.XtraEditors.TextEdit txtTare;
        private DevExpress.XtraEditors.TextEdit txtGross;
        private DevExpress.XtraEditors.TextEdit txtLastWeight;
        private DevExpress.XtraEditors.TextEdit txtPaletteNumber;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}
