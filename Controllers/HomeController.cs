using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TelerikFileUpload_MVC.DAL;
using TelerikFileUpload_MVC.Models;

namespace TelerikFileUpload_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }










        public ActionResult SingleFileUpload()
        {
            Session["NewVendorID"] = 3;
            return View();
        }

        public ActionResult MultipleUpload()
        {
            Session["NewVendorID"] = 3;
            return View();
        }

        MVC_DAL dal = new MVC_DAL();

        public ActionResult AttachmentDetails_Read([DataSourceRequest]DataSourceRequest request, Attachments attachments)
        {
            List<Attachments> list = new List<Attachments>();
            if (Session["NewVendorID"].ToString() != "0" && Session["NewVendorID"].ToString() != "")
            {
                attachments.vid = Convert.ToInt32(Session["NewVendorID"]);
            }
            list = dal.AttachmentDetails_Read(attachments.vid);
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AttachmentDetails_Insert(IEnumerable<HttpPostedFileBase> files, Attachments attachments)
        {
            if (files != null)
            {
                if (Session["NewVendorID"].ToString() != "0" && Session["NewVendorID"].ToString() != "")
                {
                    foreach (var file in files)
                    {
                        attachments.vid = Convert.ToInt32(Session["NewVendorID"]);
                        attachments.fileName = Path.GetFileName(file.FileName);
                        var fName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fExt = Path.GetExtension(file.FileName);
                        var date = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                        string dir = Path.Combine(Server.MapPath("~/App_Data/Attachments/"));
                        attachments.filePath = dir + fName + "_" + date + fExt;

                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        try
                        {
                            file.SaveAs(attachments.filePath);
                            int status = dal.AttachmentDetails_Insert(attachments);
                            if (status != 1)
                            {
                                if (System.IO.File.Exists(attachments.filePath))
                                {
                                    System.IO.File.Delete(attachments.filePath);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.SaveFileMsg = ex.Message;
                        }
                    }
                }
            }
            return RedirectToAction("SingleFileUpload");
        }

        public ActionResult AttachmentDetails_Update(Attachments attachments)
        {
            if (attachments != null)
            {
                try
                {
                    int status = dal.AttachmentDetails_Update(attachments);
                    if (status > 0)
                    {
                        return RedirectToAction("SingleFileUpload");
                    }
                    else
                    {
                        ViewBag.Err = "Something went wrong!";
                        return View("SingleFileUpload");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Err = ex.Message;
                }
            }
            return RedirectToAction("SingleFileUpload");
        }

        public ActionResult AttachmentDetails_Delete(Attachments attachments)
        {
            if(attachments != null)
            {
                try
                {
                    if (System.IO.File.Exists(attachments.filePath))
                    {
                        System.IO.File.Delete(attachments.filePath);
                        int status = dal.AttachmentDetails_Delete(attachments.id);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.SaveFileMsg = ex.Message;
                }
            }
            return RedirectToAction("SingleFileUpload");
        }

        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                if (Session["NewVendorID"].ToString() != "0" && Session["NewVendorID"].ToString() != "")
                {
                    Attachments attachments = new Attachments();
                    foreach (var file in files)
                    {
                        attachments.vid = Convert.ToInt32(Session["NewVendorID"]);
                        attachments.fileName = Path.GetFileName(file.FileName);
                        attachments.filePath = Path.Combine(Server.MapPath("~/App_Data/Attachments"), attachments.fileName);

                        try
                        {
                            file.SaveAs(attachments.filePath);
                            int status = dal.AttachmentDetails_Insert(attachments);
                            if (status != 1)
                            {
                                if (System.IO.File.Exists(attachments.filePath))
                                {
                                    System.IO.File.Delete(attachments.filePath);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.SaveFileMsg = ex.Message;
                        }
                    }
                }
            }
            return Content("");
        }

        public ActionResult Async_Remove(string[] filenames)
        {
            if(filenames!=null)
            {
                Attachments attachments = new Attachments();
                foreach (var filename in filenames)
                {
                    attachments.vid = Convert.ToInt32(Session["NewVendorID"]);
                    attachments.fileName = Path.GetFileName(filename);
                    attachments.filePath = Path.Combine(Server.MapPath("~/App_Data/Attachments"), attachments.fileName);

                    if (System.IO.File.Exists(attachments.filePath))
                    {
                        int status = dal.Async_Remove(attachments);
                        if (status > 0)
                        {
                            System.IO.File.Delete(attachments.filePath);
                        }
                    }
                }
            }
            return Content("");
        }
    }
}