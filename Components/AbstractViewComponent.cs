using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "AbstractView")]
    public class AbstractViewComponent : ViewComponent {
        private readonly DataContext _context;
        public AbstractViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}