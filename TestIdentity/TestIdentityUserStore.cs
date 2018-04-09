using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace TestIdentity
{
    public class TestIdentityUserStore : IUserStore<TestIdentityUser>, IUserPasswordStore<TestIdentityUser>
    {
        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(
                "Server=localhost;Database=TestIdentity;" +
                "Trusted_Connection=True;MultipleActiveResultSets=true;" +
                "User ID=SA;password=Abdlatol97;integrated security=false"
            );
            connection.Open();
            return connection;
        } 
        public void Dispose()
        {
//            throw new System.NotImplementedException();
        }

        public Task SetPasswordHashAsync(TestIdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TestIdentityUser user, CancellationToken caCancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

       
        public async  Task<IdentityResult> CreateAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "insert into TestUsers" +
                    "([Id] , [UserName], [NormalizedUserName]" +
                    "[PasswordHash])" +
                    "Values (@id,@userName,@normalizedUserName,@passwordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    }
                );
            }
            return  IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update TestUsers" +
                    "set [Id] = @id" +
                    "[UserName] = @userName" +
                    "[NormalizedUserName] = @normalizedUserName" +
                    "[PasswordHash] = @passwordHash",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    }
                );
            }
            return  IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }


        public async Task<TestIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
               return await connection.QueryFirstOrDefaultAsync<TestIdentityUser>(
                    "select * from TestUsers where Id = @id",
                    new {id = userId}
                );
            } 
            
        }

        public  async Task<TestIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<TestIdentityUser>(
                    "select * from TestUsers where NormalizedUserName = @normalizedUserName",
                    new {id = normalizedUserName}
                );
            }  
        }

        public Task<string> GetUserIdAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }
        
        public Task<string> GetUserNameAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetNormalizedUserNameAsync(TestIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetUserNameAsync(TestIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }
        
        public Task SetNormalizedUserNameAsync(TestIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
     }
}