using Custom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class IMController : Controller
    {
        // GET: Case
        public ActionResult IMIndex()
        {
            return View();
        }
        public ActionResult IMAdd()
        {
            return View();
        }
        public ActionResult IMEdit()
        {
            return View();
        }
        /// <summary>
        /// 案例查询
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult GetIMList()
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                int PageSize = Request["length"] == null ? 10 : Convert.ToInt32(Request["length"]);//显示多少数量
                int Page = Convert.ToInt32(Request["start"]) == 0 ? 0 : Convert.ToInt32(Request["start"]) / Convert.ToInt32(Request["length"]);//当前页  
                var search = Request["search"];//模糊搜索 
                var draw = Request["draw"];//datatable固定传值，必需接收再传送，不用赋值
                var Code = Request["Code"];
                var temp = from a in db.BA_Information select a;
                var total = temp.Where(o =>
                        ((o.Name.Contains(Code) || string.IsNullOrEmpty(Code)))
                    ).Count();
                var list = temp.OrderByDescending(s => s.Id).Where(o =>
                        ((o.Name.Contains(Code) || string.IsNullOrEmpty(Code)))
                    ).Skip(Page * PageSize).Take(PageSize);
                var data = new
                {
                    draw = draw,
                    recordsTotal = total,
                    recordsFiltered = total,
                    data = list.ToList(),
                };
                return Json(data);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetIMAdd(BA_Information result)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var m = from t in db.BA_Information
                        where t.Name == result.Name
                        select t;
                if (m.Count() > 0)
                {
                    return Json(new { data = "fail", content = "资讯名称重复！" });
                }
                result.AddUser = BasePage.GetCookie("UserNameCookie");
                result.AddTime = DateTime.Now;
                db.BA_Information.Add(result);
                db.SaveChanges();
                return Json(new { data = "success", content = "新增资讯成功！" });
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetIMEdit(BA_Information result)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var m = from t in db.BA_Information
                        where t.Name == result.Name && t.Id!=result.Id
                        select t;
                if (m.Count() > 0)
                {
                    return Json(new { data = "fail", content = "资讯名称重复！" });
                }
                var BA_Case = from t in db.BA_Information where t.Id == result.Id select t;
                var resultInfo = BA_Case.FirstOrDefault();
                resultInfo.Title = result.Title;
                resultInfo.Name = result.Name;
                resultInfo.SubName = result.SubName;
                resultInfo.Status = result.Status;
                resultInfo.Image = result.Image;
                resultInfo.Content = result.Content;
                resultInfo.AddUser = BasePage.GetCookie("UserNameCookie");
                resultInfo.AddTime = DateTime.Now;
                db.SaveChanges();
                return Json(new { data = "success", content = "修改案例名称成功！" });
            }
        }
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetIMById(int Id) {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var data = from t in db.BA_Information
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
        public JsonResult GetIMDel(string strId)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var List = strId.Split(',');
                foreach (var item in List)
                {
                    var Id = Convert.ToInt32(item);
                    var dData = from a in db.BA_Information where a.Id == Id select a;
                    db.BA_Information.Remove(dData.FirstOrDefault());
                }
                db.SaveChanges();
                return Json(new { data = "success", content = "删除成功！" });
            }
        }
    }
}