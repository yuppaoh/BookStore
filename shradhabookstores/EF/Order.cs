//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace shradhabookstores.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string PlaceOfDelivery { get; set; }
        public Nullable<int> Distance { get; set; }
        public Nullable<int> PaymentId { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string OrderStatus { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Payment Payment { get; set; }
    }
}