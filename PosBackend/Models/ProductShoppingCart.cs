using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PosBackend.Models
{
    public class ProductShoppingCart
    {
        public ProductShoppingCart()
        {
            Count = 1;
        }
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }


        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
        public int Count { get; set; }
        

        public decimal TotalItemPrice { get; set; }
    }
}
