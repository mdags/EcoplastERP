using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Win.UserForms
{
    public partial class DailyProductionAnalysisUserControl : XtraUserControl, IComplexControl
    {
        public DailyProductionAnalysisUserControl()
        {
            InitializeComponent();
        }

        private XafApplication application;
        private IObjectSpace objectSpace;
        void IComplexControl.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
            this.objectSpace = objectSpace;

            cbStation.Properties.DataSource = new XPCollection<Station>(((XPObjectSpace)objectSpace).Session, null, new SortProperty("Code", DevExpress.Xpo.DB.SortingDirection.Ascending));
            cbStation.Properties.DisplayMember = "Code";
            cbStation.Properties.ValueMember = "Code";

            txtYear.Text = DateTime.Now.Year.ToString();
            cmbMonth.EditValue = DateTime.Now.Month.ToString();
        }

        public void RefreshGrid()
        {
            Cursor.Current = Cursors.WaitCursor;

            DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, @"select datepart(day, ProductionDate) as [Gün], S.Code as [İstasyon], M.Code as [Makine], sum(GrossQuantity) as [Brüt Miktar], sum(NetQuantity) as [Net Miktar] from Production P inner join Machine M on M.Oid = P.Machine inner join Station S on S.Oid = M.Station where P.GCRecord is null and year(ProductionDate) = @year and month(ProductionDate) = @month and S.Code = @stationCode group by datepart(day, ProductionDate), S.Code, M.Name order by datepart(day, ProductionDate), S.Code, M.Name", new SqlParameter("@year", txtYear.Text), new SqlParameter("@month", cmbMonth.EditValue), new SqlParameter("@stationCode", cbStation.EditValue)).Tables[0];
            pivotGridControl1.DataSource = dt;

            PivotGridField fieldStation = new PivotGridField("İstasyon", PivotArea.RowArea);
            PivotGridField fieldMachine = new PivotGridField("Makine", PivotArea.RowArea);

            PivotGridField fieldDay = new PivotGridField("Gün", PivotArea.ColumnArea);
            fieldDay.CellFormat.FormatType = FormatType.Numeric;

            PivotGridField fieldGross = new PivotGridField("Brüt Miktar", PivotArea.DataArea);
            fieldGross.CellFormat.FormatType = FormatType.Numeric;
            fieldGross.CellFormat.FormatString = "n2";
            fieldGross.ValueFormat.FormatType = FormatType.Numeric;
            fieldGross.ValueFormat.FormatString = "n2";
            PivotGridField fieldNet = new PivotGridField("Net Miktar", PivotArea.DataArea);
            fieldNet.CellFormat.FormatType = FormatType.Numeric;
            fieldNet.CellFormat.FormatString = "n2";
            fieldNet.ValueFormat.FormatType = FormatType.Numeric;
            fieldNet.ValueFormat.FormatString = "n2";

            pivotGridControl1.Fields.AddRange(new PivotGridField[] { fieldStation, fieldMachine, fieldDay, fieldGross, fieldNet });

            if (xtraTabControl1.SelectedTabPage == xtraTabPage1) xtraTabControl1.SelectedTabPage = xtraTabPage2;
            Cursor.Current = Cursors.Default;
        }
    }
}
