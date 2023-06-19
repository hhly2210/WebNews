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
public class ContactController : Controller {
    private readonly ILogger<ContactController> _logger;
    private readonly DataContext _context;
    public ContactController(ILogger<ContactController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Danh sach lien he
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
        
        List<Contact> ctList = new List<Contact>();

        if(account.RoleID == 1) {
            ctList = _context.Contacts.OrderByDescending(x => x.ContactID).ToList();
        }

        // var cmList = (from cm in _context.Commercials select cm);
        var result = JsonConvert.SerializeObject(new { data = ctList });
        return result;
    }

    // Xoa lien he
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
        var ct = _context.Contacts.Find(id);
        if(ct == null) {
            return NotFound();
        }
        return View(ct);
    }

    [HttpPost]
    public IActionResult Delete(int id) {
        var delContact = _context.Contacts.Find(id);
        if(delContact == null) {
            return NotFound();
        }
        _context.Contacts.Remove(delContact);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chi tiet lien he
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
        var ct = _context.Contacts.Find(id);
        if(ct == null) {
            return NotFound();
        }
        return View(ct);
    }
}