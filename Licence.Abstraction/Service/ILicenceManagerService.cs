using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Licence.Abstraction.Service
{
    public interface ILicenceManagerService
    {
        event EventHandler NotifiChanged;

        Task RequestLicence(IEnumerable<string> modules, IEnumerable<string> aditionalInfos);

        Task<string> GetPublicKey();
        Task SavePublicKey(string publicKey);

        Task<string> GetLicence();
        Task SaveLicence(string licence);
    }
}
