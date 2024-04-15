using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.User
{
    public class UserUpdatePasswordDTO
    {
        [Required(ErrorMessage = "La contraseña vieja es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "La contraseña vieja debe tener entre 8 y 16 caracteres.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "La contraseña nueva es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "La contraseña nueva debe tener entre 8 y 16 caracteres.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Id del usuario es requerido.")]
        public int UserId { get; set; }
    }
}
