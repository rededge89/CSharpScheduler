using System.Text.RegularExpressions;

namespace Scheduler.Utilities
{
    public static class ValidationUtility
    {
        private static readonly Regex PhoneNumberRegex = new Regex(@"^[0-9-]+$");

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return PhoneNumberRegex.IsMatch(phoneNumber);
        }

        public static bool IsNonEmptyTrimmed(string input)
        {
            return !string.IsNullOrWhiteSpace(input?.Trim());
        }
    }
}