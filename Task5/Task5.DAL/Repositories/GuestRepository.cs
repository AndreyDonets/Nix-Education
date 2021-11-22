﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.DAL.EF;
using Task5.DAL.Entities;
using Task5.DAL.Interfaces;

namespace Task5.DAL.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private DataContext db;

        public GuestRepository(DataContext db) => this.db = db;

        public void Create(Guest item)
        {
            if (db.Guests.Find(item.Id) == null)
                db.Guests.Add(item);
        }
        public Guest Get(Guid id) => db.Guests.Find(id);
        public Guest GetByPassport(string passport) => db.Guests.AsNoTracking().FirstOrDefault(x => x.Passport == passport);
        public IEnumerable<Guest> GetAll() => db.Guests.AsNoTracking();
        public void Delete(Guid id)
        {
            var item = db.Guests.Find(id);
            if (item != null)
                db.Guests.Remove(item);
        }
        public void Update(Guest item)
        {
            if (db.Guests.Find(item.Id) != null)
                db.Entry(item).State = EntityState.Modified;
        }

        public async Task CreateAsync(Guest item)
        {
            if (db.Guests.Find(item.Id) == null)
            {
                db.Guests.Add(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task<Guest> GetAsync(Guid id) => await db.Set<Guest>().FindAsync(id);
        public async Task<Guest> GetByPassportAsync(string passport) => await db.Guests.AsNoTracking().FirstOrDefaultAsync(x => x.Passport == passport);
        public async Task<IEnumerable<Guest>> GetAllAsync() => await db.Set<Guest>().AsNoTracking().ToListAsync();
        public async Task DeleteAsync(Guid id)
        {
            var item = db.Guests.Find(id);
            if (item != null)
            {
                db.Set<Guest>().Remove(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Guest item)
        {
            if (item != null)
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
    }
}
