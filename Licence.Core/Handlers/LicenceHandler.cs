using System;
using System.Text;
using System.Threading.Tasks;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Model;
using Licence.Abstraction.Repository;
using Licence.Abstraction.Service;
using NSec.Cryptography;

namespace Licence.Core.Handlers
{
    public class LicenceHandler : ILicenceHandler
    {
        const string SIGNATURE = "---Signature---";
        const string LICENCE = "---Licence---";
        const string END = "---End---";

        private readonly ISignatureService _signatureService;
        private readonly IKeyRepository _keyRepository;

        public LicenceHandler(ISignatureService signatureService, IKeyRepository keyRepository)
        {
            _signatureService = signatureService;
            _keyRepository = keyRepository;
        }

        public async Task<string> Export(ILicenceData licenceData, Guid keyId)
        {
            if (licenceData is null)
            {
                throw new ArgumentNullException(nameof(licenceData));
            }

            if(keyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(keyId));
            }

            var key = await _keyRepository.Get(keyId);

            var licence = System.Text.Json.JsonSerializer.Serialize(licenceData);

            var signature = _signatureService.Export(licence, key.PrivateKey);

            StringBuilder licenceResult = new StringBuilder();
            licenceResult.AppendLine(SIGNATURE);
            licenceResult.AppendLine(signature);
            licenceResult.AppendLine(LICENCE);
            licenceResult.AppendLine(licence);
            licenceResult.AppendLine(END);

            return licenceResult.ToString();
        }


        public async Task<ILicenceData> Valid(string deviceId, string data, Guid keyId)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("licence data is null or empty", nameof(data));
            }

            if (keyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(keyId));
            }

            var key = await _keyRepository.Get(keyId);

            return Valid(deviceId, data, key.PublicKey);
        }

        public ILicenceData Valid(string deviceId, string data, string publicKey)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("licence data is null or empty", nameof(data));
            }

            if (string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentException("public key is null or empty", nameof(publicKey));
            }

            var readData = ReadData(data);

            if (_signatureService.Valid(readData.Item2, readData.Item1, publicKey))
            {
                ILicenceData licenceData = System.Text.Json.JsonSerializer.Deserialize<Models.LicenceData>(readData.Item2);

                if (licenceData.ExpiredAt >= DateTime.UtcNow && licenceData.DeviceId == deviceId)
                    return licenceData;
            }

            return null;
        }

        private Tuple<string, string> ReadData(string data)
        {
            string[] lines  = data.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                );

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


            return new Tuple<string, string>(signature.ToString(), licence.ToString());
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
