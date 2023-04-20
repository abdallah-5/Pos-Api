using PosBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace PosBackend.Models.RequestDTO
{
    public class AddItemRequestDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Picture { get; set; }

        [Required]
        public List<SizeModel> Sizes { get; set; }
    }
}
