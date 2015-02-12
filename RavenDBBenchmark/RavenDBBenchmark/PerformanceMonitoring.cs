using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
namespace RavenDBBenchmark
{
    public class PerformanceMonitoring
    {
        public PerformanceMonitoring(string userName = "", string password ="",string ravenDBRootDisk = "_Total")
        {
            /*oConn.Username = @"Corax\hr";
            oConn.Password = "Rhinos";*/
             connectionOptions = new ConnectionOptions(){Username = userName, Password = password};
             ManagementScope = new ManagementScope(@"\root\cimv2", connectionOptions);
        }
        public void MonitorDiskSpace()
        {
            var oConn = new ConnectionOptions();
            
            ManagementScope oMs = new ManagementScope(@"\root\cimv2", oConn);
           // ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE Name = 'Raven.Server'");
            //Win32_PerfFormattedData_PerfOS_Processor.Name='_Total'
            ObjectQuery cpuPercentQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name = '_Total'");
            ManagementObjectSearcher cpuPercentSearcher = new ManagementObjectSearcher(oMs, cpuPercentQuery);
            ManagementObjectCollection cpuPercentReturnCollection = cpuPercentSearcher.Get();
            foreach (var oReturn in cpuPercentReturnCollection) 
            {
                var cpuUssage = oReturn["PercentProcessorTime"];
            }
            ObjectQuery memoryQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObjectSearcher memorySearcher = new ManagementObjectSearcher(oMs, memoryQuery);
            ManagementObjectCollection memoryReturnCollection = memorySearcher.Get();
            foreach (var oReturn in memoryReturnCollection)
            {
                var availableBytes = oReturn["AvailableBytes"];
            }

           /* ObjectQuery networkQuery = new ObjectQuery("SELECT * FROM Win32_PerfRawData_Tcpip_NetworkInterface");
            ManagementObjectSearcher networkSearcher = new ManagementObjectSearcher(oMs, networkQuery);
            ManagementObjectCollection networkReturnCollection = networkSearcher.Get();
            foreach (var oReturn in networkReturnCollection)
            {
                var totalTcpIpBytes = oReturn["CurrentBandwidth"];
               /* BytesReceivedPersec --> How much data has come in on an adapter
                BytesSentPersec --> How much data your adapter has sent
                BytesTotalPersec --> Total bandwidth sent and received.#1#
            }*/

            ObjectQuery diskQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfDisk_LogicalDiskz WHERE Name = '_Total'");
            ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher(oMs, diskQuery);
            ManagementObjectCollection diskReturnCollection = diskSearcher.Get();
            foreach (var oReturn in diskReturnCollection)
            {
                var availableBytes = oReturn["DiskBytesPerSec"];
            }
            /*uint64 DiskBytesPerSec;
            uint64 DiskReadBytesPerSec;
            uint32 DiskReadsPerSec;
            uint32 DiskTransfersPerSec;
            uint64 DiskWriteBytesPerSec;
            uint32 DiskWritesPerSec;*/
        }

        protected ConnectionOptions connectionOptions { get; private set; }
        protected ManagementScope ManagementScope { get; private set; }
    }
}
