namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using MediatR;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="IGetStrategy" />
    /// </summary>
    public interface IGetStrategy
    {
        /// <summary>
        /// The GetListOfStrategy
        /// </summary>
        /// <param name="_mediator">The _mediator<see cref="IMediator"/></param>
        /// <returns>The <see cref="Dictionary{TKey,TValue}"/></returns>
        Dictionary<string, Func<IBusinessValidationStrategy>> GetListOfStrategy(IMediator _mediator);
    }
}
