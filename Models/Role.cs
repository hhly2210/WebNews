using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {

    public partial class Role {
        public Role() {
            Accounts = new HashSet<Account>();
        }
        public int RoleID { set; get; }
        public string? RoleName { set; get; }
        public string? RoleDescription { set; get; }

        public virtual ICollection<Account> Accounts { set; get; }
    }
}