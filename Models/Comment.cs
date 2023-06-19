using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {

    public partial class Comment {

        public int CommentID { set; get; }
        public long? PostID { set; get; }
        public string? FullName { set; get; }
        public string? Content { set; get; }
        public DateTime? CreatedDate { set; get; }
        public string? Avatar { set; get; }
        public bool? IsActive { set; get; }
        
        public virtual Post Post { set; get; }
    }
}