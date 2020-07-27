using System;
namespace Licence.Abstraction.Model
{
    public class SignatureSettings
    {
        private SignatureSettings() { }

        public SignatureSettings(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public static SignatureSettings Default
            => new SignatureSettings();

        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }
    }
}
