using System.Collections.Generic;
using System.Linq;
using FirstOneTo.Authentication;

namespace FirstOneTo.ReadModel.Infrastructure.InMemory
{
    public class InMemoryUserDatabase : IReadModelUserDatabase
    {
        private readonly List<UserIdentity> _userIdentities = new List<UserIdentity>();


        public IUser GetUser(string userName, string password)
        {
            UserIdentity user = _userIdentities.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public IUser GetUser(string userName)
        {
            UserIdentity user = _userIdentities.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public void AddUser(string userName, string password, List<string> claims )
        {
            if (userName != "nonadmin")
            {
                _userIdentities.Add(new UserIdentity
                {
                    UserName = userName,
                    Password = password,
                    Claims = claims,
                });
            }
            else
            {
                _userIdentities.Add(new UserIdentity
                {
                    UserName = userName,
                    Password = password,
                    Claims = claims,
                });
            }
        }
    }
}