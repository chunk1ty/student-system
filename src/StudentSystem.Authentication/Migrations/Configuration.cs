using System.Data.Entity.Migrations;

namespace StudentSystem.Authentication.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<StudentSystemAuthDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(StudentSystemAuthDbContext context)
        {
           
        }
    }
}
