using System.Data.Entity;
using StudentSystem.Data.Migrations;

namespace StudentSystem.Data
{
    public class DbConfig
    {
        public static void RegisterDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemDbContext, Configuration>());
        }
    }
}