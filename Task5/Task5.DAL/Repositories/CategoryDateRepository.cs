using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task5.DAL.EF;
using Task5.DAL.Entities;
using Task5.DAL.Interfaces;

namespace Task5.DAL.Repositories
{
    public class CategoryDateRepository : ICategoryDateRepository
    {
        private DataContext db;

        public CategoryDateRepository(DataContext db) => this.db = db;

        public void Create(CategoryDate item)
        {
            if (db.CategoryDates.Find(item.Id) == null && db.Categories.Find(item.CategoryId) != null)
                db.CategoryDates.Add(item);
        }
        public CategoryDate Get(Guid id) => db.CategoryDates.Find(id);
        public IEnumerable<CategoryDate> GetAll() => db.CategoryDates.AsNoTracking();
        public void Delete(Guid id)
        {
            var item = db.CategoryDates.Find(id);
            if (item != null)
                db.CategoryDates.Remove(item);
        }
        public void Update(CategoryDate item)
        {
            if (db.CategoryDates.Find(item.Id) != null)
                db.CategoryDates.Update(item);
        }

        public async Task CreateAsync(CategoryDate item)
        {
            if (db.CategoryDates.Find(item.Id) == null)
            {
                db.CategoryDates.Add(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task<CategoryDate> GetAsync(Guid id) => await db.Set<CategoryDate>().FindAsync(id);
        public async Task<IEnumerable<CategoryDate>> GetAllAsync() => await db.Set<CategoryDate>().AsNoTracking().ToListAsync();
        public async Task DeleteAsync(Guid id)
        {
            var item = db.CategoryDates.Find(id);
            if (item != null)
            {
                db.Set<CategoryDate>().Remove(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(CategoryDate item)
        {
            if (db.CategoryDates.Find(item.Id) != null)
            {
                db.Set<CategoryDate>().Update(item);
                await db.SaveChangesAsync();
            }
        }
    }
}
