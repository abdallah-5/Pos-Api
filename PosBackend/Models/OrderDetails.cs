﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PosBackend.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int OrdereId { get; set; }

        [ForeignKey("OrdereId")]
        public Order Order { get; set; }


        public int Count { get; set; }

        public decimal TotalItemPrice { get; set; }

        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }


    }
}
