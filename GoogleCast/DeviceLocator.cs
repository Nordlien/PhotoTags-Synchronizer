using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace GoogleCast
{
    /// <summary>
    /// Device locator
    /// </summary>
    public class DeviceLocator : IDeviceLocator
    {
        private const string PROTOCOL = "_googlecast._tcp.local.";

        private Receiver CreateReceiver(IZeroconfHost host)
        {
            //var service2 = host.Services.Keys[PROTOCOL];
            //Dictionary<string, IService> service2 = host.Services.Values.First();
            //string properties = service.Properties.First();
            //var service2 = host.Services.Keys[PROTOCOL];
            var serviceFound = host.Services.Values.First();

            //IService serviceFound = null;
            //foreach (IService service in host.Services.Values)
            //{
            //    if (service.Name == PROTOCOL) serviceFound = service;
            //}
            //if (serviceFound == null) return null;

            var properties = serviceFound.Properties.First();


            return new Receiver()
            {
                Id = properties["id"],
                FriendlyName = properties["fn"],
                IPEndPoint = new IPEndPoint(IPAddress.Parse(host.IPAddress), serviceFound.Port)
            };
        }

        /// <summary>
        /// Finds the available receivers
        /// </summary>
        /// <returns>a collection of receivers</returns>
        public async Task<IEnumerable<IReceiver>> FindReceiversAsync()
        {
            return (await ZeroconfResolver.ResolveAsync(PROTOCOL, new TimeSpan(0, 0, 0, 0, 4000), 4, 4000)).Select(CreateReceiver);
        }

        /// <summary>
        /// Finds the available receivers in continuous way
        /// </summary>
        /// <returns>a provider for notifications</returns>
        public IObservable<IReceiver> FindReceiversContinuous()
        {
            return ZeroconfResolver.ResolveContinuous(PROTOCOL, new TimeSpan(0, 0, 0, 0, 4000), 4, 4000).Select(CreateReceiver);
        }
    }
}
