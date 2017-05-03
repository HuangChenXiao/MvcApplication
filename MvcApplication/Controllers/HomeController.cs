using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Custom.Model;
using MvcApplication.Models;
using System.IO;
using Custom.Lib;

namespace MvcApplication.Controllers
{
    [ChkAdminLogin]
    public class HomeController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult fail404()
        {
            return View();
        }
        public JsonResult GetMenu()
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var strUser = BasePage.GetCookie("UserInfoCookie");
                dynamic User = DynamicJsonConverter.ParseJson(strUser);
                int UserId = User.UserId;
                
                if (User.IsAdmin == true)
                {
                   var Menu = from t in db.Cu_Menu select t;
                   return Json(new { data = Menu.ToList() });
                }
                else
                {
                   var Menu = from t in db.Cu_Menu
                           where
                             (int?)("," +
                               ((from a in db.Cu_User
                                 join b in db.Cu_UserAuthority on new { AuthorityId = (Int32)a.AuthorityId } equals new { AuthorityId = b.AuthorityId }
                                 where
                                  a.UserId == UserId
                                 select new
                                 {
                                     b.Authority
                                 }).FirstOrDefault().Authority) + ",").ToUpper().IndexOf(("," + t.MenuId.ToString().TrimStart() + ",").ToUpper()) + 1 > 0
                           select new
                 {
                     t.MenuId,
                     t.FMenuId,
                     t.MenuName,
                     t.Imageicon,
                     t.URLAddress,
                     t.Status,
                     t.AddTime,
                     t.AddUser,
                     t.UpdateTime,
                     t.UpdateUser,
                     t.IsBlank
                 };
                   return Json(new { data = Menu.ToList()});
                }
                
            }
        }

        [HttpPost]
        public ActionResult WebUpLoad()
        {
            HttpPostedFileBase uploadFile = Request.Files["file"];
            string FilePathName = HttpContext.Server.MapPath("../UpLoad/");
            //string M = "M" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string S =  DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            if (uploadFile.ContentLength > 0)
            {
                if (!Directory.Exists(FilePathName))//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(FilePathName);
                }
                //获得保存路径
                string filePath = Path.Combine(FilePathName,
                                Path.GetFileName(S));
                uploadFile.SaveAs(filePath);
                ////压缩图片
                //BasePage.GetPicThumbnail(filePath, FilePathName + "/" + M);
            }
            WebUpload path = new WebUpload();
            path.Path += "/UpLoad/" + S;

            return Json(new
            {
                path
            });
        }
    }
}