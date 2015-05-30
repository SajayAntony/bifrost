using System;
using System.Diagnostics;
using Web.App_Start;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {            
            const string baseAddress = "http://localhost:8080";
            OwinSelfhostStartup.Startup(baseAddress);
            Console.WriteLine("Sample Usage:");            
            //curl --request PUT 'http://localhost:8080/resource' -H "Content-Type:application/json" -H "Accept: application/json" --data "{name:'Bridge.Commands.Hostname'}"
            Console.WriteLine("curl --request PUT 'http://localhost:8080/resource' -H \"Content-Type:application/json\" -H \"Accept: application/json\" --data \"{name:'Bridge.Commands.Hostname'}\"");
            Console.ReadLine();
        }
    }
}
