using Scheduler.Data;

namespace Scheduler.Services
{
    public class AuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthenticationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Login(string username, string password)
        {
            LoggerService.LoginActivityLogModel currentLoginAttempt = new LoggerService.LoginActivityLogModel()
            {
                AttemptedUsername = username,
                LoginTime = DateTime.Now.ToUniversalTime(),
                Locale = LocalizationService.GetUserLocale()
            };

            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                LoggerService.LogLoginAttempt(currentLoginAttempt);
                throw new Exception("User not found.");
            }

            currentLoginAttempt.RetrievedUsername = user.UserName;

            if (password == user.Password)
            {
                currentLoginAttempt.Success = true;
                LoggerService.LogLoginAttempt(currentLoginAttempt);
                return user.UserId;
            }

            LoggerService.LogLoginAttempt(currentLoginAttempt);
            throw new Exception("Invalid credentials.");
        }

        public void Logout()
        {
            Console.WriteLine("User logged out successfully.");
        }
    }
}