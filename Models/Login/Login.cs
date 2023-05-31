using Microsoft.Build.Framework;

namespace NeoBankWebApp.Models.Login
{
    public class LoginRequest
    {
        [Required]
       public string UserName { get; set; }
        [Required]
        public string Password { get; set; }    
    }
    public class Tokens
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
    }
    public class UserInfoResponse
    {
        public string UserType { get; set; }
        public string CustomerId { get; set; }

    }

}
