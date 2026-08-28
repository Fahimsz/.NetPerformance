using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            // BaseMember baseMember = new BaseMember();
            // List<BaseMember> users = baseMember.getAllUsers();
            // return View(users);
            return View();
        }

        public IActionResult Edit(int id)
        {
            BaseMember baseMember = new BaseMember();
            BaseMember? user = baseMember.getUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
