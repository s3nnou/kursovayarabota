//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PService
{
    using System;
    using System.Collections.Generic;
    
    public partial class service_payments
    {
        public int id { get; set; }
        public Nullable<int> client_id { get; set; }
        public Nullable<int> service_id { get; set; }
        public Nullable<int> date { get; set; }
        public Nullable<decimal> usage { get; set; }
        public Nullable<decimal> price { get; set; }
    
        public virtual clients_table clients_table { get; set; }
        public virtual service_type service_type { get; set; }
    }
}
