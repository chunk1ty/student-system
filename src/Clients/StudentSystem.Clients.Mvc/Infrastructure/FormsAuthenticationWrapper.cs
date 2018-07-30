using System;
using System.Web.Security;

namespace StudentSystem.Clients.Mvc.Infrastructure
{
    public class FormsAuthenticationWrapper
    {
        public virtual void SetAuthCookie(string email, bool rememberMe)
        {
            if (email == null)
            {
                throw new ArgumentNullException();
            }

            FormsAuthentication.SetAuthCookie(email, rememberMe);
        }

        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}