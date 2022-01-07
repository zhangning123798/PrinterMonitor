using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcAsp.Net.PrinterMonitor;

using System.Management;
using System.Drawing.Printing;
using System.Printing;
using Newtonsoft.Json;

namespace PrinterMonitor.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rd = new Random();
            GetPrintersWinSever();
            PrinterQueueMonitor printerMonitor = new PrinterQueueMonitor();

            printerMonitor.OnPrinterStatusChange += (o, e) => {
                Console.WriteLine("打印状态变更：");
                Console.WriteLine(o);
                Console.WriteLine(JsonConvert.SerializeObject(e));
            };
            printerMonitor.OnJobStatusChange += (o, e) =>
            {
                var bianhao = rd.Next();
                Console.WriteLine("任务状态变更："+ bianhao);
                Console.WriteLine(o);
                Console.WriteLine(JsonConvert.SerializeObject(e));
                Console.WriteLine(e.JobID + "." + e.JobName + "." + e.JobStatus + "." + e.JobTotalPages);
            };
            printerMonitor.Start();
            Console.ReadKey();
        }

        private static List<string> GetPrintersWinSever()
        {
            var server = new LocalPrintServer();

            var queues = server.GetPrintQueues();
            PrintDocument prtdoc = new PrintDocument();
            List<string> lstPrinterName = new List<string>();
            foreach (string strprinter in PrinterSettings.InstalledPrinters)
            {
                lstPrinterName.Add(strprinter);
                PrintServer printServer = new PrintServer(strprinter);
                var ss = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections }).First();
            }
            return lstPrinterName;
        }

        //public static int Main(string[] args)
        //{
        //    // Create event query to be notified within 1 second of
        //    // a change in a service
        //    string query =
        //        "SELECT * FROM __InstanceCreationEvent "
        //        + "WITHIN 1 WHERE " +
        //        "TargetInstance isa \"Win32_Process\"";
        //
        //    // Initialize an event watcher and subscribe to events
        //    // that match this query
        //    ManagementEventWatcher watcher =
        //        new ManagementEventWatcher(
        //        new ManagementScope("root\\CIMV2"),
        //        new EventQuery(query));
        //
        //    // times out watcher.WaitForNextEvent in 5 seconds
        //    watcher.Options.Timeout = new TimeSpan(0, 0, 10);
        //
        //    // Block until the next event occurs
        //    // Note: this can be done in a loop if waiting for
        //    //        more than one occurrence
        //    Console.WriteLine(
        //        "Open an application (notepad.exe) to trigger an event.");
        //    ManagementBaseObject e = watcher.WaitForNextEvent();
        //
        //    //Display information from the event
        //    Console.WriteLine(
        //        "Process {0} has been created, path is: {1}",
        //        ((ManagementBaseObject)e
        //        ["TargetInstance"])["Name"],
        //        ((ManagementBaseObject)e
        //        ["TargetInstance"])["ExecutablePath"]);
        //
        //    //Cancel the subscription
        //    watcher.Stop();
        //    
        //    return 0;
        //}

    }
}
