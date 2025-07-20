namespace api_for_sambapos.Models
{
    public class MenuItemPortions
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int MenuItemId { get; set; }
        public decimal Price_Amount { get; set; }
        public MenuItem MenuItem { get; set; } = new MenuItem();
    }
}
