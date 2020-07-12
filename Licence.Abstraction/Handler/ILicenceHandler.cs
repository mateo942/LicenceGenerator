using System;
namespace Licence.Abstraction.Handler
{
    public interface ILicenceHandler
    {
        Model.ILicenceResult Export(Model.ILicenceData licenceData);
        bool Valid(string publicKey, string data, out Model.ILicenceData licence);
        bool Valid(string deviceId, string publicKey, string data, out Model.ILicenceData licence);
    }
}
