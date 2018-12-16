using System.Data.Entity;

using StudentSystem.Persistence.Migrations;

namespace StudentSystem.Persistence
{
    public class DbConfig
    {
        public static void RegisterDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemDbContext, Configuration>());
        }
    }
}