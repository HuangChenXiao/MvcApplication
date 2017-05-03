using Custom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class FLController : Controller
    {
        // GET: FL
        public ActionResult FLIndex()
        {
            return View();
        }
        public ActionResult FLAdd()
        {
            return View();
        }
        public ActionResult FLEdit()
        {
            return View();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult FLinkAdd(BA_FriendshipLink result)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var m = from t in db.BA_FriendshipLink
                        where t.Name == result.Name
                        select t;
                if (m.Count() > 0)
                {
                    return Json(new { data = "fail", content = "名称重复！" });
                }
                result.AddUser = BasePage.GetCookie("UserNameCookie");
                result.AddTime = DateTime.Now;
                db.BA_FriendshipLink.Add(result);
                db.SaveChanges();
                return Json(new { data = "success", content = "新增成功！" });
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult FLinkEdit(BA_FriendshipLink result)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var m = from t in db.BA_FriendshipLink
                        where t.Name == result.Name && t.Id != result.Id
                        select t;
                if (m.Count() > 0)
                {
                    return Json(new { data = "fail", content = "名称重复！" });
                }
                var BA_FriendshipLink = from t in db.BA_FriendshipLink where t.Id == result.Id select t;
                var resultInfo = BA_FriendshipLink.FirstOrDefault();
                resultInfo.Name = result.Name;
                resultInfo.Icon = result.Icon;
                resultInfo.ToUrl = result.ToUrl;
                resultInfo.AddUser = BasePage.GetCookie("UserNameCookie");
                resultInfo.AddTime = DateTime.Now;
                db.SaveChanges();
                return Json(new { data = "success", content = "修改成功！" });
            }
        }
        /// <summary>
        /// 根据ID查询案例
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetFLinkById(int Id)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var data = from t in db.BA_FriendshipLink
                           where t.Id == Id
                           select t;
                return Json(new { data = data.FirstOrDefault() });
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult FLinkDel(string strId)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var List = strId.Split(',');
                foreach (var item in List)
                {
                    var Id = Convert.ToInt32(item);
                    var dData = from a in db.BA_FriendshipLink where a.Id == Id select a;
                    db.BA_FriendshipLink.Remove(dData.FirstOrDefault());
                }
                db.SaveChanges();
                return Json(new { data = "success", content = "删除成功！" });
            }
        }
    }
}