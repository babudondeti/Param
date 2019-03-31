namespace Elexon.FA.BusinessValidation.Api.Infrastructure.AutofacModules
{
    using Autofac;
    using Elexon.FA.BusinessValidation.BOALFFlow.Command;
    using Elexon.FA.BusinessValidation.BODFlow.Command;
    using Elexon.FA.BusinessValidation.DISBSADFlow.Command;
    using Elexon.FA.BusinessValidation.FPNFlow.Command;
    using Elexon.FA.BusinessValidation.NETBSADFlow.Command;
    using MediatR;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="MediatorModule" />
    /// </summary>
    public class MediatorModule : Autofac.Module
    {
        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="builder">The builder<see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ValidatedBodCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(ValidatedBoalfCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(ValidatedNetbsadCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(ValidatedDisbsadCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(ValidatedFpnCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
        }
    }
}
