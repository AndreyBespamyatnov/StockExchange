using System;
using StockExchange.WebClient.StockExchangeServiceReference;

namespace StockExchange.WebClient
{
    public sealed class AuthenticationContainer
    {
        private static readonly Lazy<AuthenticationContainer> Lazy = new Lazy<AuthenticationContainer>(() => new AuthenticationContainer());
        public SecuredWebServiceHeader Header { get; private set; }

        private readonly StockExchangeServiceSoapClient _client;

        public static AuthenticationContainer Instance { get { return Lazy.Value; } }

        public string UserName
        {
            get
            {
                return Header != null ? Header.Username : string.Empty;
            }
        }

        public bool IsAuthenticated {
            get { return Header != null && !string.IsNullOrWhiteSpace(Header.AuthenticatedToken); }
        }

        public Guid UserId {
            get
            {
                return _client.GetCurrentUserId(Header);
            }
        }

        private AuthenticationContainer()
        {
            _client = new StockExchangeServiceSoapClient();
        }

        public bool Authenticate(string email, string password)
        {
            Header = new SecuredWebServiceHeader
            {
                Username = email,
                Password = password
            };
            Header.AuthenticatedToken = _client.AuthenticateUser(Header);

            return !string.IsNullOrWhiteSpace(Header.AuthenticatedToken);
        }

        public void Register(string email, string password)
        {
            var registerUserModel = new RegisterUserModel
            {
                UserEmail = email,
                UserName = email,
                UserPassword = password,
                UserAnswer = email, // TODO user real value
                UserQuestion = email + email // TODO user real value
            };

            var status = _client.CreateUser(registerUserModel);

            switch (status)
            {
                case MembershipCreateStatus.Success:
                    return;

                case MembershipCreateStatus.InvalidUserName:
                case MembershipCreateStatus.InvalidPassword:
                case MembershipCreateStatus.InvalidQuestion:
                case MembershipCreateStatus.InvalidAnswer:
                case MembershipCreateStatus.InvalidEmail:
                case MembershipCreateStatus.DuplicateUserName:
                case MembershipCreateStatus.DuplicateEmail:
                case MembershipCreateStatus.UserRejected:
                case MembershipCreateStatus.InvalidProviderUserKey:
                case MembershipCreateStatus.DuplicateProviderUserKey:
                case MembershipCreateStatus.ProviderError:
                    throw new Exception(status.ToString());
            }
        }

        public void LogOff()
        {
            Header = new SecuredWebServiceHeader();
        }
    }
}