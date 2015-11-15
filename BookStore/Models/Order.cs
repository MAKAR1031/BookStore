//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore.Models {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Order {
        public Order() {
            this.OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int ID { get; set; }
        public int ClientID { get; set; }
        public System.DateTime Date { get; set; }
        public string Address { get; set; }
        public int ConditionID { get; set; }

        public virtual Condition Condition { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}