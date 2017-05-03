using Custom.Lib;
using Custom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            BasePage.DelCookie("UserNameCookie");
            BasePage.DelCookie("UserInfoCookie");
            return View();
        }
        /// <summary>
        /// 处理登录的信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public JsonResult CheckUserLogin(Cu_User userInfo)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                //linq查询
                userInfo.UserPassword = BasePage.Md5Hash(userInfo.UserPassword);
                var users = from p in db.Cu_User
                            where p.UserCode == userInfo.UserCode && p.UserPassword == userInfo.UserPassword
                            select p;
                if (users.Count() > 0)
                {
                    string strUser = DynamicJsonConverter.GetJson<Cu_User>(users.FirstOrDefault());
                    BasePage.WriteCookie("UserNameCookie", users.FirstOrDefault().UserName, 0);
                    BasePage.WriteCookie("UserInfoCookie", strUser, 0);
                    return Json(new { result = "success", content = "" });
                }
                else
                {
                    return Json(new { result = "error", content = "用户名密码错误，请您检查" });
                }
            }
        }

    }
}