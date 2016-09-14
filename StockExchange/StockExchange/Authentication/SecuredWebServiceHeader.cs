using System;

namespace StockExchange.Authentication
{
    /// <summary>
    /// Soap Header for the Secured Web Service.
    /// Username and Password are required for AuthenticateUser(),
    ///   and AuthenticatedToken is required for everything else.
    /// </summary>
    public class SecuredWebServiceHeader : System.Web.Services.Protocols.SoapHeader
    {
        public Guid UserId;
        public string Username;
        public string Password;
        public string AuthenticatedToken;
    }
}