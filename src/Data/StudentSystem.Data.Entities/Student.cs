using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentSystem.Data.Entities
{
    //public class Student : IdentityUser
    //{
    //    public Student()
    //    {
    //        Courses = new HashSet<Course>();
    //    }

    //    public virtual ICollection<Course> Courses { get; set; }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Student> manager)
    //    {
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

    //        return userIdentity;
    //    }
    //}

    //TODO immutable class
    public class Student
    {
        public Student()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
