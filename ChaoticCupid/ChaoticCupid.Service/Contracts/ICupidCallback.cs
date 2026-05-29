using ChaoticCupid.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ChaoticCupid.Service.Contracts
{
    [ServiceContract]
    public interface ICupidCallback
    {
        [OperationContract(IsOneWay = true)]    // client does not wait for a response from the service
        void ReceiveLetter(Person from, string message, bool showPhone);
    }
}