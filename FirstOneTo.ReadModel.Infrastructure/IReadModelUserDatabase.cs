using System.Collections.Generic;
using FirstOneTo.Authentication;

namespace FirstOneTo.ReadModel.Infrastructure
{
    public interface IReadModelUserDatabase
    {
        IUser GetUser(string userName, string password);
        void AddUser(string userName, string password, List<string> claims);
        IUser GetUser(string userName);
    }
}