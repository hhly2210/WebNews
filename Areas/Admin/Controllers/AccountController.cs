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
// [Authorize(Roles = "Admin")]
public class AccountController : Controller {
    private readonly ILogger<AccountController> _logger;
    private readonly DataContext _context;
    public AccountController(ILogger<AccountController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Danh sach tai khoan
    public IActionResult Index() {
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Index", "Profile", new { Area = "Admin"});
        } 

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        return View();
    }

    [HttpPost]
    public string GetData() {
        var mnList = (from a in _context.Accounts select new {
            accountID = a.AccountID,
            fullName = a.FullName,
            email = a.Email,
            role = a.Role.RoleName,
            lastLogin = a.LastLogin,
            active = a.Active
        });
        var result = JsonConvert.SerializeObject(new { data = mnList });
        return result;
    }

    // Dang nhap tai khoan
    [HttpGet]
    [AllowAnonymous]
    [Route("dang-nhap.html", Name = "Login")]
    public IActionResult Login(string ReturnUrl = null) {
        var accountID = HttpContext.Session.GetString("AccountID");
        if(accountID != null) return RedirectToAction("Index", "Home", new { Area = "Admin"});
        ViewBag.ReturnUrl = ReturnUrl;
        return View();
    }

    // 40:00p
    [HttpPost]
    [AllowAnonymous]
    [Route("dang-nhap.html", Name = "Login")]
    public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl = null) {
        try {
            Account kh = _context.Accounts
                .Include(p=>p.Role)
                .SingleOrDefault(p => p.Email.ToLower() == model.Email.ToLower().Trim());

            if(kh == null) {
                ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                return View(model);
            }

            // string pass = (model.Password.Trim() + kh.Salt.Trim()).ToMD5();
            string pass = (model.Password.Trim()).ToMD5();
            // string pass = model.Password;
            if(kh.Password.Trim() != pass) {
                ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                return View(model);
            }
            //Đăng nhập thành công

            //Ghi nhận thời gian đăng nhập
            kh.LastLogin = DateTime.Now;
            _context.Update(kh);
            await _context.SaveChangesAsync();

            var accountID = HttpContext.Session.GetString("AccountID");
            //Identity
            //Lưu Session MaKh
            HttpContext.Session.SetString("AccountID", kh.AccountID.ToString());

            //Identity
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kh.FullName),
                new Claim(ClaimTypes.Email, kh.Email),
                new Claim("AccountID", kh.AccountID.ToString()),
                new Claim("RoleID", kh.RoleID.ToString()),
                new Claim(ClaimTypes.Role, kh.Role.RoleName)
            };
            
            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            await HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index", "Home", new { Area = "Admin"});
        }
        catch {
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }
    }

    // Dang xuat tai khoan
    // [Route("dang-xuat.html", Name = "Logout")]
    public IActionResult Logout() {
        HttpContext.SignOutAsync();
        HttpContext.Session.Remove("AccountID");
        return RedirectToAction("Index", "Home");
    }

    // Dang ky tai khoan
    [Route("dang-ky.html", Name = "Register")]
    public IActionResult Register() {
        return View();
    }

    [HttpPost]
    [Route("dang-ky.html", Name = "Register")]
    public IActionResult Register(Account account) {
        var check = _context.Accounts.Where(m=>m.Email == account.Email).FirstOrDefault();
        if(check == null) {
            account.Password = account.Password.ToMD5();
            account.CreatedDate = DateTime.Now;
            account.Active = true;
            account.RoleID = 2;
            account.Avatar = "https://localhost:7114/contents/avatar/user.png";
            ViewBag.RegisterSuccess = "Đăng ký thành công";
        }
        else {
            ViewBag.RegisterFail = "Email đã tồn tại!";
        }
        
        _context.Accounts.Add(account);
        _context.SaveChanges();
        return View();
    }

    // Quen mat khau
    [Route("quen-mat-khau.html", Name = "Forget")]
    public IActionResult Forget() {
        return View();
    }

    [HttpPost]
    [Route("quen-mat-khau.html", Name = "Forget")]
    public IActionResult Forget(Account account) {
        // if(!ModelState.IsValid) {
        //     return View();
        // }

        var check = _context.Accounts.Where(m=>m.Email == account.Email).FirstOrDefault();
        if(check != null) {
            check.Password = account.Password.ToMD5();
            ViewBag.ForgetSuccess = "Lấy lại mật khẩu thành công!";
        }
        else {
            ViewBag.ForgetFail = "Tài khoản này không tồn tại!";
        }
        _context.SaveChanges();
        return View();
    }    

    // Them moi tai khoan
    public IActionResult Create() {
        //Kiem tra tai khoan da dang nhap hay chua
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Index", "Profile", new { Area = "Admin"});
        } 

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleID", "RoleName");

        return View();
    }

    [HttpPost] 
    public IActionResult Create(Account account) {

        account.Password = account.Password.ToMD5();
        account.CreatedDate = DateTime.Now;

        _context.Accounts.Add(account);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chinh sua tai khoan
    public IActionResult Edit(int? id) {
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Index", "Profile", new { Area = "Admin"});
        }

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleID", "RoleName");
        
        if(id == null) {
            return NotFound();
        }
        var accModel = _context.Accounts.Find(id);
        if(accModel == null) {
            return NotFound();
        }

        return View(accModel);
    }

    [HttpPost] 
    public IActionResult Edit(int id, Account account) {
        if(id != account.AccountID || id == 0) {
            return NotFound();
        }

        var accModel = _context.Accounts.AsNoTracking().Where(m=>m.AccountID == id).FirstOrDefault();

        if(accModel.Password != account.Password) {
            account.Password = account.Password.ToMD5();
        }

        _context.Update(account);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Xoa tai khoan
    public IActionResult Delete(int? id) {
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Index", "Profile", new { Area = "Admin"});
        } 

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null || id == 0) {
            return NotFound();
        }

        var accModel = _context.Accounts.Include(t=>t.Role).FirstOrDefault(m=>m.AccountID == id);

        // var accModel = _context.Accounts.Find(id);
        if(accModel == null) {
            return NotFound();
        }
        return View(accModel);
    }
    
    [HttpPost] 
    public IActionResult Delete(int id) {
        var delAccount = _context.Accounts.Find(id);
        if(delAccount == null) {
            return NotFound();
        }
        _context.Accounts.Remove(delAccount);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chi tiet tai khoan
    public IActionResult Details(int? id) {
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        if(account.RoleID != 1) {
            return RedirectToAction("Index", "Profile", new { Area = "Admin"});
        } 

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null || id == 0) {
            return NotFound();
        }
        var accModel = _context.Accounts.Include(t=>t.Role).FirstOrDefault(m=>m.AccountID == id);
        if(accModel == null) {
            return NotFound();
        }
        return View(accModel);
    }
}