using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "InstagramView")]
    public class InstagramViewComponent : ViewComponent {
        private readonly DataContext _context;
        public InstagramViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}