using Microsoft.AspNetCore.Identity;
using AuthProvider.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityRole = AuthProvider.Authentication.Models.IdentityRole;
using AuthProvider.Authentication.DataAccess;

namespace AuthProvider.Authentication.Stores
{
    public class RoleStore : IRoleStore<IdentityRole>, IQueryableRoleStore<IdentityRole>
    {
        private readonly IRepository<IdentityRole> repository;
        public RoleStore(IRepository<IdentityRole> repository)
        {
            this.repository = repository;
        }

        public IQueryable<IdentityRole> Roles => this.repository.FindAll().AsQueryable();

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            await this.repository.InsertAsync(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            await this.repository.DeleteAsync(role);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await this.repository.GetAsync(roleId);
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await this.repository.FirstAsync(x => x.NormalizedName == normalizedRoleName);
        }

        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.NormalizedName);
        }

        public async Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Id);
        }

        public async Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Name);
        }

        public async Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            await Task.CompletedTask;
        }

        public async Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            await Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            await this.repository.UpdateAsync(role);
            return IdentityResult.Success;
        }
    }
}
