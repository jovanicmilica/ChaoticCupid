using ChaoticCupid.Service.Contracts;
using ChaoticCupid.Service.Models;
using ChaoticCupid.Service.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticCupid.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CupidService : IPersonService
    {
        private readonly ConcurrentDictionary<string, Person> _persons =
            new ConcurrentDictionary<string, Person>();

        private readonly Timer _timer;

        public CupidService()
        {
            //_timer = new Timer(_ => SendLetters().GetAwaiter().GetResult(), null,   // send letters every minute
            //    TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

                _timer = new Timer(_ => SendLetters().GetAwaiter().GetResult(), null,   // testing
                    TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(6));
        }

        public bool InitSinglePerson(string username, string city, int age, string phone)
        {
            if (!ValidationHelper.ValidateUser(username, city, age, phone))
                return false;

            if (_persons.ContainsKey(username))
            {
                Console.WriteLine($"User '{username}' already exists!");
                return false;
            }

            var callback = OperationContext.Current.GetCallbackChannel<ICupidCallback>();

            var person = new Person
            {
                Username = username,
                City = city,
                Age = age,
                Phone = phone,
                Callback = callback,
                BlockedUsers = new HashSet<string>(),
                LetterSemaphore = new SemaphoreSlim(1, 1)
            };

            if (_persons.TryAdd(username, person))
            {
                return true;
            }

            Console.WriteLine($"Error registering user '{username}'!");
            return false;
        }

        private async Task SendLetters()
        {
            foreach (var entry in _persons)
            {
                var receiver = entry.Value;

                if (!await receiver.LetterSemaphore.WaitAsync(0))   // if they are already processing a letter, skip them
                    continue;

                try
                {
                    var sender = MatchMaker.FindBestMatch(receiver, _persons);

                    if (sender == null)
                    {
                        receiver.LetterSemaphore.Release();
                        continue;
                    }

                    string message = MatchMaker.GetRandomMessage();
                    bool showPhone = !message.Contains("I am not interested");

                    receiver.Callback.ReceiveLetter(sender, message, showPhone);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send letter to {receiver.Username}: {ex.Message}");
                }
            }
        }

        public bool BlockUser(string username, string userToBlock)
        {
            if (!_persons.TryGetValue(username, out var person))
            {
                Console.WriteLine($"User '{username}' not found!");
                return false;
            }

            if (!_persons.ContainsKey(userToBlock))
            {
                Console.WriteLine($"User '{userToBlock}' not found!");
                return false;
            }

            if (username == userToBlock)
            {
                Console.WriteLine("You cannot block yourself!");
                return false;
            }

            lock (person.BlockedUsers)
            {
                if (person.BlockedUsers.Add(userToBlock))
                {
                    Console.WriteLine($"'{username}' blocked '{userToBlock}'");
                    return true;
                }

                Console.WriteLine($"'{userToBlock}' is already blocked!");
                return false;
            }
        }

        public void ConfirmLetterReceived(string username)
        {
            if (_persons.TryGetValue(username, out var person))
            {
                person.LetterSemaphore.Release();
                Console.WriteLine($"User '{username}' confirmed letter received.");
            }
        }
    }
}