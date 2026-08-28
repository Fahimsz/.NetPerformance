using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class AuthController : Controller
{
    // GET: Auth
    //public ActionResult Login()
    //{
    //    return View();
    //}
    //[HttpPost]
    //public ActionResult Login(string username, string password)
    //{

    //    BaseMember baseMember = new BaseMember();

    //    //Data Table login by Sp

    //    //DataTable dt = baseMember.validateasTableBySp(username, password);
    //    //List<BaseMember> baseMembers = new List<BaseMember>();
    //    //foreach (DataRow dr in dt.Rows)
    //    //{
    //    //    BaseMember bm = new BaseMember();
    //    //    bm.Id = Convert.ToInt32(dr["Id"]);
    //    //    bm.Username = dr["Username"].ToString();
    //    //    bm.Password = dr["PasswordHash"].ToString();
    //    //    baseMembers.Add(bm);
    //    //}
    //    //if (baseMembers.Count > 0)
    //    //{
    //    //    HttpContext.Session.SetString("username", username);
    //    //}
    //    //else
    //    //{
    //    //    ViewBag.error = "Invalid username or password";
    //    //}








    //    //      //List Login

    //    List<BaseMember> baseMembers = baseMember.validateasList(username, password);
    //    BaseMember? member = baseMembers.FirstOrDefault(m => m.Username == username && m.Password == password);
    //    if (member is not null)
    //    {
    //        HttpContext.Session.SetString("username", username);
    //        return RedirectToAction("Index", "Home", new { username });
    //    }

    //    ViewBag.error = "Invalid username or password";
    //    return View();
    //}

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(string username, string password)
    {
        BaseMember baseMember = new BaseMember();
        bool success = baseMember.registerUser(username, password);

        if (success)
        {
            return RedirectToAction("Index", "Users");

        }

        ViewBag.error = "Registration failed. Please try again.";
        return View();
    }
}
