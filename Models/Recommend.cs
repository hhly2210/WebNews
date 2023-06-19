using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Models {

    public partial  class Recommend {

        public int RecommendID { set; get; }
        public string? RecommendName { set; get; }
        public string? Link { set; get; }
        public bool? IsActive { set; get; }
    }
}