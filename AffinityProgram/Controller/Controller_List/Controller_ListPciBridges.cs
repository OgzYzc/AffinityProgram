using AffinityProgram.Model;
using AffinityProgram.Queries.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AffinityProgram.Controller.Controller_Set.Controller_SetAffinity
{
    internal class Controller_ListPciBridges
    {
        private const int CR_SUCCESS = 0;

        public static List<Model_PciBridgeDevices> PciBridgeList = new List<Model_PciBridgeDevices>();

        //Using Windows Configuration Manager for this because i can not get parent ids from wmi
        [DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
        private static extern int CM_Locate_DevNode(out IntPtr pdnDevInst, string pDeviceID, int ulFlags);

        [DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
        private static extern int CM_Get_Device_ID(IntPtr dnDevInst, StringBuilder buffer, int bufferLen, int ulFlags);

        [DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
        private static extern int CM_Get_Parent(out IntPtr pdnDevInst, IntPtr dnDevInst, int ulFlags);

        private static string deviceInfoFromModel;
        public void SetPciBridgeAffinity()
        {
            deviceInfoFromModel = GetDeviceInfoFromModel();

            if (CM_Locate_DevNode(out IntPtr rootNode, deviceInfoFromModel, 0) != CR_SUCCESS)
            {
                Console.WriteLine("Failed to locate root node.");
                return;
            }

            TraverseParentDevices(rootNode);
        }

        private static string GetDeviceInfoFromModel()
        {
            string deviceId = null;
            Query_PciDevices deviceInfo = new Query_PciDevices();
            List<Model_PciDevices> devices = deviceInfo.GetDevices<Model_PciDevices>();

            foreach (Model_PciDevices dev in devices)
            {
                deviceId = dev.DeviceID.ToString();
            }

            return deviceId;
        }

        private static string GetDeviceId(IntPtr deviceInstance)
        {
            StringBuilder deviceId = new StringBuilder();
            deviceId.Append(deviceInfoFromModel);

            CM_Get_Device_ID(deviceInstance, deviceId, 1024, 0);
            return deviceId.ToString();
        }

        private static void TraverseParentDevices(IntPtr deviceInstance)
        {
            while (CM_Get_Parent(out deviceInstance, deviceInstance, 0) == CR_SUCCESS)
            {
                string parentId = GetDeviceId(deviceInstance);

                if (!parentId.StartsWith("PCI"))
                    break;

                Model_PciBridgeDevices pciBridgeDevice = new Model_PciBridgeDevices(parentId);
                PciBridgeList.Add(pciBridgeDevice);
            }
        }
    }
}
