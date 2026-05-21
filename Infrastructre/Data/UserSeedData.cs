using Domain.Entites;
using Infrastructre.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructre.Data
{
    // Runtime seed data
    public static class UserSeedData
    {
        private readonly static string adminPassword = "Admin@123";
        public static void UserSeed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = serviceProvider.GetRequiredService<AppDbContext>();

                if (!context.Roles.Any())
                {
                    var roles = new List<Role>
                    {
                        new Role { Name = SystemRole.TechnicianAdmin.ToString(), Code = SystemRole.TechnicianAdmin, CreatedAt = DateTime.UtcNow },
                        new Role { Name = SystemRole.Employee.ToString(), Code = SystemRole.Employee, CreatedAt = DateTime.UtcNow },
                        new Role { Name = SystemRole.Technician.ToString(), Code = SystemRole.Technician, CreatedAt = DateTime.UtcNow }
                    };

                    context.Roles.AddRange(roles);
                    context.SaveChanges();
                }


                if (!context.Users.Any())
                {
                    var adminRoleId = context.Roles.FirstOrDefault(r => r.Code == SystemRole.TechnicianAdmin).Id;
                    var user = new User
                    {
                        Password = adminPassword,
                        Name = "Admin User",
                        Email = "admin@rms.com",
                        PhoneNumber = "0785531213",
                        CreatedAt = DateTime.UtcNow,
                        RoleId = adminRoleId
                    };

                    var passwordHasher = new PasswordHasher<User>();
                    user.Password = passwordHasher.HashPassword(user, adminPassword);

                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
        }
    }
}
