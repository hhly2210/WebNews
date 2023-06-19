using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebNews.Models {

    public partial class Contact {

        public int ContactID { set; get; }
        public string? FullName { set; get; }
        public string? Email { set; get; }
        public string? Subject { set; get; }
        public string? Message { set; get; }
    }
}