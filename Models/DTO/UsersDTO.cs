using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.DTO
{
    public class UsersDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string IdPos { get; set; }

        [Required]
        public int IdRole { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
