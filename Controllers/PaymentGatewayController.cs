using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.API_Service;
using NeoBankWebApp.Models.Common;
using NeoBankWebApp.Models.Login;
using NeoBankWebApp.Models.PaymentGateway;
using NeoBankWebApp.Rule_Common;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Plugins;
using System.Security.Policy;

namespace NeoBankWebApp.Controllers
{
    public class PaymentGatewayController : Controller
    {
        private readonly IAPIService _clientService;
        public PaymentGatewayController(IAPIService clientServiceInstance) => (_clientService) = (clientServiceInstance);
        public IActionResult Index()
        {
            
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(Variables.CustomerID)))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public IActionResult Iframe()
        {
            return View();
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Failure()
        {
            return View();
        }
        public IActionResult Link()
        {
            return View();
        }
        public IActionResult InitiatePayment(PaymentInitiateRequest PaymentInitiateRequest)
        {
            try
            {
                PaymentInitiate values = new PaymentInitiate();
                values.CustomerId = HttpContext.Session.GetString(Variables.CustomerID);
                values.Amount = PaymentInitiateRequest.Amount;
                values.Name = PaymentInitiateRequest.Name;
                values.MobileNumber = PaymentInitiateRequest.MobileNumber;
                values.MailId = PaymentInitiateRequest.MailId;
                values.Description = PaymentInitiateRequest.Description;
                values.IpAddress= PaymentInitiateRequest.IpAddress;
                values.Latitude= PaymentInitiateRequest.Latitude;
                values.Longitude = PaymentInitiateRequest.Longitude;
                using (HttpResponseMessage responseMessages = _clientService.InitiatePayment(values, HttpContext.Session.GetString(Variables.AccessToken)))
                {
                    string linkInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                    if (responseMessages.IsSuccessStatusCode)
                    {
                        ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(linkInfo);
                        if (objResult.StatusCode == "000")
                        {
                            LinkResponseFinal response = JsonConvert.DeserializeObject<LinkResponseFinal>(objResult.Data.ToString());
                            ViewBag.Link = response.Link;
                            ViewBag.TxnID = response.TxnID;
                            if (PaymentInitiateRequest.Link == "Link")
                                return View("Link");
                            else
                                return View("Iframe");
                        }
                    }
                    return View("Index");
                }

            }
            catch (Exception ex)
            {
                return View("Index");
            }

          

            

        }
    /*    public IActionResult InitiatePaymentExternal([FromBody]PaymentInitiateRequestwol PaymentInitiateRequest) // comes under zgrow
        {
            PaymentInitiate values = new PaymentInitiate();
            values.CustomerId = PaymentInitiateRequest.CustomerId;
            values.Amount = PaymentInitiateRequest.Amount;
            values.Name = PaymentInitiateRequest.Name;
            values.MobileNumber = PaymentInitiateRequest.MobileNumber;
            values.MailId = PaymentInitiateRequest.MailId;
            values.Description = PaymentInitiateRequest.Description;

            using (HttpResponseMessage responseMessages = _clientService.InitiatePaymentExternal(values, HttpContext.Session.GetString(Variables.AccessToken)))
            {
                string linkInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                if (responseMessages.IsSuccessStatusCode)
                {
                    ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(linkInfo);

                    if (objResult.StatusCode == "000")
                    {
                        LinkResponseFinal response = JsonConvert.DeserializeObject<LinkResponseFinal>(objResult.Data.ToString());
                        ViewBag.Link = response.Link;
                        ViewBag.TxnID = response.TxnID; 
                        return Redirect(response.Link);         
                    }
                }
                return View("Index");
            }

        }
*/
        public IActionResult InitiatePaymentExternal(string IpAddress, string Latitude, string Longitude, string CustomerId, string RedirectURL, string Amount,string Name,string MobileNumber,string MailId,string Description)
        {
            try {
                
                PaymentInitiate values = new PaymentInitiate();
                values.CustomerId = CustomerId;
                values.Amount = Amount;
                values.Name = Name;
                values.MobileNumber = MobileNumber;
                values.MailId = MailId;
                values.Description = Description;
                values.IpAddress = IpAddress;
                values.Latitude = Latitude;
                values.Longitude = Longitude;
                HttpResponseMessage responseMessages = _clientService.InitiatePayment(values, "");
                string linkInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(linkInfo);
                LinkResponseFinal response = JsonConvert.DeserializeObject<LinkResponseFinal>(objResult.Data.ToString());
                ViewBag.Link = response.Link;
                ViewBag.TxnID = response.TxnID;
                return Redirect(response.Link);
            }
            catch (Exception ex)
            {
                return View("");
            }                       

        } 
    
    }
}