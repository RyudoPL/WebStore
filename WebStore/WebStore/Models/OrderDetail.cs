namespace WebStore.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal EndPrice { get; set; }
        
        public virtual Product product { get; set; }
        public virtual Order order { get; set; }
    }
}