using System;
using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.DataManager;
using Task2.Cmd.Models;

namespace Task2.Cmd.Controllers
{
    public class VisitorController
    {
        private UnitOfWork unit;
        public VisitorController(UnitOfWork unitOfWork) => unit = unitOfWork;
        public Visitor AddVisitor(string firstName, string surName, string patronymic, string pasportNumber, DateTime birthDate, GenderType gender)
        {
            var visitors = unit.VisitorRepository.GetList();
            var lastId = 0;
            if (visitors.Count > 0)
                lastId = visitors.Max(x => x.Id);
            var nextId = lastId + 1;
            if (String.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Firstname cannot be empty");
            if (String.IsNullOrWhiteSpace(surName))
                throw new ArgumentException("Surname cannot be empty");
            if (String.IsNullOrWhiteSpace(pasportNumber))
                throw new ArgumentException("Pasport number cannot be empty");
            else if (visitors.Any(x => x.PasportNumber == pasportNumber))
                throw new Exception("Such a visitor is already in the database");
            if (birthDate > DateTime.Now.AddYears(-18))
                throw new Exception("Registered visitors must be over 18 years old");
            var visitor = new Visitor()
            {
                Id = nextId,
                FirstName = firstName,
                SurName = surName,
                Patronymic = patronymic,
                PasportNumber = pasportNumber,
                BirthDate = birthDate,
                Gender = gender
            };
            unit.VisitorRepository.Add(visitor);
            unit.Save();
            return visitor;
        }
        public List<Visitor> GetAllVisitor() => unit.VisitorRepository.GetList();
        public Visitor GetByIdVisitor(int id) => unit.VisitorRepository.GetById(id);
        public Visitor GetByPasportNumberVisitor(string pasportNumber) => unit.VisitorRepository.GetList().FirstOrDefault(x => x.PasportNumber == pasportNumber);
        public void DeleteVisitor(Visitor visitor)
        {
            unit.VisitorRepository.Delete(visitor);
            unit.Save();
        }
    }
}
