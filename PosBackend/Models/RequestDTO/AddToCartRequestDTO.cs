using System.ComponentModel.DataAnnotations;

namespace PosBackend.Models.RequestDTO
{
    public class AddToCartRequestDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SizeId { get; set; }


    }
}
