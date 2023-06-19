using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebNews.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using WebNews.Areas.Admin.Models;
using WebNews.Extension;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace WebNews.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize()]
public class ProfileController : Controller {
    private readonly ILogger<ProfileController> _logger;
    private readonly DataContext _context;

    public ProfileController(ILogger<ProfileController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Thong tin tai khoan
    public IActionResult Index(Account model) {
        //Kiem tra da dang nhap hay chua
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        model.AccountID = account.AccountID;
        model.FullName = account.FullName;
        model.Email = account.Email;
        model.CreatedDate = account.CreatedDate;
        model.LastLogin = account.LastLogin;
        model.Avatar = account.Avatar;
        model.Gender = account.Gender;
        model.Birthday = account.Birthday;
        model.Address = account.Address;
        model.Story = account.Story;
        model.Message = account.Message;

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        return View(model);
    }

    [Authorize, HttpGet]
    public IActionResult EditProfile() {
        //Kiem tra da dang nhap hay chua
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        return View(account);
    }

    [Authorize, HttpPost]
    public IActionResult EditProfile(Account model) {
        //Kiem tra da dang nhap hay chua
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        account.FullName = model.FullName;
        account.Address = model.Address;
        account.Avatar = model.Avatar;
        account.Birthday = model.Birthday;
        account.Gender = model.Gender;
        account.Story = model.Story;
        account.Message = model.Message;

        _context.Update(account);
        _context.SaveChanges();

        return RedirectToAction("Index", "Profile", new { Area = "Admin" });
    }

    // Doi mat khau
    // [Route("doi-mat-khau.html", Name = "ChangePassword")]
    [Authorize, HttpGet]
    public IActionResult ChangePassword() {
        //Kiem tra da dang nhap hay chua
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        return View();
    }

    // [Route("doi-mat-khau.html", Name = "ChangePassword")]
    [Authorize, HttpPost]
    public IActionResult ChangePassword(ChangePasswordViewModel model) {
        //Kiem tra da dang nhap hay chua
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));
        
        if(account == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        string passnow = model.PasswordNow.Trim().ToMD5();
        
        //Dung mat khau cu
        if(passnow == account.Password.Trim()) {
            if(model.Password == model.ConfirmPassword) {
                account.Password = model.Password.Trim().ToMD5();
                _context.Update(account);
                _context.SaveChanges();

                ViewBag.ChangePasswordSuccess = "<div class='clearfix'><div class='pull-left alert alert-success no-margin alert-dismissable'><button type='button' class='close' data-dismiss='alert'><i class='ace-icon fa fa-times'></i></button><i class='ace-icon fa fa-check bigger-120 blue'></i> Bạn đã đổi mật khẩu thành công ! </div></div>";

                ViewBag.Avatar = account.Avatar;
                ViewBag.FullName = account.FullName;

                return View();
            }
            else {
                ViewBag.Avatar = account.Avatar;
                ViewBag.FullName = account.FullName;

                ViewBag.PasswordFail = "<div class='clearfix'><div class='pull-left alert alert-danger no-margin alert-dismissable'><button type='button' class='close' data-dismiss='alert'><i class='ace-icon fa fa-times'></i></button><i class='ace-icon fa fa-warning bigger-120 blue'></i> Xác nhận lại mật khẩu mới chưa đúng </div></div>";

                return View();
            }
        }
        else {
            ViewBag.Avatar = account.Avatar;
            ViewBag.FullName = account.FullName;

            ViewBag.PasswordFail = "<div class='clearfix'><div class='pull-left alert alert-danger no-margin alert-dismissable'><button type='button' class='close' data-dismiss='alert'><i class='ace-icon fa fa-times'></i></button><i class='ace-icon fa fa-warning bigger-120 blue'></i> Xác nhận lại mật khẩu cũ chưa đúng </div></div>";

            return View();
        }

        return View();
    }
}
