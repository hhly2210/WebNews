using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebNews.Models {

    public partial class Menu {
        public Menu() {
            Posts = new HashSet<Post>();
        }
        public int MenuID { set; get; }
        public string? MenuName { set; get; }
        public bool? IsActive { set; get; }
        public string? ControllerName { set; get; }
        public string? ActionName { set; get; }
        public int Levels { set; get; }
        public int ParentID { set; get; }
        public string? Link { set; get; }
        public int MenuOrder { set; get; }
        public int Position { set; get; }

        public virtual ICollection<Post> Posts { set; get; }
    }
}