namespace CartingService.DAL.Entities
{
    public class CartItemDAO
    {
        public int Id { get; set; }
        public ItemDAO Item { get; set; }
        public double Quantity { get; set; }
        public CartDAO Cart { get; set; }
    }
}
