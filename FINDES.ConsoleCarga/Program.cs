using FINDES.ConsoleCarga.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace FINDES.ConsoleCarga
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ConsoleHelper.Log("Process iniciated.");
            try
            {
                //Load.Execute();
                Load.DateToLocalTime();
            }
            catch (Exception ex)
            {
                ConsoleHelper.Log($"ERROR: {ex.Message}");
                ConsoleHelper.Log($"STACK: {ex.StackTrace}");
            }
            Console.WriteLine("");
            Console.WriteLine("Process terminated.");
            //Console.ReadLine();
        }
    }
}
