using ChaoticCupid.Service;
using System;
using System.ServiceModel;

namespace ChaoticCupid.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(CupidService)))
            {
                host.Open();
                Console.WriteLine("Cupid service started on net.tcp://localhost:8080/CupidService");
                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
        }
    }
}