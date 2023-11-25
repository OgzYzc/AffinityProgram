using AffinityProgram.Find_Core;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_Set
{
    internal class Controller_SetUsbAffinity
    {
        public Controller_SetUsbAffinity()
        {
            bool continueProgram = CPPCError();
            if (continueProgram)
            {
                Console.Clear();
                try
                {
                    Model_RegistryPath registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                    List<Model_UsbDevices> devices = GetUsbDevices();

                    RegistrySecurity regSecurity = CreateRegistrySecurity();



                    foreach (Model_UsbDevices device in devices)
                    {
                        if (string.IsNullOrEmpty(device.DeviceID))
                            continue;

                        string keyPath = registryPath.RegistryPath.Replace("$i", device.DeviceID);


                        using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            if (Find_Core_CPPC.USBhexBytes == null)
                            {
                                AddAffintyNoCPPC(key);
                            }
                            else
                            {
                                key.SetValue("AssignmentSetOverride", Find_Core_CPPC.USBhexBytes, RegistryValueKind.Binary);
                                key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
                                Console.WriteLine("Affinity added.");
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }

            else
                return;
        }
        private List<Model_UsbDevices> GetUsbDevices()
        {
            Query_UsbDevices deviceInfo = new Query_UsbDevices();
            return deviceInfo.GetDevices<Model_UsbDevices>();
        }


        private RegistrySecurity CreateRegistrySecurity()
        {
            RegistrySecurity regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }
        private static void AddAffintyNoCPPC(RegistryKey key)
        {
            byte[] assignmentSetOverrideValue = View.MainMenu.isSmtEnabled ? new byte[] { 00, 04 } : new byte[] { 32 };

            key.SetValue("AssignmentSetOverride", assignmentSetOverrideValue, RegistryValueKind.Binary);
            key.SetValue("DevicePolicy", "4", RegistryValueKind.DWord);
            Console.WriteLine("Usb affinity added.");
        }

        private bool CPPCError()
        {

            Console.WriteLine("You are adding affinity without using CPPC. " +
                              "If you enabled CPPC, go back to the menu and press 'Find best core' then come back." +
                              " Or you can add predetermined affinity. Press Enter for adding Predetermined affinity.");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
