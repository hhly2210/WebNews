using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "CommercialView")]
    public class CommercialViewComponent : ViewComponent {
        private readonly DataContext _context;
        public CommercialViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofCommercial = (from p in _context.Commercials
                                where (p.IsActive == true)
                                orderby p.CommercialID descending
                                select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofCommercial));
        }
    }
}