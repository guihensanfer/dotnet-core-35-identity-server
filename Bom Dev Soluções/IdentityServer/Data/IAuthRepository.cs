using IdentityServer4.Test;

namespace IdentityServer.Data
{
    public interface IAuthRepository
	{
		TestUser GetUserById(string id);
		TestUser GetUserByUsername(string username);
		bool ValidatePassword(string username, string plainTextPassword);
	}
}
