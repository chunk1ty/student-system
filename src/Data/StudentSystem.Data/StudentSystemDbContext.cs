using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data
{
    public class StudentSystemDbContext : IdentityDbContext<Student>, IUnitOfWork
    {
        public StudentSystemDbContext()
            : base("DefaultConnection", false)
        {
        }

        public virtual IDbSet<Course> Courses { get; set; }

        public static StudentSystemDbContext Create()
        {
            return new StudentSystemDbContext();
        }

        public new IDbSet<TEntity> Set<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Commit()
        {
            SaveChanges();
        }

        //TODO remove onother tables
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>()
                        .Ignore(x => x.EmailConfirmed)
                        .Ignore(x => x.PhoneNumber)
                        .Ignore(x => x.PhoneNumberConfirmed)
                        .Ignore(x => x.TwoFactorEnabled)
                        .Ignore(x => x.LockoutEndDateUtc)
                        .Ignore(x => x.LockoutEnabled)
                        .Ignore(x => x.AccessFailedCount);

            modelBuilder.Entity<Course>().HasKey(x => x.Id);

            modelBuilder.Entity<Student>()
                        .HasMany(s => s.Courses)
                        .WithMany(c => c.Students)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("StudentId");
                            cs.MapRightKey("CourseId");
                            cs.ToTable("StudentCourses");
                        });
        }
    }
}
