using System;
using System.Threading.Tasks;
using Licence.Abstraction.Service;

namespace Licence.Client.Service
{
    public class InMemoryLicenceManagerService : ILicenceManagerService
    {
        private string _licence;
        private string _publicKey;

        public event EventHandler NotifiChanged;

        public Task<string> GetLicence()
        {
            return Task.FromResult(_licence);
        }

        public Task<string> GetPublicKey()
        {
            return Task.FromResult(_publicKey);
        }

        public Task SaveLicence(string licence)
        {
            _licence = licence;

            NotifiChanged?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public Task SavePublicKey(string publicKey)
        {
            _publicKey = publicKey;

            NotifiChanged?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }
    }
}
