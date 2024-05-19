namespace EmployeeManagement.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string FormType { get; set; }
        public string FormName { get; set; }
        public byte[] FormContent { get; set; }

        //public Users User { get; set; }
    }
}
