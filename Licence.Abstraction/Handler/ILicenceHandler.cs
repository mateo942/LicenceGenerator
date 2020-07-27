using System;
using System.Threading.Tasks;

namespace Licence.Abstraction.Handler
{
    public interface ILicenceHandler
    {
        Task<string> Export(Model.ILicenceData licenceData, Guid keyId);
        Task<Model.ILicenceData> Valid(string deviceId, string data, Guid keyId);
        Model.ILicenceData Valid(string deviceId, string data, string publicKey);
    }
}
