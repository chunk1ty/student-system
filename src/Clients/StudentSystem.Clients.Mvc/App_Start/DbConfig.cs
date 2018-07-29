using System.Data.Entity;

using StudentSystem.Data;
using StudentSystem.Data.Migrations;

namespace StudentSystem.Clients.Mvc
{
    public class DbConfig
    {
        public static void RegisterDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemDbContext, Configuration>());
        }
    }
}