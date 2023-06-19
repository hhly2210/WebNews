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
public class CommercialController : Controller {
    private readonly ILogger<CommercialController> _logger;
    private readonly DataContext _context;
    public CommercialController(ILogger<CommercialController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Danh sach quang cao
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
        
        List<Commercial> cmList = new List<Commercial>();

        if(account.RoleID == 1) {
            cmList = _context.Commercials.OrderByDescending(x => x.CommercialID).ToList();
        }

        // var cmList = (from cm in _context.Commercials select cm);
        var result = JsonConvert.SerializeObject(new { data = cmList });
        return result;
    } 

    // Them moi quang cao
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
    [ValidateAntiForgeryToken]
    public IActionResult Create(Commercial cm) {
        _context.Commercials.Add(cm);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chinh sua quang cao
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
        var cm = _context.Commercials.Find(id);
        if(cm == null) {
            return NotFound();
        }
        return View(cm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Commercial cm) {
        if(id != cm.CommercialID || id == 0) {
            return NotFound();
        }
        _context.Update(cm);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Xoa quang cao
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

        if(id == null) {
            return NotFound();
        }
        var cm = _context.Commercials.Find(id);
        if(cm == null) {
            return NotFound();
        }
        return View(cm);
    }

    [HttpPost]
    public IActionResult Delete(int id) {
        var deleCom = _context.Commercials.Find(id);
        if(deleCom == null) {
            return NotFound();
        }
        _context.Commercials.Remove(deleCom);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chi tiet quang cao
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
        
        if(id == null) {
            return NotFound();
        }
        var cm = _context.Commercials.Find(id);
        if(cm == null) {
            return NotFound();
        }
        return View(cm);
    }
}