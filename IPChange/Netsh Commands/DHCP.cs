using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace IPChange
{
    public static class DHCP
    {
        private static ProcessStartInfo psi = new ProcessStartInfo("netsh.exe")
        {
            WindowStyle = ProcessWindowStyle.Minimized,
            Verb = "runas",
        };

        /// <summary>
        /// We change the network to DHCP
        /// </summary>
        /// <param name="network"> The network you want to change to DHCP. </param>
        /// <returns> If sucessfull we set the exit message and code and return either true is sucessfull or false if errors occured.</returns>
        public static bool ChangeIpDhcp(NetworkInterface network)
        {
            try
            {
                // IP
                psi.Arguments = "interface ip set address \"" + network.Name + "\" dhcp";
                Process.Start(psi);
                // DNS
                psi.Arguments = "interface ip set dns \"" + network.Name + "\" dhcp";
                Process.Start(psi);

                Log.LogMessage($"Sucessfuly changed: {network.Name} to DHCP");
                return true;
            }
            catch (Exception ex)
            {
                Log.LogMessageError("There was an issue with the netsh command, here is some more details:");
                Log.LogMessageError(ex.Message);
                return false;
            }
        }
    }
}