using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebNews.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebNews.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = "Admin")]
public class RoleController : Controller {
    private readonly ILogger<RoleController> _logger;
    private readonly DataContext _context;
    public RoleController(ILogger<RoleController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Danh sach quyen
    public IActionResult Index() {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Privacy", "Home", new { Area = "Admin" });
        }

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        return View();
    }

    [HttpPost]
    public string GetData() {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));
        
        List<Role> roleList = new List<Role>();

        if(account.RoleID == 1) {
            roleList = _context.Roles.OrderByDescending(x => x.RoleID).ToList();
        }

        // var roleList = (from r in _context.Roles select r);
        var result = JsonConvert.SerializeObject(new { data = roleList});
        return result;
    }

    // Them moi quyen
    public IActionResult Create() {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Privacy", "Home", new { Area = "Admin" });
        }

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;
        
        return View();
    }

    [HttpPost] 
    public IActionResult Create(Role role) {
        _context.Roles.Add(role);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chinh sua quyen
    public IActionResult Edit(int? id) {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Privacy", "Home", new { Area = "Admin" });
        }

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null) {
            return NotFound();
        }
        var role = _context.Roles.Find(id);
        if(role == null) {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost] 
    public IActionResult Edit(int id, Role role) {
        if(id != role.RoleID || id == 0) {
            return NotFound();
        }
        _context.Update(role);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Xoa quyen
    public IActionResult Delete(int? id) {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Privacy", "Home", new { Area = "Admin" });
        }

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null || id == 0) {
            return NotFound();
        }
        var role = _context.Roles.Find(id);
        if(role == null) {
            return NotFound();
        }
        return View(role);
    }
    
    [HttpPost] 
    public IActionResult Delete(int id) {
        var delRole = _context.Roles.Find(id);
        if(delRole == null) {
            return NotFound();
        }
        _context.Roles.Remove(delRole);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chi tiet quyen
    public IActionResult Details(int? id) {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Privacy", "Home", new { Area = "Admin" });
        }

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;
        
        if(id == null || id == 0) {
            return NotFound();
        }
        var role = _context.Roles.Find(id);
        if(role == null) {
            return NotFound();
        }
        return View(role);
    }
}