namespace PosBackend.Models.ResponseDTO
{
    public class AllproductsInCartResponseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public decimal TotalItemPrice { get; set; }

    }
}
