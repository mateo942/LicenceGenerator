using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Manager;
using Licence.Abstraction.Model;
using Licence.Abstraction.Repository;
using Licence.Abstraction.Service;
using Licence.Client;
using Licence.Client.Service;
using Licence.Core.Handlers;
using Licence.Core.Models;
using Licence.Core.Repository;
using Licence.Core.Service;

namespace LicenceGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ILicenceRepository licenceRepository = new LicenceInMemoryRepository();
            IDeviceInfoService deviceInfoService = new DeviceInfoService();
            ILicenceHandler licenceHandler = new LicenceHandler(deviceInfoService);
            ILicenceManagerService licenceMangerService = new InMemoryWithRequestLicenceManagerService(deviceInfoService);
            IEncryptionService encryptionService = new EncryptionService(EncryptionSettings.Default);

            var symetricKey = encryptionService.GenerateKey();
            encryptionService = new EncryptionService(
                new EncryptionSettings(null, symetricKey));

            for (int i = 0; i < 10; i++)
            {
                var message = "Ala ma kota";
                var d = encryptionService.Encrypt(message);
                var a = encryptionService.Decrypt(d);

                Console.WriteLine("Message: {0}, Encrypt: {1}, Decrypt: {2}, Is same: {3}", message, d, a, message == a);
            }

            

            var licenceService = new LicenceService(licenceRepository, licenceHandler);
            var licence = licenceService.Create(new Licence.Core.Models.LicenceData()
            {
                DeviceId = "47DEQpj8HBSa-_TImW-5JCeuQeRkm5NMpJWZG3hSuFUa",
                CreatedAtUtc = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(14),
                ProjectName = "B2B",
                Modules = new string[]
                {
                    "Core",
                    "Products",
                    "Contractor",
                    "Discount"
                },
                AditionalInfos = new Dictionary<string, string>
                {
                    { "maxApiUsers", "3" },
                    { "maxUsers", "20" }
                }
            });

            await licenceMangerService.SavePublicKey(licence.PublicKey);
            await licenceMangerService.SaveLicence(licence.Licence);

            ILicenceManager<Feature> manager = new LicenceManager<Feature>(licenceHandler, licenceMangerService);
            manager.SetConfiguration(cfg =>
            {
                cfg.ApplicationName = "B2B";
                cfg.Version = "1.0.0.1";
                cfg.Modules = new string[] { "Core", "Discount", "Products", "Contractor" };
                cfg.AdditionalInfos = new string[] { "maxApiUsers", "maxUsers" };
            });

            manager.AddParser("maxApiUsers", (value, data) =>
            {
                data.MaxApiUsers = Convert.ToInt32(value);
            });
            manager.AddParser("maxUsers", (value, data) =>
            {
                data.MaxUsers = Convert.ToInt32(value);
            });
            var result = await manager.Valid();
            Console.WriteLine("Licence is valid: {0}", result);

            if(manager is ILicenceManagerTimer licenceManagerTimer)
            {
                licenceManagerTimer.SetCheckLicenceInterval(TimeSpan.FromMinutes(15));
            }

            Console.ReadKey(true);

            
        }
    }
}
