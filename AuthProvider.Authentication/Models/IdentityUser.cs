using AuthProvider.Authentication.DataAccess;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.Models
{
    public class IdentityUser : IdentityUser<string>, IEntity
    {
        public IdentityUser()
        {
            Roles = new List<string>();
            Logins = new List<IdentityUserLogin>();
            Claims = new List<IdentityUserClaim>();
            Tokens = new List<IdentityUserToken>();
        }

        [BsonIgnoreIfNull]
        public virtual List<string> Roles { get; set; }
        [BsonIgnoreIfNull]
        public virtual List<IdentityUserLogin> Logins { get; set; }
        [BsonIgnoreIfNull]
        public virtual List<IdentityUserClaim> Claims { get; set; }
        [BsonIgnoreIfNull]
        public virtual List<IdentityUserToken> Tokens { get; set; }

        #region IdentityHelper
        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }
        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        public virtual void AddLogin(UserLoginInfo login)
        {
            Logins.Add(new IdentityUserLogin(login));
        }
        public virtual void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
        }

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new IdentityUserClaim(claim));
        }
        public virtual void RemoveClaim(Claim claim)
        {
            Claims.RemoveAll(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }
        public virtual void ReplaceClaim(Claim existingClaim, Claim newClaim)
        {
            var claimExists = Claims
                .Any(c => c.ClaimType == existingClaim.Type && c.ClaimValue == existingClaim.Value);
            if (!claimExists)
            {
                // note: nothing to update, ignore, no need to throw
                return;
            }
            RemoveClaim(existingClaim);
            AddClaim(newClaim);
        }

        private IdentityUserToken GetToken(string loginProider, string name) => Tokens.FirstOrDefault(t => t.LoginProvider == loginProider && t.Name == name);
        public virtual void SetToken(string loginProider, string name, string value)
        {
            var existingToken = GetToken(loginProider, name);
            if (existingToken != null)
            {
                existingToken.Value = value;
                return;
            }

            Tokens.Add(new IdentityUserToken
            {
                LoginProvider = loginProider,
                Name = name,
                Value = value
            });
        }
        public virtual string GetTokenValue(string loginProider, string name)
        {
            return GetToken(loginProider, name)?.Value;
        }
        public virtual void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        #endregion
    }
}
