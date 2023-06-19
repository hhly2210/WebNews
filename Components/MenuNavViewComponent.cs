using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "MenuNavView")]
    public class MenuNavView : ViewComponent {
        private readonly DataContext _context;
        public MenuNavView(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofMenu = (from m in _context.Menus
                                where (m.IsActive == true) && (m.Position == 1)
                                select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofMenu));
        }
    }
}