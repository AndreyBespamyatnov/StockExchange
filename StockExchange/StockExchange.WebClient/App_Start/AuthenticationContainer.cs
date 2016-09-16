using System;
using System.Web;
using StockExchange.WebClient.StockExchangeServiceReference;

namespace StockExchange.WebClient
{
    public sealed class AuthenticationContainer
    {
        private static readonly Lazy<AuthenticationContainer> Lazy = new Lazy<AuthenticationContainer>(() => new AuthenticationContainer());

        private readonly StockExchangeServiceSoapClient _client;

        public static AuthenticationContainer Instance { get { return Lazy.Value; } }

        public bool IsAuthenticated
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
                if (cookie != null)
                {
                    string token = cookie[Constants.UserTokenKeyName];
                    return _client.IsUserValid(new SecuredWebServiceHeader { AuthenticatedToken = token });
                }

                return false;
            }
        }

        public Guid UserId {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
                if (cookie != null)
                {
                    string token = cookie[Constants.UserTokenKeyName];
                    return _client.GetCurrentUserId(new SecuredWebServiceHeader { AuthenticatedToken = token });
                }

                return Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
                if (cookie != null)
                {
                    string userName = cookie[Constants.UserNameKeyName];
                    return userName;
                }

                return string.Empty;
            }
        }

        private AuthenticationContainer()
        {
            _client = new StockExchangeServiceSoapClient();
        }

        public bool Authenticate(string email, string password)
        {
            var header = new SecuredWebServiceHeader
            {
                Username = email,
                Password = password
            };
            var token = _client.AuthenticateUser(header);

            HttpCookie cookie = new HttpCookie(Constants.CookiesName);
            cookie.Expires = DateTime.Now.AddHours(1);
            HttpContext.Current.Response.Cookies.Remove(Constants.CookiesName);
            HttpContext.Current.Response.Cookies.Add(cookie); 

            cookie.Values[Constants.UserTokenKeyName] = token;
            cookie.Values[Constants.UserNameKeyName] = email;

            HttpContext.Current.Response.SetCookie(cookie);

            return !string.IsNullOrWhiteSpace(token);
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
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
            if (cookie != null)
            {
                string token = cookie[Constants.UserTokenKeyName];
                _client.LogOff(new SecuredWebServiceHeader {AuthenticatedToken = token});
            }
        }
    }
}