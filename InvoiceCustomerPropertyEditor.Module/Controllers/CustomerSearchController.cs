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
        private readonly string AllowEditKey = "ReadOnlyOsoba";

        private readonly PopupWindowShowAction wybierzNajemceAction;
        private readonly ParametrizedAction wyszukajNajemceAction;
        private readonly SimpleAction nowyCustomerAction;

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

            wyszukajNajemceAction = new ParametrizedAction(this, $"{GetType().FullName}{nameof(wyszukajNajemceAction)}", "WpisywanieNajemcyCategory", typeof(string))
            {
                Caption = "Find customer",
                ImageName = "Szukaj",
                Shortcut = "F6",
                PaintStyle = ActionItemPaintStyle.Image,

            };
            wyszukajNajemceAction.Execute += WyszukajNajemceAction_Execute;

            nowyCustomerAction = new SimpleAction(this, $"{GetType().FullName}{nameof(nowyCustomerAction)}", "WpisywanieNajemcyCategory")
            {
                Caption = "New Customer",
                ImageName = "Actions_AddCircled",
                Shortcut = "F7",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
            };
            nowyCustomerAction.Execute += NowyCustomerAction_Execute;
        }

        private void NowyCustomerAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var obecnyCustomer = ((ICustomerSearch)View.CurrentObject).Customer;
            RemoveUnnecesaryCustomerObject(obecnyCustomer);
            ((ICustomerSearch)View.CurrentObject).Customer = ObjectSpace.CreateObject<Customer>();

        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var customer = ((ICustomerSearch)View.CurrentObject).Customer;

        }

        private void WyszukajNajemceAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var objectSpace = Application.CreateObjectSpace();
            var parent = (ICustomerSearch)View.CurrentObject;
            string peselNip = WalidacjaNumeruPeselNip(e);

            var customer = PrzypisanieNajemcyZBazy(objectSpace, peselNip);
            if (customer != null)
            {
                AssignNewCustomer(parent, customer, ObjectSpace);
  
                return;
            }


            if (parent.Customer != null && parent.Customer.Session.IsNewObject(parent.Customer))
            {
                ClearFields(parent.Customer);
            }
            else
            {
                parent.Customer = ObjectSpace.CreateObject<Customer>();
            }
        }

        private string WalidacjaNumeruPeselNip(ParametrizedActionExecuteEventArgs e)
       {
        //    if (e.ParameterCurrentValue == null || string.IsNullOrEmpty(e.ParameterCurrentValue.ToString()))
        //    {
        //        throw new UserFriendlyException("Pesel/NIP nie może być pusty.");
        //    }
        //    if (e.ParameterCurrentValue.ToString().Length < 9)
        //    {
        //        throw new UserFriendlyException("Pesel/NIP jest za krótki.");
        //    }
        //    if (e.ParameterCurrentValue.ToString().Length > 11)
        //    {
        //        throw new UserFriendlyException("Pesel/NIP jest za długi.");
        //    }

           return e.ParameterCurrentValue.ToString();
        }

        private Customer PrzypisanieNajemcyZBazy(IObjectSpace objectSpace, string peselNip)
        {
            BinaryOperator     binaryOperator = new BinaryOperator("VatNumer", peselNip);
     
            var Customer = objectSpace.FindObject<Customer>(binaryOperator);
            if (Customer == null)
            {
                return null;
            }

            return Customer;
        }




        private static void AssignNewCustomer(ICustomerSearch parent, Customer Customer, IObjectSpace mainObjectSpace)
        {
            var oldCustomer = parent.Customer;
            parent.Customer = mainObjectSpace.GetObject(Customer);
            RemoveUnnecesaryCustomerObject(oldCustomer);
        }

        private static void RemoveUnnecesaryCustomerObject(Customer oldCustomer)
        {
            if (oldCustomer != null && oldCustomer.Session.IsNewObject(oldCustomer))
            {
              //  oldCustomer.Delete();
            }
        }

        private void ClearFields(Customer Customer)
        {

            Customer.VatNumer = "";
            Customer.Name = "";
            Customer.Notes = "";

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
