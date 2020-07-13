using System;
using DeviceId;
using Licence.Abstraction.Service;

namespace Licence.Core.Service
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public string GetId()
        {
            string deviceId = new DeviceIdBuilder()
                    .AddSystemDriveSerialNumber()
                    .ToString();

            return deviceId;
        }
    }
}
