using ChaoticCupid.Service.Models;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace ChaoticCupid.Service.Services
{
    public static class MatchMaker
    {
        public static Person FindBestMatch(Person receiver, ConcurrentDictionary<string, Person> persons)
        {
            Person bestMatch = null;
            int bestScore = -1;

            foreach (var entry in persons)
            {
                var candidate = entry.Value;

                // Skip self
                if (candidate.Username == receiver.Username)
                    continue;

                // Skip blocked users 
                lock (receiver.BlockedUsers)
                {
                    if (receiver.BlockedUsers.Contains(candidate.Username))
                        continue;
                }

                int score = CalculateScore(receiver, candidate);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = candidate;
                }
            }

            return bestMatch;
        }

        private static int CalculateScore(Person receiver, Person candidate)
        {
            int score = 0;

            if (candidate.City == receiver.City)
                score += 30;

            if (Math.Abs(candidate.Age - receiver.Age) <= 2)
                score += 20;

            score += RandomFactor();

            return score;
        }

        private static int RandomFactor()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                return (int)(BitConverter.ToUInt32(bytes, 0) % 101);
            }
        }

        public static string GetRandomMessage()
        {
            string[] messages = {
                "Looking forward to our meeting!",
                "I would like to get to know you.",
                "I am not interested in meeting."
            };

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int index = (int)(BitConverter.ToUInt32(bytes, 0) % (uint)messages.Length);
                return messages[index];
            }
        }
    }
}