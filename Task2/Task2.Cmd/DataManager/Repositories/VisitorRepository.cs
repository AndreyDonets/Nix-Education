using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.Interfaces;
using Task2.Cmd.Models;

namespace Task2.Cmd.DataManager.Repositories
{
    public class VisitorRepository : IRepository<Visitor>
    {
        private Context _context;
        public VisitorRepository(Context context) => _context = context;
        public void Add(Visitor entity)
        {
            if (GetById(entity.Id) == null)
                _context.Visitors.Add(entity);
        }
        public void Delete(Visitor entity)
        {
            if (GetById(entity.Id) != null)
                _context.Visitors.Remove(entity);
        }
        public Visitor GetById(int id) => _context.Visitors.FirstOrDefault(x => x.Id == id);
        public List<Visitor> GetList() => _context.Visitors.ToList();
        public void Update(Visitor entity)
        {
            Delete(entity);
            Add(entity);
        }
    }
}
