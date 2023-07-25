using NeoBankWebApp.Models.Admin;
using NeoBankWebApp.Models.Login;
using NeoBankWebApp.Models.PaymentGateway;
using NeoBankWebApp.Models.Report;

namespace NeoBankWebApp.API_Service
{
    public interface IAPIService
    {
     HttpResponseMessage LoginVaildate(LoginRequest requestBody);
     HttpResponseMessage UserInfo(LoginRequest requestBody, string token, string CustomerID);
     HttpResponseMessage InitiatePayment(PaymentInitiate requestBody, string token, string CustomerID); 
     HttpResponseMessage PaymentGatewatReport(ConvertedRequest requestBody, string token, string CustomerID);
     HttpResponseMessage OnboardCompany(AddCompany addCompany, string token, string CustomerID);
     HttpResponseMessage OnboardUser(AddUser addUser, string token, string CustomerID);
        HttpResponseMessage GetOboardedUserList(GetOnboardedUser requestBody, string token, string CustomerID);

     HttpResponseMessage GetCompanyList();
    }
}
