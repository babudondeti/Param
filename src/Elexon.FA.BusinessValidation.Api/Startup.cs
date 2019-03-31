namespace Elexon.FA.BusinessValidation.API
{
    using Api.Infrastructure.AutofacModules;
    using Api.MessageHandlers;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.KeyVault;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Elexon.FA.Core.ServiceBus.Abstractions;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Elexon.FA.BusinessValidation.Api.Configurations;

    /// <summary>
    /// Defines the <see cref="Startup" />
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Gets or sets the Configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Defines the keyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient keyVaultClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        /// <param name="env">The env<see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment"/></param>
        public Startup()
        {
            AddEnvironmentVariables();
            keyVaultClient = new CoreKeyVaultClient(new CoreKeyVaultSettings(Configuration[BusinessValidationConstants.KeyVaultUri],
            Configuration[BusinessValidationConstants.KeyVaultClientId], Configuration[BusinessValidationConstants.KeyVaultClientSecret]));
        }
        private void AddEnvironmentVariables()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceProvider"/></returns>
        [ExcludeFromCodeCoverage]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {          
            services.AddMediatR(typeof(Startup).Assembly);
            services.RegisterDependencies(keyVaultClient);
            services.RegisterBlob(keyVaultClient);
            services.RegisterServiceBus(keyVaultClient);
            services.RegisterLogs(keyVaultClient);
            services.AddMvc(); 
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            return new AutofacServiceProvider(container.Build());
        }

      
        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory">The loggerFactory<see cref="ILoggerFactory"/></param>
        public static void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
            MessageHandler messageHandler = new MessageHandler(app?.ApplicationServices.GetRequiredService<IReceiveManager>(),
                app?.ApplicationServices.GetRequiredService<IMediator>());
            messageHandler.RegisterSubscription();
        }

    }
}
