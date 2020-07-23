using System;
using System.Security.Cryptography;
using System.Text;
using Licence.Abstraction.Model;
using Licence.Abstraction.Service;
using NSec.Cryptography;

namespace Licence.Core.Service
{
    public class EncryptionService : IEncryptionService, IDisposable
    {
        private readonly Aes256Gcm _aes256Gcm;
        private readonly Nonce _nonce;
        private readonly EncryptionSettings _encryptionSettings;

        private Key _key;
        private byte[] _authorizeBytes = new byte[0];

        public EncryptionService(EncryptionSettings encryptionSettings)
        {
            _encryptionSettings = encryptionSettings;
            _aes256Gcm = new Aes256Gcm();
            _nonce = new Nonce(4, 8);

            var now = DateTime.UtcNow.Date;
            var value = unchecked((int)now.Ticks);
            Nonce.TryAdd(ref _nonce, value);

            if (!string.IsNullOrEmpty(_encryptionSettings.PrivateKey))
            {
                byte[] blob = Convert.FromBase64String(_encryptionSettings.PrivateKey);
                _key = Key.Import(_aes256Gcm, blob, KeyBlobFormat.NSecSymmetricKey);
            }

            if (!string.IsNullOrEmpty(_encryptionSettings.PublicKey))
            {
                _authorizeBytes = Encoding.UTF8.GetBytes(_encryptionSettings.PublicKey);
            }
        }

        public string Decrypt(string data)
        {
            if (_key == null)
                throw new InvalidOperationException("Key has been not initialized");

            byte[] dataByte = Convert.FromBase64String(data);
            _aes256Gcm.Decrypt(_key, _nonce, _authorizeBytes, dataByte, out byte[] output);

            return Encoding.UTF8.GetString(output);
        }

        public void Dispose()
        {
            if(_key != null)
            {
                _key.Dispose();
                _key = null;
            }
        }

        public string Encrypt(string data)
        {
            if (_key == null)
                throw new InvalidOperationException("Key has been not initialized");

            byte[] dataByte = Encoding.UTF8.GetBytes(data);
            byte[] output = _aes256Gcm.Encrypt(_key, _nonce, _authorizeBytes, dataByte);

            return Convert.ToBase64String(output);
        }

        public string GenerateKey()
        {
            using (var key = Key.Create(_aes256Gcm, new KeyCreationParameters() { ExportPolicy = KeyExportPolicies.AllowPlaintextExport }))
            {
                var blob = key.Export(KeyBlobFormat.NSecSymmetricKey);
                return Convert.ToBase64String(blob);
            }
        }
    }
}
