using Microsoft.AspNetCore.Mvc;
using WebNews.Models;

namespace WebNews.Components {
    [ViewComponent(Name = "CommentView")]
    public class CommentViewComponent : ViewComponent {
        private readonly DataContext _context;
        public CommentViewComponent(DataContext context) {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(long? postID) {
            var listofComment = (from c in _context.Comments
                                    where(c.PostID == postID) && (c.IsActive == true)
                                    orderby c.CreatedDate descending
                                    select c).ToList();
            ViewBag.Count = listofComment.Count();
            return await Task.FromResult((IViewComponentResult)View("Default", listofComment));
        }
    }
}