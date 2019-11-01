using AuthProvider.Authentication.DataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.Models
{
    public class IdentityRole : IdentityRole<string>, IEntity
    {
    }
}
