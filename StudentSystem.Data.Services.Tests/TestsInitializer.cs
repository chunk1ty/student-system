//using System.Data.Entity;
//using System.Data.Entity.Migrations;

//using NUnit.Framework;

//namespace StudentSystem.Data.Services.Tests
//{
//    [SetUpFixture]
//    public class TestsInitializer
//    {
//        [OneTimeSetUp]
//        public void AssemblyInit(TestContext context)
//        {
//            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemDbContext, TestDbConfiguration>());
//        }
//    }

//    public sealed class TestDbConfiguration : DbMigrationsConfiguration<StudentSystemDbContext>
//    {
//        public TestDbConfiguration()
//        {
//            AutomaticMigrationsEnabled = true;
//            AutomaticMigrationDataLossAllowed = true;
//        }
//    }
//}
