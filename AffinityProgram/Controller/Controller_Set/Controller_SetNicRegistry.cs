using AffinityProgram.Controller.Concrete;
using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AffinityProgram.Controller.Controller_SetNicRegistry
{
    internal class Controller_SetNicRegistry
    {
        public Controller_SetNicRegistry()
        {
            try
            {
                //Adding RssBaseCpu to Ndis service
                var concreteRegistryPath = new Concrete_RegistryPath();
                string registryPath = concreteRegistryPath.NdisRegistryPath;

                var regSecurity = new RegistrySecurity();
                regSecurity.AddAccessRule(new RegistryAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), RegistryRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                using (var key = Registry.LocalMachine.CreateSubKey(registryPath, RegistryKeyPermissionCheck.ReadWriteSubTree, regSecurity))
                {
                    key.SetValue("RssBaseCpu", "2", RegistryValueKind.DWord);
                    Console.WriteLine("Registry key added successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
