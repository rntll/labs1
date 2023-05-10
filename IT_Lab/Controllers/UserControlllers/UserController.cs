
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using IT_Lab.Models;
using System.Collections.Generic;

namespace IT_Lab.Controllers.UserControlllers
{
    public class UserController : Controller
    {
        private List<User> _users = new List<User>
        {
            new User { Login = "oleg@gmail.com", Password = "0", FullName = "oleg oleg", BirthDate = new DateTime(2001, 1, 1) },
            new User { Login = "maxim@gmail.com", Password = "00", FullName = "max max", BirthDate = new DateTime(2001, 1, 1) },
            new User { Login = "gleb@gmail.com", Password = "000", FullName = "gleb gleb", BirthDate = new DateTime(2001, 1, 1) }
        };
public IActionResult Login(User user)
{
    if (Request.Method != "POST" || user.Login == null || user.Password == null)
    {
        return View();
    }

    var foundUser = _users.Find(u => u.Login == user.Login);
    if (foundUser == null)
    {
        ModelState.AddModelError("Login", "Incorrect login.");
        return View(user);
    }

    if (user.Password != foundUser.Password)
    {
        ModelState.AddModelError("Password", "Incorrect password.");
        return View(user);
    }

    Response.Cookies.Append("SignIn", foundUser.Login);

    return RedirectToAction("Account");
}

public IActionResult Logout()
{
    if (Request.Cookies.ContainsKey("SignIn"))
    {
        Response.Cookies.Delete("SignIn");
    }

    return RedirectToAction("Login");
}

public IActionResult Account()
{
    var userLogin = Request.Cookies["SignIn"];

    if (userLogin == null)
    {
        return RedirectToAction("Login");
    }

    var user = _users.Find(u => u.Login == userLogin);

    if (user == null)
    {
        return RedirectToAction("Login");
    }

    return View(user);
}
}
}
