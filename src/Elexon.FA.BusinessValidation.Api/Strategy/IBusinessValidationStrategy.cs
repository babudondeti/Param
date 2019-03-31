namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.IntegrationMessage;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IBusinessValidationStrategy" />
    /// </summary>
    public interface IBusinessValidationStrategy
    {
        /// <summary>
        /// The ExecuteStrategy
        /// </summary>
        /// <param name="FlowName">The FlowName<see cref="string"/></param>
        /// <param name="item">The item<see cref="Item"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        Task<BusinessValidationProxy> ExecuteStrategy(string FlowName, Item item);
    }
}
