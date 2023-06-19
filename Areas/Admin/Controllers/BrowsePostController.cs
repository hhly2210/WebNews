using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebNews.Models;
using Microsoft.AspNetCore.Http;
using WebNews.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebNews.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize()]
public class BrowsePostController : Controller {
    private readonly ILogger<BrowsePostController> _logger;
    private readonly DataContext _context;

    public BrowsePostController(ILogger<BrowsePostController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    // Danh sach bai viet cho duyet
    public IActionResult Index() {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });
        
        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountID == int.Parse(accountID));

        // if(account.RoleID != 1) {
        //     ViewBag.HiddenBrowsePost = "hidden";
        // }

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
        
        object postList = (from p in _context.Posts where (p.IsActive == true) orderby p.PostID descending select p).ToList(); 
        if(account.RoleID == 1) {
            postList = (from p in _context.Posts where (p.IsActive == false)
            orderby p.PostID descending
            select new {
                postID = p.PostID,
                title = p.Title,
                menuName = p.Menu.MenuName,
                author = p.Author,
                createdDate = p.CreatedDate,
                isActive = p.IsActive
            }).ToList();
        } 
        else {
            postList = (from p in _context.Posts where (p.IsActive == false && p.AccountID == account.AccountID)
            orderby p.PostID descending
            select new {
                postID = p.PostID,
                title = p.Title,
                menuName = p.Menu.MenuName,
                author = p.Author,
                createdDate = p.CreatedDate,
                isActive = p.IsActive
            }).ToList(); 
        }

        // var postList = (from p in _context.Posts select p).ToList();
        var result = JsonConvert.SerializeObject(new { data = postList });
        return result;
    }

    // Duyet bai viet
    public IActionResult Edit(long? id) {
         //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x=>x.AccountID == int.Parse(accountID));

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null) {
            return NotFound();
        } 

        var post = _context.Posts.Find(id);
        if(post == null) {
            return NotFound();
        }

        if(account.RoleID != 1) {
            if(post.AccountID != account.AccountID) {
                return RedirectToAction("Index");
            }
            ViewBag.HiddenActive = "hidden";
        }

        ViewData["MenuId"] = new SelectList(_context.Menus.Where(m=>m.MenuID != 1 && m.MenuID != 2 && m.MenuID != 5 && m.MenuID != 6), "MenuID", "MenuName");
        return View(post);
    }

    [HttpPost]
    public IActionResult Edit(long id, Post post) {
        //Kiểm tra đã đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
        var accountID = HttpContext.Session.GetString("AccountID");
        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x=>x.AccountID == int.Parse(accountID));
        if(account == null) return NotFound();

        if(account.RoleID != 1) {
            if(post.AccountID != account.AccountID) {
                return RedirectToAction("Index");
            }
            else {
                post.AccountID = account.AccountID;
                post.Author = account.FullName;
                post.IsActive = false;
            }
        }

        if(id != post.PostID || id == 0) {
            return NotFound();
        }
        _context.Update(post);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Xoa Bai viet cho duyet
    public IActionResult Delete(long? id) {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x=>x.AccountID == int.Parse(accountID));

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null || id == 0) {
            return NotFound();
        }

        var post = _context.Posts.Find(id);
        if(post == null) {
            return NotFound();
        }

        if(account.RoleID != 1) {
            if(post.AccountID != account.AccountID) {
                return RedirectToAction("Index");
            }
        }

        return View(post);
    }

    [HttpPost]
    public IActionResult Delete(long id) {
        var delPost = _context.Posts.Find(id);
        if(delPost == null) {
            return NotFound();
        }
        _context.Posts.Remove(delPost);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Chi tiet Bai viet
    public IActionResult Details(long? id) {
        //Kiểm tra đăng nhập hay chưa
        if(!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");

        //Đã đăng nhập và lấy AccountID
        var accountID = HttpContext.Session.GetString("AccountID");

        if(accountID == null) return RedirectToAction("Login", "Account", new { Area = "Admin" });

        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x=>x.AccountID == int.Parse(accountID));

        ViewBag.Avatar = account.Avatar;
        ViewBag.FullName = account.FullName;

        if(id == null || id == 0) {
            return NotFound();
        }
        var post = _context.Posts.Include(t => t.Account).Include(t => t.Menu).FirstOrDefault(m => m.PostID == id);
        if(post == null) {
            return NotFound();
        }

        if(account.RoleID != 1) {
            if(post.AccountID != account.AccountID) {
                return RedirectToAction("Index");
            }
            else {
                ViewBag.HiddenDetailsBrowsePost = "hidden";
            }
        }

        return View(post);
    }
}