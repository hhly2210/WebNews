using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {

    public partial class View_PostMenu {
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
        public string? MenuName { set; get; }
        public int ParentID { set; get; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }
}