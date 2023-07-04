using NeoBankWebApp.Models.Login;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using NeoBankWebApp.Rule_Common;
using NeoBankWebApp.Models.PaymentGateway;
using NeoBankWebApp.Models.Report;
using NeoBankWebApp.Models.Admin;

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

        public HttpResponseMessage InitiatePayment(PaymentInitiate requestBody, string token)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, Constants.ApplicationJson);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                _client.DefaultRequestHeaders.Add("CompanyCode", Constants.CompanyCode);
                return _client.PostAsync(Constants.PaymentInitiate, stringContent).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
       
        }
       
        public HttpResponseMessage PaymentGatewatReport(ConvertedRequest requestBody, string token)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, Constants.ApplicationJson);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                return _client.PostAsync(Constants.LinkReport, stringContent).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public HttpResponseMessage OnboardCompany(AddCompany addCompany,string token)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(addCompany), Encoding.UTF8, Constants.ApplicationJson);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                _client.DefaultRequestHeaders.Add("CompanyCode", Constants.CompanyCode);
                return _client.PostAsync(Constants.OnboardCompany, stringContent).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public HttpResponseMessage OnboardUser(AddUser addUser, string token)
        {
            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(addUser), Encoding.UTF8, Constants.ApplicationJson);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                _client.DefaultRequestHeaders.Add("CompanyCode", Constants.CompanyCode);
                return _client.PostAsync(Constants.OnboardUser, stringContent).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HttpResponseMessage GetCompanyList()
        {
            try
            {
                //var stringContent = new StringContent("", Encoding.UTF8, Constants.ApplicationJson); 
                _client.DefaultRequestHeaders.Add("CompanyCode", Constants.CompanyCode);
                return _client.GetAsync(Constants.GetCompanyList).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


