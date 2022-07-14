namespace CartingService.BLL
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<Item> Items { get; set; }
    }
}
