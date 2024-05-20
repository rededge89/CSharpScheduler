namespace Scheduler.Services
{
    public class LoggerService
    {
        public class LoginActivityLogModel
        {
            public string AttemptedUsername { get; set; }
            public string? RetrievedUsername { get; set; }
            public DateTime LoginTime { get; set; }
            public bool Success { get; set; }
            public string Locale { get; set; }
        }

        public static void LogLoginAttempt(LoginActivityLogModel log)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Login_History.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)); // Ensure the directory exists
            Console.WriteLine($"Login attempt logged to log file at {logFilePath}");
            string logEntry =
                $"{DateTime.Now} UTC: {log.AttemptedUsername} attempted to log in. Retrieved username: {log.RetrievedUsername}. Success: {log.Success}. Locale: {log.Locale}";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}