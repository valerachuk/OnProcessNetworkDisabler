using System.Diagnostics;

namespace OnProcessNetworkDisabler
{
    static class Netsh
    {
        private static string ExecuteCommand(string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c chcp 437 && {args} && exit";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                return process.StandardOutput.ReadToEnd();
            }
        }

        public static void SetState(bool isEnable, params string[] interfaceNames)
        {
            foreach (var interfaceName in interfaceNames)
            {
                ExecuteCommand($"netsh interface set interface \"{interfaceName}\" {(isEnable ? "en" : "dis")}able");
            }
        }

        public static string GetInterfaces()
        {
            return ExecuteCommand("netsh interface show interface");
        }

    }
}
