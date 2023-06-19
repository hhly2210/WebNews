using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebNews.Models;
using WebNews.Areas.Admin.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebNews.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = "Admin")]
public class AdminMenuController : Controller {
    private readonly ILogger<AdminMenuController> _logger;
    private readonly DataContext _context;
    public AdminMenuController(ILogger<AdminMenuController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

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
        
        List<AdminMenu> adMenuList = new List<AdminMenu>();

        if(account.RoleID == 1) {
            adMenuList = _context.AdminMenus.OrderByDescending(x => x.AdminMenuID).ToList();
        }
        
        // var adMenuList = (from a in _context.AdminMenus select a);
        var result = JsonConvert.SerializeObject(new { data = adMenuList});
        return result;
    }

    // Them moi AdminMenu
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

        var admnList = (from m in _context.AdminMenus
                        where (m.ItemLevel == 1)
                        select new SelectListItem() {
                            Text = m.ItemName,
                            Value = m.AdminMenuID.ToString()
                        }).ToList();
        admnList.Insert(0, new SelectListItem() {
            Text = "---- Lựa chọn ----",
            Value = "0"
        });

        ViewBag.admnList = admnList;

        return View();
    }

    [HttpPost] 
    [ValidateAntiForgeryToken]
    public IActionResult Create(AdminMenu adminMn) {
        _context.AdminMenus.Add(adminMn);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(long? id) {
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

        var adminMn = _context.AdminMenus.Find(id);
        if(adminMn == null) {
            return NotFound();
        }

        var admnList = (from m in _context.AdminMenus
                        where (m.ItemLevel == 1)
                        select new SelectListItem() {
                            Text = m.ItemName,
                            Value = m.AdminMenuID.ToString()
                        }).ToList();
        admnList.Insert(0, new SelectListItem() {
            Text = "---- Lựa chọn ----",
            Value = string.Empty
        });
        ViewBag.admnList = admnList;

        return View(adminMn);
    }

    [HttpPost] 
    public IActionResult Edit(long id, AdminMenu adminMn) {
        if(id != adminMn.AdminMenuID || id == 0) {
            return NotFound();
        }
        _context.Update(adminMn);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(long? id) {
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
        var adminMn = _context.AdminMenus.Find(id);
        if(adminMn == null) {
            return NotFound();
        }
        return View(adminMn);
    }
    
    [HttpPost] 
    public IActionResult Delete(long id) {
        var delAdminMn = _context.AdminMenus.Find(id);
        if(delAdminMn == null) {
            return NotFound();
        }
        _context.AdminMenus.Remove(delAdminMn);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Details(long? id) {
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
        var adminMn = _context.AdminMenus.Find(id);
        if(adminMn == null) {
            return NotFound();
        }
        return View(adminMn);
    }
}