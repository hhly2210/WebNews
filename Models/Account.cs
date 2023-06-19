using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {

    public partial class Account {
        public Account() {
            Posts = new HashSet<Post>();
        }
        public int AccountID { set; get; }
        public string? FullName { set; get; }
        public string? Email { set; get; }
        public string? Password { set; get; }
        public string? Salt { set; get; }
        public bool? Active { set; get; }
        public DateTime? CreatedDate { set; get; }
        public int? RoleID { set; get; }
        public DateTime? LastLogin { set; get; }
        public string? Avatar { set; get; }
        public int? Gender { set; get; }
        public DateTime? Birthday { set; get; }
        public string? Story { set; get; }
        public string? Message { set; get; }
        public string? Address { set; get; }

        public virtual Role Role { set; get; }
        public virtual ICollection<Post> Posts { set; get; }
    }
}