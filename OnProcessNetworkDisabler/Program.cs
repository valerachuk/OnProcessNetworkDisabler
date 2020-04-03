using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnProcessNetworkDisabler
{
    class Program
    {
        private static int _loopDelay = 2000;
        private static bool _showNotif = false;
        private static List<string> _processList = new List<string>();

        private static void ShowMsg(string text, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            if (_showNotif)
            {
                Task.Run(() => MessageBox.Show(text, caption, buttons, icon));
            }
        }

        private static void LoadConfig()
        {
            string cfgContent = File.ReadAllText("pnd.cfg");
            _showNotif = cfgContent.Contains("msgBoxes");

            var loopTimeoutMatch = Regex.Match(cfgContent, @"delay:\s+(?<delayms>\d+)");
            if (loopTimeoutMatch.Success)
            {
                _loopDelay = int.Parse(loopTimeoutMatch.Groups["delayms"].Value);
            }

            var processMatch = Regex.Match(cfgContent, @"proc:(?:\s+(\w+))+");
            if (processMatch.Success)
            {
                foreach (Capture capture in processMatch.Groups[1].Captures)
                {
                    _processList.Add(capture.Value);
                }
            }
            else
            {
                throw new ArgumentException("You have to write processes in pnd.cfg");
            }
        }

        static void Main()
        {
            bool lastState = false;

            if (ProcessDetector.IntersectionsCount(Process.GetCurrentProcess().ProcessName) > 1)
            {
                MessageBox.Show("Already running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                LoadConfig();

                while (true)
                {
                    bool currentState = ProcessDetector.IntersectionsCount(_processList.ToArray()) > 0;

                    if (currentState && !lastState)
                    {
                        int disabled = NetworkDisabler.Disable();
                        ShowMsg($"Disabled {disabled} interfaces", "Message");
                    }
                    else if (!currentState && lastState)
                    {
                        int enabled = NetworkDisabler.Enable();
                        ShowMsg($"Enabled {enabled} interfaces", "Message");
                    }

                    lastState = currentState;
                    Thread.Sleep(_loopDelay);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
