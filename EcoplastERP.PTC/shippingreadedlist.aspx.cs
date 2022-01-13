﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.PTC
{
    public partial class shippingreadedlist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userOid"] == null)
            {
                Response.Redirect("shippinglogin.aspx");
            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = Request.QueryString["exp"] != null ? Request.QueryString["exp"].ToString() : string.Empty;
        }

        protected void SqlDataSource2_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = Request.QueryString["exp"] != null ? Request.QueryString["exp"].ToString() : string.Empty;
        }

        protected void SqlDataSource3_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = Request.QueryString["dlv"] != null ? Request.QueryString["dlv"].ToString() : string.Empty;
        }

        protected void SqlDataSource4_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = Request.QueryString["dlv"] != null ? Request.QueryString["dlv"].ToString() : string.Empty;
        }

        protected void rblReportType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReportType1.SelectedValue == "expedition")
            {
                if (rblReportType2.SelectedValue == "palette")
                {
                    rptRecords.DataSourceID = "SqlDataSource1";
                    rptRecords.DataBind();
                }
                else if (rblReportType2.SelectedValue == "barcode")
                {
                    rptRecords.DataSourceID = "SqlDataSource2";
                    rptRecords.DataBind();
                }
            }
            else if (rblReportType1.SelectedValue == "delivery")
            {
                if (rblReportType2.SelectedValue == "palette")
                {
                    rptRecords.DataSourceID = "SqlDataSource3";
                    rptRecords.DataBind();
                }
                else if (rblReportType2.SelectedValue == "barcode")
                {
                    rptRecords.DataSourceID = "SqlDataSource4";
                    rptRecords.DataBind();
                }
            }
        }

        protected void rblReportType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReportType1.SelectedValue == "expedition")
            {
                if (rblReportType2.SelectedValue == "palette")
                {
                    rptRecords.DataSourceID = "SqlDataSource1";
                    rptRecords.DataBind();
                }
                else if (rblReportType2.SelectedValue == "barcode")
                {
                    rptRecords.DataSourceID = "SqlDataSource2";
                    rptRecords.DataBind();
                }
            }
            else if (rblReportType1.SelectedValue == "delivery")
            {
                if (rblReportType2.SelectedValue == "palette")
                {
                    rptRecords.DataSourceID = "SqlDataSource3";
                    rptRecords.DataBind();
                }
                else if (rblReportType2.SelectedValue == "barcode")
                {
                    rptRecords.DataSourceID = "SqlDataSource4";
                    rptRecords.DataBind();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Session session = modal.XpoHelper.GetNewSession();
            string barcode = (sender as Button).CommandArgument;
            if (barcode.StartsWith("M")) //Barkod ise
            {
                DeliveryDetailLoading deliveryDetailLoading = session.FindObject<DeliveryDetailLoading>(new BinaryOperator("Barcode", barcode));
                if (deliveryDetailLoading != null)
                {
                    if (deliveryDetailLoading.DeliveryLoadingType == Module.DeliveryLoadingType.WithBarcode)
                    {
                        var headerId = Guid.NewGuid();
                        Store store = session.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.LoadingWarehouse = true", barcode));
                        if (store != null)
                        {
                            session.BeginTransaction();
                            try
                            {
                                Warehouse returnWarehouse = session.FindObject<Warehouse>(CriteriaOperator.Parse("Code = '800'"));
                                Movement movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P127")), Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                                movement.Save();

                                movement = new Movement(session) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session.FindObject<MovementType>(new BinaryOperator("Code", "P126")), Barcode = barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = returnWarehouse, WarehouseCell = null, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                                movement.Save();

                                Store newStore = new Store(session) { Product = store.Product, Barcode = store.Barcode, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = returnWarehouse, WarehouseCell = null, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity, SalesOrderDetail = store.SalesOrderDetail };
                                newStore.Save();

                                deliveryDetailLoading.Delete();
                                session.CommitTransaction();
                            }
                            catch
                            {
                                session.RollbackTransaction();
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('İşlemler sırasında bir hata oluştu..');", true);
                            }
                        }

                        modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete Store where Barcode = @barcode and GCRecord is not null", new string[] { "@barcode" }, new object[] { barcode });
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Seçtiğiniz barkod palet ile birlikte okutulmuş bu kaydı silemezsiniz.');", true);
                    }
                }
            }
            else
            {
                XPCollection<DeliveryDetailLoading> loadingList = new XPCollection<DeliveryDetailLoading>(session, CriteriaOperator.Parse("PaletteNumber = ?", barcode));
                for (int i = 0; i < loadingList.Count; i++)
                {
                    Session session1 = modal.XpoHelper.GetNewSession();

                    var headerId = Guid.NewGuid();
                    Store store = session1.FindObject<Store>(CriteriaOperator.Parse("Barcode = ? and Warehouse.LoadingWarehouse = true", loadingList[i].Barcode));
                    if (store != null)
                    {
                        session1.BeginTransaction();
                        try
                        {
                            Warehouse returnWarehouse = session1.FindObject<Warehouse>(CriteriaOperator.Parse("Code = '800'"));
                            Movement movement = new Movement(session1) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session1.FindObject<MovementType>(new BinaryOperator("Code", "P127")), Barcode = store.Barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = store.Warehouse, WarehouseCell = store.WarehouseCell, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                            movement.Save();

                            movement = new Movement(session1) { HeaderId = headerId, DocumentDate = DateTime.Now, MovementType = session1.FindObject<MovementType>(new BinaryOperator("Code", "P126")), Barcode = store.Barcode, SalesOrderDetail = store.SalesOrderDetail, Product = store.Product, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = returnWarehouse, WarehouseCell = null, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity };
                            movement.Save();

                            Store newStore = new Store(session1) { Product = store.Product, Barcode = store.Barcode, PartyNumber = store.PartyNumber, PaletteNumber = store.PaletteNumber, Warehouse = returnWarehouse, WarehouseCell = null, Unit = store.Unit, Quantity = store.Quantity, cUnit = store.cUnit, cQuantity = store.cQuantity, SalesOrderDetail = store.SalesOrderDetail };
                            newStore.Save();
                            session1.CommitTransaction();
                        }
                        catch
                        {
                            session1.RollbackTransaction();
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('İşlemler sırasında bir hata oluştu..');", true);
                        }
                    }
                }

                session.Delete(loadingList);
                modal.XpoHelper.GetNewSession().ExecuteNonQuery(@"delete Store where PaletteNumber = @barcode and GCRecord is not null", new string[] { "@barcode" }, new object[] { barcode });
            }

            rptRecords.DataBind();
        }
    }
}