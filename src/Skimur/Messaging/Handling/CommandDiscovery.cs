using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Skimur.Messaging.Handling
{
    public class CommandDiscovery : ICommandDiscovery
    {
        private readonly IServiceCollection _services;

        public CommandDiscovery(IServiceCollection services)
        {
            _services = services;
        }

        public void Register(ICommandRegistrar registrar)
        {
            foreach (var service in _services.Select(x => x.ServiceType))
            {
                if (typeof(ICommandHandler).IsAssignableFrom(service))
                {
                    var genericArguments = service.GetGenericArguments();
                    if (genericArguments.Count() == 1)
                    {
                        RegisterCommandHandler(genericArguments[0], registrar);
                    }
                    else
                    {
                        RegisterCommandResponseHandler(genericArguments[0], genericArguments[1], registrar);
                    }
                }
            }
        }

        private void RegisterCommandHandler(Type type, ICommandRegistrar registrar)
        {
            var methodInfo = typeof(ICommandRegistrar)
                .GetMethod("RegisterCommand", BindingFlags.Public | BindingFlags.Instance)
                .MakeGenericMethod(type);

            methodInfo.Invoke(registrar, new object[0]);
        }

        private void RegisterCommandResponseHandler(Type request, Type response, ICommandRegistrar registrar)
        {
            var methodInfo = typeof(ICommandRegistrar)
                .GetMethod("RegisterCommandResponse", BindingFlags.Public | BindingFlags.Instance)
                .MakeGenericMethod(request, response);

            methodInfo.Invoke(registrar, new object[0]);
        }
    }
}
