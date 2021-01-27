using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace IdentityServer.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(ApplicationDbContext context, ILogger<AuthRepository> logger)
        {
            db = context;
            _logger = logger;
        }

        public IdentityUser GetUserById(string id)
        {
            _logger.LogInformation("teste!!!!!");
            var user = db.Users.Where(u => u.Id == id).FirstOrDefault();

            return user;
        }

        public IdentityUser GetUserByUsername(string username)
        {
            _logger.LogInformation("teste!!!!!");
            var user = db.Users.Where(u => string.Equals(u.UserName, username)).FirstOrDefault();

            return user;
        }

        public bool ValidatePassword(string username, string plainTextPassword)
        {
            _logger.LogInformation("teste!!!!!");
            System.IO.File.WriteAllText(@"C:\Users\guihe\Desktop\teste.txt", "baguio é loco");

            var user = db.Users.Where(u => string.Equals(u.UserName, username)).FirstOrDefault();
            if (user == null) return false;
            if (string.Equals(plainTextPassword, user.PasswordHash)) return true;

            return false;
        }
    }
}
