using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Custom.Model;

namespace MvcApplication.Controllers
{
    [ChkAdminLogin]
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult AdminRole()
        {
            return View();
        }

        public ActionResult AdminRoleAdd()
        {
            return View();
        }
        public ActionResult AdminRoleEdit()
        {
            return View();
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult AuthorityList()
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var Authority = db.Cu_UserAuthority;
                return Json(new { data = Authority.ToList(), total = Authority.Count() });
            }
        }
        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult MenuList()
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var Menu = db.Cu_Menu;
                return Json(new { data = Menu.ToList() });
            }
        }
        /// <summary>
        /// 新增角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult AddRole(Cu_UserAuthority Role)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                db.Cu_UserAuthority.Add(Role);
                db.SaveChanges();
                return Json(new { data = "success",content="新增角色成功！" });
            }
        }
        /// <summary>
        /// 编辑角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult EditRole(Cu_UserAuthority Role)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var RoleInfo = from a in db.Cu_UserAuthority where a.AuthorityId == Role.AuthorityId select a;
                RoleInfo.FirstOrDefault().Authority = Role.Authority;
                RoleInfo.FirstOrDefault().UserTypeName = Role.UserTypeName;
                db.SaveChanges();
                return Json(new { data = "success", content = "修改角色成功！" });
            }
        }
        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult DelRole(string AuthorityId)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var RemoveId = AuthorityId.Split(',');
                foreach (var item in RemoveId)
                {
                    int Id = Convert.ToInt32(item);
                    var Role = from t in db.Cu_UserAuthority where t.AuthorityId == Id select t;
                    db.Cu_UserAuthority.Remove(Role.FirstOrDefault());
                }
                
                db.SaveChanges();
                return Json(new { data = "success", content = "删除角色成功！" });
            }
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public JsonResult RoleById(int AuthorityId)
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var Role = db.Cu_UserAuthority.Where(o => o.AuthorityId == AuthorityId).FirstOrDefault();
                return Json(new { data = Role });
            }
        }
    }
}