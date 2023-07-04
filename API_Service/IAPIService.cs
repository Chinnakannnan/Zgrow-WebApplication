using NeoBankWebApp.Models.Admin;
using NeoBankWebApp.Models.Login;
using NeoBankWebApp.Models.PaymentGateway;
using NeoBankWebApp.Models.Report;

namespace NeoBankWebApp.API_Service
{
    public interface IAPIService
    {
     HttpResponseMessage LoginVaildate(LoginRequest requestBody);
     HttpResponseMessage UserInfo(LoginRequest requestBody, string token);
     HttpResponseMessage InitiatePayment(PaymentInitiate requestBody, string token); 
     HttpResponseMessage PaymentGatewatReport(ConvertedRequest requestBody, string token);
     HttpResponseMessage OnboardCompany(AddCompany addCompany, string token);
     HttpResponseMessage OnboardUser(AddUser addUser, string token);
     HttpResponseMessage GetCompanyList();
    }
}
