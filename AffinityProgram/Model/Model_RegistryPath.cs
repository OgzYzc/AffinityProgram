using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Model
{
    internal class Model_RegistryPath
    {
        public string RegistryPath { get; set; }
        public Model_RegistryPath(string registryPath)
        {
            RegistryPath = registryPath;
        }
    }
}
