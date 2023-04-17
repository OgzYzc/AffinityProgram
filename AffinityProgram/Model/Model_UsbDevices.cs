using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffinityProgram.Model
{
    internal class Model_UsbDevices
    {
        public Model_UsbDevices(string UsbDeviceID)
        {
            if (!UsbDeviceID.Contains("SWD"))
            {
                if (UsbDeviceID.Contains("PCI"))
                {
                    this.DeviceID = UsbDeviceID.Substring(59).Trim(new char[] { (char)34 }).Replace(@"\\", @"\") ?? throw new ArgumentNullException(nameof(UsbDeviceID));

                }
                else
                {
                    this.DeviceID = UsbDeviceID.Substring(54).Trim(new char[] { (char)34 }).Replace(@"\\", @"\") ?? throw new ArgumentNullException(nameof(UsbDeviceID));
                }
            }
        }
        public string DeviceID { get; }
    }
}
