using ChaoticCupid.Service.Contracts;
using ChaoticCupid.Service.Models;
using System;
using System.Threading.Tasks;

namespace ChaoticCupid.Client
{
    public class CupidCallbackHandler : ICupidCallback
    {
        public void ReceiveLetter(Person from, string message, bool showPhone)
        {
            Task.Run(() =>
            {
                Console.WriteLine("\nYou received a letter!");
                Console.WriteLine($"   From: {from.Username}");
                Console.WriteLine($"   City: {from.City}");
                Console.WriteLine($"   Age: {from.Age}");

                if (showPhone)
                    Console.WriteLine($"   Phone: {from.Phone}");

                Console.WriteLine($"   Message: {message}");
                Console.WriteLine("\nPress Enter to confirm you received this letter...");
            });
        }
    }
}