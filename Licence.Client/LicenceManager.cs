using System;
using System.Linq;
using System.Threading.Tasks;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Manager;
using Licence.Abstraction.Model;
using Licence.Abstraction.Service;
using Licence.Client.Model;

namespace Licence.Client
{
    public class LicenceManager : ILicenceManager
    {
        private readonly ILicenceHandler _licenceHandler;
        private readonly ILicenceManagerService _licenceManagerService;

        private string publicKey;
        private string data;

        private ILicenceData licenceData;
        private bool licenceIsValid;

        public ILicenceManagerConfiguration Configuration { get; private set; }

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

            try
            {
                await SendRequestCore();
            }
            catch (NotImplementedException) { }
            

            licenceData = null;
            licenceIsValid = false;
            return false;
        }

        protected async virtual Task SendRequestCore()
        {
            await _licenceManagerService.RequestLicence(
                Configuration.Modules ?? Enumerable.Empty<string>(),
                Configuration.AdditionalInfos ?? Enumerable.Empty<string>());
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

        public void SetConfiguration(Action<ILicenceManagerConfiguration> cfg)
        {
            var configuration = new LicenceManagerConfiguration();

            cfg.Invoke(configuration);

            Configuration = configuration;
        }
    }
}
