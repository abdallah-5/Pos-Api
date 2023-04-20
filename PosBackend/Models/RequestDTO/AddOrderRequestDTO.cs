namespace PosBackend.Models.RequestDTO
{
    public class AddOrderRequestDTO
    {

        
        public decimal TotalPrice { get; set; }

        public string UserName { get; set; }
        public string Phone { get; set; }
    }
}
