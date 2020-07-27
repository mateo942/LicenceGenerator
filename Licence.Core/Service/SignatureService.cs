using System;
using System.Text;
using Licence.Abstraction.Model;
using Licence.Abstraction.Service;
using NSec.Cryptography;

namespace Licence.Core.Service
{
    public class SignatureService : ISignatureService, IDisposable
    {
        private readonly SignatureAlgorithm _algorithm;
        private readonly KeyBlobFormat _privateKeyFormat = KeyBlobFormat.PkixPrivateKeyText;
        private readonly KeyBlobFormat _publicKeyFormat = KeyBlobFormat.PkixPublicKeyText;
        private readonly SignatureSettings _signatureSettings;

        private Key _key;
        private PublicKey _publicKey;

        public SignatureService(SignatureSettings signatureSettings)
        {
            _algorithm = SignatureAlgorithm.Ed25519;
            _signatureSettings = signatureSettings;

            if (!string.IsNullOrEmpty(_signatureSettings.PrivateKey))
            {
                var blob = Convert.FromBase64String(_signatureSettings.PrivateKey);
                _key = Key.Import(_algorithm, blob, _privateKeyFormat);
            }

            if (!string.IsNullOrEmpty(_signatureSettings.PublicKey))
            {
                var blob = Convert.FromBase64String(_signatureSettings.PublicKey);
                _publicKey = PublicKey.Import(_algorithm, blob, _publicKeyFormat);
            }
        }

        public void Dispose()
        {
            if(_key != null)
            {
                _key.Dispose();
                _key = null;
            }
        }

        public string Export(string data)
        {
            if (_key == null)
                throw new InvalidOperationException("Private key has been not intialized");

            return InternalExport(data, _key);
        }
        
        public string Export(string data, string privateKey)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data is empty", nameof(data));
            }

            if (string.IsNullOrEmpty(privateKey))
            {
                throw new ArgumentException("Private key is empty", nameof(privateKey));
            }

            var keyBlob = Convert.FromBase64String(privateKey);
            using (var key = Key.Import(_algorithm, keyBlob, _privateKeyFormat))
            {
                return InternalExport(data, key);
            }
        }

        private string InternalExport(string data, Key key)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            return InternalExport(dataBytes, key);
        }

        private string InternalExport(byte[] data, Key key)
        {
            var signature = _algorithm.Sign(key, data);

            return Convert.ToBase64String(signature);
        }

        public (string, string) GenerateKey()
        {
            var param = new KeyCreationParameters();
            param.ExportPolicy = KeyExportPolicies.AllowPlaintextExport;
            using (var key = Key.Create(_algorithm, param))
            {
                var privateKeyBlob = key.Export(_privateKeyFormat);
                var publicKeyBlob = key.Export(_publicKeyFormat);

                return (Convert.ToBase64String(privateKeyBlob), Convert.ToBase64String(publicKeyBlob));
            }
        }

        public bool Valid(string data, string signature)
        {
            if (_publicKey == null)
                throw new InvalidOperationException("Public key has been not initialized");

            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);

            return InternalValid(dataBytes, signatureBytes, _publicKey);
        }

        public bool Valid(string data, string signature, string publicKey)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data is empty", nameof(data));
            }

            if (string.IsNullOrEmpty(signature))
            {
                throw new ArgumentException("Signature is empty", nameof(signature));
            }

            if (string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentException("Public key is empty", nameof(publicKey));
            }

            var keyBlob = Convert.FromBase64String(publicKey);
            PublicKey key = PublicKey.Import(_algorithm, keyBlob, _publicKeyFormat);

            return InternalValid(data, signature, key);
        }

        private bool InternalValid(string data, string signature, PublicKey key)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);

            return InternalValid(dataBytes, signatureBytes, key);
        }

        private bool InternalValid(byte[] data, byte[] signature, PublicKey key)
        {
            return _algorithm.Verify(key, data, signature);
        }
    }
}
