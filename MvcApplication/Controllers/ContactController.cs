using Custom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult ContactIndex()
        {
            return View();
        }
        public JsonResult ContactStatusEdit(int Id) 
        {
            using (BuyunSiteEntities db = new BuyunSiteEntities())
            {
                var BA_Contact = db.BA_Contact.Where(o => o.Id == Id);
                BA_Contact.FirstOrDefault().Status = 1;
                db.SaveChanges();
                return Json(new { data = "success", content = "已查看！" });
            }
        }
    }
}