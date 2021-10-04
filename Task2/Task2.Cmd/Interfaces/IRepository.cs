using System.Collections.Generic;
using Task2.Cmd.Models;

namespace Task2.Cmd.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        List<T> GetList();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
