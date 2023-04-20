namespace PosBackend.Models.ResponseDTO
{
    public class AllProductResponseDTO
    {
        public int Id { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }


        public string Picture { get; set; }
        public decimal Price { get; set; }
    }
}
