using System;
namespace Licence.Abstraction.Model
{
    public interface ILicenceResult
    {
        public string PrivateKey { get; }
        public string PublicKey { get; }

        public string Licence { get; }
    }
}
