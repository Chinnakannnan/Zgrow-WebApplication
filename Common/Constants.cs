namespace NeoBankWebApp.Rule_Common
{
    public class Constants
    {       
        public const string BaseUrl = "https://localhost:7117/";
        public const string ContentType = "Content-Type";
        public const string ApplicationJson = "application/json";
        public const string LoginValidate = "/api/Auth/authenticate";
        public const string GetUserInformation = "/api/User/UserInformation"; 

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
