using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadingLibrary
{
    public static class Utility
    {
        private static int? _physicalCoreCount;
        public static int PhysicalCoreCount
        {
            get
            {
                if (_physicalCoreCount == null)
                {
                    int coreCount = 0;
                    foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                    {
                        coreCount += int.Parse(item["NumberOfCores"].ToString());
                    }

                    _physicalCoreCount = coreCount;
                }

                return (int)_physicalCoreCount;
            }
        }

        private static int? _logicalCoreCount;
        public static int LogicalCoreCount
        {
            get
            {
                if (_logicalCoreCount == null)
                {
                    _logicalCoreCount = Environment.ProcessorCount;
                }

                return (int)_logicalCoreCount;
            }
        }
    }
}
