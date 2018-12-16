using System.Data.Entity;

using StudentSystem.Data.Contracts;
using StudentSystem.Domain;

namespace StudentSystem.Data
{
    public class StudentSystemDbContext : DbContext, IUnitOfWork
    {
        public StudentSystemDbContext()
            : base("StudentSystemConnection")
        {
        }

        public virtual IDbSet<Course> Courses { get; set; }

        public virtual IDbSet<Student> Students { get; set; }

        public new IDbSet<TEntity> Set<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Commit()
        {
            SaveChanges();
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students").HasKey(x => x.Id);
            modelBuilder.Entity<Student>().Property(x => x.Email).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Student>().Property(x => x.Password).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<Course>().ToTable("Courses").HasKey(x => x.Id);
            modelBuilder.Entity<Course>().Property(x => x.Name).HasMaxLength(50).IsRequired();

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
