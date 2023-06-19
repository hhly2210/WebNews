using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNews.Areas.Admin.Models;

public class LoginViewModel {
    [Key]
    [MaxLength(50)]
    [Required(ErrorMessage = "Vui lòng nhập Email")]
    [Display(Name = "Địa chỉ Email")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Vui lòng nhập Email")]
    public string Email { set; get; }

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [MaxLength(30, ErrorMessage = "Mật khẩu chỉ được sử dụng 30 ký tự")]
    public string Password { set; get; }
}
