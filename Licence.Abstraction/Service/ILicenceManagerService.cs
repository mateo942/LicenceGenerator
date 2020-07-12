using System;
using System.Threading.Tasks;

namespace Licence.Abstraction.Service
{
    public interface ILicenceManagerService
    {
        event EventHandler NotifiChanged;

        Task<string> GetPublicKey();
        Task SavePublicKey(string publicKey);

        Task<string> GetLicence();
        Task SaveLicence(string licence);
    }
}
