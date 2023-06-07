using Microsoft.AspNetCore.Mvc;
using NeoBankWebApp.API_Service;
using NeoBankWebApp.Models.Login;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using NeoBankWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using System.IO;
using NeoBankWebApp.Models.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using NeoBankWebApp.Rule_Common;
using Microsoft.AspNetCore.Authorization;

namespace NeoBankWebApp.Controllers
{ 
    public class LoginController : Controller
    {
        private const string SessionKeyCaptcha = "";
        private readonly IAPIService _clientService; 
        public LoginController(IAPIService clientServiceInstance) => (_clientService) = (clientServiceInstance);
        public IActionResult Login() {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyCaptcha)))
            {
                Captcha(null);
            }
            else
            {
                Captcha(HttpContext.Session.GetString(SessionKeyCaptcha)); 
            }

            return View();
        }
        public JsonResult CaptchaRefresh()
        {
           
            string capture = Captcha(null);
           return Json(new { captchaImage = capture });           
        }
        public ActionResult LoginValidate(string UserName, string Password, string captchaInput)
        {
            ApiResponse apiResponse = new ApiResponse();

            if (string.IsNullOrEmpty(UserName))
            {            
                TempData["LoginFailedMessage"] = "Please Enter Valid User Name";
                Captcha(HttpContext.Session.GetString(SessionKeyCaptcha));
                return View("Login");
            }
            if (string.IsNullOrEmpty(Password))
            {
                TempData["LoginFailedMessage"] = "Please Enter Valid PassWord";
                Captcha(HttpContext.Session.GetString(SessionKeyCaptcha));
                return View("Login");
            }
            if (string.IsNullOrEmpty(captchaInput))
            {
                TempData["LoginFailedMessage"] = "Please Enter Valid Captcha";
                Captcha(HttpContext.Session.GetString(SessionKeyCaptcha));
                return View("Login");
            }

            string storedCaptchaCode = HttpContext.Session.GetString(SessionKeyCaptcha);
            bool isValid = !string.IsNullOrEmpty(storedCaptchaCode) &&
                           !string.IsNullOrEmpty(captchaInput) &&
                           string.Equals(storedCaptchaCode, captchaInput, StringComparison.OrdinalIgnoreCase);
            HttpContext.Session.Remove(SessionKeyCaptcha);

            if (!isValid)
            {
                TempData["LoginFailedMessage"] =   "Captcha validation Failed";
                Captcha(HttpContext.Session.GetString(SessionKeyCaptcha));
                return View("Login");
            }

            LoginRequest login = new LoginRequest();
            login.UserName = UserName;
            login.Password = Password;

            using (HttpResponseMessage responseMessages = _clientService.LoginVaildate(login))
            {
                if (responseMessages.IsSuccessStatusCode)
                {
                    string stream = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                    Tokens tokens = JsonConvert.DeserializeObject<Tokens>(stream);
                    HttpContext.Session.SetString(Variables.AccessToken, tokens.Access_Token);
                    HttpContext.Session.SetString(Variables.RefreshToken, tokens.ToString());
                    HttpContext.Session.SetString(Variables.TokenGenarationTime, DateTime.Now.ToString());
                    //var result = _clientService.UserInfo(login, tokens.Access_Token);

                    using (var result = _clientService.UserInfo(login, tokens.Access_Token))
                    {
                        string custInfo = result.Content.ReadAsStringAsync().Result.ToString();
                        ApiResponse errorResponse = JsonConvert.DeserializeObject<ApiResponse>(custInfo);   
                        if (responseMessages.IsSuccessStatusCode)
                        {    
                            UserInfoResponse custInformations = JsonConvert.DeserializeObject<UserInfoResponse>(custInfo);
                            HttpContext.Session.SetString(Variables.CustomerID, custInformations.CustomerId.ToString());
                            HttpContext.Session.SetString(Variables.UserType, custInformations.UserType.ToString());
                         
                            HttpContext.Session.SetString(Variables.UserName, custInformations.UserName);
                            if (custInformations.UserType == "1")
                            {
                                return RedirectToAction("Index", "SuperAdmin");
                            }
                            if (custInformations.UserType == "2")
                            {
                                return RedirectToAction("Index", "Admin");

                            }
                            if (custInformations.UserType == "3")
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        TempData["LoginFailedMessage"] = errorResponse.StatusDesc;
                        Captcha(HttpContext.Session.GetString(SessionKeyCaptcha));
                        return View("Login");

                    }

                }
            }
            TempData["LoginFailedMessage"] = "Technical error Please Reach Admin";
            Captcha(HttpContext.Session.GetString(SessionKeyCaptcha));
            return View("Login");
        }
        /* public JsonResult LoginVaildate(string UserName, string Password, string captchaInput)
         {


             if (string.IsNullOrEmpty(UserName))
             {
                 return Json("Please Enter UserName");
             }
             if (string.IsNullOrEmpty(Password))
             {
                 return Json("Please Enter PassWord");
             }
             if (string.IsNullOrEmpty(captchaInput))
             {
                 return Json("Please Enter Captcha");
             }

             string storedCaptchaCode = HttpContext.Session.GetString(SessionKeyCaptcha);     
             bool isValid = !string.IsNullOrEmpty(storedCaptchaCode) &&
                            !string.IsNullOrEmpty(captchaInput) &&
                            string.Equals(storedCaptchaCode, captchaInput, StringComparison.OrdinalIgnoreCase);
             HttpContext.Session.Remove(SessionKeyCaptcha);

             if (!isValid)
             {
                  return Json("InValid Captcha");
             }           

                 LoginRequest login = new LoginRequest();
                 login.UserName = UserName;
                 login.Password = Password;

             using (HttpResponseMessage responseMessages = _clientService.LoginVaildate(login))
             {
                 if (responseMessages.IsSuccessStatusCode)
                 {
                     string stream = responseMessages.Content.ReadAsStringAsync().Result.ToString();
                     Tokens tokens = JsonConvert.DeserializeObject<Tokens>(stream);
                     HttpContext.Session.SetString(Variables.AccessToken, tokens.Access_Token);
                     HttpContext.Session.SetString(Variables.RefreshToken, tokens.ToString());
                     HttpContext.Session.SetString(Variables.TokenGenarationTime, DateTime.Now.ToString());
                     //var result = _clientService.UserInfo(login, tokens.Access_Token);

                     using (var result = _clientService.UserInfo(login, tokens.Access_Token))
                     {
                         if (responseMessages.IsSuccessStatusCode)
                         {
                             string custInfo = result.Content.ReadAsStringAsync().Result.ToString();

                             UserInfoResponse custInformations = JsonConvert.DeserializeObject<UserInfoResponse>(custInfo);
                             HttpContext.Session.SetString(Variables.CustomerID, custInformations.CustomerId.ToString());
                             HttpContext.Session.SetString(Variables.UserType, custInformations.UserType.ToString());
                             ViewBag.UserName = custInformations.UserName;
                             if (custInformations.UserType=="1")
                             {
                                 return Json("superadmin");
                             }
                             if (custInformations.UserType == "2")
                             {
                                 return Json("admin");

                             }
                             if (custInformations.UserType == "3")
                             {
                                 return Json("user");
                             }
                         }
                         return Json("Technical Error");

                     }

                 }
             }                 

             return Json("UnExpected Error");
         }*/
        public string Captcha(string RandamCode)
        {
            string captchaCode = string.Empty;
            HttpContext.Session.Remove(SessionKeyCaptcha);
            if (RandamCode == null)
            {
                  captchaCode = GenerateRandomCode();
            }  
            else
            {
                  captchaCode = RandamCode;
            }
            HttpContext.Session.SetString(SessionKeyCaptcha, captchaCode);
            byte[] captchaBytes = GenerateCaptchaImage(captchaCode);
            string base64Image = Convert.ToBase64String(captchaBytes);
            ViewBag.CaptchaImage = base64Image;
            return base64Image;

        }
        private string GenerateRandomCode()
        {
            const string allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789@#$abcdefghijklmanopqrstuvwxyz";
            Random random = new Random();
            char[] chars = new char[6];
            for (int i = 0; i < 6; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
        private byte[] GenerateCaptchaImage(string captchaCode)
        {
           
            using (Bitmap bitmap = new Bitmap(250, 80))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);

                using (Font font = new Font("Arial", 36, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.DarkBlue))
                {
                    graphics.DrawString(captchaCode, font, brush, 10, 10);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }

        public async Task<JsonResult> Signout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(string.Empty);
        }
    }
    }
 