using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Utility;
public interface ICommandLineUtilityService
{
    string StartCMD(string command, bool captureOutput);
}
