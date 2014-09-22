using System.Collections.Generic;
using FirstOneTo.ReadModel.Infrastructure;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Security;

namespace FirstOneTo.Authentication.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenizer _tokenizer;
        private readonly IReadModelUserDatabase _userDatabase;

        public AuthenticationService(ITokenizer tokenizer, IReadModelUserDatabase userDatabase)
        {
            _tokenizer = tokenizer;
            _userDatabase = userDatabase;
        }

        /// <summary>
        ///     Get rid of the stupid logic in this method
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void AddUser(string userName, string password)
        {
            //todo get rid of this logic it is because the claims havent been implemented properly
            _userDatabase.AddUser(userName, password,
                userName != "nonadmin" ? new List<string> {userName, "admin"} : new List<string> {userName, "nonadmin"});
        }


        public IUser GetUser(string userName, string password)
        {
            return _userDatabase.GetUser(userName, password);
        }

        public IUser GetUser(string userName)
        {
            return _userDatabase.GetUser(userName);
        }


        public string Tokenize(IUser userIdentity, NancyContext nancyContext)
        {
            return _tokenizer.Tokenize(userIdentity, nancyContext);
        }

        public IUserIdentity Detokenize(NancyContext nancyContext)
        {
            string token = nancyContext.Request.Headers.Authorization;
            if (token.Contains("Token "))
            {
                token = token.Substring(6);
            }
            return _tokenizer.Detokenize(token, nancyContext);
        }

        public bool IsPasswordStrongerThanMedium(string password)
        {
            PasswordScore passwordStrengthScore = PasswordAdvisor.CheckStrength(password);

            switch (passwordStrengthScore)
            {
                case PasswordScore.Blank:
                case PasswordScore.VeryWeak:
                case PasswordScore.Weak:
                    return false;
            }

            return true;
        }
    }
}