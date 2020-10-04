using System;
using System.Configuration;

namespace IPChange
{
    public static class Predefined
    {
        /// <summary>
        /// Outputs to the console a list of Predefined IP addresses from the App.Config file.
        /// </summary>
        public static void OutputList()
        {
            // Set the count for the user selectio number
            var count = 0;
            foreach (var setting in ConfigurationManager.AppSettings.AllKeys)
            {
                // We write a list of the values and their key names
                Console.WriteLine($"" +
                    $"{count}: {ConfigurationManager.AppSettings[setting]} - " +
                    $"({ConfigurationManager.AppSettings.GetKey(count)})");
                // Increase the count for every entry
                count++;
            }
        }

        /// <summary>
        /// We get the ip from app.config using the User's input selection.
        /// </summary>
        /// <param name="userInput">what the user entered for the list selection</param>
        /// <returns></returns>
        public static string GetPredefinedOrEmpty(string userInput)
        {
            if (int.TryParse(userInput, out int selection))
            {
                if (ConfigurationManager.AppSettings.Count >= selection)
                {
                    return ConfigurationManager.AppSettings[selection];
                }
            }
            else Log.LogMessageError("You entered a letter, try entering a number instead next time!");
            // The user didn't select from the list, must be a entered value. e.g. 192.168.1.1
            return string.Empty;
        }
    }
}