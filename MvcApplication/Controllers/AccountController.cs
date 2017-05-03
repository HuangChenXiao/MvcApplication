using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Custom.Model;
using Custom.Lib;

namespace MvcApplication.Controllers
{
    [ChkAdminLogin]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Adminlist()
        {
            return View();
        }
        public ActionResult AdminAdd()
        {
            return View();
        }
        public ActionResult AdminEdit()
        {
            return View();
        }
        public ActionResult Advertisement()
        {
            return View();
        }
       
        /// <summary>
        /// 获取网站配置
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAdvertisementInfo()
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var temp = from a in db.BA_Advertisement select a;
                var data = temp.OrderBy(s => s.AdvertisementId);
                return Json(new { data = data.FirstOrDefault() });
            }
        }
        /// <summary>
        /// 修改网站配置
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult EditAdvertisement(BA_Advertisement Advertisement)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var BA_Advertisement = from a in db.BA_Advertisement where a.AdvertisementId == Advertisement.AdvertisementId select a;
                var AdvertisementInfo = BA_Advertisement.FirstOrDefault();
                AdvertisementInfo.CompanyName=Advertisement.CompanyName;
                AdvertisementInfo.CompanyAddress = Advertisement.CompanyAddress;
                AdvertisementInfo.CompanyLogo = Advertisement.CompanyLogo;
                AdvertisementInfo.CompanyPhone = Advertisement.CompanyPhone;
                AdvertisementInfo.CompanyQRCode = Advertisement.CompanyQRCode;
                AdvertisementInfo.Image = Advertisement.Image;
                AdvertisementInfo.Email = Advertisement.Email;
                AdvertisementInfo.UpdateTime = DateTime.Now;
                AdvertisementInfo.UpdateUser = BasePage.GetCookie("UserNameCookie");
                db.SaveChanges();
            }
            return Json(new { data = "success", content = "修改网站配置成功！" });
        }
        /// <summary>
        /// 获取管理员的所有信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUserInfo()
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var temp = from a in db.Cu_User
                           join b in db.Cu_UserAuthority on new { AuthorityId = (Int32)a.AuthorityId } equals new { AuthorityId = b.AuthorityId }
                           select new
                           {
                               a.UserId,
                               a.UserCode,
                               a.UserName,
                               a.UserPhone,
                               a.UserPassword,
                               a.IsonLine,
                               a.Status,
                               a.AddTime,
                               a.AddUser,
                               a.UpdateTime,
                               a.UpdateUser,
                               a.UserAuthority,
                               a.AuthorityId,
                               a.AgID,
                               a.IsAdmin,
                               a.PageSize,
                               b.UserTypeName
                           };
                var total = temp.Count();
                var users = temp.OrderBy(s => s.UserId);
                return Json(new { data = users.ToList(), total = total });
            }
        }
        /// <summary>
        /// 禁用启用
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public JsonResult EditStatus(Cu_User UserInfo)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                Cu_User user = db.Cu_User.First(o => o.UserId == UserInfo.UserId);
                user.Status = UserInfo.Status;
                db.SaveChanges();
                return Json(new { data = "success"});
            }
        }
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public JsonResult DelAdmin(Cu_User UserInfo)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                Cu_User user = db.Cu_User.First(o => o.UserId == UserInfo.UserId);
                db.Cu_User.Remove(user);
                db.SaveChanges();
                return Json(new { data = "success" });
            }
        }
        /// <summary>
        /// 增加管理员
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult AddAdmin(Cu_User UserInfo)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var chkUserCode = from a in db.Cu_User where a.UserCode == UserInfo.UserCode select a;
                if (chkUserCode.Count()>0)
                {
                    return Json(new { data = "fail",content="管理员编码重复！" });
                }
                UserInfo.UserPassword = BasePage.Md5Hash(UserInfo.UserPassword);
                UserInfo.AddTime = DateTime.Now;
                UserInfo.AddUser = BasePage.GetCookie("UserNameCookie");
                UserInfo.Status = 1;
                UserInfo.IsAdmin = false;
                db.Cu_User.Add(UserInfo);
                db.SaveChanges();
            }
            return Json(new { data = "success", content = "新增管理员成功！" });
        }
        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult EditAdmin(Cu_User UserInfo)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var chkUserCode = from a in db.Cu_User where a.UserCode == UserInfo.UserCode && a.UserId!=UserInfo.UserId select a;
                if (chkUserCode.Count() > 0)
                {
                    return Json(new { data = "fail", content = "管理员编码重复！" });
                }
                var User = from a in db.Cu_User where a.UserId == UserInfo.UserId select a;
                if (!string.IsNullOrEmpty(UserInfo.UserPassword))
                {
                    User.FirstOrDefault().UserPassword = BasePage.Md5Hash(UserInfo.UserPassword);
                }
                User.FirstOrDefault().UserName = UserInfo.UserName;
                User.FirstOrDefault().UserCode = UserInfo.UserCode;
                User.FirstOrDefault().UserPhone = UserInfo.UserPhone;
                User.FirstOrDefault().AuthorityId = UserInfo.AuthorityId;
                User.FirstOrDefault().UpdateTime = DateTime.Now;
                User.FirstOrDefault().UpdateUser = BasePage.GetCookie("UserNameCookie");
                db.SaveChanges();
            }
            return Json(new { data = "success", content = "修改管理员成功！" });
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult AuthorityList(Cu_User UserInfo)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var Authority = db.Cu_UserAuthority;
                return Json(new { data = Authority.ToList() });
            }
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult AdminByUserIdList(Cu_User UserInfo)
        {
            int UserId=Convert.ToInt32(Request["UserId"]);
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var Authority = from a in db.Cu_User where a.UserId == UserId select a;
                return Json(new { data = Authority.ToList() });
            }
        }
    }
}