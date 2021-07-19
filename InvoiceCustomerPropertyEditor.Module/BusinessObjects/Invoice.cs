using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCustomerPropertyEditor.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Invoice : XPObject, ICustomerSearch
    {
        public Invoice(Session session) : base(session)
        { }


        Customer customer;
        DateTime paymentDate;
        DateTime invoiceDate;
        string notes;
        string invoiceNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InvoiceNumber
        {
            get => invoiceNumber;
            set => SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }


        public DateTime InvoiceDate
        {
            get => invoiceDate;
            set => SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value);
        }


        public DateTime PaymentDate
        {
            get => paymentDate;
            set => SetPropertyValue(nameof(PaymentDate), ref paymentDate, value);
        }


        [EditorAlias(EditorAliases.DetailPropertyEditor)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            InvoiceDate = DateTime.Now;
            PaymentDate = DateTime.Now.AddDays(15);
            Customer = new Customer(Session);
        }

        //todo: Add InvoiceItems
    }
}
