using System.Data.Entity;
using NUnit.Framework;

namespace StudentSystem.Data.Services.Tests
{
    [SetUpFixture]
    internal sealed class SetUpFixture
    {
        internal static StudentSystemDbContext Context = new StudentSystemDbContext();

        [OneTimeSetUp]
        public void Initialize()
        {
            if (Context.Database.Exists())
            {
                return;
            }

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentSystemDbContext, TestDbConfiguration>());
            Context.Database.Initialize(false);
        }
    }
}
