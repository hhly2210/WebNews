using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {
    public partial class Commercial {
        public int CommercialID { set; get; }
        public string? Title { set; get; }
        public string? Link { set; get; }
        public bool? IsActive { set; get; }
        public string? Images { set; get; }
        public string? Content { set; get; }
        public string? Brand { set; get; }
    }
}