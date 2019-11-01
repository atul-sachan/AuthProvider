using AuthProvider.Authentication.DataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.Models
{
    public class IdentityUserClaim 
    {
        public IdentityUserClaim() { }
        public IdentityUserClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
        public void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
