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
    
    public partial class active_debts
    {
        public int id { get; set; }
        public Nullable<int> debt_id { get; set; }
        public Nullable<int> client_id { get; set; }
    
        public virtual clients_table clients_table { get; set; }
        public virtual debt_table debt_table { get; set; }
    }
}