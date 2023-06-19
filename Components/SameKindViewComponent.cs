using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "SameKindView")]
    public class SameKindViewComponent : ViewComponent {
        private readonly DataContext _context;
        public SameKindViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id) {
            var listofSame = (from p in _context.PostMenus
                                where (p.IsActive == true) && (p.MenuID == id)
                                orderby p.PostID descending
                                select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofSame));
        }
    }
}