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
        public JsonResult LoginVaildate(string UserName, string Password, string captchaInput)
        {
            string access_token = string.Empty;
            string refresh_token = string.Empty;
            string token_genaration_time = string.Empty;
            string customer_id = string.Empty;

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
                    HttpContext.Session.SetString(access_token, tokens.Access_Token);
                    HttpContext.Session.SetString(refresh_token, tokens.ToString());
                    HttpContext.Session.SetString(token_genaration_time, DateTime.Now.ToString());
                    //var result = _clientService.UserInfo(login, tokens.Access_Token);

                    using (var result = _clientService.UserInfo(login, tokens.Access_Token))
                    {
                        if (responseMessages.IsSuccessStatusCode)
                        {
                            string custInfo = result.Content.ReadAsStringAsync().Result.ToString();

                            UserInfoResponse custInformations = JsonConvert.DeserializeObject<UserInfoResponse>(custInfo);
                            HttpContext.Session.SetString(customer_id, custInformations.CustomerId.ToString());

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
                        return Json("");

                    }

                }
            }                 
          
            return Json("UnExpected Error");
        }
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
 