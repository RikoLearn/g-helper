﻿using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace GHelper
{
    public static class OptimizationService
    {
        public static void SetChargeLimit (int newValue)
        {
            // Set the path to the .ini file
            string path = @"C:\ProgramData\ASUS\ASUS System Control Interface\ASUSOptimization\Customization.ini";


            // Make a backup copy of the INI file
            string backupPath = path + ".bak";
            File.Copy(path, backupPath, true);

            string fileContents = File.ReadAllText(path, Encoding.Unicode);

            // Find the section [BatteryHealthCharging]
            string sectionPattern = @"\[BatteryHealthCharging\]\s*(version=\d+)?\s+value=(\d+)";
            Match sectionMatch = Regex.Match(fileContents, sectionPattern);
            if (sectionMatch.Success)
            {
                // Replace the value with the new value
                string oldValueString = sectionMatch.Groups[2].Value;
                int oldValue = int.Parse(oldValueString);
                string newSection = sectionMatch.Value.Replace($"value={oldValue}", $"value={newValue}");

                // Replace the section in the file contents
                fileContents = fileContents.Replace(sectionMatch.Value, newSection);

                File.WriteAllText(path, fileContents, Encoding.Unicode);
            }
        }

        public static bool IsRunning()
        {
            return (Process.GetProcessesByName("AsusOptimization").Count() > 0);
        }


        public static void SetBacklightOffDelay(int value = 60)
        {
            try
            {
                RegistryKey myKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ASUS\ASUS System Control Interface\AsusOptimization\ASUS Keyboard Hotkeys", true);
                if (myKey != null)
                {
                    myKey.SetValue("TurnOffKeybdLight", value, RegistryValueKind.DWord);
                    myKey.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
        }


    }

}