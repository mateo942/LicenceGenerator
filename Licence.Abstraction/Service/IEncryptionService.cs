using System;
namespace Licence.Abstraction.Service
{
    public interface IEncryptionService
    {
        string GenerateKey();
        string Encrypt(string data);
        string Decrypt(string data);
    }
}
