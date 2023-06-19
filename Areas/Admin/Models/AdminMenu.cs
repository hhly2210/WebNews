using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Areas.Admin.Models;


public partial class AdminMenu {
    
    public long AdminMenuID { set; get; }
    public string? ItemName { set; get; }
    public int ItemLevel { set; get; }
    public int ParentLevel { set; get; }
    public int ItemOrder { set; get; }
    public bool? IsActive { set; get; }
    public string? ItemTarget { set; get; }
    public string? AreaName { set; get; }
    public string? ControllerName { set; get; }
    public string? ActionName { set; get; }
    public string? Icon { set; get; }
    public string? IdName { set; get;}
}
