using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.Controllers
{
    public partial class ShippingPlanViewController : ViewController
    {
        private ChoiceActionItem setStatusItem;

        public ShippingPlanViewController()
        {
            InitializeComponent();

            SetShippingPlanStatusAction.Items.Clear();
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(ShippingPlan), "ShippingPlanStatus"), null);
            SetShippingPlanStatusAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(ShippingPlanStatus));
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

        private void CreateExpeditionByChoiceAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            Expedition expedition = objectSpace.CreateObject<Expedition>();
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            foreach (Object item in objectsToProcess)
            {
                ShippingPlan objInNewObjectSpace = (ShippingPlan)objectSpace.GetObject(item);
                ExpeditionDetail expeditionDetail = objectSpace.CreateObject<ExpeditionDetail>();
                expeditionDetail.SalesOrderDetail = objInNewObjectSpace.SalesOrderDetail;
                expeditionDetail.Unit = objInNewObjectSpace.Unit;
                expeditionDetail.Quantity = objInNewObjectSpace.Quantity;
                expeditionDetail.cUnit = objInNewObjectSpace.cUnit;
                expeditionDetail.cQuantity = objInNewObjectSpace.cQuantity;
                expeditionDetail.ShippingPlan = objInNewObjectSpace;
                expedition.ExpeditionDetails.Add(expeditionDetail);
            }

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, expedition);
        }

        private void AddExistingExpeditionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            const string expedition_ListView = "Expedition_ListView_Copy";
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Expedition), expedition_ListView);
            e.ShowViewParameters.CreatedView = Application.CreateListView(expedition_ListView, collectionSource, true);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.Tag = (e.CurrentObject as ShippingPlan).Oid;
            dc.Accepting += Dc_Accepting;
            if (e.ShowViewParameters.Controllers.Count == 0) e.ShowViewParameters.Controllers.Add(dc);
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            if (e.AcceptActionArgs.SelectedObjects[0].GetType().Name == "Expedition")
            {
                Expedition expedition = (Expedition)objectSpace.GetObject(e.AcceptActionArgs.SelectedObjects[0]);
                ExpeditionDetail expeditionDetail = null;
                ShippingPlan shippingPlan = objectSpace.FindObject<ShippingPlan>(new BinaryOperator("Oid", (sender as DialogController).Tag));

                ExpeditionDetail existsExpeditionDetail = objectSpace.FindObject<ExpeditionDetail>(CriteriaOperator.Parse("Expedition = ? and SalesOrderDetail = ?", expedition, shippingPlan.SalesOrderDetail));
                if (existsExpeditionDetail != null)
                {
                    existsExpeditionDetail.Quantity += shippingPlan.Quantity;
                    existsExpeditionDetail.cQuantity += shippingPlan.cQuantity;
                    expeditionDetail = existsExpeditionDetail;
                }
                else
                {
                    ExpeditionDetail newExpeditionDetail = objectSpace.CreateObject<ExpeditionDetail>();
                    newExpeditionDetail.SalesOrderDetail = shippingPlan.SalesOrderDetail;
                    newExpeditionDetail.Unit = shippingPlan.Unit;
                    newExpeditionDetail.Quantity = shippingPlan.Quantity;
                    newExpeditionDetail.ShippingPlan = shippingPlan;
                    expedition.ExpeditionDetails.Add(newExpeditionDetail);
                    expeditionDetail = newExpeditionDetail;
                }
                shippingPlan.ExpeditionDetail = expeditionDetail;
                shippingPlan.ShippingPlanStatus = ShippingPlanStatus.WaitingforLoading;

                DeliveryDetail deliveryDetail = null;
                DeliveryDetail existDeliveryDetail = objectSpace.FindObject<DeliveryDetail>(CriteriaOperator.Parse("Delivery.Expedition = ? and SalesOrderDetail.SalesOrder.Contact = ? and SalesOrderDetail.SalesOrder.ShippingContact = ? and SalesOrderDetail.SalesOrder.TransportType = ?", expedition, shippingPlan.SalesOrderDetail.SalesOrder.Contact, shippingPlan.SalesOrderDetail.SalesOrder.ShippingContact, shippingPlan.SalesOrderDetail.SalesOrder.TransportType));
                if (existDeliveryDetail != null)
                {
                    if (existDeliveryDetail.SalesOrderDetail == shippingPlan.SalesOrderDetail)
                    {
                        existDeliveryDetail.Quantity += shippingPlan.Quantity;
                        existDeliveryDetail.cQuantity += shippingPlan.cQuantity;
                        deliveryDetail = existDeliveryDetail;
                    }
                    else
                    {
                        DeliveryDetail newDeliveryDetail = objectSpace.CreateObject<DeliveryDetail>();
                        newDeliveryDetail.Delivery = existDeliveryDetail.Delivery;
                        newDeliveryDetail.SalesOrderDetail = shippingPlan.SalesOrderDetail;
                        newDeliveryDetail.Unit = shippingPlan.Unit;
                        newDeliveryDetail.Quantity = shippingPlan.Quantity;
                        newDeliveryDetail.cUnit = shippingPlan.cUnit;
                        newDeliveryDetail.cQuantity = shippingPlan.cQuantity;
                        newDeliveryDetail.ExpeditionDetail = expeditionDetail;
                        newDeliveryDetail.Delivery.DeliveryDetails.Add(newDeliveryDetail);
                        deliveryDetail = newDeliveryDetail;
                    }
                }
                else
                {
                    Delivery delivery = objectSpace.CreateObject<Delivery>();
                    delivery.Contact = shippingPlan.SalesOrderDetail.SalesOrder.Contact;
                    delivery.ShippingContact = shippingPlan.SalesOrderDetail.SalesOrder.ShippingContact;
                    delivery.Route = expedition.Route;
                    delivery.TransportType = shippingPlan.SalesOrderDetail.SalesOrder.TransportType;
                    delivery.Expedition = expedition;

                    DeliveryDetail newDeliveryDetail = objectSpace.CreateObject<DeliveryDetail>();
                    newDeliveryDetail.Delivery = delivery;
                    newDeliveryDetail.SalesOrderDetail = shippingPlan.SalesOrderDetail;
                    newDeliveryDetail.Unit = shippingPlan.Unit;
                    newDeliveryDetail.Quantity = shippingPlan.Quantity;
                    newDeliveryDetail.cUnit = shippingPlan.cUnit;
                    newDeliveryDetail.cQuantity = shippingPlan.cQuantity;
                    newDeliveryDetail.ExpeditionDetail = expeditionDetail;
                    delivery.DeliveryDetails.Add(newDeliveryDetail);
                    deliveryDetail = newDeliveryDetail;
                }
                expeditionDetail.DeliveryDetail = deliveryDetail;
            }
            objectSpace.CommitChanges();
        }

        private void SetShippingPlanStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    ShippingPlan objInNewObjectSpace = (ShippingPlan)objectSpace.GetObject(obj);
                    if (objInNewObjectSpace.SalesOrderDetail.SalesOrder.PaymentBlockage)
                    {
                        throw new UserFriendlyException("Siparişte ödeme blokajı olduğundan durumunu değiştiremezsiniz. Müşteri temsilcisi ile görüşünüz. !");
                    }
                    if (objInNewObjectSpace.SalesOrderDetail.SalesOrder.Blockage == Blockage.NextMonthInvoice)
                    {
                        if (objInNewObjectSpace.SalesOrderDetail.LineDeliveryDate.Year == DateTime.Now.Year)
                        {
                            if (objInNewObjectSpace.SalesOrderDetail.LineDeliveryDate.Month > DateTime.Now.Month)
                            {
                                throw new UserFriendlyException("Siparişte sonraki ay fatura blokajı olduğundan durumunu değiştiremezsiniz. Müşteri temsilcisi ile görüşünüz. !");
                            }
                        }
                    }
                    if (objInNewObjectSpace.SalesOrderDetail.SalesOrder.Blockage == Blockage.StoreProduction)
                    {
                        throw new UserFriendlyException("Sipariş stok üretim beklemeye alınmış. Müşteri temsilcisi ile görüşünüz. !");
                    }
                    if (objInNewObjectSpace.SalesOrderDetail.DeliveryBlockType == DeliveryBlockType.DeadlineShipment && DateTime.Now.Date < objInNewObjectSpace.SalesOrderDetail.LineDeliveryDate.Date.AddDays(-5))
                    {
                            throw new UserFriendlyException("Sipariş için Termininde Sevk seçilmiş. Müşteri temsilcisi ile görüşünüz. !");
                    }

                    objInNewObjectSpace.ShippingPlanStatus = (ShippingPlanStatus)e.SelectedChoiceActionItem.Data;
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

        private void RemoveFromExpeditionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ShippingPlan shippingPlan = (ShippingPlan)objectSpace.GetObject(e.CurrentObject);
            if (shippingPlan.ExpeditionDetail.DeliveryDetail != null)
            {
                if (shippingPlan.ExpeditionDetail.DeliveryDetail.Delivery != null)
                {
                    if (shippingPlan.ExpeditionDetail.DeliveryDetail.Delivery.DeliveryDetails.Count == 1)
                    {
                        shippingPlan.ExpeditionDetail.DeliveryDetail.Delivery.Delete();
                    }
                    else shippingPlan.ExpeditionDetail.DeliveryDetail.Delete();
                }
            }
            if (shippingPlan.ExpeditionDetail != null)
            {
                if (shippingPlan.ExpeditionDetail.Expedition.ExpeditionDetails.Count == 1)
                {
                    shippingPlan.ExpeditionDetail.Expedition.Delete();
                }
                else shippingPlan.ExpeditionDetail.Delete();
                shippingPlan.ExpeditionDetail = null;
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
    }
}
