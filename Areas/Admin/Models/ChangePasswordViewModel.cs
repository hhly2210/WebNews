using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Areas.Admin.Models;

public class ChangePasswordViewModel {
    [Key]
    public int AccountID { set; get; }
    
    [Display(Name = "Mật khẩu hiện tại")]
    public string? PasswordNow { set; get; }
    
    [Display(Name = "Mật khẩu mới")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
    public string? Password { set; get; }

    [Display(Name = "Nhập lại mật khẩu mới")]
    [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
    [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau")]
    public string? ConfirmPassword { set; get; }
}
