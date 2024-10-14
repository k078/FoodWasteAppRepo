using Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Medewerker",
                    NormalizedName = "MEDEWERKER".ToUpper(),
                },
                new IdentityRole
                {
                    Id="2",
                    Name="Student",
                    NormalizedName="STUDENT".ToUpper(),
                }
            );

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "kalle@mail.com",
                    Email = "kalle@mail.com",
                    PasswordHash = hasher.HashPassword(null, "Test1!")
                },
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "truus@avans.nl",
                    Email = "truus@avans.nl",
                    PasswordHash = hasher.HashPassword(null, "Truus1!")
                }

            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1",

                },
                new IdentityUserRole<string>
                {
                    RoleId = "2",
                    UserId = "2",
                });
        }

    }
}
