using ChaoticCupid.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Web;

namespace ChaoticCupid.Service.Models
{
    [DataContract]  // mark as serializable for WCF
    public class Person
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [IgnoreDataMember]  // dont serialize the callback
        public ICupidCallback Callback { get; set; }

        [IgnoreDataMember]
        public HashSet<string> BlockedUsers { get; set; } = new HashSet<string>();

        [IgnoreDataMember]
        public SemaphoreSlim LetterSemaphore { get; set; } = new SemaphoreSlim(1, 1);   // limit to 1 letter at a time
    }
}