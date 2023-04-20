namespace PosBackend.Models.ResponseDTO
{
    public class OrderDetailsResponseDTO
    {
        
        public int OrderId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public string SizeName { get; set; }
        public int Count { get; set; }

        public decimal TotalItemPrice { get; set; }
    }
}
