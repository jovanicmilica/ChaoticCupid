using ChaoticCupid.Service.Contracts;
using System;
using System.ServiceModel;
using System.Threading;

namespace ChaoticCupid.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var callback = new CupidCallbackHandler();  
            var context = new InstanceContext(callback);
            var proxy = new DuplexChannelFactory<IPersonService>(context,   // Create a duplex channel to the service
                new NetTcpBinding(),        // Use NetTcpBinding for communication
                new EndpointAddress("net.tcp://localhost:8080/CupidService")).CreateChannel();  // Specify the service endpoint address

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter city: ");
            string city = Console.ReadLine();

            Console.Write("Enter age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Enter phone: ");
            string phone = Console.ReadLine();

            bool registered = proxy.InitSinglePerson(username, city, age, phone);

            if (!registered)
            {
                Console.WriteLine("Registration failed. Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Registered successfully! Waiting for letters...");
            Console.WriteLine("Type /block <username> to block someone.");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.StartsWith("/block "))
                {
                    string userToBlock = input.Substring(7);
                    proxy.BlockUser(username, userToBlock);
                }
                else if (input == "")
                {
                    proxy.ConfirmLetterReceived(username);
                }
            }
        }
    }
}