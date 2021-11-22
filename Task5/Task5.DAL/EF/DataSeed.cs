using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Task5.DAL.Entities;

namespace Task5.DAL.EF
{
    public class DataSeed
    {
        public static void Seed(DataContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!db.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Base"
                    },
                    new Category
                    {
                        Name = "Vip"
                    }
                };

                foreach (var item in categories)
                {
                    db.Categories.Add(item);
                }
                db.SaveChanges();

                var categoryDates = new List<CategoryDate>
                {
                    new CategoryDate
                    {
                        StartDate = new DateTime(2010, 1, 1),
                        CategoryId = categories[0].Id,
                        Price = 1000
                    },
                    new CategoryDate
                    {
                        StartDate = new DateTime(2010, 1, 1),
                        CategoryId = categories[1].Id,
                        Price = 3000
                    },
                };

                foreach (var item in categoryDates)
                {
                    db.CategoryDates.Add(item);
                }
                db.SaveChanges();

                var rooms = new List<Room>
                {
                    new Room
                    {
                        CategoryId = categories[0].Id,
                        Number = 1
                    },
                    new Room
                    {
                        CategoryId = categories[1].Id,
                        Number = 2
                    },
                };

                foreach (var item in rooms)
                {
                    db.Rooms.Add(item);
                }
                db.SaveChanges();

                var guests = new List<Guest>
                {
                    new Guest
                    {
                        FirstName = "FirstGuest",
                        LastName = "FirstLastName",
                        BirthDate = new DateTime(2000, 1, 1),
                        Passport = "1234567890",
                        Patronymic = "SomePatronymic"
                    },
                    new Guest
                    {
                        FirstName = "SecondGuest",
                        LastName = "SecondLastName",
                        BirthDate = new DateTime(1998, 12, 12),
                        Passport = "0987654321"
                    }
                };

                foreach (var item in guests)
                {
                    db.Guests.Add(item);
                }
                db.SaveChanges();

                var stays = new List<Stay>
                {
                    new Stay {
                        RoomId = rooms[0].Id,
                        GuestId = guests[0].Id,
                        StartDate = DateTime.Now.AddDays(-10).Date,
                        EndDate = DateTime.Now.AddDays(-6).Date,
                        CheckedIn = true,
                        CheckedOut = true
                    },
                    new Stay {
                        RoomId = rooms[1].Id,
                        GuestId = guests[0].Id,
                        StartDate = DateTime.Now.AddDays(-8).Date,
                        EndDate = DateTime.Now.AddDays(-5).Date,
                        CheckedIn = true,
                        CheckedOut = true
                    },
                    new Stay {
                        RoomId = rooms[0].Id,
                        GuestId = guests[1].Id,
                        StartDate = DateTime.Now.AddDays(-5).Date,
                        EndDate = DateTime.Now.AddDays(-2).Date,
                        CheckedIn = true,
                        CheckedOut = true
                    },
                    new Stay {
                        RoomId = rooms[1].Id,
                        GuestId = guests[1].Id,
                        StartDate = DateTime.Now.AddDays(-4).Date,
                        EndDate = DateTime.Now.AddDays(-3).Date,
                        CheckedIn = true,
                        CheckedOut = true
                    },
                    new Stay {
                        RoomId = rooms[0].Id,
                        GuestId = guests[1].Id,
                        StartDate = DateTime.Now.AddDays(-2).Date,
                        EndDate = DateTime.Now.AddDays(-1).Date,
                        CheckedIn = true,
                        CheckedOut = false
                    },
                    new Stay {
                        RoomId = rooms[1].Id,
                        GuestId = guests[2].Id,
                        StartDate = DateTime.Now.AddDays(-3).Date,
                        EndDate = DateTime.Now.AddDays(-1).Date,
                        CheckedIn = false,
                        CheckedOut = false
                    },
                    new Stay {
                        RoomId = rooms[0].Id,
                        GuestId = guests[2].Id,
                        StartDate = DateTime.Now.Date,
                        EndDate = DateTime.Now.AddDays(3).Date,
                        CheckedIn = false,
                        CheckedOut = false
                    }
                };

                foreach (var item in stays)
                {
                    db.Stays.Add(item);
                }
                db.SaveChanges();
            }

            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole{ Name = "Admin" },
                    new IdentityRole{ Name = "Moderator" },
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                if (!userManager.Users.Any())
                {



                    var users = new List<User>
                                {
                                    new User
                                    {
                                        UserName = "Admin",
                                        Email = "admin@test.com"
                                    },
                                    new User
                                    {
                                        UserName = "Moderator",
                                        Email = "moderator@test.com"
                                    }
                                };


                    foreach (var user in users)
                    {
                        userManager.CreateAsync(user, "1Qa2Ws!").Wait();
                    }
                    userManager.AddToRolesAsync(users[0], roles.Select(x => x.Name)).Wait();
                    userManager.AddToRolesAsync(users[1], new List<string> { "Moderator" });
                    db.SaveChanges();
                }
            }
        }
    }
}
