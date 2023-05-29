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
}
