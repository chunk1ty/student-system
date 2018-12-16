using StudentSystem.Persistence;
using System.Data.Entity.Migrations;

namespace StudentSystem.Data.Services.Tests
{
    public sealed class TestDbConfiguration : DbMigrationsConfiguration<StudentSystemDbContext>
    {
        public TestDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}