using NeoBankWebApp.Models.Login;

namespace NeoBankWebApp.API_Service
{
    public interface IAPIService
    {
     HttpResponseMessage LoginVaildate(LoginRequest requestBody);
     HttpResponseMessage UserInfo(LoginRequest requestBody, string token);
    }
}
