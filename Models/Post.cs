using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {

    public partial class Post {
        public Post() {
            Comments = new HashSet<Comment>();
        }

        public long PostID { set; get; }
        public string? Title { set; get; }
        public string? Abstract { set; get; }
        public string? Contents { set; get; }
        public string? Images { set; get; }
        public string? Link { set; get; }
        public string? Author { set; get; }
        public DateTime? CreatedDate { set; get; }
        public bool? IsActive { set; get; }
        public int PostOrder { set; get; }
        public int MenuID { set; get; }
        public int Category { set; get; }
        public int Status { set; get; }
        public string? SContents { set; get; }
        public int? AccountID { set; get; }

        public virtual Account Account { set; get; }
        public virtual Menu Menu { set; get; }
        public virtual ICollection<Comment> Comments { set; get; }
    }
}