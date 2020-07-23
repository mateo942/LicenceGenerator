using System;
namespace Licence.Abstraction.Model
{
    public class EncryptionSettings
    {
        private EncryptionSettings() { }

        public EncryptionSettings(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public static EncryptionSettings Default
            => new EncryptionSettings();

        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }
    }
}
