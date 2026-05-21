using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructre.Data
{
    // Migration seed data
    public static class CategorySeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hardware", Description = "Issues related to physical components of computers and devices.", CreatedAt = DateTime.MinValue },
                new Category { Id = 2, Name = "Software", Description = "Issues related to applications, operating systems, and software functionality.", CreatedAt = DateTime.MinValue },
                new Category { Id = 3, Name = "Network", Description = "Issues related to connectivity, internet access, and network performance.", CreatedAt = DateTime.MinValue },
                new Category { Id = 4, Name = "Security", Description = "Issues related to cybersecurity threats, data breaches, and security vulnerabilities.", CreatedAt = DateTime.MinValue },
                new Category { Id = 5, Name = "Other", Description = "Miscellaneous issues that do not fit into the above categories.", CreatedAt = DateTime.MinValue }
            );
        }
    }
}
