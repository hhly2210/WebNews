using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "TagCloudsView")]
    public class TagCloudsViewComponent : ViewComponent {
        private readonly DataContext _context;
        public TagCloudsViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}