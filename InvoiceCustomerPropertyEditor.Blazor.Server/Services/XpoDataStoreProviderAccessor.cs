using System;
using DevExpress.ExpressApp.Xpo;

namespace InvoiceCustomerPropertyEditor.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
