using Elexon.FA.BusinessValidation.Domain.Seed;
using Elexon.FA.Core.Blob;
using Elexon.FA.Core.Blob.Abstractions;
using Elexon.FA.Core.KeyVault.Abstractions;
using Elexon.FA.Core.Logging;
using Elexon.FA.Core.MessageHeader;
using Elexon.FA.Core.MessageHeader.Abstractions;
using Elexon.FA.Core.ServiceBus;
using Elexon.FA.Core.ServiceBus.Abstractions;
using Elexon.FA.Core.ServiceBus.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Api.Configurations
{
    public static class DependencyConfigurationExtension
    {

        public static void RegisterBlob(this IServiceCollection services, ICoreKeyVaultClient keyVaultClient)
        {
            string storageAccount = Task.Run(async () => await keyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_STORAGEACCOUNT)).Result;
            string storageKey = Task.Run(async () => await keyVaultClient?.GetSecret(BusinessValidationConstants.KEYVAULT_STORAGEKEY)).Result;
            string containerName = Task.Run(async () => await keyVaultClient?.GetSecret(BusinessValidationConstants.KEYVAULT_CONTAINERNAME)).Result;
            services.AddScoped<ICoreBlobStorage, CoreBlobStorage>(factory =>
            {
                return new CoreBlobStorage(new CoreBlobSettings(storageAccount, storageKey, containerName));
            });
        }

        public static void RegisterServiceBus(this IServiceCollection services, ICoreKeyVaultClient keyVaultClient)
        {
            services.AddSingleton<ICoreMessageHeader, CoreMessageHeader>();
            string serviceBusConnectionString = Task.Run(async () => await keyVaultClient?.GetSecret(BusinessValidationConstants.KEYVAULT_SERVICEBUSCONNECTIONSTRING)).Result;
            string receiveTopic = Task.Run(async () => await keyVaultClient?.GetSecret(BusinessValidationConstants.KEYVAULT_RECEIVETOPIC)).Result;
            string subscriptionName = Task.Run(async () => await keyVaultClient?.GetSecret(BusinessValidationConstants.KEYVAULT_SUBSCRIPTIONNAME)).Result;
            services.AddSingleton<ISendManager, SendManager>(_ => new SendManager(new CoreTopicSettings(serviceBusConnectionString)));
            services.AddSingleton<IReceiveManager, ReceiveManager>(_ =>
            {
                return new ReceiveManager(new CoreSubscriptionSettings(serviceBusConnectionString, receiveTopic, subscriptionName));
            });
        }

        public static void RegisterLogs(this IServiceCollection services, ICoreKeyVaultClient keyVaultClient)
        {
            Log.Init(Task.Run(async () => await keyVaultClient?.GetSecret(BusinessValidationConstants.KEYVAULT_APPLICATIONINSIGHTSKEY)).Result, BusinessValidationConstants.LOGGINGLEVEL,
                BusinessValidationConstants.BUSINESSVALIDATION, BusinessValidationConstants.BUSINESSVALIDATION);
        }
    }
}
