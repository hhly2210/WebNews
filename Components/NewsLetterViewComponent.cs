using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "NewsLetterView")]
    public class NewsLetterViewComponent : ViewComponent {
        private readonly DataContext _context;
        public NewsLetterViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}