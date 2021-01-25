using IdentityServer4.Test;
using System.Linq;

namespace IdentityServer.Data
{
    public class AuthRepository : IAuthRepository
	{
		private readonly ApplicationDbContext db;

		public AuthRepository(ApplicationDbContext context)
		{
			db = context;
		}
		
        public TestUser GetUserById(string id)
        {
			var user = db.AspNetUsers.Where(u => u.SubjectId == id).FirstOrDefault();

			return user;
		}

		public TestUser GetUserByUsername(string username)
        {
			var user = db.AspNetUsers.Where(u => string.Equals(u.Username, username)).FirstOrDefault();

			return user;
		}

        public bool ValidatePassword(string username, string plainTextPassword)
        {
			var user = db.AspNetUsers.Where(u => string.Equals(u.Username, username)).FirstOrDefault();
			if (user == null) return false;
			if (string.Equals(plainTextPassword, user.Password)) return true;			

			return false;
		}        
	}
}
