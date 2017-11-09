using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.blue.Api.blues;
using Lykke.blue.Api.Core.Identity;
using Lykke.blue.Api.Core.Services;
using Lykke.blue.Api.Core.Settings.ServiceSettings;
using Lykke.blue.Api.Credentials;
using Lykke.blue.Api.Infrastructure;
using Lykke.blue.Api.Services.Identity;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ClientAccount.Client.AutorestClient;
using Lykke.Service.Pledges.Client;
using Lykke.Service.Registration;
using Lykke.Service.Session;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;
using Lykke.blue.Service.InspireStream.Client;

namespace Lykke.blue.Api.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<ApiSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<ApiSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterLocalTypes(builder);
            RegisterLocalServices(builder);
            RegisterExternalServices(builder);

            _services.AddSingleton<ClientAccountLogic>();

            builder.RegisterType<RequestContext>().As<IRequestContext>().InstancePerLifetimeScope();
            builder.RegisterType<LykkePrincipal>().As<ILykkePrincipal>().InstancePerLifetimeScope();

            builder.Populate(_services);
        }

        private void RegisterExternalServices(ContainerBuilder builder)
        {
            builder.RegisterType<ClientAccountService>()
                .As<IClientAccountService>()
                .WithParameter("baseUri", new Uri(_settings.CurrentValue.Services.ClientAccountServiceUrl));

            builder.RegisterType<ClientAccountClient>()
                .As<IClientAccountClient>()
                .WithParameter("serviceUrl", _settings.CurrentValue.Services.ClientAccountServiceUrl)
                .WithParameter("log", _log)
                .SingleInstance();

            builder.RegisterType<LykkeRegistrationClient>()
                .As<ILykkeRegistrationClient>()
                .WithParameter("serviceUrl", _settings.CurrentValue.Services.RegistrationServiceUrl)
                .WithParameter("log", _log)
                .SingleInstance();

            builder.RegisterType<ClientSessionsClient>()
                .As<IClientSessionsClient>()
                .WithParameter("serviceUrl", _settings.CurrentValue.Services.SessionServiceUrl);

            builder.RegisterType<PledgesClient>()
                .As<IPledgesClient>()
                .WithParameter("serviceUrl", _settings.CurrentValue.Services.PledgesServiceUrl)
                .WithParameter("log", _log)
                .SingleInstance();

            builder.RegisterType<InspireStreamClient>()
                .As<IInspireStreamClient>()
                .WithParameter("serviceUrl", _settings.CurrentValue.Services.InspireStreamService.ServiceUrl)
                .WithParameter("log", _log)
                .WithParameter("timeout", _settings.CurrentValue.Services.InspireStreamService.RequestTimeout)
                .SingleInstance();
        }

        private static void RegisterLocalServices(ContainerBuilder builder)
        {
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();
        }

        private void RegisterLocalTypes(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log).As<ILog>().SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue).SingleInstance();
        }
    }
}
