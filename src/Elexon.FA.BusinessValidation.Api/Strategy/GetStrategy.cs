namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using MediatR;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="GetStrategy" />
    /// </summary>
    public class GetStrategy : IGetStrategy
    {
        /// <summary>
        /// The GetListOfStrategy
        /// </summary>
        /// <param name="_mediator">The _mediator<see cref="IMediator"/></param>
        /// <returns>The <see cref="Dictionary{TKey,TValue}"/></returns>
        public Dictionary<string, Func<IBusinessValidationStrategy>> GetListOfStrategy(IMediator _mediator)
        {
            Dictionary<string, Func<IBusinessValidationStrategy>> strategies = new Dictionary<string, Func<IBusinessValidationStrategy>> {
                                    { BusinessValidationConstants.FLOWS_BOD.ToUpperInvariant(), () =>new BodStrategy(_mediator) },
                                    { BusinessValidationConstants.FLOWS_BOALF.ToUpperInvariant(), () => new BoalfStrategy(_mediator)},
                                    { BusinessValidationConstants.FLOWS_NETBSAD.ToUpperInvariant(), () => new NetbsadStrategy(_mediator)},
                                    { BusinessValidationConstants.FLOWS_DISBSAD.ToUpperInvariant(), () => new DisbsadStrategy(_mediator)},
                                    { BusinessValidationConstants.FLOWS_FPN.ToUpperInvariant(), () => new FpnStrategy(_mediator)}
                };

            return strategies;
        }
    }
}
