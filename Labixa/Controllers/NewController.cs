using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Labixa.Models;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using PagedList;
using Labixa.ViewModels;
using Labixa.Helpers;
using Outsourcing.Core.Common;

namespace Labixa.Controllers
{

    public class NewController : Controller
    {

        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;

        public NewController(IBlogService blogService,
            IBlogCategoryService blogCategoryService
            )
        {

            this._blogService = blogService;
            this._blogCategoryService = blogCategoryService;

        }
        public ActionResult News(int? page = 1)
        {
            int pageNumb = (page ?? 1);
            int pageSize = 6;
            var list = _blogService.GetBlogs().Where(p=>p.BlogCategoryId == 3 && p.IsAvailable == true).ToPagedList(pageNumb, pageSize);
            return View(list);
        }
        public ActionResult NewsDetail(string Slug)
        {
            var model = _blogService.GetBlogByUrlName(Slug);
            ViewBag.Title = model.Title;
            ViewBag.Description = model.Description;
            ViewBag.Image = "http://tueduchealthy.vn" + model.BlogImage_Default;
            ViewBag.Url = model.Slug;
            return View(model);
        }
        public ActionResult NewsFeatured()
        {

            var model = _blogService.Get3BlogNewsNewest().Where(p=>p.IsAvailable==true).ToList();
            return PartialView("_newRelated", model);
        }

        public ActionResult Recruitment(int? page = 1)
        {
            int pageNumb = (page ?? 1);
            int pageSize = 6;
            var list = _blogService.GetBlogs().Where(p => p.BlogCategoryId == 9 && p.IsAvailable == true).ToPagedList(pageNumb, pageSize);
            return View(list);
        }
        public ActionResult Promotion()
        {
            return View();
        }

    }
}

