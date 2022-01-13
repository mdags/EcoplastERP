using System;
using System.Collections;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class SalesOrderViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public SalesOrderViewController()
        {
            InitializeComponent();
            RegisterActions(components);

            SetSalesOrderDetailStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(SalesOrderDetail), "Status"), null);
            SetSalesOrderDetailStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(SalesOrderStatus));
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            Frame.GetController<ObjectMethodActionsViewController>().Actions["SalesOrderDetail.UpdatePetkimPrice"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["SalesOrderDetail.PlanningConfirm.PlanningConfirmParameters"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Sipariş Planlama Onayı") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["SalesOrderDetail.ConfirmOrder"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satış") ? true : false);
            Frame.GetController<ObjectMethodActionsViewController>().Actions["SalesOrderDetail.ShippingPlanNotify.ShippingPlanNotifyParametersObject"].Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Sevk Bildir") ? true : false);
            this.CopySalesOrderDetailAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satış") || Helpers.IsUserInRole("Satış Müdürü") ? true : false);
            this.SetSalesOrderDetailStatusAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Satış") || Helpers.IsUserInRole("Üretim Siparişi Açma") ? true : false);
            this.CreateWorkOrderAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Üretim Siparişi Açma") ? true : false);
            this.CopyWorkOrderAction.Active.SetItemValue("ObjectType", Helpers.IsUserAdministrator() || Helpers.IsUserInRole("Üretim Siparişi Açma") ? true : false);
        }
        private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)
        {
            foreach (object current in Enum.GetValues(enumType))
            {
                EnumDescriptor ed = new EnumDescriptor(enumType);
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current) { ImageName = ImageLoader.Instance.GetEnumValueImageName(current) };
                parentItem.Items.Add(item);
            }
        }
        private void SetSalesOrderDetailStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    SalesOrderDetail objInNewObjectSpace = (SalesOrderDetail)objectSpace.GetObject(obj);
                    if (objInNewObjectSpace.SalesOrderStatus != SalesOrderStatus.WaitingforPlanningConfirm)
                    {
                        if ((SalesOrderStatus)e.SelectedChoiceActionItem.Data != SalesOrderStatus.WaitingforPlanningConfirm)
                        {
                            if((SalesOrderStatus)e.SelectedChoiceActionItem.Data == SalesOrderStatus.Completed)
                            {
                                if (objInNewObjectSpace.Product.ProductGroup.Code == "OM")
                                {
                                    Store store = objectSpace.FindObject<Store>(CriteriaOperator.Parse("SalesOrderDetail = ? and Warehouse.ShippingWarehouse = true", objInNewObjectSpace));
                                    if (store != null)
                                    {
                                        throw new UserFriendlyException("Sevk depoda siparişe ait mamul mevcut sipariş tamamlama yapamazsınız.");
                                    }
                                    else objInNewObjectSpace.SalesOrderStatus = (SalesOrderStatus)e.SelectedChoiceActionItem.Data;
                                }
                                else objInNewObjectSpace.SalesOrderStatus = (SalesOrderStatus)e.SelectedChoiceActionItem.Data;
                            }
                            else objInNewObjectSpace.SalesOrderStatus = (SalesOrderStatus)e.SelectedChoiceActionItem.Data;
                        }
                    }
                    else throw new UserFriendlyException("Planlama Onayı durumunu sadece yetkisi olan kullanıcı değiştirebilir.");
                }
            }
            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if (View is ListView)
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }

        private void PrintSalesOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ReportData _reportData = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Oid", 1));
            if (_reportData != null)
            {
                IObjectSpace objectSpace = View is DetailView ? Application.CreateObjectSpace() : View.ObjectSpace;
                foreach (Object obj in e.SelectedObjects)
                {
                    SalesOrder objInNewObjectSpace = (SalesOrder)objectSpace.GetObject(obj);
                    Frame.GetController<ReportServiceController>().ShowPreview(_reportData, CriteriaOperator.Parse("[SalesOrder.OrderNumber] = ?", objInNewObjectSpace.OrderNumber));
                }
            }
        }

        private void CopyWorkOrderAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            IObjectSpace _objectSpace = Application.CreateObjectSpace();
            SalesOrderDetail salesOrderDetail = (SalesOrderDetail)objectSpace.GetObject(e.CurrentObject);
            const string filmingWorkOrder_ListView = "FilmingWorkOrder_ListView_Copy";
            const string castFilmingWorkOrder_ListView = "CastFilmingWorkOrder_ListView_Copy";
            const string castTransferingWorkOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";
            const string balloonFilmingWorkOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";
            const string printingWorkOrder_ListView = "PrintingWorkOrder_ListView_Copy";
            const string laminationWorkOrder_ListView = "LaminationWorkOrder_ListView_Copy";
            const string slicingWorkOrder_ListView = "SlicingWorkOrder_ListView_Copy";
            const string castSlicingWorkOrder_ListView = "CastSlicingWorkOrder_ListView_Copy";
            const string cuttingWorkOrder_ListView = "CuttingWorkOrder_ListView_Copy";
            const string foldingWorkOrder_ListView = "FoldingWorkOrder_ListView_Copy";
            const string balloonCuttingWorkOrder_ListView = "BalloonCuttingWorkOrder_ListView_Copy";
            const string regeneratedWorkOrder_ListView = "RegeneratedWorkOrder_ListView_Copy";
            const string castRegeneratedWorkOrder_ListView = "CastRegeneratedWorkOrder_ListView_Copy";
            const string eco6WorkOrder_ListView = "Eco6WorkOrder_ListView_Copy";
            const string eco6CuttingWorkOrder_ListView = "Eco6CuttingWorkOrder_ListView_Copy";
            const string eco6LaminationWorkOrder_ListView = "Eco6LaminationWorkOrder_ListView_Copy";
            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Aynı Stoğu")
            {
                if (salesOrderDetail == null) return;
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastFilmingWorkOrder), castFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(PrintingWorkOrder), printingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(printingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(LaminationWorkOrder), laminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(laminationWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(SlicingWorkOrder), slicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(slicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastSlicingWorkOrder), castSlicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castSlicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CuttingWorkOrder), cuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(cuttingWorkOrder_ListView, collectionSource, true);
                }
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FoldingWorkOrder), foldingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid",
                //        salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                //    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid",
                //        salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(foldingWorkOrder_ListView, collectionSource, true);
                //}
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonCuttingWorkOrder), balloonCuttingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid",
                //        salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                //    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid",
                //        salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonCuttingWorkOrder_ListView, collectionSource, true);
                //}
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(RegeneratedWorkOrder), regeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(regeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastRegeneratedWorkOrder), castRegeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castRegeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6WorkOrder), eco6WorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6WorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6CuttingWorkOrder), eco6CuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6CuttingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6LaminationWorkOrder), eco6LaminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product.Oid", salesOrderDetail.Product.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6LaminationWorkOrder_ListView, collectionSource, true);
                }
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Stoktan")
            {
                if (salesOrderDetail == null) return;
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastFilmingWorkOrder), castFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(PrintingWorkOrder), printingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(printingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(LaminationWorkOrder), laminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(laminationWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(SlicingWorkOrder), slicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(slicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastSlicingWorkOrder), castSlicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castSlicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CuttingWorkOrder), cuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(cuttingWorkOrder_ListView, collectionSource, true);
                }
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FoldingWorkOrder), foldingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                //        salesOrderDetail.Product.Oid);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(foldingWorkOrder_ListView, collectionSource, true);
                //}
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonCuttingWorkOrder), balloonCuttingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                //        salesOrderDetail.Product.Oid);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonCuttingWorkOrder_ListView, collectionSource, true);
                //}
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(RegeneratedWorkOrder), regeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(regeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastRegeneratedWorkOrder), castRegeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castRegeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6WorkOrder), eco6WorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6WorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6CuttingWorkOrder), eco6CuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6CuttingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6LaminationWorkOrder), eco6LaminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyProduct"] = new BinaryOperator("SalesOrderDetail.Product",
                        salesOrderDetail.Product.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6LaminationWorkOrder_ListView, collectionSource, true);
                }
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Firmanın Son Üretim Siparişi")
            {
                if (salesOrderDetail == null) return;
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastFilmingWorkOrder), castFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(PrintingWorkOrder), printingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(printingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(LaminationWorkOrder), laminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(laminationWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(SlicingWorkOrder), slicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(slicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastSlicingWorkOrder), castSlicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castSlicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CuttingWorkOrder), cuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(cuttingWorkOrder_ListView, collectionSource, true);
                }
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FoldingWorkOrder), foldingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid",
                //        salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(foldingWorkOrder_ListView, collectionSource, true);
                //}
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonCuttingWorkOrder), balloonCuttingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid",
                //        salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonCuttingWorkOrder_ListView, collectionSource, true);
                //}
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(RegeneratedWorkOrder), regeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(regeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastRegeneratedWorkOrder), castRegeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castRegeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6WorkOrder), eco6WorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6WorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6CuttingWorkOrder), eco6CuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid",
                        salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6CuttingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6LaminationWorkOrder), eco6LaminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", salesOrderDetail.SalesOrder.Contact.Oid, BinaryOperatorType.Equal);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6LaminationWorkOrder_ListView, collectionSource, true);
                }
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Makine Yükünden")
            {
                if (salesOrderDetail == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Machine), "Machine_ListView");
                Station station = null;
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Çekim"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Cast Çekim"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Cast Aktarma"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Balonlu Çekim"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Baskı"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Laminasyon"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Dilme"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Cast Dilme"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Kesim"));
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Katlama"));
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Balonlu Kesim"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Rejenere"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Cast Rejenere"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Eco6"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Cutting) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Eco6 Kesim"));
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination) station = objectSpace.FindObject<Station>(new BinaryOperator("Name", "Eco6 Laminasyon"));
                if (station != null) collectionSource.Criteria["FilterbyStation"] = new BinaryOperator("Station.Oid", station.Oid, BinaryOperatorType.Equal);
                
                e.ShowViewParameters.CreatedView = Application.CreateListView("Machine_ListView", collectionSource, true);
                DialogController dcMachine = Application.CreateController<DialogController>();
                dcMachine.Accepting += dcMachine_Accepting;
                dcMachine.Tag = (e.CurrentObject as SalesOrderDetail).Oid;
                e.ShowViewParameters.Controllers.Add(dcMachine);
            }

            if (e.SelectedChoiceActionItem.Caption == "Farklı Firmadan")
            {
                if (salesOrderDetail == null) return;
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Contact), "Contact_ListView");
                e.ShowViewParameters.CreatedView = Application.CreateListView("Contact_ListView", collectionSource, true);
                DialogController dcContact = Application.CreateController<DialogController>();
                dcContact.Accepting += dcContact_Accepting;
                dcContact.Tag = (e.CurrentObject as SalesOrderDetail).Oid;
                e.ShowViewParameters.Controllers.Add(dcContact);
            }

            if (e.SelectedChoiceActionItem.Caption == "Aynı Siparişten")
            {
                if (salesOrderDetail == null) return;
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastFilmingWorkOrder), castFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(PrintingWorkOrder), printingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(printingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(LaminationWorkOrder), laminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(laminationWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(SlicingWorkOrder), slicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(slicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastSlicingWorkOrder), castSlicingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castSlicingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CuttingWorkOrder), cuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(cuttingWorkOrder_ListView, collectionSource, true);
                }
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(FoldingWorkOrder), foldingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder",
                //        salesOrderDetail.SalesOrder.Oid);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(foldingWorkOrder_ListView, collectionSource, true);
                //}
                //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting)
                //{
                //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(BalloonCuttingWorkOrder), balloonCuttingWorkOrder_ListView);
                //    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder",
                //        salesOrderDetail.SalesOrder.Oid);
                //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                //    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonCuttingWorkOrder_ListView, collectionSource, true);
                //}
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(RegeneratedWorkOrder), regeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(regeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(CastRegeneratedWorkOrder), castRegeneratedWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(castRegeneratedWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6WorkOrder), eco6WorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6WorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6CuttingWorkOrder), eco6CuttingWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6CuttingWorkOrder_ListView, collectionSource, true);
                }
                if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(_objectSpace, typeof(Eco6LaminationWorkOrder), eco6LaminationWorkOrder_ListView);
                    collectionSource.Criteria["FilterbySalesOrder"] = new BinaryOperator("SalesOrderDetail.SalesOrder", salesOrderDetail.SalesOrder.Oid);
                    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                    e.ShowViewParameters.CreatedView = Application.CreateListView(eco6LaminationWorkOrder_ListView, collectionSource, true);
                }
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = (e.CurrentObject as SalesOrderDetail).Oid;
            dc.Accepting += dc_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(dc);
        }

        void dcMachine_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Machine machine = ((Machine)(e.AcceptActionArgs.SelectedObjects[0]));
            string input = Microsoft.VisualBasic.Interaction.InputBox("Ebat:", "Ebat", "", 100, 100);
            const string filmingWorkOrder_ListView = "FilmingWorkOrder_ListView_Copy";
            const string castFilmingWorkOrder_ListView = "CastFilmingWorkOrder_ListView_Copy";
            const string castTransferingWorkOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";
            const string balloonFilmingWorkOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";
            const string printingWorkOrder_ListView = "PrintingWorkOrder_ListView_Copy";
            const string laminationWorkOrder_ListView = "LaminationWorkOrder_ListView_Copy";
            const string slicingWorkOrder_ListView = "SlicingWorkOrder_ListView_Copy";
            const string castSlicingWorkOrder_ListView = "CastSlicingWorkOrder_ListView_Copy";
            const string cuttingWorkOrder_ListView = "CuttingWorkOrder_ListView_Copy";
            const string foldingWorkOrder_ListView = "FoldingWorkOrder_ListView_Copy";
            const string balloonCuttingWorkOrder_ListView = "BalloonCuttingWorkOrder_ListView_Copy";
            const string regeneratedWorkOrder_ListView = "RegeneratedWorkOrder_ListView_Copy";
            const string castRegeneratedWorkOrder_ListView = "CastRegeneratedWorkOrder_ListView_Copy";
            const string eco6WorkOrder_ListView = "Eco6WorkOrder_ListView_Copy";
            const string eco6CuttingWorkOrder_ListView = "Eco6CuttingWorkOrder_ListView_Copy";
            const string eco6LaminationWorkOrder_ListView = "Eco6LaminationWorkOrder_ListView_Copy";

            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastFilmingWorkOrder), castFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castFilmingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!string.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(PrintingWorkOrder), printingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(printingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), laminationWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(laminationWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(SlicingWorkOrder), slicingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(slicingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastSlicingWorkOrder), castSlicingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castSlicingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CuttingWorkOrder), cuttingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(cuttingWorkOrder_ListView, collectionSource, true);
            }
            //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding)
            //{
            //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FoldingWorkOrder), foldingWorkOrder_ListView);
            //    collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
            //    if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
            //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            //    e.ShowViewParameters.CreatedView = Application.CreateListView(foldingWorkOrder_ListView, collectionSource, true);
            //}
            //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting)
            //{
            //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonCuttingWorkOrder), balloonCuttingWorkOrder_ListView);
            //    collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
            //    if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
            //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            //    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonCuttingWorkOrder_ListView, collectionSource, true);
            //}
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(RegeneratedWorkOrder), regeneratedWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(regeneratedWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastRegeneratedWorkOrder), castRegeneratedWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castRegeneratedWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Eco6WorkOrder), eco6WorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(eco6WorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Eco6CuttingWorkOrder), eco6CuttingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(eco6CuttingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Eco6LaminationWorkOrder), eco6LaminationWorkOrder_ListView);
                collectionSource.Criteria["FilterbyMachine"] = new BinaryOperator("Machine.Oid", machine.Oid, BinaryOperatorType.Equal);
                if (!String.IsNullOrEmpty(input)) collectionSource.Criteria["FilterbyWidth"] = new BinaryOperator("Width", input, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(eco6LaminationWorkOrder_ListView, collectionSource, true);
            }

            DialogController dc = Application.CreateController<DialogController>();
            dc.Accepting += dc_Accepting;
            dc.Tag = salesOrderDetail.Oid;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dcContact_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            SalesOrderDetail salesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
            Contact contact = ((Contact)(e.AcceptActionArgs.SelectedObjects[0]));
            const string filmingWorkOrder_ListView = "FilmingWorkOrder_ListView_Copy";
            const string castFilmingWorkOrder_ListView = "CastFilmingWorkOrder_ListView_Copy";
            const string castTransferingWorkOrder_ListView = "CastTransferingWorkOrder_ListView_Copy";
            const string balloonFilmingWorkOrder_ListView = "BalloonFilmingWorkOrder_ListView_Copy";
            const string printingWorkOrder_ListView = "PrintingWorkOrder_ListView_Copy";
            const string laminationWorkOrder_ListView = "LaminationWorkOrder_ListView_Copy";
            const string slicingWorkOrder_ListView = "SlicingWorkOrder_ListView_Copy";
            const string castSlicingWorkOrder_ListView = "CastSlicingWorkOrder_ListView_Copy";
            const string cuttingWorkOrder_ListView = "CuttingWorkOrder_ListView_Copy";
            const string foldingWorkOrder_ListView = "FoldingWorkOrder_ListView_Copy";
            const string balloonCuttingWorkOrder_ListView = "BalloonCuttingWorkOrder_ListView_Copy";
            const string regeneratedWorkOrder_ListView = "RegeneratedWorkOrder_ListView_Copy";
            const string castRegeneratedWorkOrder_ListView = "CastRegeneratedWorkOrder_ListView_Copy";
            const string eco6WorkOrder_ListView = "Eco6WorkOrder_ListView_Copy";
            const string eco6CuttingWorkOrder_ListView = "Eco6CuttingWorkOrder_ListView_Copy";
            const string eco6LaminationWorkOrder_ListView = "Eco6LaminationWorkOrder_ListView_Copy";

            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FilmingWorkOrder), filmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(filmingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastFilmingWorkOrder), castFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castFilmingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastTransferingWorkOrder), castTransferingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castTransferingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonFilmingWorkOrder), balloonFilmingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(balloonFilmingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(PrintingWorkOrder), printingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(printingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(LaminationWorkOrder), laminationWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(laminationWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(SlicingWorkOrder), slicingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(slicingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastSlicingWorkOrder), castSlicingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castSlicingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CuttingWorkOrder), cuttingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(cuttingWorkOrder_ListView, collectionSource, true);
            }
            //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFolding)
            //{
            //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(FoldingWorkOrder), foldingWorkOrder_ListView);
            //    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
            //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            //    e.ShowViewParameters.CreatedView = Application.CreateListView(foldingWorkOrder_ListView, collectionSource, true);
            //}
            //if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonCutting)
            //{
            //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(BalloonCuttingWorkOrder), balloonCuttingWorkOrder_ListView);
            //    collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
            //    collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
            //    e.ShowViewParameters.CreatedView = Application.CreateListView(balloonCuttingWorkOrder_ListView, collectionSource, true);
            //}
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(RegeneratedWorkOrder), regeneratedWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(regeneratedWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CastRegeneratedWorkOrder), castRegeneratedWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(castRegeneratedWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Eco6WorkOrder), eco6WorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(eco6WorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Eco6CuttingWorkOrder), eco6CuttingWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(eco6CuttingWorkOrder_ListView, collectionSource, true);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
            {
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Eco6LaminationWorkOrder), eco6LaminationWorkOrder_ListView);
                collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrderDetail.SalesOrder.Contact.Oid", contact.Oid, BinaryOperatorType.Equal);
                collectionSource.Sorting.Add(new SortProperty("WorkOrderDate", DevExpress.Xpo.DB.SortingDirection.Descending));
                e.ShowViewParameters.CreatedView = Application.CreateListView(eco6LaminationWorkOrder_ListView, collectionSource, true);
            }

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = salesOrderDetail.Oid;
            dc.Accepting += dc_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "FilmingWorkOrder")
            {
                FilmingWorkOrder obj = objectSpace.CreateObject<FilmingWorkOrder>();
                FilmingWorkOrder filmingWorkOrder = ((FilmingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (filmingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", filmingWorkOrder.Machine.Oid));
                if (filmingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", filmingWorkOrder.NextStation.Oid));
                obj.ProductionOption = filmingWorkOrder.ProductionOption;
                obj.ProductionNote = filmingWorkOrder.ProductionNote;
                obj.QualityNote = filmingWorkOrder.QualityNote;
                obj.RollDiameter = filmingWorkOrder.RollDiameter;
                obj.RollCount = filmingWorkOrder.RollCount;
                obj.MinimumRollWeight = filmingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = filmingWorkOrder.MaximumRollWeight;
                obj.WayCount = filmingWorkOrder.WayCount;
                obj.GramM2 = filmingWorkOrder.GramM2;
                obj.GramMetretul = filmingWorkOrder.GramMetretul;
                obj.InflationRate = filmingWorkOrder.InflationRate;
                if (filmingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", filmingWorkOrder.Bobbin.Oid));
                if (filmingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", filmingWorkOrder.Palette.Oid));
                obj.PaletteLayout = filmingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = filmingWorkOrder.PaletteBobbinCount;
                obj.Reciept = filmingWorkOrder.Reciept != null ? objectSpace.FindObject<Reciept>(new BinaryOperator("Oid", filmingWorkOrder.Reciept.Oid)) : null;
                obj.PartString = filmingWorkOrder.PartString;
                obj.RecieptString = filmingWorkOrder.RecieptString;

                #region ısıl değerler kopyala
                obj.LineSpeed = filmingWorkOrder.LineSpeed;
                obj.CoolingLine = filmingWorkOrder.CoolingLine;
                obj.FilmingTension = filmingWorkOrder.FilmingTension;
                obj.WrapperTension = filmingWorkOrder.WrapperTension;
                obj.BubbleCooling = filmingWorkOrder.BubbleCooling;
                obj.IBCFeed = filmingWorkOrder.IBCFeed;
                obj.IBCExtruder = filmingWorkOrder.IBCExtruder;
                obj.TowerSpeed = filmingWorkOrder.TowerSpeed;
                obj.Neck = filmingWorkOrder.Neck;
                obj.Head1 = filmingWorkOrder.Head1;
                obj.Head2 = filmingWorkOrder.Head2;
                obj.Lip = filmingWorkOrder.Lip;
                #endregion

                //foreach (var item in filmingWorkOrder.FilmingWorkOrderParts)
                //{
                //    FilmingWorkOrderPart filmingWorkOrderPart = objectSpace.CreateObject<FilmingWorkOrderPart>();
                //    filmingWorkOrderPart.FilmingWorkOrder = obj;
                //    filmingWorkOrderPart.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                //    filmingWorkOrderPart.Thickness = item.Thickness;
                //}
                objectSpace.Delete(obj.FilmingWorkOrderReciepts);
                foreach (var item in filmingWorkOrder.FilmingWorkOrderReciepts)
                {
                    FilmingWorkOrderReciept filmingWorkOrderReciept = objectSpace.CreateObject<FilmingWorkOrderReciept>();
                    filmingWorkOrderReciept.FilmingWorkOrder = obj;
                    filmingWorkOrderReciept.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                    filmingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    filmingWorkOrderReciept.Quantity = item.Quantity;
                    filmingWorkOrderReciept.Rate = item.Rate;
                    filmingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    filmingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    filmingWorkOrderReciept.WorkOrderRate = item.WorkOrderRate;
                    obj.FilmingWorkOrderReciepts.Add(filmingWorkOrderReciept);
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "CastFilmingWorkOrder")
            {
                CastFilmingWorkOrder obj = objectSpace.CreateObject<CastFilmingWorkOrder>();
                var castFilmingWorkOrder = ((CastFilmingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (castFilmingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", castFilmingWorkOrder.Machine.Oid));
                if (castFilmingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", castFilmingWorkOrder.NextStation.Oid));
                obj.ProductionOption = castFilmingWorkOrder.ProductionOption;
                obj.ProductionNote = castFilmingWorkOrder.ProductionNote;
                obj.QualityNote = castFilmingWorkOrder.QualityNote;
                obj.RollDiameter = castFilmingWorkOrder.RollDiameter;
                obj.RollCount = castFilmingWorkOrder.RollCount;
                obj.MinimumRollWeight = castFilmingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = castFilmingWorkOrder.MaximumRollWeight;
                obj.WayCount = castFilmingWorkOrder.WayCount;
                obj.GramM2 = castFilmingWorkOrder.GramM2;
                obj.GramMetretul = castFilmingWorkOrder.GramMetretul;
                obj.InflationRate = castFilmingWorkOrder.InflationRate;
                if (castFilmingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", castFilmingWorkOrder.OuterPacking.Oid));
                if (castFilmingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", castFilmingWorkOrder.Bobbin.Oid));
                if (castFilmingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", castFilmingWorkOrder.Palette.Oid));
                obj.PaletteLayout = castFilmingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = castFilmingWorkOrder.PaletteBobbinCount;
                obj.Reciept = castFilmingWorkOrder.Reciept != null ? objectSpace.FindObject<Reciept>(new BinaryOperator("Oid", castFilmingWorkOrder.Reciept.Oid)) : null;
                obj.PartString = castFilmingWorkOrder.PartString;
                obj.RecieptString = castFilmingWorkOrder.RecieptString;
                //foreach (var item in castFilmingWorkOrder.CastFilmingWorkOrderParts)
                //{
                //    CastFilmingWorkOrderPart castFilmingWorkOrderPart = objectSpace.CreateObject<CastFilmingWorkOrderPart>();
                //    castFilmingWorkOrderPart.CastFilmingWorkOrder = obj;
                //    castFilmingWorkOrderPart.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                //    castFilmingWorkOrderPart.Thickness = item.Thickness;
                //}
                objectSpace.Delete(obj.CastFilmingWorkOrderReciepts);
                foreach (var item in castFilmingWorkOrder.CastFilmingWorkOrderReciepts)
                {
                    CastFilmingWorkOrderReciept castFilmingWorkOrderReciept = objectSpace.CreateObject<CastFilmingWorkOrderReciept>();
                    castFilmingWorkOrderReciept.CastFilmingWorkOrder = obj;
                    castFilmingWorkOrderReciept.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                    castFilmingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    castFilmingWorkOrderReciept.Quantity = item.Quantity;
                    castFilmingWorkOrderReciept.Rate = item.Rate;
                    castFilmingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    castFilmingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    castFilmingWorkOrderReciept.WorkOrderRate = item.WorkOrderRate;
                    obj.CastFilmingWorkOrderReciepts.Add(castFilmingWorkOrderReciept);
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "CastTransferingWorkOrder")
            {
                CastTransferingWorkOrder obj = objectSpace.CreateObject<CastTransferingWorkOrder>();
                var castTransferingWorkOrder = ((CastTransferingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (castTransferingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", castTransferingWorkOrder.Machine.Oid));
                if (castTransferingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", castTransferingWorkOrder.NextStation.Oid));
                obj.ProductionOption = castTransferingWorkOrder.ProductionOption;
                obj.ProductionNote = castTransferingWorkOrder.ProductionNote;
                obj.QualityNote = castTransferingWorkOrder.QualityNote;
                obj.RollDiameter = castTransferingWorkOrder.RollDiameter;
                obj.RollCount = castTransferingWorkOrder.RollCount;
                obj.MinimumRollWeight = castTransferingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = castTransferingWorkOrder.MaximumRollWeight;
                obj.WayCount = castTransferingWorkOrder.WayCount;
                obj.GramM2 = castTransferingWorkOrder.GramM2;
                obj.GramMetretul = castTransferingWorkOrder.GramMetretul;
                obj.InflationRate = castTransferingWorkOrder.InflationRate;
                if (castTransferingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", castTransferingWorkOrder.OuterPacking.Oid));
                if (castTransferingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", castTransferingWorkOrder.Bobbin.Oid));
                if (castTransferingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", castTransferingWorkOrder.Palette.Oid));
                obj.PaletteLayout = castTransferingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = castTransferingWorkOrder.PaletteBobbinCount;
                obj.Reciept = castTransferingWorkOrder.Reciept != null ? objectSpace.FindObject<Reciept>(new BinaryOperator("Oid", castTransferingWorkOrder.Reciept.Oid)) : null;
                obj.PartString = castTransferingWorkOrder.PartString;
                obj.RecieptString = castTransferingWorkOrder.RecieptString;
                //foreach (var item in castTransferingWorkOrder.CastTransferingWorkOrderParts)
                //{
                //    CastTransferingWorkOrderPart castTransferingWorkOrderPart = objectSpace.CreateObject<CastTransferingWorkOrderPart>();
                //    castTransferingWorkOrderPart.CastTransferingWorkOrder = obj;
                //    castTransferingWorkOrderPart.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                //    castTransferingWorkOrderPart.Thickness = item.Thickness;
                //}
                objectSpace.Delete(obj.CastTransferingWorkOrderReciepts);
                foreach (var item in castTransferingWorkOrder.CastTransferingWorkOrderReciepts)
                {
                    CastTransferingWorkOrderReciept castTransferingWorkOrderReciept = objectSpace.CreateObject<CastTransferingWorkOrderReciept>();
                    castTransferingWorkOrderReciept.CastTransferingWorkOrder = obj;
                    castTransferingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    castTransferingWorkOrderReciept.Quantity = item.Quantity;
                    castTransferingWorkOrderReciept.Rate = item.Rate;
                    castTransferingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    castTransferingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    obj.CastTransferingWorkOrderReciepts.Add(castTransferingWorkOrderReciept);
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "BalloonFilmingWorkOrder")
            {
                BalloonFilmingWorkOrder obj = objectSpace.CreateObject<BalloonFilmingWorkOrder>();
                var balloonFilmingWorkOrder = ((BalloonFilmingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (balloonFilmingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", balloonFilmingWorkOrder.Machine.Oid));
                if (balloonFilmingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", balloonFilmingWorkOrder.NextStation.Oid));
                obj.ProductionOption = balloonFilmingWorkOrder.ProductionOption;
                obj.ProductionNote = balloonFilmingWorkOrder.ProductionNote;
                obj.QualityNote = balloonFilmingWorkOrder.QualityNote;
                obj.RollDiameter = balloonFilmingWorkOrder.RollDiameter;
                obj.RollCount = balloonFilmingWorkOrder.RollCount;
                obj.MinimumRollWeight = balloonFilmingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = balloonFilmingWorkOrder.MaximumRollWeight;
                obj.WayCount = balloonFilmingWorkOrder.WayCount;
                obj.GramM2 = balloonFilmingWorkOrder.GramM2;
                obj.GramMetretul = balloonFilmingWorkOrder.GramMetretul;
                obj.InflationRate = balloonFilmingWorkOrder.InflationRate;
                if (balloonFilmingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", balloonFilmingWorkOrder.OuterPacking.Oid));
                if (balloonFilmingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", balloonFilmingWorkOrder.Bobbin.Oid));
                if (balloonFilmingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", balloonFilmingWorkOrder.Palette.Oid));
                obj.PaletteLayout = balloonFilmingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = balloonFilmingWorkOrder.PaletteBobbinCount;
                obj.Reciept = balloonFilmingWorkOrder.Reciept != null ? objectSpace.FindObject<Reciept>(new BinaryOperator("Oid", balloonFilmingWorkOrder.Reciept.Oid)) : null;
                obj.Reciept = balloonFilmingWorkOrder.Reciept != null ? objectSpace.FindObject<Reciept>(new BinaryOperator("Oid", balloonFilmingWorkOrder.Reciept.Oid)) : null;
                obj.PartString = balloonFilmingWorkOrder.PartString;
                obj.RecieptString = balloonFilmingWorkOrder.RecieptString;
                //foreach (var item in balloonFilmingWorkOrder.BalloonFilmingWorkOrderParts)
                //{
                //    BalloonFilmingWorkOrderPart balloonFilmingWorkOrderPart = objectSpace.CreateObject<BalloonFilmingWorkOrderPart>();
                //    balloonFilmingWorkOrderPart.BalloonFilmingWorkOrder = obj;
                //    balloonFilmingWorkOrderPart.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                //    balloonFilmingWorkOrderPart.Thickness = item.Thickness;
                //}
                objectSpace.Delete(obj.BalloonFilmingWorkOrderReciepts);
                foreach (var item in balloonFilmingWorkOrder.BalloonFilmingWorkOrderReciepts)
                {
                    BalloonFilmingWorkOrderReciept balloonFilmingWorkOrderReciept = objectSpace.CreateObject<BalloonFilmingWorkOrderReciept>();
                    balloonFilmingWorkOrderReciept.BalloonFilmingWorkOrder = obj;
                    balloonFilmingWorkOrderReciept.MachinePart = objectSpace.FindObject<MachinePart>(new BinaryOperator("Oid", item.MachinePart.Oid));
                    balloonFilmingWorkOrderReciept.Product = objectSpace.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid));
                    balloonFilmingWorkOrderReciept.Quantity = item.Quantity;
                    balloonFilmingWorkOrderReciept.Rate = item.Rate;
                    balloonFilmingWorkOrderReciept.Unit = objectSpace.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid));
                    balloonFilmingWorkOrderReciept.Warehouse = objectSpace.FindObject<Warehouse>(new BinaryOperator("Oid", item.Warehouse.Oid));
                    balloonFilmingWorkOrderReciept.WorkOrderRate = item.WorkOrderRate;
                    obj.BalloonFilmingWorkOrderReciepts.Add(balloonFilmingWorkOrderReciept);
                }
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "PrintingWorkOrder")
            {
                PrintingWorkOrder obj = objectSpace.CreateObject<PrintingWorkOrder>();
                var printingWorkOrder = ((PrintingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (printingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", printingWorkOrder.Machine.Oid));
                if (printingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", printingWorkOrder.NextStation.Oid));
                obj.ProductionOption = printingWorkOrder.ProductionOption;
                obj.ProductionNote = printingWorkOrder.ProductionNote;
                obj.QualityNote = printingWorkOrder.QualityNote;
                if (printingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", printingWorkOrder.Bobbin.Oid));
                obj.RollWeight = printingWorkOrder.RollWeight;
                obj.MinimumRollWeight = printingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = printingWorkOrder.MaximumRollWeight;
                obj.RollDiameter = printingWorkOrder.RollDiameter;
                obj.RollCount = printingWorkOrder.RollCount;
                obj.BandCount = printingWorkOrder.BandCount;
                obj.Anilox = printingWorkOrder.Anilox;
                obj.ColorCount = printingWorkOrder.ColorCount;
                obj.PrintingColors = printingWorkOrder.PrintingColors;
                obj.PrintForm = printingWorkOrder.PrintForm;
                obj.PrintDirection = printingWorkOrder.PrintDirection;
                obj.PrintWorkStatus = printingWorkOrder.PrintWorkStatus;
                obj.ClicheBand = printingWorkOrder.ClicheBand;
                obj.ClicheThickness = printingWorkOrder.ClicheThickness;
                obj.ClicheCode = printingWorkOrder.ClicheCode;
                obj.CylinderRepetition = printingWorkOrder.CylinderRepetition;
                obj.Logo = printingWorkOrder.Logo;
                obj.Barcode = printingWorkOrder.Barcode;
                if (printingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", printingWorkOrder.Palette.Oid));
                obj.PaletteBobbinCount = printingWorkOrder.PaletteBobbinCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "LaminationWorkOrder")
            {
                LaminationWorkOrder obj = objectSpace.CreateObject<LaminationWorkOrder>();
                var laminationWorkOrder = ((LaminationWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (laminationWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", laminationWorkOrder.Machine.Oid));
                if (laminationWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", laminationWorkOrder.NextStation.Oid));
                obj.ProductionOption = laminationWorkOrder.ProductionOption;
                obj.ProductionNote = laminationWorkOrder.ProductionNote;
                obj.QualityNote = laminationWorkOrder.QualityNote;
                if (laminationWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", laminationWorkOrder.Bobbin.Oid));
                obj.RollWeight = laminationWorkOrder.RollWeight;
                obj.MinimumRollWeight = laminationWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = laminationWorkOrder.MaximumRollWeight;
                if (laminationWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", laminationWorkOrder.Palette.Oid));
                obj.PaletteBobbinCount = laminationWorkOrder.PaletteBobbinCount;
                obj.Length = laminationWorkOrder.Length;
                obj.RollDiameter = laminationWorkOrder.RollDiameter;
                obj.SubFilm = laminationWorkOrder.SubFilm;
                obj.SubFilmThickness = laminationWorkOrder.SubFilmThickness;
                obj.SubFilmWidth = laminationWorkOrder.SubFilmWidth;
                obj.SubFilmLength = laminationWorkOrder.SubFilmLength;
                obj.SubFilmTension = laminationWorkOrder.SubFilmTension;
                obj.TopFilm = laminationWorkOrder.TopFilm;
                obj.TopFilmThickness = laminationWorkOrder.TopFilmThickness;
                obj.TopFilmWidth = laminationWorkOrder.TopFilmWidth;
                obj.TopFilmLength = laminationWorkOrder.TopFilmLength;
                obj.TopFilmTension = laminationWorkOrder.TopFilmTension;
                obj.CoatingRollerSize = laminationWorkOrder.CoatingRollerSize;
                obj.SingleComponent = laminationWorkOrder.SingleComponent;
                obj.SingleComponentReservoir = laminationWorkOrder.SingleComponentReservoir;
                obj.SingleComponentHose = laminationWorkOrder.SingleComponentHose;
                obj.SingleComponentSprayGun = laminationWorkOrder.SingleComponentSprayGun;
                obj.DoubleComponent = laminationWorkOrder.DoubleComponent;
                obj.DoubleComponentA = laminationWorkOrder.DoubleComponentA;
                obj.DoubleComponentAReservoir = laminationWorkOrder.DoubleComponentAReservoir;
                obj.DoubleComponentAHose = laminationWorkOrder.DoubleComponentAHose;
                obj.DoubleComponentASprayGun = laminationWorkOrder.DoubleComponentASprayGun;
                obj.DoubleComponentB = laminationWorkOrder.DoubleComponentB;
                obj.DoubleComponentBReservoir = laminationWorkOrder.DoubleComponentBReservoir;
                obj.DoubleComponentBHose = laminationWorkOrder.DoubleComponentBHose;
                obj.DoubleComponentBSprayGun = laminationWorkOrder.DoubleComponentBSprayGun;
                obj.Performance = laminationWorkOrder.Performance;
                obj.LineSpeed = laminationWorkOrder.LineSpeed;
                obj.Tension = laminationWorkOrder.Tension;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "SlicingWorkOrder")
            {
                SlicingWorkOrder obj = objectSpace.CreateObject<SlicingWorkOrder>();
                var slicingWorkOrder = ((SlicingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (slicingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", slicingWorkOrder.Machine.Oid));
                if (slicingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", slicingWorkOrder.NextStation.Oid));
                obj.ProductionOption = slicingWorkOrder.ProductionOption;
                obj.ProductionNote = slicingWorkOrder.ProductionNote;
                obj.QualityNote = slicingWorkOrder.QualityNote;
                if (slicingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", slicingWorkOrder.Bobbin.Oid));
                obj.RollWeight = slicingWorkOrder.RollWeight;
                obj.MinimumRollWeight = slicingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = slicingWorkOrder.MaximumRollWeight;
                obj.SlicingWidth= slicingWorkOrder.SlicingWidth;
                obj.RollDiameter = slicingWorkOrder.RollDiameter;
                obj.RollCount = slicingWorkOrder.RollCount;
                obj.PrintDirection = slicingWorkOrder.PrintDirection;
                obj.PrintWorkStatus = slicingWorkOrder.PrintWorkStatus;
                if (slicingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", slicingWorkOrder.Palette.Oid));
                obj.PaletteBobbinCount = slicingWorkOrder.PaletteBobbinCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "CastSlicingWorkOrder")
            {
                CastSlicingWorkOrder obj = objectSpace.CreateObject<CastSlicingWorkOrder>();
                var castSlicingWorkOrder = ((CastSlicingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (castSlicingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", castSlicingWorkOrder.Machine.Oid));
                if (castSlicingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", castSlicingWorkOrder.NextStation.Oid));
                obj.ProductionOption = castSlicingWorkOrder.ProductionOption;
                obj.ProductionNote = castSlicingWorkOrder.ProductionNote;
                obj.QualityNote = castSlicingWorkOrder.QualityNote;
                if (castSlicingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", castSlicingWorkOrder.Bobbin.Oid));
                obj.RollWeight = castSlicingWorkOrder.RollWeight;
                obj.MinimumRollWeight = castSlicingWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = castSlicingWorkOrder.MaximumRollWeight;
                obj.SlicingWidth = castSlicingWorkOrder.SlicingWidth;
                obj.RollDiameter = castSlicingWorkOrder.RollDiameter;
                obj.RollCount = castSlicingWorkOrder.RollCount;
                obj.PrintDirection = castSlicingWorkOrder.PrintDirection;
                obj.PrintWorkStatus = castSlicingWorkOrder.PrintWorkStatus;
                if (castSlicingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", castSlicingWorkOrder.Palette.Oid));
                obj.PaletteBobbinCount = castSlicingWorkOrder.PaletteBobbinCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "CuttingWorkOrder")
            {
                CuttingWorkOrder obj = objectSpace.CreateObject<CuttingWorkOrder>();
                var cuttingWorkOrder = ((CuttingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (cuttingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", cuttingWorkOrder.Machine.Oid));
                if (cuttingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", cuttingWorkOrder.NextStation.Oid));
                obj.ProductionOption = cuttingWorkOrder.ProductionOption;
                obj.ProductionNote = cuttingWorkOrder.ProductionNote;
                obj.QualityNote = cuttingWorkOrder.QualityNote;
                obj.MinimumPieceWeight = cuttingWorkOrder.MinimumPieceWeight;
                obj.MaximumPieceWeight = cuttingWorkOrder.MaximumPieceWeight;
                obj.WayCount = cuttingWorkOrder.WayCount;
                obj.GramM2 = cuttingWorkOrder.GramM2;
                obj.GramMetretul = cuttingWorkOrder.GramMetretul;
                if (cuttingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", cuttingWorkOrder.OuterPacking.Oid));
                if (cuttingWorkOrder.InnerPacking != null) obj.InnerPacking = objectSpace.FindObject<InnerPacking>(new BinaryOperator("Oid", cuttingWorkOrder.InnerPacking.Oid));
                if (cuttingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", cuttingWorkOrder.Palette.Oid));
                obj.PaletteLayout = cuttingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = cuttingWorkOrder.PaletteBobbinCount;
                obj.DeckCount = cuttingWorkOrder.DeckCount;
                obj.OuterPackingPackageCount = cuttingWorkOrder.OuterPackingPackageCount;
                obj.InnerPackingPieceCount = cuttingWorkOrder.InnerPackingPieceCount;
                obj.SupportBandColor = cuttingWorkOrder.SupportBandColor;
                obj.SupportBandThickness = cuttingWorkOrder.SupportBandThickness;
                obj.SupportBandSize = cuttingWorkOrder.SupportBandSize;
                obj.SupportBandNote = cuttingWorkOrder.SupportBandNote;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "FoldingWorkOrder")
            {
                FoldingWorkOrder obj = objectSpace.CreateObject<FoldingWorkOrder>();
                var foldingWorkOrder = ((FoldingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (foldingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", foldingWorkOrder.Machine.Oid));
                if (foldingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", foldingWorkOrder.NextStation.Oid));
                obj.ProductionOption = foldingWorkOrder.ProductionOption;
                obj.ProductionNote = foldingWorkOrder.ProductionNote;
                obj.QualityNote = foldingWorkOrder.QualityNote;
                obj.MinimumPieceWeight = foldingWorkOrder.MinimumPieceWeight;
                obj.MaximumPieceWeight = foldingWorkOrder.MaximumPieceWeight;
                obj.WayCount = foldingWorkOrder.WayCount;
                obj.FoldingWidth = foldingWorkOrder.FoldingWidth;
                obj.GramM2 = foldingWorkOrder.GramM2;
                obj.GramMetretul = foldingWorkOrder.GramMetretul;
                if (foldingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", foldingWorkOrder.OuterPacking.Oid));
                if (foldingWorkOrder.InnerPacking != null) obj.InnerPacking = objectSpace.FindObject<InnerPacking>(new BinaryOperator("Oid", foldingWorkOrder.InnerPacking.Oid));
                if (foldingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", foldingWorkOrder.Palette.Oid));
                obj.PaletteLayout = foldingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = foldingWorkOrder.PaletteBobbinCount;
                obj.DeckCount = foldingWorkOrder.DeckCount;
                obj.OuterPackingPackageCount = foldingWorkOrder.OuterPackingPackageCount;
                obj.InnerPackingPieceCount = foldingWorkOrder.InnerPackingPieceCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "BalloonCuttingWorkOrder")
            {
                BalloonCuttingWorkOrder obj = objectSpace.CreateObject<BalloonCuttingWorkOrder>();
                var balloonCuttingWorkOrder = ((BalloonCuttingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (balloonCuttingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", balloonCuttingWorkOrder.Machine.Oid));
                if (balloonCuttingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", balloonCuttingWorkOrder.NextStation.Oid));
                obj.ProductionOption = balloonCuttingWorkOrder.ProductionOption;
                obj.ProductionNote = balloonCuttingWorkOrder.ProductionNote;
                obj.QualityNote = balloonCuttingWorkOrder.QualityNote;
                obj.MinimumPieceWeight = balloonCuttingWorkOrder.MinimumPieceWeight;
                obj.MaximumPieceWeight = balloonCuttingWorkOrder.MaximumPieceWeight;
                obj.WayCount = balloonCuttingWorkOrder.WayCount;
                obj.GramM2 = balloonCuttingWorkOrder.GramM2;
                obj.GramMetretul = balloonCuttingWorkOrder.GramMetretul;
                if (balloonCuttingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", balloonCuttingWorkOrder.OuterPacking.Oid));
                if (balloonCuttingWorkOrder.InnerPacking != null) obj.InnerPacking = objectSpace.FindObject<InnerPacking>(new BinaryOperator("Oid", balloonCuttingWorkOrder.InnerPacking.Oid));
                if (balloonCuttingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", balloonCuttingWorkOrder.Palette.Oid));
                obj.PaletteLayout = balloonCuttingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = balloonCuttingWorkOrder.PaletteBobbinCount;
                obj.DeckCount = balloonCuttingWorkOrder.DeckCount;
                obj.OuterPackingPackageCount = balloonCuttingWorkOrder.OuterPackingPackageCount;
                obj.InnerPackingPieceCount = balloonCuttingWorkOrder.InnerPackingPieceCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "RegeneratedWorkOrder")
            {
                RegeneratedWorkOrder obj = objectSpace.CreateObject<RegeneratedWorkOrder>();
                var regeneratedWorkOrder = ((RegeneratedWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (regeneratedWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", regeneratedWorkOrder.Machine.Oid));
                if (regeneratedWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", regeneratedWorkOrder.NextStation.Oid));
                obj.ProductionOption = regeneratedWorkOrder.ProductionOption;
                obj.ProductionNote = regeneratedWorkOrder.ProductionNote;
                obj.QualityNote = regeneratedWorkOrder.QualityNote;
                obj.MaxiumPressure = regeneratedWorkOrder.MaxiumPressure;
                obj.ExtruderCurent = regeneratedWorkOrder.ExtruderCurent;
                obj.AkromelCurrent = regeneratedWorkOrder.AkromelCurrent;
                obj.AkromelSpeed = regeneratedWorkOrder.AkromelSpeed;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "CastRegeneratedWorkOrder")
            {
                CastRegeneratedWorkOrder obj = objectSpace.CreateObject<CastRegeneratedWorkOrder>();
                var castRegeneratedWorkOrder = ((CastRegeneratedWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (castRegeneratedWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", castRegeneratedWorkOrder.Machine.Oid));
                if (castRegeneratedWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", castRegeneratedWorkOrder.NextStation.Oid));
                obj.ProductionOption = castRegeneratedWorkOrder.ProductionOption;
                obj.ProductionNote = castRegeneratedWorkOrder.ProductionNote;
                obj.QualityNote = castRegeneratedWorkOrder.QualityNote;
                obj.MaxiumPressure = castRegeneratedWorkOrder.MaxiumPressure;
                obj.ExtruderCurent = castRegeneratedWorkOrder.ExtruderCurent;
                obj.AkromelCurrent = castRegeneratedWorkOrder.AkromelCurrent;
                obj.AkromelSpeed = castRegeneratedWorkOrder.AkromelSpeed;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "Eco6WorkOrder")
            {
                Eco6WorkOrder obj = objectSpace.CreateObject<Eco6WorkOrder>();
                var eco6WorkOrder = ((Eco6WorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (eco6WorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", eco6WorkOrder.Machine.Oid));
                if (eco6WorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", eco6WorkOrder.NextStation.Oid));
                obj.ProductionOption = eco6WorkOrder.ProductionOption;
                obj.ProductionNote = eco6WorkOrder.ProductionNote;
                obj.QualityNote = eco6WorkOrder.QualityNote;
                obj.M2 = eco6WorkOrder.M2;
                obj.GramM2 = eco6WorkOrder.GramM2;
                obj.MaterialColor = eco6WorkOrder.MaterialColor;
                obj.LotNumber = eco6WorkOrder.LotNumber;
                obj.S1Rate = eco6WorkOrder.S1Rate;
                obj.MRate = eco6WorkOrder.MRate;
                obj.S2Rate = eco6WorkOrder.S2Rate;
                if (eco6WorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", eco6WorkOrder.Bobbin.Oid));
                if (eco6WorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", eco6WorkOrder.OuterPacking.Oid));
                if (eco6WorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", eco6WorkOrder.Palette.Oid));
                obj.PaletteLayout = eco6WorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = eco6WorkOrder.PaletteBobbinCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "Eco6CuttingWorkOrder")
            {
                Eco6CuttingWorkOrder obj = objectSpace.CreateObject<Eco6CuttingWorkOrder>();
                var eco6CuttingWorkOrder = ((Eco6CuttingWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (eco6CuttingWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", eco6CuttingWorkOrder.Machine.Oid));
                if (eco6CuttingWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", eco6CuttingWorkOrder.NextStation.Oid));
                obj.ProductionOption = eco6CuttingWorkOrder.ProductionOption;
                obj.ProductionNote = eco6CuttingWorkOrder.ProductionNote;
                obj.QualityNote = eco6CuttingWorkOrder.QualityNote;
                obj.M2 = eco6CuttingWorkOrder.M2;
                obj.GramM2 = eco6CuttingWorkOrder.GramM2;
                obj.MaterialColor = eco6CuttingWorkOrder.MaterialColor;
                if (eco6CuttingWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", eco6CuttingWorkOrder.Bobbin.Oid));
                if (eco6CuttingWorkOrder.OuterPacking != null) obj.OuterPacking = objectSpace.FindObject<OuterPacking>(new BinaryOperator("Oid", eco6CuttingWorkOrder.OuterPacking.Oid));
                if (eco6CuttingWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", eco6CuttingWorkOrder.Palette.Oid));
                obj.PaletteLayout = eco6CuttingWorkOrder.PaletteLayout;
                obj.PaletteBobbinCount = eco6CuttingWorkOrder.PaletteBobbinCount;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "Eco6LaminationWorkOrder")
            {
                Eco6LaminationWorkOrder obj = objectSpace.CreateObject<Eco6LaminationWorkOrder>();
                var eco6LaminationWorkOrder = ((Eco6LaminationWorkOrder)(e.AcceptActionArgs.SelectedObjects[0]));
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (sender as DialogController).Tag));
                if (eco6LaminationWorkOrder.Machine != null) obj.Machine = objectSpace.FindObject<Machine>(new BinaryOperator("Oid", eco6LaminationWorkOrder.Machine.Oid));
                if (eco6LaminationWorkOrder.NextStation != null) obj.NextStation = objectSpace.FindObject<Station>(new BinaryOperator("Oid", eco6LaminationWorkOrder.NextStation.Oid));
                obj.ProductionOption = eco6LaminationWorkOrder.ProductionOption;
                obj.ProductionNote = eco6LaminationWorkOrder.ProductionNote;
                obj.QualityNote = eco6LaminationWorkOrder.QualityNote;
                if (eco6LaminationWorkOrder.Bobbin != null) obj.Bobbin = objectSpace.FindObject<Bobbin>(new BinaryOperator("Oid", eco6LaminationWorkOrder.Bobbin.Oid));
                obj.RollWeight = eco6LaminationWorkOrder.RollWeight;
                obj.MinimumRollWeight = eco6LaminationWorkOrder.MinimumRollWeight;
                obj.MaximumRollWeight = eco6LaminationWorkOrder.MaximumRollWeight;
                if (eco6LaminationWorkOrder.Palette != null) obj.Palette = objectSpace.FindObject<Palette>(new BinaryOperator("Oid", eco6LaminationWorkOrder.Palette.Oid));
                obj.PaletteBobbinCount = eco6LaminationWorkOrder.PaletteBobbinCount;
                obj.Length = eco6LaminationWorkOrder.Length;
                obj.RollDiameter = eco6LaminationWorkOrder.RollDiameter;
                obj.SubFilm = eco6LaminationWorkOrder.SubFilm;
                obj.SubFilmThickness = eco6LaminationWorkOrder.SubFilmThickness;
                obj.SubFilmWidth = eco6LaminationWorkOrder.SubFilmWidth;
                obj.SubFilmLength = eco6LaminationWorkOrder.SubFilmLength;
                obj.SubFilmTension = eco6LaminationWorkOrder.SubFilmTension;
                obj.TopFilm = eco6LaminationWorkOrder.TopFilm;
                obj.TopFilmThickness = eco6LaminationWorkOrder.TopFilmThickness;
                obj.TopFilmWidth = eco6LaminationWorkOrder.TopFilmWidth;
                obj.TopFilmLength = eco6LaminationWorkOrder.TopFilmLength;
                obj.TopFilmTension = eco6LaminationWorkOrder.TopFilmTension;
                obj.CoatingRollerSize = eco6LaminationWorkOrder.CoatingRollerSize;
                obj.SingleComponent = eco6LaminationWorkOrder.SingleComponent;
                obj.SingleComponentReservoir = eco6LaminationWorkOrder.SingleComponentReservoir;
                obj.SingleComponentHose = eco6LaminationWorkOrder.SingleComponentHose;
                obj.SingleComponentSprayGun = eco6LaminationWorkOrder.SingleComponentSprayGun;
                obj.DoubleComponent = eco6LaminationWorkOrder.DoubleComponent;
                obj.DoubleComponentA = eco6LaminationWorkOrder.DoubleComponentA;
                obj.DoubleComponentAReservoir = eco6LaminationWorkOrder.DoubleComponentAReservoir;
                obj.DoubleComponentAHose = eco6LaminationWorkOrder.DoubleComponentAHose;
                obj.DoubleComponentASprayGun = eco6LaminationWorkOrder.DoubleComponentASprayGun;
                obj.DoubleComponentB = eco6LaminationWorkOrder.DoubleComponentB;
                obj.DoubleComponentBReservoir = eco6LaminationWorkOrder.DoubleComponentBReservoir;
                obj.DoubleComponentBHose = eco6LaminationWorkOrder.DoubleComponentBHose;
                obj.DoubleComponentBSprayGun = eco6LaminationWorkOrder.DoubleComponentBSprayGun;
                obj.Performance = eco6LaminationWorkOrder.Performance;
                obj.LineSpeed = eco6LaminationWorkOrder.LineSpeed;
                obj.Tension = eco6LaminationWorkOrder.Tension;
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
        }

        private void CreateWorkOrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            SalesOrderDetail salesOrderDetail = (SalesOrderDetail)objectSpace.GetObject(e.CurrentObject);
            if(salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforPrinting)
            {
                PrintingWorkOrder obj = objectSpace.CreateObject<PrintingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforLamination)
            {
                LaminationWorkOrder obj = objectSpace.CreateObject<LaminationWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforFilming)
            {
                FilmingWorkOrder obj = objectSpace.CreateObject<FilmingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforRegenerated)
            {
                RegeneratedWorkOrder obj = objectSpace.CreateObject<RegeneratedWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCutting)
            {
                CuttingWorkOrder obj = objectSpace.CreateObject<CuttingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforSlicing)
            {
                SlicingWorkOrder obj = objectSpace.CreateObject<SlicingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastFilming)
            {
                CastFilmingWorkOrder obj = objectSpace.CreateObject<CastFilmingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforBalloonFilming)
            {
                BalloonFilmingWorkOrder obj = objectSpace.CreateObject<BalloonFilmingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastSlicing)
            {
                CastSlicingWorkOrder obj = objectSpace.CreateObject<CastSlicingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastTransfering)
            {
                CastTransferingWorkOrder obj = objectSpace.CreateObject<CastTransferingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforCastRegenerated)
            {
                CastRegeneratedWorkOrder obj = objectSpace.CreateObject<CastRegeneratedWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6)
            {
                Eco6WorkOrder obj = objectSpace.CreateObject<Eco6WorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Cutting)
            {
                Eco6CuttingWorkOrder obj = objectSpace.CreateObject<Eco6CuttingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (salesOrderDetail.SalesOrderStatus == SalesOrderStatus.WaitingforEco6Lamination)
            {
                Eco6LaminationWorkOrder obj = objectSpace.CreateObject<Eco6LaminationWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
        }

        protected internal void CreateWorkOrderAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (e.SelectedChoiceActionItem.Caption == "Eco1 Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                SalesOrderDetail salesOrderDetail = (SalesOrderDetail)objectSpace.GetObject(e.CurrentObject);
                PrintingWorkOrder obj = objectSpace.CreateObject<PrintingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", salesOrderDetail));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco1 Laminasyon Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                LaminationWorkOrder obj = objectSpace.CreateObject<LaminationWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco2 Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                FilmingWorkOrder obj = objectSpace.CreateObject<FilmingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco3 Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                RegeneratedWorkOrder obj = objectSpace.CreateObject<RegeneratedWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco4 Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CuttingWorkOrder obj = objectSpace.CreateObject<CuttingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco4 Dilme Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                SlicingWorkOrder obj = objectSpace.CreateObject<SlicingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco5 Dilme Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CastSlicingWorkOrder obj = objectSpace.CreateObject<CastSlicingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco5 CPP Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                BalloonFilmingWorkOrder obj = objectSpace.CreateObject<BalloonFilmingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco5 Stretch Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CastFilmingWorkOrder obj = objectSpace.CreateObject<CastFilmingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco5 Aktarma Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CastTransferingWorkOrder obj = objectSpace.CreateObject<CastTransferingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco5 Rejenere Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CastRegeneratedWorkOrder obj = objectSpace.CreateObject<CastRegeneratedWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco6 Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Eco6WorkOrder obj = objectSpace.CreateObject<Eco6WorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco6 Kesim Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Eco6CuttingWorkOrder obj = objectSpace.CreateObject<Eco6CuttingWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            if (e.SelectedChoiceActionItem.Caption == "Eco6 Laminasyon Üretim Siparişi")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Eco6LaminationWorkOrder obj = objectSpace.CreateObject<Eco6LaminationWorkOrder>();
                obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            }
            //if (e.SelectedChoiceActionItem.Caption == "Katlama Üretim Siparişi")
            //{
            //    IObjectSpace objectSpace = Application.CreateObjectSpace();
            //    FoldingWorkOrder obj = objectSpace.CreateObject<FoldingWorkOrder>();
            //    obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
            //    e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            //}
            //if (e.SelectedChoiceActionItem.Caption == "Balonlu Kesim Üretim Siparişi")
            //{
            //    IObjectSpace objectSpace = Application.CreateObjectSpace();
            //    BalloonCuttingWorkOrder obj = objectSpace.CreateObject<BalloonCuttingWorkOrder>();
            //    obj.SalesOrderDetail = objectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", (e.CurrentObject as SalesOrderDetail).Oid));
            //    e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, obj);
            //}
        }

        private void CopySalesOrderDetailAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SalesOrderDetail));
            SalesOrder salesOrder = (SalesOrder)View.CurrentObject;
            const string noteListViewId = "SalesOrderDetail_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(SalesOrderDetail), noteListViewId);
            collectionSource.Criteria["FilterbyContact"] = new BinaryOperator("SalesOrder.Contact", salesOrder.Contact.Oid);
            collectionSource.Criteria["FilterbyQualityBlock"] = new BinaryOperator("QualityBlock", false);
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }

        private void CopySalesOrderDetailAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesOrder salesOrder = (SalesOrder)View.CurrentObject;
            foreach (SalesOrderDetail detail in e.PopupWindowViewSelectedObjects)
            {
                DevExpress.Persistent.Base.Cloner cloner = new DevExpress.Persistent.Base.Cloner();
                var clonedSOD = View.ObjectSpace.FindObject<SalesOrderDetail>(new BinaryOperator("Oid", detail.Oid));
                SalesOrderDetail newSalesOrderDetail = (SalesOrderDetail)cloner.CloneTo(clonedSOD, detail.GetType());
                salesOrder.SalesOrderDetails.Add(newSalesOrderDetail);
                salesOrder.UpdateTotals();
            }
            if (((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                View.ObjectSpace.CommitChanges();
            }
        }
    }
}