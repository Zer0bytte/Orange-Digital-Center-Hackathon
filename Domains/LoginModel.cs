namespace Domains
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CategoryUpdateModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}