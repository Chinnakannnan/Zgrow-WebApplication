using NeoBankWebApp.Models.Login;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using NeoBankWebApp.Rule_Common;

namespace NeoBankWebApp.API_Service
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public APIService(HttpClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = client;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _client.BaseAddress = new Uri(_configuration.GetSection("ApplicationSettings").GetSection("BaseUrl").Value);
            _client.DefaultRequestHeaders.TryAddWithoutValidation(Constants.ContentType, Constants.ApplicationJson);

        }
        public  HttpResponseMessage LoginVaildate(LoginRequest requestBody)      
        {

            try
            {
                string clientId = _configuration.GetSection("ApplicationSettings").GetSection("ClientId").Value;
                string clientSecret = _configuration.GetSection("ApplicationSettings").GetSection("ClientSecrect").Value;
                var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, Constants.ApplicationJson);
                _client.DefaultRequestHeaders.Add("clientId", clientId);
                _client.DefaultRequestHeaders.Add("clientSecret", clientSecret);
                return _client.PostAsync(Constants.LoginValidate, stringContent).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
    

        }

        public HttpResponseMessage UserInfo(LoginRequest requestBody, string token)
        {

            try
            {               
                var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, Constants.ApplicationJson);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer "+ token); 
                return _client.PostAsync(Constants.GetUserInformation, stringContent).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
