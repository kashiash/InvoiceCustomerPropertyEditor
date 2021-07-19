using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using InvoiceCustomerPropertyEditor.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCustomerPropertyEditor.Module.Controllers
{
    public class CustomerSearchController : ObjectViewController<DetailView, ICustomerSearch>
    {


        private readonly PopupWindowShowAction wybierzNajemceAction;


        public CustomerSearchController()
        {
            wybierzNajemceAction = new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(wybierzNajemceAction)}", "WpisywanieNajemcyCategory")
            {
                Caption = "Select customer",
                ImageName = "Wybierz",
                Shortcut = "F5",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
            };
            wybierzNajemceAction.CustomizePopupWindowParams += WybierzNajemceAction_CustomizePopupWindowParams;
            wybierzNajemceAction.Execute += WybierzNajemceAction_Execute;


        }

        private void WybierzNajemceAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count == 0)
            {
                return;
            }

            var selectedCustomer = e.PopupWindowViewSelectedObjects[0] as Customer;
            var Customer = ObjectSpace.GetObject(selectedCustomer);
            if (Customer == null)
            {
                return;
            }

            var parent = (ICustomerSearch)View.CurrentObject;
            Customer oldCustomer = parent?.Customer;
            parent.Customer = Customer;

            if (oldCustomer != null && oldCustomer.Session.IsNewObject(oldCustomer))
            {
                oldCustomer.Delete();
            }

            View.Refresh();
            ObjectSpace.SetModified(parent);
        }

        private void WybierzNajemceAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            e.View = Application.CreateListView(typeof(Customer), true);
        }


    }
}
