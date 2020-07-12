using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Licence.Abstraction.Manager;
using Licence.Abstraction.Repository;
using Licence.Abstraction.Service;
using Licence.Client;
using Licence.Client.Service;
using Licence.Core.Handlers;
using Licence.Core.Repository;
using Licence.Core.Service;

namespace LicenceGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var licenceService = new LicenceService(new LicenceInMemoryRepository(), new LicenceHandler());
            var licence = licenceService.Create(new Licence.Core.Models.LicenceData()
            {
                DeviceId = "47DEQpj8HBSa-_TImW-5JCeuQeRkm5NMpJWZG3hSuFU",
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

            ILicenceManagerService licenceMangerService = new InMemoryLicenceManagerService();
            await licenceMangerService.SavePublicKey(licence.PublicKey);
            await licenceMangerService.SaveLicence(licence.Licence);

            ILicenceManager manager = new LicenceManager<Feature>( new LicenceHandler(), licenceMangerService);
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

            Console.ReadKey(true);
        }
    }
}
