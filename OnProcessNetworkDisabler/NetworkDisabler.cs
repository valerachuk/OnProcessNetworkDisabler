using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OnProcessNetworkDisabler
{
    static class NetworkDisabler
    {
        private static List<string> _disabled = new List<string>();

        private static List<string> GetAllEnabledAdapters()
        {
            string netshShowResult = Netsh.GetInterfaces();

            var regex = new Regex(@"^Enabled(\s+\w+){2}\s+(?<value>\b.+?)\r$", RegexOptions.Compiled | RegexOptions.Multiline);

            var result = new List<string>();

            foreach (Match match in regex.Matches(netshShowResult))
            {
                result.Add(match.Groups["value"].Value);
            }

            return result;
        }

        public static int Disable()
        {
            _disabled = GetAllEnabledAdapters();
            Netsh.SetState(false, _disabled.ToArray());
            return _disabled.Count;
        }

        public static int Enable()
        {
            Netsh.SetState(true, _disabled.ToArray());
            int enabledCount = _disabled.Count();
            _disabled.Clear();
            return enabledCount;
        }

    }
}
