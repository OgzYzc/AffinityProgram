using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Controller.Concrete
{
    internal class Concrete_RegistryPath
    {
        public Concrete_RegistryPath() { }
        public string registryPath = @"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\Affinity Policy";
        public string MsiLimitRegistryPath = @"SYSTEM\CurrentControlSet\Enum\$i\Device Parameters\Interrupt Management\MessageSignaledInterruptProperties";

    }
}
