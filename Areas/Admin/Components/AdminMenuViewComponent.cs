using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Areas.Admin.Components;

[ViewComponent(Name = "AdminMenuView")]
[Area("Admin")]
[Authorize()]
public class AdminMenuComponent : ViewComponent {
    private readonly ILogger<AdminMenuComponent> _logger;
    private readonly DataContext _context;

    public AdminMenuComponent(ILogger<AdminMenuComponent> logger, DataContext context) {
        _logger = logger;
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync() {
        var accountID = HttpContext.Session.GetString("AccountID");
        var account = _context.Accounts.AsNoTracking().FirstOrDefault(x=>x.AccountID == int.Parse(accountID));
        var mnList = (from mn in _context.AdminMenus
                        where (mn.IsActive == true)
                        select mn).ToList();
        if(account.RoleID == 1) {
            mnList = (from mn in _context.AdminMenus
                        where (mn.IsActive == true)
                        select mn).ToList();
        }
        if(account.RoleID != 1) {
            mnList = (from mn in _context.AdminMenus
                        where (mn.IsActive == true 
                                && mn.AdminMenuID != 20 
                                && mn.AdminMenuID != 10 
                                && mn.AdminMenuID != 13
                                && mn.AdminMenuID != 19
                                && mn.AdminMenuID != 23
                                )
                        select mn).ToList();
        }
        return await Task.FromResult((IViewComponentResult)View("Default", mnList));
        // var mnList = (from mn in _context.AdminMenus
        //                 where (mn.IsActive == true)
        //                 select mn).ToList();
        // return await Task.FromResult((IViewComponentResult)View("Default", mnList));
    }
}