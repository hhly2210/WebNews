using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNews.Models;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace WebNews.Controllers;

// [Authorize()]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Chi tiet bai viet
    [Route("/post-{slug}-{id:long}.html", Name = "Details")]
    public IActionResult Details(long? id) {
        if(id == null) {
            return NotFound();
        }
        var post = _context.Posts
                    .Include(m=>m.Menu)
                    .Include(m=>m.Account)
                    .FirstOrDefault(m=>(m.PostID == id) && (m.IsActive == true));
        if(post == null) {
            return NotFound();
        }
        return View(post);
    }

    [HttpPost]
    [Route("/post-{slug}-{id:long}.html", Name = "Details")]
    public IActionResult Details(int? id, [FromForm]Comment comment) {
        if(User.Identity.IsAuthenticated) {
            var accountID = HttpContext.Session.GetString("AccountID");
            var account = _context.Accounts.AsNoTracking().FirstOrDefault(x=>x.AccountID == int.Parse(accountID));
            comment.FullName = account.FullName;
            comment.CreatedDate = DateTime.Now;
            comment.Avatar = account.Avatar;
            comment.IsActive = true;
        }
        else {
            comment.CreatedDate = DateTime.Now;
            comment.Avatar = "https://cdn-icons-png.flaticon.com/512/149/149071.png";
            comment.IsActive = true;
        }

        _context.Comments.Add(comment);
        _context.SaveChanges();
        var post = _context.Posts
                    .Include(m=>m.Menu)
                    .Include(m=>m.Account)
                    .FirstOrDefault(m=>(m.PostID == id) && (m.IsActive == true));
        if(post == null) {
            return NotFound();
        }
        return View(post);
    }

    // Danh sach bai viet
    [Route("/list-{slug}-{id:int}.html", Name = "List")]
    public IActionResult List(int? id, int? page, string? name) {
        var pageNumber = page ?? 1;
        int pageSize = 10;
        if(id == 1) {
            return View("Index");
        }
        if(id == 6) {
            return RedirectToAction("Index", "Contact");
        }
        if(id == null) {
            return NotFound();
        }
        var list = _context.Posts
                    .Include(m=>m.Menu)
                    .Include(m=>m.Account)
                    .Where(m=>(m.MenuID == id) && (m.IsActive == true))
                    .OrderByDescending(m=>m.PostID)
                    .ToPagedList(pageNumber, pageSize);
        if(list == null) {
            return NotFound();
        }

        if(!string.IsNullOrEmpty(name)) {
            name = name.ToLower();
            list = list.Where(m=> (m.MenuID == id) && (m.Title.ToLower().Contains(name)) || (m.Author.ToLower().Contains(name)) || (m.CreatedDate.ToString().Contains(name)))
                    .OrderByDescending(m=>m.PostID)
                    .ToPagedList(pageNumber, pageSize);
        }
        
        return View(list);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
