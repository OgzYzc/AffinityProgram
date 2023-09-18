using AffinityProgram.Controller.Controller_Set.Controller_SetAffinity;
using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Controller_Rem
{
    internal class Controller_RemPciBridgeAffinity
    {
        public Controller_RemPciBridgeAffinity()
        {
            try
            {
                var registryPath = new Model_RegistryPath(@"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy");

                var regSecurity = CreateRegistrySecurity();

                Controller_SetPciBridgeAffinity controller_SetPciBridgeAffinity = new Controller_SetPciBridgeAffinity();
                controller_SetPciBridgeAffinity.SetPciBridgeAffinity();

                foreach (var device in Controller_SetPciBridgeAffinity.PciBridgeList)
                {
                    if (!string.IsNullOrEmpty(device.ToString()))
                    {
                        var keyPath = registryPath.RegistryPath.Replace("$i", device.ToString());
                        using (var key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                        {
                            key.DeleteValue("AssignmentSetOverride");
                            key.DeleteValue("DevicePolicy");
                            Console.WriteLine("Affinity removed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private RegistrySecurity CreateRegistrySecurity()
        {
            var regSecurity = new RegistrySecurity();
            regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            return regSecurity;
        }
    }
}
