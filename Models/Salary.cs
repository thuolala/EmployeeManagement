namespace EmployeeManagement.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long Base { get; set; }
        public int AllowedOff { get; set; }
        public int ActualOff { get; set; }
        public long? Bonus { get; set; }
        public long? Deduction { get; set; }
        public long Final { get; set; }
        public DateTime Date { get; set; }
        //public Users User { get; set; }
    }
}
