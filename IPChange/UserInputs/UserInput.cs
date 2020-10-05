using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace IPChange
{
    public class UserInput
    {
        public static string log = string.Empty;

        /// <summary>
        /// We see if the user wants to set DHCP (1) or Static (2)
        /// </summary>
        /// <returns> Either DHCP or Static depending on the user selection. </returns>
        public static Mode GetMode()
        {
            // Set the mode to invlid until set to a valid option.
            var mode = Mode.Invalid;
            // Used to break the do loop.
            bool userSelectionValid;
            do
            {
                // Ask the user what mode they want
                Console.WriteLine("Do you want to set DHCP or Static?");
                Console.WriteLine("1 = DHCP");
                Console.WriteLine("2 = Static");
                //Get the user selection.
                var userSelection = Console.ReadKey();
                // If it is either 1 or 2 we can continue.
                switch (userSelection.Key)
                {
                    // DHCP
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        mode = Mode.DHCP;
                        userSelectionValid = true;
                        break;
                    // Static
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        mode = Mode.Static;
                        userSelectionValid = true;
                        break;
                    //Error state, repromt user until we get 1-5.
                    default:
                        Log.LogMessageError($" Sorry - {userSelection.Key} is not a correct selection, please try again.");
                        userSelectionValid = false;
                        break;
                }
            } while (!userSelectionValid);
            Log.LogMessage($"Mode selected: {mode}");
            return mode;
        }

        /// <summary>
        /// We provide a list of Network Interfaces to the user and ask for which one they want changed.
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface GetNetwork()
        {
            // A list of network interfaces ont his device.
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces().ToArray();
            // The count for the index of network interfaces.
            var count = 0;
            // Only true if user has selected a correct key.
            bool userSelectionValid = false;
            // We set the networkSelected to Lookback Device.
            NetworkInterface networkSelected = networkInterfaces.Where(w => w.Name.Contains("Loopback")).FirstOrDefault();
            int userSelectionNumber = 0;

            do
            {
                // We reset the count incase this is not the first loop (user entered in wrong data)
                count = 0;
                // Ask the user what intrface...
                Console.WriteLine($"Which one would you like to assign the address to?");
                // We show them a list of network interfaces...
                foreach (var network in networkInterfaces)
                {
                    Console.WriteLine($"{count}: {network.Name}");
                    count++;
                }
                Console.WriteLine();
                // Get the selection.
                var userSelection = Console.ReadKey(true);
                if (!char.IsNumber(userSelection.KeyChar))
                    // Error state
                    continue;
                // We know the user selected a number.
                userSelectionNumber = int.Parse(userSelection.KeyChar.ToString());
                // The user selected number is not in the list of interfaces.
                if (count <= userSelectionNumber)
                {
                    Log.LogMessageError($"Sorry you selected {userSelectionNumber} and this is not a valid interface. Try again.");
                    continue;
                }
                // We got a network selected and can return
                networkSelected = networkInterfaces[userSelectionNumber];
                // We break from the do while loop
                userSelectionValid = true;
            } while (!userSelectionValid);
            // We show the use the network they selected
            Log.LogMessage($"Selected network: {networkSelected.Name}");
            return networkSelected;
        }

        /// <summary>
        /// A method to get a valid ip from the user.
        /// </summary>
        /// <param name="type"> This is for the user messsage, if you want the user to enter in Gateway type should be "Gateway"</param>
        /// <returns>The IP address from the user.</returns>
        public static string GetIp(StaticTypes type)
        {
            // Onlt true if the IP address is valid.
            bool userSelectionValid;
            // the IP the user enters and will be returned if correct.
            string userIp = string.Empty;
            do
            {
                // We ask the user for input
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(Environment.NewLine + type + Environment.NewLine);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Enter in an address, select from this list");
                if (type != StaticTypes.IP)
                    Console.WriteLine("or press enter to skip:");
                // We write to console a list of AppSettings with a 0-99 for user section.
                Predefined.OutputList();
                Console.WriteLine();
                // Get user input...
                var userInput = Console.ReadLine();
                // Only for Subnet, Gateway or DNS...
                if (string.IsNullOrWhiteSpace(userInput) && type != StaticTypes.IP)
                    switch (type)
                    {
                        case StaticTypes.Subnet:
                            userIp = "255.255.255.0";
                            break;
                        case StaticTypes.Gateway:
                            userIp = "192.168.0.1";
                            break;
                        case StaticTypes.DNS:
                            userIp = "192.168.0.1";
                            break;
                    }
                // We can only accept 0 - 99, otherwise it'll be a full IP address (e.g. 192.168.0.1)
                else if (userInput.Length < 2)
                // We get it from the list
                {
                    userIp = Predefined.GetPredefinedOrEmpty(userInput);
                }
                // Otherwise it isn't a predefined ip so we use the user's entered data
                else
                {
                    userIp = userInput;
                }
                // We check if it is valid and return if it is.
                userSelectionValid = IP.CheckIpIsValid(userIp, type);
                if (!userSelectionValid)
                    // The user entered in something wrong, we will reset and try again.
                    Log.LogMessageError($"Sorry you entered: {userIp} and this is not a valid for {type}. Try again.");
                // The user made a right entry or selected from the list.
            } while (!userSelectionValid);
            // We tell the use rhwat they selected and move on.
            Log.LogMessage($"{type} has been set as: {userIp}");
            return userIp;
        }
    }
}