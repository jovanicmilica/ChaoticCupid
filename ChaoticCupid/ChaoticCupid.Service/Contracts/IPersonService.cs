using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ChaoticCupid.Service.Contracts
{
    [ServiceContract(CallbackContract = typeof(ICupidCallback))]    
    public interface IPersonService
    {
        [OperationContract] // client waits for a response from the service
        bool InitSinglePerson(string username, string city, int age, string phone); 

        [OperationContract]
        bool BlockUser(string username, string userToBlock);

        [OperationContract]
        void ConfirmLetterReceived(string username);
    }
}