using System.Data.Entity;

namespace StudentSystem.Authentication
{
    public class StudentSystemAuthDbContext : DbContext
    {
        public StudentSystemAuthDbContext()
            : base("StudentSystemAuthConnection")
        {
        }

        public virtual IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users")
                                       .HasKey(x => x.Id);

            modelBuilder.Entity<User>().Property(x => x.Email)
                                       .HasMaxLength(100)
                                       .IsRequired();

            modelBuilder.Entity<User>().Property(x => x.Password)
                                       .HasMaxLength(100)
                                       .IsRequired();
        }
    }
}
