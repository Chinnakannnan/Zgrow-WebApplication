using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.API_Service;
using NeoBankWebApp.Models.Common;
using NeoBankWebApp.Models.PaymentGateway;
using NeoBankWebApp.Models.Report;
using NeoBankWebApp.Rule_Common;
using Newtonsoft.Json;
using System.Composition;

namespace NeoBankWebApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IAPIService _clientService;
        public ReportController(IAPIService clientServiceInstance) => (_clientService) = (clientServiceInstance);
        public IActionResult Index()
        {
            string CustId = HttpContext.Session.GetString(Variables.CustomerID); if (CustId == null) { return RedirectToAction("Login", "Login"); }
           return View();
        }            
        public ActionResult Report(ActualRequest actualRequest)
        {
            try {
                string CustId = HttpContext.Session.GetString(Variables.CustomerID); if (CustId == null) { return RedirectToAction("Login", "Login"); }

                if (string.IsNullOrEmpty(actualRequest.SelectedReport)||string.IsNullOrEmpty(actualRequest.FromDate) || string.IsNullOrEmpty(actualRequest.ToDate.ToString()))
                        return Json("1");

                ConvertedRequest values = new ConvertedRequest();
                values.CustomerId = HttpContext.Session.GetString(Variables.CustomerID);
                values.FromDate = DateTime.Parse(actualRequest.FromDate.ToString()).ToString("yyyy-MM-dd");
                values.ToDate = DateTime.Parse(actualRequest.ToDate.ToString()).ToString("yyyy-MM-dd");

                if (actualRequest.SelectedReport == "2")
                {
                    using (HttpResponseMessage responseMessages = _clientService.PaymentGatewatReport(values, HttpContext.Session.GetString(Variables.AccessToken), HttpContext.Session.GetString(Variables.CustomerID)))
                    {
                        string reportInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                        if (responseMessages.IsSuccessStatusCode)
                        {
                            ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(reportInfo);
                            List<ReportResponse> Report = JsonConvert.DeserializeObject<List<ReportResponse>>(objResult.Data.ToString());

                            return Json(Report);
                        }

                        return Json("");
                    }
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex) { throw; }

        }
    }
}
