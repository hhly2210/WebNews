using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNews.Models;
using X.PagedList;

namespace WebNews.Controllers;

public class AboutController : Controller
{
    private readonly ILogger<AboutController> _logger;
    private readonly DataContext _context;

    public AboutController(ILogger<AboutController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("gioi-thieu.html")]
    public IActionResult Index()
    {
        return View();
    }
}