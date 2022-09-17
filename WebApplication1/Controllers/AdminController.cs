using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        dbemarketingEntities3 db = new dbemarketingEntities3();
        // GET: Admin
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        public ActionResult Signout()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("login");
        }


        [HttpPost]
        public ActionResult login(tbl_admin avm)
        {
            tbl_admin ad = db.tbl_admin.Where(x => x.ad_username == avm.ad_username && x.ad_password == avm.ad_password).SingleOrDefault();
            if (ad!=null)
            {

                Session["ad_id"] = ad.ad_id.ToString();

                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.error = "Hatalı Kullanıcı Adı ya da Şifre girdiniz!";
                
            }

            return View();
        }

        public ActionResult Index()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }

            return View();
        }

        public ActionResult Create()
        {
            if ( Session["ad_id"]==null)
            {
                return RedirectToAction("login");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Create(tbl_category cvm, HttpPostedFileBase imgfile)
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }

            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Resim yüklenemedi!";
            }
            else
            {
                tbl_category cat = new tbl_category();
                cat.cat_name = cvm.cat_name;
                cat.cat_image = path;
                cat.cat_status = 1;
                cat.cat_fk_ad=Convert.ToInt32(Session["ad_id"].ToString());
                db.tbl_category.Add(cat);
                db.SaveChanges();
                return RedirectToAction("ViewCategory");
            }
            
            return View();
        } //end,,,,,,,,,,,,,,,,,,,



        public ActionResult ViewCategory(int?page)
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);



            
        }




        public string uploadimgfile(HttpPostedFileBase file)
        {
            if (Session["ad_id"] == null)
            {
                RedirectToAction("login");
            }

            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                           ViewBag.Message = "Dosya yükleme başarılı!"; // ORJİNALDE KAPALIYDI İSTEĞE BAĞLI OLARAK TEKRAR KAPATILABİLİR
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Sadece jpg,jpeg,png formatları kabul edilir!'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Lütfen bir dosya seçiniz!'); </script>");
                path = "-1";
            }



            return path;
        }








    }
}