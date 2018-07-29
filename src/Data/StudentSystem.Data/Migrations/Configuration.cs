using System.Data.Entity.Migrations;

namespace StudentSystem.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<StudentSystemDbContext>
    {
        // TODO AutomaticMigrationsEnabled = false
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(StudentSystemDbContext context)
        {
           
        }
    }
}
