using Custom.Lib;
using Custom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcApplication
{
    public class ChkAdminLogin : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            string User = BasePage.GetCookie("UserInfoCookie");
            if (string.IsNullOrEmpty(User))
            {
                HttpContext.Current.Response.Redirect("/Login/Login");
            }
            else
            {
                using (BuyunSiteEntities db = new BuyunSiteEntities())
                {
                    var serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    dynamic data = serializer.Deserialize<object>(User);
                    string UserCode = data.UserCode;
                    string UserPassword = data.UserPassword;
                    var UserInfo=db.Cu_User.Where(o => o.UserCode == UserCode && o.UserPassword == UserPassword);
                    if (UserInfo.Count()<=0)
                    {
                        HttpContext.Current.Response.Redirect("/Login/Login");
                    }
                }
            }
        }
    }
}