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
    
    public partial class staff_acc_table
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Nullable<int> worker_id { get; set; }
        public Nullable<int> role_id { get; set; }
    
        public virtual worker_table worker_table { get; set; }
    }
}
