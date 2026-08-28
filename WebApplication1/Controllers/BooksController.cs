using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(string bookname, string author, string isbn)
        {
            Book bookS = new Book();

            bool success = bookS.AddBook(bookname, author, isbn);

            if (success)
            {
                return RedirectToAction("Index", "Books");
            }

            ViewBag.error = "Failed to add book.";
            return View();
        }
    }
}