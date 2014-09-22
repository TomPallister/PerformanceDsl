using System.Collections.Generic;

namespace FirstOneTo.Authentication
{
    public class UserIdentity : IUser
    {
        public UserIdentity()
        {
        }

        public UserIdentity(
            string userName,
            string password,
            IEnumerable<string> claims)
        {
            Password = password;
            UserName = userName;
            Claims = claims;
        }

        public string Password { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}