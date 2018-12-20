using System.Data.Entity;

using StudentSystem.Persistence.Contracts;
using StudentSystem.Persistence.Entities;

namespace StudentSystem.Persistence
{
    public class StudentSystemDbContext : DbContext, IUnitOfWork
    {
        public StudentSystemDbContext()
            : base("StudentSystemConnection")
        {
        }

        public virtual IDbSet<CourseEntity> Courses { get; set; }

        public virtual IDbSet<StudentEntity> Students { get; set; }

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
            modelBuilder.Entity<StudentEntity>().ToTable("Students").HasKey(x => x.Id);
            modelBuilder.Entity<StudentEntity>().Property(x => x.Email).HasMaxLength(100).IsRequired();
            //modelBuilder.Entity<Student>().Property(x => x.Password).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<CourseEntity>().ToTable("Courses").HasKey(x => x.Id);
            modelBuilder.Entity<CourseEntity>().Property(x => x.Name).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<StudentEntity>()
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
