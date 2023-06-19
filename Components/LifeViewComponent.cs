using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "LifeView")]
    public class LifeViewComponent : ViewComponent {
        private readonly DataContext _context;
        public LifeViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofPost = (from p in _context.PostMenus
                                where(p.IsActive == true) && (p.MenuID == 3)
                                select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofPost));
        }
    }
}