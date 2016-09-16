using System;
using System.Web;
using System.Web.Security;
using StockExchange.Extensions;
using StockExchange.Models;

namespace StockExchange.Authentication
{
    public class AuthenticationHelper
    {
        public string AuthenticateUser(SecuredWebServiceHeader soapHeader)
        {
            if (soapHeader == null)
                return "Please provide a Username and Password";
            if (string.IsNullOrEmpty(soapHeader.Username) || string.IsNullOrEmpty(soapHeader.Password))
                return "Please provide a Username and Password";
            // Are the credentials valid?
            if (!IsUserValid(soapHeader.Username, soapHeader.Password))
                return "Invalid Username or Password";

            // Create and store the AuthenticatedToken before returning it
            string token = Guid.NewGuid().ToString();
            HttpRuntime.Cache.Add(
                token,
                soapHeader.Username,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(60),
                System.Web.Caching.CacheItemPriority.NotRemovable,
                null);
            return token;
        }

        public void LogOff(SecuredWebServiceHeader soapHeader)
        {
            if (soapHeader == null)
                return;

            HttpRuntime.Cache.Remove(soapHeader.AuthenticatedToken);
        }

        public bool IsUserValid(SecuredWebServiceHeader soapHeader)
        {
            if (soapHeader == null)
                return false;
            // Does the token exists in our Cache?
            if (!string.IsNullOrEmpty(soapHeader.AuthenticatedToken))
                return (HttpRuntime.Cache[soapHeader.AuthenticatedToken] != null);
            return false;
        }

        public Guid GetCurrentUserId(SecuredWebServiceHeader soapHeader)
        {
            if (soapHeader == null)
            {
                return Guid.Empty;
            }

            // Does the token exists in our Cache?
            if (string.IsNullOrEmpty(soapHeader.AuthenticatedToken) ||
                HttpRuntime.Cache[soapHeader.AuthenticatedToken] == null)
            {
                return Guid.Empty;
            }

            var membershipUser = Membership.GetUser(HttpRuntime.Cache[soapHeader.AuthenticatedToken].ToString());
            if (membershipUser == null || membershipUser.ProviderUserKey == null)
            {
                return Guid.Empty;
            }

            Guid userId;
            return Guid.TryParse(membershipUser.ProviderUserKey.ToString(), out userId) ? userId : Guid.Empty;
        }

        public bool IsUserValid(string username, string password)
        {
            // Ask the SQL Memebership to verify the credentials for us
            return Membership.ValidateUser(username, password);
        }

        public MembershipCreateStatus MembershipCreateUser(RegisterUserModel registerUserModel)
        {
            var status = MembershipCreateStatus.ProviderError;

            try
            {
                const bool isApproved = true; 
                Membership.CreateUser(registerUserModel.UserName, registerUserModel.UserPassword,
                    registerUserModel.UserEmail, registerUserModel.UserQuestion, registerUserModel.UserAnswer, isApproved,
                    out status);
            }
            catch (MembershipCreateUserException ex)
            {
                // TODO Log it
                true.BreakOnTrue(ex.Message);
            }
            catch (Exception ex)
            {
                // TODO Log it
                true.BreakOnTrue(ex.Message);
            }

            return status;
        }
    }
}