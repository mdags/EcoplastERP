using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using EcoplastERP.Module.Win.UserForms;
using DevExpress.Persistent.AuditTrail;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ParametersObjects;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class StoreTransferFromOldSystemViewController : ViewController
    {
        public StoreTransferFromOldSystemViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            Frame.GetController<RefreshController>().RefreshAction.Execute += RefreshAction_Execute;
        }

        private void RefreshAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreTransferFromOldSystemUserControl userControl = cvi.Control as StoreTransferFromOldSystemUserControl;
                userControl.RefreshGrid(userControl.txtWarehouse.Text, userControl.txtOrderNumber.Text, userControl.txtPletteNumber.Text);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void TransferStoreAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                StoreTransferFromOldSystemUserControl userControl = cvi.Control as StoreTransferFromOldSystemUserControl;

                TransferStoreParameters obj = objectSpace.CreateObject<TransferStoreParameters>();
                DetailView detailView = Application.CreateDetailView(objectSpace, obj);
                detailView.Tag = userControl.gridView1.GetFocusedRowCellValue("Palet No").ToString();
                detailView.ViewEditMode = ViewEditMode.Edit;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.Context = TemplateContext.PopupWindow;
                e.ShowViewParameters.CreatedView = detailView;
                DialogController dialogController = Application.CreateController<DialogController>();
                dialogController.AcceptAction.Execute += AcceptAction_Execute;
                e.ShowViewParameters.Controllers.Add(dialogController);
            }
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            if ((e.CurrentObject as TransferStoreParameters).SalesOrderDetail == null)
            {
                throw new UserFriendlyException("Sipariş Kalemi seçiniz.");
            }
            if ((e.CurrentObject as TransferStoreParameters).Warehouse == null)
            {
                throw new UserFriendlyException("Depo seçiniz.");
            }

            ControlViewItem cvi = ((DashboardView)View).Items[0] as ControlViewItem;
            if (cvi != null)
            {
                AuditTrailService.Instance.EndSessionAudit(((XPObjectSpace)objectSpace).Session);
                StoreTransferFromOldSystemUserControl userControl = cvi.Control as StoreTransferFromOldSystemUserControl;
                Guid headerId = Guid.NewGuid();
                SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as TransferStoreParameters).SalesOrderDetail.Oid));
                Warehouse warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", (e.CurrentObject as TransferStoreParameters).Warehouse.Oid));
                MovementType input = objectSpace.FindObject<MovementType>(new BinaryOperator("Code", "P120"));
                int[] selectedRows = userControl.gridView1.GetSelectedRows();
                for (int i = 0; i < selectedRows.Length; i++)
                {
                    int rowHandle = selectedRows[i];

                    if (!string.IsNullOrEmpty(userControl.gridView1.GetRowCellValue(rowHandle, "Palet No").ToString()))
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(string.Format("SELECT TOP 1 SUM(P.GROSSAMOUNT) AS GrossQuantity, SUM(P.NETAMOUNT) as NetQuantity, MAX(PP.TARE) as Tare, MAX(PP.LAMOUNT) as LastWeight, (MAX(PP.LAMOUNT) - (SUM(P.GROSSAMOUNT) + MAX(PP.TARE))) as ConsumeMaterialWeight FROM PRODUCTS P INNER JOIN PALETTES PP ON PP.PALETTEID = P.PALETTENO WHERE P.ACTIVE = 1 AND P.BARCODE LIKE 'M%' AND P.PALETTENO = {0}", userControl.gridView1.GetRowCellValue(rowHandle, "Palet No").ToString()), SqlProvider.ConnectionStringOldERP);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ((XPObjectSpace)objectSpace).Session.ExecuteNonQuery(@"insert into ProductionPalette(Oid, Active, PaletteNumber, Tare, GrossWeight, NetWeight, LastWeight, ConsumeMaterialWeight) values(NEWID(), 0, @paletteNumber, @tare, @grossWeight, @netWeight, @lastWeight, @consumeMaterialWeight)", new string[] { "@paletteNumber", "@tare", "@grossWeight", "@netWeight", "@lastWeight", "@consumeMaterialWeight" }, new object[] { userControl.gridView1.GetRowCellValue(rowHandle, "Palet No").ToString(), Convert.ToDecimal(dt.Rows[0]["Tare"]), Convert.ToDecimal(dt.Rows[0]["GrossQuantity"]), Convert.ToDecimal(dt.Rows[0]["NetQuantity"]), Convert.ToDecimal(dt.Rows[0]["LastWeight"]), Convert.ToDecimal(dt.Rows[0]["ConsumeMaterialWeight"]) });
                        }

                        dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionStringOldERP, CommandType.Text, @"SELECT PALETTENO as [Palet No], BARCODE AS [Barkod], AMOUNT AS [Miktar], UNIT as [Birim], (SELECT NETAMOUNT FROM PRODUCTS WHERE BARCODE = STORES.BARCODE) as [Net Miktar] FROM STORES WHERE PALETTENO = @paletteNumber AND BARCODE LIKE 'M%'", new SqlParameter("@paletteNumber", userControl.gridView1.GetRowCellValue(rowHandle, "Palet No"))).Tables[0];
                        foreach (DataRow row in dt.Rows)
                        {
                            decimal quantity = Convert.ToDecimal(row["Miktar"]);
                            if (salesOrderDetail.Unit.Code == "AD")
                            {
                                quantity = salesOrderDetail.Product.OuterPackingInPiece;
                            }
                            else if (salesOrderDetail.Unit.Code == "RL")
                            {
                                quantity = salesOrderDetail.Product.OuterPackingRollCount;
                            }
                            else if (salesOrderDetail.Unit.Code == "KL")
                            {
                                quantity = 1;
                            }
                            else if (salesOrderDetail.Unit.Code == "PK")
                            {
                                quantity = 1;
                            }
                            else if (salesOrderDetail.Unit.Code == "M2")
                            {
                                quantity = salesOrderDetail.Product.Width * salesOrderDetail.Product.Lenght / 1000;
                            }
                            else if (salesOrderDetail.Unit.Code == "NKG")
                            {
                                quantity = Convert.ToDecimal(row["Net Miktar"]);
                            }
                            else if (salesOrderDetail.Unit.Code == "MT")
                            {
                                quantity = salesOrderDetail.Product.Lenght;
                            }

                            //insert into Production

                            Movement inputMovement = objectSpace.CreateObject<Movement>();
                            inputMovement.HeaderId = headerId;
                            inputMovement.DocumentNumber = string.Empty;
                            inputMovement.DocumentDate = DateTime.Now;
                            inputMovement.Barcode = row["Barkod"].ToString();
                            inputMovement.SalesOrderDetail = salesOrderDetail;
                            inputMovement.Product = salesOrderDetail.Product;
                            inputMovement.PartyNumber = string.Empty;
                            inputMovement.PaletteNumber = row["Palet No"].ToString();
                            inputMovement.Warehouse = warehouse;
                            inputMovement.MovementType = input;
                            inputMovement.Unit = salesOrderDetail.Unit;
                            inputMovement.Quantity = quantity;
                            inputMovement.cUnit = salesOrderDetail.cUnit;
                            inputMovement.cQuantity = Convert.ToDecimal(row["Miktar"]);
                        }

                        Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(SqlProvider.ConnectionStringOldERP, CommandType.Text, @"UPDATE STORES SET STOREID = 404 where PALETTENO = @paletteNumber", new SqlParameter("@paletteNumber", userControl.gridView1.GetRowCellValue(rowHandle, "Palet No")));
                    }
                    else
                    {
                        DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(SqlProvider.ConnectionStringOldERP, CommandType.Text, @"SELECT BARCODE as Barkod, AMOUNT as Miktar FROM STORES WHERE STOREID = (SELECT STOREID FROM STORAGE WHERE STORECODE = @storeCode) AND ORCODE = @orcode", new SqlParameter("@storeCode", userControl.gridView1.GetRowCellValue(rowHandle, "Depo Kodu")), new SqlParameter("@orcode", userControl.gridView1.GetRowCellValue(rowHandle, "Sipariş No"))).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                decimal quantity = Convert.ToDecimal(row["Miktar"]);
                                if (salesOrderDetail.Unit.Code == "AD")
                                {
                                    quantity = salesOrderDetail.Product.OuterPackingInPiece;
                                }
                                else if (salesOrderDetail.Unit.Code == "RL")
                                {
                                    quantity = salesOrderDetail.Product.OuterPackingRollCount;
                                }
                                else if (salesOrderDetail.Unit.Code == "KL")
                                {
                                    quantity = 1;
                                }
                                else if (salesOrderDetail.Unit.Code == "PK")
                                {
                                    quantity = 1;
                                }
                                else if (salesOrderDetail.Unit.Code == "M2")
                                {
                                    quantity = salesOrderDetail.Product.Width * salesOrderDetail.Product.Lenght / 1000;
                                }
                                else if (salesOrderDetail.Unit.Code == "NKG")
                                {
                                    quantity = Convert.ToDecimal(row["Miktar"]);
                                }
                                else if (salesOrderDetail.Unit.Code == "MT")
                                {
                                    quantity = salesOrderDetail.Product.Lenght;
                                }

                                //insert into Production

                                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(((XPObjectSpace)objectSpace).Session.ConnectionString, CommandType.Text, @"insert into Store (Oid, Product, Barcode, SalesOrderDetail, PartyNumber, PaletteNumber, Warehouse, Unit, Quantity, cUnit, cQuantity, OptimisticLockField, GCRecord, WarehouseCell) values(NEWID(), @product, @barcode, @salesOrderDetail, '', '', @warehouse, @unit, @quantity, @cunit, @cquantity, 0, NULL, NULL)", new SqlParameter("@product", salesOrderDetail.Product.Oid), new SqlParameter("@barcode", row["Barkod"].ToString()), new SqlParameter("@salesOrderDetail", salesOrderDetail.Oid), new SqlParameter("@warehouse", warehouse.Oid), new SqlParameter("@unit", salesOrderDetail.Unit.Oid), new SqlParameter("@quantity", quantity), new SqlParameter("@cunit", salesOrderDetail.cUnit.Oid), new SqlParameter("@cquantity", Convert.ToDecimal(row["Miktar"])));
                            }
                        }

                        Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(SqlProvider.ConnectionStringOldERP, CommandType.Text, @"UPDATE STORES SET STOREID = 404 WHERE STOREID = (SELECT STOREID FROM STORAGE WHERE STORECODE = @storeCode) AND ORCODE = @orcode", new SqlParameter("@storeCode", userControl.gridView1.GetRowCellValue(rowHandle, "Depo Kodu")), new SqlParameter("@orcode", userControl.gridView1.GetRowCellValue(rowHandle, "Sipariş No")));
                    }
                }

                userControl.RefreshGrid(userControl.txtWarehouse.Text, userControl.txtOrderNumber.Text, userControl.txtPletteNumber.Text);
                objectSpace.CommitChanges();
                XtraMessageBox.Show("Aktarım tamamlandı.");
            }
        }
    }
}