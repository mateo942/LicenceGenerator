using System;
using System.Threading.Tasks;

namespace Licence.Abstraction.Manager
{
    public interface ILicenceManager
    {
        string GetInfo(string name);
        bool IsActiveModule(string moduleName);
        Task<bool> Valid();

        ILicenceManagerConfiguration Configuration { get; }
        void SetConfiguration(Action<ILicenceManagerConfiguration> cfg);
    }

    public interface ILicenceManager<TData> : ILicenceManager
    {
        TData Data { get; }
        ILicenceManager<TData> AddParser(string key, Action<string, TData> action);
    }
}
