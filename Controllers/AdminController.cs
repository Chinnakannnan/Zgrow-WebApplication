using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.API_Service;
using NeoBankWebApp.Models.Admin;
using NeoBankWebApp.Models.Common;
using NeoBankWebApp.Rule_Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
using NeoBankWebApp.Models.Report;

namespace NeoBankWebApp.Controllers
{
 
    public class AdminController : Controller
    {
        private readonly IAPIService _clientService;
        public AdminController(IAPIService clientServiceInstance) => (_clientService) = (clientServiceInstance);

        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
             if(user != "2" && user != "1")  
             return RedirectToAction("Login", "Login");  

            if (HttpContext.Session.GetString(Variables.CustomerID) == null)          
             return RedirectToAction("Login", "Login");
            

            return View();
        }
        public IActionResult SubIndex()
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2" && user != "1")
                return RedirectToAction("Login", "Login");

            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");


            return View();
        }
        public IActionResult ViewUser()
        {
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2" && user != "1")
                return RedirectToAction("Login", "Login");
            return View();
        }
        public IActionResult ViewUserSubAdmin()
        {
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2" && user != "1")
                return RedirectToAction("Login", "Login");
            return View();
        }
        public IActionResult AddCompany( )
        {
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
            return RedirectToAction("Login", "Login");
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2" && user != "1") 
            return RedirectToAction("Login", "Login");            
            return View();
        }

        public IActionResult CompanyOnBoard(AddCompany addCompany)
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "1") 
                return RedirectToAction("Login", "Login");
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");

            using (HttpResponseMessage responseMessages = _clientService.OnboardCompany(addCompany, HttpContext.Session.GetString(Variables.AccessToken), HttpContext.Session.GetString(Variables.CustomerID)))
            {
                string apiInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(apiInfo);
                TempData["StatusDesc"] = objResult.StatusDesc;
                TempData["StatusCode"] = objResult.StatusCode;
                if (responseMessages.IsSuccessStatusCode)
                {                   
                    if (objResult.StatusCode == "000")
                    {
                        return View("AddCompany");
                    }
                }
            }
            return View("AddCompany");
        }

        public IActionResult AddUser()
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2" && user != "1") 
                return RedirectToAction("Login", "Login"); 
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)               
                return RedirectToAction("Login", "Login");
            return View();
        }
        public IActionResult AddUserSubAdmin()
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "2" && user != "1")
                return RedirectToAction("Login", "Login");
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");
            return View();
        }
        public JsonResult GetCompanyList()
        {

            var results =  _clientService.GetCompanyList(); 
            string reportInfo = results.Content.ReadAsStringAsync().Result.ToString();             
            ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(reportInfo);
            List<CompanyList> companyList = JsonConvert.DeserializeObject<List<CompanyList>>(objResult.Data.ToString());
            return Json(companyList);           

             
        }
        public IActionResult UserOnBoard(AddUser addUser)
        {
            string user = HttpContext.Session.GetString(Variables.UserType);
            if (user != "1") {
                return RedirectToAction("Login", "Login"); }
            if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                return RedirectToAction("Login", "Login");

            using (HttpResponseMessage responseMessages = _clientService.OnboardUser(addUser, HttpContext.Session.GetString(Variables.AccessToken), HttpContext.Session.GetString(Variables.CustomerID)))
            {
                string apiInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(apiInfo);
                TempData["StatusDesc"] = objResult.StatusDesc;
                TempData["StatusCode"] = objResult.StatusCode;
                if (responseMessages.IsSuccessStatusCode)
                {
                    if (objResult.StatusCode == "000")
                    {
                        return View("AddUser");
                    }
                }

            }
            return View("AddUser");
        }

        public ActionResult GetOnboardedUserList(string CustomerID)
        {
            try
            {
                if (HttpContext.Session.GetString(Variables.CustomerID) == null)
                    return RedirectToAction("Login", "Login");
                GetOnboardedUser values = new GetOnboardedUser();
                
                if (string.IsNullOrEmpty(CustomerID.ToString()))
                    values.CustomerId = HttpContext.Session.GetString(Variables.CustomerID);
                else
                    values.CustomerId = CustomerID;

                    using (HttpResponseMessage responseMessages = _clientService.GetOboardedUserList(values, HttpContext.Session.GetString(Variables.AccessToken), HttpContext.Session.GetString(Variables.CustomerID)))
                    {
                        string reportInfo = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                        if (responseMessages.IsSuccessStatusCode)
                        {
                            ApiResponse objResult = JsonConvert.DeserializeObject<ApiResponse>(reportInfo);
                            List<OnboardedUserList> Report = JsonConvert.DeserializeObject<List<OnboardedUserList>>(objResult.Data.ToString());

                            return Json(Report);
                        }
                        return Json("");
                    }                 
            }
            catch (Exception ex) { throw; }

        }
    }
}
