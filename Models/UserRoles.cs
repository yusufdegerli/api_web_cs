namespace api_for_sambapos.Models
{
    public class UserRoles
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
