using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Licence.Abstraction.Service;
using Licence.Client.Model;

namespace Licence.Client.Service
{
    public class InMemoryWithRequestLicenceManagerService : InMemoryLicenceManagerService
    {
        private readonly IDeviceInfoService _deviceInfoService;

        public InMemoryWithRequestLicenceManagerService(IDeviceInfoService deviceInfoService)
        {
            _deviceInfoService = deviceInfoService;
        }

        public override Task RequestLicence(IEnumerable<string> modules, IEnumerable<string> aditionalInfos)
        {
            var licenceRequest = new LicenceRequest();
            licenceRequest.DeviceId = _deviceInfoService.GetId();
            licenceRequest.Modules = modules;
            licenceRequest.AditionalInfos = aditionalInfos;

            var json = System.Text.Json.JsonSerializer.Serialize(licenceRequest);
            Console.WriteLine("Send request licence: {0}", json);

            return Task.CompletedTask;
        }
    }
}

