using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
	{
		IAuthRepository _rep;
		private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

		public ResourceOwnerPasswordValidator(IAuthRepository rep, ILogger<ResourceOwnerPasswordValidator> logger)
		{
			_rep = rep;
			_logger = logger;
		}

		public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
		{
			_logger.LogInformation("Teste");
			System.IO.File.WriteAllText(@"C:\Users\guihe\Desktop\teste.txt", "Teste");

			if (_rep.ValidatePassword(context.UserName, context.Password))
			{
				context.Result = new GrantValidationResult(_rep.GetUserByUsername(context.UserName).Id, "password", null, "local", null);
				return Task.FromResult(context.Result);
			}
			context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The username and password do not match", null);
			return Task.FromResult(context.Result);
		}
	}
}
