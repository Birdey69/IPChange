using System;
using System.Net.NetworkInformation;

namespace IPChange
{
    internal class Program
    {
        public static bool Sucess { get; set; }

        private static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Welcome to the IP Change tool. {Environment.NewLine}");
            Console.ForegroundColor = ConsoleColor.White;

            do
            {
                // We get either DHCP or Static from the user.
                Mode mode = UserInput.GetMode();
                // We get the network to change from the user.
                NetworkInterface network = UserInput.GetNetwork();

                switch (mode)
                {
                    case Mode.DHCP:
                        DHCP.ChangeIpDhcp(network);
                        Sucess = true;
                        break;

                    case Mode.Static:
                        // We get the IP from the user.
                        string ip = UserInput.GetIp(StaticTypes.IP);
                        // We get the Subnet from the user.
                        string subnet = UserInput.GetIp(StaticTypes.Subnet);
                        // We get the Subnet from the user.
                        string gateway = UserInput.GetIp(StaticTypes.Gateway);
                        // We get the Subnet from the user.
                        string dns = UserInput.GetIp(StaticTypes.DNS);
                        // We change the selected network's address
                        Static.ChangeIP(network, ip, subnet, gateway, dns);
                        Sucess = true;
                        break;

                    default:
                        Log.LogMessageError($" Sorry something went wrong, please try again.");
                        Sucess = false;
                        break;
                }
            } while (!Sucess);

            Console.WriteLine("Thank you for using my program - Shane Bird 2020");
            Console.ReadLine();
        }
    }
}