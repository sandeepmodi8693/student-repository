namespace Student.Web.Models
{
    public class RegisterModel: User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
