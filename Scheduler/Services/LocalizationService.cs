using System.Diagnostics;
using System.Globalization;

namespace Scheduler.Services
{
    public class LocalizationService
    {
        public static string GetUserLocale()
        {
            var userLocale = CultureInfo.CurrentCulture.Name;
            const string defaultLocale = "en-US";
            var supportedLocales = new HashSet<string> { "en-US", "es-ES", "fr-FR" }; // Supported locales
            
            userLocale = supportedLocales.Contains(userLocale) ? userLocale : defaultLocale;
            Debug.Print($"Locale: {userLocale}");
            // return "es-ES"; // testing purposes to ensure the resource file is being used
            return supportedLocales.Contains(userLocale) ? userLocale : defaultLocale;
        }
    }
}