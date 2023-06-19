using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "EntertainView")]
    public class EntertainViewComponent : ViewComponent {
        private readonly DataContext _context;
        public EntertainViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofPost = (from p in _context.PostMenus
                                where (p.IsActive == true) && (p.ParentID == 2)
                                orderby p.PostID descending
                                select p).ToList();
            // var listofPost = _context.Posts.Include()
            return await Task.FromResult((IViewComponentResult)View("Default", listofPost));
        }
    }
}