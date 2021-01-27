using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data
{
    public interface IAuthRepository
	{
		IdentityUser GetUserById(string id);
		IdentityUser GetUserByUsername(string username);
		bool ValidatePassword(string username, string plainTextPassword);
	}
}
