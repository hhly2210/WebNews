using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebNews.Models;
using X.PagedList;

namespace WebNews.Controllers;

public class ContactController : Controller {
    private readonly ILogger<ContactController> _logger;

    private readonly DataContext _context;

    public ContactController(ILogger<ContactController> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Contact contact) {
        _context.Contacts.Add(contact);
        _context.SaveChanges();
        return RedirectToAction("Success");
    }

    public IActionResult Success() {
        return View();
    }
}