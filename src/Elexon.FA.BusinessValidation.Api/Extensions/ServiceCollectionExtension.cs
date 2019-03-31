using Elexon.FA.BusinessValidation.Api.Strategy;
using Elexon.FA.BusinessValidation.BOALFlow.FileProcess;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.Core.HttpClient;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Elexon.FA.BusinessValidation.Api.Configurations
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterDependencies(this IServiceCollection services, ICoreKeyVaultClient keyVaultClient)
        {
            services.AddScoped<ICoreHttpServiceClient, CoreHttpServiceClient>();
            services.AddScoped<IApplicationBuilder, ApplicationBuilder>();
            services.AddScoped<ICoreKeyVaultClient>(factory =>
            {
                return keyVaultClient;
            });
            services.AddScoped<IQueryFlow<Bod>, QueryFlow<Bod>>();
            services.AddScoped<IQueryFlow<Boalf>, QueryFlow<Boalf>>();
            services.AddScoped<IQueryFlow<Netbsad>, QueryFlow<Netbsad>>();
            services.AddScoped<IQueryFlow<Disbsad>, QueryFlow<Disbsad>>();
            services.AddScoped<IQueryFlow<Fpn>, QueryFlow<Fpn>>();
            services.AddScoped<IProjectionWriter, ProjectionWriter>();
            services.AddScoped<IBusinessValidationStrategy, BodStrategy>();
            services.AddScoped<IBusinessValidationStrategy, NetbsadStrategy>();
            services.AddScoped<IBusinessValidationStrategy, DisbsadStrategy>();
            services.AddScoped<IBusinessValidationStrategy, FpnStrategy>();
            services.AddScoped<IGetStrategy, GetStrategy>();
            services.AddScoped<IFileProcessService, FileProcessService>();
        }        
    }
}
