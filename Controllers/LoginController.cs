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

namespace NeoBankWebApp.Controllers
{
    public class LoginController : Controller
    {
        private const string SessionKeyCaptcha = "Ajith123";
        private readonly IAPIService _clientService;
        private object stream;

        public LoginController(IAPIService clientServiceInstance) => (_clientService) = (clientServiceInstance);
        public IActionResult Login() {
            Captcha();
           return View();
        }
        public JsonResult CaptchaRefresh()
        {
            HttpContext.Session.Remove(SessionKeyCaptcha);
            string capture = Captcha();
           return Json(new { captchaImage = capture });           
        }      
        public JsonResult LoginVaildate(string UserName, string Password, string captchaInput)
        {
         /*   string storedCaptchaCode = HttpContext.Session.GetString(SessionKeyCaptcha); 
            HttpContext.Session.Remove(SessionKeyCaptcha); 
            bool isValid = !string.IsNullOrEmpty(storedCaptchaCode) &&
                           !string.IsNullOrEmpty(captchaInput) &&
                           string.Equals(storedCaptchaCode, captchaInput, StringComparison.Ordinal);
            if (isValid)
            {
                
            }
            else
            {
                return Json("Not valid");
            }
         */

                LoginRequest login = new LoginRequest();
                login.UserName = UserName;
                login.Password = Password;
               

            using (HttpResponseMessage responseMessages =  _clientService.LoginVaildate(login))
            {
                string stream = responseMessages.Content.ReadAsStringAsync().Result.ToString();

                if (responseMessages.IsSuccessStatusCode)
                { 


                }
            }


            // return Json(stream);

            return Json("admin");




        }
        public string Captcha()
        {
            string captchaCode = GenerateRandomCode();
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
 