﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Licence.Abstraction.Handler;
using Licence.Abstraction.Manager;
using Licence.Abstraction.Service;

namespace Licence.Client
{
    public class LicenceManager<TData> : LicenceManager, ILicenceManager<TData> where TData : new()
    {
        public TData Data { get; private set; }

        private IList<KeyValuePair<string, Action<string, TData>>> _parsers;

        public LicenceManager(ILicenceHandler licenceHandler, ILicenceManagerService licenceManagerService, IDeviceInfoService deviceInfoService)
            : base(licenceHandler, licenceManagerService, deviceInfoService)
        {
            _parsers = new List<KeyValuePair<string, Action<string, TData>>>();
        }

        public ILicenceManager<TData> AddParser(string key, Action<string, TData> action)
        {
            _parsers.Add(new KeyValuePair<string, Action<string, TData>>(key, action));

            return this;
        }

        protected override Task SuccessedValidation()
        {
            base.SuccessedValidation();

            Parse();

            return Task.CompletedTask;
        }

        private void Parse()
        {
            Data = Activator.CreateInstance<TData>();

            foreach (var item in _parsers)
            {
                try
                {
                    var value = this.GetInfo(item.Key);

                    if (value != null)
                    {
                        item.Value.Invoke(value, Data);
                    }
                } catch(Exception ex)
                {
                    throw new InvalidOperationException("licence parse", ex);
                }
            }
        }
    }
}
