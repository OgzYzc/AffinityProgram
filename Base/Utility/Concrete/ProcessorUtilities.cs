using Base.Model;
using Base.Utility.Abstract;
using System.Runtime.InteropServices;

namespace Base.Utility.Concrete;
public class ProcessorUtilities : IProcessorUtilityService
{

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcessorCore
    {
        public byte Flags;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct SystemLogicalProcessorInformationUnion
    {
        [FieldOffset(0)]
        public ProcessorCore ProcessorCore;
        [FieldOffset(2)]
        private ulong Reserved;
    }

    private enum LogicalProcessorRelationship
    {
        RelationProcessorCore,
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SystemLogicalProcessorInformation
    {
        public nuint ProcessorMask;
        public LogicalProcessorRelationship Relationship;
        public SystemLogicalProcessorInformationUnion ProcessorInformation;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetLogicalProcessorInformation(nint Buffer, ref uint ReturnLength);

    private const int ERROR_INSUFFICIENT_BUFFER = 122;

    public ProcessorUtilities()
    {
        ProcessorModel.ProcessorInfoModel processorModel = new();

    }

    public ProcessorModel.ProcessorInfoModel GetProcessorInformation()
    {
        uint returnLength = 0;
        GetLogicalProcessorInformation(nint.Zero, ref returnLength);
        if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
        {
            nint ptr = Marshal.AllocHGlobal((int)returnLength);
            try
            {
                if (!GetLogicalProcessorInformation(ptr, ref returnLength))
                {
                    throw new System.ComponentModel.Win32Exception();
                }

                int offset = 0;
                int physicalProcessorCount = 0;
                int logicalProcessorCount = Environment.ProcessorCount;

                while (offset < returnLength)
                {
                    var info = Marshal.PtrToStructure<SystemLogicalProcessorInformation>(nint.Add(ptr, offset));

                    if (info.Relationship == LogicalProcessorRelationship.RelationProcessorCore)
                    {
                        physicalProcessorCount++;
                    }

                    offset += Marshal.SizeOf<SystemLogicalProcessorInformation>();
                }
                return new ProcessorModel.ProcessorInfoModel
                {
                    PhysicalProcessorCount = physicalProcessorCount,
                    LogicalProcessorCount = logicalProcessorCount,
                    IsSMTEnabled = physicalProcessorCount < logicalProcessorCount

                };
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
        else
        {
            throw new InvalidOperationException("Failed to retrieve processor information.");
        }
    }
}