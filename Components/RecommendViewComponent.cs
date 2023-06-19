using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "RecommendView")]
    public class RecommendViewComponent : ViewComponent {
        private readonly DataContext _context;
        public RecommendViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var listofRecommend = (from p in _context.Recommends
                                where (p.IsActive == true)
                                orderby p.RecommendID descending
                                select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofRecommend));
        }
    }
}