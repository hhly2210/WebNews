using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNews.Models;
using X.PagedList;

namespace WebNews.Controllers;

public class SearchController : Controller
{
    private readonly ILogger<SearchController> _logger;
    private readonly DataContext _context;

    public SearchController(ILogger<SearchController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("tim-kiem.html")]
    [HttpGet]
    public IActionResult Index(int? id, int? page, string name) {
        var pageNumber = page ?? 1;
        int pageSize = 10;
        var list = _context.Posts
                    .Include(m=>m.Menu)
                    .Include(m=>m.Account)
                    .Where(m=>m.IsActive == true)
                    .OrderByDescending(m=>m.PostID)
                    .ToPagedList(pageNumber, pageSize);
        if(list == null) {
            return NotFound();
        }
        if(!string.IsNullOrEmpty(name)) {
            list = list.Where(x=>x.Title.Contains(name))
                    .OrderByDescending(m=>m.PostID)
                    .ToPagedList(pageNumber, pageSize);;
        }
        return View(list);
    }

}