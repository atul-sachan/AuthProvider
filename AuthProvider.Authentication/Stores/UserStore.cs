using AuthProvider.Authentication.DataAccess;
using AuthProvider.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityUser = AuthProvider.Authentication.Models.IdentityUser;

namespace AuthProvider.Authentication.Stores
{
    public class UserStore : 
        IUserStore<IdentityUser>, 
        IUserPasswordStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserLockoutStore<IdentityUser>,
        IUserSecurityStampStore<IdentityUser>,
        IQueryableUserStore<IdentityUser>,
        IUserLoginStore<IdentityUser>,
        IUserAuthenticationTokenStore<IdentityUser>,
        //IUserAuthenticatorKeyStore<IdentityUser>,
        IUserTwoFactorStore<IdentityUser> //,
        //IUserTwoFactorRecoveryCodeStore<IdentityUser>,
        
    {
        private readonly IRepository<IdentityUser> repository;

        public UserStore(IRepository<IdentityUser> repository)
        {
            this.repository = repository;
        }


        #region IUserStore
        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            await this.repository.InsertAsync(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            await this.repository.DeleteAsync(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await this.repository.GetAsync(userId);
        }

        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await this.repository.FirstAsync(x => x.NormalizedUserName == normalizedUserName);
        }

        public async Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.NormalizedUserName);
        }

        public async Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Id);
        }

        public async Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            await this.repository.UpdateAsync(user); 
            return IdentityResult.Success;
        }
        #endregion

        #region IUserPasswordStore
        public async Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }

        public  Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserEmailStore
        public async Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await this.repository.FirstAsync(x => x.NormalizedEmail == normalizedEmail);
        }

        public async Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.NormalizedEmail);
        }

        public Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserPhoneNumberStore
        public Task<string> GetPhoneNumberAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserRoleStore
        public async Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            user.AddRole(roleName);
            await Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Roles);
        }

        public async Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await this.repository.FindAsync(u => u.Roles.Contains(roleName));
        }

        public async Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Roles.Contains(roleName));
        }

        public async Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            user.RemoveRole(roleName);
            await Task.CompletedTask;
        }
        #endregion

        #region IUserLockoutStore
        public async Task<int> GetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnd);
        }

        public async Task<int> IncrementAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;
            return await Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserSecurityStampStore
        public Task<string> GetSecurityStampAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (stamp == null)
            {
                throw new ArgumentNullException(nameof(stamp));
            }
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserTwoFactorStore
        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }
        #endregion

        #region IUserClaimStore
        public async Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                user.AddClaim(claim);
            }
            await Task.CompletedTask;
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Claims.Select(c => c.ToClaim()).ToList());
        }

        public async Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return await this.repository.FindAsync(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value));
        }

        public async Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                user.RemoveClaim(claim);
            }
            await Task.CompletedTask;
        }

        public async Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            user.ReplaceClaim(claim, newClaim);
            await Task.CompletedTask;
        }
        #endregion

        #region IUserLoginStore
        public async Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.AddLogin(login);
            await Task.CompletedTask;
        }

        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return await this.repository.FirstAsync(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Logins.Select(l => l.ToUserLoginInfo()).ToList());
        }

        public async Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.RemoveLogin(loginProvider, providerKey);
            await Task.CompletedTask;
        }
        #endregion

        #region IQueryableUserStore
        public IQueryable<IdentityUser> Users => this.repository.FindAll().AsQueryable();
        #endregion

        #region IUserAuthenticationTokenStore
        public async Task<string> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.GetTokenValue(loginProvider, name));
        }

        public async Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.RemoveToken(loginProvider, name);
            await Task.CompletedTask;
        }

        public async Task SetTokenAsync(IdentityUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            user.SetToken(loginProvider, name, value);
            await Task.CompletedTask;
        }
        #endregion

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        
    }
}
