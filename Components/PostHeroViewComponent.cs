using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "PostHeroView")]
    public class PostViewComponent : ViewComponent {
        public readonly DataContext _context;
        public PostViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofPost = (from p in _context.PostMenus
                                where (p.IsActive == true)
                                orderby p.PostID descending
                                select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofPost));
        }
    }
}