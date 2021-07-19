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
    public class Customer : XPObject
    {
        public Customer(Session session) : base(session)
        { }


        string notes;
        string address;
        string vatNumer;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string VatNumer
        {
            get => vatNumer;
            set => SetPropertyValue(nameof(VatNumer), ref vatNumer, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

    }
}
