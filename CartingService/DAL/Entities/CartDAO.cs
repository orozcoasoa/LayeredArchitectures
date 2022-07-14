namespace CartingService.DAL.Entities
{
    public class CartDAO
    {
        public Guid Id { get; set; }
        public List<CartItemDAO> Items { get; set; }
    }
}
