namespace PosBackend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }

        public string UserName { get; set; }
        public string Phone { get; set; }
    }
}
