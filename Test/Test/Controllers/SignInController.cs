using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Test.Data;
using Test.Models;

namespace Test.Controllers
{
    public class SignInController : Controller
    {
        private readonly UserInfoContext user_context;
        private readonly SmsInfoContext sms_context;

        public SignInController(UserInfoContext context, SmsInfoContext s_context)
        {
            user_context = context;
            sms_context = s_context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SignIn/GetSms/{userPhone}")]
        public string getSms(string userPhone)
        {
            if (phoneIsValid(userPhone))
            {
                if (!phoneExists(userPhone))
                {
                    return "phone does not exist";
                }
                else
                {
                    if (hasCode(userPhone))
                    {
                        return "has code";
                    }
                }
            }
            else
            {
                return "phone error";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                sb.Append((new Random()).Next(0, 10));
            }
            var uuid = Guid.NewGuid().ToString();
            sms_context.Add(new SmsInfo { smsId = uuid, sendPhone = userPhone, smsCode = sb.ToString(), isValid = '0', expireTime = DateTime.Now.AddMinutes(10), createTime = DateTime.Today, lastModTime = DateTime.Today });
            sms_context.SaveChanges();
            return sb.ToString();
        }

        //模拟登录
        [Route("SignIn/{userPhone}/{smsCode}")]
        [HttpGet]
        public string SignUp(string userPhone, string smsCode)
        {

            if (phoneIsValid(userPhone))
            {
                if (!phoneExists(userPhone))
                {
                    return "phone does not exist";
                }
                else
                {
                    if (!codeIsValid(userPhone, smsCode))
                    {
                        return "code error";
                    }
                }
            }
            else
            {
                return "phone error";
            }

            var user = user_context.UserInfo.Single(m => m.userPhone == userPhone);
            var sms = sms_context.SmsInfo.Where(s => s.sendPhone == userPhone).Single(s=>s.smsCode==smsCode);
            user.lastLoginTime = DateTime.Now;
            sms.isValid = '1';
            user_context.SaveChanges();
            sms_context.SaveChanges();
            return user.userName + ":"+user.userIdCard+":"+userPhone+" sign in";
        }

        //校验手机号格式
        public bool phoneIsValid(string userPhone)
        {
            Type t = typeof(UserInfo);
            PropertyInfo phone = t.GetProperty("userPhone");
            foreach (Attribute a in phone.GetCustomAttributes())
            {
                if (a is RegularExpressionAttribute)
                {
                    if (Regex.IsMatch(userPhone, ((RegularExpressionAttribute)a).Pattern))
                    {
                        return true;
                    }
                    break;
                }
            }
            return false;
        }

        //校验手机号是否存在
        public bool phoneExists(string userPhone)
        {
            var user = user_context.UserInfo.Where(m => m.userPhone == userPhone);
            if (user.Count() == 0)
            {
                return false;
            }
            return true;
        }

        //校验是否存在验证码
        public bool hasCode(string userPhone)
        {
            var sms = sms_context.SmsInfo.Where(m => m.sendPhone == userPhone)
                .Where(m => m.expireTime > DateTime.Now)
                .Where(m => m.isValid == '0');
            if (sms.Count() != 0)
            {
                return true;
            }
            return false;
        }

        //校验验证码是否合法
        public bool codeIsValid(string userPhone, string code)
        {
            var sms = sms_context.SmsInfo.Where(m => m.sendPhone == userPhone)
                .Where(m => m.expireTime > DateTime.Now)
                .Where(m => m.smsCode == code)
                .Where(m=>m.isValid=='0');
            if (sms.Count() != 0)
            {
                return true;
            }
            return false;
        }
    }
}
