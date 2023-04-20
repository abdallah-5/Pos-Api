namespace PosBackend.Models.ResponseDTO
{
    public class GetItemByResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string Picture { get; set; }


        public IEnumerable<SizeModel> Sizes { get; set; }


        
    }
}
