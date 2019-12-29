using System.ComponentModel.DataAnnotations;

namespace Student.Web.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Name { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class CustomExternalLoginInfo
    {
        public string LoginProvider { get; set; }
        public string Email { get; set; }
        public string ProviderKey { get; set; }
    }
}
