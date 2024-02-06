using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    
    public class UserCreationDTO
    {
        [Required(ErrorMessage = "El id del ROL es requerido.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El id del ROL solo acepta números.")]
        public int RoleId { get; set; }

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

        [Required]
        [RegularExpression("^[0-9]{7,8}$", ErrorMessage = "El número de documento debe contener entre 7 y 8 dígitos.")]
        public string Dninumber { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 16 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El id del distrito es requerido.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El id del distrito solo acepta números.")]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "El nombre de la calle del usuario es requerido.")]
        [DataType(DataType.Text, ErrorMessage = "El nombre de la calle del usuario solo acepta texto.")]
        [StringLength(maximumLength: 100, ErrorMessage = "El nombre de la calle del usuario no debe tener más de {1} caracteres")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "El nombre del barrio del usuario es requerido.")]
        [DataType(DataType.Text, ErrorMessage = "El nombre del barrio del usuario solo acepta texto.")]
        [StringLength(maximumLength: 100, ErrorMessage = "El nombre del barrio del usuario no debe tener más de {1} caracteres")]
        public string Neighborhood { get; set; }

        [Required(ErrorMessage = "El numero de la calle del usuario es requerido.")]
        [DataType(DataType.Text, ErrorMessage = "El numero de la calle del usuario solo acepta texto.")]
        [StringLength(maximumLength: 10, ErrorMessage = "El numero de la calle del barrio del usuario no debe tener más de {1} caracteres")]
        public string StreetNumber { get; set; }

        [Required(ErrorMessage = "El codigo postal de la localidad del usuario es requerido.")]
        [DataType(DataType.Text, ErrorMessage = "El codigo postal de la localidad del usuario solo acepta texto.")]
        [StringLength(maximumLength: 50, ErrorMessage = "El codigo postal de la localidad del usuario no debe tener más de {1} caracteres")]
        public string PostalCode {  get; set; } 
    }
}
