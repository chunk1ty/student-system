using System.Data.Entity.Migrations;

namespace StudentSystem.Persistence.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<StudentSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(StudentSystemDbContext context)
        {
           
        }
    }
}
