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
    
    public partial class metrics_archive
    {
        public int id { get; set; }
        public Nullable<int> service_id { get; set; }
        public Nullable<int> client_id { get; set; }
        public Nullable<int> month_id { get; set; }
        public Nullable<decimal> usage { get; set; }
    
        public virtual clients_table clients_table { get; set; }
        public virtual months_2020_table months_2020_table { get; set; }
        public virtual service_type service_type { get; set; }
    }
}
