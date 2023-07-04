namespace NeoBankWebApp.Rule_Common
{
    public class Constants
    {
        public const string CompanyCode ="ZG001";
        public const string BaseUrl = "https://localhost:7117/";
        public const string ContentType = "Content-Type";
        public const string ApplicationJson = "application/json";
        public const string LoginValidate = "/api/Auth/authenticate";
        public const string GetUserInformation = "/api/User/UserInformation";
        public const string PaymentInitiate = "/api/PaymentGateway/InitiatePay";
        public const string PaymentInitiateExternal = "/api/PaymentGateway/InitiatePayExternal";
        public const string LinkReport = "/api/Report/PaymentGatewayReport";
        public const string OnboardCompany = "/api/Admin/AddCompany";
        public const string OnboardUser = "/api/Admin/AddUser";
        public const string GetCompanyList = "/api/Admin/GetCompanyList";
    }

    public class Variables
    {
        public const string AccessToken = "access_token";
        public const string RefreshToken = "refresh_token";
        public const string TokenGenarationTime = "token_genaration_time";
        public const string CustomerID = "customer_id"; 
        public const string UserType = "user_type";
        public const string UserName = "user_name";

    }



}
