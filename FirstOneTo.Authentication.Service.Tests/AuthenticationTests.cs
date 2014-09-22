using System.Linq;
using FirstOneTo.ReadModel.Infrastructure;
using FirstOneTo.ReadModel.Infrastructure.InMemory;
using FluentAssertions;
using Nancy;
using Nancy.Authentication.Token;
using NUnit.Framework;

namespace FirstOneTo.Authentication.Service.Tests
{
    [TestFixture]
    public class AuthenticationTests
    {
        [SetUp]
        public void SetUp()
        {
            _userDatabase = new InMemoryUserDatabase();
            _nancyContext = new NancyContext();
            _tokenizer = new Tokenizer(cfg =>
                cfg.AdditionalItems(
                    ctx =>
                        ctx.Request.Headers["X-Custom-Header"].FirstOrDefault(),
                    ctx => ctx.Request.Headers.UserAgent
                    ));
            _authenticationService = new AuthenticationService(_tokenizer, _userDatabase);
        }

        private IReadModelUserDatabase _userDatabase;
        private IAuthenticationService _authenticationService;
        private ITokenizer _tokenizer;
        private NancyContext _nancyContext;

        [Test]
        public void can_add_user()
        {
            string userName = "Tom";
            string password = "LittleBalls";
            _authenticationService.AddUser(userName, password);

            IUser user = _authenticationService.GetUser(userName, password);
            user.UserName.Should().Be(userName);
        }

        [Test]
        public void should_return_user()
        {
            string userName = "demo";
            string password = "demo";
            _authenticationService.AddUser(userName, password);

            IUser user = _authenticationService.GetUser("demo", "demo");
            user.UserName.Should().Be("demo");
        }
    }
}