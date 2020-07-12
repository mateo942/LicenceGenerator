using System;
using System.Linq;
using System.Threading.Tasks;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Model;
using Licence.Abstraction.Service;

namespace Licence.Client
{
    public class LicenceManager
    {
        private readonly ILicenceHandler _licenceHandler;
        private readonly ILicenceManagerService _licenceManagerService;

        private string publicKey;
        private string data;

        private ILicenceData licenceData;
        private bool licenceIsValid;


        internal LicenceManager(ILicenceHandler licenceHandler, ILicenceManagerService licenceManagerService)
        {
            _licenceHandler = licenceHandler;
            _licenceManagerService = licenceManagerService;

            _licenceManagerService.NotifiChanged += _licenceManagerService_NotifiChanged;
        }

        private async void _licenceManagerService_NotifiChanged(object sender, EventArgs e)
        {
            await Valid();
        }

        protected virtual Task SuccessedValidation()
        {
            return Task.CompletedTask;
        }

        public async Task<bool> Valid()
        {
            publicKey = await _licenceManagerService.GetPublicKey();
            data = await _licenceManagerService.GetLicence();

            if(_licenceHandler.Valid(publicKey, data, out var lic))
            {
                licenceData = lic;
                licenceIsValid = true;
                await SuccessedValidation();
                return true;
            }

            licenceData = null;
            licenceIsValid = false;
            return false;
        }

        public bool IsActiveModule(string moduleName)
        {
            if (licenceIsValid == false)
                return false;

            if (licenceData is null)
                throw new InvalidOperationException();

            return licenceData.Modules.Contains(moduleName);
        }

        public string GetInfo(string name)
        {
            if (licenceIsValid == false || licenceData is null)
                throw new InvalidOperationException();

            if(licenceData.AditionalInfos.ContainsKey(name))
                return licenceData.AditionalInfos[name];

            return null;
        }
    }
}
