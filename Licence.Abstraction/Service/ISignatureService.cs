using System;
namespace Licence.Abstraction.Service
{
    public interface ISignatureService
    {
        /// <summary>
        /// Create Key
        /// </summary>
        /// <returns>Private key, Public key</returns>
        (string, string) GenerateKey();
        /// <summary>
        /// Sign the data
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <returns>Signature</returns>
        string Export(string data);
        string Export(string datam, string key);
        /// <summary>
        /// Valid signed data
        /// </summary>
        /// <param name="data">Signed data</param>
        /// <param name="signature">Check signature</param>
        /// <returns></returns>
        bool Valid(string data, string signature);
        bool Valid(string data, string signature, string key);
    }
}
