using AffinityProgram.Controller.Concrete;
using AffinityProgram.Controller.Controller_SetNicPowershell;
using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

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
                    Console.WriteLine("Registry key added successfully.");
                    
                }
                Console.WriteLine("Executing powershell.");
                Thread.Sleep(3000);
                Console.Clear();
                //I don't know why, I don't know how but here we are.
                Controller.Controller_SetNicPowershell.Controller_SetNicPowershell controller_SetNicPowershell = new Controller_SetNicPowershell.Controller_SetNicPowershell();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
    
}
