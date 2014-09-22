using Nancy;
using Nancy.Security;

namespace FirstOneTo.Authentication.Service
{
    public interface IAuthenticationService
    {
        IUser GetUser(string userName, string password);
        string Tokenize(IUser userIdentity, NancyContext nancyContext);
        IUserIdentity Detokenize(NancyContext nancyContext);
        void AddUser(string userName, string password);
        IUser GetUser(string userName);
        bool IsPasswordStrongerThanMedium(string password);
    }
}