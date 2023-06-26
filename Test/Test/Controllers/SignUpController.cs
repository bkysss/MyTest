using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Test.Data;
using Test.Models;

namespace Test.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserInfoContext user_context;
        private readonly SmsInfoContext sms_context;

        public SignUpController(UserInfoContext context,SmsInfoContext s_context)
        {
            user_context = context;
            sms_context = s_context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //模拟注册
        [Route("SignUp/{userName}/{userIdCard}/{userPhone}/{smsCode}")]
        [HttpGet]
        public string SignUp(string userName,string userIdCard,string userPhone,string smsCode)
        {
            
            if (isValid(userName, userIdCard))
            {
                if (phoneIsValid(userPhone))
                {
                    if (phoneExists(userPhone))
                    {
                        return "phone exists";
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
            }
            else
            {
                return "name or idcard error";
            }


            var uuid = Guid.NewGuid().ToString();
            string year = userIdCard.Substring(6, 4);
            String month = userIdCard.Substring(10, 2);
            String day = userIdCard.Substring(12, 2);
            DateTime birthday = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            
            user_context.Add(new UserInfo { userId=uuid,userName=userName,userIdCard=userIdCard,userPhone=userPhone,userBirthday=birthday,
                registerTime=DateTime.Now,lastLoginTime=DateTime.Now,createTime=DateTime.Now,lastModTime=DateTime.Now,userType="00",userGender='0',createUserId=uuid,lastModUserId=uuid});
            var sms = sms_context.SmsInfo.Where(s => s.smsCode == smsCode)
                .Where(s=>s.expireTime>DateTime.Now)
                .Single(s => s.sendPhone == userPhone);
            
            sms.isValid = '1';
            
            user_context.SaveChanges();
            
            sms_context.SaveChanges();
            return userName+" ";
        }

        //模拟发送验证码
        [HttpGet]
        [Route("SignUp/GetSms/{userPhone}")]
        public string getSms(string userPhone)
        {
            if (phoneIsValid(userPhone))
            {
                if (phoneExists(userPhone))
                {
                    return "phone exists";
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
            for(int i = 0; i < 6; i++)
            {
                sb.Append((new Random()).Next(0, 10)) ;
            }
            var uuid = Guid.NewGuid().ToString();
            sms_context.Add(new SmsInfo { smsId = uuid, sendPhone = userPhone, smsCode = sb.ToString(), isValid = '0', expireTime = DateTime.Now.AddMinutes(10), createTime = DateTime.Now, lastModTime = DateTime.Now });
            sms_context.SaveChanges();

            return sb.ToString();
        }

        //校验姓名/身份证格式
        public bool isValid(string userName,string userIdCard)
        {
            Type type = typeof(UserInfo);
            PropertyInfo name = type.GetProperty("userName");
            PropertyInfo idcard = type.GetProperty("userIdCard");

            foreach (Attribute a in name.GetCustomAttributes(true))
            {
                if (a is RegularExpressionAttribute)
                {
                    RegularExpressionAttribute re = (RegularExpressionAttribute)a;
                    if (Regex.IsMatch(userName, re.Pattern))
                    {
                        foreach (Attribute aa in idcard.GetCustomAttributes(true))
                        {
                            if(aa is RegularExpressionAttribute)
                            {
                                RegularExpressionAttribute rea = (RegularExpressionAttribute)aa;
                                if (Regex.IsMatch(userIdCard, rea.Pattern))
                                {
                                    return true;
                                }
                                break;
                            }
               
                        }
                        break;
                    }
                    
                }
            }
            return false;
        }

        //校验手机号格式
        public bool phoneIsValid(string userPhone)
        {
            Type t = typeof(UserInfo);
            PropertyInfo phone = t.GetProperty("userPhone");
            foreach(Attribute a in phone.GetCustomAttributes())
            {
                if(a is RegularExpressionAttribute)
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
            var user = user_context.UserInfo.Where(m=>m.userPhone==userPhone);
            if (user.Count()==0)
            {
                return false;
            }
            return true;
        }

        //校验是否存在验证码
        public bool hasCode(string userPhone)
        {
            var sms=sms_context.SmsInfo.Where(m => m.sendPhone == userPhone)
                .Where(m=>m.expireTime>DateTime.Now);
            if (sms.Count() != 0)
            {
                return true;
            }
            return false;
        }

        //校验验证码是否合法
        public bool codeIsValid(string userPhone,string code)
        {
            var sms = sms_context.SmsInfo.Where(m => m.sendPhone == userPhone)
                .Where(m => m.expireTime > DateTime.Now)
                .Where(m=>m.smsCode==code)
                .Where(m=>m.isValid=='0');
            if (sms.Count()>0)
            {
                return true;
            }
            return false;
        }
    }
}

