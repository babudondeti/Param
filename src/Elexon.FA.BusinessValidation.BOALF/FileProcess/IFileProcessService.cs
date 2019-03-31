namespace Elexon.FA.BusinessValidation.BOALFlow.FileProcess
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFileProcessService" />
    /// </summary>
    public interface IFileProcessService
    {
        /// <summary>
        /// The FileProcess
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task FileProcess(Aggregate<Boalf> aggregate);
    }
}
