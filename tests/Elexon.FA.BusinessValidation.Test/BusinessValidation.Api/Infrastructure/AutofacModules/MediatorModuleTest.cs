using Autofac.Core;
using Autofac.Core.Registration;
using Elexon.FA.BusinessValidation.Api.Infrastructure.AutofacModules;
using Elexon.FA.BusinessValidation.BOALFFlow.Command;
using Elexon.FA.BusinessValidation.BODFlow.Command;
using Elexon.FA.BusinessValidation.DISBSADFlow.Command;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.FPNFlow.Command;
using Elexon.FA.BusinessValidation.NETBSADFlow.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Infrastructure.AutofacModules
{
    public  class MediatorModuleTest 
    {

        [Fact]
        public void Should_Have_Register_Types()
        {
            //Arrange
            var typesToCheck = new List<Type>
        {
            typeof(IMediator),
            typeof(IRequestHandler<ValidatedBodCommand,BusinessValidationProxy>),
            typeof(ValidatedBodCommand),
            typeof(IRequestHandler<ValidatedBoalfCommand,BusinessValidationProxy>),
            typeof(ValidatedBoalfCommand),
            typeof(IRequestHandler<ValidatedNetbsadCommand,BusinessValidationProxy>),
            typeof(ValidatedNetbsadCommand),
            typeof(IRequestHandler<ValidatedDisbsadCommand,BusinessValidationProxy>),
            typeof(ValidatedDisbsadCommand),
            typeof(IRequestHandler<ValidatedFpnCommand,BusinessValidationProxy>),
            typeof(ValidatedFpnCommand)

        };

            //Act
            var typesRegistered = this.GetTypesRegisteredInModule(new MediatorModule());

            //Arrange
            Assert.Equal(typesToCheck.Count, typesRegistered.Count());     

        }

        private IEnumerable<Type> GetTypesRegisteredInModule(MediatorModule module)
        {
            IComponentRegistry componentRegistry = new ComponentRegistry();

            module.Configure(componentRegistry);

            var typesRegistered =
                componentRegistry.Registrations.SelectMany(x => x.Services)
                    .Cast<TypedService>()
                    .Select(x => x.ServiceType);

            return typesRegistered;
        }
    }
}