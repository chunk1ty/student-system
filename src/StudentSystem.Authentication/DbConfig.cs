using System.Data.Entity;

using StudentSystem.Authentication.Migrations;

namespace StudentSystem.Authentication
{
    public class DbConfig
    {
        public static void RegisterDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemAuthDbContext, Configuration>());
        }
    }
}
