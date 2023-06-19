using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {
    [Table("View_PostAccount")]
    public class View_PostAccount {
        [Key]
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
        [StringLength(255)]
        public string? SContents { set; get; }
        public int? AccountID { set; get; }
        public string? FullName { set; get; }
        public string? Avatar { set; get; }
    }
}