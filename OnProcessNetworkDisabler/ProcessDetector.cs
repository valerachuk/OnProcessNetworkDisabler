using System.Diagnostics;
using System.Linq;
namespace OnProcessNetworkDisabler
{
    static class ProcessDetector
    {
        public static int IntersectionsCount(params string[] processNames)
        {
            return Process.GetProcesses().Select(x => x.ProcessName).Count(processNames.Contains);
        }
    }
}
