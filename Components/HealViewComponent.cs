using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "HealView")]
    public class HealViewComponent : ViewComponent {
        private readonly DataContext _context;
        public HealViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofPost = (from p in _context.PostMenus
                                where(p.IsActive == true) && (p.MenuID == 4)
                                orderby p.PostID descending
                                select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofPost));
        }
    }
}