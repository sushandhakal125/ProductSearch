using ProductSearch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        List<User> users;
        public HomeController()
        {
            _applicationDbContext = new ApplicationDbContext();
            users = _applicationDbContext.Users.ToList();
        }
        public ActionResult Index()
        {

            if (!users.Any(x => x.Name == User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1)))
                return View("~/Views/Home/Unauthorized.cshtml");
            return View(_applicationDbContext.Products.ToList());
        }

        [HttpPost]
        public JsonResult LoadNavMenus()
        {
            bool showAdminMenu = false;
            if (users.Any(x => x.Name == User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1)))
                showAdminMenu = users.Where(x => x.Name == User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1)).FirstOrDefault().AllowToEdit;
            return Json(showAdminMenu);
        }

        public ActionResult Users()
        {
            return View(users);
        }
        public ActionResult CreateUser()
        {
            return View();
        }
        public ActionResult AddUserToDB(User user)
        {
            _applicationDbContext.Users.Add(user);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Users");
        }
        public ActionResult EditUser(int Id)
        {
            var userToEdit = _applicationDbContext.Users.Single(x => x.Id == Id);
            return View(userToEdit);
        }
        [HttpPost]
        public ActionResult SaveUser(User user)
        {
            var userToSave = _applicationDbContext.Users.Single(x => x.Id == user.Id);
            userToSave.Name = user.Name;
            userToSave.code = user.code;
            userToSave.AllowToEdit = user.AllowToEdit;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Users");
        }
        public ActionResult DeleteUser(int Id)
        {
            var userToDelete = _applicationDbContext.Users.Single(x => x.Id == Id);
            _applicationDbContext.Users.Remove(userToDelete);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Users");
        }

    }
}