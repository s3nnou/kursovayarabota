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
    
    public partial class virtual_money
    {
        public int id { get; set; }
        public Nullable<int> client_id { get; set; }
        public Nullable<decimal> money { get; set; }
        public Nullable<int> service_id { get; set; }
    
        public virtual clients_table clients_table { get; set; }
    }
}