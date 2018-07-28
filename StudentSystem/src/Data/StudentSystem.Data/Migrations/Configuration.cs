using System.Data.Entity.Migrations;

namespace StudentSystem.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<StudentSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(StudentSystem.Data.StudentSystemDbContext context)
        {
           
        }
    }
}
