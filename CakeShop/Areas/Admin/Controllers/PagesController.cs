using CakeShop.Models.Data;
using CakeShop.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CakeShop.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list og PageVM
            List<PageVM> pagesList;

            
            using (Db db = new Db())
            {
                //Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();

            }


            //Return view with list
            return View(pagesList);
        }


        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Проверка состояния модели
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Обьявление
                string slug;

                //Инициализация
                PageDTO dto = new PageDTO();

                //DTO title
                dto.Title = model.Title;


                //Проверки slug-a
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                if(db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug) )
                {
                    ModelState.AddModelError("","Старница с таким названием уже существует.");
                    return View(model);
                }

                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //Сохранение
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Set TempData message
            TempData["SM"] = "Вы добавили новую страницу";

            //Redirect
            return RedirectToAction("AddPage");
        }

        //GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;


            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);

                if (dto == null)
                {
                    return Content("Страницы не существует");
                }

                model = new PageVM(dto);

            }


                return View(model);
        }


        //POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //Проверка состояния модели
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                int id = model.id;
                string slug = "home";

                PageDTO dto = db.Pages.Find(id);


                dto.Title = model.Title;


                //Проверки slug-a
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
               
                if (db.Pages.Where(x => x.id != id).Any(x => x.Title == model.Title) || 
                    db.Pages.Where(x => x.id != id).Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "Старница с таким названием уже существует.");
                    return View(model);
                }

                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                db.SaveChanges();
            }

            //Set TempData message
            TempData["SM"] = "Вы добавили изменили страницу";

            //Redirect
            return RedirectToAction("EditPage");

        }

        //GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            PageVM model;


            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);

                if (dto == null)
                {
                    return Content("Страницы не существует");
                }

                model = new PageVM(dto);

            }


            return View(model);
        }

        //GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);

                db.Pages.Remove(dto);

                db.SaveChanges();
            }

                return RedirectToAction("Index");
        }


        //POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {

            using (Db db = new Db())
            {
                //Set initial count
                int count = 1;

                //Declare DTO
                PageDTO dto;

                //Set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
                

        }

        //GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Declare model
            SidebarVM model;

            using (Db db = new Db())
            {
                //Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //Init model
                model = new SidebarVM(dto);
            }
                

                //Return view with model

                return View(model);
        }

        //POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                //Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //DTO the body 
                dto.Body = model.Body;

                //Save
                db.SaveChanges();

            }

            //Set TempData message
            TempData["SM"] = "Вы изменили навигацию сайта!";



            //Redirect
            return RedirectToAction("EditSidebar");
        }



    }
}