using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace IPChange
{
    public static class Static
    {
        // Setup for all Netsh commands.
        private static readonly ProcessStartInfo psi = new ProcessStartInfo("netsh.exe")
        {
            WindowStyle = ProcessWindowStyle.Minimized,
            Verb = "runas"
        };

        public static void ChangeIP(NetworkInterface network, string ip, string subnet, string gateway, string dns)
        {
            psi.Arguments = "interface ip set address \"" + network.Name + "\" static " + ip + " " + subnet + " " + gateway + " " + dns;
            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Log.LogMessageError("There was an issue with the netsh command, here is some more details:");
                Log.LogMessageError(ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(dns))
            {
                // Dns
                psi.Arguments = "interface ip set dns \"" + network.Name + "\" static " + dns;
                try
                {
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    Log.LogMessageError("There was an issue with the netsh command, here is some more details:");
                    Log.LogMessageError(ex.Message);
                }
            }
        }
    }
}