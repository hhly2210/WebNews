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
public class MenuController : Controller {
    private readonly ILogger<MenuController> _logger;
    private readonly DataContext _context;
    public MenuController(ILogger<MenuController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Danh sach Menu
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
    public string GetData()
    {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));
        
        List<Menu> mnList = new List<Menu>();

        if(account.RoleID == 1) {
            mnList = _context.Menus.OrderByDescending(x => x.MenuID).ToList();
        }

        // var mnList = (from mn in _context.Menus select mn);

        var result = JsonConvert.SerializeObject(new { data = mnList });
        return result;
    }

    // Them moi Menu
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
        
        var mnList = (from m in _context.Menus
                        where (m.Levels == 1)
                        select new SelectListItem() {
                            Text = m.MenuName,
                            Value = m.MenuID.ToString()
                        }).ToList();
        mnList.Insert(0, new SelectListItem() {
            Text = "---- Lựa chọn ----",
            Value = "0"
        });
        ViewBag.mnList = mnList;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("MenuID, MenuName, IsActive, ControllerName, Levels, ParentID, Link, MenuOrder, Position")] Menu mn) {    
        _context.Menus.Add(mn);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chinh sua Menu
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
        var mn = _context.Menus.Find(id);
        if(mn == null) {
            return NotFound();
        }
        
        var mnList = (from m in _context.Menus.Where(m=>m.Levels == 1)
                        select new SelectListItem() {
                            Text = m.MenuName,
                            Value = m.MenuID.ToString()
                        }).ToList();
        mnList.Insert(0, new SelectListItem() {
            Text = "---- Lựa chọn ----",
            Value = string.Empty
        });
        ViewBag.mnList = mnList;
        return View(mn);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("MenuID, MenuName, IsActive, ControllerName, Levels, ParentID, Link, MenuOrder, Position")] Menu mn) {
        if(id != mn.MenuID || id == 0) {
            return NotFound();
        }
        _context.Update(mn);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Xoa Menu
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
        var mn = _context.Menus.Find(id);
        if(mn == null) {
            return NotFound();
        }
        return View(mn);
    }

    [HttpPost]
    public IActionResult Delete(int id) {
        var deleMenu = _context.Menus.Find(id);
        if(deleMenu == null) {
            return NotFound();
        }
        _context.Menus.Remove(deleMenu);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chi tiet Menu
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
        var mn = _context.Menus.Find(id);
        if(mn == null) {
            return NotFound();
        }
        return View(mn);
    }
    
}