using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    public class ProfileService : IProfileService
	{
		private IAuthRepository _repository;

		public ProfileService(IAuthRepository rep)
		{
			this._repository = rep;
		}

		public Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			try
			{
				//_logger.LogInformation("Teste");
				System.IO.File.WriteAllText(@"C:\Users\guihe\Desktop\teste.txt", "Teste");

				var subjectId = context.Subject.GetSubjectId();
				var user = _repository.GetUserById(subjectId);

				var claims = new List<Claim>
				{
					new Claim(JwtClaimTypes.Subject, user.Id)
                    
                };

				context.IssuedClaims = claims;
				return Task.FromResult(0);
			}
			catch 
			{
				return Task.FromResult(0);
			}
		}

		public Task IsActiveAsync(IsActiveContext context)
		{
			//_logger.LogInformation("Teste");
			System.IO.File.WriteAllText(@"C:\Users\guihe\Desktop\teste.txt", "Teste");

			var user = _repository.GetUserById(context.Subject.GetSubjectId());
			context.IsActive = (user != null) && user.EmailConfirmed;

			return Task.FromResult(0);
		}
	}
}
