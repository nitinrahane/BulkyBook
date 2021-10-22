using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db = null;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }       
        public IActionResult Index()
        {
            return View();
        }
       

        #region API Calls
        [HttpGet]
        public IActionResult GetAll() {
            var userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(x => x.Id == roleId).Name;
                if (user.Company == null) {
                    user.Company = new Company()
                    {
                        Name = "",
                    };
                }
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var obj = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(obj == null)
            {
                return Json(new { success=false, message = "Error while locking/ unlocking"});
            }
            if (obj.LockoutEnd != null && obj.LockoutEnd > DateTime.Now)
            {
                obj.LockoutEnd = DateTime.Now;

            }
            else {
                obj.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation completed" });
        }



        #endregion 
    }
}
