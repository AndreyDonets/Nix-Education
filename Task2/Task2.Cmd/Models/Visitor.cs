using System;

namespace Task2.Cmd.Models
{
    public enum GenderType
    {
        Man = 1,
        Woman
    }
    public class Visitor : BaseEntity
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public string PasportNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderType Gender { get; set; }
    }
}
