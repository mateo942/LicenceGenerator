using System;
using System.Text;
using DeviceId;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Model;
using NSec.Cryptography;

namespace Licence.Core.Handlers
{
    public class LicenceHandler : ILicenceHandler
    {
        const string SIGNATURE = "---Signature---";
        const string LICENCE = "---Licence---";
        const string END = "---End---";

        private readonly SignatureAlgorithm _algorithm;
        private readonly KeyBlobFormat _privateKeyFormat = KeyBlobFormat.PkixPrivateKeyText;
        private readonly KeyBlobFormat _publicKeyFormat = KeyBlobFormat.PkixPublicKeyText;

        public LicenceHandler()
        {
            _algorithm = SignatureAlgorithm.Ed25519;
        }

        public ILicenceResult Export(ILicenceData licenceData)
        {
            if (licenceData is null)
            {
                throw new ArgumentNullException(nameof(licenceData));
            }

            var licence = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(licenceData);

            var param = new KeyCreationParameters();
            param.ExportPolicy = KeyExportPolicies.AllowPlaintextExport;
            var key = Key.Create(_algorithm, param);

            var signature = _algorithm.Sign(key, licence);

            var result = new Models.LicenceResult();

            result.PrivateKey = Convert.ToBase64String(key.Export(_privateKeyFormat), Base64FormattingOptions.InsertLineBreaks);
            result.PublicKey = Convert.ToBase64String(key.Export(_publicKeyFormat), Base64FormattingOptions.InsertLineBreaks);


            StringBuilder licenceResult = new StringBuilder();
            licenceResult.AppendLine(SIGNATURE);
            licenceResult.AppendLine(Convert.ToBase64String(signature, Base64FormattingOptions.InsertLineBreaks));
            licenceResult.AppendLine(LICENCE);
            licenceResult.AppendLine(Convert.ToBase64String(licence, Base64FormattingOptions.InsertLineBreaks));
            licenceResult.AppendLine(END);

            result.Licence = licenceResult.ToString();

            return result;
        }

        public bool Valid(string deviceId, string publicKey, string data, out ILicenceData licenceData)
        {
            if (publicKey is null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("licence data is null or empty", nameof(data));
            }

            var readData = ReadData(data);

            PublicKey key = PublicKey.Import(_algorithm, Convert.FromBase64String(publicKey), _publicKeyFormat);

            if (_algorithm.Verify(key, readData.Item2, readData.Item1))
            {
                licenceData = System.Text.Json.JsonSerializer.Deserialize<Models.LicenceData>(readData.Item2);

                if (licenceData.ExpiredAt >= DateTime.UtcNow && licenceData.DeviceId == deviceId)
                    return true;
            }

            licenceData = null;
            return false;
        }

        public bool Valid(string publicKey, string data, out ILicenceData licenceData)
        {
            string deviceId = new DeviceIdBuilder()
                    .AddSystemDriveSerialNumber()
                    .ToString();

            return Valid(deviceId, publicKey, data, out licenceData);
        }

        private Tuple<byte[], byte[]> ReadData(string data)
        {
            string[] lines = data.Split("\n");

            StringBuilder signature = new StringBuilder();
            StringBuilder licence = new StringBuilder();

            string currentSegment = string.Empty;
            bool canRead = true;
            int index = 0;
            while (index < lines.Length && canRead)
            {
                string value = lines[index];

                if (!TryGetCurrentSegment(value, ref currentSegment))
                {
                    switch (currentSegment)
                    {
                        case SIGNATURE:
                            signature.Append(value);
                            break;
                        case LICENCE:
                            licence.Append(value);
                            break;
                        case END:
                            canRead = false;
                            break;
                    }
                }

                index++;
            }


            return new Tuple<byte[], byte[]>(
                Convert.FromBase64String(signature.ToString()),
                Convert.FromBase64String(licence.ToString())
            );
        }

        private bool TryGetCurrentSegment(string value, ref string currentSegment)
        {
            switch (value)
            {
                case SIGNATURE:
                    currentSegment = SIGNATURE;
                    return true;
                case LICENCE:
                    currentSegment = LICENCE;
                    return true;
                case END:
                    currentSegment = END;
                    return true;
            }

            return false;
        }
    }
}
