using System.Net;
using System.Text.RegularExpressions;

namespace IPChange
{
    public static class IP
    {
        public static bool CheckIpIsValid(string userIp, StaticTypes type)
        {
            // Reg ex for IP not Subnets
            var regex = new Regex(@"^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");

            if (type == StaticTypes.Subnet)
            {
                regex = new Regex(@"^(((255\.){3}(255|254|252|248|240|224|192|128|0+))|((255\.){2}(255|254|252|248|240|224|192|128|0+)\.0)|((255\.)(255|254|252|248|240|224|192|128|0+)(\.0+){2})|((255|254|252|248|240|224|192|128|0+)(\.0+){3}))$");
            }
            // First check
            if (regex.IsMatch(userIp))
            {
                // Second check
                return IPAddress.TryParse(userIp, out _);
            }
            // Error state
            return false;
        }
    }
}