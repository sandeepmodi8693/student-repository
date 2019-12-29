using System;

namespace Student.Models
{
    public class ServiceResponse
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }
}
