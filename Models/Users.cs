using EmployeeManagement.Models.DTO;

namespace EmployeeManagement.Models
{
    public class Users
    {
        private UsersDTO user;

        public Users()
        {
        }

        public Users(UsersDTO user)
        {
            this.user = user;
        }

        // Without Id field
        public Users(string fullName, DateTime dOB, string gender, string address, string email, DateTime joinedDate, string phone, string idPos, int idRole, string password)
        {
            FullName = fullName;
            DOB = dOB;
            Gender = gender;
            Address = address;
            Email = email;
            JoinedDate = joinedDate;
            Phone = phone;
            IdPos = idPos;
            IdRole = idRole;
            Password = password;
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email {  get; set; }
        public DateTime JoinedDate {  get; set; }
        public string Phone { get; set; }
        public string IdPos { get; set; }
        public int IdRole { get; set; }
        public string Password { get; set; }
    }
}
