using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.User
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "El nombre del usuario es requerido.")]
        [DataType(DataType.Text, ErrorMessage = "El nombre del usuario solo acepta texto.")]
        [StringLength(maximumLength: 50, ErrorMessage = "El nombre del usuario no debe tener más de {1} caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        [DataType(DataType.Text, ErrorMessage = "El apellido del usuario solo acepta texto.")]
        [StringLength(maximumLength: 50, ErrorMessage = "El apellido del usuario no debe tener más de {1} caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Id del usuario es requerido.")]
        public int UserId { get; set; }
    }
}