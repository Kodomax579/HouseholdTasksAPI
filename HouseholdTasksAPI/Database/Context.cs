using HouseholdTasksAPI.DataModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace HouseholdTasksAPI.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<MealSuggestion> MealSuggestions { get; set; }
    }
}
